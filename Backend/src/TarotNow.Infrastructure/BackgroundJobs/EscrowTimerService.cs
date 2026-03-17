using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Background hosted service cho 3 escrow timer jobs.
///
/// Chạy mỗi 60 giây, kiểm tra:
/// 1. Expired offers → auto-cancel (refund nếu đã freeze).
/// 2. No-reply 24h → auto-refund.
/// 3. Replied + no dispute 24h → auto-release (- 10% fee).
///
/// Dùng IServiceScopeFactory vì IHostedService là Singleton,
/// nhưng repositories/wallet là Scoped.
/// </summary>
public class EscrowTimerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EscrowTimerService> _logger;

    /// <summary>Khoảng thời gian giữa mỗi lần scan (60 giây).</summary>
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(60);

    public EscrowTimerService(IServiceScopeFactory scopeFactory, ILogger<EscrowTimerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[EscrowTimer] Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessTimers(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Unhandled error in timer loop.");
            }

            await Task.Delay(ScanInterval, stoppingToken);
        }

        _logger.LogInformation("[EscrowTimer] Service stopped.");
    }

    private async Task ProcessTimers(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var financeRepo = scope.ServiceProvider.GetRequiredService<IChatFinanceRepository>();
        var walletRepo = scope.ServiceProvider.GetRequiredService<IWalletRepository>();

        // 1. Expired offers → auto-cancel
        await ProcessExpiredOffers(financeRepo, ct);

        // 2. No-reply 24h → auto-refund
        await ProcessAutoRefunds(financeRepo, walletRepo, ct);

        // 3. Replied + no dispute 24h → auto-release
        await ProcessAutoReleases(financeRepo, walletRepo, ct);
    }

    /// <summary>
    /// Pending offers quá hạn → status = refunded (nếu đã freeze thì refund).
    /// Hiện tại accept ngay nên chưa tạo pending-only items.
    /// Nhưng giữ logic cho extensibility.
    /// </summary>
    private async Task ProcessExpiredOffers(IChatFinanceRepository repo, CancellationToken ct)
    {
        var expired = await repo.GetExpiredOffersAsync(ct);
        foreach (var item in expired)
        {
            try
            {
                item.Status = QuestionItemStatus.Refunded;
                item.RefundedAt = DateTime.UtcNow;
                await repo.UpdateItemAsync(item, ct);
                _logger.LogInformation("[EscrowTimer] Expired offer cancelled: {ItemId}", item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Failed to cancel offer: {ItemId}", item.Id);
            }
        }
        if (expired.Count > 0) await repo.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Accepted + no reply 24h → refund diamond cho user.
    /// </summary>
    private async Task ProcessAutoRefunds(IChatFinanceRepository repo, IWalletRepository wallet, CancellationToken ct)
    {
        var items = await repo.GetItemsForAutoRefundAsync(ct);
        foreach (var item in items)
        {
            try
            {
                // Refund qua stored procedure
                await wallet.RefundAsync(
                    item.PayerId, item.AmountDiamond,
                    referenceSource: "chat_question_item",
                    referenceId: item.Id.ToString(),
                    description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                    idempotencyKey: $"autorefund_{item.Id}",
                    cancellationToken: ct);

                item.Status = QuestionItemStatus.Refunded;
                item.RefundedAt = DateTime.UtcNow;
                item.DisputeWindowStart = DateTime.UtcNow;
                item.DisputeWindowEnd = DateTime.UtcNow.AddHours(24);
                await repo.UpdateItemAsync(item, ct);

                // Cập nhật session total_frozen
                var session = await repo.GetSessionByIdAsync(item.FinanceSessionId, ct);
                if (session != null)
                {
                    session.TotalFrozen -= item.AmountDiamond;
                    if (session.TotalFrozen < 0) session.TotalFrozen = 0;
                    await repo.UpdateSessionAsync(session, ct);
                }

                _logger.LogInformation("[EscrowTimer] Auto-refund: {ItemId}, {Amount}💎", item.Id, item.AmountDiamond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-refund failed: {ItemId}", item.Id);
            }
        }
        if (items.Count > 0) await repo.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Replied + no dispute 24h → auto-release diamond cho reader (- 10% fee).
    /// </summary>
    private async Task ProcessAutoReleases(IChatFinanceRepository repo, IWalletRepository wallet, CancellationToken ct)
    {
        var items = await repo.GetItemsForAutoReleaseAsync(ct);
        foreach (var item in items)
        {
            try
            {
                var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
                var readerAmount = item.AmountDiamond - fee;

                // Release cho reader
                await wallet.ReleaseAsync(
                    item.PayerId, item.ReceiverId, readerAmount,
                    referenceSource: "chat_question_item",
                    referenceId: item.Id.ToString(),
                    description: $"Auto-release {readerAmount}💎 (fee {fee}💎)",
                    idempotencyKey: $"autorelease_{item.Id}",
                    cancellationToken: ct);

                // Consume fee
                if (fee > 0)
                {
                    await wallet.ConsumeAsync(
                        item.PayerId, fee,
                        referenceSource: "platform_fee",
                        referenceId: item.Id.ToString(),
                        description: $"Platform fee auto 10% = {fee}💎",
                        idempotencyKey: $"autofee_{item.Id}",
                        cancellationToken: ct);
                }

                item.Status = QuestionItemStatus.Released;
                item.ReleasedAt = DateTime.UtcNow;
                item.DisputeWindowStart = DateTime.UtcNow;
                item.DisputeWindowEnd = DateTime.UtcNow.AddHours(24);
                await repo.UpdateItemAsync(item, ct);

                var session = await repo.GetSessionByIdAsync(item.FinanceSessionId, ct);
                if (session != null)
                {
                    session.TotalFrozen -= item.AmountDiamond;
                    if (session.TotalFrozen < 0) session.TotalFrozen = 0;
                    await repo.UpdateSessionAsync(session, ct);
                }

                _logger.LogInformation("[EscrowTimer] Auto-release: {ItemId}, {Amount}💎 (fee {Fee}💎)", item.Id, readerAmount, fee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-release failed: {ItemId}", item.Id);
            }
        }
        if (items.Count > 0) await repo.SaveChangesAsync(ct);
    }
}

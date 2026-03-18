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
        var transactionCoordinator = scope.ServiceProvider.GetRequiredService<ITransactionCoordinator>();

        // 1. Expired offers → auto-cancel
        await ProcessExpiredOffers(financeRepo, ct);

        // 2. No-reply 24h → auto-refund
        await ProcessAutoRefunds(financeRepo, walletRepo, transactionCoordinator, ct);

        // 3. Replied + no dispute 24h → auto-release
        await ProcessAutoReleases(financeRepo, walletRepo, transactionCoordinator, ct);
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
    private async Task ProcessAutoRefunds(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        CancellationToken ct)
    {
        var candidates = await repo.GetItemsForAutoRefundAsync(ct);
        foreach (var candidate in candidates)
        {
            try
            {
                await transactionCoordinator.ExecuteAsync(async transactionCt =>
                {
                    var item = await repo.GetItemForUpdateAsync(candidate.Id, transactionCt);
                    if (item == null) return;

                    var now = DateTime.UtcNow;
                    var eligible = item.Status == QuestionItemStatus.Accepted
                                   && item.RepliedAt == null
                                   && item.AutoRefundAt != null
                                   && item.AutoRefundAt <= now;
                    if (!eligible) return;

                    await wallet.RefundAsync(
                        item.PayerId, item.AmountDiamond,
                        referenceSource: "chat_question_item",
                        referenceId: item.Id.ToString(),
                        description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                        idempotencyKey: $"settle_refund_{item.Id}",
                        cancellationToken: transactionCt);

                    item.Status = QuestionItemStatus.Refunded;
                    item.RefundedAt = now;
                    item.DisputeWindowStart = now;
                    item.DisputeWindowEnd = now.AddHours(24);
                    await repo.UpdateItemAsync(item, transactionCt);

                    var session = await repo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
                    if (session != null)
                    {
                        session.TotalFrozen -= item.AmountDiamond;
                        if (session.TotalFrozen < 0) session.TotalFrozen = 0;
                        await repo.UpdateSessionAsync(session, transactionCt);
                    }

                    await repo.SaveChangesAsync(transactionCt);
                }, ct);

                _logger.LogInformation("[EscrowTimer] Auto-refund: {ItemId}, {Amount}💎", candidate.Id, candidate.AmountDiamond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-refund failed: {ItemId}", candidate.Id);
            }
        }
    }

    /// <summary>
    /// Replied + no dispute 24h → auto-release diamond cho reader (- 10% fee).
    /// </summary>
    private async Task ProcessAutoReleases(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        CancellationToken ct)
    {
        var candidates = await repo.GetItemsForAutoReleaseAsync(ct);
        foreach (var candidate in candidates)
        {
            try
            {
                await transactionCoordinator.ExecuteAsync(async transactionCt =>
                {
                    var item = await repo.GetItemForUpdateAsync(candidate.Id, transactionCt);
                    if (item == null) return;

                    var now = DateTime.UtcNow;
                    var eligible = item.Status == QuestionItemStatus.Accepted
                                   && item.RepliedAt != null
                                   && item.AutoReleaseAt != null
                                   && item.AutoReleaseAt <= now;
                    if (!eligible) return;

                    var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
                    var readerAmount = item.AmountDiamond - fee;

                    await wallet.ReleaseAsync(
                        item.PayerId, item.ReceiverId, readerAmount,
                        referenceSource: "chat_question_item",
                        referenceId: item.Id.ToString(),
                        description: $"Auto-release {readerAmount}💎 (fee {fee}💎)",
                        idempotencyKey: $"settle_release_{item.Id}",
                        cancellationToken: transactionCt);

                    if (fee > 0)
                    {
                        await wallet.ConsumeAsync(
                            item.PayerId, fee,
                            referenceSource: "platform_fee",
                            referenceId: item.Id.ToString(),
                            description: $"Platform fee auto 10% = {fee}💎",
                            idempotencyKey: $"settle_fee_{item.Id}",
                            cancellationToken: transactionCt);
                    }

                    item.Status = QuestionItemStatus.Released;
                    item.ReleasedAt = now;
                    item.DisputeWindowStart = now;
                    item.DisputeWindowEnd = now.AddHours(24);
                    await repo.UpdateItemAsync(item, transactionCt);

                    var session = await repo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
                    if (session != null)
                    {
                        session.TotalFrozen -= item.AmountDiamond;
                        if (session.TotalFrozen < 0) session.TotalFrozen = 0;
                        await repo.UpdateSessionAsync(session, transactionCt);
                    }

                    await repo.SaveChangesAsync(transactionCt);
                    _logger.LogInformation("[EscrowTimer] Auto-release: {ItemId}, {Amount}💎 (fee {Fee}💎)", item.Id, readerAmount, fee);
                }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-release failed: {ItemId}", candidate.Id);
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EscrowTimerService> _logger;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo EscrowTimer background service.
    /// Luồng xử lý: nhận scope factory để resolve dependency scoped theo từng vòng quét và logger vận hành.
    /// </summary>
    public EscrowTimerService(
        IServiceScopeFactory scopeFactory,
        ILogger<EscrowTimerService> logger,
        ISystemConfigSettings systemConfigSettings)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Vòng lặp chạy nền của escrow timer.
    /// Luồng xử lý: gọi ProcessTimers định kỳ theo ScanInterval, bắt lỗi cục bộ và xử lý shutdown graceful.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("[EscrowTimer] Service started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessTimers(stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Bỏ qua lỗi dispose khi host đang shutdown.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "[EscrowTimer] Unhandled error in timer loop.");
                    // Bắt lỗi để job tiếp tục vòng quét sau thay vì dừng hẳn.
                }

                await Task.Delay(ResolveScanInterval(), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[EscrowTimer] Service is shutting down gracefully.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "[EscrowTimer] Fatal error in service loop.");
        }

        _logger.LogDebug("[EscrowTimer] Service stopped.");
    }

    /// <summary>
    /// Thực thi toàn bộ pipeline timer escrow trong một vòng quét.
    /// Luồng xử lý: resolve dependencies scoped, dựng RefundDependencies, rồi chạy tuần tự các tác vụ timeout/refund/release.
    /// </summary>
    private async Task ProcessTimers(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var financeRepo = scope.ServiceProvider.GetRequiredService<IChatFinanceRepository>();
        var walletRepo = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
        var conversationRepo = scope.ServiceProvider.GetRequiredService<IConversationRepository>();
        var messageRepo = scope.ServiceProvider.GetRequiredService<IChatMessageRepository>();
        var escrowSettlementService = scope.ServiceProvider.GetRequiredService<IEscrowSettlementService>();
        var domainEventPublisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();
        var transactionCoordinator = scope.ServiceProvider.GetRequiredService<ITransactionCoordinator>();
        var refundDependencies = new RefundDependencies(
            financeRepo,
            walletRepo,
            conversationRepo,
            messageRepo,
            domainEventPublisher,
            transactionCoordinator);

        await ProcessExpiredOffers(refundDependencies, cancellationToken);
        await ProcessAutoRefunds(refundDependencies, cancellationToken);
        await ProcessAutoReleases(refundDependencies, escrowSettlementService, cancellationToken);
        await ProcessCompletionTimeouts(refundDependencies, escrowSettlementService, cancellationToken);
        await ProcessDisputeAutoResolutions(refundDependencies, escrowSettlementService, cancellationToken);
        await ProcessExpiredAddMoneyOffers(refundDependencies, cancellationToken);
        // Chạy tuần tự để giảm xung đột state giữa các nhánh cùng thao tác trên session/item.
    }

    // Gói dependency dùng chung cho các nhánh xử lý escrow để giảm số tham số truyền lặp.
    private readonly record struct RefundDependencies(
        IChatFinanceRepository FinanceRepository,
        IWalletRepository WalletRepository,
        IConversationRepository ConversationRepository,
        IChatMessageRepository MessageRepository,
        IDomainEventPublisher DomainEventPublisher,
        ITransactionCoordinator TransactionCoordinator);

    private TimeSpan ResolveScanInterval()
    {
        return TimeSpan.FromSeconds(
            Math.Clamp(_systemConfigSettings.OperationalEscrowTimerScanIntervalSeconds, 10, 3600));
    }
}

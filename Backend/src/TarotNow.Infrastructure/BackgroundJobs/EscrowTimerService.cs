using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService : BackgroundService
{
    private static readonly TimeSpan ScanInterval = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EscrowTimerService> _logger;

    public EscrowTimerService(IServiceScopeFactory scopeFactory, ILogger<EscrowTimerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

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
                    
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "[EscrowTimer] Unhandled error in timer loop.");
                }

                await Task.Delay(ScanInterval, stoppingToken);
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
    }

    private readonly record struct RefundDependencies(
        IChatFinanceRepository FinanceRepository,
        IWalletRepository WalletRepository,
        IConversationRepository ConversationRepository,
        IChatMessageRepository MessageRepository,
        IDomainEventPublisher DomainEventPublisher,
        ITransactionCoordinator TransactionCoordinator);
}

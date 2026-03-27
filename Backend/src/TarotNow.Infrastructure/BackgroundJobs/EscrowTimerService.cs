using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService : BackgroundService
{
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(60);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EscrowTimerService> _logger;

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

    private async Task ProcessTimers(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var financeRepo = scope.ServiceProvider.GetRequiredService<IChatFinanceRepository>();
        var walletRepo = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
        var escrowSettlementService = scope.ServiceProvider.GetRequiredService<IEscrowSettlementService>();
        var transactionCoordinator = scope.ServiceProvider.GetRequiredService<ITransactionCoordinator>();

        await ProcessExpiredOffers(financeRepo, cancellationToken);
        await ProcessAutoRefunds(financeRepo, walletRepo, transactionCoordinator, cancellationToken);
        await ProcessAutoReleases(financeRepo, escrowSettlementService, transactionCoordinator, cancellationToken);
    }
}

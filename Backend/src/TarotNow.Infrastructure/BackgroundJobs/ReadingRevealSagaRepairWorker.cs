using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Worker repair saga reveal bị kẹt: hoàn tiền exactly-once cho case đã debit nhưng flow không thể hoàn tất.
/// </summary>
public sealed partial class ReadingRevealSagaRepairWorker : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(20);
    private const int BatchSize = 40;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReadingRevealSagaRepairWorker> _logger;

    public ReadingRevealSagaRepairWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<ReadingRevealSagaRepairWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Reading reveal saga repair worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBatchAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Reading reveal saga repair worker iteration failed.");
            }

            await Task.Delay(PollInterval, stoppingToken);
        }

        _logger.LogInformation("Reading reveal saga repair worker stopped.");
    }

    private async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var deps = ResolveDependencies(scope.ServiceProvider);
        var dueItems = await deps.SagaRepository.ListDueRepairAsync(DateTime.UtcNow, BatchSize, cancellationToken);
        foreach (var saga in dueItems)
        {
            await ProcessSagaAsync(deps, saga, cancellationToken);
        }
    }

    private async Task ProcessSagaAsync(
        RepairDependencies deps,
        ReadingRevealSagaState saga,
        CancellationToken cancellationToken)
    {
        if (saga.IsTerminal())
        {
            return;
        }

        if (ShouldDelayCompensation(saga))
        {
            await RequeueForInvestigationAsync(deps.SagaRepository, saga, saga.LastError, cancellationToken);
            return;
        }

        if (!TryBuildCompensationPayload(saga, out var payload))
        {
            await RequeueForInvestigationAsync(
                deps.SagaRepository,
                saga,
                "Charge snapshot missing amount for compensation.",
                cancellationToken);
            return;
        }

        await ExecuteCompensationAsync(deps, saga, payload, cancellationToken);
    }

    private async Task ExecuteCompensationAsync(
        RepairDependencies deps,
        ReadingRevealSagaState saga,
        CompensationPayload payload,
        CancellationToken cancellationToken)
    {
        await deps.TransactionCoordinator.ExecuteAsync(
            async transactionCt =>
            {
                var refundResult = await deps.WalletRepository.CreditWithResultAsync(
                    saga.UserId,
                    payload.Currency,
                    TransactionType.ReadingRefund,
                    payload.Amount,
                    referenceSource: "Reading",
                    payload.ReferenceId,
                    description: "Saga repair compensation refund for failed reading reveal flow.",
                    metadataJson: null,
                    idempotencyKey: $"reading_reveal_refund_{payload.ReferenceId}",
                    cancellationToken: transactionCt);

                if (refundResult.Executed)
                {
                    await deps.DomainEventPublisher.PublishAsync(
                        new MoneyChangedDomainEvent
                        {
                            UserId = saga.UserId,
                            Currency = payload.Currency,
                            ChangeType = TransactionType.ReadingRefund,
                            DeltaAmount = payload.Amount,
                            ReferenceId = payload.ReferenceId
                        },
                        transactionCt);
                }

                saga.MarkCompensated(DateTime.UtcNow);
                await deps.SagaRepository.UpdateAsync(saga, transactionCt);
            },
            cancellationToken);
    }
}

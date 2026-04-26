using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed partial class ReadingRevealSagaRepairWorker
{
    private static readonly TimeSpan RetryBackoff = TimeSpan.FromSeconds(60);

    private static RepairDependencies ResolveDependencies(IServiceProvider serviceProvider)
    {
        return new RepairDependencies(
            SagaRepository: serviceProvider.GetRequiredService<IReadingRevealSagaStateRepository>(),
            WalletRepository: serviceProvider.GetRequiredService<IWalletRepository>(),
            DomainEventPublisher: serviceProvider.GetRequiredService<IDomainEventPublisher>(),
            TransactionCoordinator: serviceProvider.GetRequiredService<ITransactionCoordinator>());
    }

    private static bool ShouldDelayCompensation(ReadingRevealSagaState saga)
    {
        return !saga.ChargeDebited || saga.RefundCompensated || saga.SessionCompleted;
    }

    private static bool TryBuildCompensationPayload(
        ReadingRevealSagaState saga,
        out CompensationPayload payload)
    {
        payload = default;
        var amount = Math.Max(0, saga.ChargeAmount);
        if (amount <= 0)
        {
            return false;
        }

        var currency = string.IsNullOrWhiteSpace(saga.ChargeCurrency)
            ? CurrencyType.Gold
            : saga.ChargeCurrency!;
        var referenceId = string.IsNullOrWhiteSpace(saga.ChargeReferenceId)
            ? saga.SessionId
            : saga.ChargeReferenceId!;

        payload = new CompensationPayload(currency, referenceId, amount);
        return true;
    }

    private static async Task RequeueForInvestigationAsync(
        IReadingRevealSagaStateRepository sagaRepository,
        ReadingRevealSagaState saga,
        string? reason,
        CancellationToken cancellationToken)
    {
        saga.MarkFailed(
            error: reason ?? "Awaiting manual investigation.",
            nowUtc: DateTime.UtcNow,
            nextRepairAtUtc: DateTime.UtcNow.Add(RetryBackoff));
        await sagaRepository.UpdateAsync(saga, cancellationToken);
    }

    private readonly record struct RepairDependencies(
        IReadingRevealSagaStateRepository SagaRepository,
        IWalletRepository WalletRepository,
        IDomainEventPublisher DomainEventPublisher,
        ITransactionCoordinator TransactionCoordinator);

    private readonly record struct CompensationPayload(
        string Currency,
        string ReferenceId,
        long Amount);
}

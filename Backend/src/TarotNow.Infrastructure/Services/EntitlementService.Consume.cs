using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

public partial class EntitlementService
{
    public async Task<EntitlementConsumeResult> ConsumeAsync(EntitlementConsumeRequest request, CancellationToken ct)
    {
        var outcome = new EntitlementConsumeResult(false, "Unknown error");
        await _transactionCoordinator.ExecuteAsync(
            async cancellation => outcome = await ConsumeCoreAsync(request, cancellation),
            ct);

        if (outcome.Success)
        {
            await _cacheService.RemoveAsync(BuildBalanceCacheKey(request.UserId));
        }

        return outcome;
    }

    private async Task<EntitlementConsumeResult> ConsumeCoreAsync(EntitlementConsumeRequest request, CancellationToken ct)
    {
        if (await _repository.ConsumeLogExistsAsync(request.IdempotencyKey, ct))
        {
            _logger.LogInformation("Consume Idempotency Key {Key} already exists.", request.IdempotencyKey);
            return new EntitlementConsumeResult(true, "Already consumed (idempotent)");
        }

        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        var buckets = await _repository.GetBucketsForConsumeAsync(request.UserId, request.EntitlementKey, todayUtc, ct);
        var targetBucket = buckets.FirstOrDefault(b => b.CanConsume(todayUtc));
        if (targetBucket == null) return await BuildNoQuotaResultAsync(request.EntitlementKey, ct);

        await ConsumeBucketAsync(request, targetBucket, todayUtc, ct);
        return new EntitlementConsumeResult(true, "Consumed successfully");
    }

    private async Task ConsumeBucketAsync(
        EntitlementConsumeRequest request,
        SubscriptionEntitlementBucket bucket,
        DateOnly businessDate,
        CancellationToken ct)
    {
        bucket.Consume(businessDate);
        var log = new EntitlementConsume(
            userId: request.UserId,
            bucketId: bucket.Id,
            entitlementKey: request.EntitlementKey,
            referenceSource: request.ReferenceSource,
            referenceId: request.ReferenceId,
            idempotencyKey: request.IdempotencyKey);

        await _repository.AddConsumeLogAsync(log, ct);
        await _repository.SaveChangesAsync(ct);
        await _domainEventPublisher.PublishAsync(
            new EntitlementConsumedDomainEvent(request.UserId, request.EntitlementKey, bucket.Id),
            ct);
    }

    private async Task<EntitlementConsumeResult> BuildNoQuotaResultAsync(string entitlementKey, CancellationToken ct)
    {
        var mapRules = await _repository.GetEnabledMappingRulesAsync(entitlementKey, ct);
        if (mapRules.Any())
        {
            _logger.LogInformation("Cross mapping is configured but currently disabled by business policy.");
        }

        return new EntitlementConsumeResult(false, "No active quota available for this entitlement");
    }

    private static string BuildBalanceCacheKey(Guid userId) => $"entitlement_balance:{userId}";
}

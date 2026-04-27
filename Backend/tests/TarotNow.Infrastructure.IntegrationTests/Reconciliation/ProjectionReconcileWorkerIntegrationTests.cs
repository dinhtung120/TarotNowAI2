using System.Reflection;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.BackgroundJobs;
using TarotNow.Infrastructure.IntegrationTests.Outbox;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.IntegrationTests.Reconciliation;

[Collection("InfrastructurePostgres")]
public sealed class ProjectionReconcileWorkerIntegrationTests
{
    private readonly InfrastructurePostgresFixture _fixture;

    public ProjectionReconcileWorkerIntegrationTests(InfrastructurePostgresFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task RequeueDeadLetterProjectionEventsAsync_ShouldRequeueOnlyProjectionEventTypes()
    {
        await _fixture.ResetOutboxAsync();

        await using var dbContext = _fixture.CreateDbContext();
        var now = DateTime.UtcNow;

        dbContext.OutboxMessages.AddRange(
            new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = typeof(ReadingSessionContentSyncRequestedDomainEvent).FullName!,
                PayloadJson = "{}",
                OccurredAtUtc = now,
                Status = OutboxMessageStatus.DeadLetter,
                AttemptCount = 10,
                NextAttemptAtUtc = now.AddHours(1),
                CreatedAtUtc = now,
                LastError = "projection-failed"
            },
            new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = typeof(MoneyChangedDomainEvent).FullName!,
                PayloadJson = "{}",
                OccurredAtUtc = now,
                Status = OutboxMessageStatus.DeadLetter,
                AttemptCount = 10,
                NextAttemptAtUtc = now.AddHours(1),
                CreatedAtUtc = now
            });
        await dbContext.SaveChangesAsync();

        var method = typeof(ProjectionReconcileWorker).GetMethod(
            "RequeueDeadLetterProjectionEventsAsync",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);

        var task = (Task<int>)method!.Invoke(null, new object[] { dbContext, CancellationToken.None })!;
        var requeued = await task;
        Assert.Equal(1, requeued);

        var messages = dbContext.OutboxMessages.OrderBy(x => x.EventType).ToArray();
        Assert.Equal(2, messages.Length);

        var moneyChanged = messages.Single(x => x.EventType == typeof(MoneyChangedDomainEvent).FullName);
        Assert.Equal(OutboxMessageStatus.DeadLetter, moneyChanged.Status);

        var readingSync = messages.Single(x => x.EventType == typeof(ReadingSessionContentSyncRequestedDomainEvent).FullName);
        Assert.Equal(OutboxMessageStatus.Failed, readingSync.Status);
        Assert.NotNull(readingSync.LastError);
        Assert.Contains("requeued_at=", readingSync.LastError!, StringComparison.Ordinal);
    }
}

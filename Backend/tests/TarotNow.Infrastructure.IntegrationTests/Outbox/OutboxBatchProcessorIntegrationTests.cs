using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.BackgroundJobs.Outbox;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.IntegrationTests.Outbox;

[Collection("InfrastructurePostgres")]
// Kiểm thử crash-recovery và idempotency cho outbox batch processor.
public sealed class OutboxBatchProcessorIntegrationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly InfrastructurePostgresFixture _fixture;

    /// <summary>
    /// Khởi tạo test class outbox processor.
    /// </summary>
    public OutboxBatchProcessorIntegrationTests(InfrastructurePostgresFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Xác nhận message processing có stale lock được reclaim và xử lý thành công.
    /// </summary>
    [Fact]
    public async Task ProcessOnceAsync_ShouldRecover_StaleProcessingLock()
    {
        await _fixture.ResetOutboxAsync();
        var nowUtc = DateTime.UtcNow;
        var message = CreateOutboxMessage(
            status: OutboxMessageStatus.Processing,
            attemptCount: 0,
            nextAttemptAtUtc: nowUtc,
            createdAtUtc: nowUtc.AddMinutes(-10),
            lockedAtUtc: nowUtc.AddMinutes(-5));

        await SeedOutboxMessageAsync(message);

        using var serviceProvider = BuildServiceProvider();
        await ProcessOnceAsync(serviceProvider);

        await using var assertContext = _fixture.CreateDbContext();
        var updated = await assertContext.OutboxMessages.SingleAsync(x => x.Id == message.Id);

        Assert.Equal(OutboxMessageStatus.Processed, updated.Status);
        Assert.NotNull(updated.ProcessedAtUtc);
    }

    /// <summary>
    /// Xác nhận message failed đã tới hạn retry sẽ được xử lý lại thành công.
    /// </summary>
    [Fact]
    public async Task ProcessOnceAsync_ShouldRecover_FailedMessageDueForRetry()
    {
        await _fixture.ResetOutboxAsync();
        var nowUtc = DateTime.UtcNow;
        var message = CreateOutboxMessage(
            status: OutboxMessageStatus.Failed,
            attemptCount: 2,
            nextAttemptAtUtc: nowUtc.AddSeconds(-30),
            createdAtUtc: nowUtc.AddMinutes(-12));

        await SeedOutboxMessageAsync(message);

        using var serviceProvider = BuildServiceProvider();
        await ProcessOnceAsync(serviceProvider);

        await using var assertContext = _fixture.CreateDbContext();
        var updated = await assertContext.OutboxMessages.SingleAsync(x => x.Id == message.Id);

        Assert.Equal(OutboxMessageStatus.Processed, updated.Status);
        Assert.NotNull(updated.ProcessedAtUtc);
    }

    /// <summary>
    /// Xác nhận message failed chạm ngưỡng retry tối đa sẽ chuyển dead-letter.
    /// </summary>
    [Fact]
    public async Task ProcessOnceAsync_ShouldMoveToDeadLetter_WhenRetryThresholdReached()
    {
        await _fixture.ResetOutboxAsync();
        var nowUtc = DateTime.UtcNow;
        var message = CreateOutboxMessage(
            status: OutboxMessageStatus.Failed,
            attemptCount: 11,
            nextAttemptAtUtc: nowUtc.AddSeconds(-10),
            createdAtUtc: nowUtc.AddMinutes(-20));

        await SeedOutboxMessageAsync(message);

        using var serviceProvider = BuildServiceProvider(shouldThrow: true);
        await ProcessOnceAsync(serviceProvider);

        await using var assertContext = _fixture.CreateDbContext();
        var updated = await assertContext.OutboxMessages.SingleAsync(x => x.Id == message.Id);

        Assert.Equal(OutboxMessageStatus.DeadLetter, updated.Status);
        Assert.Equal(12, updated.AttemptCount);
        Assert.False(string.IsNullOrWhiteSpace(updated.LastError));
    }

    /// <summary>
    /// Xác nhận idempotency theo từng handler sẽ chặn side-effect chạy trùng.
    /// </summary>
    [Fact]
    public async Task ProcessOnceAsync_ShouldSkipSideEffect_WhenHandlerStateAlreadyExists()
    {
        await _fixture.ResetOutboxAsync();
        var nowUtc = DateTime.UtcNow;
        var message = CreateOutboxMessage(
            status: OutboxMessageStatus.Failed,
            attemptCount: 1,
            nextAttemptAtUtc: nowUtc.AddSeconds(-10),
            createdAtUtc: nowUtc.AddMinutes(-15));

        await SeedOutboxMessageWithHandlerStateAsync(message, HandlerIdentity.Name);

        using var serviceProvider = BuildServiceProvider();
        var probe = serviceProvider.GetRequiredService<OutboxHandlerProbe>();

        await ProcessOnceAsync(serviceProvider);

        await using var assertContext = _fixture.CreateDbContext();
        var updated = await assertContext.OutboxMessages.SingleAsync(x => x.Id == message.Id);

        Assert.Equal(OutboxMessageStatus.Processed, updated.Status);
        Assert.Equal(0, probe.ExecutionCount);
    }

    private ServiceProvider BuildServiceProvider(bool shouldThrow = false)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ProbeMoneyChangedHandler>());
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_fixture.ConnectionString));
        services.AddOptions<OutboxOptions>();
        services.AddScoped<IOutboxBatchProcessor, OutboxBatchProcessor>();
        services.AddScoped<IEventHandlerIdempotencyService, OutboxHandlerIdempotencyService>();
        services.AddSingleton(new OutboxHandlerProbe(shouldThrow));
        return services.BuildServiceProvider();
    }

    private async Task ProcessOnceAsync(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<IOutboxBatchProcessor>();
        await processor.ProcessOnceAsync();
    }

    private async Task SeedOutboxMessageAsync(OutboxMessage message)
    {
        await using var dbContext = _fixture.CreateDbContext();
        dbContext.OutboxMessages.Add(message);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedOutboxMessageWithHandlerStateAsync(OutboxMessage message, string handlerName)
    {
        await using var dbContext = _fixture.CreateDbContext();
        dbContext.OutboxMessages.Add(message);
        dbContext.OutboxHandlerStates.Add(new OutboxHandlerState
        {
            Id = Guid.NewGuid(),
            OutboxMessageId = message.Id,
            HandlerName = handlerName,
            ProcessedAtUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }

    private static OutboxMessage CreateOutboxMessage(
        string status,
        int attemptCount,
        DateTime nextAttemptAtUtc,
        DateTime createdAtUtc,
        DateTime? lockedAtUtc = null)
    {
        var domainEvent = new MoneyChangedDomainEvent
        {
            UserId = Guid.NewGuid(),
            Currency = "gold",
            ChangeType = "credit",
            ReferenceId = Guid.NewGuid().ToString("N"),
            OccurredAtUtc = createdAtUtc
        };

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = typeof(MoneyChangedDomainEvent).FullName!,
            PayloadJson = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), JsonOptions),
            OccurredAtUtc = domainEvent.OccurredAtUtc,
            Status = status,
            AttemptCount = attemptCount,
            NextAttemptAtUtc = nextAttemptAtUtc,
            CreatedAtUtc = createdAtUtc,
            LockedAtUtc = lockedAtUtc,
            LockOwner = lockedAtUtc.HasValue ? "stale-worker" : null
        };
    }

    private static class HandlerIdentity
    {
        public static string Name => typeof(ProbeMoneyChangedHandler).FullName ?? nameof(ProbeMoneyChangedHandler);
    }

    private sealed class OutboxHandlerProbe
    {
        public OutboxHandlerProbe(bool shouldThrow)
        {
            ShouldThrow = shouldThrow;
        }

        public bool ShouldThrow { get; }

        public int ExecutionCount { get; private set; }

        public void MarkExecuted()
        {
            ExecutionCount += 1;
        }
    }

    private sealed class ProbeMoneyChangedHandler : IdempotentDomainEventNotificationHandler<MoneyChangedDomainEvent>
    {
        private readonly OutboxHandlerProbe _probe;

        public ProbeMoneyChangedHandler(
            IEventHandlerIdempotencyService idempotencyService,
            OutboxHandlerProbe probe)
            : base(idempotencyService)
        {
            _probe = probe;
        }

        protected override Task HandleDomainEventAsync(
            MoneyChangedDomainEvent domainEvent,
            Guid? outboxMessageId,
            CancellationToken cancellationToken)
        {
            if (_probe.ShouldThrow)
            {
                throw new InvalidOperationException("Simulated handler failure.");
            }

            _probe.MarkExecuted();
            return Task.CompletedTask;
        }
    }
}

using System.Collections.Concurrent;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Common;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.BackgroundJobs.Outbox;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;
using TarotNow.Infrastructure.Services.Configuration;

namespace TarotNow.Infrastructure.IntegrationTests.Outbox;

[Collection("InfrastructurePostgres")]
public sealed class ChatModerationOutboxIntegrationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly InfrastructurePostgresFixture _fixture;

    public ChatModerationOutboxIntegrationTests(InfrastructurePostgresFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ProcessOnceAsync_ShouldMoveToDeadLetter_WhenModerationDisabled()
    {
        await _fixture.ResetOutboxAsync();
        var message = CreateOutboxMessage(attemptCount: 11);
        await SeedOutboxMessageAsync(message);

        var settings = new MutableModerationSettings
        {
            Enabled = false,
            Keywords = ["scam"]
        };
        var reportRepository = new InMemoryReportRepository();
        var chatMessageRepository = new InMemoryChatMessageRepository();
        using var serviceProvider = BuildServiceProvider(settings, reportRepository, chatMessageRepository);

        await ProcessOnceAsync(serviceProvider);

        await using var assertContext = _fixture.CreateDbContext();
        var updated = await assertContext.OutboxMessages.SingleAsync(x => x.Id == message.Id);

        Assert.Equal(OutboxMessageStatus.DeadLetter, updated.Status);
        Assert.Equal(12, updated.AttemptCount);
        Assert.Empty(reportRepository.Items);
        Assert.Equal(0, chatMessageRepository.FlagUpdateCount);
    }

    [Fact]
    public async Task ProcessOnceAsync_ShouldReplaySuccessfully_WhenModerationIsEnabledAgain()
    {
        await _fixture.ResetOutboxAsync();
        var message = CreateOutboxMessage(attemptCount: 11);
        await SeedOutboxMessageAsync(message);

        var settings = new MutableModerationSettings
        {
            Enabled = false,
            Keywords = ["scam"]
        };
        var reportRepository = new InMemoryReportRepository();
        var chatMessageRepository = new InMemoryChatMessageRepository();
        using var serviceProvider = BuildServiceProvider(settings, reportRepository, chatMessageRepository);

        await ProcessOnceAsync(serviceProvider);
        await AssertOutboxStatusAsync(message.Id, OutboxMessageStatus.DeadLetter);

        settings.Enabled = true;
        await RequeueDeadLetterMessageAsFailedAsync(message.Id);
        await ProcessOnceAsync(serviceProvider);
        await AssertOutboxStatusAsync(message.Id, OutboxMessageStatus.Processed);

        var reportCountAfterReplay = reportRepository.Items.Count;
        var flagCountAfterReplay = chatMessageRepository.FlagUpdateCount;

        await ProcessOnceAsync(serviceProvider);

        Assert.Equal(1, reportCountAfterReplay);
        Assert.Equal(1, flagCountAfterReplay);
        Assert.Equal(reportCountAfterReplay, reportRepository.Items.Count);
        Assert.Equal(flagCountAfterReplay, chatMessageRepository.FlagUpdateCount);
    }

    private ServiceProvider BuildServiceProvider(
        MutableModerationSettings settings,
        InMemoryReportRepository reportRepository,
        InMemoryChatMessageRepository chatMessageRepository)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ChatModerationRequestedDomainEventHandler>();
        });
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_fixture.ConnectionString));
        services.AddOptions<SystemConfigOptions>();
        services.AddOptions<OutboxOptions>();
        services.AddSingleton<SystemConfigSnapshotStore>();
        services.AddScoped<ISystemConfigSettings, SystemConfigSettings>();
        services.AddScoped<IOutboxBatchProcessor, OutboxBatchProcessor>();
        services.AddScoped<IEventHandlerIdempotencyService, OutboxHandlerIdempotencyService>();
        services.AddSingleton<IChatModerationSettings>(settings);
        services.AddSingleton<IReportRepository>(reportRepository);
        services.AddSingleton<IChatMessageRepository>(chatMessageRepository);
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

    private async Task AssertOutboxStatusAsync(Guid messageId, string expectedStatus)
    {
        await using var dbContext = _fixture.CreateDbContext();
        var updated = await dbContext.OutboxMessages.SingleAsync(x => x.Id == messageId);
        Assert.Equal(expectedStatus, updated.Status);
    }

    private async Task RequeueDeadLetterMessageAsFailedAsync(Guid messageId)
    {
        await using var dbContext = _fixture.CreateDbContext();
        var message = await dbContext.OutboxMessages.SingleAsync(x => x.Id == messageId);
        message.Status = OutboxMessageStatus.Failed;
        message.NextAttemptAtUtc = DateTime.UtcNow.AddSeconds(-1);
        message.LockOwner = null;
        message.LockedAtUtc = null;
        await dbContext.SaveChangesAsync();
    }

    private static OutboxMessage CreateOutboxMessage(int attemptCount)
    {
        var nowUtc = DateTime.UtcNow;
        var domainEvent = new ChatModerationRequestedDomainEvent
        {
            MessageId = "msg-moderation-1",
            ConversationId = "conv-moderation-1",
            SenderId = "sender-moderation-1",
            MessageType = "text",
            Content = "this contains scam keyword",
            CreatedAtUtc = nowUtc,
            OccurredAtUtc = nowUtc
        };

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = typeof(ChatModerationRequestedDomainEvent).FullName!,
            PayloadJson = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), JsonOptions),
            OccurredAtUtc = domainEvent.OccurredAtUtc,
            Status = OutboxMessageStatus.Failed,
            AttemptCount = attemptCount,
            NextAttemptAtUtc = nowUtc.AddSeconds(-10),
            CreatedAtUtc = nowUtc.AddMinutes(-10)
        };
    }

    private sealed class MutableModerationSettings : IChatModerationSettings
    {
        public bool Enabled { get; set; }

        public IReadOnlyCollection<string> Keywords { get; set; } = Array.Empty<string>();
    }

    private sealed class InMemoryReportRepository : IReportRepository
    {
        private readonly ConcurrentBag<ReportDto> _items = new();

        public IReadOnlyCollection<ReportDto> Items => _items.ToArray();

        public Task AddAsync(ReportDto report, CancellationToken cancellationToken = default)
        {
            _items.Add(report);
            return Task.CompletedTask;
        }

        public Task<ReportDto?> GetByIdAsync(string reportId, CancellationToken cancellationToken = default)
        {
            var item = _items.FirstOrDefault(x => x.Id == reportId);
            return Task.FromResult<ReportDto?>(item);
        }

        public Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? statusFilter = null,
            string? targetType = null,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<ReportDto> query = _items.ToArray();
            if (string.IsNullOrWhiteSpace(statusFilter) == false)
            {
                query = query.Where(x => string.Equals(x.Status, statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (string.IsNullOrWhiteSpace(targetType) == false)
            {
                query = query.Where(x => string.Equals(x.TargetType, targetType, StringComparison.OrdinalIgnoreCase));
            }

            var materialized = query.ToArray();
            return Task.FromResult<(IEnumerable<ReportDto> Items, long TotalCount)>((materialized, materialized.LongLength));
        }

        public Task<bool> ResolveAsync(
            string reportId,
            string status,
            string result,
            string resolvedBy,
            string? adminNote,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }

    private sealed class InMemoryChatMessageRepository : IChatMessageRepository
    {
        private int _flagUpdateCount;

        public int FlagUpdateCount => _flagUpdateCount;

        public Task<ChatMessageDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ChatMessageDto?>(null);
        }

        public Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
            string conversationId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<(IEnumerable<ChatMessageDto>, long)>((Array.Empty<ChatMessageDto>(), 0));
        }

        public Task<(IReadOnlyList<ChatMessageDto> Items, string? NextCursor)> GetByConversationIdCursorAsync(
            string conversationId,
            string? cursor,
            int limit,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<(IReadOnlyList<ChatMessageDto>, string?)>((Array.Empty<ChatMessageDto>(), null));
        }

        public Task<bool> HasPaymentOfferResponseAsync(
            string conversationId,
            string offerMessageId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        public Task<ChatMessageDto?> FindLatestPendingPaymentOfferAsync(
            string conversationId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ChatMessageDto?>(null);
        }

        public Task<IReadOnlyList<ChatMessageDto>> GetExpiredPendingPaymentOffersAsync(
            DateTime nowUtc,
            int limit = 200,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<ChatMessageDto>>(Array.Empty<ChatMessageDto>());
        }

        public Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0L);
        }

        public Task<IEnumerable<ChatMessageDto>> GetLatestMessagesAsync(
            IEnumerable<string> conversationIds,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<ChatMessageDto>>(Array.Empty<ChatMessageDto>());
        }

        public Task UpdateFlagAsync(string messageId, bool isFlagged, CancellationToken cancellationToken = default)
        {
            if (isFlagged)
            {
                Interlocked.Increment(ref _flagUpdateCount);
            }

            return Task.CompletedTask;
        }
    }
}

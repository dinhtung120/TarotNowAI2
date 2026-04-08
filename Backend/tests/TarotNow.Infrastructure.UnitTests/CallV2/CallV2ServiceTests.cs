using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Services.CallV2;

namespace TarotNow.Infrastructure.UnitTests.CallV2;

public sealed class CallV2ServiceTests
{
    private static class TestOptions
    {
        public static IOptions<T> Create<T>(T value) where T : class => Microsoft.Extensions.Options.Options.Create(value);
    }

    private static readonly Guid CallerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid CalleeId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public async Task StartAsync_ShouldCreateRequestedSession_AndBroadcastIncoming()
    {
        var conversationId = "conv-start-001";
        var sessionRepo = new InMemoryCallSessionV2Repository();
        var roomGateway = new FakeLiveKitRoomGateway();
        var realtimePush = new RecordingRealtimePushService();
        var conversationRepo = BuildConversationRepository(conversationId, CallerId, CalleeId, "ongoing");
        var service = BuildService(sessionRepo, conversationRepo.Object, roomGateway, realtimePush);

        var ticket = await service.StartAsync(CallerId, conversationId, CallTypeValues.Audio);

        Assert.False(string.IsNullOrWhiteSpace(ticket.Session.Id));
        Assert.Equal(CallSessionV2Statuses.Requested, ticket.Session.Status);
        Assert.Equal(conversationId, ticket.Session.ConversationId);
        Assert.Equal("https://livekit.test", ticket.LiveKitUrl);
        Assert.Equal("user:" + CallerId, ticket.ParticipantIdentity);
        Assert.Single(realtimePush.IncomingEvents);
        Assert.Single(roomGateway.CreatedRooms);
        Assert.Equal(60, ticket.Timeouts.RingTimeoutSeconds);
        Assert.Equal(45, ticket.Timeouts.JoinTimeoutSeconds);
    }

    [Fact]
    public async Task AcceptAsync_ShouldBeIdempotent_AndBroadcastAcceptedOnce()
    {
        var conversationId = "conv-accept-001";
        var sessionRepo = new InMemoryCallSessionV2Repository();
        var roomGateway = new FakeLiveKitRoomGateway();
        var realtimePush = new RecordingRealtimePushService();
        var conversationRepo = BuildConversationRepository(conversationId, CallerId, CalleeId, "ongoing");
        var service = BuildService(sessionRepo, conversationRepo.Object, roomGateway, realtimePush);
        var session = SeedRequestedSession(sessionRepo, conversationId, CallerId, CalleeId);

        var firstTicket = await service.AcceptAsync(CalleeId, session.Id);
        var secondTicket = await service.AcceptAsync(CalleeId, session.Id);

        Assert.Equal(CallSessionV2Statuses.Accepted, firstTicket.Session.Status);
        Assert.Equal(CallSessionV2Statuses.Accepted, secondTicket.Session.Status);
        Assert.Single(realtimePush.AcceptedEvents);

        var persisted = await sessionRepo.GetByIdAsync(session.Id);
        Assert.NotNull(persisted);
        Assert.Equal(CallSessionV2Statuses.Accepted, persisted!.Status);
    }

    [Fact]
    public async Task EndAsync_ShouldBeIdempotent_AndBroadcastEndedOnce()
    {
        var conversationId = "conv-end-001";
        var sessionRepo = new InMemoryCallSessionV2Repository();
        var roomGateway = new FakeLiveKitRoomGateway();
        var realtimePush = new RecordingRealtimePushService();
        var conversationRepo = BuildConversationRepository(conversationId, CallerId, CalleeId, "ongoing");
        var service = BuildService(sessionRepo, conversationRepo.Object, roomGateway, realtimePush);
        var session = SeedAcceptedSession(sessionRepo, conversationId, CallerId, CalleeId);

        var firstResult = await service.EndAsync(CallerId, session.Id, "normal");
        var secondResult = await service.EndAsync(CallerId, session.Id, "normal");

        Assert.Equal(CallSessionV2Statuses.Ended, firstResult.Status);
        Assert.Equal(CallSessionV2Statuses.Ended, secondResult.Status);
        Assert.Single(roomGateway.DeletedRooms);
        Assert.Single(realtimePush.EndedEvents);
    }

    [Fact]
    public async Task EndAsync_ShouldRejectUserOutsideSession()
    {
        var conversationId = "conv-auth-001";
        var sessionRepo = new InMemoryCallSessionV2Repository();
        var roomGateway = new FakeLiveKitRoomGateway();
        var realtimePush = new RecordingRealtimePushService();
        var conversationRepo = BuildConversationRepository(conversationId, CallerId, CalleeId, "ongoing");
        var service = BuildService(sessionRepo, conversationRepo.Object, roomGateway, realtimePush);
        var session = SeedAcceptedSession(sessionRepo, conversationId, CallerId, CalleeId);

        var outsider = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => service.EndAsync(outsider, session.Id, "normal"));

        Assert.Equal(CallV2ErrorCodes.CallNotAllowed, exception.ErrorCode);
    }

    [Fact]
    public async Task ProcessTimeoutsAsync_ShouldFinalizeExpiredRequested_AndJoiningSessions()
    {
        var sessionRepo = new InMemoryCallSessionV2Repository();
        var realtimePush = new RecordingRealtimePushService();
        var options = TestOptions.Create(new CallV2Options
        {
            RingTimeoutSeconds = 10,
            JoinTimeoutSeconds = 10,
            ReconnectGracePeriodSeconds = 15,
            TimeoutSweepIntervalSeconds = 5,
        });
        var service = new CallV2MaintenanceService(sessionRepo, realtimePush, options, NullLogger<CallV2MaintenanceService>.Instance);

        var requestedSession = new CallSessionV2Dto
        {
            Id = "requested-timeout",
            ConversationId = "conv-timeout-1",
            RoomName = "room-timeout-1",
            InitiatorId = CallerId.ToString(),
            CalleeId = CalleeId.ToString(),
            Type = CallTypeValues.Audio,
            Status = CallSessionV2Statuses.Requested,
            CreatedAt = DateTime.UtcNow.AddMinutes(-5),
            UpdatedAt = DateTime.UtcNow.AddMinutes(-5),
        };

        var joiningSession = new CallSessionV2Dto
        {
            Id = "joining-timeout",
            ConversationId = "conv-timeout-2",
            RoomName = "room-timeout-2",
            InitiatorId = CallerId.ToString(),
            CalleeId = CalleeId.ToString(),
            Type = CallTypeValues.Video,
            Status = CallSessionV2Statuses.Joining,
            CreatedAt = DateTime.UtcNow.AddMinutes(-5),
            UpdatedAt = DateTime.UtcNow.AddMinutes(-5),
        };

        sessionRepo.Seed(requestedSession);
        sessionRepo.Seed(joiningSession);

        await service.ProcessTimeoutsAsync();

        var updatedRequested = await sessionRepo.GetByIdAsync(requestedSession.Id);
        var updatedJoining = await sessionRepo.GetByIdAsync(joiningSession.Id);
        Assert.NotNull(updatedRequested);
        Assert.NotNull(updatedJoining);
        Assert.Equal(CallSessionV2Statuses.Ended, updatedRequested!.Status);
        Assert.Equal("timeout_server", updatedRequested.EndReason);
        Assert.Equal(CallSessionV2Statuses.Failed, updatedJoining!.Status);
        Assert.Equal("join_timeout", updatedJoining.EndReason);
        Assert.Equal(2, realtimePush.EndedEvents.Count);
    }

    [Fact]
    public async Task IssueTokenAsync_ShouldFailForFinalSession()
    {
        var conversationId = "conv-token-001";
        var sessionRepo = new InMemoryCallSessionV2Repository();
        var roomGateway = new FakeLiveKitRoomGateway();
        var realtimePush = new RecordingRealtimePushService();
        var conversationRepo = BuildConversationRepository(conversationId, CallerId, CalleeId, "ongoing");
        var service = BuildService(sessionRepo, conversationRepo.Object, roomGateway, realtimePush);
        var session = SeedAcceptedSession(sessionRepo, conversationId, CallerId, CalleeId);
        await service.EndAsync(CallerId, session.Id, "normal");

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => service.IssueTokenAsync(CallerId, session.Id));

        Assert.Equal(CallV2ErrorCodes.TokenExpired, exception.ErrorCode);
    }

    private static Mock<IConversationRepository> BuildConversationRepository(
        string conversationId,
        Guid callerId,
        Guid calleeId,
        string status)
    {
        var mock = new Mock<IConversationRepository>();
        mock.Setup(x => x.GetByIdAsync(conversationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConversationDto
            {
                Id = conversationId,
                UserId = callerId.ToString(),
                ReaderId = calleeId.ToString(),
                Status = status,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
            });
        return mock;
    }

    private static CallV2Service BuildService(
        ICallSessionV2Repository sessionRepo,
        IConversationRepository conversationRepo,
        ILiveKitRoomGateway roomGateway,
        ICallRealtimePushService realtimePush)
    {
        var liveKitOptions = TestOptions.Create(new LiveKitOptions
        {
            Url = "https://livekit.test",
            ApiKey = "lk_test_key",
            ApiSecret = "lk_test_secret_128_bits_minimum_value",
            TokenTtlMinutes = 30,
        });

        var callOptions = TestOptions.Create(new CallV2Options
        {
            RingTimeoutSeconds = 60,
            JoinTimeoutSeconds = 45,
            ReconnectGracePeriodSeconds = 15,
            TimeoutSweepIntervalSeconds = 15,
        });

        var tokenFactory = new LiveKitTokenFactory(liveKitOptions);
        return new CallV2Service(
            sessionRepo,
            conversationRepo,
            roomGateway,
            tokenFactory,
            realtimePush,
            liveKitOptions,
            callOptions,
            NullLogger<CallV2Service>.Instance);
    }

    private static CallSessionV2Dto SeedRequestedSession(
        InMemoryCallSessionV2Repository repo,
        string conversationId,
        Guid callerId,
        Guid calleeId)
    {
        var session = new CallSessionV2Dto
        {
            Id = "session-requested-" + Guid.NewGuid().ToString("N"),
            ConversationId = conversationId,
            RoomName = "room-" + Guid.NewGuid().ToString("N"),
            InitiatorId = callerId.ToString(),
            CalleeId = calleeId.ToString(),
            Type = CallTypeValues.Audio,
            Status = CallSessionV2Statuses.Requested,
            CreatedAt = DateTime.UtcNow.AddSeconds(-10),
            UpdatedAt = DateTime.UtcNow.AddSeconds(-10),
        };

        repo.Seed(session);
        return session;
    }

    private static CallSessionV2Dto SeedAcceptedSession(
        InMemoryCallSessionV2Repository repo,
        string conversationId,
        Guid callerId,
        Guid calleeId)
    {
        var session = new CallSessionV2Dto
        {
            Id = "session-accepted-" + Guid.NewGuid().ToString("N"),
            ConversationId = conversationId,
            RoomName = "room-" + Guid.NewGuid().ToString("N"),
            InitiatorId = callerId.ToString(),
            CalleeId = calleeId.ToString(),
            Type = CallTypeValues.Audio,
            Status = CallSessionV2Statuses.Accepted,
            CreatedAt = DateTime.UtcNow.AddMinutes(-1),
            UpdatedAt = DateTime.UtcNow.AddMinutes(-1),
            AcceptedAt = DateTime.UtcNow.AddSeconds(-30),
        };

        repo.Seed(session);
        return session;
    }

    private sealed class FakeLiveKitRoomGateway : ILiveKitRoomGateway
    {
        public List<string> CreatedRooms { get; } = [];
        public List<string> DeletedRooms { get; } = [];
        public bool CreateRoomResult { get; set; } = true;

        public Task<bool> CreateRoomAsync(string roomName, CancellationToken ct = default)
        {
            CreatedRooms.Add(roomName);
            return Task.FromResult(CreateRoomResult);
        }

        public Task<bool> DeleteRoomAsync(string roomName, CancellationToken ct = default)
        {
            DeletedRooms.Add(roomName);
            return Task.FromResult(true);
        }
    }

    private sealed class RecordingRealtimePushService : ICallRealtimePushService
    {
        public List<CallSessionV2Dto> IncomingEvents { get; } = [];
        public List<CallSessionV2Dto> AcceptedEvents { get; } = [];
        public List<(CallSessionV2Dto Session, string Reason)> EndedEvents { get; } = [];

        public Task BroadcastIncomingAsync(CallSessionV2Dto session, CallTimeoutsDto? timeouts = null, CancellationToken ct = default)
        {
            IncomingEvents.Add(Clone(session));
            return Task.CompletedTask;
        }

        public Task BroadcastAcceptedAsync(CallSessionV2Dto session, CallTimeoutsDto? timeouts = null, CancellationToken ct = default)
        {
            AcceptedEvents.Add(Clone(session));
            return Task.CompletedTask;
        }

        public Task BroadcastEndedAsync(CallSessionV2Dto session, string reason, CancellationToken ct = default)
        {
            EndedEvents.Add((Clone(session), reason));
            return Task.CompletedTask;
        }
    }

    private sealed class InMemoryCallSessionV2Repository : ICallSessionV2Repository
    {
        private readonly Dictionary<string, CallSessionV2Dto> _store = new(StringComparer.Ordinal);

        public void Seed(CallSessionV2Dto session)
        {
            var cloned = Clone(session);
            if (string.IsNullOrWhiteSpace(cloned.Id))
            {
                cloned.Id = Guid.NewGuid().ToString("N");
            }

            _store[cloned.Id] = cloned;
        }

        public Task AddAsync(CallSessionV2Dto session, CancellationToken ct = default)
        {
            var cloned = Clone(session);
            if (string.IsNullOrWhiteSpace(cloned.Id))
            {
                cloned.Id = Guid.NewGuid().ToString("N");
            }

            _store.Add(cloned.Id, cloned);
            session.Id = cloned.Id;
            return Task.CompletedTask;
        }

        public Task<CallSessionV2Dto?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            return Task.FromResult(_store.TryGetValue(id, out var session) ? Clone(session) : null);
        }

        public Task<CallSessionV2Dto?> GetByRoomNameAsync(string roomName, CancellationToken ct = default)
        {
            var session = _store.Values.FirstOrDefault(x => x.RoomName == roomName);
            return Task.FromResult(session == null ? null : Clone(session));
        }

        public Task<CallSessionV2Dto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default)
        {
            var session = _store.Values.FirstOrDefault(x =>
                x.ConversationId == conversationId && CallSessionV2Statuses.IsActive(x.Status));
            return Task.FromResult(session == null ? null : Clone(session));
        }

        public Task<CallSessionV2Dto?> TryPatchAsync(string id, CallSessionV2Patch patch, CancellationToken ct = default)
        {
            if (_store.TryGetValue(id, out var current) == false)
            {
                return Task.FromResult<CallSessionV2Dto?>(null);
            }

            if (CanApplyPatch(current.Status, patch.ExpectedPreviousStatuses) == false)
            {
                return Task.FromResult<CallSessionV2Dto?>(null);
            }

            var updated = Clone(current);
            updated.Status = patch.NewStatus;
            updated.UpdatedAt = DateTime.UtcNow;
            if (patch.AcceptedAt.HasValue) updated.AcceptedAt = patch.AcceptedAt;
            if (patch.ConnectedAt.HasValue) updated.ConnectedAt = patch.ConnectedAt;
            if (patch.EndedAt.HasValue) updated.EndedAt = patch.EndedAt;
            if (patch.InitiatorJoinedAt.HasValue) updated.InitiatorJoinedAt = patch.InitiatorJoinedAt;
            if (patch.CalleeJoinedAt.HasValue) updated.CalleeJoinedAt = patch.CalleeJoinedAt;
            if (string.IsNullOrWhiteSpace(patch.EndReason) == false) updated.EndReason = patch.EndReason;

            _store[id] = updated;
            return Task.FromResult<CallSessionV2Dto?>(Clone(updated));
        }

        public Task<IReadOnlyList<CallSessionV2Dto>> ListStaleByStatusAsync(
            IReadOnlyCollection<string> statuses,
            DateTime updatedBeforeUtc,
            int limit,
            CancellationToken ct = default)
        {
            var set = new HashSet<string>(statuses, StringComparer.OrdinalIgnoreCase);
            var items = _store.Values
                .Where(x => set.Contains(x.Status) && x.UpdatedAt < updatedBeforeUtc)
                .OrderBy(x => x.UpdatedAt)
                .Take(Math.Max(0, limit))
                .Select(Clone)
                .ToArray();
            return Task.FromResult<IReadOnlyList<CallSessionV2Dto>>(items);
        }

        private static bool CanApplyPatch(string currentStatus, IReadOnlyCollection<string>? expectedStatuses)
        {
            if (expectedStatuses == null || expectedStatuses.Count == 0)
            {
                return true;
            }

            return expectedStatuses.Contains(currentStatus, StringComparer.OrdinalIgnoreCase);
        }
    }

    private static CallSessionV2Dto Clone(CallSessionV2Dto session)
    {
        return new CallSessionV2Dto
        {
            Id = session.Id,
            ConversationId = session.ConversationId,
            RoomName = session.RoomName,
            InitiatorId = session.InitiatorId,
            CalleeId = session.CalleeId,
            Type = session.Type,
            Status = session.Status,
            CreatedAt = session.CreatedAt,
            UpdatedAt = session.UpdatedAt,
            AcceptedAt = session.AcceptedAt,
            ConnectedAt = session.ConnectedAt,
            EndedAt = session.EndedAt,
            EndReason = session.EndReason,
            InitiatorJoinedAt = session.InitiatorJoinedAt,
            CalleeJoinedAt = session.CalleeJoinedAt,
        };
    }
}

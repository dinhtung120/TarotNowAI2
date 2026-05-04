# BE Presence

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Presence`
- Hub/runtime: `Backend/src/TarotNow.Api/Hubs/PresenceHub.cs`, `Backend/src/TarotNow.Api/Realtime/PresenceGroupNames.cs`, `InMemoryUserPresenceTracker.cs`, `RedisUserPresenceTracker*.cs`
- Tests: `PresenceHubTests.cs`, `RedisUserPresenceTrackerTests.cs`, `InMemoryUserPresenceTrackerTests.cs`, `RedisRealtimeBridgeSourceQueuePolicyTests.cs`
- Datastore/runtime: Redis or in-memory presence tracker; không thấy DbSet/collection riêng cho Presence trong `ApplicationDbContext.cs`/`MongoDbContext.cs`
- Related controller usage: `ReaderController` uses `IUserPresenceTracker` to overlay reader online status

## Entry points & luồng chính

Presence không có controller riêng trong evidence đã đọc. Runtime boundary chính là SignalR `PresenceHub` với `[Authorize]`.

Hub flows:

- `OnConnectedAsync`: mark connected, join `PresenceGroupNames.User(userId)`, publish `PublishUserStatusChangedCommand` status `online`.
- `OnDisconnectedAsync`: mark disconnected, leave group, publish `offline` khi không còn active connection.
- `Heartbeat`: record heartbeat.
- `SubscribeUserStatusObservers` / `UnsubscribeUserStatusObservers`: join/leave observer groups cho danh sách user ids.

Observer subscription bị giới hạn `MaxObserverSubscriptionsPerRequest = 200`, normalize trim/distinct và truncate.

## Dependency và dữ liệu

State runtime nằm ở `IUserPresenceTracker` implementations:

- In-memory tracker cho local/dev/test.
- Redis tracker cho distributed production/realtime consistency.

Presence publish status change qua MediatR command `PublishUserStatusChangedCommand`, không gửi trực tiếp event migrated từ controller. Reader directory/profile dùng tracker để hiển thị online status.

## Boundary / guard

- Hub phải `[Authorize]`; user id lấy từ claims của connection.
- Không dùng `Context.ConnectionAborted` khi publish status change vì code hiện cố gắng không mất event khi socket đóng nhanh.
- Direct SignalR broadcast migrated events bị architecture tests kiểm soát; PresenceHub cần tránh phát các event bị cấm như `conversation.updated`, `notification.new`.
- Redis required in Production theo `DependencyInjection.Cache.cs`, nên presence distributed behavior không được coi optional ở production.

## Test coverage hiện có

- `PresenceHubTests.cs`: hub behavior.
- `RedisUserPresenceTrackerTests.cs`: Redis tracker behavior.
- `InMemoryUserPresenceTrackerTests.cs`: in-memory tracker behavior.
- `RedisRealtimeBridgeSourceQueuePolicyTests.cs`: Redis realtime bridge queue/source policy liên quan.

## Rủi ro

- P0: unauthenticated presence spoof; stale online/offline state do disconnect/heartbeat bug; direct forbidden realtime broadcast.
- P1: observer subscription không giới hạn gây fanout lớn; Redis/in-memory behavior lệch giữa local và production.
- P2: docs claim database persistence for presence trong khi evidence runtime là tracker/Redis.

## Kết luận

Presence là realtime runtime module, không phải DB feature. Review đúng phải đọc `PresenceHub`, tracker implementation và Redis production behavior, đồng thời kiểm tác động lên Reader online status.

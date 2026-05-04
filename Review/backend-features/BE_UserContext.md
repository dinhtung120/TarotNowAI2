# BE UserContext

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/UserContext`
- Controllers: `Backend/src/TarotNow.Api/Controllers/UserContextController.cs`, `Backend/src/TarotNow.Api/Controllers/MeController.cs`
- Tests: `Backend/tests/TarotNow.Api.IntegrationTests/UserContextHomeIntegrationTests.cs`
- Datastore/runtime: tổng hợp từ nhiều module qua repositories/query abstractions; evidence state gồm `Users`, wallet state, `Notifications`, `Conversations`, `ChatMessages`, `DailyCheckins`, reader/profile/gamification collections tùy snapshot
- Guards: API auth/rate-limit/XML summary tests và Clean Architecture boundary tests

## Entry points & luồng chính

UserContext có hai API boundary liên quan bootstrap user:

- `UserContextController.cs`: `GET /metadata` → `GetInitialMetadataQuery(userId)`.
- `MeController.cs`: `GET navbar-snapshot`, `GET reading-setup-snapshot`, `GET runtime-policies`.

Cả hai controller đều `[Authorize]` và dùng `User.TryGetUserId` để lấy chủ thể hiện tại. Không có endpoint nào nhận user id từ body/query cho snapshot hiện tại.

`UserContextController` dùng `[EnableRateLimiting("auth-session")]`; `MeController` cũng có rate-limit ở controller level.

## Dependency và dữ liệu

Feature listing cho thấy UserContext là query/read-model module, không phải command/mutation module chính.

Các snapshot có thể chạm nhiều bounded context:

- Wallet balance/state.
- Streak/check-in summary.
- Unread notification count và recent notifications.
- Unread chat count và active conversations.
- Reading setup snapshot gồm wallet + cards/catalog.
- Runtime policies từ system configs.

`UserContextHomeIntegrationTests.cs` kiểm response shape của `/api/v1/user-context/metadata` gồm các property:

- `wallet`
- `streak`
- `unreadNotificationCount`
- `unreadChatCount`
- `recentNotifications`
- `activeConversations`

## Boundary / guard

- Đây là aggregation/read API, controller không nên tự query DbContext/repository; orchestration phải nằm trong query handlers/Application services.
- Vì snapshot tổng hợp nhiều module, review phải tránh thêm side effect trong query path.
- Endpoint phải fail-closed khi thiếu user id; controller hiện trả `UnauthorizedProblem` nếu claim không hợp lệ.
- Response shape là contract frontend bootstrap; thay đổi schema phải đọc frontend consume/prefetch tương ứng.

## Test coverage hiện có

- `UserContextHomeIntegrationTests.cs` có test `UserContextMetadata_ShouldReturnExpectedSnapshotShape`, seed test user và verify shape các field chính.
- Cùng file có `HomeSnapshot_ShouldReturnFeaturedReadersPayload`, liên quan BE Home nhưng cũng cho thấy home/user bootstrap integration được gom chung.

Không thấy unit test riêng theo từng query UserContext trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P2/P1 tùy mức thay đổi snapshot.

## Rủi ro

- P1: snapshot query coupling quá nhiều module, dễ tăng latency hoặc fail toàn bộ bootstrap nếu một dependency phụ lỗi.
- P1: response shape thay đổi làm frontend hydration/prefetch mismatch.
- P1: thiếu ownership guard nếu future endpoint cho phép truyền user id.
- P2: docs mô tả UserContext như mutation feature trong khi source hiện là query aggregation.

## Kết luận

UserContext là bootstrap aggregation read model cho client, không phải bounded context mutation độc lập. Review đúng phải đọc query handlers và frontend consumers/prefetch để đảm bảo response shape, latency và fail-closed auth không bị phá.

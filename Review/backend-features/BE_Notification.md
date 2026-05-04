# BE Notification

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Notification`
- Controllers: `NotificationController.cs`, `NotificationController.Queries.cs`, `NotificationController.Commands.cs`
- Datastore: `MongoDbContext.cs` collection `notifications`
- Runtime/realtime: notification side effects liên quan outbox/SignalR/Redis bridge; migrated realtime event `notification.new` bị guard cấm direct hub broadcast
- Tests: không thấy file test có `Notification` trong tên theo search hiện tại

## Entry points & luồng chính

`NotificationController` là authenticated API với `[Authorize]` và rate limit `auth-session`.

Query endpoints:

- `GET notifications`: `GetNotificationsQuery { UserId, Page, PageSize, IsRead }`.
- `GET notifications/unread-count`: `CountUnreadQuery(userId)`.

Command endpoints:

- `PATCH {id}/read`: `MarkNotificationReadCommand { NotificationId, UserId }`, trả 404 khi không thành công.
- `PATCH read-all`: `MarkAllNotificationsReadCommand { UserId }`, trả 204 nếu không có thay đổi.

Controller lấy user id từ token cho mọi endpoint.

## Dependency và dữ liệu

State chính:

- MongoDB `notifications`: list/unread/read state của user.

Notification creation thường là side effect từ domain events khác; API này chủ yếu là read/mark-read. Khi review notification creation, phải đọc domain event handlers/outbox bridge tương ứng, không chỉ `Features/Notification`.

## Boundary / guard

- User chỉ được query/mark read notification của chính mình.
- `MarkAsRead` trả 404 cho not found hoặc không thuộc user, tránh phân biệt ownership.
- Realtime `notification.new` không được broadcast trực tiếp từ controller/hub theo `EventDrivenArchitectureRulesTests` migrated events list.
- Mark-all nên idempotent; controller trả 204 khi no-op.

## Test coverage hiện có

Không thấy direct unit/integration test tên `Notification` trong evidence đã đọc. Nếu audit sâu không tìm thêm, đây là gap P1 cho ownership/read state/realtime event path.

## Rủi ro

- P0: user mark/read notification của user khác; notification realtime bypass outbox/bridge; notification payload chứa dữ liệu nhạy cảm sai audience.
- P1: missing direct tests cho pagination/unread count/mark all no-op.
- P2: docs mô tả notification state ở PostgreSQL trong khi runtime collection đã thấy là MongoDB `notifications`.

## Kết luận

Notification backend là MongoDB read/read-state module cộng với realtime side-effect pipeline. Review đúng phải đọc cả API mark-read/list và domain event handlers tạo notification.

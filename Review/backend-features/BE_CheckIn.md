# BE CheckIn

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/CheckIn`
- Controller: `Backend/src/TarotNow.Api/Controllers/CheckInController.cs`
- Runtime/data: `MongoDbContext.cs` collection `daily_checkins`; domain event `DailyCheckInCompletedDomainEvent.cs`
- Related integration evidence: `UserContextHomeIntegrationTests.cs` có metadata `streak` trong user context snapshot
- Guards: API auth/rate-limit và event-driven architecture tests

## Entry points & luồng chính

`CheckInController.cs` là authenticated API với `[Authorize]` và `[EnableRateLimiting("auth-session")]`.

Endpoints chính:

- `POST /check-in`: `DailyCheckInCommand { UserId }`.
- `GET /check-in/streak`: `GetStreakStatusQuery { UserId }`.
- `POST /check-in/freeze`: `PurchaseStreakFreezeCommand`, controller override `UserId` từ token.

Controller lấy user id bằng `User.TryGetUserId`; client không được quyết định chủ streak/freeze trong payload.

## Dependency và dữ liệu

Feature source có:

- `Commands/DailyCheckIn`
- `Commands/PurchaseFreeze`
- `Queries/GetStreakStatus`

State chính là MongoDB `daily_checkins`. `PurchaseFreeze` là điểm cần đọc sâu vì có thể chạm wallet/reward state; không khẳng định transaction/money event nếu chưa đọc handler cụ thể.

## Boundary / guard

- Check-in/freeze/status đều phải fail-closed khi thiếu user id.
- Daily check-in có thể publish `DailyCheckInCompletedDomainEvent`; side effects như reward/gamification phải ở event handlers/outbox, không ở controller.
- Freeze purchase là flow có khả năng finance, phải review idempotency/transaction/wallet mutation nếu sửa.

## Test coverage hiện có

Không thấy file test có `CheckIn` trong tên theo search hiện tại. `UserContextHomeIntegrationTests.cs` chỉ chứng minh snapshot user context có field `streak`, không chứng minh daily check-in/freeze API.

## Rủi ro

- P0: user giả mạo `UserId` khi mua freeze/check-in; reward streak bị nhận nhiều lần trong cùng ngày; freeze purchase trừ tiền nhiều lần khi retry.
- P1: thiếu test direct cho `DailyCheckIn`, `PurchaseFreeze`, `GetStreakStatus`.
- P2: docs mô tả CheckIn như route frontend độc lập trong khi frontend có thể chỉ tích hợp qua shell/navbar.

## Kết luận

CheckIn là authenticated streak module, state chính ở `daily_checkins`. Review đúng phải đọc command handlers trước khi kết luận reward/freeze/idempotency an toàn.

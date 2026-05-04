# BE Inventory

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Inventory`
- Controller: `Backend/src/TarotNow.Api/Controllers/InventoryController.cs`
- Tests: `UseInventoryItemCommandHandlerTests.cs`, `ItemUsedDomainEventHandlerTests.cs`, `InventoryRewardEventHandlersTests.cs`, `InventoryRealtimeDomainEventHandlersTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `ItemDefinitions`, `UserItems`, `InventoryItemUseOperations`, `FreeDrawCredits`, `InventoryLuckEffects`
- Related Mongo: `cards_catalog`, `user_collections` nếu item tác động collection/card

## Entry points & luồng chính

`InventoryController.cs` là authenticated API với `[Authorize]` và `[EnableRateLimiting("auth-session")]`.

Endpoints chính:

- `GET my inventory`: `GetUserInventoryQuery(userId)`.
- `POST use item`: `UseInventoryItemCommand`.

`UseItem` lấy `UserId` từ token, resolve idempotency key từ header/body, reject request thiếu key bằng `400 Missing idempotency key`, clamp `Quantity` vào `1..10` trước khi dispatch command.

## Dependency và dữ liệu

State PostgreSQL chính:

- `ItemDefinitions`: catalog item.
- `UserItems`: số lượng item user sở hữu.
- `InventoryItemUseOperations`: operation/idempotency record cho use item.
- `FreeDrawCredits`: reward/free pull integration.
- `InventoryLuckEffects`: effect tác động gacha/luck.

Inventory nhận side effects từ Gacha/reward flows và phát domain events/realtime inventory updates.

`UseInventoryItemCommandHandlerTests.cs` chứng minh command publish `ItemUsedDomainEvent`, normalize `ItemCode`/`IdempotencyKey`, và phản ánh idempotent replay trong response.

## Boundary / guard

- Use item là mutation cần idempotency end-to-end.
- Reward grant/use operation phải chống duplicate grant/consume.
- Controller không trực tiếp trừ item hoặc apply effect; xử lý nằm trong command/event handlers.
- Realtime inventory update phải qua event/outbox/bridge; không broadcast trực tiếp từ controller.

## Test coverage hiện có

- `UseInventoryItemCommandHandlerTests.cs`: publish event và replay response.
- `ItemUsedDomainEventHandlerTests.cs`: xử lý domain event item used.
- `InventoryRewardEventHandlersTests.cs`: reward grant handlers.
- `InventoryRealtimeDomainEventHandlersTests.cs`: realtime/outbox side effect handlers.

Không thấy API integration test riêng cho `InventoryController` trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P1 cho auth/idempotency contract.

## Rủi ro

- P0: duplicate use/grant do idempotency fail; quantity clamp bị bypass ở handler khác; item effect apply không atomic với inventory decrement.
- P1: reward source từ gacha/gamification không ghi operation/audit đủ; realtime update chạy trước commit.
- P2: docs gọi inventory là Mongo module dù state chính đã thấy ở PostgreSQL.

## Kết luận

Inventory là reward/entitlement module có state PostgreSQL và side effects realtime/gacha/collection. Review đúng phải đọc command handler, domain event handlers và idempotency operation records.

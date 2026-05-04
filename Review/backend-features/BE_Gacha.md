# BE Gacha

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Gacha`
- Controller: `Backend/src/TarotNow.Api/Controllers/GachaController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/Gacha/Commands/PullGachaCommandHandlerTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `GachaPools`, `GachaPoolRewardRates`, `GachaPullOperations`, `GachaPullRewardLogs`, `UserGachaPities`, `GachaHistoryEntries`, `FreeDrawCredits`, `InventoryLuckEffects`
- Domain events/constants: `Backend/src/TarotNow.Domain/Events/Gacha`, `GachaRoutes.cs`

## Entry points & luồng chính

`GachaController.cs` là authenticated API với `[Authorize]`, `[ApiVersion(ApiVersions.V1)]`, `[EnableRateLimiting("auth-session")]`.

Endpoints chính:

- `GET pools`: list active pools, query nhận optional current `UserId`.
- `GET pool odds`: odds theo `poolCode`.
- `GET history`: lịch sử pull của user hiện tại.
- `POST pull`: thực hiện pull gacha bằng `PullGachaCommand`.

`POST pull` lấy `UserId` từ token và yêu cầu idempotency key từ body hoặc header. Nếu thiếu key, controller trả `400 Missing idempotency key`.

## Dependency và dữ liệu

Gacha là reward/finance-adjacent module vì pull có thể tiêu currency/free draw và grant inventory.

State PostgreSQL chính:

- Pool/rate: `GachaPools`, `GachaPoolRewardRates`.
- Operation/log: `GachaPullOperations`, `GachaPullRewardLogs`, `GachaHistoryEntries`.
- Pity/free/luck: `UserGachaPities`, `FreeDrawCredits`, `InventoryLuckEffects`.
- Inventory integration: item rewards có thể chạm `ItemDefinitions`, `UserItems` qua event handlers/repositories.

`PullGachaCommandHandlerTests.cs` chứng minh command entry handler publish `GachaPulledDomainEvent`, normalize `PoolCode`/`IdempotencyKey`, map reward snapshot, và reject missing idempotency key bằng `GachaErrorCodes.InvalidIdempotencyKey`.

## Boundary / guard

- Pull command phải giữ idempotency từ API tới domain event/operation record.
- Random/reward/pity operation phải transactionally consistent với history/reward grants.
- Controller không tự random/grant reward, chỉ dispatch command/query.
- Realtime result event `gacha.result` thuộc migrated event list trong architecture guard; không broadcast trực tiếp từ hub/controller.

## Test coverage hiện có

- `PullGachaCommandHandlerTests.cs`: publish-only command behavior, normalization, missing idempotency key.

Không thấy API integration test riêng cho `GachaController` trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P1 cho auth/idempotency/header/body contract và history/pool endpoints.

## Rủi ro

- P0: duplicate pull/reward do idempotency fail; pity counter update không atomic; inventory reward grant trùng; direct realtime broadcast bypass outbox.
- P1: odds/pool response không khớp actual reward rates; missing integration test cho pull endpoint.
- P2: docs nói gacha state ở MongoDB trong khi runtime DbSets đã thấy nằm ở PostgreSQL.

## Kết luận

Gacha là module reward-critical với PostgreSQL operation log/pity/history và event-driven reward grant. Review đúng phải đọc controller, pull command/event handler, transaction/idempotency path và inventory side effects.

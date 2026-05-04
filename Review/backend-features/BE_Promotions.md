# BE Promotions

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Promotions`
- Controller: `Backend/src/TarotNow.Api/Controllers/PromotionsController.cs`
- Test: `Backend/tests/TarotNow.Api.IntegrationTests/PromotionIntegrationTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `DepositPromotions`; related Deposit state `DepositOrders`, `WalletTransactions`
- Boundary: admin route under `ApiRoutes.Admin + "/promotions"`

## Entry points & luồng chính

`PromotionsController.cs` là admin-only API với `[Authorize(Roles = "admin")]` và rate limit `auth-session`.

Endpoints chính:

- `GET /admin/promotions`: `ListPromotionsQuery { OnlyActive }`.
- `POST /admin/promotions`: `CreatePromotionCommand`.
- `PUT /admin/promotions/{id}`: maps `UpdatePromotionRequest` + route id to `UpdatePromotionCommand`.
- `DELETE /admin/promotions/{id}`: `DeletePromotionCommand { Id }`.

Controller chỉ dispatch MediatR, không inject DbContext/repository trực tiếp.

## Dependency và dữ liệu

Promotion state chính là PostgreSQL `DepositPromotions`, liên quan deposit order calculation/bonus.

Fields đọc từ update request gồm:

- `MinAmountVnd`
- `BonusGold`
- `IsActive`

Promotion không trực tiếp mutate wallet trong controller, nhưng ảnh hưởng finance flow Deposit khi bonus được áp dụng.

## Boundary / guard

- Admin RBAC phải giữ fail-closed.
- Promotion create/update/delete phải không làm lệch deposit bonus calculation đang active.
- Khi đổi promotion schema/rule, phải review `BE_Deposit.md` vì deposit order creation/settlement phụ thuộc promotion.
- Không coi Promotions là user-facing public API nếu chỉ thấy admin controller.

## Test coverage hiện có

- `PromotionIntegrationTests.cs`: API integration evidence cho promotion flow.

Cần đọc thêm handler tests nếu audit sâu; evidence hiện tại chủ yếu là integration test và controller.

## Rủi ro

- P0: non-admin mutate promotion; promotion thay đổi làm deposit bonus sai hoặc double-credit wallet.
- P1: active/inactive rule không đồng bộ với deposit order creation; thiếu audit cho admin mutation.
- P2: docs gọi Promotions là reward/gacha module trong khi source evidence là deposit promotion.

## Kết luận

Promotions backend là admin-managed deposit promotion module. Review đúng phải đọc cùng Deposit order creation/bonus logic, không review tách rời khỏi finance deposit flow.

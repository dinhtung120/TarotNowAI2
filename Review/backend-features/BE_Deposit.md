# BE Deposit

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Deposit`
- Controllers: `Backend/src/TarotNow.Api/Controllers/DepositController.cs`, `DepositController.Orders.cs`, `DepositController.Webhook.cs`, `AdminDepositsController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/Deposit/DepositCommandHandlersPublishOnlyTests.cs`, `DepositOrderCreateRequestedDomainEventHandlerTests.cs`, `Backend/tests/TarotNow.Api.IntegrationTests/DepositOrderIntegrationTests.cs`, `DepositWebhookIntegrationTests.cs`, `Backend/tests/TarotNow.Domain.UnitTests/Events/DepositWebhookReceivedDomainEventTests.cs`, `Backend/tests/TarotNow.Infrastructure.UnitTests/Migrations/AddDepositOrderCodeSequenceMigrationTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `DepositOrders`, `DepositPromotions`, `WalletTransactions`

## Entry points & luồng chính

Deposit có API entry points tách theo order và webhook:

- `DepositController.Orders.cs`: user-facing deposit order operations.
- `DepositController.Webhook.cs`: provider/webhook callback boundary.
- `AdminDepositsController.cs`: admin review/operation path.

Application feature có `Commands` và `Queries`. Test `DepositCommandHandlersPublishOnlyTests.cs` là evidence quan trọng: command handlers trong Deposit được kỳ vọng chỉ publish/dispatch theo event-driven model, không orchestration trực tiếp trong entry handler.

`DepositOrderCreateRequestedDomainEventHandlerTests.cs` là evidence cho orchestration thực tế khi tạo deposit order nằm ở requested domain event handler.

## Dependency và dữ liệu

PostgreSQL state chính:

- `DepositOrders`: order nạp tiền.
- `DepositPromotions`: promotion áp dụng cho deposit.
- `WalletTransactions`: ledger khi deposit thành công/settle.

`ApplicationDbContext` có sequence `deposit_order_code_seq` được cấu hình trong `ConfigureDatabaseSequences`; migration test `AddDepositOrderCodeSequenceMigrationTests.cs` cho thấy order code sequence là phần cần giữ ổn định khi sửa deposit schema.

Webhook event được test bởi `DepositWebhookReceivedDomainEventTests.cs`; khi review webhook phải kiểm idempotency/replay protection và mapping provider status -> wallet mutation.

## Boundary / guard

- Deposit write command handlers phải qua `IInlineDomainEventDispatcher` theo `EventDrivenArchitectureRulesTests.cs`.
- Wallet mutation từ deposit phải publish `MoneyChangedDomainEvent` trong command module liên quan.
- Webhook controller là external boundary: phải review signature/authentication/idempotency, không được cộng tiền trực tiếp ngoài Application/domain event flow.

## Test coverage hiện có

- Unit: `DepositCommandHandlersPublishOnlyTests.cs`, `DepositOrderCreateRequestedDomainEventHandlerTests.cs`.
- API integration: `DepositOrderIntegrationTests.cs`, `DepositWebhookIntegrationTests.cs`.
- Domain event: `DepositWebhookReceivedDomainEventTests.cs`.
- Migration: `AddDepositOrderCodeSequenceMigrationTests.cs`.

## Rủi ro

- P0: webhook replay cộng tiền nhiều lần; deposit order settlement thiếu idempotency; wallet ledger không phát money event; controller xử lý provider side effect trực tiếp.
- P1: promotion calculation không được test cùng order creation; admin deposit action thiếu audit/rate-limit evidence.
- P2: docs không nhắc `deposit_order_code_seq` khi thay đổi schema/order code.

## Kết luận

Deposit có evidence test tốt hơn nhiều module khác, đặc biệt quanh publish-only command handlers, requested event handler, webhook integration và migration sequence. Đây vẫn là finance-critical module, mọi thay đổi phải đi kèm idempotency + wallet ledger + webhook tests.

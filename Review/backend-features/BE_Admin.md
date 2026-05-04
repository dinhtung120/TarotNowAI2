# BE Admin

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Admin`
- Controllers: `AdminUsersController.cs`, `AdminDepositsController.cs`, `AdminWithdrawalsController.cs`, `AdminDisputesController.cs`, `AdminReaderRequestsController.cs`, `AdminCommunityController.cs`, `AdminGamificationController.cs`, `AdminOutboxController.cs`, `AdminReconciliationController.cs`, `AdminSystemConfigsController.cs`
- Tests: `Backend/tests/TarotNow.Api.IntegrationTests/AdminRbacIntegrationTests.cs`, `Backend/tests/TarotNow.Application.UnitTests/Admin/GetLedgerMismatchQueryHandlerTests.cs`, `Backend/tests/TarotNow.Application.UnitTests/Features/Admin/ApproveReaderCommandHandlerTests.cs`, `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Reconciliation/AdminRepositoryIntegrationTests.cs`, `Backend/tests/TarotNow.Infrastructure.UnitTests/Configuration/SystemConfigAdminServiceTests.cs`
- Datastore/runtime: spans `Users`, `WalletTransactions`, `DepositOrders`, `WithdrawalRequests`, `SystemConfigs`, outbox tables and Mongo read models for reader/community/gamification depending endpoint

## Entry points & luồng chính

Admin API là một boundary rộng, tất cả controller đã đọc/map đều dùng `[Authorize(Roles = "admin")]`, `[ApiVersion(ApiVersions.V1)]` và rate limit `auth-session`.

Các nhóm endpoint chính:

- User operations qua `AdminUsersController.cs`: list/create/lock/update/add-balance.
- Finance operations qua `AdminDepositsController.cs`, `AdminWithdrawalsController.cs`, `AdminReconciliationController.cs`.
- Dispute/reader/community/gamification operations qua các controller admin tương ứng.
- Ops/system qua `AdminOutboxController.cs` và `AdminSystemConfigsController.cs`.

`AdminUsersController.UpdateUser` và `AddUserBalance` có idempotency key handling ở API boundary. `UpdateUser` yêu cầu key từ body/header và trả `400 Missing idempotency key` nếu thiếu; `AddUserBalance` đọc key từ header khi body chưa có.

`AdminOutboxController.GetDashboard` dispatch `GetOutboxDashboardQuery`; đây là ops read path cho pending/failed/dead-letter/retry age.

`AdminReconciliationController.GetWalletMismatches` dispatch `GetLedgerMismatchQuery`; đây là finance reconciliation read path giữa ledger và wallet aggregate.

## Dependency và dữ liệu

Feature `Admin` có commands/queries quan trọng:

- Commands: `AddUserBalance`, `ApproveReader`, `CreateUser`, `ResolveDispute`, `ToggleUserLock`, `UpdateUser`.
- Queries: `ListUsers`, `ListDeposits`, `ListReaderRequests`, `ListDisputes`, `GetLedgerMismatch`, `GetOutboxDashboard`.

Datastore touched tùy endpoint:

- PostgreSQL `Users`, `WalletTransactions`, `DepositOrders`, `WithdrawalRequests`, `SystemConfigs`.
- PostgreSQL outbox tables: `OutboxMessages`, `OutboxHandlerStates`, `OutboxInlineHandlerStates`.
- MongoDB reader/community/gamification/report collections khi controller admin thao tác read-model hoặc moderation.

## Boundary / guard

- Admin route phải RBAC fail-closed theo role `admin`.
- Controller không được inject DbContext/repository trực tiếp; evidence đã đọc cho admin users/outbox/reconciliation chỉ inject `IMediator`.
- Finance/admin mutation như add balance, dispute settlement, withdrawal processing phải review transaction/idempotency/money event.
- System config/admin ops không được leak secret hoặc expose write endpoints thiếu rate-limit/RBAC.

## Test coverage hiện có

- `AdminRbacIntegrationTests.cs`: user role `User` gọi `/api/v1/admin/users` bị `403`; role `admin` gọi thành công `200`.
- `GetLedgerMismatchQueryHandlerTests.cs`: coverage reconciliation query.
- `ApproveReaderCommandHandlerTests.cs`: coverage admin reader approval command.
- `AdminRepositoryIntegrationTests.cs`: infrastructure integration evidence cho reconciliation/admin repository.
- `SystemConfigAdminServiceTests.cs`: coverage admin system config service.

Các controller admin còn lại có thể có coverage trong feature module tương ứng; khi review PR phải đọc test của bounded context liên quan, không chỉ `Features/Admin`.

## Rủi ro

- P0: RBAC admin fail-open; admin finance mutation thiếu idempotency/transaction; add balance không publish `MoneyChangedDomainEvent`; dispute settlement double payout/refund.
- P1: admin endpoint bypass Application bằng direct repository/db; system config write expose secret hoặc thiếu audit.
- P1: reconciliation/outbox dashboard thiếu integration test cho production-like state.
- P2: docs gom mọi admin action vào `Features/Admin` trong khi nhiều admin controllers thuộc feature khác.

## Kết luận

Admin là operational boundary phủ nhiều bounded context, không phải một module nghiệp vụ đơn lẻ. Review đúng phải bắt đầu từ controller admin cụ thể, sau đó đọc command/query của feature đích và test RBAC/idempotency/finance/outbox tương ứng.

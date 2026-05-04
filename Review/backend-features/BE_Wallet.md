# BE Wallet

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Wallet`
- Controller: `Backend/src/TarotNow.Api/Controllers/WalletController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Wallet/GetWalletBalanceQueryHandlerTests.cs`, `GetLedgerListQueryHandlerTests.cs`, `Backend/tests/TarotNow.Domain.UnitTests/Entities/UserWalletTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `WalletTransactions`; user wallet balances nằm trên `User`/`UserWallet` domain model và được đọc qua `IUserRepository`
- Guard: `EventDrivenArchitectureRulesTests.WalletMutationCommands_ShouldPublishMoneyChangedDomainEvent`

## Entry points & luồng chính

`WalletController.cs` là API boundary user-facing, có `[Authorize]`, `[ApiVersion(ApiVersions.V1)]` và `[EnableRateLimiting("auth-session")]`.

Controller hiện expose hai query endpoint:

- `GET balance` → `GetWalletBalanceQuery(userId)`.
- `GET ledger` → `GetLedgerListQuery(userId, page, limit)`.

Controller lấy `userId` từ authenticated principal bằng `User.TryGetUserId`; payload/query không được quyết định chủ ví. Đây là evidence ownership quan trọng cho review API ví.

Feature `Wallet` hiện chỉ có `Queries`, không thấy command folder trong listing đã đọc. Vì vậy module `BE_Wallet.md` phải phân biệt rõ:

- Wallet API trực tiếp hiện là read path.
- Wallet mutation nằm rải rác ở các finance/AI/escrow/deposit/withdrawal flows khác qua Application interfaces/domain model, không nằm trong `Features/Wallet/Commands` tại thời điểm đọc.

## Dependency và dữ liệu

Read path:

- `GetWalletBalanceQueryHandler.cs` inject `IUserRepository`, load user theo `UserId`, trả `GoldBalance`, `DiamondBalance`, `FrozenDiamondBalance`.
- `GetLedgerListQueryHandler.cs` inject `ILedgerRepository`, đọc `GetTotalCountAsync` và `GetTransactionsAsync`, map `WalletTransaction` sang `WalletTransactionDto`.

PostgreSQL state liên quan:

- `WalletTransactions`: ledger giao dịch ví.
- `Users`: chứa aggregate/user wallet balances được repository trả về cho balance query.

Domain invariant evidence từ `UserWalletTests.cs`:

- `Credit_DiamondDeposit_ShouldIncreaseBalanceAndPurchasedTotal`.
- `Debit_WhenInsufficientBalance_ShouldThrow`.
- `FreezeAndRefund_ShouldRestoreDiamondBalance`.
- `FreezeAndRelease_ShouldConsumeFrozenBalance`.
- `ConsumeFrozenDiamond_ShouldDecreaseFrozenOnly`.
- `Credit_WithInvalidCurrency_ShouldThrowArgumentException`.

Các test này cho thấy wallet domain model có rule chống số dư âm, freeze/refund/release diamond và whitelist currency.

## Boundary / guard

- `WalletController.cs` chỉ dispatch MediatR query, không inject repository/db context trực tiếp.
- Query handlers được phép đọc repository interfaces trong Application layer.
- Mutation wallet ở các module khác phải được review theo finance rule: transaction/idempotency và event/outbox.
- `EventDrivenArchitectureRulesTests.WalletMutationCommands_ShouldPublishMoneyChangedDomainEvent` là guard quan trọng: command module có wallet mutation phải publish canonical `MoneyChangedDomainEvent`.

## Test coverage hiện có

- `GetWalletBalanceQueryHandlerTests.cs`: kiểm mapping `GoldBalance`, `DiamondBalance`, `FrozenDiamondBalance` sau credit/freeze.
- `GetLedgerListQueryHandlerTests.cs`: kiểm pagination metadata và mapping ledger transaction DTO.
- `UserWalletTests.cs`: kiểm domain invariant cho credit/debit/freeze/refund/release/consume.

Không thấy API integration test riêng cho `WalletController` trong evidence đã đọc; nếu audit sâu không tìm thêm test, đây là gap P1 cho ownership/rate-limit/response contract của balance và ledger endpoints.

## Rủi ro

- P0: finance flow khác mutate wallet nhưng không publish `MoneyChangedDomainEvent`, thiếu idempotency hoặc làm double-spend/double-refund.
- P1: WalletController thiếu integration test chứng minh user chỉ đọc được ví của chính mình và ledger pagination contract ổn định.
- P1: ledger/balance divergence nếu mutation update balance nhưng không ghi `WalletTransactions` cùng transaction.
- P2: docs gọi Wallet là command module độc lập trong khi source hiện chỉ có query feature.

## Kết luận

Wallet backend hiện là read API trực tiếp cộng với domain/persistence state được nhiều finance flows dùng để mutate. Review đúng phải đọc `WalletController`, query handlers, `UserWallet` domain tests và mọi module đang gọi wallet mutation, đặc biệt Deposit, Withdrawal, Escrow, Chat finance và Reading AI billing.

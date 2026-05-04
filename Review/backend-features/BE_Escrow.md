# BE Escrow

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Escrow`
- Chat finance controllers: `Backend/src/TarotNow.Api/Controllers/ConversationController.Finance.cs`, `ConversationController.Completion.cs`, `ConversationController.Acceptance.cs`
- Related Chat commands: `Backend/src/TarotNow.Application/Features/Chat/Commands/*Conversation*Money*`, `*Complete*`, `SendMessage/*FirstMessageFreeze*`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/*`, `Backend/tests/TarotNow.Application.UnitTests/Services/EscrowSettlementServiceSessionTests.cs`, `Backend/tests/TarotNow.Application.UnitTests/DomainEvents/EscrowSessionReleasedDomainEventHandlersTests.cs`, `Backend/tests/TarotNow.Infrastructure.UnitTests/BackgroundJobs/EscrowTimerServiceTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `ChatFinanceSessions`, `ChatQuestionItems`, `WalletTransactions`

## Entry points & luồng chính

Feature Escrow hiện có commands riêng đã thấy từ test/source listing:

- `AddQuestion` — được test bởi `AddQuestionCommandHandlerTests.cs`.
- `OpenDispute` — được test bởi `OpenDisputeCommandHandlerTests.cs`.

Tuy nhiên phần lớn escrow lifecycle gắn chặt với Chat commands:

- first message freeze trong `Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze*.cs`;
- add money request/response trong `RequestConversationAddMoney` và `RespondConversationAddMoney`;
- settlement khi complete trong `RequestConversationComplete` và `RespondConversationComplete`;
- dispute/completion/acceptance qua `ConversationController` partials.

Vì vậy `BE_Escrow.md` không nên được review như module tách rời hoàn toàn; nó là finance subdomain của conversation flow.

## Dependency và dữ liệu

PostgreSQL state chính:

- `ChatFinanceSessions`: session escrow/tài chính theo conversation.
- `ChatQuestionItems`: question items trong flow chat/escrow.
- `WalletTransactions`: ledger tiền liên quan freeze/settlement/refund.

Outbox/domain event liên quan:

- `EscrowSessionReleasedDomainEventHandlersTests.cs` cho thấy có handler xử lý khi escrow session released.
- `EscrowTimerServiceTests.cs` cho thấy infrastructure background job/timer là một phần của lifecycle cần review khi thay đổi timeout/auto-release.

## Boundary / guard

- Escrow commands nếu là write commands phải tuân thủ thin handler rule trong `EventDrivenArchitectureRulesTests.cs`.
- Wallet mutation liên quan escrow phải publish canonical `MoneyChangedDomainEvent` theo architecture rule.
- Controller không được settlement/refund trực tiếp; route chỉ dispatch command/query qua Application.

## Test coverage hiện có

- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/AddQuestionCommandHandlerTests.cs`
- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/OpenDisputeCommandHandlerTests.cs`
- `Backend/tests/TarotNow.Application.UnitTests/Services/EscrowSettlementServiceSessionTests.cs`
- `Backend/tests/TarotNow.Application.UnitTests/DomainEvents/EscrowSessionReleasedDomainEventHandlersTests.cs`
- `Backend/tests/TarotNow.Infrastructure.UnitTests/BackgroundJobs/EscrowTimerServiceTests.cs`
- Chat settlement tests liên quan: `ConversationCompleteSessionSettlementTests.cs`

## Rủi ro

- P0: double settlement/refund, missing idempotency key, wallet ledger không khớp escrow session, timer release chạy cạnh tranh với manual settlement.
- P1: logic escrow phân tán giữa `Escrow` và `Chat` khiến review bỏ sót first-freeze/add-money/complete path.
- P2: docs ghi Escrow như feature độc lập mà không link Chat finance controllers/commands.

## Kết luận

Escrow là vùng finance-critical nằm giữa `Features/Escrow` và `Features/Chat`. Review đúng phải đọc cả Escrow commands/tests lẫn Chat finance/settlement paths, cộng thêm DbSet PostgreSQL và background timer tests.

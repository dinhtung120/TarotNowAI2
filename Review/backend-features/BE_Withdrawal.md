# BE Withdrawal

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Withdrawal`
- Controllers: `Backend/src/TarotNow.Api/Controllers/WithdrawalController.cs`, `AdminWithdrawalsController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/Withdrawal/CreateWithdrawalCommandHandlerTests.cs`, `ProcessWithdrawalCommandHandlerTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `WithdrawalRequests`, `WalletTransactions`; sensitive field encryption for `WithdrawalRequest.BankAccountName` and `BankAccountNumber`
- Guards: `ApiAndConfigurationStandardsTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `ArchitectureBoundariesTests.cs`

## Entry points & luồng chính

Withdrawal có hai boundary chính:

- User-facing request path qua `WithdrawalController.cs`.
- Admin processing path qua `AdminWithdrawalsController.cs`.

Application tests cho thấy hai command path quan trọng:

- `CreateWithdrawalCommandHandlerTests.cs`: user tạo withdrawal request.
- `ProcessWithdrawalCommandHandlerTests.cs`: admin/process withdrawal request.

Vì đây là flow rút tiền, review phải đọc command handlers và requested-domain-event handlers liên quan để xác định transaction boundary, wallet ledger mutation và admin audit.

## Dependency và dữ liệu

PostgreSQL state chính:

- `WithdrawalRequests`: yêu cầu rút tiền.
- `WalletTransactions`: ledger tiền.

`ApplicationDbContext.ApplySensitiveFieldEncryption` mã hóa/giải mã `WithdrawalRequest.BankAccountName` và `BankAccountNumber` bằng `ISensitiveDataProtector`. Đây là evidence security quan trọng: docs/PR không được log hoặc expose các fields này.

Nếu có payout profile tables trong schema, cần đối chiếu repository/entity cụ thể trước khi ghi vào module docs; trong `ApplicationDbContext` phần đã đọc không thấy DbSet `ReaderPayoutProfiles`, nên không khẳng định runtime EF đang map DbSet đó nếu chưa đọc thêm entity/config.

## Boundary / guard

- Write command handler phải mỏng theo `EventDrivenArchitectureRulesTests.cs`.
- Admin endpoint phải có API version/rate-limit/XML summary theo `ApiAndConfigurationStandardsTests.cs`.
- API không được inject repository/db context trực tiếp theo `ArchitectureBoundariesTests.cs`.
- Wallet mutation trong withdrawal phải được review cùng `MoneyChangedDomainEvent` rule.

## Test coverage hiện có

- `CreateWithdrawalCommandHandlerTests.cs`
- `ProcessWithdrawalCommandHandlerTests.cs`

Không tìm thấy từ listing hiện tại integration test riêng cho withdrawal API; nếu audit chi tiết không thấy thêm test, đây là gap P1 cho flow finance/admin.

## Rủi ro

- P0: double withdrawal/process; thiếu transaction/idempotency; sensitive bank fields log/expose plaintext; admin process bypass Application.
- P1: thiếu API integration test cho user/admin withdrawal; audit trail/admin action chưa được chứng minh trong file tests đã thấy.
- P2: docs claim payout profile table/runtime mapping mà chưa đọc DbSet/config cụ thể.

## Kết luận

Withdrawal là finance + sensitive-data module. Evidence đã đọc xác nhận có unit tests cho create/process và encryption hook trong `ApplicationDbContext`; cần bổ sung/đối chiếu integration và audit evidence khi review thay đổi thực tế.

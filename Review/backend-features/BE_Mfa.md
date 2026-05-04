# BE MFA

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Mfa`
- Controller: `Backend/src/TarotNow.Api/Controllers/MfaController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/Mfa/MfaSetupCommandHandlerTests.cs`, `MfaVerifyCommandHandlerTests.cs`, `MfaChallengeCommandHandlerTests.cs`
- Datastore: không thấy DbSet/collection riêng tên MFA trong `ApplicationDbContext.cs`/`MongoDbContext.cs` theo evidence đã đọc; state có khả năng gắn với user/auth aggregate, cần đọc repository/entity khi audit sâu
- Guards: `ApiAndConfigurationStandardsTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `ArchitectureBoundariesTests.cs`

## Entry points & luồng chính

`MfaController.cs` là authenticated API boundary với `[Authorize]`, `[ApiVersion(ApiVersions.V1)]`.

Endpoints chính:

- `POST setup` → `MfaSetupCommand { UserId }`.
- `POST verify` → `MfaVerifyCommand { UserId, Code }`.
- `POST challenge` → `MfaChallengeCommand { UserId, Code }`.
- `GET status` → `GetMfaStatusQuery { UserId }`.

Controller lấy user id từ authenticated principal bằng `User.GetUserIdOrNull()`. User id không lấy từ body, nên review ownership phải giữ invariant này.

Rate limit evidence:

- setup/status dùng `auth-session`.
- verify/challenge dùng `auth-mfa-challenge`.

## Dependency và dữ liệu

Feature source có commands và query:

- `Commands/MfaSetup`
- `Commands/MfaVerify`
- `Commands/MfaChallenge`
- `Queries/GetMfaStatus`

Không có evidence DbSet/collection riêng `Mfa*` từ datastore files đã đọc. Vì vậy docs không nên khẳng định table riêng cho MFA. Khi review implementation sâu, phải đọc command handlers/repository để xác định secret, enabled flag và challenge state nằm ở đâu.

## Boundary / guard

- MFA endpoints đều là sensitive authenticated endpoints và phải giữ rate-limit metadata.
- Controller chỉ dispatch command/query qua MediatR, không inject db/repository trực tiếp.
- Command entry handlers trong `Features/Mfa/Commands` nếu là `IRequestHandler<,>` phải tuân thủ `IInlineDomainEventDispatcher` rule theo architecture tests.
- MFA code/secret là sensitive data: không log plaintext secret/code, không expose secret sau setup ngoài contract cần thiết.

## Test coverage hiện có

- `MfaSetupCommandHandlerTests.cs`: coverage setup flow.
- `MfaVerifyCommandHandlerTests.cs`: coverage verify/bật MFA.
- `MfaChallengeCommandHandlerTests.cs`: coverage challenge cho thao tác nhạy cảm.

Không thấy API integration test riêng cho `MfaController` trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P1 cho rate-limit/auth/response contract.

## Rủi ro

- P0: challenge/verify fail-open; MFA code/secret bị log hoặc trả về quá rộng; endpoint thiếu auth/rate-limit.
- P1: thiếu integration test controller; trạng thái MFA nằm chung trong user/auth aggregate nên dễ bỏ sót migration/encryption/audit khi sửa.
- P2: docs nói có MFA table riêng khi source không chứng minh.

## Kết luận

MFA là security-sensitive submodule của Auth, hiện có controller và unit tests command-level nhưng chưa có evidence datastore riêng. Review đúng phải đọc command handlers và user/auth persistence trước khi khẳng định nơi lưu secret/enabled state.

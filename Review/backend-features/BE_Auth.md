# BE Auth

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Auth`
- Controllers: `Backend/src/TarotNow.Api/Controllers/AuthRegistrationController.cs`, `AuthSessionController.cs`, `AuthPasswordController.cs`, `MeController.cs`
- Constants/services liên quan: `Backend/src/TarotNow.Api/Constants/AuthHeaders.cs`, `AuthCookieNames.cs`, `Backend/src/TarotNow.Api/Services/IAuthService`, `IAuthCookieService`
- Tests: `Backend/tests/TarotNow.Api.IntegrationTests/AuthRegistrationIntegrationTests.cs`, `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/*CommandHandlerTests.cs`, `Backend/tests/TarotNow.Infrastructure.UnitTests/Auth/JwtTokenValidationTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `Users`, `RefreshTokens`, `AuthSessions`, `EmailOtps`; `MongoDbContext.cs` collection `refresh_tokens`

## Entry points & luồng chính

Auth API đang tách thành controller theo sub-flow:

- `AuthRegistrationController.cs`: `POST register`, `POST send-verification-email`, `POST verify-email`.
- `AuthSessionController.cs`: `POST login`, `POST refresh`, `POST logout`.
- `AuthPasswordController.cs`: `POST forgot-password`, `POST reset-password`.
- `MeController.cs`: authenticated snapshots/policies cho user hiện tại, như `navbar-snapshot`, `reading-setup-snapshot`, `runtime-policies`.

`AuthSessionController` không dispatch MediatR trực tiếp cho login/refresh/logout mà gọi `IAuthService`, rồi set/clear cookies qua `IAuthCookieService`. Đây là API-layer service boundary cần review cùng cookie security.

Refresh flow đọc refresh token từ `AuthCookieNames.RefreshToken`, dùng idempotency header từ `AuthHeaders.IdempotencyKey` hoặc legacy header, và clear auth cookies khi token thiếu/refresh fail.

Registration flow tạo user bằng `RegisterCommand`, sau đó best-effort gửi `SendEmailVerificationOtpCommand`; SMTP/provider lỗi không làm fail `register`, được test trong `AuthRegistrationIntegrationTests.cs`.

## Dependency và dữ liệu

Application commands đã thấy từ feature listing:

- Register/login/session: `Register`, `Login`, `RefreshToken`, `Logout`, `RevokeRefreshToken`, `RevokeToken`.
- Email/password: `SendEmailVerificationOtp`, `VerifyEmail`, `ForgotPassword`, `ResetPassword`.

PostgreSQL state chính:

- `Users`: tài khoản, trạng thái, password hash, auth flags.
- `RefreshTokens`: refresh token rotation/revocation state.
- `AuthSessions`: phiên đăng nhập.
- `EmailOtps`: OTP verify email/reset password.

MongoDB có collection `refresh_tokens`; khi review auth persistence phải đối chiếu repository thực tế để biết flow nào dùng PostgreSQL hay Mongo, không suy đoán chỉ từ tên collection.

## Boundary / guard

- Sensitive endpoints có rate limit riêng: `auth-register`, `auth-login`, `auth-refresh-token-family`, `auth-logout`, `auth-password`.
- `ApiPipeline_ShouldAuthenticateBeforeRateLimiting` trong `ApiAndConfigurationStandardsTests.cs` bảo vệ thứ tự middleware auth/rate-limit.
- Auth command handlers vẫn thuộc event-driven architecture rule nếu là `IRequestHandler<,>` command entry handler trong Application.
- Password reset/forgot-password trả message trung tính để giảm account enumeration, evidence ở `AuthPasswordController.cs`.
- Cookie/token behavior nằm ở `IAuthCookieService`/auth infrastructure; review security phải đọc service implementation trước khi kết luận HttpOnly/Secure/SameSite.

## Test coverage hiện có

- `AuthRegistrationIntegrationTests.cs`: register tạo user `Pending`, persist `EmailOtp` type `VerifyEmail`, và vẫn trả `Created` khi email sender fail.
- Unit tests trong `Features/Auth/Commands`: register/login/refresh/revoke/forgot/reset/send OTP/verify email handlers.
- `JwtTokenValidationTests.cs`: infrastructure token validation evidence.
- `RefreshTokenRepositoryIntegrationTests.cs` và `Argon2idPasswordHasherTests.cs` được map trong test tree, cần đọc khi audit refresh persistence/password hashing chi tiết.

## Rủi ro

- P0: refresh token rotation fail-open; cookies không HttpOnly/Secure/SameSite đúng policy; reset/forgot-password leak email existence; login throttle bị bypass.
- P1: AuthSessionController dùng API service boundary nên review phải đọc cả service implementation, không chỉ controller/command handler.
- P1: PostgreSQL/Mongo refresh token dual evidence có thể gây nhầm persistence source nếu docs/PR không đối chiếu repository hiện hành.
- P2: hardcoded user-facing auth message trong controller cần được cân nhắc nếu API contract/i18n policy thay đổi, nhưng không tự sửa ngoài scope.

## Kết luận

Auth là module security-critical gồm registration, email OTP, session cookie/token, refresh rotation và password reset. Review đúng phải đọc controller sub-flow, Application command tests, auth service/cookie service implementation và persistence repository trước khi kết luận thay đổi an toàn.

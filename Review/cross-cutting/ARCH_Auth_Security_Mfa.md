# Auth, Security, MFA

## 1. Source đã đọc thủ công

- `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`
- `Backend/src/TarotNow.Infrastructure/Persistence/ApplicationDbContext.cs`
- `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- `Frontend/scripts/check-auth-fail-closed.mjs` cần đọc chi tiết ở batch frontend/security tiếp theo.
- Feature source cần đọc khi rewrite feature docs:
  - `Backend/src/TarotNow.Application/Features/Auth`
  - `Backend/src/TarotNow.Application/Features/Mfa`
  - `Backend/src/TarotNow.Application/Features/UserContext`
  - `Backend/src/TarotNow.Application/Features/Legal`
  - `Backend/src/TarotNow.Application/Features/Admin`

## 2. Backend security guard từ architecture tests

`ApiAndConfigurationStandardsTests.cs` enforce nhiều rule bảo vệ API/security posture:

- Mọi API controller cần version metadata (`ApiControllers_ShouldDeclareApiVersionMetadata`).
- Không hardcode `/api/v1` trong route attributes (`ApiLayer_ShouldNotHardcodeV1RouteLiteralsInAttributes`).
- Infrastructure services không dùng `IConfiguration` trực tiếp ngoài allowlist DI/composition files (`InfrastructureServices_ShouldUseOptionsPatternInsteadOfIConfiguration`).
- Không khởi tạo `SmtpClient` trực tiếp ngoài composition root (`Infrastructure_ShouldNotInstantiateSmtpClientDirectlyOutsideCompositionRoot`).
- Feature flag keys ở API phải dùng constants, không dùng raw string trong `IsEnabledAsync("...")` (`ApiLayer_ShouldUseFeatureFlagConstants`).
- HTTP actions phải có XML summary (`ApiHttpActions_ShouldHaveXmlSummaryComments`).
- Middleware pipeline phải gọi `UseAuthentication()` trước `UseRateLimiter()` (`ApiPipeline_ShouldAuthenticateBeforeRateLimiting`).
- Authorized HTTP actions cần rate limiting metadata (`AuthorizedHttpActions_ShouldDeclareRateLimitingMetadata`).

## 3. Auth/session data state từ runtime code

`ApplicationDbContext.cs` khai báo các DbSet liên quan identity/security:

- `Users`
- `RefreshTokens`
- `AuthSessions`
- `EmailOtps`
- `UserConsents`

`MongoDbContext.cs` cũng khai báo collection `refresh_tokens`. Khi rewrite Auth/MFA/UserContext docs phải làm rõ repository/runtime flow thực tế dùng PostgreSQL `RefreshTokens`, Mongo `refresh_tokens`, hoặc cả hai; không được suy đoán chỉ từ tên collection.

`MongoDbContext.cs` có `Notifications`, `UploadSessions`, `CommunityMediaAssets`; các feature auth/profile/community nếu dùng upload/session token phải review ownership và one-time token path từ code cụ thể.

## 4. Redis/security runtime

`DependencyInjection.Cache.cs` yêu cầu Redis trong Production. Redis absence ở non-production fallback memory cache, nhưng production fail-fast. Điều này ảnh hưởng auth/rate-limit/session/realtime consistency review: không được mô tả Redis là optional trong Production.

## 5. Frontend auth fail-closed

Guard `Frontend/scripts/check-auth-fail-closed.mjs` là source cần đọc trước khi kết luận chi tiết frontend auth. Ở mức cross-cutting, rule review là: frontend route/API wrapper không được fail-open khi session/auth state chưa xác định, đặc biệt với `(user)`, admin, wallet, profile, chat, notification và payment/reward flows.

## 6. Rủi ro

- P0: authorized endpoint thiếu rate limiting metadata; auth middleware đứng sau rate limiter; frontend protected route fail-open; token/refresh/session state không có ownership/replay protection evidence; secret/env bị log hoặc hardcode.
- P1: Auth/MFA/legal consent flow thiếu integration/unit tests; PostgreSQL/Mongo refresh token ownership không rõ; admin endpoints thiếu RBAC evidence.
- P2: i18n auth/MFA/security copy thiếu locale hoặc docs không phân biệt API guard vs frontend guard.

## 7. Verify khi review PR

- Backend auth/security changes: chạy `ApiAndConfigurationStandardsTests` cùng affected unit/integration tests.
- Frontend protected-route changes: chạy `check-auth-fail-closed.mjs` và clean architecture guard.
- Feature docs Auth/MFA/UserContext/Legal/Admin phải dẫn controller/action/handler/repository cụ thể, không chỉ dẫn file cross-cutting này.

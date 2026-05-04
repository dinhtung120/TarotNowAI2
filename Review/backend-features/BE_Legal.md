# BE Legal

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Legal`
- Controller: `Backend/src/TarotNow.Api/Controllers/LegalController.cs`
- Test: `Backend/tests/TarotNow.Api.IntegrationTests/LegalIntegrationTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `UserConsents`; không thấy Mongo collection riêng cho legal trong `MongoDbContext.cs`
- Runtime config: `ISystemConfigSettings.LegalMinimumAge` được expose qua public runtime policies

## Entry points & luồng chính

`LegalController.cs` có ba endpoint chính:

- `GET runtime-policies` với `[AllowAnonymous]`: trả policy public cho frontend unauthenticated, hiện có `auth.minimumAge` từ `LegalMinimumAge`.
- `GET consent-status` với `[Authorize]` và `[EnableRateLimiting("auth-session")]`: dispatch `CheckConsentQuery` theo user hiện tại, document type/version optional.
- `POST consent` với `[Authorize]` và `[EnableRateLimiting("auth-session")]`: dispatch `RecordConsentCommand`.

`RecordConsent` lấy `UserId` từ authenticated principal và ghi thêm metadata audit gồm IP address và `User-Agent` vào command.

## Dependency và dữ liệu

Application feature gồm:

- `Commands/RecordConsent`
- `Queries/CheckConsent`

PostgreSQL state chính:

- `UserConsents`: consent records theo user/document/version và metadata.

Không có evidence runtime Mongo collection riêng cho legal. Không nên claim `data_rights_requests` hoặc `admin_actions` là runtime Legal state nếu chưa đọc DbSet/entity/config tương ứng.

## Boundary / guard

- `runtime-policies` intentionally public; chỉ nên trả policy an toàn public, không trả secret/config nội bộ.
- `consent-status` và `consent` phải fail-closed khi thiếu user id.
- `RecordConsent` là compliance/audit path: IP/User-Agent/document/version phải được review khi đổi contract.
- Controller hiện inject `ISystemConfigSettings` để trả public policy; các mutation/query vẫn qua MediatR.

## Test coverage hiện có

`LegalIntegrationTests.cs` kiểm luồng:

- seed authenticated test user;
- `POST /api/v1/legal/consent` cho `TOS`, `PrivacyPolicy`, `AiDisclaimer` version `1.0`;
- `GET /api/v1/legal/consent-status` trả `isFullyConsented: true`;
- query version mới `TOS v2.0` trả trạng thái chưa consent.

Không thấy unit test riêng cho `RecordConsentCommandHandler`/`CheckConsentQueryHandler` trong evidence đã đọc; integration test đang là evidence chính.

## Rủi ro

- P0: consent được ghi cho user id từ payload thay vì token; public runtime policies leak config nhạy cảm; legal consent status fail-open.
- P1: đổi document type/version policy mà không update integration tests và frontend legal flow.
- P1: metadata audit IP/User-Agent bị bỏ hoặc ghi sai khi qua proxy nếu không có trusted forwarding policy.
- P2: docs claim legal Mongo/table phụ không có evidence trong current DbContext.

## Kết luận

Legal backend là compliance module nhỏ nhưng quan trọng, tập trung vào runtime policy public và user consent audit. Evidence tốt nhất hiện tại là `LegalController.cs`, `UserConsents` trong `ApplicationDbContext`, và `LegalIntegrationTests.cs`.

# BE Profile

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Profile`
- Controller: `Backend/src/TarotNow.Api/Controllers/ProfileController.cs`
- Tests: `Backend/tests/TarotNow.Api.IntegrationTests/ProfileIntegrationTests.cs`, `UpdateProfileCommandHandlerTests.cs`, `PresignAvatarUploadCommandHandlerTests.cs`, `UserProfileProjectionSyncRequestedDomainEventHandlerTests.cs`
- Datastore/runtime: `ApplicationDbContext.cs` DbSet `Users`; `MongoDbContext.cs` collections `upload_sessions`, `reader_profiles`
- Related constants: `VietnamBankCatalog`, upload contracts `AvatarPresignRequest`, `AvatarConfirmRequest`

## Entry points & luồng chính

`ProfileController.cs` dùng `[EnableRateLimiting("auth-session")]`; từng endpoint yêu cầu `[Authorize]`.

Endpoints chính:

- `GET profile`: `GetProfileQuery { UserId }`.
- `GET payout-banks`: trả bank catalog public cho user đã auth.
- `PATCH profile`: `UpdateProfileCommand` gồm display/date-of-birth và payout bank fields.
- `POST avatar/presign`: `PresignAvatarUploadCommand`.
- `POST avatar/confirm`: `ConfirmAvatarUploadCommand`.

Controller luôn lấy `UserId` từ token, không nhận user id từ body.

## Dependency và dữ liệu

Profile state chính nằm trên `Users` và projection/read model liên quan:

- User profile fields: display name/date of birth/avatar/payout fields.
- Upload flow dùng `upload_sessions` để presign/confirm avatar.
- Reader profile projection có thể sync khi user profile đổi, evidence từ `UserProfileProjectionSyncRequestedDomainEventHandlerTests.cs`.

Payout bank account fields là sensitive personal/financial data; review cần đọc entity/config encryption trước khi khẳng định có mã hóa. Evidence trong Withdrawal đã thấy encryption cho withdrawal bank fields, nhưng không tự suy ra Profile payout fields cũng được mã hóa nếu chưa đọc mapping.

## Boundary / guard

- Profile update phải enforce ownership bằng token user id.
- Avatar presign/confirm phải validate ownership token/object key/session expiry ở handler, không chỉ controller.
- Payout fields không được log/expose quá rộng.
- Controller chỉ dispatch MediatR, không trực tiếp gọi storage provider.

## Test coverage hiện có

- `ProfileIntegrationTests.cs`: API integration coverage cho profile flow.
- `UpdateProfileCommandHandlerTests.cs`: command update coverage.
- `PresignAvatarUploadCommandHandlerTests.cs`: avatar presign coverage.
- `UserProfileProjectionSyncRequestedDomainEventHandlerTests.cs`: sync projection khi profile đổi.

Cần đọc thêm `ConfirmAvatarUploadCommandHandler` tests nếu audit avatar confirm sâu; evidence list hiện không thấy tên test riêng cho confirm avatar.

## Rủi ro

- P0: user cập nhật profile/payout/avatar của người khác; upload confirm chấp nhận object key/token không thuộc user; payout bank fields bị log/expose plaintext.
- P1: profile update không sync reader profile projection; avatar upload session không expiry/ownership check.
- P2: docs claim payout profile table riêng khi source chưa chứng minh.

## Kết luận

Profile là user-owned data module có avatar upload và payout bank fields nhạy cảm. Review đúng phải đọc controller, update/presign/confirm handlers, upload session repository và projection sync tests.

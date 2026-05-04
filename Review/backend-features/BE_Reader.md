# BE Reader

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Reader`
- Controllers: `Backend/src/TarotNow.Api/Controllers/ReaderController.cs`, `ReaderController.Directory.cs`, `ReaderController.ReaderFlow.cs`, `AdminReaderRequestsController.cs`
- Tests: `SubmitReaderRequestCommandHandlerTests.cs`, `UpdateReaderStatusCommandHandlerTests.cs`, `UpdateReaderProfileCommandHandlerTests.cs`, `UserStatusChangedReaderProjectionDomainEventHandlerTests.cs`
- Runtime/data: `MongoDbContext.cs` collections `reader_requests`, `reader_profiles`, `conversation_reviews`; presence overlay qua `IUserPresenceTracker`
- Constants: `Backend/src/TarotNow.Api/Constants/ApiReaderStatusConstants.cs`, `ApiRoleConstants.TarotReader`

## Entry points & luồng chính

Reader API tách thành public directory/profile và authenticated reader flow:

- `GET profile/{userId}`: lấy `GetReaderProfileQuery`, trả 404 nếu không có profile, apply presence status trước response.
- `GET readers` qua `ApiRoutes.ReadersAbsolute`: list readers bằng `ListReadersQuery`, apply presence cho từng reader.
- `POST apply`: authenticated user gửi `SubmitReaderRequestCommand`.
- `GET my-request`: authenticated user đọc trạng thái đơn của chính mình.
- `PATCH profile`: role `TarotReader` cập nhật hồ sơ bằng `UpdateReaderProfileCommand`.
- `PATCH status`: role `TarotReader` cập nhật status bằng `UpdateReaderStatusCommand`.

`ReaderController` inject `IUserPresenceTracker`; đây là realtime/status overlay tại API boundary, không phải persistence write.

## Dependency và dữ liệu

Source/test evidence cho thấy Reader dùng:

- MongoDB `reader_requests`: đơn đăng ký reader.
- MongoDB `reader_profiles`: hồ sơ reader hiển thị directory/profile.
- MongoDB `conversation_reviews`: rating/review liên quan profile reader.
- Auth/user role state trong `Users` khi admin approve hoặc user status projection sync.
- Presence tracker Redis/in-memory để nâng trạng thái offline → online khi runtime thấy user đang online.

Commands đã thấy từ tests/listing:

- `SubmitReaderRequest`
- `UpdateReaderProfile`
- `UpdateReaderStatus`

Admin path:

- `AdminReaderRequestsController.cs` và `ApproveReaderCommandHandlerTests.cs` liên quan duyệt reader request.

## Boundary / guard

- Public directory/profile endpoint không nên expose sensitive application documents/proof documents.
- `apply` và `my-request` lấy user id từ token; không nhận chủ đơn từ payload.
- `profile/status` update yêu cầu role `TarotReader`.
- Reader command entry handler phải mỏng theo event-driven architecture nếu là write command handler.
- Không khẳng định có PostgreSQL `reader_payout_profiles` runtime nếu chưa đọc entity/config; evidence hiện tại từ DbContext đã đọc không thấy DbSet tên này.

## Test coverage hiện có

- `SubmitReaderRequestCommandHandlerTests.cs`: command publish `ReaderRequestSubmitRequestedDomainEvent` và map fields như bio/specialties/experience/social URLs/diamond price/proof docs.
- `UpdateReaderProfileCommandHandlerTests.cs`: coverage cập nhật hồ sơ reader.
- `UpdateReaderStatusCommandHandlerTests.cs`: coverage status update.
- `UserStatusChangedReaderProjectionDomainEventHandlerTests.cs`: coverage sync projection khi user status đổi.

Không thấy API integration test riêng cho ReaderController trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P1 cho role/ownership/public profile contract.

## Rủi ro

- P0: reader update/profile endpoint cho phép user sửa profile của người khác; proof documents hoặc thông tin nhạy cảm trong request bị expose public; admin approve bypass role/audit.
- P1: presence overlay làm trạng thái hiển thị khác persistence, cần test rõ khi sửa status logic.
- P1: nhầm `conversation_reviews` thành collection generic `reviews` trong docs hoặc query.
- P2: docs claim payout profile table không có evidence runtime.

## Kết luận

Reader backend là module profile/directory + application workflow, phụ thuộc Mongo read models và presence runtime. Review đúng phải đọc cả public directory, authenticated reader flow, admin approval và projection/domain event tests.

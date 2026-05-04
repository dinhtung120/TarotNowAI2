# BE History

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/History`
- Controller: `Backend/src/TarotNow.Api/Controllers/HistoryController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/History/Queries/GetReadingHistoryQueryHandlerTests.cs`, `GetReadingDetailQueryHandlerTests.cs`
- Runtime/data: `MongoDbContext.cs` collection `reading_sessions`, `cards_catalog`, `user_collections`; `ApplicationDbContext.cs` có `ReadingRevealSagaStates` cho reading lifecycle liên quan
- Related feature: `Backend/src/TarotNow.Application/Features/Reading`

## Entry points & luồng chính

`HistoryController.cs` là authenticated API với `[Authorize]` và `[EnableRateLimiting("auth-session")]`.

Endpoints chính:

- `GET sessions`: lấy lịch sử reading của user hiện tại, có pagination/filter `spreadType`, `date`.
- `GET sessions/{id}`: lấy chi tiết reading session của user hiện tại, trả 404 nếu không tìm thấy.
- `GET admin/all-sessions`: endpoint admin role để xem toàn bộ reading sessions với filters username/spread/date range.

Controller lấy user id từ `ClaimTypes.NameIdentifier`, parse `Guid`; không nhận user id từ query/body cho user-facing history.

Pagination được clamp ở controller:

- `page <= 0` → `1`.
- `pageSize` chỉ nhận `1..50`, ngoài range → `10`.

## Dependency và dữ liệu

History là read/query module trên reading sessions:

- `GetReadingHistoryQueryHandler` đọc `IReadingSessionRepository.GetSessionsByUserIdAsync`.
- `GetReadingDetailQueryHandler` đọc chi tiết session/card data theo user/session.
- `GetAllReadingsQuery` phục vụ admin dashboard.

Runtime state chính là MongoDB `reading_sessions`, kèm card metadata từ `cards_catalog` và user collection nếu detail enrich cards.

## Boundary / guard

- User-facing detail/list phải enforce ownership bằng user id từ token.
- Admin all-sessions phải giữ `[Authorize(Roles = "admin")]`.
- History query không được mutate reading session hoặc wallet/AI state.
- Khi đổi page/pageSize/filter semantics cần update FE history consume và tests.

## Test coverage hiện có

- `GetReadingHistoryQueryHandlerTests.cs`: kiểm pagination metadata, completed flag mapping và empty list contract.
- `GetReadingDetailQueryHandlerTests.cs`: coverage detail query.

Không thấy API integration test riêng cho `HistoryController` trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P1 cho ownership/admin RBAC/pagination clamp.

## Rủi ro

- P0: user đọc được reading session của user khác; admin endpoint thiếu role; detail query leak prompt/AI result nhạy cảm sai audience.
- P1: pagination clamp ở controller khác với FE expectation hoặc query handler expectation.
- P1: history/detail mapping lệch với `reading_sessions` schema khi Reading module thay đổi.
- P2: docs nói History có command/mutation trong khi source hiện là query-only module.

## Kết luận

History backend là read model của Reading, chủ yếu trên `reading_sessions`. Review đúng phải đọc controller ownership/admin route, query handlers và tests detail/history; không review History như module write độc lập.

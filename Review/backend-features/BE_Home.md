# BE Home

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Home`
- Controller: `Backend/src/TarotNow.Api/Controllers/HomeController.cs`
- Test evidence: `Backend/tests/TarotNow.Api.IntegrationTests/UserContextHomeIntegrationTests.cs`
- Runtime/data: home snapshot đọc featured readers; map evidence cho thấy liên quan `MongoDbContext.cs` collection `reader_profiles` và presence/runtime status nếu handler áp dụng tương tự reader directory
- Guards: API version/XML summary và clean architecture boundaries

## Entry points & luồng chính

`HomeController.cs` là public API với `[AllowAnonymous]`.

Endpoint chính:

- `GET /snapshot` → `GetHomeSnapshotQuery` → trả `HomeSnapshotDto`.

Controller chỉ inject `IMediator`, không có repository/db context trực tiếp.

## Dependency và dữ liệu

Home là read/aggregation module cho landing page. Evidence integration test `HomeSnapshot_ShouldReturnFeaturedReadersPayload` kiểm response có:

- `featuredReaders` array.
- `totalCount` number.

Datastore/runtime thực tế cần đối chiếu ở query handler/repository khi audit sâu, nhưng map hiện tại cho thấy featured readers chủ yếu đến từ reader/profile read model, không phải command state riêng của Home.

## Boundary / guard

- Vì `[AllowAnonymous]`, endpoint không được trả dữ liệu user-private hoặc admin-only.
- Home should remain read-only; không thêm side effect/cache mutation business vào controller.
- Nếu snapshot dùng presence/status, phải đảm bảo public response chỉ chứa trạng thái hiển thị an toàn.
- Frontend route/home prefetch phụ thuộc response shape; thay đổi `featuredReaders`/`totalCount` cần đối chiếu FE Home.

## Test coverage hiện có

- `UserContextHomeIntegrationTests.cs` có `HomeSnapshot_ShouldReturnFeaturedReadersPayload`, gọi `/api/v1/home/snapshot` và assert `featuredReaders` + `totalCount`.

Không thấy unit test riêng cho `GetHomeSnapshotQueryHandler` trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P2/P1 tùy thay đổi payload.

## Rủi ro

- P1: anonymous snapshot leak field nhạy cảm từ reader profile/request.
- P1: response shape thay đổi làm FE landing/home prefetch hydration mismatch.
- P2: docs mô tả Home có command/mutation trong khi source hiện chỉ thấy query snapshot.

## Kết luận

Home backend là public read snapshot cho landing page, hiện evidence chính là `HomeController.cs` và integration test shape trong `UserContextHomeIntegrationTests.cs`. Review đúng phải đọc query handler và FE consume khi thay đổi payload.

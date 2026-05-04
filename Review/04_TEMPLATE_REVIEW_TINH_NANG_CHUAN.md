# Format review tính năng chuẩn

File này mô tả format đang dùng cho `BE_*.md` và `FE_*.md`. Đây không phải template để copy nguyên văn; mỗi mục phải được điền bằng evidence source cụ thể.

## Format backend feature

```md
# BE <Tên tính năng>

## Source đã đọc thủ công
- Application feature paths
- API controller/hub paths
- Infrastructure/persistence/realtime paths
- Test/architecture evidence paths

## Entry points & luồng chính
- Controller endpoints
- Commands/queries and handlers
- Requested domain events/outbox/realtime path nếu có

## Dependency và dữ liệu
- Upstream callers
- Downstream Application interfaces / Infrastructure implementations
- PostgreSQL tables
- MongoDB collections
- Redis/cache/pubsub path nếu có
- Transaction/idempotency/settlement path nếu flow nhạy cảm

## Boundary / guard
- Clean Architecture guard
- Event-driven command handler guard
- Auth/RBAC/ownership/rate-limit guard
- Money/AI/realtime side-effect constraints

## Rủi ro
- P0
- P1
- P2

## Kết luận
- Phạm vi và điểm review chính
```

## Format frontend feature

```md
# FE <Tên tính năng>

## Source đã đọc thủ công
- Feature folder
- Public export
- App routes
- App API proxy routes nếu có
- Prefetch runners/query keys
- Message namespaces

## Entry points & luồng chính
- Route wrapper behavior
- Feature components exported through public API
- SSR hydration/Suspense/dynamic import nếu có

## Dependency và dữ liệu
- Feature actions/hooks/server actions
- Backend API contract via proxy/shared client
- TanStack Query keys and cache hydration
- Role/auth/i18n dependencies

## Boundary / guard
- Thin route/public export boundary
- Protected route/API proxy fail-closed behavior
- Query key/hydration consistency
- i18n/a11y/component-size concerns

## Rủi ro
- P0
- P1
- P2

## Kết luận
- Phạm vi và điểm review chính
```

## Evidence source bắt buộc

- Backend feature reviews phải đối chiếu `Backend/src/TarotNow.Application/Features`, `Backend/src/TarotNow.Api/Controllers`, `Backend/src/TarotNow.Infrastructure`, `Backend/tests/TarotNow.ArchitectureTests` và test feature nếu có.
- Frontend feature reviews phải đối chiếu `Frontend/src/features`, `Frontend/src/app/[locale]`, `Frontend/src/app/api`, `Frontend/src/shared/server/prefetch`, `Frontend/messages/{vi,en,zh}` và `Frontend/scripts`.
- Data/ops/cross-cutting reviews phải đối chiếu `ApplicationDbContext.cs`, `MongoDbContext.cs`, `database/postgresql/schema.sql`, `database/mongodb/schema.md`, `docker-compose*.yml`, `deploy/scripts/*`, `.github/workflows/*`.

## Quy tắc viết

- Không viết claim chính nếu không có path evidence.
- Không ghi route/API/prefetch tồn tại khi chưa đọc hoặc chưa tìm thấy source.
- Nếu evidence không có, ghi rõ “không thấy evidence ... trong source đã đọc”.
- Không dùng path không tồn tại; đặc biệt `database/*` và `deploy/*` là hai nhánh riêng.
- Không gọi Mongo review collection là `reviews`; runtime collection đúng là `conversation_reviews` khi nói về conversation review.

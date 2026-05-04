# Template review tính năng chuẩn

```md
# [BE|FE] <Tên tính năng>

## 1. Phạm vi
- Mục tiêu nghiệp vụ:
- Module liên quan:
- In scope:
- Out of scope:

## 2. Entry points & luồng chính
- API/Command/Query/Route chính:
- Requested Domain Event (nếu có):
- Realtime event (nếu có):
- External integration (nếu có):

## 3. Dependency map
### 3.1 Upstream phụ thuộc vào module này
- ...

### 3.2 Module này phụ thuộc downstream
- Application interfaces:
- Infrastructure repositories/services:
- Shared utilities:
- Data stores:

### 3.3 Ràng buộc kiến trúc
- Clean Architecture boundary:
- Event-driven rules:
- Thin handler / thin route rules:

## 4. Dữ liệu & trạng thái
- Entity/Document chính:
- Transaction boundary:
- Idempotency key path (nếu có):
- Outbox/realtime bridge path (nếu có):

## 5. Frontend contract (nếu áp dụng)
- public.ts exports:
- App route wrapper:
- i18n keys:
- Prefetch/hydration/guard liên quan:

## 6. Test coverage hiện tại
- Architecture tests liên quan:
- Unit/Integration tests liên quan:
- Gaps:

## 7. Rủi ro kiến trúc
- P0:
- P1:
- P2:

## 8. Output review chuẩn
- Kết luận:
- Evidence:
- Việc cần làm ưu tiên cao:
- Việc theo dõi sau:
```

# Template review tính năng chuẩn

Template này là format bắt buộc cho mỗi `BE_*.md` và `FE_*.md`. Không dùng câu chung chung nếu không có evidence.

```md
# [BE|FE] <Tên tính năng>

## 1. Phạm vi source đã rà
- Feature source:
- API/App route source:
- Infrastructure/source phụ trợ:
- Test/guard source:

## 2. Entry points & luồng chính
- Commands/Queries/Actions/Routes:
- Handler/component/hook chính:
- Event/realtime/external integration:

## 3. Dependency map thực tế
### Upstream
- Module/route/controller gọi vào tính năng này:

### Downstream
- Application interfaces / API routes / shared utilities:
- Infrastructure repositories/services:
- Data stores:

## 4. Dữ liệu & trạng thái
- PostgreSQL tables:
- MongoDB collections:
- Redis/cache/pubsub:
- Transaction/idempotency/outbox path:

## 5. Boundary và guard
- Backend architecture guard hoặc frontend script guard:
- Thin handler/thin route evidence:
- Side-effect/realtime/i18n/a11y/size concerns:

## 6. Test coverage hiện tại
- Unit tests:
- Integration/e2e/architecture tests:
- Không tìm thấy evidence trực tiếp:

## 7. Rủi ro kiến trúc
- P0:
- P1:
- P2:

## 8. Kết luận review
- Mức độ phù hợp kiến trúc:
- Evidence quan trọng:
- Việc cần làm ưu tiên cao:
- Follow-up:
```

## Evidence source bắt buộc

- Backend feature reviews phải đối chiếu `Backend/src/TarotNow.Application/Features`, `Backend/src/TarotNow.Api`, `Backend/src/TarotNow.Infrastructure`, `Backend/tests/TarotNow.ArchitectureTests`.
- Frontend feature reviews phải đối chiếu `Frontend/src/features`, `Frontend/src/app/[locale]`, `Frontend/src/shared`, `Frontend/messages`, `Frontend/scripts`.

# BE History

## 1. Phạm vi

- Mục tiêu nghiệp vụ: review backend feature `History` thuộc nhóm `Reader/Reading domain`.
- Module liên quan: `Backend/src/TarotNow.Application/Features/History` và các Domain/Infrastructure/API tương ứng.
- In scope: command, query, requested domain event, repository/interface dependency, transaction, outbox, tests.
- Out of scope: refactor code hoặc sửa logic ngoài phạm vi review.

## 2. Entry points & luồng chính

- API/Command/Query/Route chính: rà soát controller/API và handlers trỏ tới feature `History`.
- Requested Domain Event: xác định các `*RequestedDomainEvent` và `*RequestedDomainEventHandler` nếu có.
- Realtime event: chỉ áp dụng nếu feature phát realtime/notification.
- External integration: rà soát provider/payment/AI/email/storage nếu feature gọi qua Application interface.

## 3. Dependency map

### 3.1 Upstream phụ thuộc vào module này

- API controllers hoặc application services gọi command/query của `History`.
- Frontend feature tương ứng nếu có.
- Domain event handlers hoặc workflows cross-feature nếu có.

### 3.2 Module này phụ thuộc downstream

- Application interfaces: repository, provider, cache, transaction coordinator, event publisher.
- Infrastructure repositories/services: chỉ được truy cập qua Application-owned interface.
- Shared utilities: validation, mapping, pipeline behaviors, result/error model.
- Data stores: PostgreSQL transactional state; MongoDB document/read-model nếu module có document; Redis nếu có cache/rate-limit/pubsub.

### 3.3 Ràng buộc kiến trúc

- Clean Architecture boundary: Application không reference concrete Infrastructure hoặc web framework.
- Event-driven rules: command entry handler phải thin và chỉ dispatch requested domain event khi là write command.
- Thin handler / thin route rules: controller không chứa orchestration nghiệp vụ hoặc side effect phụ.

## 4. Dữ liệu & trạng thái

- Entity/Document chính: xác định từ Domain entities, EF configs, Mongo documents liên quan `History`.
- Transaction boundary: review `CommandTransactionBehavior` và transaction coordinator nếu state mutation quan trọng.
- Idempotency key path: Không mặc định bắt buộc; rà soát nếu command có side effect, reward, notification hoặc external provider.
- Outbox/realtime bridge path: Rà soát nếu module phát notification, telemetry, reward hoặc side effect phụ.

## 5. Frontend contract (nếu áp dụng)

- public.ts exports: đối chiếu `Frontend/src/features` nếu có feature tương ứng.
- App route wrapper: route chỉ nên composition, không chứa business orchestration.
- i18n keys: user-facing text phải có VI/EN/ZH khi liên quan UI.
- Prefetch/hydration/guard liên quan: rà query key và SSR prefetch nếu flow hiển thị server state.

## 6. Test coverage hiện tại

- Architecture tests liên quan: `ArchitectureBoundariesTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `CodeQualityRulesTests.cs`.
- Unit/Integration tests liên quan: tìm trong `Backend/tests/TarotNow.Application.UnitTests`, `TarotNow.Infrastructure.*Tests`, `TarotNow.Api.IntegrationTests`.
- Gaps: ghi rõ command/query/event nào chưa có test hoặc chưa có architecture evidence.

## 7. Rủi ro kiến trúc

- P0: vi phạm boundary, thiếu idempotency/transaction khi có finance/quota/AI, side effect trực tiếp sai layer.
- P1: coupling chéo module, test coverage thiếu cho flow chính, event/outbox path chưa rõ.
- P2: tài liệu/evidence thiếu, naming hoặc cấu trúc chưa đồng nhất.

## 8. Output review chuẩn

- Kết luận: Pass / Pass có điều kiện / Cần remediation.
- Evidence: liệt kê file code/test/guard đã đọc.
- Việc cần làm ưu tiên cao: chỉ ghi item có impact kiến trúc.
- Việc theo dõi sau: gap P1/P2 hoặc câu hỏi cần owner xác nhận.

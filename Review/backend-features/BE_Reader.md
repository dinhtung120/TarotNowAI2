# BE Reader

## 1. Phạm vi source đã rà

- Feature source: `Backend/src/TarotNow.Application/Features/Reader`.
- API/controller source cần đối chiếu: `Backend/src/TarotNow.Api` với grep `Reader`.
- Infrastructure source cần đối chiếu: `Backend/src/TarotNow.Infrastructure` với repositories/services liên quan `Reader`.
- Test/guard source: `Backend/tests/TarotNow.ArchitectureTests/*.cs` và `Backend/tests` grep `Reader`.

## 2. Entry points & luồng chính

- Commands/Queries: source nằm dưới `Features/Reader/Commands` và/hoặc `Features/Reader/Queries` nếu thư mục tồn tại.
- Requested events/handlers: cần xác minh các file `*RequestedDomainEvent*` trong feature; write command phải đi qua `IInlineDomainEventDispatcher` theo `EventDrivenArchitectureRulesTests.cs`.
- Realtime/external integration: không mặc định; chỉ áp dụng nếu feature publish notification/realtime/event phụ.
- Finance/AI/reward integration: không mặc định; rà khi command có state mutation hoặc side effect.

## 3. Dependency map thực tế

### Upstream

- API controllers hoặc background/event handlers gọi command/query thuộc `Reader`.
- Frontend feature tương ứng nếu có route/API contract liên quan.
- Cross-feature events nếu `Reader` nhận hoặc phát domain events.

### Downstream

- Application interfaces: repository/provider/cache/transaction/event publisher abstractions được inject trong handlers.
- Infrastructure: implementation trong `Backend/src/TarotNow.Infrastructure` phải chỉ được gọi qua Application-owned interfaces.
- Data stores: xác minh bằng `ApplicationDbContext.cs`, `MongoDbContext.cs`, `database/postgresql/schema.sql`, `database/mongodb/schema.md`.

## 4. Dữ liệu & trạng thái

Evidence dữ liệu cụ thể: MongoDB reader_requests/reader_profiles; finance payout profile có PostgreSQL reader_payout_profiles nếu liên quan.


- PostgreSQL: rà nếu feature có transactional state.
- MongoDB: rà collection document/read-model nếu feature lưu hồ sơ, messages, reading sessions, community hoặc gamification documents.
- Redis/cache/pubsub: rà nếu feature dùng cache/rate-limit/pubsub.
- Transaction/idempotency/outbox: áp dụng nếu command mutate state hoặc publish side effect.

## 5. Boundary và guard

- Clean Architecture: `ArchitectureBoundariesTests.cs`.
- Event-driven command model: `EventDrivenArchitectureRulesTests.cs`.
- API/config/code quality: `ApiAndConfigurationStandardsTests.cs`, `CodeQualityRulesTests.cs`.
- Rule review: controller không orchestration nghiệp vụ; command handler mỏng; side effects qua event/outbox/handler.

## 6. Test coverage hiện tại

- Architecture tests: dùng toàn cục cho mọi backend feature.
- Feature tests: tìm bằng `find Backend/tests -type f | grep -E 'Reader|Architecture|EventDriven'`.
- Không tìm thấy evidence trực tiếp: ghi rõ từng command/query/event chưa có test khi audit chi tiết.

## 7. Rủi ro kiến trúc

- P0: boundary/event-driven violation; state mutation/side effect sai layer.
- P1: coupling chéo module, thiếu integration test cho luồng chính, outbox/realtime path chưa rõ.
- P2: evidence docs thiếu hoặc naming/path không đồng bộ.

## 8. Kết luận review

- Mức độ phù hợp kiến trúc: cần audit chi tiết theo source files trong `Features/Reader`; khung review này đã neo đúng source và guard.
- Evidence quan trọng: `Features/Reader`, architecture tests, Infrastructure persistence/repositories, API controllers.
- Việc cần làm ưu tiên cao: điền command/query/event/test cụ thể khi review PR hoặc module deep dive.
- Follow-up: không suy đoán nếu chưa thấy evidence trực tiếp.

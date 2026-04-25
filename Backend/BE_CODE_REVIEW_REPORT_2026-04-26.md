# Backend Code Review Report (2026-04-26) — Updated

## 1. Executive summary

- Đã review lại toàn bộ batch code đã implement để kiểm tra: sửa đúng chỗ, sửa tận gốc, có tạo lỗi mới hay không.
- Kết quả hiện tại:
  - `dotnet build Backend/TarotNow.slnx -v minimal`: pass.
  - `dotnet test Backend/TarotNow.slnx -v minimal`: pass toàn bộ.
- Trạng thái issue:
  - Tổng issue ban đầu: **34**.
  - Đã fix tận gốc và đã loại khỏi danh sách chi tiết: **26**.
  - Còn mở (chưa fix tận gốc hoặc mới ở mức partial): **8** issue.
- Các khu vực còn rủi ro cao nhất:
  - Kiến trúc command/event boundary (TD-01, TD-02).
  - Community create post/comment còn nguy cơ partial write (FB-08).
  - Outbox throughput mới cải thiện một phần, chưa tối ưu theo partition key nghiệp vụ (PF-02).
  - Thiếu test concurrency cho các flow race-critical (TG-01).

## 2. Review theo từng tính năng / module

### 2.1 Community (Post/Comment + media)
- Chức năng: tạo post/comment và attach media theo `contextDraftId`.
- Luồng xử lý chính: create entity -> publish event/counter -> attach media.
- Vấn đề phát hiện:
  - Flow vẫn lưu entity trước, attach media sau, chưa có saga/outbox retry cho attach thất bại.
- Bug tiềm ẩn:
  - Post/comment tồn tại nhưng media không đồng bộ, gây dữ liệu partial.
- Code thừa/dead code: không đáng kể.
- Nợ kỹ thuật: cao ở consistency nhiều bước.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Đưa attach media sang outbox worker với state `pending_media` và retry/dlq.

### 2.2 Outbox / Background throughput
- Chức năng: claim và dispatch outbox messages.
- Luồng xử lý chính: claim batch -> group partition -> xử lý từng message trong partition.
- Vấn đề phát hiện:
  - Đã có song song hóa theo `EventType`, nhưng trong mỗi partition vẫn tuần tự; nếu traffic dồn vào một event type thì vẫn nghẽn.
  - Chưa có partition key nghiệp vụ (aggregate/reference key) để vừa giữ ordering cần thiết vừa scale ổn định.
- Bug tiềm ẩn:
  - Backlog tăng khi một event type chiếm đa số.
- Code thừa/dead code: không.
- Nợ kỹ thuật: medium.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý:
  - Bổ sung partition key ổn định theo business identity thay vì chỉ `EventType`.

### 2.3 AI Stream API (Controller design)
- Chức năng: endpoint SSE stream reading.
- Luồng xử lý chính: controller feature-gate + auth + start stream + stream/finalize.
- Vấn đề phát hiện:
  - Controller đã tách partial file, nhưng orchestration runtime/failure/finalization vẫn nằm chủ yếu ở controller, chưa đạt thin-controller rõ ràng.
- Bug tiềm ẩn:
  - Khó unit-test, tăng coupling transport vs business orchestration.
- Code thừa/dead code: không.
- Nợ kỹ thuật: medium.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý:
  - Trích `AiStreamOrchestrator`/application service để controller chỉ bind request + return response.

### 2.4 Kiến trúc command/event boundary
- Chức năng: enforce clean architecture + side-effects qua event handlers.
- Luồng xử lý chính: commands -> domain events -> handlers.
- Vấn đề phát hiện:
  - Command handlers vẫn inject repository/service/provider diện rộng.
  - Architecture tests hiện chưa chặn kiểu vi phạm này một cách tổng quát.
- Bug tiềm ẩn:
  - Coupling cao, side-effects phân tán, khó kiểm soát regression.
- Code thừa/dead code: không.
- Nợ kỹ thuật: rất cao.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Mở rộng rule test để fail khi command handlers inject `I*Repository/I*Service/I*Provider` (allowlist tường minh).

### 2.5 Mapping strategy
- Chức năng: mapping DTO/view model.
- Luồng xử lý chính: đăng ký AutoMapper global + manual mapping ở nhiều module.
- Vấn đề phát hiện:
  - Mapping strategy vẫn chưa thống nhất; AutoMapper có nhưng phần lớn code vẫn map tay.
- Bug tiềm ẩn:
  - Drift logic mapping giữa các module.
- Code thừa/dead code: có dấu hiệu over-configuration.
- Nợ kỹ thuật: low-medium.
- Mức độ ưu tiên: Low.
- Đề xuất xử lý:
  - Chốt guideline chính thức: module nào dùng AutoMapper, module nào map tay, và tiêu chí bắt buộc.

### 2.6 Dependencies + Test coverage
- Chức năng: runtime alignment và confidence trước release.
- Luồng xử lý chính: build/test + dependency governance.
- Vấn đề phát hiện:
  - Project vẫn target `net9.0`.
  - Chưa thấy test concurrency chuyên biệt cho check-in/consent/wallet-deadlock theo scope issue gốc.
- Bug tiềm ẩn:
  - Regression race-condition khó phát hiện sớm.
- Code thừa/dead code: không.
- Nợ kỹ thuật: medium.
- Mức độ ưu tiên: High (cho test gaps), Medium (cho dependency/runtime).
- Đề xuất xử lý:
  - Bổ sung integration tests concurrent 2-10 requests cho các flow race-critical.
  - Lên wave nâng runtime/dependency đồng bộ.

## 3. Danh sách issue chi tiết

### Functional bugs

- ID: FB-08
- Module: Community Post/Comment
- Loại: Functional bugs
- Mô tả: Entity được ghi trước, attach media chạy sau; lỗi attach vẫn để lại entity partial.
- Tác động: Dữ liệu post/comment và media không nhất quán, khó reconcile.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Community/Commands/CreatePost/CreatePostCommand.cs:91-107`
  - `src/TarotNow.Application/Features/Community/Commands/AddComment/AddCommentCommand.cs:93-103`
- Cách sửa:
  - Chuyển attach media sang outbox worker với trạng thái `pending_media`, retry + dead-letter rõ ràng.
- Priority: High

### Performance issues

- ID: PF-02
- Module: Outbox Worker
- Loại: Performance issues
- Mô tả: Đã có parallelism nhưng partition theo `EventType`; trong từng partition vẫn xử lý tuần tự.
- Tác động: Nếu tải dồn vào 1 event type, throughput vẫn nghẽn.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs:49-64`
  - `src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.Processing.cs:16-37`
- Cách sửa:
  - Chia partition theo business key/aggregate key và giữ ordering trong từng key.
- Priority: Medium

### Code smell / dead code

- ID: CS-02
- Module: AI Controller
- Loại: Code smell / dead code
- Mô tả: Controller vẫn chứa orchestration stream/failure/finalize đáng kể, chưa đạt thin-controller.
- Tác động: Khó test, coupling transport/business cao.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/AiController.cs:47-94`
  - `src/TarotNow.Api/Controllers/AiController.Streaming.cs:17-127`
  - `src/TarotNow.Api/Controllers/AiController.Streaming.Completion.cs:21-97`
- Cách sửa:
  - Đưa orchestration vào service riêng, controller chỉ giữ concern HTTP.
- Priority: Medium

- ID: CS-04
- Module: Mapping Layer
- Loại: Code smell / dead code
- Mô tả: AutoMapper đăng ký global nhưng dùng cục bộ; phần lớn mapping vẫn manual.
- Tác động: Dễ drift và khó bảo trì consistency mapping.
- Bằng chứng trong code:
  - `src/TarotNow.Application/DependencyInjection.cs:79-83`
  - `src/TarotNow.Application/Features/Community/Queries/GetFeed/GetFeedQuery.cs:41-57,112-126`
- Cách sửa:
  - Chốt guideline mapping toàn codebase + chuẩn hóa theo module.
- Priority: Low

### Tech debt

- ID: TD-01
- Module: Application Commands Architecture
- Loại: Tech debt
- Mô tả: Command handlers vẫn inject dependency business/infrastructure trực tiếp diện rộng.
- Tác động: Vi phạm guardrail strict event-driven, tăng coupling.
- Bằng chứng trong code:
  - Quét hiện tại: `221` file có pattern `CommandHandler`, `274` điểm inject `I*Repository/I*Service/I*Provider` trong Commands.
- Cách sửa:
  - Refactor theo wave: command chỉ validate + publish domain events; side-effects đẩy về event handlers idempotent.
- Priority: High

- ID: TD-02
- Module: Architecture Tests
- Loại: Tech debt
- Mô tả: Rule kiến trúc hiện cấm một tập interface hẹp, chưa enforce cấm tổng quát dependency trong command handlers.
- Tác động: Quality gate xanh nhưng vẫn lọt nhiều vi phạm guardrail.
- Bằng chứng trong code:
  - `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs:19-27`
  - `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs:172-181`
- Cách sửa:
  - Mở rộng rule regex/semantic để chặn tổng quát `I*Repository/I*Service/I*Provider` (kèm allowlist rõ ràng).
- Priority: High

- ID: TD-06
- Module: Dependencies / Runtime
- Loại: Tech debt
- Mô tả: Runtime target vẫn `net9.0`; dependency wave chưa đồng bộ theo kế hoạch nâng cấp.
- Tác động: Chậm nhận fix bảo mật/hiệu năng dài hạn.
- Bằng chứng trong code:
  - `src/TarotNow.Api/TarotNow.Api.csproj:4`
  - `src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj:6`
- Cách sửa:
  - Thực hiện wave nâng cấp runtime/dependency có regression suite đầy đủ.
- Priority: Medium

### Test gaps

- ID: TG-01
- Module: Concurrency-Critical Flows
- Loại: Test gaps
- Mô tả: Chưa có bộ test concurrency chuyên biệt cho các race-condition critical đã nêu trong report gốc.
- Tác động: Nguy cơ tái phát race bug mà CI vẫn xanh.
- Bằng chứng trong code:
  - Chưa có suite riêng cho CheckIn/Legal consent/wallet deadlock trong `tests/TarotNow.Application.UnitTests/Features` và integration tests tương ứng.
  - `rg` hiện tại chỉ thấy test reader submit dạng unit (không phải stress/concurrent integration).
- Cách sửa:
  - Thêm integration tests concurrent cho: check-in double submit, consent double submit, reader pending duplicate, wallet release A->B/B->A.
- Priority: High

## 4. Danh sách refactor đề xuất

### Việc nên làm ngay

1. Fix FB-08 bằng outbox/state machine cho media attach để loại partial write.
2. Hoàn thiện PF-02 theo partition key nghiệp vụ thay vì `EventType`.
3. Bổ sung test concurrency bắt buộc cho TG-01.
4. Nâng architecture rules để chặn vi phạm TD-01/TD-02 ngay trên CI.

### Việc có thể trì hoãn (nhưng nên lên roadmap)

1. Tách orchestration AI stream khỏi controller (CS-02).
2. Chuẩn hóa chiến lược mapping toàn hệ thống (CS-04).
3. Nâng runtime/dependency theo wave (TD-06).

## 5. Kết luận

- Trạng thái hiện tại đã cải thiện mạnh so với report gốc: phần lớn lỗi critical/security/concurrency đã được xử lý và test suite hiện đang xanh.
- Tuy nhiên chưa thể coi là “đóng toàn bộ” vì còn 8 issue mở, trong đó có các mục High ảnh hưởng trực tiếp tới độ bền kiến trúc và consistency dữ liệu.
- Mức an toàn release:
  - Có thể release có kiểm soát nếu bắt buộc.
  - Để đạt mức release-confidence cao và bền vững, cần đóng dứt điểm nhóm FB-08, TD-01, TD-02, TG-01.

---

## Danh sách issue đã fix tận gốc và đã xóa khỏi danh sách chi tiết

`CB-01, CB-02, CB-03, CB-04, CB-05, FB-01, FB-02, FB-03, FB-04, FB-05, FB-06, FB-07, FB-09, PF-01, PF-03, SE-01, SE-02, SE-03, SE-04, SE-05, SE-06, CS-01, CS-03, TD-03, TD-04, TD-05`

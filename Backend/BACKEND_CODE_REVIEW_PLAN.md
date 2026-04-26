# Kế hoạch review backend (TarotNow)

## 0. Snapshot codebase hiện tại

- Kiến trúc: `TarotNow.Api` + `TarotNow.Application` + `TarotNow.Domain` + `TarotNow.Infrastructure` + test projects.
- Quy mô source backend: `1173` file `.cs` trong `Backend/src`.
- Quy mô test: `120` file `.cs` trong `Backend/tests`.
- Phân bố lớn:
  - `TarotNow.Application`: `564` file (trọng tâm business logic)
  - `TarotNow.Infrastructure`: `342` file (DB, cache, queue, outbox, background jobs)
  - `TarotNow.Api`: `140` file (controllers, middleware, startup, hubs)
  - `TarotNow.Domain`: `127` file (entities/events/value objects)
- Baseline kiến trúc: `TarotNow.ArchitectureTests` đang pass `26/26`.

## 1. Mục tiêu review

- Review toàn bộ backend theo từng **feature/module/service/API/job/flow**.
- Review theo mức độ **nghiêm ngặt, thiên về phát hiện lỗi**.
- Chỉ ghi vấn đề, không ghi điểm tốt.
- Tạo một báo cáo tổng hợp duy nhất: `Backend/BACKEND_REVIEW_REPORT.md`.

## 2. Phân rã module để review

## 2.1 Module theo business domain

1. `Auth`, `Mfa`, `UserContext`
2. `Wallet`, `Deposit`, `Withdrawal`, `Escrow`, `AdminReconciliation`
3. `Reading`, `Reader`, `Ai`, `Conversation` (bao gồm flow tài chính trong chat)
4. `Chat`, `Notification`, `Presence`, `History`
5. `Community` (post/comment/reaction/report/media)
6. `Gamification`, `Gacha`, `Inventory`, `CheckIn`, `Promotions`, `Home`, `Profile`, `Legal`
7. `Admin` (users, moderation, outbox, system config)

## 2.2 Module kỹ thuật bắt buộc

1. API boundary:
   - `Controllers`, `Middlewares`, `Hubs`, `Startup`, `Contracts`
2. Application:
   - `Features/*`, `Behaviors`, `DomainEvents`, `Interfaces`, `Validators`, `Mappings`
3. Domain:
   - `Entities`, `Events`, `ValueObjects`, `Enums`
4. Infrastructure:
   - `Persistence`, `Repositories`, `Services`, `Messaging`, `BackgroundJobs`, `Security`, `Options`, `Migrations`
5. Test layer:
   - Unit, Integration, Architecture tests

## 3. Chiến lược review theo phase (ưu tiên rủi ro cao trước)

## Phase 1: Guardrails kiến trúc + side-effect (ưu tiên cao nhất)

- Mục tiêu: tìm vi phạm Clean Architecture và event-driven rules.
- Trọng tâm:
  - Command handler gọi trực tiếp repository/service/notification/email/AI provider.
  - Side-effect không đi qua domain events + event handlers.
  - API/Domain phụ thuộc sai layer.
- Output: danh sách issue kiến trúc + side-effect, gắn priority `High` nếu ảnh hưởng flow tiền/quota/notification.

## Phase 2: Money/Wallet/Escrow/Quota/AI calls

- Mục tiêu: bóc tách lỗi transaction, idempotency, double-spend, refund/retry/timeout.
- Trọng tâm:
  - ACID transaction boundaries.
  - Invariant `balance >= 0`.
  - Idempotency key cho flow thanh toán/rút tiền/webhook.
  - Reserve quota atomically trước AI call, controlled retry + timeout + auto-refund.
  - Mọi mutation tiền publish event đúng chuẩn.
- Output: issue bucket `Critical bugs`, `Functional bugs`, `Security issues`.

## Phase 3: API/Authorization/Security/Observability

- Mục tiêu: kiểm tra an toàn endpoint và khả năng vận hành production.
- Trọng tâm:
  - Ownership checks, policy-based authorization, rate limiting endpoint nhạy cảm.
  - ProblemDetails consistency.
  - JWT/refresh rotation/cookie flags.
  - Serilog structured logging, OpenTelemetry instrumentation, correlation id.
- Output: issue bucket `Security issues`, `Tech debt`, `Performance issues`.

## Phase 4: Background jobs/queue/outbox/cache/concurrency

- Mục tiêu: tìm race condition, retry sai, duplicate processing, data drift.
- Trọng tâm:
  - Outbox processor, Redis pub/sub, cron jobs, compensation flows.
  - Locking strategy, dedup, idempotent consumers.
  - Timeout/cancellation token propagation.
- Output: issue bucket `Critical bugs`, `Performance issues`, `Tech debt`.

## Phase 5: Chất lượng code + maintainability + test gaps

- Mục tiêu: chốt code smell/dead code/naming/structure và khoảng trống test.
- Trọng tâm:
  - Method quá dài, class quá tải trách nhiệm, magic numbers/strings.
  - Code trùng lặp, dead code, util/helper không dùng.
  - Thiếu unit/integration test cho flow rủi ro cao.
- Output: issue bucket `Code smell/dead code`, `Test gaps`, `Refactoring suggestions`.

## 4. Quy trình review từng file (đơn vị thực thi)

Mỗi file đều đi theo checklist bắt buộc:

1. Xác định vai trò file trong flow nào.
2. Đối chiếu với guardrails kiến trúc của dự án.
3. Tìm bug logic + edge case + lỗi dữ liệu + lỗi transaction/concurrency.
4. Đánh giá exception handling/logging/retry/timeout.
5. Kiểm tra bảo mật + authorization + validation.
6. Tìm code thừa/dead code/duplicate/hardcode.
7. Đối chiếu test hiện có: thiếu test nào để bắt lỗi này.
8. Ghi issue có bằng chứng cụ thể (file/function/line hoặc đoạn code liên quan).

## 5. Định nghĩa issue (bắt buộc)

Mỗi issue trong report phải có đủ:

- `ID`
- `Module`
- `Loại` (thuộc 1 bucket chuẩn)
- `Mô tả`
- `Tác động`
- `Bằng chứng trong code` (file + function/class/endpoint + trích dẫn ngữ cảnh)
- `Nguyên nhân gốc rễ`
- `Cách sửa đề xuất`
- `Priority` (`High` | `Medium` | `Low`)

Buckets chuẩn:

- Critical bugs
- Functional bugs
- Performance issues
- Security issues
- Code smell / dead code
- Tech debt
- Test gaps
- Refactoring suggestions

## 6. Cấu trúc file output cuối cùng

Tên file: `Backend/BACKEND_REVIEW_REPORT.md`

## 1. Executive summary

- Trạng thái tổng thể backend
- Mức độ rủi ro release
- Top khu vực đáng lo nhất

## 2. Review theo từng tính năng/module

Mỗi module ghi:

- Chức năng
- Luồng xử lý chính
- Vấn đề phát hiện
- Bug tiềm ẩn
- Code thừa/dead code
- Nợ kỹ thuật
- Mức độ ưu tiên
- Đề xuất xử lý

## 3. Danh sách issue chi tiết

Theo template issue ở Mục 5.

## 4. Danh sách refactor đề xuất

- Việc nên làm ngay
- Việc có thể trì hoãn

## 5. Kết luận

- Đánh giá chất lượng backend
- Mức an toàn để release
- Rủi ro lớn nhất còn tồn tại

## 7. Kế hoạch thực thi đề xuất

- Pass A (rủi ro cao): Auth + Money + Escrow + AI + Webhook + Background jobs liên quan tiền.
- Pass B (runtime boundary): Controllers/Middlewares/Hubs/Policies/Rate limit/ProblemDetails.
- Pass C (feature business): Chat/Community/Reading/Gamification/Gacha/Notification/Profile.
- Pass D (infra & maintainability): repositories/services/migrations/scripts/tests/dead code.
- Pass E (chốt report): chuẩn hóa issue IDs, khử trùng lặp, ưu tiên fix roadmap.

## 8. Tiêu chí hoàn thành review

- Bao phủ `100%` module backend trong `src` và test layer liên quan.
- Mỗi issue có bằng chứng cụ thể từ code.
- Không còn issue mô tả chung chung.
- Report có thể chuyển thẳng cho team để tạo ticket sửa lỗi.

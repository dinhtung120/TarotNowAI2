# Backend Code Review Report (2026-04-26)

## 1. Executive summary

- Phạm vi review: toàn bộ backend theo các layer `Api`, `Application`, `Domain`, `Infrastructure`, `tests`, tập trung theo feature/module/service/API/job/flow và các điểm giao dịch tài chính.
- Kết quả tổng quan: mức rủi ro **High**. Có nhiều điểm có thể gây lỗi production về concurrency, bảo mật, idempotency và sai lệch so với guardrails kiến trúc.
- Các vùng rủi ro cao nhất:
  - Luồng AI streaming + quota + retry.
  - Luồng ví/tài chính/escrow và transaction isolation.
  - Luồng inventory/gacha/check-in (idempotency và race-condition).
  - Kiến trúc Application đang gọi trực tiếp repository/service thay vì cô lập side-effects qua domain event handlers.
- Số liệu kiểm chứng:
  - `dotnet build TarotNow.slnx`: pass, 0 warning.
  - `dotnet test TarotNow.ArchitectureTests`: pass 26/26.
  - Tuy nhiên kiến trúc strict theo guardrail vẫn đang vi phạm: quét code cho thấy **58 command files** có inject dependency kiểu repository/service/provider trực tiếp.
  - `dotnet list package --outdated`: nhiều package backend chính đang behind đáng kể (ASP.NET/OpenTelemetry/Serilog/Swashbuckle...).

## 2. Review theo từng tính năng / module

### 2.1 Auth / Session / Security
- Chức năng: login, refresh token rotation, revoke session, JWT auth, cookie auth.
- Luồng xử lý chính: `AuthSessionController` -> command handlers -> repository/session store.
- Vấn đề phát hiện:
  - Nhận JWT từ query string cho hub/AI stream (SE-02).
  - Tin cậy `x-forwarded-user-agent` trực tiếp từ request (SE-06).
  - Trả message lỗi runtime/raw message trong một số nhánh SSE/exception mapping (SE-03).
- Bug tiềm ẩn:
  - Rò rỉ token qua URL logs/proxy/history.
  - Fingerprint sai/giả mạo theo header tự khai báo từ client.
- Code thừa/dead code: chưa thấy dead code lớn.
- Nợ kỹ thuật: chuẩn hóa source-of-truth cho client fingerprint và trusted headers chưa đầy đủ.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Bỏ token-from-query cho API stream, chỉ giữ cho SignalR nếu bắt buộc và giới hạn chặt endpoint.
  - Chỉ chấp nhận forwarded UA khi request đi từ trusted proxy/network đã xác thực.

### 2.2 AI Streaming / Quota / Telemetry
- Chức năng: stream AI response, freeze/consume/refund escrow, finalize trạng thái AI request.
- Luồng xử lý chính: `AiController` -> `StreamReadingCommandHandler` -> `OpenAiProvider` -> `CompleteAiStreamCommandHandler`.
- Vấn đề phát hiện:
  - Retry chưa cover HTTP 429/5xx response path (CB-03).
  - Parse chunk SSE không có guard JSON malformed (FB-04).
  - Endpoint stream thiếu idempotency key phía client; mỗi retry tạo request key ngẫu nhiên mới (FB-03).
  - Daily quota đang bỏ qua trạng thái fail-before-first-token (FB-05).
  - Telemetry exception bị nuốt hoàn toàn, mất tín hiệu vận hành (TD-03).
- Bug tiềm ẩn:
  - Mất ổn định stream khi provider trả chunk xấu/không chuẩn.
  - Double charge follow-up nếu client retry ngoài cửa sổ rate-limit.
- Code thừa/dead code: controller chưa đủ "thin", đang chứa nhiều orchestration logic stream/runtime state (CS-02).
- Nợ kỹ thuật: chuẩn retry/failure taxonomy chưa nhất quán toàn luồng AI.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Retry theo status-code policy + jittered backoff.
  - Thêm idempotency key vào API stream.
  - Guard parse chunk JSON và degrade an toàn.

### 2.3 Wallet / Escrow / Finance
- Chức năng: debit/credit/freeze/release/refund/consume, ledger, settlement.
- Luồng xử lý chính: command/domain event -> `WalletRepository` -> ledger + publish money changed events.
- Vấn đề phát hiện:
  - Lock thứ tự payer/receiver theo input, có nguy cơ deadlock A->B/B->A (CB-01).
  - Transaction isolation global `Serializable` cho command pipeline dễ gây contention/retry storm (PF-01).
  - Một số money changed event có `DeltaAmount = 0` gây nhiễu downstream semantics (TD-02 liên quan rule consistency).
- Bug tiềm ẩn:
  - Tắc nghẽn giao dịch giờ cao điểm, tăng timeout/retry ở API.
- Code thừa/dead code: chưa thấy dead code lớn.
- Nợ kỹ thuật: chiến lược lock/order chưa chuẩn hóa theo userId canonical order.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Áp lock ordering theo `min(userId), max(userId)`.
  - Giảm `Serializable` theo flow cần thiết, dùng `ReadCommitted/RepeatableRead` + retry chính xác điểm xung đột.

### 2.4 Check-in / Inventory / Gacha / Gamification
- Chức năng: daily check-in, consume item, free draw credit, gacha pull.
- Luồng xử lý chính: commands/domain events + Mongo/EF mixed persistence.
- Vấn đề phát hiện:
  - Daily check-in race check-then-insert làm request thứ hai trả lỗi dù reward đã xử lý idempotent (CB-04).
  - Non-consumable item vẫn cho `quantity > 1`, effect bị apply lặp (CB-02).
  - AddCredits free draw dùng read-then-insert/update, dễ dính unique race (FB-02).
  - Gacha replay khi operation chưa complete ném `InvalidOperationException` thay vì response deterministic pending (FB-06).
- Bug tiềm ẩn:
  - Bùng phát claim effect/item bất thường và support ticket khó truy dấu.
- Code thừa/dead code: chưa thấy dead code lớn.
- Nợ kỹ thuật: idempotency contract chưa đồng nhất giữa command và domain handlers.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Chốt invariant non-consumable chỉ quantity=1.
  - Upsert/atomic increment cho free draw credits.

### 2.5 Deposit / Webhook / Payment
- Chức năng: tạo lệnh nạp, verify webhook, reconcile trạng thái.
- Luồng xử lý chính: create order event -> PayOS link -> webhook handler -> mark success/fail -> wallet grant.
- Vấn đề phát hiện:
  - Webhook endpoint `AllowAnonymous + DisableRateLimiting` (SE-05).
  - Sinh `PayOsOrderCode` từ `unixMs*1000 + random(0..999)` có nguy cơ collision khi burst đa instance (TD-05).
  - Header idempotency không đồng nhất giữa deposit và các endpoint khác (CS-01).
- Bug tiềm ẩn:
  - Đột biến traffic webhook gây áp lực app mà không có tầng bảo vệ app-level.
- Code thừa/dead code: chưa thấy dead code lớn.
- Nợ kỹ thuật: contract idempotency API chưa thống nhất.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý:
  - Rate-limit theo IP/prefix + WAF, vẫn giữ verify signature.
  - Đổi strategy order code sang sequence/ULID đảm bảo uniqueness mạnh.

### 2.6 Reader / Legal / Profile
- Chức năng: submit reader request, review request, legal consent, profile update.
- Luồng xử lý chính: command -> domain event/repository Mongo/EF.
- Vấn đề phát hiện:
  - Reader request pending duplicate race (CB-05).
  - Mongo reader request update dạng full replace không optimistic concurrency (FB-07).
  - Legal consent idempotency race check-then-insert (FB-01).
  - `DateTimeStyles.AssumeLocal` trong parse PayOS datetime có risk lệch timezone (TD-04).
- Bug tiềm ẩn:
  - Dữ liệu hồ sơ/review bị ghi đè khi thao tác đồng thời.
- Code thừa/dead code: chưa thấy dead code lớn.
- Nợ kỹ thuật: thiếu versioning/concurrency token cho documents quan trọng.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Unique index partial cho pending per user.
  - ReplaceOne với filter version/etag, hoặc chuyển sang update operators.

### 2.7 Community
- Chức năng: create post, add comment, media attach, reaction/report.
- Luồng xử lý chính: create entity -> update counters -> attach media assets.
- Vấn đề phát hiện:
  - Partial write khi attach media fail sau khi entity đã được lưu (FB-08).
- Bug tiềm ẩn:
  - Post/comment tồn tại nhưng media không đồng bộ, counters lệch.
- Code thừa/dead code: logic resolve idempotency bị copy lặp ở nhiều controller (CS-01).
- Nợ kỹ thuật: thiếu outbox/saga cho media attachment consistency.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý:
  - Tách flow thành state machine `pending_media -> completed` hoặc outbox job retry attach.

### 2.8 Background Jobs / Outbox / Moderation
- Chức năng: outbox dispatch, moderation queue/worker, timer services.
- Luồng xử lý chính: claim batch -> process từng message -> publish handlers.
- Vấn đề phát hiện:
  - Outbox process tuần tự từng message trong batch, 1 transaction/message (PF-02).
  - Moderation queue dùng `DropOldest`, mất sự kiện moderation khi quá tải (PF-03).
- Bug tiềm ẩn:
  - Event backlog và mất dữ liệu moderation trong giờ cao điểm.
- Code thừa/dead code:
  - Endpoint `DiagController.Wipe` luôn disabled, tồn tại nhưng không phục vụ use-case thực tế (CS-03).
- Nợ kỹ thuật: chưa có dead-letter/replay riêng cho moderation queue nội bộ.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý:
  - Backpressure + persisted queue cho moderation.
  - Tối ưu outbox throughput theo batch và song song có kiểm soát.

### 2.9 Kiến trúc / Guardrails / Cross-cutting
- Chức năng: đảm bảo clean architecture + event-driven side effects.
- Luồng xử lý chính: commands -> domain events -> handlers.
- Vấn đề phát hiện:
  - Có 58 command files inject repo/service/provider trực tiếp (TD-01), trái guardrail strict đã nêu.
  - Architecture tests hiện hữu chưa đủ chặn vi phạm này (TD-02).
  - AutoMapper được đăng ký toàn cục nhưng usage thực tế rất hẹp (chủ yếu community queries), mapping tay lặp nhiều nơi (CS-04).
- Bug tiềm ẩn:
  - Tăng coupling, khó refactor, dễ phát sinh regression khi module phụ thay đổi.
- Code thừa/dead code: có dấu hiệu over-configuration (tooling có nhưng không dùng nhất quán).
- Nợ kỹ thuật: cao.
- Mức độ ưu tiên: High.
- Đề xuất xử lý:
  - Chuẩn hóa event-first cho write flows, command chỉ validate + emit event.
  - Nâng architecture test để fail ngay khi command inject dependency cấm.

### 2.10 Tests / Dependencies
- Chức năng: quality gate và release confidence.
- Luồng xử lý chính: unit/integration/architecture tests + dependency hygiene.
- Vấn đề phát hiện:
  - Thiếu test cho các vùng concurrency rủi ro cao (daily check-in, legal consent race, reader request pending race, wallet release deadlock) (TG-01).
  - Nhiều package outdated đáng kể; backend đang `net9.0` trong khi môi trường runtime đã `dotnet 10.0.103` (TD-06).
- Bug tiềm ẩn:
  - Regression khó phát hiện sớm trong production.
- Code thừa/dead code: không trọng yếu.
- Nợ kỹ thuật: Medium.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý:
  - Bổ sung test concurrency/integration có chủ đích.
  - Lập kế hoạch upgrade package theo wave có test hồi quy.

## 3. Danh sách issue chi tiết

### Critical bugs

- ID: CB-01
- Module: Wallet / Escrow
- Loại: Critical bugs
- Mô tả: Nguy cơ deadlock khi release escrow do lock 2 user theo thứ tự input (`payer` rồi `receiver`) thay vì thứ tự canonical.
- Tác động: Treo giao dịch, tăng timeout, rollback dây chuyền ở traffic cao.
- Nguyên nhân gốc rễ: Không chuẩn hóa lock ordering cho multi-row lock.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.ReleaseOperations.cs:28-29`
  - `src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.Internal.cs:21-25`
- Cách sửa:
  - Khóa theo thứ tự cố định (sort theo `Guid`), hoặc 1 câu SQL lock cả 2 row theo order.
  - Thêm integration test A->B và B->A chạy song song.
- Priority: High

- ID: CB-02
- Module: Inventory
- Loại: Critical bugs
- Mô tả: Item non-consumable vẫn có thể apply effect nhiều lần bằng `quantity` > 1.
- Tác động: Lạm dụng buff/credit, phá economy và fairness.
- Nguyên nhân gốc rễ: Nhánh non-consumable chỉ check ownership, không ràng buộc quantity-effect parity.
- Bằng chứng trong code:
  - `src/TarotNow.Application/DomainEvents/Handlers/InventoryItemUsedDomainEventHandler.cs:55,79-90`
  - `src/TarotNow.Infrastructure/Persistence/Repositories/UserItemRepository.InventoryConsume.cs:26-29,98-105`
- Cách sửa:
  - Nếu `IsConsumable == false` thì ép `quantityToUse = 1` và reject request quantity>1.
  - Thêm invariant trong domain event handler + validator.
- Priority: High

- ID: CB-03
- Module: AI Provider
- Loại: Critical bugs
- Mô tả: Retry chỉ bắt exception transport; HTTP 429/5xx response không được retry đúng cách.
- Tác động: Tăng fail rate khi provider throttling/tạm lỗi.
- Nguyên nhân gốc rễ: `SendWithRetryAsync` return response ngay; `EnsureSuccessStatusCode` chạy ngoài vòng retry.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.Streaming.cs:22-24,57-87`
- Cách sửa:
  - Retry theo status-code policy (`429`, `5xx`), exponential backoff + jitter.
  - Tách retry policy dùng Polly/ResiliencePipeline.
- Priority: High

- ID: CB-04
- Module: Check-in
- Loại: Critical bugs
- Mô tả: Race check-then-insert: request concurrent có thể trả lỗi duplicate dù reward đã được xử lý idempotent.
- Tác động: User nhận lỗi sai trạng thái, tăng retry và khiếu nại.
- Nguyên nhân gốc rễ: Thiết kế `HasCheckedInAsync` rồi `InsertAsync` không atomic ở Mongo.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs:58-60,79`
  - `src/TarotNow.Infrastructure/Persistence/Repositories/MongoDailyCheckinRepository.cs:57-60`
- Cách sửa:
  - Bỏ pre-check, dùng insert-first với duplicate-key => return idempotent success payload.
  - Hoặc dùng upsert + kiểm tra `modifiedCount/upsertedId`.
- Priority: High

- ID: CB-05
- Module: Reader Request
- Loại: Critical bugs
- Mô tả: Có thể tạo nhiều pending reader request cho cùng user khi concurrent submit.
- Tác động: Queue duyệt admin nhiễu và sai nghiệp vụ one-pending-per-user.
- Nguyên nhân gốc rễ: Pre-check pending + insert tách rời, không có unique constraint cho trạng thái pending.
- Bằng chứng trong code:
  - `src/TarotNow.Application/DomainEvents/Handlers/ReaderRequestSubmitRequestedDomainEventHandler.cs:90-96`
  - `src/TarotNow.Infrastructure/Persistence/Repositories/MongoReaderRequestRepository.cs:24-29`
  - `src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Reader.cs:15-27`
- Cách sửa:
  - Tạo unique partial index theo `(UserId, Status='pending', IsDeleted=false)`.
  - Bắt duplicate key và trả idempotent response.
- Priority: High

### Functional bugs

- ID: FB-01
- Module: Legal Consent
- Loại: Functional bugs
- Mô tả: Idempotency race trong ghi consent: check-then-insert có thể ném unique violation.
- Tác động: Client nhận lỗi dù intent chỉ là ghi nhận consent 1 lần.
- Nguyên nhân gốc rễ: Không xử lý xung đột unique ở nhánh `AddAsync`.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Legal/Commands/RecordConsent/RecordConsentCommandHandler.cs:29-50`
  - `src/TarotNow.Infrastructure/Persistence/Configurations/UserConsentConfiguration.cs:36-38`
  - `src/TarotNow.Infrastructure/Persistence/Repositories/UserConsentRepository.cs:54-58`
- Cách sửa:
  - Chuyển sang insert-first + catch duplicate => success idempotent.
  - Hoặc upsert theo business key.
- Priority: Medium

- ID: FB-02
- Module: Free Draw Credit
- Loại: Functional bugs
- Mô tả: `AddCreditsAsync` dễ đụng race khi 2 request cùng tạo record mới.
- Tác động: 500/unique violation hoặc mất một phần credit grant.
- Nguyên nhân gốc rễ: Read-then-insert/update không atomic.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs:30-45`
  - `src/TarotNow.Infrastructure/Persistence/Configurations/FreeDrawCreditConfiguration.cs:35`
- Cách sửa:
  - Dùng `INSERT ... ON CONFLICT ... DO UPDATE` để cộng dồn atomically.
- Priority: Medium

- ID: FB-03
- Module: AI Stream API
- Loại: Functional bugs
- Mô tả: Endpoint stream không có idempotency key phía client; mỗi request tạo key ngẫu nhiên mới.
- Tác động: Retry từ client/proxy có thể tạo nhiều AI request độc lập, tăng phí ngoài mong muốn.
- Nguyên nhân gốc rễ: Contract API thiếu idempotency cho stream follow-up có charge.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/AiController.cs:47-52`
  - `src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.RequestCreation.cs:31`
- Cách sửa:
  - Bắt buộc `Idempotency-Key` cho stream có charge; dùng key đó cho `AiRequest.IdempotencyKey`.
- Priority: High

- ID: FB-04
- Module: AI Provider Parsing
- Loại: Functional bugs
- Mô tả: Parse JSON chunk SSE không có try/catch, chunk lỗi làm hỏng cả stream.
- Tác động: Mất phiên stream dù chỉ một chunk malformed.
- Nguyên nhân gốc rễ: `JsonNode.Parse(data)` gọi trực tiếp không graceful fallback.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.Streaming.cs:145`
- Cách sửa:
  - Bọc parse bằng try/catch, log warning + skip chunk lỗi.
- Priority: Medium

- ID: FB-05
- Module: AI Quota
- Loại: Functional bugs
- Mô tả: Daily quota không tính `FailedBeforeFirstToken`.
- Tác động: Có thể spam request fail-sớm để vượt intent quota/day.
- Nguyên nhân gốc rễ: Điều kiện count chỉ gồm `Completed` và `FailedAfterFirstToken`.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/Repositories/AiRequestRepository.cs:64-69`
- Cách sửa:
  - Quyết định rõ policy quota theo business (count tất cả attempt hoặc tách soft quota/hard quota).
- Priority: Medium

- ID: FB-06
- Module: Gacha Idempotency
- Loại: Functional bugs
- Mô tả: Replay idempotency khi operation đang xử lý ném `InvalidOperationException`.
- Tác động: Trải nghiệm retry không deterministic; dễ phát sinh lỗi client handling.
- Nguyên nhân gốc rễ: Không có state `ProcessingReplay`/`202 Accepted` cho idempotent in-progress.
- Bằng chứng trong code:
  - `src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.ReplayAndFinalize.cs:17-20`
- Cách sửa:
  - Trả về trạng thái pending có thể poll (`operationId`, `status=processing`) thay vì throw.
- Priority: Medium

- ID: FB-07
- Module: Reader Request Mongo
- Loại: Functional bugs
- Mô tả: `ReplaceOneAsync` full document update không có version guard, dễ lost update.
- Tác động: Admin thao tác đồng thời có thể ghi đè dữ liệu review của nhau.
- Nguyên nhân gốc rễ: Thiếu optimistic concurrency token.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/Repositories/MongoReaderRequestRepository.cs:87-93`
- Cách sửa:
  - Thêm `Version`/`UpdatedAt` filter trong update; nếu mismatch => conflict.
- Priority: Medium

- ID: FB-08
- Module: Community Post/Comment
- Loại: Functional bugs
- Mô tả: Entity được lưu trước, attach media sau; lỗi attach gây trạng thái partial.
- Tác động: Post/comment tồn tại nhưng media không đồng bộ, khó reconcile.
- Nguyên nhân gốc rễ: Flow đa bước không có saga/outbox retry.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Community/Commands/CreatePost/CreatePostCommand.cs:91-107`
  - `src/TarotNow.Application/Features/Community/Commands/AddComment/AddCommentCommand.cs:93-103`
- Cách sửa:
  - Đưa attach media vào outbox/worker retry với trạng thái `pending_media`.
- Priority: Medium

- ID: FB-09
- Module: Reading Session Mongo
- Loại: Functional bugs
- Mô tả: Update followups dạng snapshot ghi đè cả list, dễ mất dữ liệu khi concurrent update.
- Tác động: Mất follow-up answer/question đã ghi trước đó.
- Nguyên nhân gốc rễ: Dùng `Set(r => r.Followups, mappedFollowups)` thay vì append/positional update.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/Repositories/MongoReadingSessionRepository.Commands.cs:72-80`
- Cách sửa:
  - Dùng atomic append (`$push`) hoặc optimistic concurrency trước khi overwrite.
- Priority: High

### Performance issues

- ID: PF-01
- Module: Transaction Coordinator
- Loại: Performance issues
- Mô tả: Toàn bộ command transaction chạy ở `IsolationLevel.Serializable`.
- Tác động: Contention cao, retry nhiều, throughput giảm dưới tải lớn.
- Nguyên nhân gốc rễ: Chọn isolation level cao nhất mặc định cho mọi command.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/TransactionCoordinator.cs:41-43`
- Cách sửa:
  - Phân loại isolation theo use-case; chỉ nâng isolation cho flow thực sự cần.
- Priority: High

- ID: PF-02
- Module: Outbox Worker
- Loại: Performance issues
- Mô tả: Claim batch nhưng process tuần tự từng message với transaction riêng.
- Tác động: Throughput thấp, backlog outbox tăng khi burst event.
- Nguyên nhân gốc rễ: Thiết kế loop one-by-one cố định.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs:52-58`
- Cách sửa:
  - Parallel có giới hạn theo partition key, hoặc commit theo mini-batch.
- Priority: Medium

- ID: PF-03
- Module: Chat Moderation Queue
- Loại: Performance issues
- Mô tả: Queue đầy sẽ `DropOldest`.
- Tác động: Mất event moderation, giảm độ tin cậy kiểm duyệt khi cao tải.
- Nguyên nhân gốc rễ: Ưu tiên non-blocking throughput thay vì durability.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/BackgroundJobs/ChatModerationQueue.cs:30-31,46-52`
- Cách sửa:
  - Chuyển sang persisted queue/outbox moderation hoặc backpressure + metrics/alert.
- Priority: Medium

### Security issues

- ID: SE-01
- Module: Static Files / Uploads
- Loại: Security issues
- Mô tả: Cho phép phục vụ unknown file types từ thư mục uploads.
- Tác động: Mở rộng attack surface cho upload độc hại.
- Nguyên nhân gốc rễ: `ServeUnknownFileTypes = true`.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Startup/ApiApplicationBuilderExtensions.cs:127-133`
- Cách sửa:
  - Set `ServeUnknownFileTypes = false`; allowlist mime/extensions bắt buộc.
- Priority: High

- ID: SE-02
- Module: JWT Authentication
- Loại: Security issues
- Mô tả: Cho phép token từ query string cho hub/AI stream.
- Tác động: Rò token qua logs/proxy/referrer/history.
- Nguyên nhân gốc rễ: Fallback auth token source không đủ hạn chế.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/DependencyInjection.Auth.cs:80-95`
- Cách sửa:
  - Ưu tiên cookie/header; hạn chế query-token chỉ cho SignalR nếu bắt buộc và có hardening.
- Priority: High

- ID: SE-03
- Module: Error Handling / SSE
- Loại: Security issues
- Mô tả: Trả raw error message ở một số nhánh (BadRequest/NotFound/InvalidOperation).
- Tác động: Lộ thông tin nội bộ/luật nghiệp vụ chi tiết.
- Nguyên nhân gốc rễ: Map exception message trực tiếp ra response.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/AiController.Streaming.cs:37-49`
  - `src/TarotNow.Api/Middlewares/GlobalExceptionHandler.Mapping.cs:63-67`
  - `src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.Internal.cs:27`
- Cách sửa:
  - Chuẩn hóa safe error catalog; không trả trực tiếp message internal.
- Priority: Medium

- ID: SE-04
- Module: API Exposure
- Loại: Security issues
- Mô tả: `MapOpenApi()` bật cả production (chỉ tắt UI, không tắt spec endpoint).
- Tác động: Lộ metadata API công khai ngoài ý muốn.
- Nguyên nhân gốc rễ: Gate theo environment chỉ áp cho Swagger UI.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Startup/ApiApplicationBuilderExtensions.cs:77-87`
- Cách sửa:
  - Chỉ map OpenAPI ở dev/internal network hoặc bảo vệ bằng auth.
- Priority: Medium

- ID: SE-05
- Module: Deposit Webhook
- Loại: Security issues
- Mô tả: Webhook endpoint anonymous và tắt rate limiting hoàn toàn.
- Tác động: Tăng nguy cơ flood/DoS tầng app.
- Nguyên nhân gốc rễ: Chỉ dựa vào signature verification, thiếu traffic guard.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/DepositController.Webhook.cs:16-18`
- Cách sửa:
  - Thêm giới hạn theo IP/network, WAF/CDN shield, circuit-breaker theo route.
- Priority: Medium

- ID: SE-06
- Module: Auth Session
- Loại: Security issues
- Mô tả: `x-forwarded-user-agent` được dùng trực tiếp cho fingerprint/hash.
- Tác động: Client có thể spoof header và làm lệch chống gian lận/telemetry.
- Nguyên nhân gốc rễ: Không ràng buộc nguồn header từ trusted proxy.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/AuthSessionController.cs:163-166`
- Cách sửa:
  - Chỉ tin header khi request đi qua known proxy; còn lại dùng `UserAgent` chuẩn.
- Priority: Medium

### Code smell / dead code

- ID: CS-01
- Module: API Idempotency Contract
- Loại: Code smell / dead code
- Mô tả: Không đồng nhất tên header idempotency giữa endpoints, logic resolve lặp nhiều controller.
- Tác động: Dễ sai khi tích hợp client, tăng duplicate code khó bảo trì.
- Nguyên nhân gốc rễ: Thiếu shared abstraction cho idempotency extraction.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Constants/AuthHeaders.cs:11`
  - `src/TarotNow.Api/Controllers/DepositController.Orders.cs:15,138-149`
  - `src/TarotNow.Api/Controllers/WithdrawalController.cs:109-118`
  - `src/TarotNow.Api/Controllers/InventoryController.cs:88`
- Cách sửa:
  - Tạo middleware/helper dùng chung, thống nhất 1 header chuẩn (`Idempotency-Key`).
- Priority: Medium

- ID: CS-02
- Module: AI Controller
- Loại: Code smell / dead code
- Mô tả: Controller đang chứa nhiều orchestration/stream runtime logic, chưa "thin" đúng chuẩn.
- Tác động: Khó test unit, khó tái sử dụng, coupling cao.
- Nguyên nhân gốc rễ: Business flow và transport concerns trộn trong controller.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/AiController.cs:47-79`
  - `src/TarotNow.Api/Controllers/AiController.Streaming.Execution.cs:12-35`
  - `src/TarotNow.Api/Controllers/AiController.Streaming.Completion.cs:21-98`
- Cách sửa:
  - Tách `AiStreamOrchestrator` ở Application/Infrastructure; controller chỉ bind request + return stream.
- Priority: Medium

- ID: CS-03
- Module: Diagnostics
- Loại: Code smell / dead code
- Mô tả: Endpoint `wipe` luôn disabled by default.
- Tác động: Mã dư gây nhiễu bảo trì, tăng bề mặt API nội bộ không dùng.
- Nguyên nhân gốc rễ: Chưa dọn cleanup sau giai đoạn debug.
- Bằng chứng trong code:
  - `src/TarotNow.Api/Controllers/DiagController.cs:57-66`
- Cách sửa:
  - Xóa endpoint nếu không dùng, hoặc chuyển thành tool script riêng ngoài runtime API.
- Priority: Low

- ID: CS-04
- Module: Mapping Layer
- Loại: Code smell / dead code
- Mô tả: AutoMapper được đăng ký toàn cục nhưng sử dụng rất hẹp, phần lớn mapping làm thủ công rải rác.
- Tác động: Mapping logic trùng lặp và dễ drift giữa modules.
- Nguyên nhân gốc rễ: Không có guideline mapping thống nhất toàn codebase.
- Bằng chứng trong code:
  - `src/TarotNow.Application/DependencyInjection.cs:82`
  - `src/TarotNow.Application/Features/Community/Queries/GetFeed/GetFeedQuery.cs:43`
- Cách sửa:
  - Quy định rõ khi nào dùng AutoMapper vs manual mapping; gom mapper profile theo module.
- Priority: Low

### Tech debt

- ID: TD-01
- Module: Application Commands Architecture
- Loại: Tech debt
- Mô tả: Nhiều command handlers inject repository/service/provider trực tiếp, trái strict guardrail event-driven side effects.
- Tác động: Coupling cao, khó thay đổi kiến trúc, khó kiểm soát side-effects tập trung.
- Nguyên nhân gốc rễ: Chưa hoàn tất migration sang mô hình command-publish-event-only.
- Bằng chứng trong code:
  - Quét mã: 58 command files, 133 dòng inject dependency kiểu repo/service/provider.
  - Ví dụ:
    - `src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.cs:12-16`
    - `src/TarotNow.Application/Features/Community/Commands/AddComment/AddCommentCommand.cs:31-34`
    - `src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandHandler.cs:9-14`
- Cách sửa:
  - Từng module chuyển command handler về validate + publish domain events.
  - Đẩy side-effects sang domain event handlers có idempotency.
- Priority: High

- ID: TD-02
- Module: Architecture Tests
- Loại: Tech debt
- Mô tả: Rule test cấm dependency trực tiếp còn hẹp, không chặn phần lớn vi phạm guardrail strict hiện tại.
- Tác động: Quality gate pass nhưng sai chuẩn kiến trúc mong muốn.
- Nguyên nhân gốc rễ: Forbidden patterns chỉ cover vài interface cụ thể.
- Bằng chứng trong code:
  - `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs:19-27`
  - `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs:44`
- Cách sửa:
  - Mở rộng rule: cấm toàn bộ `I*Repository/I*Service/I*Provider` trong command handlers (allowlist rõ ràng nếu có).
- Priority: High

- ID: TD-03
- Module: Observability
- Loại: Tech debt
- Mô tả: Nuốt exception ở telemetry làm mất dấu lỗi vận hành.
- Tác động: Thiếu tín hiệu giám sát, khó điều tra incident.
- Nguyên nhân gốc rễ: Best-effort logging không có fallback log/warn tối thiểu.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.Telemetry.cs:32-35`
  - `src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.WalletAndTelemetry.cs:72-75`
- Cách sửa:
  - Log warning có throttling + metric counter khi telemetry fail.
- Priority: Medium

- ID: TD-04
- Module: PayOS Time Parsing
- Loại: Tech debt
- Mô tả: Parse datetime dùng `DateTimeStyles.AssumeLocal`.
- Tác động: Lệch thời gian khi host timezone khác kỳ vọng, ảnh hưởng reconcile/tracing.
- Nguyên nhân gốc rễ: Không parse theo timezone contract explicit.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Services/PayOsGateway.Helpers.cs:132-143`
- Cách sửa:
  - Parse theo timezone explicit (`AssumeUniversal` hoặc offset từ payload/provider spec).
- Priority: Medium

- ID: TD-05
- Module: Deposit Order Code
- Loại: Tech debt
- Mô tả: Thuật toán order code có entropy thấp ở high throughput multi-instance.
- Tác động: Rủi ro collision -> unique violation khi burst cao.
- Nguyên nhân gốc rễ: Dùng `unixMs * 1000 + random(0..999)`.
- Bằng chứng trong code:
  - `src/TarotNow.Application/DomainEvents/Handlers/DepositOrderCreateRequestedDomainEventHandler.cs:170-174`
  - `src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs:59-61`
- Cách sửa:
  - Dùng sequence từ DB hoặc ULID/KSUID đảm bảo uniqueness mạnh.
- Priority: Low

- ID: TD-06
- Module: Dependencies / Runtime
- Loại: Tech debt
- Mô tả: Nhiều package backend chính đang outdated đáng kể; project target `net9.0`.
- Tác động: Chậm nhận fix hiệu năng/bảo mật, tăng chi phí maintain.
- Nguyên nhân gốc rễ: Chưa có wave upgrade định kỳ.
- Bằng chứng trong code:
  - `src/TarotNow.Api/TarotNow.Api.csproj:4,12-27`
  - `src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj:6,15-34`
  - Kết quả `dotnet list package --outdated` (AspNetCore/OpenTelemetry/Serilog/Swashbuckle đều có bản mới hơn).
- Cách sửa:
  - Lập kế hoạch upgrade theo batch + smoke/regression tests.
- Priority: Medium

### Test gaps

- ID: TG-01
- Module: Concurrency-Critical Flows
- Loại: Test gaps
- Mô tả: Thiếu test chuyên biệt cho các race-condition đã nêu (check-in, consent, reader pending, wallet release lock order).
- Tác động: Rủi ro regression cao và khó phát hiện trước release.
- Nguyên nhân gốc rễ: Coverage hiện tập trung vào happy-path/module phổ biến, thiếu stress/concurrency scenarios.
- Bằng chứng trong code:
  - Có flow source:
    - `src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs`
    - `src/TarotNow.Application/Features/Legal/Commands/RecordConsent/RecordConsentCommandHandler.cs`
    - `src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.ReleaseOperations.cs`
  - Nhưng không có test tương ứng trong `tests/TarotNow.Application.UnitTests/Features/CheckIn/*`, `tests/TarotNow.Application.UnitTests/Features/Legal/*` và không có test deadlock cho wallet release.
- Cách sửa:
  - Bổ sung integration tests song song (2-10 concurrent calls) và assert deterministic outcome.
- Priority: High

## 4. Danh sách refactor đề xuất

### Việc nên làm ngay

1. Chặn rủi ro deadlock wallet release bằng lock ordering canonical (CB-01).
2. Vá inventory non-consumable quantity exploit (CB-02).
3. Sửa retry policy cho OpenAI theo status code + guard parse chunk (CB-03, FB-04).
4. Chuẩn hóa idempotency cho AI stream API (FB-03).
5. Fix race cho daily check-in/reader pending/legal consent/free draw bằng upsert hoặc duplicate-safe path (CB-04, CB-05, FB-01, FB-02).
6. Tắt `ServeUnknownFileTypes` và hạn chế token từ query string (SE-01, SE-02).
7. Bổ sung test concurrency cho 4 flow critical (TG-01).

### Việc có thể trì hoãn (nhưng nên lên roadmap)

1. Refactor command handlers theo strict event-driven architecture (TD-01).
2. Nâng architecture tests để bắt vi phạm guardrail mới (TD-02).
3. Tối ưu outbox throughput và moderation queue durability (PF-02, PF-03).
4. Chuẩn hóa idempotency header + helper dùng chung toàn API (CS-01).
5. Thu gọn AiController theo mô hình thin controller (CS-02).
6. Upgrade dependency wave + runtime alignment (TD-06).
7. Chuẩn hóa datetime parsing theo timezone explicit (TD-04).

## 5. Kết luận

- Đánh giá tổng thể: codebase có nền tảng tốt về tổ chức layer, test/build pipeline và nhiều cơ chế idempotency/outbox đã hiện diện, nhưng còn nhiều điểm **high-risk** trong concurrency và security hardening.
- Mức độ an toàn để release:
  - **Không nên release production rộng** nếu chưa xử lý nhóm Critical/High đã nêu.
  - Có thể release giới hạn nếu buộc phải phát hành, nhưng cần mitigation ngay: giám sát chặt AI error rate, wallet lock timeout, duplicate request metrics và webhook traffic.
- Rủi ro lớn nhất còn tồn tại:
  1. Deadlock/transaction contention ở luồng ví-escrow.
  2. Exploit/abuse ở inventory non-consumable quantity.
  3. Retry/idempotency chưa đủ chặt ở AI stream và một số flow Mongo.
  4. Sai lệch guardrail kiến trúc (command handlers còn side-effect trực tiếp) khiến code khó ổn định dài hạn.

---

## Phần thiếu context (đã review theo code hiện có)

- Chưa có tài liệu business requirement chính thức cho toàn bộ rules (chỉ suy luận từ code), nên một số nhận định quota/billing được đánh giá theo hướng nghiêm ngặt nhất.
- Chưa có benchmark production traffic profile để định lượng chính xác impact của `Serializable`/outbox throughput; nhận định hiện tại dựa trên pattern và implementation code.

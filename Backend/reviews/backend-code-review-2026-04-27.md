## 1. Executive summary

- Phạm vi review đã quét: toàn bộ `Backend/src` + `Backend/tests` (không tính `bin/obj`) với tổng **1309 file C#**.
- Coverage manifest theo module đã lập và dùng để rà không bỏ sót: 23 feature ở Application, 67 domain event handlers, 54 controllers API, 89 repository persistence, 26 background jobs, 65 migration files, 84 test files.
- Baseline kiến trúc hiện tại: architecture tests vẫn pass (`33/33`), nhưng còn nhiều rủi ro production **không bị các test kiến trúc hiện tại bắt được**.
- Mức độ rủi ro tổng thể: **High**.
- Khu vực đáng lo nhất:
  - Tính nhất quán dữ liệu đa kho (PostgreSQL + Mongo) trong Chat/Reading completion.
  - Invariants tài chính/idempotency (Gacha reward, Lucky Star reward, escrow conversation sync).
  - Bảo mật/quyền truy cập (report user tùy ý, presence broadcast toàn cục, lưu bank account plaintext).
  - Một số flow moderation/operations chưa hoàn chỉnh (freeze account path, dead code luồng process deposit).

## 2. Review theo từng tính năng / module

### 2.1 Auth, Mfa, UserContext, Profile, Reader

**Auth / Mfa**
- Chức năng: đăng nhập, refresh, logout, password reset, MFA setup/verify/challenge.
- Luồng chính: API -> Command -> Domain Event -> Handler -> Repository.
- Vấn đề phát hiện:
  - Mapping lỗi toàn cục làm mất chi tiết lỗi nghiệp vụ (`BE-API-001`, `BE-API-002`).
  - Nhiều endpoint gọi `_mediator.Send(...)` không truyền cancellation token (`BE-API-003`).
- Bug tiềm ẩn: disconnect client nhưng backend vẫn chạy hết handler gây tốn tài nguyên.
- Nợ kỹ thuật: chuẩn hóa cancellation chưa đồng đều trên controllers/hubs.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý: chuẩn hóa signature endpoint/hub có `CancellationToken` và truyền xuyên suốt.

**UserContext**
- Chức năng: trả metadata/navbar snapshot.
- Luồng chính: Query -> tổng hợp unread/chat/streak/wallet.
- Vấn đề phát hiện:
  - Cast `long -> int` có nguy cơ overflow (`BE-USERCTX-001`).
  - Thiếu coverage test (`BE-TEST-001`).
- Bug tiềm ẩn: đếm unread lớn bất thường gây giá trị âm/exception trên client contract.
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý: dùng `long` end-to-end hoặc clamp có cảnh báo.

**Profile**
- Chức năng: cập nhật profile, avatar upload confirm.
- Luồng chính: Command -> event handler -> cập nhật User (PG) + ReaderProfile (Mongo).
- Vấn đề phát hiện:
  - Confirm avatar không verify object tồn tại trên R2 trước khi commit URL (`BE-PROFILE-001`).
  - Đồng bộ profile PG -> Mongo không atomic (`BE-PROFILE-002`).
- Bug tiềm ẩn: avatar hỏng, profile lệch giữa 2 datastore.
- Mức độ ưu tiên: High/Medium.
- Đề xuất xử lý: HEAD-check object R2 trước commit; áp dụng outbox/saga cho sync xuyên kho.

**Reader**
- Chức năng: directory/list/filter reader.
- Luồng chính: Query -> Mongo filter/pagination.
- Vấn đề phát hiện:
  - Regex injection/perf risk khi dùng raw searchTerm vào `BsonRegularExpression` (`BE-READER-001`).
  - Thiếu validator cho `ListReadersQuery` (`BE-READER-002`).
- Bug tiềm ẩn: regex pattern độc hại làm query chậm hoặc behavior bất ngờ.
- Mức độ ưu tiên: High/Medium.
- Đề xuất xử lý: `Regex.Escape(searchTerm)` + thêm FluentValidation cho paging/filter.

### 2.2 Chat, Reading, Notification, Presence, AI stream

**Chat + Escrow First Message**
- Chức năng: gửi tin nhắn, freeze escrow tin đầu.
- Luồng chính: validate -> freeze transaction -> insert message/conversation update -> realtime events.
- Vấn đề phát hiện:
  - Freeze tài chính commit trước, insert message Mongo chạy sau -> cross-store non-atomic (`BE-ARCH-001`).
- Bug tiềm ẩn: tiền bị freeze nhưng message không lưu do lỗi Mongo/network.
- Mức độ ưu tiên: Critical.
- Đề xuất xử lý: tách thành saga rõ ràng với trạng thái pending/compensate, không commit tiền trước khi bước Mongo có khả năng rollback/compensate.

**Reading completion / AI stream finalize**
- Chức năng: complete stream, settle ví, cập nhật reading session.
- Luồng chính: transaction coordinator -> update ai request + wallet settle + update reading session.
- Vấn đề phát hiện:
  - Trong cùng flow có cả PG transaction và Mongo update -> không có distributed atomicity (`BE-READING-001`).
  - SSE sanitize chưa xử lý `\r` (`BE-AI-001`).
- Bug tiềm ẩn: partial success giữa billing và session content; SSE frame break edge-case.
- Mức độ ưu tiên: Critical/Medium.
- Đề xuất xử lý: outbox choreography + idempotent compensations; sanitize CRLF đầy đủ.

**Notification**
- Chức năng: tạo/list/mark-read thông báo.
- Vấn đề phát hiện:
  - Dữ liệu đa ngôn ngữ có `Zh` ở create nhưng DTO output không có field Zh (`BE-NOTIF-001`).
- Bug tiềm ẩn: mất dữ liệu hiển thị với locale zh.
- Mức độ ưu tiên: Low.
- Đề xuất xử lý: mở rộng DTO output hoặc bỏ hẳn zh ở create nếu không dùng.

**Presence / Realtime**
- Chức năng: online presence, bridge Redis -> SignalR.
- Vấn đề phát hiện:
  - Presence status broadcast cho `Clients.All` (`BE-PRESENCE-001`).
  - Event name không nhất quán naming convention (`BE-REALTIME-001`).
  - Redis presence set không TTL, dễ stale online (`BE-PRESENCE-002`).
  - Redis publisher không honor cancellation token (`BE-MESSAGING-001`).
- Bug tiềm ẩn: rò trạng thái online toàn hệ thống; user offline nhưng vẫn hiện online; shutdown chậm.
- Mức độ ưu tiên: High/Medium.
- Đề xuất xử lý: broadcast theo scoped groups, thêm TTL/heartbeat eviction, chuẩn hóa event names, propagate cancellation.

### 2.3 Wallet, Deposit, Withdrawal, Escrow, Admin finance/reconciliation

**Wallet / Deposit / Withdrawal**
- Vấn đề phát hiện:
  - Webhook idempotency dựa hash raw payload dễ lệch theo formatting (`BE-DEPOSIT-001`).
  - Gọi `CREATE SEQUENCE IF NOT EXISTS` mỗi lần lấy order code (`BE-DEPOSIT-002`).
  - Thông tin ngân hàng rút tiền lưu plaintext (`BE-WITHDRAWAL-001`).
- Bug tiềm ẩn: duplicate xử lý webhook semantics; overhead DB không cần thiết; rò PII tài chính.
- Mức độ ưu tiên: High/Medium.
- Đề xuất xử lý: canonicalize payload + idempotency theo transaction reference chuẩn; migrate init sequence ra migration; mã hóa/masking bank fields at-rest.

**Escrow timer / auto refund/release/dispute**
- Vấn đề phát hiện:
  - Cập nhật conversation/message sau khi transaction tài chính đã commit (`BE-ESCROW-001`).
- Bug tiềm ẩn: finance đã settle nhưng conversation state không đồng bộ khi bước hậu kỳ lỗi.
- Mức độ ưu tiên: High.
- Đề xuất xử lý: đẩy conversation sync sang outbox event idempotent, có retry riêng và trạng thái reconciliation.

### 2.4 Community, Legal, Report, Home, History

**Community / Moderation**
- Vấn đề phát hiện:
  - `FreezeAccount` được cho phép nhưng không có logic thực thi (`BE-COMMUNITY-001`).
  - Remove post + resolve report không transaction (`BE-COMMUNITY-002`).
  - Counter reaction/comment có thể âm; swap reaction cập nhật hai bước tách rời (`BE-COMMUNITY-003`, `BE-COMMUNITY-004`).
  - Confirm community image không verify object tồn tại (`BE-COMMUNITY-005`).
- Bug tiềm ẩn: moderation “thành công giả”, drift thống kê engagement, gắn URL media hỏng.
- Mức độ ưu tiên: High/Medium.
- Đề xuất xử lý: triển khai lock user thật cho freeze action; transaction/saga cho resolve flow; bounded counters; verify object tồn tại trước confirm.

**Report (chat report)**
- Vấn đề phát hiện:
  - Report target `user` cho phép gửi không cần `conversationRef` -> có thể report user bất kỳ chỉ cần biết userId (`BE-REPORT-001`).
- Mức độ ưu tiên: High.
- Đề xuất xử lý: bắt buộc context ownership (conversation/message) hoặc rule kiểm chứng quan hệ reporter-target.

**History / Home / Legal**
- Vấn đề phát hiện:
  - `GetReadingHistory` và `GetAllReadings` tính `TotalPages` theo `request.PageSize` trực tiếp, không chuẩn hóa ở handler (`BE-HISTORY-001`, `BE-HISTORY-002`).
  - Thiếu test Home/UserContext (`BE-TEST-001`).
- Mức độ ưu tiên: Medium/Low.
- Đề xuất xử lý: normalize paging ngay trong handler/query validator.

### 2.5 Gacha, Gamification, Inventory, Promotions, CheckIn

**Gacha**
- Vấn đề phát hiện:
  - Idempotency key reward currency không chứa roll index -> trùng key khi nhiều roll cùng reward rate (`BE-GACHA-001`).
- Bug tiềm ẩn: under-credit reward trong pull nhiều lượt.
- Mức độ ưu tiên: Critical.
- Đề xuất xử lý: thêm `rollIndex` hoặc reward log id vào idempotency key.

**Inventory**
- Vấn đề phát hiện:
  - LuckyStar bonus credit dùng `idempotencyKey: null` (`BE-INVENTORY-001`).
- Bug tiềm ẩn: retry event có thể credit trùng khi fail sau bước credit.
- Mức độ ưu tiên: High.
- Đề xuất xử lý: idempotency key ổn định theo event/op id.

**CheckIn**
- Vấn đề phát hiện:
  - Purchase freeze ném `InvalidOperationException` cho rule nghiệp vụ, không map thành 4xx trong global handler (`BE-CHECKIN-001`).
- Mức độ ưu tiên: Medium.
- Đề xuất xử lý: dùng `BusinessRuleException/BadRequestException` cho business path.

**Promotions**
- Vấn đề phát hiện:
  - Thiếu test cho update/delete promotion (`BE-TEST-002`).
- Mức độ ưu tiên: Low.
- Đề xuất xử lý: thêm integration + unit tests cho nhánh không tìm thấy, nhánh conflict.

### 2.6 Admin còn lại + Cross-cutting infra (Outbox, BackgroundJobs, Messaging, Realtime bridge, Security, Config, Migrations, Tests)

- Vấn đề phát hiện:
  - Dead code luồng process deposit cũ vẫn tồn tại (`BE-ADMIN-001`).
  - `LeaderboardSnapshotJob` dùng `Guid.Parse` không guard (`BE-BGJOBS-001`).
  - `StreakBreakBackgroundJob` N+1 query + save từng user (`BE-BGJOBS-002`).
  - Upsert system config không atomic với projection runtime (`BE-CONFIG-001`).
  - Nhiều controller/hub không truyền cancellation token (`BE-API-003`).
  - Test gaps ở các module runtime-critical (`BE-TEST-001`, `BE-TEST-002`).
- Mức độ ưu tiên: High/Medium.
- Đề xuất xử lý: dọn dead code, harden jobs, tách config-write/projection transactionally hoặc outbox, bổ sung test coverage và cancellation propagation.

## 3. Danh sách issue chi tiết

- ID: BE-ARCH-001
- Module: Chat / Escrow Freeze
- Loại: Critical bugs
- Mô tả: Luồng freeze tài chính commit trước, sau đó mới ghi message Mongo; không có atomicity xuyên kho.
- Tác động: Có thể freeze tiền thành công nhưng message/conversation không ghi được, gây kẹt tiền và lệch nghiệp vụ.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.cs:45-47 (TryFreezeMainQuestionOnFirstUserMessageAsync)`, `Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.cs:77 (Handle)`, `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.cs:11`, `Backend/src/TarotNow.Infrastructure/Repositories/ChatFinanceRepository.cs:10`, `Backend/src/TarotNow.Infrastructure/Persistence/TransactionCoordinator.cs:12`.
- Nguyên nhân gốc rễ: Transaction coordinator chỉ bao phủ `ApplicationDbContext` (PG), trong khi message/conversation ở Mongo.
- Cách sửa: Chuyển freeze sang saga/outbox step sau khi message insert acknowledged, hoặc dùng state machine có compensation (auto-unfreeze) khi fail sau PG commit.
- Priority: High

- ID: BE-READING-001
- Module: Reading / CompleteAiStream
- Loại: Critical bugs
- Mô tả: Completion flow chạy settlement ví + cập nhật AI request trong transaction PG nhưng đồng thời cập nhật ReadingSession trên Mongo.
- Tác động: Partial success giữa billing và session content, khó reconcile khi retry/fail.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.cs:58-60`, `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.Processing.cs:31`, `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.SessionPersistence.cs:35`, `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReadingSessionRepository.Commands.cs:54-81`, `Backend/src/TarotNow.Infrastructure/Persistence/TransactionCoordinator.cs:12`.
- Nguyên nhân gốc rễ: Không có distributed transaction/outbox choreography cho luồng đa datastore.
- Cách sửa: Tách write-model settle (PG) và session-content update (Mongo) thành event-driven steps có idempotency + retry + reconciliation marker.
- Priority: High

- ID: BE-PRESENCE-001
- Module: Presence / Realtime Bridge
- Loại: Security issues
- Mô tả: Event trạng thái user được broadcast `Clients.All`.
- Tác động: Lộ trạng thái online/offline cho toàn bộ client kết nối.
- Bằng chứng trong code: `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.Forwarding.cs:55 (ForwardPresenceEventAsync)`.
- Nguyên nhân gốc rễ: Thiết kế broadcast không phân vùng theo quan hệ/authorization scope.
- Cách sửa: Chỉ broadcast theo nhóm liên quan (`user:{id}`, contacts, conversation participants) và enforce authorization tại hub group membership.
- Priority: High

- ID: BE-REALTIME-001
- Module: Realtime Contracts
- Loại: Tech debt
- Mô tả: Event name `UserStatusChanged` dùng PascalCase, lệch convention snake/dot đang dùng toàn hệ thống.
- Tác động: Tăng rủi ro client contract mismatch và lỗi subscribe khó phát hiện.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Common/Realtime/RealtimeEventNames.cs:91`.
- Nguyên nhân gốc rễ: Chuẩn naming không được enforce bằng test contract.
- Cách sửa: Chuẩn hóa thành `user.status_changed` (hoặc chuẩn chung), thêm contract test cho naming format.
- Priority: Medium

- ID: BE-MESSAGING-001
- Module: Redis Publisher
- Loại: Tech debt
- Mô tả: `PublishAsync` nhận `CancellationToken` nhưng không dùng token khi publish.
- Tác động: Shutdown/chuyển trạng thái hủy request không cắt được publish path, tăng latency và work thừa.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/Messaging/Redis/RedisPublisher.cs:31-52`.
- Nguyên nhân gốc rễ: API StackExchange.Redis publish không được bọc với check cancellation trước/giữa bước.
- Cách sửa: Check `cancellationToken.ThrowIfCancellationRequested()` trước publish; nếu cần, bọc timeout/cancel logic ở caller.
- Priority: Low

- ID: BE-AI-001
- Module: AI Stream SSE
- Loại: Security issues
- Mô tả: SSE chỉ sanitize `\n`, chưa sanitize `\r`.
- Tác động: Edge case payload có CR có thể làm hỏng framing SSE.
- Bằng chứng trong code: `Backend/src/TarotNow.Api/Services/AiStreamSseOrchestrator.Streaming.cs:81-95`.
- Nguyên nhân gốc rễ: Sanitization chưa đầy đủ cho CRLF framing.
- Cách sửa: Escape cả `\r` và `\n`, hoặc encode payload JSON rồi truyền qua `data: <json>`.
- Priority: Medium

- ID: BE-API-001
- Module: Global Exception Mapping
- Loại: Functional bugs
- Mô tả: BadRequest/NotFound/ArgumentException bị map về detail generic, mất message nghiệp vụ cụ thể.
- Tác động: Client khó hiển thị lỗi đúng ngữ cảnh, giảm khả năng debug vận hành.
- Bằng chứng trong code: `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.Mapping.cs:39-63`, `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.ProblemDetailsFactories.cs:11-27`.
- Nguyên nhân gốc rễ: Factory cố định detail text, bỏ qua message exception.
- Cách sửa: Cho phép pass-through message có whitelist/an toàn hoặc map theo error-code chi tiết hơn.
- Priority: Medium

- ID: BE-API-002
- Module: Global Exception Logging
- Loại: Tech debt
- Mô tả: Mọi exception vào global handler đều log `Error` với full exception.
- Tác động: Noise log cao cho lỗi expected (validation/business), giảm signal của incident thật.
- Bằng chứng trong code: `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.cs:30`.
- Nguyên nhân gốc rễ: Chưa phân tầng log level theo loại exception.
- Cách sửa: Validation/Business -> Warning/Information; system/unexpected -> Error.
- Priority: Low

- ID: BE-PRESENCE-002
- Module: Presence Tracker (Redis)
- Loại: Functional bugs
- Mô tả: Online check ưu tiên `SetLength > 0`; key set connections không TTL.
- Tác động: Missed disconnect có thể giữ user online vô thời hạn.
- Bằng chứng trong code: `Backend/src/TarotNow.Api/Realtime/RedisUserPresenceTracker.cs:42`, `:86-89`, `:172-175`.
- Nguyên nhân gốc rễ: Không có cơ chế expiry/cleanup cưỡng bức cho connection set stale.
- Cách sửa: Thêm TTL/lease cho connection keys, heartbeat refresh TTL, fallback cleanup định kỳ mạnh hơn.
- Priority: High

- ID: BE-READER-001
- Module: Reader Directory
- Loại: Security issues
- Mô tả: Search term được đưa trực tiếp vào regex Mongo.
- Tác động: Regex DoS/perf degradation với pattern xấu, behavior search khó kiểm soát.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReaderProfileRepository.Pagination.Filters.cs:73-74`.
- Nguyên nhân gốc rễ: Không escape input regex.
- Cách sửa: `Regex.Escape(searchTerm)` hoặc chuyển sang full-text/indexed prefix search.
- Priority: High

- ID: BE-READER-002
- Module: Reader Queries
- Loại: Tech debt
- Mô tả: `ListReadersQuery` không có FluentValidation validator.
- Tác động: Validation phân tán xuống repository/controller, khó duy trì nhất quán rule.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Reader/Queries/ListReaders/ListReadersQuery.cs:9-25`; thư mục `ListReaders` chỉ có `ListReadersQuery.cs` và `ListReadersQueryHandler.cs`.
- Nguyên nhân gốc rễ: CQRS query chưa chuẩn hóa validation layer.
- Cách sửa: thêm `ListReadersQueryValidator` cho paging/filter/search.
- Priority: Medium

- ID: BE-PROFILE-001
- Module: Profile Avatar
- Loại: Functional bugs
- Mô tả: Confirm avatar không kiểm tra object tồn tại thực tế trên R2 trước khi lưu URL.
- Tác động: Có thể persist avatar URL hỏng/404.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Profile/Commands/ConfirmAvatarUpload/ConfirmAvatarUploadCommand.cs:67-90`.
- Nguyên nhân gốc rễ: Chỉ verify upload session token, không verify object existence.
- Cách sửa: thêm HEAD/check object trước `ApplyManagedAvatar`.
- Priority: Medium

- ID: BE-PROFILE-002
- Module: Profile Sync
- Loại: Tech debt
- Mô tả: Update User (PG) rồi sync ReaderProfile (Mongo) tuần tự, không có cơ chế atomic/reconcile.
- Tác động: Dễ lệch dữ liệu profile giữa 2 nguồn khi lỗi ở bước Mongo.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/DomainEvents/Handlers/UserProfileUpdateRequestedDomainEventHandler.cs:58-60`, `:151-162`.
- Nguyên nhân gốc rễ: Đồng bộ xuyên datastore không qua outbox/saga.
- Cách sửa: phát event profile-changed + consumer Mongo idempotent + job reconcile định kỳ.
- Priority: Medium

- ID: BE-API-003
- Module: API/Hubs Cross-cutting
- Loại: Refactoring suggestions
- Mô tả: Có nhiều call `_mediator.Send(...)` không truyền cancellation token.
- Tác động: Client đã disconnect nhưng backend tiếp tục xử lý đầy đủ; tăng tải.
- Bằng chứng trong code: `Backend/src/TarotNow.Api/Hubs/ChatHub.Messages.Send.cs:47`, `Backend/src/TarotNow.Api/Controllers/UserContextController.cs:46`, `Backend/src/TarotNow.Api/Controllers/PromotionsController.cs:37`; truy vấn tĩnh ghi nhận **86** vị trí tương tự trong Controllers/Hubs.
- Nguyên nhân gốc rễ: Chuẩn coding về cancellation propagation chưa được enforce.
- Cách sửa: chuẩn hóa bắt buộc `CancellationToken` tại endpoints/hubs và truyền xuyên suốt đến MediatR/repository.
- Priority: Medium

- ID: BE-DEPOSIT-001
- Module: Deposit Webhook
- Loại: Functional bugs
- Mô tả: Idempotency key webhook lấy từ hash raw payload đã trim.
- Tác động: Cùng event semantics nhưng khác formatting JSON có thể sinh key khác.
- Bằng chứng trong code: `Backend/src/TarotNow.Domain/Events/DepositWebhookReceivedDomainEvent.cs:29-33`.
- Nguyên nhân gốc rễ: Idempotency dựa raw string thay vì canonical transaction identity.
- Cách sửa: dùng khóa chuẩn từ provider (`orderCode` + `transactionId` + event type), canonicalize JSON trước hash nếu vẫn cần hash.
- Priority: Medium

- ID: BE-DEPOSIT-002
- Module: Deposit Repository
- Loại: Performance issues
- Mô tả: Mỗi lần lấy `PayOsOrderCode` đều chạy `CREATE SEQUENCE IF NOT EXISTS`.
- Tác động: Tăng round-trip DB không cần thiết ở path nóng tạo order.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/DepositOrderRepository.cs:56-58`.
- Nguyên nhân gốc rễ: DDL initialization đặt trong runtime path thay vì migration/bootstrap.
- Cách sửa: chuyển tạo sequence vào migration; runtime chỉ `nextval`.
- Priority: Low

- ID: BE-WITHDRAWAL-001
- Module: Withdrawal
- Loại: Security issues
- Mô tả: Tên/Số tài khoản ngân hàng lưu plaintext.
- Tác động: Rủi ro lộ PII tài chính khi DB dump/log/insider access.
- Bằng chứng trong code: `Backend/src/TarotNow.Domain/Entities/WithdrawalRequest.cs:35-38`, `Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs:40-41`.
- Nguyên nhân gốc rễ: Chưa có field-level encryption/tokenization cho dữ liệu nhạy cảm.
- Cách sửa: mã hóa at-rest (application-level encryption hoặc pgcrypto), chỉ lưu masked value cho read paths.
- Priority: High

- ID: BE-ESCROW-001
- Module: Escrow Timer
- Loại: Critical bugs
- Mô tả: Sau khi transaction tài chính kết thúc, mới cập nhật conversation/message ngoài transaction.
- Tác động: Settlement thành công nhưng conversation state không đổi khi bước hậu kỳ lỗi.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoReleases.cs:51-79` và update conversation ở `:83-88`; `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.cs:51-59`; `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs:52-87` và `:91-96`.
- Nguyên nhân gốc rễ: Side-effects conversation chạy ngoài boundary transaction tài chính.
- Cách sửa: phát outbox events cho conversation sync, retry độc lập, và reconciliation status.
- Priority: High

- ID: BE-USERCTX-001
- Module: UserContext
- Loại: Functional bugs
- Mô tả: Ép kiểu unread notification từ `long` về `int`.
- Tác động: Overflow khi số lượng lớn bất thường, trả giá trị sai.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/UserContext/Queries/GetInitialMetadata/GetInitialMetadataQueryHandler.cs:74`, `Backend/src/TarotNow.Application/Features/UserContext/Queries/GetNavbarSnapshot/GetNavbarSnapshotQueryHandler.cs:48`.
- Nguyên nhân gốc rễ: DTO dùng int trong khi repository trả long.
- Cách sửa: dùng `long` end-to-end hoặc clamp + metric cảnh báo overflow.
- Priority: Medium

- ID: BE-GACHA-001
- Module: Gacha
- Loại: Critical bugs
- Mô tả: Idempotency key reward currency không có roll index; nhiều roll cùng reward rate trong 1 operation sẽ trùng key.
- Tác động: Có thể chỉ credit 1 lần dù trúng cùng reward nhiều lần.
- Bằng chứng trong code: loop nhiều roll `Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Rewards.cs:19-31`, key hiện tại `:76`.
- Nguyên nhân gốc rễ: Thiết kế idempotency key theo `operationId + rewardRateId` thiếu entropy theo từng roll.
- Cách sửa: thêm `rollIndex`/rewardLogId vào key (`..._{operationId}_{rewardRateId}_{index}`).
- Priority: High

- ID: BE-CHECKIN-001
- Module: CheckIn
- Loại: Functional bugs
- Mô tả: Rule nghiệp vụ purchase freeze ném `InvalidOperationException`.
- Tác động: Global handler không map loại này => dễ thành 500 thay vì 4xx business error.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/CheckIn/Commands/PurchaseFreeze/PurchaseStreakFreezeCommandHandler.cs:118,133,144`; global mapping không có InvalidOperationException `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.Mapping.cs:31-79`.
- Nguyên nhân gốc rễ: Dùng exception type không phù hợp cho business rule.
- Cách sửa: đổi sang `BusinessRuleException` hoặc `BadRequestException` + error code rõ ràng.
- Priority: Medium

- ID: BE-COMMUNITY-001
- Module: Community Moderation
- Loại: Functional bugs
- Mô tả: `FreezeAccount` có trong allow-list nhưng handler không thực thi khóa tài khoản.
- Tác động: Admin chọn freeze nhưng hệ thống chỉ mark report resolved, không có hiệu lực moderation.
- Bằng chứng trong code: allow-list `Backend/src/TarotNow.Application/Features/Community/Commands/ResolvePostReport/ResolvePostReportCommand.cs:110-113`; logic Handle chỉ xử lý `RemovePost` `:71-75`.
- Nguyên nhân gốc rễ: Nhánh action chưa được implement.
- Cách sửa: thêm branch gọi user lock service/command + audit trail; bổ sung test bắt buộc cho branch này.
- Priority: High

- ID: BE-COMMUNITY-002
- Module: Community Moderation
- Loại: Critical bugs
- Mô tả: Remove post và resolve report chạy tách rời, không transaction.
- Tác động: Có thể xóa post nhưng không resolve report (hoặc ngược lại), tạo trạng thái moderation lệch.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Community/Commands/ResolvePostReport/ResolvePostReportCommand.cs:71-83`.
- Nguyên nhân gốc rễ: Không có transaction coordinator/saga cho hai bước.
- Cách sửa: bọc 2 bước trong transaction nếu cùng datastore; nếu khác datastore thì saga + compensation.
- Priority: High

- ID: BE-COMMUNITY-003
- Module: Community Engagement
- Loại: Functional bugs
- Mô tả: Counter reaction/comment dùng `$inc` trực tiếp, không chặn dưới 0.
- Tác động: Drift thống kê âm khi remove/swap race hoặc dữ liệu lệch.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/CommunityPostRepository.Engagement.cs:19-20,33`.
- Nguyên nhân gốc rễ: Thiếu guard condition/canonical recount path.
- Cách sửa: dùng pipeline update với `max(0, value+delta)` hoặc scheduled recount reconciliation.
- Priority: Medium

- ID: BE-COMMUNITY-004
- Module: Community Reactions
- Loại: Functional bugs
- Mô tả: Swap reaction update count cũ và mới bằng 2 lệnh tách rời.
- Tác động: Lỗi giữa chừng gây lệch total và per-type counts.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Community/Commands/ToggleReaction/ToggleReactionCommand.cs:143-144`.
- Nguyên nhân gốc rễ: Không có atomic batch update cho cặp decrement/increment.
- Cách sửa: gom update count vào 1 operation atomic ở repository hoặc dùng transaction session Mongo.
- Priority: Medium

- ID: BE-COMMUNITY-005
- Module: Community Media Upload
- Loại: Functional bugs
- Mô tả: Confirm community image chỉ check upload session, không verify object tồn tại trên R2.
- Tác động: Persist asset uploaded dù object đã bị mất/chưa upload thật.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Community/Commands/ConfirmCommunityImage/ConfirmCommunityImageCommand.cs:66-96`.
- Nguyên nhân gốc rễ: Thiếu check object existence trước `UpsertUploadedAsync`.
- Cách sửa: thêm check HEAD/object metadata trước consume token hoặc trước upsert asset.
- Priority: Medium

- ID: BE-HISTORY-001
- Module: History Query
- Loại: Functional bugs
- Mô tả: `TotalPages` tính từ `request.PageSize` chưa normalize ở handler.
- Tác động: Khi query handler bị gọi từ context khác không normalize có thể chia cho 0.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/History/Queries/GetReadingHistory/GetReadingHistoryQuery.cs:102`.
- Nguyên nhân gốc rễ: Tin tưởng input từ caller bên ngoài handler.
- Cách sửa: normalize `page/pageSize` ngay trong handler hoặc bắt buộc validator.
- Priority: Low

- ID: BE-HISTORY-002
- Module: History Admin Query
- Loại: Functional bugs
- Mô tả: `GetAllReadingsResponse.TotalPages` dùng trực tiếp `request.PageSize`.
- Tác động: Cùng rủi ro robustness khi dùng ngoài controller path.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/History/Queries/GetAllReadings/GetAllReadingsQueryHandler.Mapping.cs:121`.
- Nguyên nhân gốc rễ: Logic response không dùng normalized page size.
- Cách sửa: normalize từ đầu handler và truyền giá trị normalized xuống BuildResponse.
- Priority: Low

- ID: BE-REPORT-001
- Module: Report (Chat)
- Loại: Security issues
- Mô tả: Report target `user` cho phép `conversationRef` rỗng, không verify ownership/quan hệ.
- Tác động: User có thể report tùy ý bất kỳ userId tồn tại.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Chat/Commands/CreateReport/CreateReportCommand.cs:186-189`.
- Nguyên nhân gốc rễ: Rule authz cho target `user` phụ thuộc vào `conversationRef` tùy chọn.
- Cách sửa: bắt buộc context chứng minh tương tác hợp lệ (conversation/message) hoặc verify relationship riêng.
- Priority: High

- ID: BE-INVENTORY-001
- Module: Inventory / LuckyStar
- Loại: Critical bugs
- Mô tả: Credit bonus gold dùng `idempotencyKey: null`.
- Tác động: Retry handler có thể credit lặp.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryLuckyStarTitleUsedDomainEventHandler.cs:73`; cơ chế wallet không dedupe khi key null `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.cs:59-70`.
- Nguyên nhân gốc rễ: Bỏ idempotency key ở flow tài chính có thể retry.
- Cách sửa: key ổn định theo `eventIdempotencyKey` hoặc `sourceItemCode + userId + occurredAt bucket`.
- Priority: High

- ID: BE-BGJOBS-001
- Module: Leaderboard Snapshot Job
- Loại: Functional bugs
- Mô tả: Dùng `Guid.Parse` trực tiếp trên `entry.UserId`.
- Tác động: Một record dữ liệu xấu có thể làm fail toàn bộ cycle snapshot.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs:126,131`.
- Nguyên nhân gốc rễ: Thiếu guard parse (`TryParse`) và skip invalid rows.
- Cách sửa: dùng `Guid.TryParse`, log warning và bỏ qua entry lỗi.
- Priority: Medium

- ID: BE-BGJOBS-002
- Module: Streak Break Job
- Loại: Performance issues
- Mô tả: N+1 load user + `SaveChanges` từng dòng.
- Tác động: Scale kém khi user lớn, tăng lock/churn DB.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs:78-81`, `:100`, `:104`.
- Nguyên nhân gốc rễ: Thiết kế xử lý row-by-row thay vì batch update.
- Cách sửa: update batch theo điều kiện SQL, commit theo chunk.
- Priority: Medium

- ID: BE-NOTIF-001
- Module: Notification
- Loại: Functional bugs
- Mô tả: Input tạo notification có `TitleZh/BodyZh` nhưng output DTO không có field zh.
- Tác động: Mất khả năng hiển thị locale zh ở read path.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Interfaces/INotificationRepository.cs:57,66,76-104`, map output `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoNotificationRepository.cs:96-103`.
- Nguyên nhân gốc rễ: Contract DTO bất đối xứng create/read.
- Cách sửa: thêm `TitleZh/BodyZh` vào DTO read hoặc đồng nhất bỏ zh ở create.
- Priority: Low

- ID: BE-ADMIN-001
- Module: Admin Deposit
- Loại: Code smell / dead code
- Mô tả: `ProcessDepositCommandHandler` tồn tại nhưng luôn trả false; endpoint thực tế đã disable 410.
- Tác động: Dead path gây nhiễu maintainers và test surface.
- Bằng chứng trong code: `Backend/src/TarotNow.Application/Features/Admin/Commands/ProcessDeposit/ProcessDepositCommand.cs:29-33`; endpoint disable `Backend/src/TarotNow.Api/Controllers/AdminDepositsController.cs:46-53`.
- Nguyên nhân gốc rễ: Refactor sang webhook chưa dọn sạch command cũ.
- Cách sửa: xóa command/validator/handler cũ hoặc đánh dấu obsolete + remove registration.
- Priority: Low

- ID: BE-CONFIG-001
- Module: System Config
- Loại: Tech debt
- Mô tả: Upsert DB thành công trước, sau đó mới refresh projection; projection fail sẽ để DB và runtime snapshot lệch nhau.
- Tác động: Runtime behavior không phản ánh đúng giá trị vừa update dù DB đã ghi.
- Bằng chứng trong code: `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigAdminService.cs:85-94`, projection step `:97-116`.
- Nguyên nhân gốc rễ: Không có transaction boundary chung cho write DB + apply projection.
- Cách sửa: dùng outbox/projector async có trạng thái apply, hoặc rollback-safe transactional approach cho config apply.
- Priority: Medium

- ID: BE-TEST-001
- Module: Test Coverage
- Loại: Test gaps
- Mô tả: Không tìm thấy test cho UserContext và Home snapshot flow.
- Tác động: Regression ở metadata/navbar/home khó được phát hiện sớm.
- Bằng chứng trong code: truy vấn test source `rg -n "UserContext|GetInitialMetadata|GetNavbarSnapshot" Backend/tests` và `rg -n "GetHomeSnapshot|HomeSnapshot" Backend/tests` trả rỗng.
- Nguyên nhân gốc rễ: Coverage tập trung vào finance/chat/auth, bỏ sót context APIs.
- Cách sửa: thêm unit/integration tests cho `UserContextController`, `GetInitialMetadataQueryHandler`, `GetNavbarSnapshotQueryHandler`, `HomeController`.
- Priority: Medium

- ID: BE-TEST-002
- Module: Test Coverage
- Loại: Test gaps
- Mô tả: Không có test cho nhánh moderation `freeze_account` và không có test update/delete promotions.
- Tác động: Các nhánh lỗi/chưa hoàn thiện dễ lọt production.
- Bằng chứng trong code: `rg -n "FreezeAccount|freeze_account|ResolvePostReport" Backend/tests` trả rỗng; `rg -n "UpdatePromotionCommand|DeletePromotionCommand|UpdatePromotion|DeletePromotion" Backend/tests` trả rỗng.
- Nguyên nhân gốc rễ: Thiếu test theo negative/edge branches của admin flows.
- Cách sửa: thêm tests cho `ResolvePostReport` mọi action; thêm integration tests PUT/DELETE `/admin/promotions`.
- Priority: Medium

## 4. Danh sách refactor đề xuất

### Việc nên làm ngay

1. Chặn release: xử lý các lỗi Critical tài chính/dữ liệu trước khi deploy.
2. Sửa `BE-GACHA-001` và `BE-INVENTORY-001` để đảm bảo idempotency tài chính.
3. Sửa `BE-ARCH-001`, `BE-READING-001`, `BE-ESCROW-001` theo hướng saga/outbox + reconciliation.
4. Triển khai thực thi thật cho action `freeze_account` (`BE-COMMUNITY-001`) và thêm test.
5. Khóa bảo mật dữ liệu ngân hàng bằng encryption/masking (`BE-WITHDRAWAL-001`).
6. Chặn abuse report user không context (`BE-REPORT-001`).

### Việc có thể trì hoãn (nhưng cần ticket rõ ràng)

1. Chuẩn hóa cancellation token propagation (`BE-API-003`).
2. Chuẩn hóa contract realtime naming + publish cancellation (`BE-REALTIME-001`, `BE-MESSAGING-001`).
3. Dọn dead code process deposit cũ (`BE-ADMIN-001`).
4. Tối ưu background jobs (`BE-BGJOBS-001`, `BE-BGJOBS-002`).
5. Cân bằng lại DTO notification đa ngôn ngữ (`BE-NOTIF-001`).
6. Nâng coverage test cho UserContext/Home/Promotions/Moderation (`BE-TEST-001`, `BE-TEST-002`).

## 5. Kết luận

- Đánh giá tổng thể backend: **chưa an toàn để release production** nếu chưa xử lý nhóm lỗi Critical liên quan tài chính và nhất quán dữ liệu.
- Các rủi ro lớn nhất còn tồn tại:
  - Double-credit/under-credit do idempotency key thiết kế chưa đúng ở một số flow.
  - Partial success giữa PG và Mongo trong các luồng chat/reading/escrow.
  - Một số nhánh moderation/security chưa khép kín.
- Khuyến nghị release readiness:
  - Chỉ xem xét release sau khi đóng tối thiểu các issue: `BE-ARCH-001`, `BE-READING-001`, `BE-GACHA-001`, `BE-INVENTORY-001`, `BE-COMMUNITY-001`, `BE-ESCROW-001`, `BE-WITHDRAWAL-001`, `BE-REPORT-001`.

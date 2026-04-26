# BACKEND_REVIEW_REPORT

## 1. Executive summary

- Mức rủi ro tổng thể: **High**.
- Khuyến nghị release: **chưa nên release production** trước khi xử lý các issue `High` liên quan money flow, outbox ordering, escrow timeout và quota AI.
- Khu vực rủi ro lớn nhất:
  - `Outbox processing` (partition key sai -> nguy cơ xử lý song song sai thứ tự theo business key).
  - `Flow tiền trong chat/escrow` (nhiều điểm cross-store không atomic tuyệt đối giữa PostgreSQL và MongoDB).
  - `AI stream quota` (check quota/in-flight chưa atomic -> race condition).
  - `Security hardening` cho endpoint nhạy cảm (rate limit/policy chưa tương xứng).

### Baseline kỹ thuật đã khóa

- Architecture tests: `TarotNow.ArchitectureTests` **pass 26/26**.
- Unit/Integration tests đã chạy:
  - `TarotNow.Application.UnitTests` pass.
  - `TarotNow.Domain.UnitTests` pass.
  - `TarotNow.Infrastructure.UnitTests` pass.
  - `TarotNow.Infrastructure.IntegrationTests` pass.
  - `TarotNow.Api.IntegrationTests` pass `33/33` (chạy lại thành công).

### Delta update theo pass

- Delta #1 (Pass A - Auth/Money/Escrow/Reading/AI/Conversation):
  - Issue mới: `Critical 4`, `Functional 5`, `Security 1`, `Code smell 1`, `Test gaps 2`.
  - High: `CRIT-002`, `CRIT-003`, `CRIT-004`, `CRIT-005`, `FUNC-001`, `FUNC-002`, `FUNC-003`.
  - Module cover: `Auth, Mfa, Wallet, Deposit, Withdrawal, Escrow, Reading, AI, Conversation`.
  - Còn lại: API boundary sâu, domain khác, infra sâu, test-gap map.

- Delta #2 (Pass B - Controllers/Middlewares/Hubs/Startup/Security):
  - Issue mới: `Security 3`, `Functional 1`, `Code smell 1`, `Tech debt 1`, `Test gaps 1`.
  - High: `SEC-001`, `SEC-002`, `SEC-003`, `FUNC-004`.
  - Module cover: `Controllers, Middlewares, Hubs, Startup, Auth cookie/rate-limit/policy`.
  - Còn lại: feature domain còn lại + infra sâu.

- Delta #3 (Pass C - Chat/Community/Notification/Presence/History/Gamification/Gacha/Inventory/CheckIn/Promotions/Profile/Home/Legal/Admin):
  - Issue mới: `Functional 1`, `Security 1`, `Code smell 1`, `Tech debt 1`, `Test gaps 1`.
  - High: `FUNC-005`, `SEC-004`.
  - Module cover: toàn bộ domain features còn lại ở mức module-level + review sâu các flow có side-effects.
  - Còn lại: infra/outbox/concurrency final + normalize report.

- Delta #4 (Pass D - Persistence/Repositories/Services/Messaging/BackgroundJobs/Migrations/Options/Security):
  - Issue mới: `Critical 1`, `Performance 3`, `Code smell 1`, `Tech debt 1`, `Test gaps 1`.
  - High: `CRIT-001`, `PERF-002`.
  - Module cover: `Outbox, TransactionCoordinator, WalletRepository, ChatFinanceRepository, EscrowTimer, Redis bridge, event dispatch`.
  - Còn lại: test-gap map + de-dup issue + ưu tiên fix roadmap.

- Delta #5 (Pass E - Test layer + consolidate):
  - Issue mới: `Test gaps 2`, chuẩn hóa lại priority/risk, khử trùng lặp issue.
  - High còn lại (final):
    - `CRIT-001`, `CRIT-002`, `CRIT-003`, `CRIT-004`, `CRIT-005`
    - `FUNC-001`, `FUNC-002`, `FUNC-003`, `FUNC-004`
    - `SEC-001`, `SEC-002`, `SEC-003`
    - `DEBT-001`

## 2. Review theo từng tính năng / module

### 2.1 Auth / Mfa / UserContext

- Chức năng:
  - Login/refresh/logout, đăng ký/xác minh email, quên mật khẩu, MFA setup/challenge/verify.
- Luồng chính:
  - Controller -> MediatR command -> repository/event publisher.
- Vấn đề phát hiện:
  - VerifyEmail cộng `RegisterBonus` trực tiếp nhưng không publish `MoneyChangedDomainEvent`.
  - Rate-limit policy dùng lại chưa đúng ngữ cảnh cho register/password/MFA.
  - Reset/verify flow phụ thuộc mạnh vào pipeline transaction ngầm.
- Bug tiềm ẩn:
  - Drift realtime wallet (UI/analytics) sau verify email.
  - Brute-force cho endpoints nhạy cảm khi RL quá nới.
- Code thừa/dead code:
  - Chưa phát hiện dead code rõ ràng; nhưng có nhiều hardcoded policy mapping trong controller.
- Nợ kỹ thuật:
  - Kiến trúc event-driven chưa đồng nhất giữa các command auth.
- Mức độ ưu tiên:
  - **High**.
- Đề xuất xử lý:
  - Chuẩn hóa mọi wallet mutation qua publish event.
  - Tách RL policy chuyên biệt cho register/password/MFA challenge.

### 2.2 Wallet / Deposit / Withdrawal / Escrow / Reconciliation

- Chức năng:
  - Nạp/rút, freeze/refund/release escrow, xử lý webhook/reconcile.
- Luồng chính:
  - Request -> command -> domain event -> wallet repository -> outbox/realtime.
- Vấn đề phát hiện:
  - `OpenDispute` validate reason nhưng không persist reason.
  - Timer auto-resolve dispute hardcode 48h, lệch với config dispute window.
  - Một số nhánh timer tách settlement và conversation update khỏi 1 transaction nhất quán.
- Bug tiềm ẩn:
  - Auto-resolve dispute sai deadline khi config thay đổi.
  - Conversation state lệch với settlement thực tế khi lỗi Mongo/update.
- Code thừa/dead code:
  - Nhiều literal status string (`"active"`, `"completed"`, `"refunded"`).
- Nợ kỹ thuật:
  - Cross-store transactional guarantee chưa có pattern bù trừ rõ ràng.
- Mức độ ưu tiên:
  - **High**.
- Đề xuất xử lý:
  - Persist dispute reason vào entity/audit table.
  - Dùng `DisputeWindowEnd` làm nguồn sự thật duy nhất.
  - Thiết kế compensation/outbox choreography cho cross-store updates.

### 2.3 Reading / Reader / AI / Conversation (bao gồm flow tài chính chat)

- Chức năng:
  - Stream AI reading, completion settlement, add-money flow trong conversation.
- Luồng chính:
  - `StreamReading` validate -> create ai request -> freeze -> stream SSE -> `CompleteAiStream` settlement.
- Vấn đề phát hiện:
  - Check quota/in-flight chỉ là pre-check không atomic.
  - Accept add-money: freeze tiền rồi mới ghi accept message (cross-store partial failure risk).
  - `CompleteAiStream` có thể append followup trùng khi callback completion lặp.
- Bug tiềm ẩn:
  - Vượt quota khi concurrent burst.
  - User bị freeze tiền nhưng không có accept message timeline tương ứng.
- Code thừa/dead code:
  - Build JSON bằng string interpolation thủ công ở offer response.
- Nợ kỹ thuật:
  - Chưa có idempotency guard business-level cho append followup theo request id.
- Mức độ ưu tiên:
  - **High**.
- Đề xuất xử lý:
  - Atomic quota reservation trước provider call.
  - Saga/compensation cho freeze + message persist.
  - Gắn dedupe key khi cập nhật session followup.

### 2.4 API boundary (Controllers / Middlewares / Hubs / Startup)

- Chức năng:
  - Bảo vệ endpoint, ProblemDetails, realtime hubs, rate limit, auth cookies.
- Luồng chính:
  - Middleware chain -> auth/rate-limit -> controller/hub -> mediator.
- Vấn đề phát hiện:
  - Nhiều endpoint nhạy cảm dùng RL policy quá rộng hoặc chưa có RL.
  - `CorrelationIdMiddleware` tin tưởng input header chưa giới hạn độ dài/ký tự.
  - Presence hub publish offline theo từng disconnect mà không kiểm tra user còn connection khác.
- Bug tiềm ẩn:
  - Flood/replay/bruteforce ở auth/mfa/payment-related endpoints.
  - Presence UI nhảy trạng thái sai (online -> offline giả).
- Code thừa/dead code:
  - Chưa thấy dead code rõ ràng.
- Nợ kỹ thuật:
  - Boundary policy chưa nhất quán theo risk level endpoint.
- Mức độ ưu tiên:
  - **High**.
- Đề xuất xử lý:
  - Policy matrix rate-limit theo endpoint criticality.
  - Validate/normalize correlation-id nghiêm ngặt.
  - Publish offline chỉ khi `IUserPresenceTracker.IsOnline(userId) == false`.

### 2.5 Community / Notification / Presence / History

- Chức năng:
  - Bài viết, bình luận, báo cáo, notification state, presence realtime.
- Luồng chính:
  - Command/query + event handlers + Mongo repositories.
- Vấn đề phát hiện:
  - CreateReport chưa validate target existence/ownership/context.
  - Report endpoint chưa có anti-spam/rate limiting phù hợp.
  - Presence status có thể phát event sai theo multi-connection.
- Bug tiềm ẩn:
  - Rác moderation data, report giả mạo bằng target id tùy ý.
- Code thừa/dead code:
  - Một số hardcoded status/literal strings.
- Nợ kỹ thuật:
  - Chưa chuẩn hóa điều kiện ownership ở report flow.
- Mức độ ưu tiên:
  - **Medium -> High** (tùy môi trường abuse thực tế).
- Đề xuất xử lý:
  - Validate target theo type + ownership permission check.
  - Bổ sung RL cho `/reports`.

### 2.6 Gamification / Gacha / Inventory / CheckIn / Promotions / Profile / Home / Legal / Admin

- Chức năng:
  - Tính năng domain bổ sung + admin operations.
- Luồng chính:
  - CQRS command/query với repository + domain events.
- Vấn đề phát hiện:
  - `AdminUsersController.UpdateUser` tự phát sinh idempotency key khi thiếu -> semantics idempotency bị suy yếu.
  - `UpdateUser` dùng `TransactionType.AdminTopup` cho cả debit adjustment -> lệch nghĩa audit.
  - Nhiều command handlers vẫn thao tác repo trực tiếp (vi phạm guardrail event-only nếu áp dụng strict toàn dự án).
- Bug tiềm ẩn:
  - Replay khó triage vì key ngẫu nhiên mới mỗi request.
- Code thừa/dead code:
  - Chưa phát hiện dead code rõ ràng.
- Nợ kỹ thuật:
  - Chuẩn event-only chưa phủ đầy module admin/gamification/checkin.
- Mức độ ưu tiên:
  - **Medium**.
- Đề xuất xử lý:
  - Bắt buộc client cung cấp idempotency key hợp lệ.
  - Tách transaction type cho admin debit.

### 2.7 Infrastructure sâu (Persistence / Messaging / BackgroundJobs / Outbox / Options / Migrations)

- Chức năng:
  - Transaction coordinator, outbox processor, repositories, background workers.
- Luồng chính:
  - DB transaction + outbox enqueue -> worker claim/dispatch/retry.
- Vấn đề phát hiện:
  - Partition key extraction của outbox không match payload camelCase.
  - Idempotency base cho domain event handlers chỉ hiệu lực với outbox message id.
  - Timer scan interval và batch strategy hardcoded, khả năng backlog lớn.
- Bug tiềm ẩn:
  - Event cùng business key xử lý lệch thứ tự, race side-effect.
  - Duplicate side-effects ở inline dispatch flows nếu retry ở tầng trên.
- Code thừa/dead code:
  - Literal constants phân tán nhiều file timer/service.
- Nợ kỹ thuật:
  - Thiếu test regression cho partitioning và concurrency path.
- Mức độ ưu tiên:
  - **High**.
- Đề xuất xử lý:
  - Normalize partition key parser theo camelCase/case-insensitive.
  - Bổ sung dedupe strategy cho inline dispatch.

### 2.8 Test layer

- Chức năng:
  - Unit/integration/architecture tests.
- Luồng chính:
  - Architecture constraints + unit business rules + integration outbox/api.
- Vấn đề phát hiện:
  - Thiếu test cho các flow high-risk concurrency/cross-store.
  - Thiếu test cho modules `RespondConversationAddMoney`, `OpenConversationDispute`, `PublishUserStatusChanged`.
  - Chưa có test phát hiện outbox partition key sai casing.
- Mức độ ưu tiên:
  - **High** cho test cần bắt regression tiền/quota/realtime.
- Đề xuất xử lý:
  - Thêm integration tests concurrency + idempotency + compensation.

## 3. Danh sách issue chi tiết

### 3.1 Critical bugs

- ID: `CRIT-001`
- Module: `Infrastructure/Outbox`
- Loại: `Critical bugs`
- Mô tả: Outbox partition key extraction gần như không hoạt động do mismatch casing.
- Tác động: Event cùng business entity có thể bị xử lý song song khác partition, gây sai thứ tự side-effects trong production.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs` (serialize bằng `JsonSerializerDefaults.Web` -> camelCase, dòng 14, 49).
  - `src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.Partitioning.cs` (candidate key PascalCase ở dòng 8-22, đọc `TryGetProperty` case-sensitive ở dòng 74).
- Nguyên nhân gốc rễ: Thiết kế parser partition key không đồng bộ naming policy của payload JSON.
- Cách sửa: Parse property case-insensitive hoặc map cả camelCase/PascalCase; bổ sung test regression partition key.
- Priority: `High`

- ID: `CRIT-002`
- Module: `Reading/AI quota`
- Loại: `Critical bugs`
- Mô tả: Kiểm tra quota/in-flight chỉ là pre-check, không có atomic reservation.
- Tác động: Burst concurrent request có thể vượt quota hoặc vượt cap in-flight trước khi hệ thống kịp cập nhật trạng thái.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.Validation.cs` (đếm `GetDailyAiRequestCountAsync`, `GetActiveAiRequestCountAsync` dòng 45-59).
  - `src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.cs` (sau check mới tạo request/freeze, dòng 57-70).
- Nguyên nhân gốc rễ: Thiếu primitive reservation/locking tại storage layer trước khi gọi provider.
- Cách sửa: Atomic reserve quota slot trong transaction/Redis script, rollback/refund slot khi fail.
- Priority: `High`

- ID: `CRIT-003`
- Module: `Conversation add-money`
- Loại: `Critical bugs`
- Mô tả: Flow chấp nhận add-money có nguy cơ partial success giữa freeze tiền và ghi message.
- Tác động: Tài chính đã freeze nhưng timeline chat thiếu accept message; khó reconcile, ảnh hưởng UX và support.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommand.cs` (freeze rồi send message ở dòng 73-74).
  - `.../RespondConversationAddMoneyCommandHandler.Workflow.cs` (`FreezeOfferAsync` gọi `AddQuestionCommand`, dòng 114-122).
  - `src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.cs` (`AddAsync` Mongo insert, dòng 43-47).
- Nguyên nhân gốc rễ: Cross-store operation (PostgreSQL + MongoDB) không có saga/compensation transaction.
- Cách sửa: Dùng outbox choreography + compensation message hoặc persist accept-intent trong write model rồi async materialize chat message.
- Priority: `High`

- ID: `CRIT-004`
- Module: `Escrow completion timeout`
- Loại: `Critical bugs`
- Mô tả: Settlement và cập nhật conversation/message thực hiện ở hai pha tách biệt, không atomic.
- Tác động: Có thể đã settle tiền nhưng conversation chưa chuyển trạng thái completed (hoặc ngược lại khi retry).
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.CompletionTimeouts.cs` (settle ở dòng 67, sau đó add message + update conversation ngoài transaction ở dòng 68-75).
- Nguyên nhân gốc rễ: Thiết kế tách persistence PG và Mongo không có cơ chế nhất quán cuối cùng bắt buộc.
- Cách sửa: Introduce durable state transition event + idempotent consumer để cập nhật conversation; thêm compensation/retry state machine.
- Priority: `High`

- ID: `CRIT-005`
- Module: `Escrow dispute auto-resolution`
- Loại: `Critical bugs`
- Mô tả: Auto-resolve dispute dùng hardcoded 48h và `UpdatedAt` thay vì mốc dispute window cấu hình.
- Tác động: Resolve sớm/trễ sai business policy, đặc biệt khi `EscrowDisputeWindowHours` thay đổi.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs` (set dispute window theo config, dòng 70-75).
  - `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs` (hardcoded `AddHours(-48)` dòng 19,57).
- Nguyên nhân gốc rễ: Timer không dùng cùng source-of-truth (`DisputeWindowEnd`) với command mở dispute.
- Cách sửa: Query trực tiếp theo `DisputeWindowEnd <= now`; bỏ hardcode 48h.
- Priority: `High`

### 3.2 Functional bugs

- ID: `FUNC-001`
- Module: `Escrow/OpenDispute`
- Loại: `Functional bugs`
- Mô tả: Lý do tranh chấp được validate nhưng không được lưu.
- Tác động: Mất dữ liệu audit cho vận hành/compliance và khó giải quyết tranh chấp sau này.
- Bằng chứng trong code:
  - `OpenDisputeCommand` có `Reason` và validate (dòng 20-21, 67, 123-131).
  - `ChatQuestionItem` không có field reason (toàn entity không có thuộc tính lưu reason, dòng 5-78).
- Nguyên nhân gốc rễ: Domain model thiếu thuộc tính dispute reason / audit record.
- Cách sửa: Thêm `DisputeReason` (hoặc bảng lịch sử dispute) + persist khi mở dispute.
- Priority: `High`

- ID: `FUNC-002`
- Module: `Auth/VerifyEmail`
- Loại: `Functional bugs`
- Mô tả: Verify email cộng thưởng wallet trực tiếp nhưng không publish event tiền.
- Tác động: Dễ lệch realtime wallet/analytics/downstream subscribers.
- Bằng chứng trong code:
  - `src/TarotNow.Application/Features/Auth/Commands/VerifyEmail/VerifyEmailCommandHandler.cs` (`user.Wallet.Credit(...)` dòng 58, `UpdateAsync` dòng 59, không có publish event).
  - Đối chiếu chuẩn đúng ở `DepositPaymentSucceededDomainEventHandler` có publish `MoneyChangedDomainEvent` (dòng 81-90, 118-127).
- Nguyên nhân gốc rễ: Flow auth bonus chưa được chuẩn hóa theo event-driven money mutation.
- Cách sửa: Publish `MoneyChangedDomainEvent` sau wallet mutation (hoặc chuyển mutation vào event handler thống nhất).
- Priority: `High`

- ID: `FUNC-003`
- Module: `Reading/CompleteAiStream`
- Loại: `Functional bugs`
- Mô tả: Completion callback lặp có thể append follow-up trùng.
- Tác động: Dữ liệu phiên đọc bị duplicate câu trả lời, ảnh hưởng UX và billing/audit.
- Bằng chứng trong code:
  - `BuildUpdatedSession` luôn `newFollowups.Add(...)` khi `AiSummary` đã có (dòng 46-55) ở `CompleteAiStreamCommandHandler.SessionPersistence.cs`.
  - Không có check theo `AiRequestId`/sequence trước khi append.
- Nguyên nhân gốc rễ: Thiếu idempotency guard ở cập nhật session content.
- Cách sửa: Lưu marker processed-by-request và skip nếu đã apply; hoặc enforce unique followup key.
- Priority: `High`

- ID: `FUNC-004`
- Module: `Presence`
- Loại: `Functional bugs`
- Mô tả: PresenceHub luôn publish offline khi một connection disconnect, không kiểm tra user còn connection khác.
- Tác động: Presence state chập chờn sai trên multi-tab/multi-device.
- Bằng chứng trong code:
  - `PresenceHub.OnDisconnectedAsync` luôn gọi publish offline (dòng 75-78).
  - Tracker hỗ trợ multi-connection (`_connectionsByUser`, remove từng connection rồi còn/không còn) ở `InMemoryUserPresenceTracker` dòng 14, 58-65.
- Nguyên nhân gốc rễ: Hub publish status theo event disconnect thô, không dựa trên trạng thái aggregate online.
- Cách sửa: Chỉ publish offline khi `IsOnline(userId)` trả false sau `MarkDisconnected`.
- Priority: `High`

- ID: `FUNC-005`
- Module: `Report`
- Loại: `Functional bugs`
- Mô tả: Tạo report không validate target có tồn tại/thuộc ngữ cảnh hợp lệ.
- Tác động: Có thể tạo report rác cho ID tùy ý, tăng noise moderation.
- Bằng chứng trong code:
  - `CreateReportCommandHandler` chỉ check `TargetType` và độ dài `Reason`, rồi insert thẳng (dòng 47-58, 60-73) trong `Features/Chat/Commands/CreateReport/CreateReportCommand.cs`.
- Nguyên nhân gốc rễ: Thiếu validation liên module (message/conversation/user existence + ownership/access).
- Cách sửa: Bổ sung validation theo từng `TargetType` trước khi persist.
- Priority: `Medium`

- ID: `FUNC-006`
- Module: `Admin/UpdateUser`
- Loại: `Functional bugs`
- Mô tả: Debit adjustment vẫn dùng `TransactionType.AdminTopup`.
- Tác động: Ledger/audit sai nghĩa nghiệp vụ, khó đối soát và analytics sai phân loại.
- Bằng chứng trong code:
  - `UpdateUserCommandHandler.Balance.cs` gọi `DebitAsync(... type: TransactionType.AdminTopup ...)` dòng 115-124.
- Nguyên nhân gốc rễ: Dùng chung type cho cả credit/debit admin adjustment.
- Cách sửa: Tách transaction type riêng cho admin debit (ví dụ `AdminAdjustmentDebit`).
- Priority: `Medium`

### 3.3 Performance issues

- ID: `PERF-001`
- Module: `UserRepository`
- Loại: `Performance issues`
- Mô tả: Search username dùng `ToLower().Contains(...)` trên cột DB.
- Tác động: Dễ mất index usage, query chậm khi user table tăng lớn.
- Bằng chứng trong code:
  - `src/TarotNow.Infrastructure/Persistence/Repositories/UserRepository.cs` dòng 124-127.
- Nguyên nhân gốc rễ: Pattern tìm kiếm không tối ưu cho index/database collation.
- Cách sửa: Dùng `EF.Functions.ILike` (PostgreSQL) + index phù hợp hoặc cột normalized.
- Priority: `Medium`

- ID: `PERF-002`
- Module: `EscrowTimer`
- Loại: `Performance issues`
- Mô tả: Timer scan mỗi 1 giờ + limit xử lý completion timeout mặc định 200.
- Tác động: Backlog có thể dồn, SLA settlement chậm khi traffic cao.
- Bằng chứng trong code:
  - `EscrowTimerService.cs` `ScanInterval = TimeSpan.FromHours(1)` dòng 11.
  - `EscrowTimerService.CompletionTimeouts.cs` gọi query limit 200 dòng 20-22.
  - `MongoConversationRepository.GetConversationsAwaitingCompletionResolutionAsync` default limit 200, clamp max 1000 dòng 103-109.
- Nguyên nhân gốc rễ: Tham số vận hành chưa externalize đầy đủ + batch strategy cố định.
- Cách sửa: Cho phép config scan interval/batch dynamic; loop drain theo cursor cho đến khi hết due items.
- Priority: `High`

- ID: `PERF-003`
- Module: `EscrowTimer transaction flow`
- Loại: `Performance issues`
- Mô tả: Nhiều `SaveChangesAsync` lặp trong cùng flow timer làm tăng lock duration/round-trips.
- Tác động: Throughput giảm và tăng contention trên rows hot khi load cao.
- Bằng chứng trong code:
  - `EscrowTimerService.DisputesAndOffers.cs` save changes nhiều lần trong cùng candidate (dòng 64, 84).
  - `EscrowTimerService.AutoReleases.cs` save ở dòng 66 và 75.
- Nguyên nhân gốc rễ: Persist chia nhỏ theo bước thay vì batch cuối transaction.
- Cách sửa: Gom mutate state và save một lần cuối transaction (trừ khi cần flush vì ràng buộc rõ ràng).
- Priority: `Medium`

### 3.4 Security issues

- ID: `SEC-001`
- Module: `Auth/Mfa/Withdrawal controllers`
- Loại: `Security issues`
- Mô tả: Policy rate-limit chưa phù hợp mức nhạy cảm endpoint.
- Tác động: Tăng nguy cơ brute-force/abuse cho auth và tác vụ tài chính.
- Bằng chứng trong code:
  - `AuthRegistrationController` class-level `[EnableRateLimiting("auth-login")]` dòng 14.
  - `AuthPasswordController` class-level `[EnableRateLimiting("auth-login")]` dòng 12.
  - `MfaController` class-level `[EnableRateLimiting("auth-session")]` dòng 23.
  - `WithdrawalController` class-level `[EnableRateLimiting("auth-session")]` dòng 19.
  - `auth-session` default 100 req/60s ở `ApiServiceCollectionExtensions.RateLimit.Options.cs` dòng 15.
- Nguyên nhân gốc rễ: Dùng chung policy convenience thay vì risk-based per endpoint.
- Cách sửa: Tách policy riêng cho register/forgot/reset/mfa-challenge/withdraw-create với ngưỡng thấp hơn và partition phù hợp.
- Priority: `High`

- ID: `SEC-002`
- Module: `Deposit order create`
- Loại: `Security issues`
- Mô tả: Tạo đơn nạp tiền chưa có attribute rate-limit riêng.
- Tác động: Có thể bị spam tạo order, tăng tải gateway/reconciliation.
- Bằng chứng trong code:
  - `DepositController.Orders.cs` action `CreateOrder` chỉ có `[Authorize]` (dòng 29-33), không có `[EnableRateLimiting]`.
- Nguyên nhân gốc rễ: Bỏ sót policy cho endpoint payment-initiation.
- Cách sửa: Áp policy riêng (ví dụ `payment-create-order`) theo user/device + burst nhỏ.
- Priority: `High`

- ID: `SEC-003`
- Module: `CorrelationId middleware`
- Loại: `Security issues`
- Mô tả: Chấp nhận thẳng `X-Correlation-ID` từ client không sanitize/length limit.
- Tác động: Log/header poisoning, tăng rủi ro memory/log abuse.
- Bằng chứng trong code:
  - `CorrelationIdMiddleware.ResolveCorrelationId` trả raw header sau trim (dòng 54-62).
  - Giá trị này được set vào response header + log scope (dòng 35, 38-41).
- Nguyên nhân gốc rễ: Thiếu validation whitelist charset/length.
- Cách sửa: Giới hạn độ dài (ví dụ 64), regex cho charset an toàn, fallback GUID khi invalid.
- Priority: `High`

- ID: `SEC-004`
- Module: `Report endpoint`
- Loại: `Security issues`
- Mô tả: Endpoint tạo report chưa có rate limiting.
- Tác động: Abuse/spam report gây quá tải moderation queue.
- Bằng chứng trong code:
  - `ReportController` không có `[EnableRateLimiting]` ở controller/action (file `src/TarotNow.Api/Controllers/ReportController.cs`, action `Create` dòng 37-58).
- Nguyên nhân gốc rễ: Chưa phân loại endpoint report vào nhóm write-sensitive.
- Cách sửa: Áp policy riêng `community-write` hoặc policy report dedicated.
- Priority: `Medium`

### 3.5 Code smell / dead code

- ID: `SMELL-001`
- Module: `Escrow/Chat finance`
- Loại: `Code smell / dead code`
- Mô tả: Trạng thái session/item dùng magic strings phân tán.
- Tác động: Dễ typo, khó refactor, dễ drift state machine giữa module.
- Bằng chứng trong code:
  - `EscrowTimerService.*` set `"completed"`, `"active"`, `"refunded"` (ví dụ: `DisputesAndOffers.cs` dòng 72, 78; `CompletionTimeouts.Helpers.cs` dòng 107; `Conversations.cs` dòng 25).
- Nguyên nhân gốc rễ: Thiếu enum/constant tập trung cho finance session statuses.
- Cách sửa: Tạo constants/enum thống nhất + mapper persistence.
- Priority: `Medium`

- ID: `SMELL-002`
- Module: `RespondConversationAddMoney`
- Loại: `Code smell / dead code`
- Mô tả: Dựng JSON payload bằng string interpolation thủ công.
- Tác động: Dễ lỗi escaping edge-case và khó bảo trì schema payload.
- Bằng chứng trong code:
  - `RespondConversationAddMoneyCommandHandler.Workflow.Messages.cs` `BuildOfferResponseContent` dòng 106-119.
- Nguyên nhân gốc rễ: Không dùng serializer/object contract cho payload.
- Cách sửa: Dùng DTO + `JsonSerializer.Serialize`.
- Priority: `Medium`

- ID: `SMELL-003`
- Module: `AdminUsersController`
- Loại: `Code smell / dead code`
- Mô tả: Tự sinh idempotency key random khi request thiếu key.
- Tác động: Hành vi idempotent phía client bị phá ngầm, khó trace retry semantics.
- Bằng chứng trong code:
  - `AdminUsersController.UpdateUser` fallback `Guid.NewGuid().ToString()` dòng 84-89.
- Nguyên nhân gốc rễ: Endpoint “chữa cháy” contract thay vì fail-fast.
- Cách sửa: Trả 400 bắt buộc `Idempotency-Key`.
- Priority: `Medium`

- ID: `SMELL-004`
- Module: `CreateReportCommandHandler`
- Loại: `Code smell / dead code`
- Mô tả: `validTypes` hardcoded array đặt trong handler mỗi lần xử lý.
- Tác động: Khó tái sử dụng/đồng bộ khi thêm loại report mới.
- Bằng chứng trong code:
  - `CreateReportCommandHandler` dòng 47-48.
- Nguyên nhân gốc rễ: Thiếu constants/enum cho report target type.
- Cách sửa: Chuẩn hóa enum/constants + validator riêng.
- Priority: `Low`

### 3.6 Tech debt

- ID: `DEBT-001`
- Module: `Application/CQRS architecture`
- Loại: `Tech debt`
- Mô tả: Nhiều command handlers vẫn inject repository/service/provider trực tiếp (vi phạm guardrail event-only nếu áp dụng strict toàn dự án).
- Tác động: Kiến trúc side-effect không nhất quán, khó kiểm soát dependency direction, khó migrate event-driven chuẩn.
- Bằng chứng trong code:
  - Quét toàn `Features/*/Commands`: **55 file** có dependency `I*Repository|I*Service|I*Provider` trong command handlers.
  - Ví dụ: `StreamReadingCommandHandler`, `SendMessageCommandHandler`, `VerifyEmailCommandHandler`, `UpdateUserCommandHandler`, `DailyCheckInCommandHandler`.
- Nguyên nhân gốc rễ: Migration event-only chưa hoàn tất, architecture tests hiện chưa enforce toàn cục.
- Cách sửa: Lập roadmap migrate theo bounded-context, ưu tiên money/auth/escrow trước; cập nhật architecture tests enforce dần.
- Priority: `High`

- ID: `DEBT-002`
- Module: `Domain event idempotency`
- Loại: `Tech debt`
- Mô tả: Cơ chế idempotent handler chỉ hoạt động khi có `OutboxMessageId`; inline dispatch không được dedupe.
- Tác động: Retry ở tầng request/hub có thể kích hoạt cùng domain event handler nhiều lần trong một số flow inline.
- Bằng chứng trong code:
  - `InlineMediatRDomainEventDispatcher` tạo notification với `outboxMessageId = null` (dòng 48) trong `InlineMediatRDomainEventDispatcher.cs`.
  - `IdempotentDomainEventNotificationHandler` chỉ check/mark khi `outboxMessageId.HasValue` (dòng 30-44).
- Nguyên nhân gốc rễ: Dedupe key thiết kế gắn chặt outbox message id.
- Cách sửa: Thêm optional event-level idempotency key cho inline flow hoặc buộc critical side-effects đi outbox.
- Priority: `Medium`

- ID: `DEBT-003`
- Module: `Cross-store consistency (Postgres + Mongo)`
- Loại: `Tech debt`
- Mô tả: Nhiều flow nghiệp vụ cần atomic semantics nhưng dữ liệu nằm ở 2 store khác nhau.
- Tác động: Partial state khó tránh khi fail giữa các bước (đặc biệt tiền + chat timeline).
- Bằng chứng trong code:
  - `RespondConversationAddMoney` freeze (PG) rồi send message (Mongo).
  - `EscrowTimer completion timeout` settle trước rồi update conversation/message sau.
- Nguyên nhân gốc rễ: Chưa có saga/compensation framework chuẩn cho cross-store business transaction.
- Cách sửa: Chuẩn hóa eventual-consistency pattern dựa trên outbox + idempotent consumers + compensation commands.
- Priority: `High`

### 3.7 Test gaps

- ID: `TEST-001`
- Module: `Chat finance commands`
- Loại: `Test gaps`
- Mô tả: Thiếu test cho `RespondConversationAddMoney` và `OpenConversationDispute`.
- Tác động: Regression ở flow tiền trong chat khó bị bắt sớm.
- Bằng chứng trong code:
  - Search test code: `RespondConversationAddMoney:0`, `OpenConversationDispute:0` match trong `tests`.
- Nguyên nhân gốc rễ: Chưa ưu tiên test cho flow orchestration cross-store.
- Cách sửa: Thêm unit + integration tests cho accept/reject/dispute idempotency và failure compensation.
- Priority: `High`

- ID: `TEST-002`
- Module: `Reading quota/concurrency`
- Loại: `Test gaps`
- Mô tả: Không có test mô phỏng race concurrent stream requests để verify quota atomicity.
- Tác động: Lỗi oversubscription quota có thể lọt production.
- Bằng chứng trong code:
  - Không có test reference tới `GetDailyAiRequestCountAsync`/`GetActiveAiRequestCountAsync` trong `tests`.
- Nguyên nhân gốc rễ: Test hiện tại thiên về happy path/single request.
- Cách sửa: Viết concurrency integration test (N requests đồng thời, assert không vượt cap).
- Priority: `High`

- ID: `TEST-003`
- Module: `Outbox partitioning`
- Loại: `Test gaps`
- Mô tả: Chưa có test xác nhận extraction partition key từ payload camelCase.
- Tác động: Bug partition ordering khó phát hiện nếu chỉ test stale-lock/retry cơ bản.
- Bằng chứng trong code:
  - Không có test match `OutboxBatchProcessor.Partitioning` trong `tests`.
- Nguyên nhân gốc rễ: Bộ integration test outbox tập trung retry/dead-letter, chưa kiểm tra partition semantics.
- Cách sửa: Thêm unit test cho `ResolveProcessingPartitionKey` với payload camelCase/PascalCase.
- Priority: `High`

- ID: `TEST-004`
- Module: `Presence`
- Loại: `Test gaps`
- Mô tả: Thiếu test multi-connection cho publish online/offline.
- Tác động: Regression presence flicker khó bắt.
- Bằng chứng trong code:
  - Không có test match `PublishUserStatusChanged` trong `tests`.
- Nguyên nhân gốc rễ: Thiếu integration test cho hub lifecycle theo multi-device.
- Cách sửa: Thêm tests cho connect/disconnect 2 connection cùng user và assert chỉ publish offline khi connection count về 0.
- Priority: `Medium`

- ID: `TEST-005`
- Module: `Auth/VerifyEmail`
- Loại: `Test gaps`
- Mô tả: Test hiện tại chỉ assert user active + OTP used, chưa assert event tiền/realtime.
- Tác động: Nếu thêm event sau này dễ bị quên hoặc triển khai thiếu.
- Bằng chứng trong code:
  - `VerifyEmailCommandHandlerTests` chỉ verify `UpdateAsync` của OTP/user (dòng 110-113), không có assert event publish.
- Nguyên nhân gốc rễ: Handler chưa inject publisher -> test không thể bắt side-effect event.
- Cách sửa: Refactor handler theo event-driven rồi bổ sung assert publish.
- Priority: `Medium`

- ID: `TEST-006`
- Module: `Escrow/OpenDispute`
- Loại: `Test gaps`
- Mô tả: Không có test xác nhận persistence dispute reason.
- Tác động: Thiếu guard cho yêu cầu audit/compliance dispute.
- Bằng chứng trong code:
  - `OpenDisputeCommandHandlerTests` chỉ assert status/time window; không có assert trường reason lưu trữ.
- Nguyên nhân gốc rễ: Domain chưa có field reason nên test chưa bao phủ.
- Cách sửa: Bổ sung field + test persistence reason + query/read model.
- Priority: `Medium`

## 4. Danh sách refactor đề xuất

### Việc nên làm ngay

1. Fix `CRIT-001`: sửa outbox partition key parser theo camelCase/case-insensitive + thêm test regression.
2. Fix `CRIT-002`: thiết kế atomic quota reservation cho AI stream.
3. Fix `CRIT-003` + `CRIT-004`: chuẩn hóa cross-store consistency bằng saga/compensation cho flow tiền-chat và completion-timeout.
4. Fix `CRIT-005` + `FUNC-001`: dispute phải dùng `DisputeWindowEnd` và lưu reason đầy đủ.
5. Fix `SEC-001/002/003`: harden rate-limit matrix và sanitize correlation-id.
6. Fix `DEBT-001` cho module tiền/auth trước: chặn command handler gọi trực tiếp side-effect dependencies.

### Việc có thể trì hoãn (nhưng cần backlog rõ)

1. Chuẩn hóa magic status strings (`SMELL-001`) thành enum/constants shared.
2. Chuyển JSON string thủ công sang DTO serializer (`SMELL-002`).
3. Tối ưu search/index ở user repository (`PERF-001`).
4. Tối ưu timer scheduling/batch strategy (`PERF-002`, `PERF-003`).
5. Bổ sung full test suite cho các test gaps (`TEST-001..006`).

## 5. Kết luận

- Backend hiện có nền tảng tốt về tổ chức layer, outbox, test architecture baseline; tuy nhiên còn nhiều điểm **rủi ro cao** ở các flow liên quan tiền, quota, và consistency cross-store.
- Nếu release ngay, rủi ro chính là:
  - xử lý event sai thứ tự/đua luồng,
  - partial state giữa wallet và conversation,
  - SLA timeout/dispute lệch policy,
  - abuse endpoint nhạy cảm do policy chưa chặt.
- Khuyến nghị: xử lý toàn bộ issue `High` trước khi release production chính thức; sau đó mới chuyển sang nhóm `Medium/Low` theo roadmap refactor.


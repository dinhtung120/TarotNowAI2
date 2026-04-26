# BACKEND_REVIEW_REPORT

## 1. Executive summary

- Trạng thái tổng thể sau khi thực thi kế hoạch fix: **đã xử lý toàn bộ lỗi functional/critical/security/performance được ưu tiên trực tiếp cho production flow**.
- Mức rủi ro hiện tại: **Medium** (không còn blocker `High` cho release kỹ thuật).
- Các điểm đã được đóng:
  - Outbox partitioning theo business key/casing.
  - Quota AI stream race condition.
  - Cross-store inconsistency ở add-money/completion-timeout.
  - Dispute window/reason persistence.
  - Security hardening (rate-limit matrix, correlation id sanitize, endpoint nhạy cảm).
  - Test gaps trọng yếu cho money/quota/outbox/presence.
- Điểm còn lại cần roadmap dài hạn:
  - `DEBT-001` (migrate toàn bộ command handlers sang event-only tuyệt đối) chưa thể đại phẫu hết trong một nhịp; đã khóa baseline bằng architecture gate để **không tăng thêm nợ**.

### Baseline kỹ thuật đã khóa (sau fix)

- `TarotNow.ArchitectureTests`: **pass 27/27**.
- `TarotNow.Domain.UnitTests`: **pass 7/7**.
- `TarotNow.Application.UnitTests`: **pass 138/138**.
- `TarotNow.Infrastructure.IntegrationTests`: **pass 11/11**.
- `TarotNow.Api.IntegrationTests`: **pass 36/36**.

### Delta update cuối (Pass F - hardening + closeout)

- Issue mới: `0`.
- Issue đóng thêm: `DEBT guardrail` (khóa baseline dependency debt bằng kiến trúc test).
- High còn lại: **0**.
- Module đã cover: toàn bộ theo plan review/fix trước đó + architecture governance.

## 2. Review theo từng tính năng / module

### 2.1 Auth / Mfa / UserContext

- Chức năng: login/register/reset/verify/mfa/session.
- Đã xử lý:
  - Verify email bonus wallet giờ publish `MoneyChangedDomainEvent`.
  - Harden policy rate-limit riêng cho register/password/mfa challenge.
- Bằng chứng chính:
  - `src/TarotNow.Application/Features/Auth/Commands/VerifyEmail/VerifyEmailCommandHandler.cs`
  - `src/TarotNow.Api/Controllers/AuthRegistrationController.cs`
  - `src/TarotNow.Api/Controllers/AuthPasswordController.cs`
  - `src/TarotNow.Api/Controllers/MfaController.cs`
- Rủi ro còn lại: kiến trúc event-only full migration chưa phủ 100% command handlers.

### 2.2 Wallet / Deposit / Withdrawal / Escrow / Reconciliation

- Chức năng: flow tiền, freeze/refund/release, dispute.
- Đã xử lý:
  - Persist `DisputeReason` + enforce `DisputeWindowEnd` làm source-of-truth.
  - Payment-initiation/withdrawal endpoints có RL chuyên biệt.
  - Timer/settlement path được chuẩn hóa thêm theo config runtime.
- Bằng chứng chính:
  - `src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs`
  - `src/TarotNow.Domain/Entities/ChatQuestionItem.cs`
  - `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`
  - `src/TarotNow.Api/Controllers/DepositController.Orders.cs`
  - `src/TarotNow.Api/Controllers/WithdrawalController.cs`

### 2.3 Reading / AI / Conversation (bao gồm flow tiền chat)

- Chức năng: stream AI, add-money, completion settlement.
- Đã xử lý:
  - Quota reservation có critical section lock trước khi tạo request.
  - Add-money accept có compensation refund khi fail ghi message.
  - Completion callback dedupe followup theo `AiRequestId`.
- Bằng chứng chính:
  - `src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.RequestCreation.cs`
  - `src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommand.cs`
  - `src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.SessionPersistence.cs`

### 2.4 API boundary (Controllers / Middlewares / Hubs / Startup)

- Chức năng: bảo vệ API, middleware, realtime boundary.
- Đã xử lý:
  - Correlation-id sanitize (charset + length whitelist).
  - Report endpoint có rate-limit riêng.
  - Presence publish offline chỉ khi không còn active connection.
- Bằng chứng chính:
  - `src/TarotNow.Api/Middlewares/CorrelationIdMiddleware.cs`
  - `src/TarotNow.Api/Controllers/ReportController.cs`
  - `src/TarotNow.Api/Hubs/PresenceHub.cs`
  - `src/TarotNow.Api/Realtime/InMemoryUserPresenceTracker.cs`

### 2.5 Community / Notification / Presence / History

- Chức năng: moderation/report, presence, notification.
- Đã xử lý:
  - Create report validate target existence + access context.
  - Bổ sung kiểm thử presence multi-connection.
- Bằng chứng chính:
  - `src/TarotNow.Application/Features/Chat/Commands/CreateReport/CreateReportCommand.cs`
  - `tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs`

### 2.6 Domain feature bổ sung (Gamification/Gacha/CheckIn/Admin...)

- Đã xử lý:
  - `UpdateUser` yêu cầu idempotency key rõ ràng.
  - Tách transaction type admin credit/debit cho audit đúng nghĩa.
  - Check-in concurrency được khóa bằng lock + pre-check.
- Bằng chứng chính:
  - `src/TarotNow.Api/Controllers/AdminUsersController.cs`
  - `src/TarotNow.Domain/Enums/TransactionType.cs`
  - `src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs`

### 2.7 Infrastructure sâu (Outbox / Jobs / Persistence / Migrations)

- Đã xử lý:
  - Outbox partition key parser case-insensitive + regression test.
  - Inline/outbox event-handler idempotency unify bằng event key.
  - Completion-timeout sync chuyển sang outbox choreography idempotent.
  - Index/migration bổ sung cho dispute reason + user search + inline handler state.
- Bằng chứng chính:
  - `src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.Partitioning.cs`
  - `src/TarotNow.Application/Common/DomainEvents/IdempotentDomainEventNotificationHandler.cs`
  - `src/TarotNow.Application/DomainEvents/Handlers/CompletionTimeoutConversationSyncRequestedDomainEventHandler.cs`
  - `src/TarotNow.Infrastructure/Migrations/20260426075733_AddOutboxInlineIdempotencyAndDisputeReasonAndUserSearchIndex.cs`

### 2.8 Test layer

- Đã xử lý:
  - Bổ sung test cho add-money compensation, open-dispute routing, outbox partitioning, AI concurrency near cap, presence multi-connection.
- Bằng chứng chính:
  - `tests/TarotNow.Application.UnitTests/Features/Chat/RespondConversationAddMoneyCommandHandlerTests.cs`
  - `tests/TarotNow.Application.UnitTests/Features/Chat/OpenConversationDisputeCommandHandlerTests.cs`
  - `tests/TarotNow.Infrastructure.UnitTests/BackgroundJobs/OutboxBatchProcessorPartitioningTests.cs`
  - `tests/TarotNow.Api.IntegrationTests/AiStreamingTests.cs`
  - `tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs`

## 3. Danh sách issue chi tiết

### 3.1 Critical bugs

- ID: `CRIT-001`
- Module: `Infrastructure/Outbox`
- Loại: `Critical bugs`
- Mô tả: **Đã xử lý** parser partition key hỗ trợ case-insensitive/camelCase.
- Tác động: giữ đúng thứ tự xử lý theo business key.
- Bằng chứng trong code: `src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.Partitioning.cs`, `tests/TarotNow.Infrastructure.UnitTests/BackgroundJobs/OutboxBatchProcessorPartitioningTests.cs`.
- Nguyên nhân gốc rễ: mismatch naming policy JSON.
- Cách sửa: parse key case-insensitive + test regression.
- Priority: `Low`

- ID: `CRIT-002`
- Module: `Reading/AI quota`
- Loại: `Critical bugs`
- Mô tả: **Đã xử lý** quota check + create request trong lock critical section.
- Tác động: giảm race oversubscription ở burst concurrent.
- Bằng chứng trong code: `src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.RequestCreation.cs`.
- Nguyên nhân gốc rễ: pre-check không atomic.
- Cách sửa: `AcquireLockAsync` theo user trước `EnsureQuotaAsync` + create request.
- Priority: `Low`

- ID: `CRIT-003`
- Module: `Conversation add-money`
- Loại: `Critical bugs`
- Mô tả: **Đã xử lý** thêm compensation refund khi fail ghi accept message.
- Tác động: tránh frozen funds bị orphan khi Mongo write lỗi.
- Bằng chứng trong code: `src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommand.cs`, `tests/TarotNow.Application.UnitTests/Features/Chat/RespondConversationAddMoneyCommandHandlerTests.cs`.
- Nguyên nhân gốc rễ: cross-store partial success.
- Cách sửa: catch + compensating transaction + publish `MoneyChangedDomainEvent`.
- Priority: `Low`

- ID: `CRIT-004`
- Module: `Escrow completion timeout`
- Loại: `Critical bugs`
- Mô tả: **Đã xử lý** đồng bộ projection Mongo bằng domain-event/outbox idempotent thay vì update trực tiếp 2 pha.
- Tác động: giảm lệch state giữa settlement PG và conversation/message Mongo.
- Bằng chứng trong code: `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.CompletionTimeouts.cs`, `src/TarotNow.Domain/Events/CompletionTimeoutConversationSyncRequestedDomainEvent.cs`, `src/TarotNow.Application/DomainEvents/Handlers/CompletionTimeoutConversationSyncRequestedDomainEventHandler.cs`.
- Nguyên nhân gốc rễ: thiếu choreography nhất quán cuối cùng.
- Cách sửa: publish durable event + idempotent handler + system_event_key dedupe.
- Priority: `Low`

- ID: `CRIT-005`
- Module: `Escrow dispute auto-resolution`
- Loại: `Critical bugs`
- Mô tả: **Đã xử lý** dùng `DisputeWindowEnd` thay hardcode 48h.
- Tác động: không còn resolve sai policy khi config thay đổi.
- Bằng chứng trong code: `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`, `src/TarotNow.Infrastructure/Repositories/ChatFinanceRepository.Timers.cs`.
- Nguyên nhân gốc rễ: timer không dùng source-of-truth dispute window.
- Cách sửa: query/process theo `DisputeWindowEnd <= now`.
- Priority: `Low`

### 3.2 Functional bugs

- ID: `FUNC-001`
- Module: `Escrow/OpenDispute`
- Loại: `Functional bugs`
- Mô tả: **Đã xử lý** persist `DisputeReason`.
- Tác động: đảm bảo audit/compliance dữ liệu tranh chấp.
- Bằng chứng trong code: `src/TarotNow.Domain/Entities/ChatQuestionItem.cs`, `src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`, `src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs`.
- Nguyên nhân gốc rễ: domain model thiếu field reason.
- Cách sửa: thêm field + migration + persist tại command.
- Priority: `Low`

- ID: `FUNC-002`
- Module: `Auth/VerifyEmail`
- Loại: `Functional bugs`
- Mô tả: **Đã xử lý** publish money event sau register bonus.
- Tác động: đồng bộ realtime/analytics downstream.
- Bằng chứng trong code: `src/TarotNow.Application/Features/Auth/Commands/VerifyEmail/VerifyEmailCommandHandler.cs`, `tests/TarotNow.Application.UnitTests/Features/Auth/Commands/VerifyEmailCommandHandlerTests.cs`.
- Nguyên nhân gốc rễ: flow auth bonus chưa chuẩn event-driven.
- Cách sửa: publish `MoneyChangedDomainEvent`.
- Priority: `Low`

- ID: `FUNC-003`
- Module: `Reading/CompleteAiStream`
- Loại: `Functional bugs`
- Mô tả: **Đã xử lý** dedupe followup theo `AiRequestId`.
- Tác động: ngăn append follow-up trùng khi callback lặp.
- Bằng chứng trong code: `src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.SessionPersistence.cs`, `src/TarotNow.Domain/Entities/ReadingSession.cs`.
- Nguyên nhân gốc rễ: thiếu idempotency marker ở session content.
- Cách sửa: lưu/so sánh `AiRequestId` trước append.
- Priority: `Low`

- ID: `FUNC-004`
- Module: `Presence`
- Loại: `Functional bugs`
- Mô tả: **Đã xử lý** chỉ publish offline khi không còn active connection.
- Tác động: giảm presence flicker multi-tab/multi-device.
- Bằng chứng trong code: `src/TarotNow.Api/Hubs/PresenceHub.cs`, `src/TarotNow.Application/Common/Interfaces/IUserPresenceTracker.cs`, `src/TarotNow.Api/Realtime/RedisUserPresenceTracker.ActiveConnections.cs`.
- Nguyên nhân gốc rễ: publish theo disconnect event thô.
- Cách sửa: aggregate-check bằng `HasActiveConnection`.
- Priority: `Low`

- ID: `FUNC-005`
- Module: `Report`
- Loại: `Functional bugs`
- Mô tả: **Đã xử lý** validate target existence + ownership/context.
- Tác động: giảm report rác/fake target.
- Bằng chứng trong code: `src/TarotNow.Application/Features/Chat/Commands/CreateReport/CreateReportCommand.cs`.
- Nguyên nhân gốc rễ: thiếu validation liên module.
- Cách sửa: validate theo `TargetType` + conversation access.
- Priority: `Low`

- ID: `FUNC-006`
- Module: `Admin/UpdateUser`
- Loại: `Functional bugs`
- Mô tả: **Đã xử lý** tách type admin credit/debit.
- Tác động: ledger/audit đúng semantics.
- Bằng chứng trong code: `src/TarotNow.Domain/Enums/TransactionType.cs`, `src/TarotNow.Application/Features/Admin/Commands/UpdateUser/UpdateUserCommandHandler.Balance.cs`.
- Nguyên nhân gốc rễ: dùng chung `AdminTopup` cho cả debit.
- Cách sửa: `AdminAdjustmentCredit` + `AdminAdjustmentDebit`.
- Priority: `Low`

### 3.3 Performance issues

- ID: `PERF-001`
- Module: `UserRepository`
- Loại: `Performance issues`
- Mô tả: **Đã xử lý** search username chuyển sang `ILike` + escape pattern.
- Tác động: cải thiện khả năng tận dụng index PostgreSQL.
- Bằng chứng trong code: `src/TarotNow.Infrastructure/Persistence/Repositories/UserRepository.cs`, migration index trigram trong `src/TarotNow.Infrastructure/Migrations/20260426075733_AddOutboxInlineIdempotencyAndDisputeReasonAndUserSearchIndex.cs`.
- Nguyên nhân gốc rễ: `ToLower().Contains` làm query khó tối ưu.
- Cách sửa: `EF.Functions.ILike` + pg_trgm index.
- Priority: `Low`

- ID: `PERF-002`
- Module: `EscrowTimer`
- Loại: `Performance issues`
- Mô tả: **Đã xử lý** externalize scan interval/batch size + drain nhiều batch mỗi vòng.
- Tác động: giảm backlog timeout khi traffic cao.
- Bằng chứng trong code: `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.cs`, `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.CompletionTimeouts.cs`, `src/TarotNow.Application/Interfaces/ISystemConfigSettings.cs`.
- Nguyên nhân gốc rễ: thông số runtime hardcode.
- Cách sửa: đọc runtime config + loop drain.
- Priority: `Low`

- ID: `PERF-003`
- Module: `EscrowTimer transaction flow`
- Loại: `Performance issues`
- Mô tả: **Đã xử lý** gom persist hợp lý hơn, giảm save lặp trong cùng flow.
- Tác động: giảm round-trip/lock contention.
- Bằng chứng trong code: `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`, `src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoReleases.cs`.
- Nguyên nhân gốc rễ: flush chia nhỏ nhiều bước.
- Cách sửa: save theo transaction boundary.
- Priority: `Low`

### 3.4 Security issues

- ID: `SEC-001`
- Module: `Auth/Mfa/Withdrawal controllers`
- Loại: `Security issues`
- Mô tả: **Đã xử lý** policy RL tách theo endpoint nhạy cảm.
- Tác động: giảm brute-force/abuse.
- Bằng chứng trong code: `src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.Policies.cs`, các controller auth/mfa/withdrawal.
- Nguyên nhân gốc rễ: dùng chung policy quá rộng.
- Cách sửa: phân tách policy matrix risk-based.
- Priority: `Low`

- ID: `SEC-002`
- Module: `Deposit order create`
- Loại: `Security issues`
- Mô tả: **Đã xử lý** thêm policy RL cho `CreateOrder`.
- Tác động: giảm spam payment-initiation.
- Bằng chứng trong code: `src/TarotNow.Api/Controllers/DepositController.Orders.cs`.
- Nguyên nhân gốc rễ: thiếu RL attribute.
- Cách sửa: `[EnableRateLimiting("payment-create-order")]`.
- Priority: `Low`

- ID: `SEC-003`
- Module: `CorrelationId middleware`
- Loại: `Security issues`
- Mô tả: **Đã xử lý** sanitize header correlation-id.
- Tác động: giảm log/header poisoning risk.
- Bằng chứng trong code: `src/TarotNow.Api/Middlewares/CorrelationIdMiddleware.cs`.
- Nguyên nhân gốc rễ: trust raw header trực tiếp.
- Cách sửa: regex whitelist + max length + fallback guid.
- Priority: `Low`

- ID: `SEC-004`
- Module: `Report endpoint`
- Loại: `Security issues`
- Mô tả: **Đã xử lý** thêm RL policy cho create report.
- Tác động: giảm spam moderation queue.
- Bằng chứng trong code: `src/TarotNow.Api/Controllers/ReportController.cs`.
- Nguyên nhân gốc rễ: thiếu classify endpoint write-sensitive.
- Cách sửa: `[EnableRateLimiting("report-create")]`.
- Priority: `Low`

### 3.5 Code smell / dead code

- ID: `SMELL-001`
- Module: `Escrow/Chat finance`
- Loại: `Code smell / dead code`
- Mô tả: **Đã xử lý** chuẩn hóa status constants.
- Tác động: giảm typo/drift state machine.
- Bằng chứng trong code: `src/TarotNow.Domain/Enums/ChatFinanceSessionStatus.cs` và các callsite timer/commands liên quan.
- Nguyên nhân gốc rễ: magic strings phân tán.
- Cách sửa: constants shared.
- Priority: `Low`

- ID: `SMELL-002`
- Module: `RespondConversationAddMoney`
- Loại: `Code smell / dead code`
- Mô tả: **Đã xử lý** build payload bằng serializer thay cho string interpolation.
- Tác động: giảm lỗi escaping/schema drift.
- Bằng chứng trong code: `src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommandHandler.Workflow.Messages.cs`.
- Nguyên nhân gốc rễ: manual JSON building.
- Cách sửa: DTO/payload + `JsonSerializer.Serialize`.
- Priority: `Low`

- ID: `SMELL-003`
- Module: `AdminUsersController`
- Loại: `Code smell / dead code`
- Mô tả: **Đã xử lý** không còn tự sinh random idempotency key; thiếu key trả 400.
- Tác động: giữ semantics idempotency rõ ràng.
- Bằng chứng trong code: `src/TarotNow.Api/Controllers/AdminUsersController.cs`.
- Nguyên nhân gốc rễ: fallback “chữa cháy” contract.
- Cách sửa: fail-fast khi thiếu `Idempotency-Key`.
- Priority: `Low`

- ID: `SMELL-004`
- Module: `CreateReportCommandHandler`
- Loại: `Code smell / dead code`
- Mô tả: **Đã xử lý** target types đưa vào constants domain.
- Tác động: giảm hardcode/lệch enum.
- Bằng chứng trong code: `src/TarotNow.Domain/Enums/ReportTargetTypes.cs`, `src/TarotNow.Application/Features/Chat/Commands/CreateReport/CreateReportCommand.cs`.
- Nguyên nhân gốc rễ: hardcoded array tại handler.
- Cách sửa: constants tập trung.
- Priority: `Low`

### 3.6 Tech debt

- ID: `DEBT-001`
- Module: `Application/CQRS architecture`
- Loại: `Tech debt`
- Mô tả: Migration event-only tuyệt đối cho toàn bộ command handlers vẫn còn backlog lớn.
- Tác động: tăng chi phí bảo trì kiến trúc dài hạn nếu không tiếp tục migration.
- Bằng chứng trong code: static scan command handlers còn nhiều dependency `I*Repository|I*Service|I*Provider`; baseline hiện tại khóa ở 55.
- Nguyên nhân gốc rễ: legacy design trước khi áp guardrail strict.
- Cách sửa: đã thêm gate kiến trúc chặn tăng nợ (`tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`); tiếp tục migrate theo từng bounded-context.
- Priority: `Medium`

- ID: `DEBT-002`
- Module: `Domain event idempotency`
- Loại: `Tech debt`
- Mô tả: **Đã xử lý** inline dispatch có idempotency key dedupe.
- Tác động: giảm duplicate side-effects khi retry.
- Bằng chứng trong code: `src/TarotNow.Application/Common/DomainEvents/IdempotentDomainEventNotificationHandler.cs`, `src/TarotNow.Infrastructure/Persistence/Outbox/OutboxHandlerIdempotencyService.cs`, `src/TarotNow.Infrastructure/Persistence/Outbox/OutboxInlineHandlerState.cs`.
- Nguyên nhân gốc rễ: trước đó dedupe phụ thuộc outbox message id.
- Cách sửa: thêm bảng/tracking inline event key.
- Priority: `Low`

- ID: `DEBT-003`
- Module: `Cross-store consistency`
- Loại: `Tech debt`
- Mô tả: **Đã xử lý trọng tâm production path** bằng outbox choreography + compensation.
- Tác động: giảm mạnh rủi ro partial state ở money/chat timeout.
- Bằng chứng trong code: `EscrowTimerService.CompletionTimeouts*`, `CompletionTimeoutConversationSyncRequestedDomainEvent*`, `RespondConversationAddMoneyCommand*`.
- Nguyên nhân gốc rễ: transaction trải qua PG + Mongo.
- Cách sửa: compensation/idempotent event flow.
- Priority: `Low`

### 3.7 Test gaps

- ID: `TEST-001`
- Module: `Chat finance commands`
- Loại: `Test gaps`
- Mô tả: **Đã xử lý** thêm test `RespondConversationAddMoney` và `OpenConversationDispute`.
- Tác động: bắt regression orchestration cross-store.
- Bằng chứng trong code: `tests/TarotNow.Application.UnitTests/Features/Chat/RespondConversationAddMoneyCommandHandlerTests.cs`, `tests/TarotNow.Application.UnitTests/Features/Chat/OpenConversationDisputeCommandHandlerTests.cs`.
- Nguyên nhân gốc rễ: thiếu coverage flow tiền.
- Cách sửa: unit tests mới + assertions side-effects.
- Priority: `Low`

- ID: `TEST-002`
- Module: `Reading quota/concurrency`
- Loại: `Test gaps`
- Mô tả: **Đã xử lý** thêm integration test race near in-flight cap.
- Tác động: bắt regression quota reservation.
- Bằng chứng trong code: `tests/TarotNow.Api.IntegrationTests/AiStreamingTests.cs`.
- Nguyên nhân gốc rễ: test thiên về single request.
- Cách sửa: concurrent integration scenario.
- Priority: `Low`

- ID: `TEST-003`
- Module: `Outbox partitioning`
- Loại: `Test gaps`
- Mô tả: **Đã xử lý** thêm unit tests cho camelCase/PascalCase/fallback.
- Tác động: bắt regression partition semantics.
- Bằng chứng trong code: `tests/TarotNow.Infrastructure.UnitTests/BackgroundJobs/OutboxBatchProcessorPartitioningTests.cs`.
- Nguyên nhân gốc rễ: thiếu test partition parser.
- Cách sửa: regression tests cụ thể.
- Priority: `Low`

- ID: `TEST-004`
- Module: `Presence`
- Loại: `Test gaps`
- Mô tả: **Đã xử lý** thêm test multi-connection publish online/offline.
- Tác động: bắt presence flicker regression.
- Bằng chứng trong code: `tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs`.
- Nguyên nhân gốc rễ: chưa có lifecycle test theo multi-device.
- Cách sửa: 2-connection lifecycle assertions.
- Priority: `Low`

- ID: `TEST-005`
- Module: `Auth/VerifyEmail`
- Loại: `Test gaps`
- Mô tả: **Đã xử lý** test assert publish money event.
- Tác động: guard regression side-effect auth bonus.
- Bằng chứng trong code: `tests/TarotNow.Application.UnitTests/Features/Auth/Commands/VerifyEmailCommandHandlerTests.cs`.
- Nguyên nhân gốc rễ: trước đó test không kiểm money event.
- Cách sửa: verify mock publisher.
- Priority: `Low`

- ID: `TEST-006`
- Module: `Escrow/OpenDispute`
- Loại: `Test gaps`
- Mô tả: **Đã xử lý** test assert persistence reason/dispute window.
- Tác động: giữ audit invariants ổn định.
- Bằng chứng trong code: `tests/TarotNow.Application.UnitTests/Features/Escrow/OpenDisputeCommandHandlerTests.cs`.
- Nguyên nhân gốc rễ: trước đó chưa assert reason.
- Cách sửa: bổ sung assert `DisputeReason` + time window.
- Priority: `Low`

## 4. Danh sách refactor đề xuất

### Việc nên làm ngay

1. Tiếp tục migration `DEBT-001` theo từng bounded-context (ưu tiên `Auth`, `Wallet/Escrow`, `Reading`).
2. Nâng architecture test từ baseline-count sang strict module-by-module khi từng module migrate xong.
3. Chuẩn hóa playbook saga/compensation thành template dùng chung cho các flow cross-store mới.

### Việc có thể trì hoãn (nhưng cần backlog rõ)

1. Tối ưu thêm query search/list users theo từng pattern thực tế traffic.
2. Theo dõi telemetry để tinh chỉnh runtime config outbox/escrow timer theo môi trường.
3. Migrate dần command handlers còn legacy dependency sang event-only full.

## 5. Kết luận

- Kế hoạch fix đã được thực thi đầy đủ cho toàn bộ issue vận hành quan trọng: **không còn issue High blocker trong report**.
- Backend hiện tại đủ điều kiện cho rollout kỹ thuật theo chuẩn thận trọng (staged/monitoring) với trọng tâm theo dõi:
  - money-flow reconciliation,
  - outbox lag,
  - AI stream concurrency.
- Rủi ro lớn nhất còn lại là nợ kiến trúc dài hạn `DEBT-001`; đã được khóa không tăng thêm bằng architecture gate và cần roadmap migration tiếp tục.

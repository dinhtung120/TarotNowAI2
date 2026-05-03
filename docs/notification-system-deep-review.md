# Deep System Review - Notification System (TarotNow)

- Review date: 2026-05-03 (Asia/Ho_Chi_Minh)
- Scope: `/Backend/src` + architecture/integration tests liên quan realtime/outbox/notification
- Ruleset audited: Event-Driven bắt buộc + Clean Architecture + CQRS + MediatR + Outbox + Redis Pub/Sub
- Evidence source: static code scan + test run `TarotNow.ArchitectureTests` (33/33 pass)

## Executive Summary

Hệ thống đã có nền tảng event-driven/outbox tốt ở mức infrastructure, nhưng **chưa production-ready theo chuẩn “Global Antigravity Rules”** vì còn các vấn đề kiến trúc nghiêm trọng:

1. **Realtime fast-lane phát side-effect trước commit transaction** trong luồng command (nguy cơ ghost event và duplicate).
2. **Dual-path realtime (fast-lane + durable)** đang phát trùng một số event contract (`message.created`, `conversation.updated`, `chat.unread_changed` dạng delta), tạo rủi ro spam và bất nhất ordering.
3. **Notification in-app không idempotent ở mức storage** (Mongo insert thuần) khi handler retry sau lỗi Redis -> có thể tạo bản ghi notification trùng.
4. **Event model bị “command hóa” mạnh** với 65 `*CommandHandlerRequestedDomainEvent` + nhiều domain event mutable (`Result/Handled/...`), làm mờ ranh giới domain fact vs command workflow.
5. Coverage notification cho các scenario quan trọng (deposit success/failure, AI result ready, security anomalies) còn thiếu.

## 1) Full Event Inventory

### 1.1 Tổng quan inventory

- Tổng event scan được: `128`
- Domain events: `63`
- Application-layer command wrapper events: `65` (`*CommandHandlerRequestedDomainEvent`)
- Domain events có map channel: `62/63` (thiếu map: `PityTriggeredDomainEvent`)

### 1.2 Ghi chú phương pháp

- `Business meaning` được lấy từ XML summary của event class nếu có.
- Event nào không có XML summary sẽ được đánh dấu “inferred from name only”.
- `Trigger source` được phân loại từ call-site:
  - `Command`: publish từ `/Features/.../Commands/...`
  - `DomainEventHandler`: publish từ handler khác
  - `Service/Other`: publish từ service/background infra

### 1.3 Full list

- Toàn bộ 63 domain events + trigger + payload + consumer đã được liệt kê ở:
  - `Appendix A.1 - Domain Event Inventory`
- Mapping event -> channel (InApp/Email/Push/Realtime) ở:
  - `Appendix A.2 - Event -> Notification Channel Map`

## 2) Notification Coverage Matrix

### 2.1 Coverage summary (Domain events)

- Events có ít nhất 1 channel (InApp/Email/Push/Realtime): `25/62`
- InApp: `9`
- Email: `3`
- Push: `2`
- Realtime: `18`
- Không có channel: `37`

### 2.2 Business matrix

| Business Action | Domain Event(s) | Có Notification? | Loại | Thiếu? | Thừa/Spam? |
|---|---|---|---|---|---|
| Gửi OTP email | `EmailOtpIssuedDomainEvent` | Có | Email | Không | Không |
| Login thất bại | `UserAuthenticationFailedDomainEvent` | Không (chỉ log) | - | Có | Không |
| Login thành công | `UserLoggedInDomainEvent` | Không | - | Có (security-aware org) | Không |
| Refresh token replay | `RefreshTokenReplayDetectedDomainEvent` | Không (cache+log) | - | Có (high-security) | Không |
| Tạo deposit order | `DepositOrderCreateRequestedDomainEvent` | Không | - | Tùy UX | Không |
| Provision payment link | `DepositPaymentLinkProvisionRequestedDomainEvent` | Không | - | Tùy UX | Không |
| Deposit thành công | `DepositPaymentSucceededDomainEvent` + `MoneyChangedDomainEvent` | Gián tiếp realtime wallet | Realtime | Có (thiếu in-app/email explicit) | Không |
| Deposit thất bại/hết hạn | `DepositWebhookReceivedDomainEvent` fail branch / `DepositOrderReconciliationRequestedDomainEvent` fail branch | Không | - | Có | Không |
| Tạo withdrawal request | `WithdrawalRequestedDomainEvent` | Có | Email | Có thể thiếu in-app “pending created” | Không |
| Withdrawal processed | `WithdrawalProcessedDomainEvent` | Có | InApp + Realtime(`notification.new`) | Không | Không |
| Chat message created | `ChatMessageCreatedDomainEvent` | Có | Realtime | Không | Có (dual path với fast-lane) |
| Conversation updated | `ConversationUpdatedDomainEvent` | Có | Realtime | Không | Có (dual path với fast-lane) |
| Unread changed | `UnreadCountChangedDomainEvent` | Có | Realtime | Không | Có (dual path với fast-lane) |
| Chat typing | `ChatTypingChangedDomainEvent` | Có | Realtime | Không | Có thể noisy theo tần suất typing |
| Escrow release | `EscrowReleasedDomainEvent` | Có | InApp + Realtime + Wallet realtime | Không | Không |
| Escrow refund | `EscrowRefundedDomainEvent` | Có | InApp + Realtime + Wallet realtime | Không | Không |
| Gacha result | `GachaPullCompletedDomainEvent` | Có | Realtime | Không | Không |
| Item granted from gacha | `ItemGrantedFromGachaDomainEvent` | Có | InApp + Realtime(inventory.changed) | Không | Có thể dày nếu pull nhiều |
| Card enhanced | `CardEnhancedDomainEvent` | Có | InApp + Realtime(inventory.changed) | Không | Có thể dày |
| Free draw granted | `FreeDrawGrantedDomainEvent` | Có | InApp + Realtime(reading.quota_changed) | Không | Thấp |
| AI stream completed | `ReadingSessionContentSyncRequestedDomainEvent`, `ReadingBillingCompletedDomainEvent` | Không user-facing | - | Có (nên có result-ready) | Không |
| Reading session revealed | `ReadingSessionRevealedDomainEvent` | Không (handler chỉ log) | - | Có | Không |
| Reader request reviewed | `ReaderRequestReviewRequestedDomainEvent` | Có | InApp + Realtime(`notification.new`) | Không | Không |
| Wallet snapshot changed | `WalletSnapshotChangedDomainEvent` | Có | Realtime wallet | Có thể thiếu audit in-app cho một số flow | Không |

## 3) Thiếu Notification Quan Trọng

1. **Deposit success/failure chưa có user-facing notification chuẩn**
- Evidence:
  - Success xử lý ở `DepositPaymentSucceededDomainEventHandler` nhưng không tạo in-app/email: `Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs:38-54`
  - Failure chỉ `MarkAsFailed`: `.../DepositWebhookReceivedDomainEventHandler.cs:93-102`, `.../DepositOrderReconciliationRequestedDomainEventHandler.cs:103-119`
- Vì sao quan trọng: tiền là high-trust action; user cần xác nhận thành công/thất bại rõ ràng.

2. **AI result ready chưa có notification**
- Evidence:
  - `ReadingSessionRevealedDomainEventHandler` chỉ log skip: `.../ReadingSessionRevealedDomainEventHandler.cs:33-39`
  - `CompleteAiStream` chủ yếu billing/telemetry/content sync: `.../CompleteAiStreamCommandHandler.WalletAndTelemetry.cs:63-131`, `...SessionPersistence.cs:28-38`
- Vì sao quan trọng: người dùng rời app có thể bỏ lỡ kết quả AI hoàn tất.

3. **Security events chưa notify chủ tài khoản**
- Evidence:
  - Login fail chỉ log: `.../Auth/UserAuthenticationFailedDomainEventHandler.cs:26-33`
  - Refresh replay chỉ cache+log: `.../Auth/RefreshTokenReplayDetectedDomainEventHandler.cs:49-87`
- Vì sao quan trọng: phát hiện truy cập bất thường là security-critical.

4. **Các event lỗi/quota hệ thống không có kênh user-facing**
- Ví dụ liên quan AI timeout/error hiện nằm ở telemetry/event nội bộ, chưa có kênh notification cho user khi cần retry thủ công.

## 4) Notification Thừa / Kém Chất Lượng

1. **Realtime duplicate do dual-path fast-lane + durable**
- Evidence:
  - Fast-lane forward `message.created.fast` và đồng thời emit `message.created`: `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.Forwarding.FastLane.cs:58-62`
  - Durable path cũng emit `message.created`: `Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.cs:133-141`
  - Mô hình tương tự với `conversation.updated` và unread delta.
- Impact: UI có thể nhận 2 tín hiệu cho cùng hành vi nghiệp vụ, gây spam/chớp state.

2. **Event semantic duplication trong auth**
- `LoginFailedDomainEvent` và `UserAuthenticationFailedDomainEvent` cùng miền nghĩa, nhưng `LoginFailedDomainEvent` không trigger.
- Evidence: event class có handler nhưng không call-site trigger (`triggers=0` trong inventory; `rg` chỉ thấy class/handler).

3. **Technical event lộ contract FE quá sâu**
- Payload fast-lane chứa field UI-specific (`lastMessagePreview`, `lastMessageType`, delta shape): `.../SendMessageCommandHandler.FastLane.cs:91-97`
- Rủi ro coupling BE contract chặt với state model FE.

## 5) Đánh Giá Chất Lượng Thiết Kế Event

### 5.1 Too granular

- Chat có nhiều lớp event cho cùng hành vi: `ChatMessageCreated`, `ConversationUpdated`, `UnreadCountChanged`, cộng thêm fast-lane delta variants.
- Dễ tạo duplication/noise nếu không có correlation + dedupe downstream.

### 5.2 Too coarse / overloaded

- Nhiều `RequestedDomainEvent` vừa mang input vừa bị mutate output (không còn immutable fact):
  - `DepositOrderCreateRequestedDomainEvent` có nhiều setter kết quả: `Backend/src/TarotNow.Domain/Events/DepositOrderCreateRequestedDomainEvent.cs:26-86`
  - `ReaderRequestSubmitRequestedDomainEvent` có `Submitted`, `RequestId`: `.../ReaderRequestSubmitRequestedDomainEvent.cs:54-62`
  - `ReadingSessionInitRequestedDomainEvent` chứa `Session`, `Cost...`: `.../ReadingSessionInitRequestedDomainEvent.cs:31-59`

### 5.3 Thiếu business meaning rõ ở một số event

- Một số event class không có XML summary rõ nghĩa business (inventory đánh dấu inferred), ví dụ `EscrowReleasedDomainEvent`, `ReadingBillingCompletedDomainEvent`.

### 5.4 Naming

- `*CommandHandlerRequestedDomainEvent` không phải ubiquitous language domain; đây là orchestration event ở application.
- Hệ quả: boundary domain/application khó đọc, team mới onboard khó phân biệt domain fact và command workflow.

## 6) Vi Phạm Kiến Trúc (CRITICAL)

### CRITICAL-1: Side-effect realtime chạy trước transaction commit

- Vị trí:
  - `SendMessage`: fast-lane publish trước durable domain events: `.../SendMessageCommandHandler.PersistenceFlow.cs:30-31`
  - `AcceptConversation`: `.../AcceptConversationCommandHandler.cs:72-75`
  - `MarkMessagesRead`: `.../MarkMessagesReadCommand.cs:82-105`
  - `Request/RespondConversationComplete`: `...RequestConversationCompleteCommandHandler.cs:72-95`, `...RespondConversationCompleteCommandHandler.cs:62-103`
- Vì sao sai:
  - Command pipeline commit transaction ở cuối: `Backend/src/TarotNow.Application/Behaviors/CommandTransactionBehavior.cs:39-41`
  - Khi publish realtime trước commit, có thể phát event cho dữ liệu chưa commit/rollback (ghost signal).
- Root cause:
  - Fast-lane được đặt trong command workflow inline thay vì publish từ outbox-driven post-commit stage.
- Cách fix tận gốc:
  1. Chuyển fast-lane sang “post-commit projector” dùng outbox (hoặc một queue in-memory commit callback an toàn).
  2. Nếu cần ultra-low latency: emit fast-lane với cờ `tentative=true` + bắt buộc reconcile bằng durable event có cùng correlation id.
  3. Bổ sung invariant test: không cho gọi `IChatRealtimeFastLanePublisher` trong transaction scope.

### CRITICAL-2: Duplicate emission cùng semantic event (fast + durable)

- Vị trí:
  - Fast-lane bridge phát thêm event durable contract: `...Forwarding.FastLane.cs:58-62`, `71-73`, `82-84`
  - Durable handlers phát lại contract tương ứng: `...RealtimeDomainEventHandlers.cs:84-95`, `133-141`, `170-180`
- Vì sao sai:
  - Không có correlation-level dedupe giữa hai lane ở client contract layer.
  - Dedupe bridge chỉ dựa `eventId` của fast-lane (`ShouldSkipDuplicatedFastLaneEvent`), trong khi durable lane không cùng `eventId`.
- Root cause:
  - Một semantic event được biểu diễn bằng 2 pipeline cùng lúc, không có merge protocol.
- Cách fix tận gốc:
  1. Chọn 1 source-of-truth cho contract `message.created|conversation.updated|chat.unread_changed` (khuyến nghị durable).
  2. Fast-lane chỉ gửi `*.delta.fast` riêng, FE tự merge và chờ durable confirm.
  3. Bắt buộc `correlationId` xuyên lane để dedupe deterministic.

### CRITICAL-3: In-app notification không idempotent storage khi retry

- Vị trí:
  - Base handler chỉ mark processed sau khi `HandleDomainEventAsync` thành công: `Backend/src/TarotNow.Application/Common/DomainEvents/IdempotentDomainEventNotificationHandler.cs:37-40`
  - Notification handlers pattern `CreateAsync` rồi `PublishAsync`, ví dụ escrow: `.../EscrowReleasedInAppNotificationHandler.cs:52-66`
  - Mongo create là `InsertOneAsync` không unique idempotency key: `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoNotificationRepository.cs:29-59`
- Vì sao sai:
  - Nếu Redis publish fail sau Mongo insert -> outbox retry chạy lại -> tạo notification trùng.
- Root cause:
  - Cross-store (Postgres outbox + Mongo notification + Redis) không có dedupe key chung trên notification document.
- Cách fix tận gốc:
  1. Thêm `dedupeKey` (ví dụ `eventType:eventBusinessId:userId`) vào notification document.
  2. Tạo unique index Mongo cho `dedupeKey`.
  3. Đổi `CreateAsync` sang upsert theo `dedupeKey`.
  4. Chỉ publish realtime khi insert/upsert là “newly-created”.

### CRITICAL-4: Domain event model bị commandized, phá ranh giới domain fact

- Vị trí:
  - 65 application events kiểu `*CommandHandlerRequestedDomainEvent`.
  - Ví dụ event chứa `Command` + `Result`: `Backend/src/TarotNow.Application/Features/Chat/Commands/AcceptConversation/AcceptConversationCommandHandler.EventOnly.cs:28-34`
  - Domain events mutable output fields: `.../DepositOrderCreateRequestedDomainEvent.cs:26-86`
- Vì sao sai:
  - Event không còn là immutable business fact; trở thành request/response transport object.
  - Handler event đang giữ business logic cốt lõi, trái tinh thần event handler = side-effects.
- Root cause:
  - Refactor “thin command handler” đã dồn orchestration vào event handlers.
- Cách fix tận gốc:
  1. Tách rõ `UseCaseCommandHandler` (business orchestration) và `DomainEventHandler` (side-effect hậu commit).
  2. Domain event chỉ immutable fact, không setter trạng thái xử lý.
  3. Bỏ dần pattern `CommandHandlerRequestedDomainEvent` ở các flow core.

### Không phát hiện vi phạm trực tiếp các rule sau

- Không thấy gọi trực tiếp `NotificationService` từ Command/Domain/Application.
- Không thấy Domain/Application reference trực tiếp namespace Infrastructure/Redis/Mongo/EF.

## 7) Trace Luồng Event (Rất Quan Trọng)

## 7.1 Flow A - User thanh toán nạp tiền

1. `CreateDepositOrderCommand` -> publish `DepositOrderCreateRequestedDomainEvent`
- `Backend/src/TarotNow.Application/Features/Deposit/Commands/CreateDepositOrder/CreateDepositOrderCommandHandler.cs:27-35`
2. Handler tạo order -> publish `DepositPaymentLinkProvisionRequestedDomainEvent`
- `.../DepositOrderCreateRequestedDomainEventHandler.cs:79-85`
3. Webhook vào -> `DepositWebhookReceivedDomainEvent`
- `.../ProcessDepositWebhookCommandHandler.cs:25-31`
4. Nếu success -> inline publish `DepositPaymentSucceededDomainEvent`
- `.../DepositWebhookReceivedDomainEventHandler.cs:127-136`
5. `DepositPaymentSucceededDomainEventHandler` credit wallet -> publish `MoneyChangedDomainEvent`
- `.../DepositPaymentSucceededDomainEventHandler.cs:81-90`, `118-127`
6. `MoneyChangedRealtimeHandler` -> realtime wallet
- `.../RealtimeDomainEventHandlers.cs:35-47`

Gap:
- Không có notification explicit cho deposit success/failure.

## 7.2 Flow B - AI hoàn tất / user nhận kết quả

1. Reveal command -> `ReadingSessionRevealRequestedDomainEvent`
- `.../RevealReadingSessionCommandHandler.cs:33-41`
2. Saga handler charge/collection/exp -> publish `MoneyChangedDomainEvent`
- `.../ReadingSessionRevealRequestedDomainEventHandler.BillingAndExp.cs:87-97`
3. Saga publish `ReadingSessionRevealedDomainEvent`
- `.../ReadingSessionRevealRequestedDomainEventHandler.Workflow.cs:144-152`
4. `ReadingSessionRevealedDomainEventHandler` chỉ log, không notify user
- `.../ReadingSessionRevealedDomainEventHandler.cs:33-39`
5. Complete AI stream publish telemetry + billing + content sync
- `.../CompleteAiStreamCommandHandler.WalletAndTelemetry.cs:63-131`
- `.../CompleteAiStreamCommandHandler.SessionPersistence.cs:28-38`

Gap:
- Thiếu notify “AI result ready”.

## 7.3 Flow C - Booking/Conversation lifecycle

1. User gửi tin đầu tiên -> `ChatMessageCreated`, `ConversationUpdated`, `UnreadCountChanged`, `ChatModerationRequested` + fast-lane deltas
- `.../SendMessageCommandHandler.PersistenceFlow.cs:30-31`, `109-146`
2. Reader accept -> `ConversationUpdated("accepted")` + fast-lane delta
- `.../AcceptConversationCommandHandler.cs:72-75`
3. Request/Respond complete -> `ConversationUpdated("complete_requested"|"complete_responded"|"completed")` + fast-lane
- `.../RequestConversationCompleteCommandHandler.cs:72-95`
- `.../RespondConversationCompleteCommandHandler.cs:62-103`
4. Escrow settle -> `EscrowReleasedDomainEvent` -> in-app + wallet realtime
- `.../EscrowSettlementService.State.cs:83-93`
- `.../EscrowReleasedInAppNotificationHandler.cs:42-67`

Inconsistency:
- Cùng semantic realtime phát bởi fast + durable (duplicate risk).

## 7.4 Flow D - Wallet thay đổi

1. Nhiều source publish `MoneyChangedDomainEvent`
- ví dụ deposit/withdraw/chat/admin/reading: `.../RealtimeDomainEventHandlers.cs:13-47` + call-sites trong inventory
2. Wallet realtime emit qua `wallet.balance_changed`
- `.../RealtimeDomainEventHandlers.cs:35-47`
3. Trường hợp không có delta dùng `WalletSnapshotChangedDomainEvent`
- `.../WalletSnapshotChangedRealtimeHandler.cs:30-41`

Gap:
- Wallet change high-impact chưa có policy notification user-facing phân loại theo severity.

## 8) Scalability & Reliability

### 8.1 Điểm tốt

1. Outbox write bắt buộc trong active transaction
- `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs:32-36`
2. Claim batch với `FOR UPDATE SKIP LOCKED`
- `.../OutboxBatchProcessor.cs:85-99`
3. Retry + backoff + dead-letter có sẵn
- `.../OutboxBatchProcessor.Processing.cs:93-127`
4. Handler idempotency infra có `outbox_handler_states` + `outbox_inline_handler_states`
- `.../OutboxHandlerIdempotencyService.cs:47-53`, `95-101`
5. Partition processing key để cải thiện ordering theo business key
- `.../OutboxBatchProcessor.Partitioning.cs:24-34`

### 8.2 Rủi ro chính

1. **Cross-store atomicity gap** (Postgres outbox vs Mongo notification vs Redis publish).
2. **Fast-lane bypass outbox ordering/atomicity** (pre-commit publish).
3. **Dead-letter recovery giới hạn** chỉ tự requeue 2 projection event types
- `Backend/src/TarotNow.Infrastructure/BackgroundJobs/ProjectionReconcileWorker.cs:19-23`
4. **Unknown event type dead-letter** nếu registry resolve fail
- `.../OutboxBatchProcessor.Processing.cs:58-61`

### 8.3 Kết luận reliability

- Durable outbox path: tốt.
- Notification end-to-end exactly-once: chưa đạt.
- Cần hardening idempotency ở notification storage + correlation across lanes.

## 9) Thiết Kế Realtime

### 9.1 Redis Pub/Sub

- Đang dùng đúng vai trò transport bridge sang SignalR: `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs:16-25,95-118`.
- Production bắt buộc Redis (good guardrail):
  - `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs:40-44`
  - `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs:99-107`

### 9.2 SignalR role

- SignalR chủ yếu delivery layer; architecture tests cấm broadcast trực tiếp từ controllers/hubs cho migrated events.
- Nhưng bridge có logic transform event khá dày (không chỉ “dumb delivery”), ví dụ `chat.unread_changed` đổi sang `conversation.updated` + timestamp bridge-time:
  - `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.Forwarding.cs:92-106`

### 9.3 Coupling BE <-> Realtime contract

- Fast-lane payload chứa field UI-centric (`lastMessagePreview`, `lastMessageType`):
  - `Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FastLane.cs:95-97`
- Đây là coupling đáng kể; thay đổi UI schema sẽ kéo theo BE contract changes.

## 10) Kế Hoạch Cải Tiến (Actionable Roadmap)

### CRITICAL

1. Vấn đề: Pre-commit fast-lane publish
- Root cause: fast-lane nằm trong command inline path.
- Fix: chuyển sang post-commit emitter/outbox-projected fast lane; thêm `correlationId` và state tentative/confirmed.
- Impact: loại ghost event, giảm race và inconsistency.

2. Vấn đề: Duplicate semantic events giữa fast-lane và durable
- Root cause: hai pipeline cùng emit cùng contract event name.
- Fix: tách namespace event (`*.delta.fast`) + FE merge strategy + dedupe bằng correlation.
- Impact: giảm spam, giảm flicker UI.

3. Vấn đề: Notification duplicate khi retry
- Root cause: Mongo insert không dedupe key.
- Fix: thêm `dedupeKey` + unique index + upsert.
- Impact: ngăn duplicate in-app notification ở lỗi transient.

4. Vấn đề: Event model commandized
- Root cause: dùng `CommandHandlerRequestedDomainEvent` làm execute vehicle.
- Fix: rollback về use-case command handlers + immutable domain facts hậu commit.
- Impact: rõ boundary, dễ maintain, đúng Event-Driven semantics.

### HIGH

1. Bổ sung notification cho deposit success/failure.
2. Bổ sung notification cho AI result ready/error timeout (phân severity).
3. Bổ sung security notification policy cho suspicious auth events.
4. Chuẩn hóa naming taxonomy event (Fact/Requested/Internal).
5. Thêm dashboard DLQ cho notification-related events.

### MEDIUM

1. Gộp/điều tiết chat event granularity để giảm noise.
2. Chuẩn hóa timestamp semantics (domain occurredAt vs bridge generated at).
3. Rà soát và xoá dead events (`LoginFailedDomainEvent`, `TokenRefreshedDomainEvent`, ... nếu không dùng).

### LOW

1. Bổ sung XML summary đầy đủ cho event chưa có meaning rõ.
2. Bổ sung contract tests cho anti-dup across fast/durable lane.

## 11) Target Architecture (Đề xuất)

### 11.1 Event naming convention

- `DomainFact` (immutable, past-tense): `PaymentSucceeded`, `EscrowReleased`, `ReadingCompleted`
- `UseCaseRequested` (application internal, không đi outbox external): nếu bắt buộc dùng thì đặt rõ namespace `ApplicationEvents.*`
- Cấm `*CommandHandlerRequestedDomainEvent` ở Domain namespace.

### 11.2 Mapping event -> notification

- Tạo `NotificationPolicyRegistry` (application layer):
  - Input: domain fact + context
  - Output: channels + template + priority + dedupe key + throttling key
- Mọi kênh (InApp/Email/Push/Realtime) đi qua policy này để tránh hardcode rải rác.

### 11.3 Handler separation

- `BusinessStateHandler`: mutate write-model only.
- `ProjectionHandler`: sync read-model.
- `NotificationHandler`: chỉ tạo dispatch intent (không chứa business decision).
- `DeliveryWorker`: gửi realtime/email/push sau khi persist notification record idempotent.

### 11.4 Multi-channel notification design

1. Persist `NotificationMessage` chuẩn hoá (id, dedupeKey, userId, type, payload, channels, status).
2. Delivery channel workers độc lập (EmailWorker, RealtimeWorker, PushWorker).
3. Retry per-channel + dead-letter per-channel.

### 11.5 Anti-spam

1. Debounce cho typing/unread delta theo conversation + user (window 500-1500ms).
2. Aggregation cho event dày (gacha/item/card upgrades): gom trong 10-30s.
3. Priority lane: security/financial events bypass aggregation.
4. Client dedupe theo `(correlationId, semanticType, entityId)`.

## 12) Kết Luận Cuối

- Production-ready theo rule hiện tại: **Chưa**.
- Rủi ro lớn nhất: **realtime phát trước commit + dual-path duplicate** gây bất nhất trạng thái user-facing.
- Lỗi kiến trúc nghiêm trọng nhất: **trộn command workflow vào domain event model** và để external side-effect chạy pre-commit.
- Maintainability score: **5.5/10**
- Scalability score: **6.5/10**

## Open Assumptions / Audit Limits

1. Inventory dựa trên static scan call-sites; dynamic reflection/runtime dispatch ngoài pattern có thể không hiện trong scan.
2. `Business meaning` của event thiếu XML summary được đánh dấu inferred.
3. Review tập trung notification/event/realtime; không audit full security hardening ngoài phạm vi này.

---

### Appendix A.1 - Domain Event Inventory (63 events)

| Event | Trigger Source | Trigger Location(s) | Business Meaning | Payload | Consumers |
|---|---|---|---|---|---|
| AchievementUnlockedDomainEvent | Service/Other | Backend/src/TarotNow.Infrastructure/Services/GamificationService.Reading.cs:43 | Khởi tạo sự kiện mở khóa thành tựu để publish tới các handler liên quan. Luồng xử lý: nhận userId/achievementCode và chốt OccurredAtUtc tại thời điểm tạo event. | UserId, AchievementCode, OccurredAtUtc | AchievementUnlockedDomainEventHandler (Backend/src/TarotNow.Application/Features/Gamification/EventHandlers/GamificationEventHandlers.cs) |
| AiStreamCompletionTelemetryRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.WalletAndTelemetry.cs:64 | Domain event yêu cầu ghi telemetry completion cho AI stream theo cơ chế bất đồng bộ. | UserId, AiRequestId, SessionId, RequestId, InputTokens, OutputTokens, LatencyMs, Status, ErrorCode, PromptVersion, OccurredAtUtc | AiStreamCompletionTelemetryRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/AiStreamCompletionTelemetryRequestedDomainEventHandler.cs) |
| CardEnhancedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryItemUsedDomainEventHandler.Effects.cs:92 | Domain event phát sinh khi item tác động lên chỉ số lá bài. | UserId, CardId, EnhancementType, ExpDelta, AttackDelta, DefenseDelta, UpgradeSucceeded, SourceItemCode, OccurredAtUtc | CardEnhancedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryCardEnhancedDomainEventHandler.cs)<br/>CardEnhancedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.InventoryState.cs) |
| ChatMessageCreatedDomainEvent | DomainEventHandler, Command | Backend/src/TarotNow.Application/DomainEvents/Handlers/ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler.cs:126<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.PersistenceFlow.cs:110 | Domain event phát sinh khi tin nhắn chat mới được tạo. | ConversationId, MessageId, SenderId, MessageType, ClientMessageId, OccurredAtUtc | ChatMessageCreatedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.cs) |
| ChatMessageReadDomainEvent | Command | Backend/src/TarotNow.Application/Features/Chat/Commands/MarkMessagesRead/MarkMessagesReadCommand.cs:97 | Domain event phát sinh khi một participant đã đọc tin nhắn trong conversation. | ConversationId, UserId, OccurredAtUtc | ChatMessageReadRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.ChatEphemeral.cs) |
| ChatModerationRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.PersistenceFlow.cs:136 | Domain event phát sinh khi một tin nhắn cần được đưa vào luồng kiểm duyệt. | MessageId, ConversationId, SenderId, MessageType, Content, CreatedAtUtc, OccurredAtUtc | ChatModerationRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ChatModerationRequestedDomainEventHandler.cs) |
| ChatOfferReceivedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.PersistenceFlow.cs:95 | Domain event phát sinh khi User gửi nội dung tư vấn đầu tiên, kích hoạt trạng thái cần phê duyệt từ Reader. | ConversationId, UserId, ReaderId, OfferExpiresAtUtc, OccurredAtUtc | ChatOfferReceivedEmailNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ChatOfferReceivedEmailNotificationHandler.cs) |
| ChatTypingChangedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Chat/Commands/PublishTypingState/PublishTypingStateCommand.cs:72 | Domain event phát sinh khi trạng thái typing của participant thay đổi. | ConversationId, UserId, IsTyping, OccurredAtUtc | ChatTypingChangedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.ChatEphemeral.cs) |
| CommunityCommentAddRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Community/Commands/AddComment/AddCommentCommand.cs:58 | Domain event yêu cầu tạo comment community theo mô hình command event-only. | PostId, AuthorId, Content, ContextDraftId, CreatedCommentId, AuthorDisplayName, AuthorAvatarUrl, CreatedAtUtc, MediaAttachStatus, OccurredAtUtc | CommunityCommentAddRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/CommunityCommentAddRequestedDomainEventHandler.cs) |
| CommunityMediaAttachRequestedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/CommunityCommentAddRequestedDomainEventHandler.cs:121<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/CommunityPostCreateRequestedDomainEventHandler.cs:115 | Domain event yêu cầu worker attach media community cho entity vừa tạo. | OwnerUserId, ContextType, ContextDraftId, ContextEntityId, MarkdownContent, OccurredAtUtc | CommunityMediaAttachRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/CommunityMediaAttachRequestedDomainEventHandler.cs) |
| CommunityPostCreatedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/CommunityPostCreateRequestedDomainEventHandler.cs:102 | Domain event phát sinh khi bài viết community mới được tạo. | AuthorId, PostId, OccurredAtUtc | CommunityPostCreatedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/BusinessDomainEventHandlers.cs) |
| CommunityPostCreateRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Community/Commands/CreatePost/CreatePostCommand.cs:60 | Domain event yêu cầu tạo post community theo mô hình command event-only. | AuthorId, Content, Visibility, ContextDraftId, CreatedPostId, AuthorDisplayName, AuthorAvatarUrl, CreatedAtUtc, MediaAttachStatus, OccurredAtUtc | CommunityPostCreateRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/CommunityPostCreateRequestedDomainEventHandler.cs) |
| CompletionTimeoutConversationSyncRequestedDomainEvent | Service/Other | Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.CompletionTimeouts.Helpers.cs:123 | Khởi tạo event đồng bộ completion-timeout projection. Luồng xử lý: giữ nguyên dữ liệu context conversation để event handler cập nhật Mongo idempotent. | ConversationId, ActorId, MessageContent, ResolvedAtUtc, OccurredAtUtc | CompletionTimeoutConversationSyncRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/CompletionTimeoutConversationSyncRequestedDomainEventHandler.cs) |
| ConversationAddMoneyAcceptedSyncRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommand.cs:85 | Khởi tạo event đồng bộ phản hồi add-money accept. Luồng xử lý: chụp toàn bộ context cần cho handler materialize message idempotent. | ConversationId, SenderUserId, OfferMessageId, ProposalId, ResponseMessageId, OccurredAtUtc | ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler.cs) |
| ConversationUpdatedDomainEvent | DomainEventHandler, Command | Backend/src/TarotNow.Application/DomainEvents/Handlers/ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler.cs:147<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/AcceptConversation/AcceptConversationCommandHandler.cs:73<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/CancelPendingConversation/CancelPendingConversationCommand.cs:75<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/MarkMessagesRead/MarkMessagesReadCommand.cs:93<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/OpenConversationDispute/OpenConversationDisputeCommand.cs:97<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.cs:68<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.cs:65<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationComplete/RequestConversationCompleteCommandHandler.cs:73,88<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/RespondConversationComplete/RespondConversationCompleteCommandHandler.cs:63,81,97<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.PersistenceFlow.cs:122<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/SubmitConversationReview/SubmitConversationReviewCommandHandler.cs:96 | Khởi tạo sự kiện cập nhật conversation cho luồng publish domain event. Luồng xử lý: nhận conversationId/type và giữ nguyên occurredAtUtc do caller cung cấp. | ConversationId, Type, OccurredAtUtc | ConversationUpdatedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.cs) |
| DailyCheckInCompletedDomainEvent | Command | Backend/src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs:150 | Domain event phát sinh khi người dùng check-in hằng ngày thành công. | UserId, CurrentStreak, BusinessDate, GoldRewarded, OccurredAtUtc | DailyCheckInCompletedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/BusinessDomainEventHandlers.cs) |
| DepositOrderCreateRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Deposit/Commands/CreateDepositOrder/CreateDepositOrderCommandHandler.cs:27 | Domain event yêu cầu tạo đơn nạp tiền và payment link PayOS. | UserId, PackageCode, IdempotencyKey, OrderId, Status, AmountVnd, BaseDiamondAmount, BonusGoldAmount, TotalDiamondAmount, PayOsOrderCode, PaymentLinkStatus, CheckoutUrl, QrCode, PaymentLinkId, ExpiresAtUtc, PaymentLinkFailureReason, OccurredAtUtc | DepositOrderCreateRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositOrderCreateRequestedDomainEventHandler.cs) |
| DepositOrderReconciliationRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Deposit/Commands/ReconcileMyDepositOrder/ReconcileMyDepositOrderCommandHandler.cs:27 | Domain event yêu cầu reconcile trạng thái một đơn nạp theo dữ liệu trực tiếp từ PayOS. | UserId, OrderId, Handled, OccurredAtUtc | DepositOrderReconciliationRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositOrderReconciliationRequestedDomainEventHandler.cs) |
| DepositPaymentLinkProvisionRequestedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositOrderCreateRequestedDomainEventHandler.cs:80 | Domain event yêu cầu tạo payment link cho đơn nạp đã được persist. | OrderId, PayOsOrderCode, OccurredAtUtc | DepositPaymentLinkProvisionRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentLinkProvisionRequestedDomainEventHandler.cs) |
| DepositPaymentSucceededDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositOrderReconciliationRequestedDomainEventHandler.cs:90<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositWebhookReceivedDomainEventHandler.cs:128 | Domain event phát sinh khi đơn nạp thanh toán thành công, dùng để cấp ví. | OrderId, UserId, DiamondAmount, BonusGoldAmount, ReferenceId, OccurredAtUtc | DepositPaymentSucceededDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs) |
| DepositWebhookReceivedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Deposit/Commands/ProcessDepositWebhook/ProcessDepositWebhookCommandHandler.cs:25 | Domain event nhận webhook PayOS để xử lý trạng thái nạp tiền. | RawPayload, Handled, OccurredAtUtc | DepositWebhookReceivedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositWebhookReceivedDomainEventHandler.cs) |
| EmailOtpIssuedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Auth/Commands/ForgotPassword/ForgotPasswordCommandHandler.cs:69<br/>Backend/src/TarotNow.Application/Features/Auth/Commands/SendEmailVerificationOtp/SendEmailVerificationOtpCommandHandler.cs:77 | Domain event phát sinh khi hệ thống phát hành OTP qua email. | UserId, Email, Subject, Body, Purpose, OccurredAtUtc | EmailOtpIssuedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/BusinessDomainEventHandlers.cs) |
| EscrowConversationSyncRequestedDomainEvent | Service/Other | Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.Workflow.cs:46<br/>Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoReleases.cs:78<br/>Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs:78<br/>Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.ExpiredOffers.Workflow.cs:33 | Domain event yêu cầu đồng bộ projection conversation/message sau khi escrow đã settle ở write-model. | ConversationId, TargetStatus, ActorId, MessageType, MessageContent, SyncReason, ResolvedAtUtc, OccurredAtUtc | EscrowConversationSyncRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowConversationSyncRequestedDomainEventHandler.cs) |
| EscrowRefundedDomainEvent | Command, Service/Other | Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.Workflow.cs:165<br/>Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.Workflow.cs:99<br/>Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.ExpiredOffers.Workflow.cs:112 | (No XML summary in event class; meaning inferred from name only) | ItemId, UserId, AmountDiamond, RefundSource, OccurredAtUtc | EscrowRefundedInAppNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowRefundedInAppNotificationHandler.cs)<br/>EscrowRefundedMoneyChangedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowMoneyChangedDomainEventHandlers.cs)<br/>EscrowRefundedNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/FinancialDomainEventLoggingHandlers.cs) |
| EscrowReleasedDomainEvent | Service/Other | Backend/src/TarotNow.Application/Services/EscrowSettlementService.State.cs:83 | (No XML summary in event class; meaning inferred from name only) | ItemId, PayerId, ReceiverId, GrossAmountDiamond, ReleasedAmountDiamond, FeeAmountDiamond, IsAutoRelease, OccurredAtUtc | EscrowReleasedInAppNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowReleasedInAppNotificationHandler.cs)<br/>EscrowReleasedMoneyChangedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowMoneyChangedDomainEventHandlers.cs)<br/>EscrowReleasedNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/FinancialDomainEventLoggingHandlers.cs) |
| FreeDrawGrantedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryItemUsedDomainEventHandler.Effects.cs:128 | Domain event phát sinh khi người dùng nhận lượt rút/trải bài miễn phí. | UserId, GrantedCount, SpreadCardCount, SourceItemCode, OccurredAtUtc | FreeDrawGrantedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryFreeDrawGrantedDomainEventHandler.cs)<br/>FreeDrawGrantedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.UserProfileState.cs) |
| GachaPullCompletedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.ReplayAndFinalize.cs:88 | Domain event follow-up sau khi pull gacha hoàn tất thành công. | UserId, PoolCode, PullCount, WasPityTriggered, OccurredAtUtc | GachaPullCompletedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.cs) |
| GachaPulledDomainEvent | Command | Backend/src/TarotNow.Application/Features/Gacha/Commands/PullGacha/PullGachaCommandHandler.cs:32 | Domain event phát sinh khi user thực hiện pull gacha. | UserId, PoolCode, Count, IdempotencyKey, IsIdempotentReplay, IsProcessingReplay, OperationId, CurrentPityCount, HardPityThreshold, WasPityTriggered, Rewards, OccurredAtUtc | GachaPulledDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.cs) |
| ItemGrantedFromGachaDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.ItemRewards.cs:29 | Domain event phát sinh khi gacha cấp item vào inventory. | UserId, ItemDefinitionId, ItemCode, QuantityGranted, PoolCode, PullOperationId, OccurredAtUtc | ItemGrantedFromGachaDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ItemGrantedFromGachaDomainEventHandler.cs)<br/>ItemGrantedFromGachaRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.InventoryState.cs) |
| ItemUsedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Inventory/Commands/UseInventoryItemCommandHandler.cs:50 | Domain event phát sinh khi người dùng sử dụng item trong kho đồ. | UserId, Quantity, ItemCode, TargetCardId, IdempotencyKey, OccurredAtUtc, IsIdempotentReplay, EffectSummaries | InventoryItemUsedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.InventoryState.cs)<br/>ItemUsedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryItemUsedDomainEventHandler.cs) |
| LoginFailedDomainEvent | (no trigger found) | (none) | Domain event phát sinh khi login thất bại. | IdentityHash, IpHash, ReasonCode, OccurredAtUtc | LoginFailedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/LoginFailedDomainEventHandler.cs) |
| LuckAppliedDomainEvent | (no trigger found) | (none) | Domain event phát sinh khi item may mắn được áp dụng. | UserId, LuckValue, SourceItemCode, OccurredAtUtc | LuckAppliedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryLuckAppliedDomainEventHandler.cs) |
| LuckyStarTitleUsedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryItemUsedDomainEventHandler.Effects.cs:42 | Domain event phát sinh khi user dùng item Lucky Star. | UserId, SourceItemCode, SourceIdempotencyKey, OccurredAtUtc | LuckyStarTitleUsedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryLuckyStarTitleUsedDomainEventHandler.cs) |
| MoneyChangedDomainEvent | DomainEventHandler, Command, Service/Other | Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs:82,119<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowMoneyChangedDomainEventHandlers.cs:35,84<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.ReplayAndFinalize.cs:101<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryLuckyStarTitleUsedDomainEventHandler.cs:84<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.BillingAndExp.cs:88<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalCreateRequestedDomainEventHandler.cs:133<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalProcessRequestedDomainEventHandler.Helpers.cs:38,62<br/>Backend/src/TarotNow.Application/Features/Admin/Commands/AddUserBalance/AddUserBalanceCommand.cs:82<br/>Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Events.cs:16<br/>Backend/src/TarotNow.Application/Features/Admin/Commands/UpdateUser/UpdateUserCommandHandler.Balance.cs:135<br/>Backend/src/TarotNow.Application/Features/Auth/Commands/VerifyEmail/VerifyEmailCommandHandler.cs:72<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs:78<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.PersistenceFlow.cs:81<br/>Backend/src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs:140<br/>Backend/src/TarotNow.Application/Features/CheckIn/Commands/PurchaseFreeze/PurchaseStreakFreezeCommandHandler.cs:69<br/>Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs:46<br/>Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandHandler.Rewards.cs:63<br/>Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.WalletAndTelemetry.cs:110<br/>Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.cs:92<br/>Backend/src/TarotNow.Infrastructure/BackgroundJobs/ReadingRevealSagaRepairWorker.cs:120 | Domain event phát sinh khi số dư ví người dùng thay đổi. | UserId, Currency, ChangeType, DeltaAmount, ReferenceId, OccurredAtUtc | MoneyChangedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.cs)<br/>MoneyChangedSpendingLeaderboardDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/MoneyChangedSpendingLeaderboardDomainEventHandler.cs) |
| MysteryPackOpenedDomainEvent | (no trigger found) | (none) | Domain event phát sinh khi người dùng mở mystery card pack. | UserId, SourceItemCode, OccurredAtUtc | MysteryPackOpenedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs) |
| PityTriggeredDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.ItemRewards.cs:42 | Domain event phát sinh khi pull gacha trigger pity force. | UserId, PoolCode, PullOperationId, RarityForced, OccurredAtUtc | (none) |
| QuestCompletedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandHandler.Rewards.cs:103 | Khởi tạo sự kiện quest completed để các handler hậu xử lý nhận thưởng chạy đồng bộ. Luồng xử lý: nhận dữ liệu quest/reward và chốt OccurredAtUtc tại thời điểm tạo event. | UserId, QuestCode, PeriodKey, RewardType, RewardAmount, OccurredAtUtc | QuestCompletedDomainEventHandler (Backend/src/TarotNow.Application/Features/Gamification/EventHandlers/GamificationEventHandlers.cs) |
| ReaderProfileUpdateRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reader/Commands/UpdateReaderProfile/UpdateReaderProfileCommandHandler.cs:27 | Domain event yêu cầu cập nhật hồ sơ Reader. | UserId, BioVi, BioEn, BioZh, DiamondPerQuestion, Specialties, YearsOfExperience, FacebookUrl, InstagramUrl, TikTokUrl, Updated, OccurredAtUtc | ReaderProfileUpdateRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderProfileUpdateRequestedDomainEventHandler.cs) |
| ReaderRequestReviewRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Admin/Commands/ApproveReader/ApproveReaderCommandHandler.cs:33 | Domain event yêu cầu admin duyệt hoặc từ chối đơn Reader. | RequestId, Action, AdminNote, AdminId, Processed, OccurredAtUtc | ReaderRequestReviewRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderRequestReviewRequestedDomainEventHandler.cs) |
| ReaderRequestSubmitRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reader/Commands/SubmitReaderRequest/SubmitReaderRequestCommandHandler.cs:27 | Domain event yêu cầu gửi đơn đăng ký Reader. | UserId, Bio, Specialties, YearsOfExperience, FacebookUrl, InstagramUrl, TikTokUrl, DiamondPerQuestion, ProofDocuments, Submitted, RequestId, OccurredAtUtc | ReaderRequestSubmitRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderRequestSubmitRequestedDomainEventHandler.cs) |
| ReaderStatusUpdateRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reader/Commands/UpdateReaderStatus/UpdateReaderStatusCommandHandler.cs:27 | Domain event yêu cầu cập nhật trạng thái hoạt động Reader. | UserId, Status, Updated, OccurredAtUtc | ReaderStatusUpdateRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderStatusUpdateRequestedDomainEventHandler.cs) |
| ReadingBillingCompletedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.WalletAndTelemetry.cs:121 | (No XML summary in event class; meaning inferred from name only) | UserId, AiRequestId, ReadingSessionRef, ChargeDiamond, FinalStatus, WasRefunded, OccurredAtUtc | ReadingBillingCompletedNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/FinancialDomainEventLoggingHandlers.cs) |
| ReadingCompletedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.cs:72 | Domain event phát sinh khi một phiên đọc bài hoàn tất. | UserId, OccurredAtUtc | ReadingCompletedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/BusinessDomainEventHandlers.cs) |
| ReadingSessionContentSyncRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.SessionPersistence.cs:29 | Domain event yêu cầu đồng bộ nội dung ReadingSession ở Mongo sau khi AI stream hoàn tất. | SessionId, AiRequestId, FollowupQuestion, FullResponse, OccurredAtUtc | ReadingSessionContentSyncRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionContentSyncRequestedDomainEventHandler.cs) |
| ReadingSessionInitRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reading/Commands/InitSession/InitReadingSessionCommandHandler.cs:32 | Domain event yêu cầu khởi tạo reading session. | UserId, SpreadType, Question, Currency, SessionId, CostGold, CostDiamond, CurrencyUsed, AmountCharged, Session, OccurredAtUtc | ReadingSessionInitRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionInitRequestedDomainEventHandler.cs) |
| ReadingSessionRevealedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.Workflow.cs:145 | Domain event phát sinh sau khi một reading session được reveal thành công. | UserId, SessionId, Language, RevealedCards, OccurredAtUtc | ReadingSessionRevealedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealedDomainEventHandler.cs) |
| ReadingSessionRevealRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Reading/Commands/RevealSession/RevealReadingSessionCommandHandler.cs:33 | Domain event yêu cầu reveal một reading session. | UserId, SessionId, Language, RevealedCards, IsIdempotentReplay, OccurredAtUtc | ReadingSessionRevealRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs) |
| RefreshTokenReplayDetectedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandHandler.Helpers.cs:174 | Domain event phát sinh khi phát hiện refresh token replay. | UserId, SessionId, FamilyId, SourceIpHash, OccurredAtUtc | RefreshTokenReplayDetectedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/RefreshTokenReplayDetectedDomainEventHandler.cs) |
| RefreshTokenRotatedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandHandler.cs:144 | Domain event phát sinh khi refresh token rotation thành công. | UserId, SessionId, OldTokenId, NewTokenId, AccessTokenJti, DeviceId, IpHash, UserAgentHash, OccurredAtUtc | RefreshTokenRotatedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/RefreshTokenRotatedDomainEventHandler.cs) |
| TitleGrantedDomainEvent | DomainEventHandler, Command, Service/Other | Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryLuckyStarTitleUsedDomainEventHandler.cs:53<br/>Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandHandler.Rewards.cs:83<br/>Backend/src/TarotNow.Application/Features/Gamification/EventHandlers/GamificationEventHandlers.cs:78 | Domain event phát sinh khi người dùng được cấp title. | UserId, TitleCode, Source, OccurredAtUtc | TitleGrantedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/BusinessDomainEventHandlers.cs)<br/>TitleGrantedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.UserProfileState.cs) |
| TokenRefreshedDomainEvent | (no trigger found) | (none) | Domain event phát sinh khi refresh token rotate thành công. | UserId, SessionId, OldTokenId, NewTokenId, AccessTokenJti, DeviceId, IpHash, UserAgentHash, OccurredAtUtc | TokenRefreshedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/TokenRefreshedDomainEventHandler.cs) |
| UnreadCountChangedDomainEvent | DomainEventHandler, Command | Backend/src/TarotNow.Application/DomainEvents/Handlers/ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler.cs:137<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/MarkMessagesRead/MarkMessagesReadCommand.cs:83<br/>Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.PersistenceFlow.cs:126 | Domain event phát sinh khi số lượng unread trong hội thoại thay đổi. | ConversationId, UserId, ReaderId, OccurredAtUtc | UnreadCountChangedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.cs) |
| UserAuthenticationFailedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandHandler.Helpers.cs:183 | Domain event phát sinh khi xác thực đăng nhập thất bại. | IdentityHash, IpHash, ReasonCode, OccurredAtUtc | UserAuthenticationFailedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserAuthenticationFailedDomainEventHandler.cs) |
| UserLoggedInDomainEvent | Command | Backend/src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandHandler.Helpers.cs:73 | Domain event phát sinh khi user đăng nhập thành công. | UserId, SessionId, DeviceId, UserAgentHash, IpHash, AccessTokenJti, OccurredAtUtc | UserLoggedInDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedInDomainEventHandler.cs) |
| UserLoggedOutDomainEvent | DomainEventHandler, Command | Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderRequestReviewRequestedDomainEventHandler.Auth.cs:15<br/>Backend/src/TarotNow.Application/Features/Auth/Commands/ResetPassword/ResetPasswordCommandHandler.cs:81<br/>Backend/src/TarotNow.Application/Features/Auth/Commands/RevokeToken/RevokeTokenCommandHandler.cs:80,178 | Domain event phát sinh khi user logout hoặc revoke sessions. | UserId, SessionId, RevokeAll, SessionIds, Reason, OccurredAtUtc | UserLoggedOutDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedOutDomainEventHandler.cs) |
| UserProfileProjectionSyncRequestedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/UserProfileUpdateRequestedDomainEventHandler.cs:60 | Domain event yêu cầu đồng bộ projection profile từ write-model (PG) sang read-model (Mongo). | UserId, DisplayName, AvatarUrl, SourceUpdatedAtUtc, OccurredAtUtc | UserProfileProjectionSyncRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/UserProfileProjectionSyncRequestedDomainEventHandler.cs) |
| UserProfileUpdateRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Profile/Commands/UpdateProfile/UpdateProfileCommandHandler.cs:28 | Domain event yêu cầu cập nhật hồ sơ người dùng. | UserId, DisplayName, DateOfBirth, PayoutBankName, PayoutBankBin, PayoutBankAccountNumber, PayoutBankAccountHolder, Updated, OccurredAtUtc | UserProfileUpdateRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/UserProfileUpdateRequestedDomainEventHandler.cs) |
| UserStatusChangedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Presence/Commands/PublishUserStatusChanged/PublishUserStatusChangedCommand.cs:98 | Domain event phát sinh khi trạng thái hiện diện của user thay đổi. | UserId, Status, OccurredAtUtc | UserStatusChangedReaderProjectionDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/UserStatusChangedReaderProjectionDomainEventHandler.cs)<br/>UserStatusChangedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/RealtimeDomainEventHandlers.UserProfileState.cs) |
| WalletSnapshotChangedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/EscrowMoneyChangedDomainEventHandlers.cs:47<br/>Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalProcessRequestedDomainEventHandler.Helpers.cs:49 | Domain event phát sinh khi snapshot ví thay đổi nhưng không làm đổi số dư khả dụng. | UserId, Currency, ChangeType, ReferenceId, OccurredAtUtc | WalletSnapshotChangedRealtimeHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/WalletSnapshotChangedRealtimeHandler.cs) |
| WithdrawalCreateRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs:46 | Domain event yêu cầu tạo một withdrawal request mới. | UserId, AmountDiamond, IdempotencyKey, UserNote, RequestId, Status, OccurredAtUtc | WithdrawalCreateRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalCreateRequestedDomainEventHandler.cs) |
| WithdrawalProcessedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalProcessRequestedDomainEventHandler.Helpers.cs:81 | Domain event phát sinh khi admin đã xử lý xong withdrawal request. | RequestId, UserId, AdminId, Action, Status, AmountDiamond, AdminNote, ProcessedAtUtc, OccurredAtUtc | WithdrawalProcessedAuditLogHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalProcessedAuditLogHandler.cs)<br/>WithdrawalProcessedInAppNotificationHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalProcessedInAppNotificationHandler.cs) |
| WithdrawalProcessRequestedDomainEvent | Command | Backend/src/TarotNow.Application/Features/Withdrawal/Commands/ProcessWithdrawal/ProcessWithdrawalCommand.cs:47 | Domain event yêu cầu admin xử lý withdrawal request. | RequestId, AdminId, Action, AdminNote, IdempotencyKey, OccurredAtUtc | WithdrawalProcessRequestedDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalProcessRequestedDomainEventHandler.cs) |
| WithdrawalRequestedDomainEvent | DomainEventHandler | Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalCreateRequestedDomainEventHandler.cs:144 | Domain event phát sinh khi yêu cầu rút tiền được tạo thành công. | RequestId, UserId, AmountDiamond, NetAmountVnd, BankName, BankAccountNumber, OccurredAtUtc | WithdrawalRequestedEmailDomainEventHandler (Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalRequestedEmailDomainEventHandler.cs) |

### Appendix A.2 - Event -> Notification Channel Map (Domain Events)

| Event | InApp | Email | Push | Realtime | Channels |
|---|---:|---:|---:|---:|---|
| AchievementUnlockedDomainEvent | N | N | Y | N | Push |
| AiStreamCompletionTelemetryRequestedDomainEvent | N | N | N | N | (none) |
| CardEnhancedDomainEvent | Y | N | N | Y | InApp, Realtime |
| ChatMessageCreatedDomainEvent | N | N | N | Y | Realtime |
| ChatMessageReadDomainEvent | N | N | N | Y | Realtime |
| ChatModerationRequestedDomainEvent | N | N | N | N | (none) |
| ChatOfferReceivedDomainEvent | N | Y | N | N | Email |
| ChatTypingChangedDomainEvent | N | N | N | Y | Realtime |
| CommunityCommentAddRequestedDomainEvent | N | N | N | N | (none) |
| CommunityMediaAttachRequestedDomainEvent | N | N | N | N | (none) |
| CommunityPostCreatedDomainEvent | N | N | N | N | (none) |
| CommunityPostCreateRequestedDomainEvent | N | N | N | N | (none) |
| CompletionTimeoutConversationSyncRequestedDomainEvent | N | N | N | N | (none) |
| ConversationAddMoneyAcceptedSyncRequestedDomainEvent | N | N | N | N | (none) |
| ConversationUpdatedDomainEvent | N | N | N | Y | Realtime |
| DailyCheckInCompletedDomainEvent | N | N | N | N | (none) |
| DepositOrderCreateRequestedDomainEvent | N | N | N | N | (none) |
| DepositOrderReconciliationRequestedDomainEvent | N | N | N | N | (none) |
| DepositPaymentLinkProvisionRequestedDomainEvent | N | N | N | N | (none) |
| DepositPaymentSucceededDomainEvent | N | N | N | N | (none) |
| DepositWebhookReceivedDomainEvent | N | N | N | N | (none) |
| EmailOtpIssuedDomainEvent | N | Y | N | N | Email |
| EscrowConversationSyncRequestedDomainEvent | N | N | N | N | (none) |
| EscrowRefundedDomainEvent | Y | N | N | Y | InApp, Realtime |
| EscrowReleasedDomainEvent | Y | N | N | Y | InApp, Realtime |
| FreeDrawGrantedDomainEvent | Y | N | N | Y | InApp, Realtime |
| GachaPullCompletedDomainEvent | N | N | N | Y | Realtime |
| GachaPulledDomainEvent | N | N | N | N | (none) |
| ItemGrantedFromGachaDomainEvent | Y | N | N | Y | InApp, Realtime |
| ItemUsedDomainEvent | N | N | N | Y | Realtime |
| LoginFailedDomainEvent | N | N | N | N | (none) |
| LuckAppliedDomainEvent | Y | N | N | N | InApp |
| LuckyStarTitleUsedDomainEvent | N | N | N | N | (none) |
| MoneyChangedDomainEvent | N | N | N | Y | Realtime |
| MysteryPackOpenedDomainEvent | Y | N | N | N | InApp |
| PityTriggeredDomainEvent | - | - | - | - | (missing in map) |
| QuestCompletedDomainEvent | N | N | Y | N | Push |
| ReaderProfileUpdateRequestedDomainEvent | N | N | N | N | (none) |
| ReaderRequestReviewRequestedDomainEvent | Y | N | N | Y | InApp, Realtime |
| ReaderRequestSubmitRequestedDomainEvent | N | N | N | N | (none) |
| ReaderStatusUpdateRequestedDomainEvent | N | N | N | N | (none) |
| ReadingBillingCompletedDomainEvent | N | N | N | N | (none) |
| ReadingCompletedDomainEvent | N | N | N | N | (none) |
| ReadingSessionContentSyncRequestedDomainEvent | N | N | N | N | (none) |
| ReadingSessionInitRequestedDomainEvent | N | N | N | N | (none) |
| ReadingSessionRevealedDomainEvent | N | N | N | N | (none) |
| ReadingSessionRevealRequestedDomainEvent | N | N | N | N | (none) |
| RefreshTokenReplayDetectedDomainEvent | N | N | N | N | (none) |
| RefreshTokenRotatedDomainEvent | N | N | N | N | (none) |
| TitleGrantedDomainEvent | N | N | N | Y | Realtime |
| TokenRefreshedDomainEvent | N | N | N | N | (none) |
| UnreadCountChangedDomainEvent | N | N | N | Y | Realtime |
| UserAuthenticationFailedDomainEvent | N | N | N | N | (none) |
| UserLoggedInDomainEvent | N | N | N | N | (none) |
| UserLoggedOutDomainEvent | N | N | N | N | (none) |
| UserProfileProjectionSyncRequestedDomainEvent | N | N | N | N | (none) |
| UserProfileUpdateRequestedDomainEvent | N | N | N | N | (none) |
| UserStatusChangedDomainEvent | N | N | N | Y | Realtime |
| WalletSnapshotChangedDomainEvent | N | N | N | Y | Realtime |
| WithdrawalCreateRequestedDomainEvent | N | N | N | N | (none) |
| WithdrawalProcessedDomainEvent | Y | N | N | Y | InApp, Realtime |
| WithdrawalProcessRequestedDomainEvent | N | N | N | N | (none) |
| WithdrawalRequestedDomainEvent | N | Y | N | N | Email |

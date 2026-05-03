# Notification System Refactor Plan (TarotNow)

- Created at: 2026-05-03 (Asia/Ho_Chi_Minh)
- Based on: `docs/notification-system-deep-review.md`
- Goal: fix toàn bộ vấn đề kiến trúc và chất lượng notification/realtime đã audit, đồng thời đáp ứng yêu cầu sản phẩm mới.

## 1. Quyết định đã khóa với bạn

1. Chat realtime contract: **Fast-lane only** cho message-center/navbar.
2. Message-center scope: **Active + unread**, có preview tin nhắn cuối, click item mở thẳng `/chat/{conversationId}`.
3. AI result ready: **không gửi notification**.
4. Fast-lane timing: **Hybrid tentative/confirmed**.
5. Deposit failure notification: **chỉ notify khi FAILED** (không notify cancelled/expired).
6. `ItemGrantedFromGacha`, `CardEnhanced`, `FreeDrawGranted`: **tắt in-app notification, giữ realtime state sync**.
7. Browser popup từ `useChatUnreadNotifications`: **tắt** để tránh spam.
8. Phạm vi triển khai message icon: **mobile-first**. Ưu tiên bề mặt mobile trước (header/bottom navigation), desktop triển khai parity cùng behavior trong cùng roadmap.

## 2. Non-Negotiable Guardrails (bắt buộc)

1. Side-effects chỉ nằm trong Event Handlers.
2. Command/Application/Domain không gọi trực tiếp Notification/Email/Redis/SignalR publisher nghiệp vụ.
3. Clean Architecture dependency direction: Domain -> Application -> Infrastructure -> Presentation.
4. Mọi thay đổi notification phải đi từ Domain Event.
5. Outbox là đường chính để đảm bảo reliability cho handler side-effects.
6. Rule 0 là blocker: nếu vi phạm test kiến trúc thì merge bị chặn.

## 3. Danh sách vấn đề cần fix (map với deep-review)

### CRITICAL-1: Pre-commit fast-lane publish
- Root cause: command flow đang gọi `IChatRealtimeFastLanePublisher` inline trước khi transaction commit.
- Risk: ghost event, state lệch khi rollback, race condition.
- Fix dứt điểm:
1. Loại bỏ mọi call fast-lane trực tiếp trong các command handlers chat.
2. Dùng event-handler post-commit (đọc từ outbox) để phát fast-lane.
3. Triển khai payload `phase: "tentative" | "confirmed"` + `correlationId` để giữ hybrid model nhưng vẫn an toàn transaction.

### CRITICAL-2: Duplicate semantic events giữa fast + durable
- Root cause: bridge đang forward cả `message.created.fast` và `message.created`, `conversation.updated.delta` và `conversation.updated`.
- Risk: spam, flicker UI, unread badge nhảy sai.
- Fix dứt điểm:
1. Chuẩn hóa contract chat-navbar chỉ consume fast-lane (`*.fast`/`*.delta`).
2. Ngừng phát durable chat semantic trùng cho luồng message-center.
3. Bridge chỉ map fast-lane theo namespace mới, không re-emit durable alias trùng nghĩa.

### CRITICAL-3: In-app notification chưa idempotent storage-level
- Root cause: Mongo `InsertOne` không có dedupe key unique.
- Risk: retry sau lỗi Redis có thể insert trùng notification.
- Fix dứt điểm:
1. Bổ sung `dedupeKey` vào notification document + DTO.
2. Tạo unique index Mongo cho `(user_id, dedupe_key)`.
3. Đổi `CreateAsync` sang upsert theo dedupe key.
4. Chỉ phát `notification.new` khi upsert tạo bản ghi mới.

### CRITICAL-4: Event model commandized quá mức
- Root cause: `*CommandHandlerRequestedDomainEvent` đang gánh orchestration core business.
- Risk: boundary domain/event bị mờ, maintainability thấp.
- Fix dứt điểm theo 2 pha:
1. Pha trong refactor này: chặn side-effects hạ tầng khỏi command orchestration, đẩy về hậu-commit handlers.
2. Pha tiếp theo: giảm dần requested-domain-events ở các flow core (chat/deposit/notification), tách use-case orchestration và domain facts immutable.

## 4. Target Architecture sau refactor

### 4.1 Chat Realtime Lane (theo quyết định fast-lane only + hybrid)
1. Domain facts (outbox): `ChatMessageCreatedDomainEvent`, `ConversationUpdatedDomainEvent`, `UnreadCountChangedDomainEvent`, `ChatMessageReadDomainEvent`.
2. Post-commit Chat Realtime Handler phát events:
- `message.created.fast` + `phase=tentative`
- `conversation.updated.delta` + `phase=tentative`
- `chat.unread.delta` + `phase=tentative`
3. Confirmation handler phát `phase=confirmed` cùng `correlationId` sau khi projection/state đã đồng bộ.
4. FE dedupe theo `(correlationId, eventType, conversationId, phase)`.

### 4.2 Notification Policy Registry
1. Tạo policy mapping tập trung theo domain event -> channel(s) -> template -> dedupe key.
2. Chat-related events: route vào message-center state, **không** tạo in-app notification bell.
3. Deposit:
- Success -> in-app + `notification.new`.
- Failed only -> in-app + `notification.new`.
4. Inventory noisy events theo yêu cầu: không tạo in-app.
5. AI result ready và login fail: không tạo notification.

### 4.3 Message Center Mobile-First
1. Mobile là bề mặt chính: thêm icon message ở shell mobile (header hoặc bottom navigation theo route), đặt gần entry thông báo.
2. Badge unread lấy từ `chat.unread-badge`.
3. UI mobile mở panel/sheet tối ưu màn nhỏ; desktop dùng popover cùng data-contract và hành vi.
4. Danh sách lấy từ `chat.inbox` (active conversations), hiển thị avatar, name, preview, time, unread badge.
5. Click item: mark read optimistic (nếu cần) rồi điều hướng `/chat/{conversationId}`.
6. Realtime cập nhật qua fast-lane events, không phụ thuộc `notification.new`.

## 5. Kế hoạch triển khai theo ưu tiên

## CRITICAL

### C1. Loại bỏ fast-lane side-effects khỏi command flows
- Vấn đề: pre-commit publish ở `SendMessage`, `AcceptConversation`, `MarkMessagesRead`, `Request/RespondConversationComplete`, `RespondConversationAddMoney`.
- Root cause: inject `IChatRealtimeFastLanePublisher` vào command handlers.
- Cách fix chính xác:
1. Gỡ dependency này khỏi command handlers.
2. Tạo dedicated post-commit handlers cho chat fast-lane output.
3. Bổ sung `correlationId` thống nhất xuyên events.
- Impact: loại ghost event, chuẩn Rule 0, ổn định realtime.

### C2. Dọn dual-path duplicate ở bridge + FE listeners
- Vấn đề: cùng semantic bị emit 2 lane.
- Root cause: bridge forward alias trùng + FE listen cả fast và durable.
- Cách fix chính xác:
1. Bridge không forward `message.created` từ fast-lane payload nữa.
2. `useChatRealtimeSync` và presence-registration chỉ subscribe tập event fast-lane chuẩn.
3. Xóa invalidation trùng giữa `useChatRealtimeSync` và presence chat invalidators.
- Impact: giảm spam/refetch storm, unread chính xác hơn.

### C3. Dedupe bền vững cho notification persistence
- Vấn đề: duplicate notification khi retry cross-store.
- Root cause: thiếu unique business key.
- Cách fix chính xác:
1. `NotificationDocument` thêm `dedupe_key`.
2. Mongo index unique `(user_id, dedupe_key)`.
3. Repository `CreateAsync` -> upsert semantics.
4. Handler publish realtime chỉ khi insert mới.
- Impact: gần exactly-once cho in-app notification.

## HIGH

### H1. Message-center UI/UX theo mẫu Messenger
- Vấn đề: chưa có message icon/dropdown riêng.
- Cách fix chính xác:
1. Tạo `MessageCenter` theo hướng mobile-first: mobile dùng panel/sheet, desktop dùng dropdown/popover.
2. Tích hợp entry-point ở mobile shell trước (header/bottom navigation), sau đó giữ parity trên desktop navbar.
3. Reuse conversation preview logic từ chat sidebar.
4. Badge unread theo query key `chat.unread-badge`.
5. Click item -> `/chat/{conversationId}`.
- Impact: tách rõ kênh chat vs notification, realtime UX tốt trên mobile là kênh chính.

### H2. Tách chat ra khỏi bell notification
- Vấn đề: kiến trúc kênh chưa phân tầng rõ ràng.
- Cách fix chính xác:
1. Notification policy chặn persist các event chat vào notifications collection.
2. FE bell dropdown thêm defensive filter cho nhóm type chat (nếu có dữ liệu cũ).
- Impact: bell sạch, message-center là nguồn duy nhất cho chat alerts.

### H3. Deposit in-app notifications (success + failed only)
- Vấn đề: thiếu user-facing financial signal.
- Cách fix chính xác:
1. Tạo domain handlers riêng cho `DepositPaymentSucceededDomainEvent` và failure transition event.
2. Template VI/EN/ZH + metadata deeplink wallet history.
3. Emit `notification.new` sau persist idempotent.
- Impact: tăng trust tài chính, giảm ticket support.

### H4. Tắt in-app noisy inventory notifications theo yêu cầu
- Vấn đề: `ItemGrantedFromGacha`, `CardEnhanced`, `FreeDrawGranted` gây noise.
- Cách fix chính xác:
1. Disable/remove in-app handlers cho 3 event này.
2. Giữ realtime state handlers (`inventory.changed`, `reading.quota_changed`) để UI vẫn cập nhật.
- Impact: giảm noise nhưng vẫn giữ tính realtime của state.

## MEDIUM

### M1. Chuẩn hóa i18n notification client theo VI/EN/ZH + fallback locale->vi
- Vấn đề: FE notification item hiện chủ yếu dùng VI/EN.
- Cách fix chính xác:
1. Mở rộng type contract FE có `titleZh/bodyZh`.
2. Utility resolve title/body fallback: locale -> vi -> en.
3. Cập nhật icon/type labels cho deposit success/failure.
- Impact: đúng rule i18n toàn cục.

### M2. Tắt browser notification popup cho chat
- Vấn đề: trùng tín hiệu với message-center.
- Cách fix chính xác:
1. Bỏ `window.Notification` branch ở `useChatUnreadNotifications`.
2. Giữ logic unread badge + realtime state.
- Impact: giảm spam, giảm nhiễu người dùng.

## LOW

### L1. Giảm dần commandized event model
- Vấn đề: maintainability thấp vì quá nhiều requested-domain-event mutable.
- Cách fix chính xác:
1. Ưu tiên flow chat/deposit/notification chuyển sang immutable domain facts.
2. Tách orchestration logic khỏi notification/realtime handlers.
- Impact: kiến trúc rõ ràng, dễ mở rộng lâu dài.

## 6. Thiết kế interface/contracts cần thay đổi

### Backend
1. `NotificationCreateDto` thêm `DedupeKey` và metadata điều hướng chuẩn.
2. `NotificationDocument` thêm `dedupe_key`.
3. Realtime chat payload thêm:
- `correlationId`
- `phase` (`tentative`/`confirmed`)
- `lane` (`fast`)
4. Notification type constants thêm:
- `deposit.success`
- `deposit.failed`

### Frontend
1. `NotificationItem` thêm `titleZh`, `bodyZh`.
2. Thêm message-center view model cho conversation preview:
- `conversationId`, `peerName`, `peerAvatar`, `lastMessagePreview`, `updatedAt`, `unreadCount`.
3. Mở rộng contracts cho `NavbarRightSection` và `BottomTabBar` để enable message-center theo mobile-first.

## 7. Trace luồng chính sau refactor

### A. User gửi chat message
1. Command ghi dữ liệu + publish domain events.
2. Outbox commit.
3. Chat fast-lane projector phát tentative events.
4. FE message-center cập nhật ngay.
5. Confirmation event phát sau khi projection đồng bộ, FE reconcile state.

### B. Deposit success
1. Webhook/reconcile -> `DepositPaymentSucceededDomainEvent`.
2. Wallet credit + `MoneyChangedDomainEvent`.
3. Deposit notification handler upsert in-app (`deposit.success`) + publish `notification.new`.
4. FE bell cập nhật badge/list.

### C. Deposit failed (FAILED only)
1. Payment transition sang failed.
2. Publish failure domain event.
3. Notification handler tạo `deposit.failed` (idempotent) + `notification.new`.

### D. Inventory reward/enhance/free draw
1. Event vẫn phát như cũ để state update.
2. In-app handler bị tắt cho 3 event đã chỉ định.
3. FE chỉ nhận realtime state update, không có bell notification.

## 8. Test plan bắt buộc

### Backend tests
1. Unit test: chat command handlers không còn inject/call fast-lane publisher trực tiếp.
2. Unit test: notification upsert dedupe behavior (insert mới vs duplicate retry).
3. Unit test: deposit success + failed only tạo đúng notification type.
4. Integration test: Redis bridge không còn emit duplicate semantic (`message.created` alias) từ fast lane.
5. Architecture test mới: cấm `IChatRealtimeFastLanePublisher` trong command handlers.

### Frontend tests
1. Hook test cho message-center realtime merge/dedupe theo `correlationId + phase`.
2. Mobile-first interaction test: icon badge, open panel/sheet, click điều hướng `/chat/{id}`.
3. Regression test: bell dropdown không chứa chat notification types.
4. Desktop parity test: desktop navbar vẫn đồng bộ badge/list như mobile contract.
5. Hook test: `useChatUnreadNotifications` không gọi browser Notification API.

### E2E/Manual
1. Gửi tin nhắn khi 2 tab mở song song -> không duplicate item/badge.
2. Deposit success -> có bell notification đúng locale.
3. Deposit failed -> chỉ xuất hiện khi FAILED.
4. Dùng item enhance/free draw/gacha reward -> không có bell notification, state vẫn cập nhật realtime.

## 9. Kế hoạch rollout

1. Phase 1 (Safety): thêm dedupe key/index + deploy tương thích ngược.
2. Phase 2 (Realtime): deploy fast-lane projector/bridge mới + FE listeners song song có feature flag.
3. Phase 3 (Cutover): tắt durable duplicate contracts và browser popup.
4. Phase 4 (Cleanup): remove code cũ + nâng coverage tests + bật architecture gate mới.

## 10. Rủi ro và giảm thiểu

1. Rủi ro lag nhẹ khi bỏ pre-commit emit.
- Mitigation: tối ưu outbox poll interval cho chat events, ưu tiên partition theo conversationId.
2. Rủi ro mismatch payload tentative/confirmed.
- Mitigation: schema version + contract test bridge/FE parser.
3. Rủi ro duplicate lịch sử cũ trong notifications.
- Mitigation: migration script đánh dấu/compact theo dedupe rules.

## 11. Definition of Done

1. Không còn fast-lane side-effect trực tiếp trong command handlers.
2. Không còn duplicate semantic event giữa fast và durable trên message-center contract.
3. In-app notification có dedupe key + unique index + upsert behavior.
4. Có message icon theo thiết kế mobile-first (mobile trước, desktop parity), realtime hoạt động đúng theo fast-lane.
5. Chat alerts không đi vào bell.
6. Deposit success + failed-only có in-app notifications.
7. 3 inventory events chỉ realtime state, không in-app.
8. AI result ready và login fail không phát notification.
9. Tất cả test kiến trúc + unit/integration liên quan pass.

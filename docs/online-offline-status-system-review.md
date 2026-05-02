# Online/Offline Status System Review (Antigravity Compliant)

## 1. System Overview
- Kiến trúc hiện tại của luồng presence đã đi theo CQRS + Event-Driven:
  - `PresenceHub` nhận lifecycle realtime (`OnConnected`, `OnDisconnected`, `Heartbeat`).
  - `PublishUserStatusChangedCommand` (Application Command) publish domain event qua MediatR.
  - `IDomainEventPublisher` ghi `UserStatusChangedDomainEvent` vào transactional outbox PostgreSQL.
  - `OutboxProcessorWorker` dispatch event tới handlers idempotent.
  - `UserStatusChangedRealtimeHandler` publish lên Redis channel `realtime:user-state` với event `user.status_changed`.
  - `RedisRealtimeSignalRBridgeService` forward từ Redis Pub/Sub sang SignalR.
  - Frontend `usePresenceConnection` nhận `user.status_changed` và invalidate query reader.
- Dữ liệu hiển thị status reader đang đến từ Reader Profile (Mongo), có overlay realtime qua `_presenceTracker.IsOnline` tại API query (`ReaderController.ApplyPresenceStatus`).

## 2. Event-Driven Flow

### End-to-end flow: User connect -> FE render
| Step | Event / Action | Handler / Component | Data | Layer | Rule 0 |
|---|---|---|---|---|---|
| 1 | SignalR connect | `PresenceHub.OnConnectedAsync` | `userId`, `connectionId` | API/Realtime | Hợp lệ (transport lifecycle) |
| 2 | Command dispatch | `IMediator.Send(PublishUserStatusChangedCommand)` | `{ userId, status: "online" }` | Application entry | Hợp lệ |
| 3 | Requested domain event | `PublishUserStatusChangedCommandHandler` -> `PublishUserStatusChangedCommandHandlerRequestedDomainEvent` | command payload | Application | Hợp lệ |
| 4 | Domain event publish | `PublishUserStatusChangedCommandHandlerRequestedDomainEventHandler` | `UserStatusChangedDomainEvent { userId, status, occurredAtUtc }` | Application | Hợp lệ |
| 5 | Persist event | `MediatRDomainEventPublisher` (outbox write) | outbox row | Infrastructure | Hợp lệ |
| 6 | Outbox dispatch | `OutboxProcessorWorker` + `OutboxBatchProcessor.DispatchAsync` | deserialize + MediatR publish | Infrastructure | Hợp lệ |
| 7a | Realtime side-effect | `UserStatusChangedRealtimeHandler` | publish `user.status_changed` | Application handler -> Infrastructure | Hợp lệ |
| 7b | Projection side-effect | `UserStatusChangedReaderProjectionDomainEventHandler` | update `ReaderProfile.Status` | Application handler -> Infrastructure | Hợp lệ |
| 8 | Redis Pub/Sub consume | `RedisRealtimeBridgeSource` | envelope JSON | Infrastructure | Hợp lệ |
| 9 | SignalR broadcast | `RedisRealtimeSignalRBridgeService.ForwardPresenceEventAsync` | `(userId, status)` | API/Realtime bridge | Hợp lệ |
| 10 | FE receive + invalidate | `registerPresenceDomainEventHandlers` | invalidate `['readers']`, `['reader-profile', userId]` | Frontend app | Hợp lệ |
| 11 | FE refetch + render | React Query + Readers UI | latest profile/status | Frontend UI | Hợp lệ |

### End-to-end flow: User disconnect
| Step | Event / Action | Handler / Component | Data | Layer | Rule 0 |
|---|---|---|---|---|---|
| 1 | SignalR disconnect | `PresenceHub.OnDisconnectedAsync` | `userId`, `connectionId` | API/Realtime | Hợp lệ |
| 2 | Connection state update | `_presenceTracker.MarkDisconnected` + `HasActiveConnection` | active connection count | Realtime infra | Hợp lệ |
| 3 | If last connection, publish offline command | `PublishUserStatusChangedCommand` | `{ userId, status: "offline" }` | Application entry | Hợp lệ |
| 4+ | Same as connect flow (outbox -> handlers -> redis -> bridge -> FE) | Same chain | same schema | Mixed | Hợp lệ |

### End-to-end flow: Timeout cleanup
| Step | Event / Action | Handler / Component | Data | Layer | Rule 0 |
|---|---|---|---|---|---|
| 1 | Scan timed out users | `PresenceTimeoutBackgroundService` | timed-out userIds | API hosted service | Hợp lệ |
| 2 | Remove transient presence state | `_presenceTracker.RemoveUser` | userId | Realtime infra | Hợp lệ |
| 3 | Publish offline command | `IMediator.Send(PublishUserStatusChangedCommand)` | `{ userId, status: "offline" }` | Application entry | Hợp lệ |
| 4+ | Same outbox/handler/redis/bridge flow | Same chain | same schema | Mixed | Hợp lệ |

## 3. Realtime Lifecycle
- Khi user ONLINE:
  - `OnConnectedAsync` ghi connection set, join group `user:{userId}`, publish `online` event.
  - FE self nhận `user.status_changed`; observer clients chỉ nhận nếu đã subscribe `presence:watch:user:{userId}`.
- Khi user OFFLINE:
  - `OnDisconnectedAsync` chỉ publish `offline` khi không còn active connections (multi-tab safe).
  - Timeout worker cũng có thể publish `offline` nếu heartbeat quá hạn và không active connections.
- Mất kết nối / timeout:
  - FE reconnect theo schedule runtime policy.
  - Tracker dựa vào connection set + last activity; timeout default 15 phút, scan mỗi 60s.
- Reconnect:
  - FE `onreconnected` trigger invalidation; với fix mới, coordinator clear + resync observer groups.
  - Server `OnConnectedAsync` publish lại `online`.
- Multiple connections (multi-tab/device):
  - `HasActiveConnection` chặn offline false-positive khi còn tab/device khác.
- Nhiều instance:
  - Redis presence tracker + SignalR Redis backplane + Redis bridge cho fanout liên node.
  - Nếu Redis không khả dụng, fallback in-memory làm state không nhất quán cross-instance.

## 4. Timeline Analysis

### Timeline chuẩn
- `T0`: client establish SignalR `/api/v1/presence`.
- `T1`: `PublishUserStatusChangedCommand` được gửi (online/offline).
- `T2`: requested event handler publish `UserStatusChangedDomainEvent` vào outbox (trong transaction).
- `T3`: outbox worker pick & dispatch; realtime handler publish Redis envelope.
- `T4`: Redis bridge consume và broadcast SignalR `user.status_changed`.
- `T5`: FE listener invalidate query; query refetch; UI render trạng thái mới.

### Delay/race/lost/out-of-order đã xác định
- Delay tự nhiên:
  - outbox poll interval + worker scheduling + FE refetch latency.
- Race condition:
  - connect/disconnect liên tiếp nhanh có thể tạo chuỗi online/offline sát nhau.
- Lost event (đã có root-cause cụ thể):
  - trước fix, dùng `ConnectionAborted` token khi dispatch command trong `PresenceHub` có thể hủy send khi socket đóng nhanh.
- Out-of-order potential:
  - outbox có partitioning logic trong batch, nhưng multi-worker/multi-instance vẫn có cửa sổ reorder xuyên batch.
- Pub/Sub drop:
  - bridge queue bounded (`DropOldest`) có thể làm rơi message khi quá tải.

## 5. Rule Violations (CRITICAL)

### Vi phạm đã phát hiện
1. `PresenceTimeoutBackgroundService` ghi trực tiếp `IReaderProfileRepository.UpdateAsync` (trước khi sửa).
- Vi phạm Rule 0: side-effect business projection chạy trực tiếp ở service thay vì qua domain event handler.
- Trạng thái hiện tại: đã sửa, side-effect projection dời vào `UserStatusChangedReaderProjectionDomainEventHandler`.

2. Observer delivery thiếu trong bridge (trước khi sửa).
- `user.status_changed` chỉ gửi `user:{userId}`, không gửi cho clients đang xem reader đó.
- Đây là sai event flow realtime (không đến consumer cần nhận), gây UI giữ trạng thái cũ.
- Trạng thái hiện tại: đã sửa, broadcast tới cả `user:{userId}` và `presence:watch:user:{userId}`.

### Không thấy vi phạm Rule 0 còn lại trong luồng online/offline sau khi sửa
- Command/handler presence không gọi repository/service trực tiếp trong business command path.
- Side-effects (Redis publish, Reader projection update) đều đi qua domain event handlers.

## 6. Potential Issues
- Redis fallback in-memory trong môi trường nhiều instance sẽ gây presence mismatch cross-node.
- Redis Pub/Sub là at-most-once; bridge queue đầy có thể drop event (`DropOldest`).
- FE observer subscribe hiện cap 300, hub cap 200/request; với watchlist cực lớn cần batching/chunking rõ ràng.
- Outbox reorder xuyên batch vẫn là risk nhỏ trong hệ phân tán (đặc biệt khi online/offline churn cao).
- Integration matrix test full bridge cần Docker; chưa verify E2E runtime trong môi trường hiện tại.

## 7. Hypotheses

### Hypothesis 1 (Primary): event không tới observer clients
- Mô tả lỗi: user A online, user B đang mở reader directory/profile của A nhưng không nhận `user.status_changed`.
- Vi phạm rule: sai realtime fanout flow (event không tới đúng consumer group).
- Điều kiện: B không nằm group `user:{A}`, chỉ xem A như observer.
- Reproduce:
  1. Mở 2 user A/B.
  2. B vào page danh sách reader, A connect/disconnect.
  3. B không thấy status đổi realtime.
- Verify log/tracing:
  - Kiểm tra `RedisRealtimeSignalRBridgeService` log broadcast target groups.
  - FE log `[PresenceRealtimeSync] UserStatusChanged received` ở B.

### Hypothesis 2 (Primary): FE không bật presence connection trên route readers/profile
- Mô tả lỗi: user đang online nhưng UI vẫn offline vì page đó không subscribe realtime.
- Vi phạm rule: không vi phạm Rule 0 backend, nhưng sai lifecycle consumption ở client.
- Điều kiện: route không thuộc tập route được bật realtime (trước fix).
- Reproduce:
  1. Vào trang `/readers` hoặc `/readers/{id}` bằng user B.
  2. User A online/offline thay đổi.
  3. B không nhận event cho đến khi refresh/manual refetch.
- Verify:
  - Browser network: không có kết nối `/api/v1/presence` khi ở route affected.
  - FE logs không có lifecycle reconnect/event receive.

### Hypothesis 3 (Secondary): command dispatch bị cancel khi socket đóng nhanh
- Mô tả lỗi: offline/online event không vào outbox khi disconnect/reconnect nhanh.
- Vi phạm rule: không phải kiến trúc, nhưng bug reliability do cancellation coupling.
- Điều kiện: `ConnectionAborted` bị trigger trước `mediator.Send` hoàn tất.
- Reproduce:
  1. Connect rồi close tab ngay lập tức.
  2. Quan sát outbox thiếu event tương ứng.
- Verify:
  - Correlate logs `PresenceHub` với outbox rows theo userId/time.
  - So sánh số disconnect với số `UserStatusChangedDomainEvent` persisted.

### Hypothesis 4 (Secondary): Redis connection-set lease hết hạn trong session dài
- Mô tả lỗi: user vẫn connected nhưng key connection hết TTL -> tracker coi offline.
- Vi phạm rule: không.
- Điều kiện: session dài + lease không được gia hạn định kỳ.
- Reproduce:
  1. Giữ một kết nối mở lâu.
  2. Quan sát TTL key `presence:user:{id}:connections` giảm về hết.
  3. API query trả offline dù tab còn mở.
- Verify:
  - Redis `TTL` key theo thời gian.
  - So sánh heartbeat log và TTL refresh behavior.

## 8. Root Cause
- Root cause chính (đã xác định):
  1. `user.status_changed` chỉ routed tới group self (`user:{userId}`), thiếu observer group routing.
  2. FE presence connection không luôn bật ở các route reader nên không nhận event lifecycle.
- Root cause phụ (gây tăng xác suất sai lệch):
  3. Dispatch presence command buộc theo `ConnectionAborted` token có thể làm mất event.
  4. Redis connection lease không được refresh chắc chắn theo heartbeat gây false offline trên session dài.
- Vì sao gây “Online nhưng hiển thị Offline”:
  - UI status phụ thuộc query cache + invalidation sau `user.status_changed`.
  - Khi event không đến được client đang quan sát, hoặc client không có presence connection, cache không refresh -> giữ trạng thái `offline` cũ.

## 9. Fix Proposal (Rule 0 Compliant)

### Đã triển khai
1. Observer fanout chuẩn event-flow
- `PresenceHub`: thêm `SubscribeUserStatusObservers` / `UnsubscribeUserStatusObservers`.
- `RedisRealtimeSignalRBridgeService`: `user.status_changed` broadcast tới cả self + observer group.

2. FE observer lifecycle sync
- Thêm coordinator theo dõi active queries `['readers', ...]` và `['reader-profile', userId]`.
- Auto subscribe/unsubscribe groups theo diff; resync khi reconnect.

3. Bật presence realtime cho toàn bộ authenticated routes (trừ auth/admin/legal)
- Đảm bảo reader directory/profile luôn có realtime channel khi user đã xác thực.

4. Loại bỏ direct repository side-effect khỏi timeout service (Rule 0)
- `PresenceTimeoutBackgroundService` chỉ remove presence state + publish offline command.
- Projection update dời về `UserStatusChangedReaderProjectionDomainEventHandler`.

5. Reliability fixes
- Không dùng `ConnectionAborted` token khi publish presence status trong `PresenceHub`.
- `RedisUserPresenceTracker.RecordHeartbeat` chủ động renew TTL key connections.

### Tuân thủ Rule 0
- Mọi side-effects business (projection update, realtime publish) đều qua domain event handlers idempotent.
- Không có direct repository write trong service workflow timeout sau fix.

## 10. Improvements & Monitoring

### Logging (Serilog structured logs)
- Bắt buộc thêm correlation fields: `userId`, `connectionId`, `status`, `outboxMessageId`, `eventName`, `channel`, `hubGroup`.
- Log checkpoints tại: Hub connect/disconnect, command send, outbox enqueue, outbox processed, redis publish, bridge fanout, FE receive.

### Distributed tracing (OpenTelemetry)
- Tạo span chain:
  - `PresenceHub.OnConnectedAsync/OnDisconnectedAsync`
  - `PublishUserStatusChangedCommand`
  - `OutboxDispatch(UserStatusChangedDomainEvent)`
  - `RedisPublish(user.status_changed)`
  - `SignalRBroadcast(user.status_changed)`
- Propagate trace-id vào Redis envelope metadata (nếu mở rộng schema).

### Event audit log
- Lưu audit stream riêng cho presence status transitions (`online/offline`, source: connect/disconnect/timeout).
- Cho phép replay/forensics khi user report mismatch.

### Realtime monitoring
- Dashboard metrics:
  - outbox pending/failed/dead-letter,
  - bridge queue depth/drop count,
  - publish-to-broadcast latency p95/p99,
  - presence status mismatch rate (tracker online vs reader profile offline).

### Dead-letter / retry strategy
- Outbox đã có retry + dead-letter.
- Khuyến nghị bổ sung playbook tự động:
  - alert khi dead-letter tăng,
  - job reconcile riêng cho `UserStatusChangedDomainEvent` dead-letter,
  - admin endpoint replay selective theo `userId` + time range.

---

## Verification Executed
- Frontend tests:
  - `usePresenceConnection.test.tsx` + `usePresenceConnection.registration.test.ts` passed (12/12).
- Backend tests:
  - `UserStatusChangedReaderProjectionDomainEventHandlerTests` passed (4/4).
  - `PresenceHubTests` passed (4/4).
- E2E bridge routing matrix:
  - Không chạy được trong môi trường hiện tại do Docker/Testcontainers unavailable.

## Required Extra Evidence (để kết luận production-grade 100%)
- Structured logs thực tế theo một phiên connect/disconnect/reconnect của cùng `userId` trên multi-instance.
- Metrics bridge drop count và outbox lag trong giờ cao điểm.
- Trace end-to-end xác nhận `T0 -> T5` latency budget.

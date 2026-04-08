# Call V2 LiveKit Contract (Web + Mobile)

## 1) Scope
- Big-bang cutover sang LiveKit Cloud.
- Chỉ hỗ trợ `1-1 call` (`audio`, `video`).
- Không recording.
- Không migrate lịch sử call cũ.
- SignalR chỉ dùng cho event trạng thái nghiệp vụ.

## 2) Canonical State Machine
- `requested -> accepted -> joining -> connected -> ending -> ended`
- Nhánh lỗi:
- `requested -> ended` (ring timeout/reject/cancel)
- `accepted|joining -> failed` (join timeout/network/token)

## 3) Timeout Policy (server source of truth)
- `ring_timeout`: hết thời gian đổ chuông.
- `join_timeout`: hết thời gian chờ 2 bên join room.
- `reconnect_grace_period`: cửa sổ reconnect sau gián đoạn mạng.

Client phải lấy timeout từ `CallJoinTicketDto.timeouts` và dùng thống nhất.

## 4) DTO Contract
### `CallSessionV2`
```json
{
  "id": "string",
  "conversationId": "string",
  "roomName": "string",
  "initiatorId": "string",
  "calleeId": "string",
  "type": "audio|video",
  "status": "requested|accepted|joining|connected|ending|ended|failed",
  "createdAt": "2026-04-09T10:20:30Z",
  "updatedAt": "2026-04-09T10:20:30Z",
  "acceptedAt": "2026-04-09T10:20:40Z",
  "connectedAt": "2026-04-09T10:20:50Z",
  "endedAt": "2026-04-09T10:25:50Z",
  "endReason": "normal|timeout_server|join_timeout|disconnected|cancelled|rejected|..."
}
```

### `CallTimeoutsDto`
```json
{
  "ringTimeoutSeconds": 60,
  "joinTimeoutSeconds": 45,
  "reconnectGracePeriodSeconds": 15
}
```

### `CallJoinTicketDto`
```json
{
  "session": { "id": "..." },
  "liveKitUrl": "wss://<project>.livekit.cloud",
  "accessToken": "<jwt>",
  "participantIdentity": "user:<userId>",
  "timeouts": {
    "ringTimeoutSeconds": 60,
    "joinTimeoutSeconds": 45,
    "reconnectGracePeriodSeconds": 15
  }
}
```

## 5) REST API Contract (`/api/v1`)
### `POST /calls/start`
Request:
```json
{
  "conversationId": "string",
  "type": "audio|video"
}
```
Response: `CallJoinTicketDto` (caller ticket).

### `POST /calls/{id}/accept`
Response: `CallJoinTicketDto` (callee ticket).

### `POST /calls/{id}/end`
Request:
```json
{
  "reason": "normal|cancelled|rejected|timeout|..."
}
```
Response: `CallSessionV2` (idempotent).

### `POST /calls/{id}/token`
Response: `CallJoinTicketDto` (re-issue token cho reconnect).

### `POST /realtime/livekit/webhook`
- Server-to-server endpoint nhận room/participant events.
- Không dùng trực tiếp từ mobile/web client.

## 6) SignalR Contract (`CallHub`)
Legacy signaling methods `SendOffer/SendAnswer/SendIceCandidate` đã bị khóa.

### Event: `call.incoming`
```json
{
  "sessionId": "string",
  "conversationId": "string",
  "status": "requested",
  "reason": null,
  "timeouts": {
    "ringTimeoutSeconds": 60,
    "joinTimeoutSeconds": 45,
    "reconnectGracePeriodSeconds": 15
  },
  "session": { "id": "..." }
}
```

### Event: `call.accepted`
```json
{
  "sessionId": "string",
  "conversationId": "string",
  "status": "accepted",
  "reason": null,
  "timeouts": {
    "ringTimeoutSeconds": 60,
    "joinTimeoutSeconds": 45,
    "reconnectGracePeriodSeconds": 15
  },
  "session": { "id": "..." }
}
```

### Event: `call.ended`
```json
{
  "sessionId": "string",
  "conversationId": "string",
  "status": "ended|failed",
  "reason": "normal|timeout_server|join_timeout|disconnected|...",
  "session": { "id": "..." }
}
```

### Event: `call.error`
```json
{
  "errorKey": "legacy_call_disabled|...",
  "message": "string"
}
```

## 7) Error Code Matrix
API lỗi nghiệp vụ trả qua `ProblemDetails.extensions.errorCode`.

- `CALL_NOT_ALLOWED`: user không có quyền thao tác call/session.
- `CALL_ALREADY_ACTIVE`: conversation đã có active session.
- `MEDIA_PERMISSION_DENIED`: client không có media permission.
- `JOIN_TIMEOUT`: hết thời gian join.
- `TOKEN_EXPIRED`: token/session không còn hiệu lực.
- `ROOM_UNAVAILABLE`: không tạo/không truy cập được room.

## 8) Retry Policy
- `start/accept`: chỉ retry khi lỗi network/5xx; không retry khi `CALL_ALREADY_ACTIVE` hoặc `CALL_NOT_ALLOWED`.
- `token`: retry theo exponential backoff ngắn (`1s -> 2s -> 4s`, tối đa 3 lần).
- `end`: idempotent, có thể retry an toàn khi timeout/network error.
- SignalR reconnect: dùng retry built-in; nếu quá `reconnect_grace_period` thì chuyển call sang `failed`.

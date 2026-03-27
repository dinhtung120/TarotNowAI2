# Chat v2 Implementation Plan (Phased, Ready-to-Code)

## Locked Decisions
1. Rule cảnh báo/freeze Reader: `> 3 dispute trong 7 ngày`.
2. SLA Reader: `6h / 12h / 24h`.
3. Thêm trạng thái Reader `away`.
4. Worker timeout/auto-actions chạy `mỗi 1 giờ`.

## Public Contracts (Implemented)
- Reader status: `accepting_questions | away | offline`.
- Conversation status: `pending | awaiting_acceptance | ongoing | completed | cancelled | expired | disputed`.
- Chat Hub route: `/api/v1/chat`.
- Realtime events: `message.created`, `message.read`, `typing.started`, `typing.stopped`, `conversation.updated`.
- Group key chuẩn: `conversation:{conversationId}`.
- REST workflow endpoints cho conversations + admin disputes đã map theo plan v2.

## Phase Plan
- [x] Phase 0: Baseline & spec freeze, build xanh với `CHAT_V2_ENABLED=false`.
- [x] Phase 1: Domain/persistence foundation cho chat + escrow + mongo indexes.
- [x] Phase 2: Conversation lifecycle core (`1 active/cặp`, `<=5 active/user`, first-message transition).
- [x] Phase 3: Acceptance & timeout foundations (SLA 6/12/24, worker hourly).
- [x] Phase 4: Realtime messaging/read model foundation (`/chat`, room, SignalR contract).
- [x] Phase 5: Completion/settlement core + idempotency hooks.
- [x] Phase 6: Add-money request/respond flow.
- [x] Phase 7: Dispute/admin resolve + policy `>3/7d`.
- [x] Phase 8: Notifications/moderation/media hardening + rollout strategy.

## Test Gates
- Backend build: `dotnet build Backend/TarotNow.slnx`.
- Backend tests: `dotnet test Backend/TarotNow.slnx --no-build`.
- Frontend build: `npm run build` (trong `Frontend`).
- Frontend unit tests: `npm test` (trong `Frontend`).

## Current Validation Snapshot
- Backend build: ✅
- Backend tests: ✅
- Frontend build: ✅
- Frontend unit tests: ✅

## Phase 8 Completed Items
1. In-app notifications cho escrow `released/refunded` qua domain event handlers.
2. Async keyword moderation queue + background worker (auto-flag tạo reports).
3. Media metadata validation pipeline cho `image/voice` + fallback parsing.
4. Chat feature gate middleware (`ChatV2Enabled`) cho `/api/v1/conversations` và `/api/v1/chat`.
5. Escrow timer hardening: expired offers giờ refund ví thật + publish refunded domain events.

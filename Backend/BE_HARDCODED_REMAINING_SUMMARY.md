# BE HARDCODED REMAINING SUMMARY (2026-04-24)

## 1) Trạng thái hiện tại
- **Đã hoàn thành** toàn bộ kế hoạch xử lý hardcode còn lại (P0/P1/P2) trong backend.
- Tất cả business config cần tuning runtime đã được đưa về:
  - `system_configs` (business policy/economy/chat)
  - `appsettings` + typed options (vận hành/rate-limit/job tuning)

## 2) Nhóm đã xử lý xong

### P0 (Business Runtime Config)
- Chat policy đã chuyển sang `system_configs`:
  - `chat.allowed_sla_hours` (json)
  - `chat.default_sla_hours` (scalar)
  - `chat.max_active_conversations_per_user` (scalar)
- Economy conversion đã chuyển sang `system_configs`:
  - `economy.vnd_per_diamond` (scalar)
- Luồng đã cập nhật đầy đủ:
  - `CreateConversation` validation
  - `AcceptConversation` SLA normalization
  - `ConversationController` default SLA
  - `Withdrawal` conversion plan

### P1 (Operational Tuning)
- Rate limit không còn hardcode trong startup:
  - section `RateLimitPolicies` trong `appsettings`
- Outbox không còn hardcode batch/retry/backoff/poll:
  - section `Outbox` + `OutboxOptions`
- Leaderboard snapshot không còn hardcode schedule/top-N:
  - section `LeaderboardSnapshot` + `LeaderboardSnapshotOptions`
- Platform limits không còn hardcode:
  - `ForwardedHeaders:ForwardLimit`
  - `SignalR:MaximumReceiveMessageSizeBytes`
- AI streaming tuning không còn hardcode:
  - `AiProvider:StreamingRetryBaseDelayMs`
  - `AiProvider:StreamingTemperature`
- Auth cleanup loop cap không còn hardcode:
  - `AuthSecurity:CleanupMaxBatchLoopsPerCycle`

### P2 (Cleanup)
- Đã xóa constants dư:
  - `EconomyConstants` (file removed)
  - `WithdrawalPolicyConstants.MinimumWithdrawDiamond`
  - `WithdrawalPolicyConstants.FeeRate`

## 3) Các giá trị còn giữ trong code (chủ đích)
- Guardrail/schema/security constants vẫn giữ cố định (không đưa admin runtime):
  - `WithdrawalPolicyConstants.IdempotencyKeyMaxLength`
  - `WithdrawalPolicyConstants.NoteMaxLength`
  - Một số clamp tối thiểu/tối đa để chống config lỗi gây mất ổn định hệ thống.

## 4) Kết quả xác minh
- `dotnet build TarotNow.slnx`: **PASS** (0 warnings, 0 errors)
- `dotnet test TarotNow.slnx`: **PASS** toàn bộ test projects.

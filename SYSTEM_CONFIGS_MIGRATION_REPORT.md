# SYSTEM CONFIGS MIGRATION REPORT

## 1) Trạng thái triển khai
- Đã thêm bảng PostgreSQL `system_configs` làm nguồn dữ liệu runtime cho business config thay đổi thường xuyên.
- Đã áp dụng convention hybrid:
  - `scalar`: giá trị đơn (`withdrawal.fee_rate=0.10`)
  - `json`: cấu trúc phức tạp (`deposit.packages=[...]`, `gacha.pools=[...]`)
- Đã thêm API admin + UI admin để xem/sửa trực tiếp.

## 2) Thành phần đã triển khai
- Backend:
  - Entity + EF config + repository cho `system_configs`
  - Migration: `20260423185242_AddSystemConfigsTable`
  - Bootstrap service: tự seed key mặc định, validate, load snapshot, projection
  - Snapshot runtime in-memory + projection service
  - API admin:
    - `GET /api/v1/admin/system-configs`
    - `PUT /api/v1/admin/system-configs/{key}`
- Frontend:
  - Trang admin: `/[locale]/admin/system-configs`
  - Menu admin đã thêm mục `System Configs`
  - Cho phép chỉnh `value`, `valueKind`, `description`

## 3) Các dữ liệu đã chuyển vào DB (`system_configs`)

### Scalar keys
- `pricing.spread_3.gold`
- `pricing.spread_3.diamond`
- `pricing.spread_5.gold`
- `pricing.spread_5.diamond`
- `pricing.spread_10.gold`
- `pricing.spread_10.diamond`
- `ai.daily_quota`
- `ai.in_flight_cap`
- `reading.rate_limit_seconds`
- `checkin.daily_gold`
- `streak.freeze_window_hours`
- `gacha.cost_diamond`
- `withdrawal.min_diamond`
- `withdrawal.fee_rate`
- `followup.max_allowed`
- `followup.free_slots.threshold_low`
- `followup.free_slots.threshold_mid`
- `followup.free_slots.threshold_high`
- `presence.timeout_minutes`
- `presence.scan_interval_seconds`
- `escrow.dispute_window_hours`
- `escrow.reader_response_due_hours`
- `escrow.auto_refund_hours`
- `deposit.link_expiry_minutes`

### JSON keys
- `deposit.packages`
- `followup.price_tiers`
- `gacha.pools`
- `gamification.quests`
- `gamification.achievements`
- `gamification.titles`

## 4) Luồng đã dùng key DB trong nghiệp vụ
- Reading pricing (spread 3/5/10)
- AI quota/rate-limit
- Follow-up pricing & free-slot policy
- Check-in gold + streak freeze window
- Presence timeout/scan interval
- Withdrawal minimum/fee
- Escrow timing (dispute window / reader due / auto refund)
- Deposit package catalog + PayOS link expiry
- Projection gacha/gamification từ JSON config

## 5) Các dữ liệu vẫn giữ ngoài DB (chưa chuyển)

### Giữ ở appsettings/options (vận hành kỹ thuật)
- JWT/session/cache/replay/auth cleanup
- SMTP/AI provider timeout-retry
- Object storage limits/TTL/cleanup batch
- OpenTelemetry/CORS/forwarded headers

### Giữ ở constants (policy ổn định/guardrails)
- `WithdrawalPolicyConstants.IdempotencyKeyMaxLength`
- `WithdrawalPolicyConstants.NoteMaxLength`
- Một số validation/security floor không nên mở cho admin runtime

### Cần chuyển tiếp (đợt tiếp theo đề xuất)
- Các hard-coded còn lại trong module chat/conversation SLA & timeout không nằm trong key set hiện tại
- Các key economy campaign đặc thù cần effective-date/versioning nâng cao

## 6) Kiểm thử
- `dotnet build` backend: PASS
- `npm run build` frontend: PASS
- `dotnet test tests/TarotNow.Api.IntegrationTests --filter AdminRbacIntegrationTests.AdminRoute_ShouldReject_WhenUserIsNotAdmin`: PASS
- `dotnet test tests/TarotNow.ArchitectureTests`: còn fail các rule line-budget/parameter-budget ở một số file legacy lớn (không riêng phần API system-config mới)

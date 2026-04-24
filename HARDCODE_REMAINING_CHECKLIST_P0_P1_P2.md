# CHECKLIST HARDCODE P0/P1/P2 (FE/BE) - TRẠNG THÁI SAU KHI MIGRATE

Ngày cập nhật: 2026-04-25  
Phạm vi: `Backend/src`, `Frontend/src`  
Nguyên tắc: chỉ tính các hard-code business/operational/runtime tuning thuộc checklist P0/P1/P2 đã chốt trước đó.

## Tổng quan
- Tổng mục trong checklist gốc: **25**
- Đã xử lý: **25/25**
- Còn lại trong phạm vi checklist gốc: **0**
- Build xác nhận:
  - `dotnet build Backend/src/TarotNow.Api/TarotNow.Api.csproj`: **PASS**
  - `dotnet test Backend/tests/TarotNow.Application.UnitTests/TarotNow.Application.UnitTests.csproj`: **PASS**
  - `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`: **PASS**
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj --no-build`: **PASS**
  - `npm -C Frontend test`: **PASS**
  - `npm -C Frontend run build`: **PASS**

---

## P0 - Hoàn tất (8/8)
- `P0-01`: Open dispute reason/dispute window bỏ literal cứng, đọc từ `escrow.dispute.min_reason_length` và `escrow.dispute_window_hours`.
- `P0-02`: Default split admin dispute bỏ `50` cứng, dùng `admin.dispute.default_split_percent_to_reader` cho cả BE/FE runtime policy.
- `P0-03`: Freeze reader policy bỏ `7 ngày/3 lần` cứng, dùng `admin.dispute.reader_freeze.lookback_days` và `admin.dispute.reader_freeze.threshold`.
- `P0-04`: Bỏ fallback `: 24` phân tán trong flow reject/reply/question, dùng policy escrow typed từ `system_configs`.
- `P0-05`: Reading EXP + non-daily multiplier đọc từ `progression.reading.exp_per_card`, `progression.reading.diamond_multiplier_non_daily`.
- `P0-06`: Lucky star reward bỏ constant business cứng, đọc từ `inventory.lucky_star.owned_title_gold_reward`.
- `P0-07`: Tắt đăng ký startup `GachaSeedService`; nguồn active gacha là projection từ `system_configs`.
- `P0-08`: Rule tuổi đăng ký thống nhất FE/BE theo runtime config `legal.minimum_age`.

## P1 - Hoàn tất (9/9)
- `P1-01`: Payment offer defaults FE đọc runtime (`chat.payment_offer.default_amount`, `chat.payment_offer.max_note_length`).
- `P1-02`: Reader apply/profile policy dùng runtime policy thay vì fallback literal phân tán.
- `P1-03`: Realtime reconnect/cooldown/negotiation/server timeout chuyển sang key `realtime.*`.
- `P1-04`: HTTP client/server timeout/min timeout lấy từ `operational.http.*`.
- `P1-05`: Runtime policy fetch timeout/stale dùng `operational.runtime_policies.*`.
- `P1-06`: Redis startup bootstrap có đọc override từ DB (`operational.redis.*`) với fallback an toàn.
- `P1-07`: OpenAI runtime tuning dùng `operational.ai.*`.
- `P1-08`: Outbox runtime tuning dùng `operational.outbox.*`.
- `P1-09`: AddQuestion flow bỏ fallback `24` cứng, dùng typed settings escrow.

## P2 - Hoàn tất (8/8)
- `P2-01`: Reader directory/prefetch constants chuyển sang runtime policy (`ui.pagination.*`, `ui.prefetch.*`).
- `P2-02`: Chat page size lấy từ `chat.history.page_size`.
- `P2-03`: Realtime chat typing/debounce/guard timings chuyển sang `realtime.chat.*`.
- `P2-04`: Participant conversation query default/max page size đọc từ `chat.participants.*`.
- `P2-05`: SystemConfig options giữ vai trò bootstrap fallback tập trung, không còn fallback business phân tán ở handlers.
- `P2-06`: Chuẩn hóa fallback qua settings helper/runtime policy store thay vì literal rải rác.
- `P2-07`: Defaults gacha/gamification giữ vai trò bootstrap/registry để khởi tạo key DB, không còn là source active trong flow business.
- `P2-08`: Media upload limits/compression/retry FE dùng runtime policy `media.upload.*` có clamp guardrail.

---

## Ghi chú vận hành
- Vẫn còn một số constant kỹ thuật/guardrail có chủ đích (ví dụ max length bảo mật, giới hạn clamp an toàn, schema-bound limits). Các giá trị này không nằm trong checklist migrate business tuning.
- Nguồn active config đã ưu tiên `system_configs`; fallback được gom về tầng bootstrap/helper để giảm drift và nợ kỹ thuật.

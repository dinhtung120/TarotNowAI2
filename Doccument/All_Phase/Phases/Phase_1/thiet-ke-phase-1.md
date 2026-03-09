# Phase 1 – Tài liệu thiết kế liên quan

**Mục đích:** Trích các phần thiết kế cần đọc khi làm Phase 1 (Trải bài Tarot + AI Streaming MVP).  
**Nguồn gốc:** Trích từ `01-business-rules.md`, `02-product-ux-specs.md`, `03-tech-architecture.md`, `04-ops-security-compliance.md`

---

## 1. Quy tắc kinh doanh Phase 1 (từ BR)

### 1.1 Auth Baseline (BR Phase 1.1)
- Đăng ký/đăng nhập email + mật khẩu
- Xác minh email (verify email) + OTP
- JWT + refresh token rotation
- Thưởng đăng ký: **+5 Gold** chỉ cấp sau verify email thành công (BR-1)

### 1.2 Wallet + Ledger Minimum (BR Phase 1.2)
- **Gold**: tiền tệ miễn phí (free currency)
- **Diamond**: tiền tệ trả phí, tỷ giá **1 Diamond = 1,000 VND**
- Mọi biến động số dư phải có **dòng sổ cái** (ledger row)
  - **Ngoại lệ:** `frozen_diamond_balance` khi escrow release – diamond_balance payer không đổi, audit qua view `v_user_frozen_ledger_balance`
- Sổ cái theo mô hình **double-entry** (debit/credit pair)
- Diamond lưu **BIGINT** (số nguyên)
- **Không âm số dư** (CHECK constraint)
- **Mọi credit/debit/freeze/release/refund phải đi qua `proc_wallet_*`** (KHÔNG UPDATE trực tiếp)

### 1.3 Daily 1 Card (BR Phase 1.3)
- Rút 1 lá/ngày (daily spread)
- RNG công bằng: CSPRNG, Fisher-Yates, audit package
- Không trùng lá trong phiên
- Giới hạn: 1 lần/ngày theo UTC business date

### 1.4 Spread 3/5/10 Cards (BR Phase 1.4)
- Loại trải: 3 lá, 5 lá, 10 lá
- Câu hỏi tùy chọn (optional question)
- Định giá theo spread (Gold/Diamond)

### 1.5 AI Streaming (BR Phase 1.5)
- SSE token streaming cho diễn giải
- Tích hợp Grok/ChatGPT API
- Timeout/retry + refund tự động khi lỗi provider
- **Daily quota theo tier** (configurable):
  - Free: mặc định **3 AI requests/ngày**
  - Premium: mặc định **30 AI requests/ngày**
- Đơn vị quota: `1 AI request` = mỗi lần gọi model (initial + follow-up)
- Free draw vẫn tiêu tốn AI quota

### 1.6 Follow-up + History (BR Phase 1.6)
- **Free slots** theo cấp lá cao nhất trong session:
  - Level >= 6: miễn phí 1 lượt
  - Level >= 11: miễn phí 2 lượt
  - Level >= 16: miễn phí 3 lượt
- **Paid tiers base**: [1, 2, 4, 8, 16] Diamond
- Free slots dùng trước → paid bắt đầu ở tier tiếp theo
- **Hard cap: tối đa 5 follow-up/session**
- Lịch sử trải bài (reading history) cơ bản

### 1.7 Legal + Profile + Deposit (BR Phase 1.7)
- Cổng tuổi 18+ (hoặc theo luật địa phương nếu cao hơn)
- TOS, Privacy Policy, AI disclaimer bắt buộc
- Hồ sơ cơ bản: display name, avatar, DOB
- Auto-calc zodiac + numerology từ DOB
- Nạp tiền tối thiểu 1 kênh (VietQR/Bank)

---

## 2. Đặc tả UX Phase 1 (từ UX)

### 2.1 Auth UX (UX 4.1.1, 4.1.3)
- Register: username + email + password + consent checkboxes + DOB picker
- Login: email hoặc username + password
- Verify OTP: UI verify + resend + loading/error/success states
- Forgot/reset password flow
- Error mapping: ProblemDetails → thông điệp thân thiện

### 2.2 Hồ sơ người dùng (UX 4.2.1)
- Display name, DOB, avatar, active title
- Auto-calc zodiac + numerology từ DOB
- Xem level, EXP, tổng trải bài, thống kê bộ sưu tập

### 2.3 Wallet UX (UX 2.6)
- Widget số dư Gold/Diamond
- Lịch sử giao dịch có paging
- Empty state, error state

### 2.4 Reading UX (UX 4.4.1)
- Spread selector (1/3/5/10)
- Question form (optional)
- Card reveal animations (lật bài mượt)
- Cost display + insufficient balance UX
- Daily 1 card: nếu có AI vẫn tiêu AI quota → UI hiển thị copy rõ

### 2.5 AI Streaming UX (UX 4.4.3, 4.4.4, 4.4.5, 4.4.6)
- SSE streaming UI states: loading → streaming → completed → error
- AI safety: pre/post moderation (allow/soft_block/hard_block)
- Content bị hard_block → không trả raw output
- Follow-up UI: list + composer + cost badge
- Copy "Miễn phí Diamond, vẫn tiêu tốn AI quota ngày" khi free-slot
- Cap reached UX + CTA upsell/quota exhausted
- Localization: prompt theo locale (vi/en/zh-Hans)
- Ghi log: `requested_locale`, `returned_locale`, `fallback_reason`

### 2.6 Card Progression (UX 4.5.1)
- Mỗi lá bài có level (1-20+), EXP tích lũy
- EXP theo loại tiền dùng (Gold ít hơn Diamond)
- Visual FX theo level tier (L1-5: basic, L6-10: glow, L11-15: particle, L16-20: premium)
- Performance profiles: low/medium/high

### 2.7 Error Handling (UX 4.15.3)
- Mọi error API → ProblemDetails format
- FE map error code → UX copy thân thiện
- Insufficient balance → hiển thị cost + CTA nạp

---

## 3. Kiến trúc kỹ thuật Phase 1 (từ ARCH)

### 3.1 RNG Pipeline (ARCH 4.4.2)
- `session_nonce` = CSPRNG random bytes
- `seed_digest` = HMAC-SHA256(server_secret_versioned, nonce + user_id + timestamp)
- Fisher-Yates shuffle deterministic theo seed
- Audit package: `algorithm_version`, `secret_version`, `session_nonce`, `seed_digest`, `deck_order_hash`
- **Không lưu `server_secret` trong DB/log**
- Secret rotation: secret cũ giữ >= 24 tháng trong secure vault

### 3.2 AI Streaming State Machine (ARCH 4.4.3) – LOCKED
```
requested → first_token_received → completed
                                 → failed_after_first_token
         → failed_before_first_token (timeout 30s)
```

**Guard order bắt buộc:**
1. Follow-up cap check (max 5/session)
2. Daily quota reserve (atomic)
3. Rate-limit check
4. Free-slot / Diamond balance check
5. **Moderation pre-check**
6. Chỉ pass 1-5 mới gọi AI provider

**Retry & Refund matrix:**
| Terminal state | Retry budget | Quota | Diamond |
|---|---|---|---|
| `completed` | n/a | giữ | không refund |
| `failed_before_first_token` | hết (max=1) | rollback 1 | refund full |
| `failed_after_first_token` | hết (max=1) | rollback 1 | refund full |

- In-flight cap: max **2 AI requests** đồng thời per user
- Timeout: **30s** chờ first token
- Refund idempotent theo `ai_request_id`
- Client disconnect → backend vẫn track completion, không auto-refund

### 3.3 Deposit Flow (ARCH 4.3.1, 4.3.2)
- Webhook signature verification bắt buộc
- Idempotent payment callback (chống double-credit)
- FX rate snapshot nếu non-VND
- Credit Diamond qua `proc_wallet_credit`
- Promotions: auto-apply theo deposit amount + event window

### 3.4 Entitlement Consume Atomicity (ARCH 4.3.4)
- Source of truth = DB write path (không phải cache)
- Earliest-expiry-first + deterministic tie-break (subscription_id ASC)
- Atomic consume: transaction + row lock
- Cache key: `user_id + business_date`

---

## 4. Ops & Security Phase 1 (từ OPS)

### 4.1 Legal Compliance (OPS 4.13.1)
- Age gate 18+ bắt buộc
- TOS/Privacy/AI disclaimer pages
- Consent events lưu với version
- Re-consent khi legal document version thay đổi

### 4.2 Performance Targets (OPS 5)
- Auth/profile/read endpoints: **P95 <= 300ms**
- Finance write (deposit/debit): **P95 <= 500ms**
- First AI token latency: target thấp

### 4.3 Tax Baseline (OPS 4.13.6)
- Lưu tax components tách biệt trong payment record
- Tách FX rate snapshot cho non-VND transactions

### 4.4 Data Retention
- AI provider logs: **90 ngày** TTL
- Reading RNG audits: **>= 24 tháng**
- Notifications: **30 ngày** TTL

---

## 5. Database Schema liên quan Phase 1

### PostgreSQL - Bảng cần quan tâm
- `users` – tài khoản + balance + streak
- `email_otps` – OTP verify email
- `password_reset_tokens` – reset password
- `refresh_tokens` – JWT refresh rotation
- `user_consents` – consent tracking
- `wallet_transactions` – sổ cái Gold/Diamond
- `deposit_orders` – đơn nạp Diamond
- `deposit_promotions` – khuyến mãi nạp
- `ai_requests` – state machine AI
- `reading_rng_audits` – audit RNG
- `user_exp_levels` + `card_exp_levels` – EXP lookup
- `system_configs` – runtime configs
- `admin_actions` – audit admin

### MongoDB - Collections cần quan tâm
- `cards_catalog` – 78 lá bài
- `user_collections` – bộ sưu tập user
- `reading_sessions` – phiên xem bài + followups
- `ai_provider_logs` – log AI (TTL 90d)
- `notifications` – thông báo (TTL 30d)

### Stored Procedures (bắt buộc sử dụng)
- `proc_wallet_credit` – cộng Gold/Diamond
- `proc_wallet_debit` – trừ Gold/Diamond
- Views: `v_user_ledger_balance` (reconciliation)

---

## Tham chiếu đầy đủ

| Tài liệu | Sections liên quan |
|---|---|
| [01-business-rules.md](../All_Phase/tai-lieu-thiet-ke/01-business-rules.md) | Phase 1 (1.1–1.7), BR decisions 1-3, 11, 14-18, 20-21 |
| [02-product-ux-specs.md](../All_Phase/tai-lieu-thiet-ke/02-product-ux-specs.md) | 4.1.1, 4.1.3, 4.2.1, 4.4.1–4.4.6, 4.5.1, 4.15.3 |
| [03-tech-architecture.md](../All_Phase/tai-lieu-thiet-ke/03-tech-architecture.md) | 4.1.5, 4.4.2, 4.4.3, 4.3.1–4.3.4, 4.14.4 |
| [04-ops-security-compliance.md](../All_Phase/tai-lieu-thiet-ke/04-ops-security-compliance.md) | 4.13.1, 4.13.6, 5 (Performance, Security) |
| [database/DESIGN_DECISIONS.md](../../database/DESIGN_DECISIONS.md) | Double-entry, ON DELETE, Idempotency, proc_wallet_* |

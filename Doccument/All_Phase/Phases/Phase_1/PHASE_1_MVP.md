# Phase 1 – Trải bài Tarot + AI Streaming (MVP)

**Nguồn:** `CODING_PLAN.md` Sections 4.1–4.7  
**Mục tiêu:** User có thể đăng ký → verify email → nhận +5 Gold → rút bài (daily/spread) → AI stream → follow-up (cap 5) → xem lịch sử → nạp Diamond 1 kênh → admin xem cơ bản.

---

## Quy ước

- **PD** = person-days (1 người × 1 ngày ≈ 8h focus)
- Spec mapping: `BR` = 01-business-rules, `UX` = 02-product-ux-specs, `ARCH` = 03-tech-architecture, `OPS` = 04-ops-security-compliance, `DB` = database/

---

## 1.1 Auth Baseline (BR Phase 1.1)

### BE – Auth Backend

- [ ] **P1-AUTH-BE-1.1** (1.0 PD) – Endpoint register: password policy + unique email/username
  - Kiểm tra policy mật khẩu, validate input, reject trùng email/username (error code rõ)
  - Spec: UX(4.1.1)

- [ ] **P1-AUTH-BE-1.2** (0.75 PD) – Password hashing Argon2id + verify
  - Tích hợp Argon2id, đảm bảo verify đúng và không log dữ liệu nhạy cảm
  - Spec: UX(4.1.1)

- [ ] **P1-AUTH-BE-1.3** (1.25 PD) – Issue JWT access + refresh rotation (happy path)
  - Trả access token short-lived, refresh token rotation; web dùng cookie transport
  - Spec: BR(Phase-1.1), ARCH(4.1.5)

- [ ] **P1-AUTH-BE-1.4** (1.5 PD) – Refresh rotation + reuse detection → revoke chain
  - Nếu phát hiện reuse refresh token thì revoke toàn chuỗi và buộc login lại
  - Spec: UX(4.1.1)

- [ ] **P1-AUTH-BE-1.5** (1.0 PD) – Revoke session + revoke-all
  - Cho user thu hồi theo thiết bị hoặc thu hồi tất cả phiên đang active
  - Spec: UX(4.1.1)

### BE – OTP & Email Verify

- [ ] **P1-AUTH-BE-2.1** (1.0 PD) – Create OTP + store `email_otps` + email send hook
  - Sinh OTP 6 số, lưu TTL 30 phút, tích hợp provider email (stub OK)
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-BE-2.2** (1.0 PD) – Verify OTP (expiry + one-time)
  - Validate expiry + one-time use; OTP dùng lại/expired phải bị chặn
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-BE-2.3** (0.75 PD) – Verify success → credit +5 Gold via proc
  - Khi verify email xong, cộng +5 Gold qua `proc_wallet_credit`, idempotent
  - Spec: BR(3.1), DB(DESIGN_DECISIONS)

### BE – Password Reset

- [ ] **P1-AUTH-BE-3.1** (0.75 PD) – Forgot password: generate reset token
  - Tạo token reset 30 phút, lưu 1 lần dùng, gửi link email (stub OK)
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-BE-3.2** (1.0 PD) – Reset password: validate token + invalidate sessions
  - Token hợp lệ mới cho reset; reset xong revoke sessions để bảo mật
  - Spec: UX(4.1.3)

### BE – Consent & Age Gate

- [ ] **P1-AUTH-BE-4.1** (0.75 PD) – Consent enforcement gate
  - Không cho hoàn tất đăng ký nếu thiếu TOS/Privacy/AI disclaimer consent required
  - Spec: OPS(4.13.1)

- [ ] **P1-AUTH-BE-4.2** (0.5 PD) – Age gate 18+
  - Validate DOB đủ 18+, lưu DOB; fail phải trả error code + UX copy
  - Spec: OPS(4.13.1), UX(4.2.1)

### FE – Auth Frontend

- [ ] **P1-AUTH-FE-1.1** (1.5 PD) – Register UI: consent + DOB
  - Form đăng ký có checkbox consent, DOB picker, validation và error states
  - Spec: OPS(4.13.1), UX(4.1.1)

- [ ] **P1-AUTH-FE-1.2** (1.0 PD) – Verify OTP UI
  - UI verify OTP + resend + trạng thái loading/error/success rõ
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-FE-1.3** (1.0 PD) – Login UI + error mapping
  - Map ProblemDetails/error code thành thông điệp thân thiện
  - Spec: UX(4.15.3)

- [ ] **P1-AUTH-FE-1.4** (1.0 PD) – Forgot/reset password UI
  - UI luồng forgot + reset (token expired, token used, success)
  - Spec: UX(4.1.3)

### QA – Auth Tests

- [ ] **P1-AUTH-QA-1.1** (1.0 PD) – E2E: register → verify → +5 Gold
  - Kiểm chứng end-to-end: verify email xong ví Gold tăng đúng +5
  - Spec: BR(3.1)

- [ ] **P1-AUTH-QA-1.2** (1.0 PD) – E2E: refresh reuse → revoke chain
  - Test reuse refresh token: hệ thống thu hồi chain và buộc đăng nhập lại
  - Spec: UX(4.1.1)

---

## 1.2 Wallet + Ledger Minimum (BR Phase 1.2)

### DB – Wallet Verify

- [ ] **P1-WALLET-DB-1.1** (0.5 PD) – Verify CHECK constraints: non-negative balances, ledger invariant
  - Rà soát constraint để đảm bảo ledger và users balance không âm
  - Spec: DB(schema.sql)

- [ ] **P1-WALLET-DB-1.2** (0.5 PD) – Verify indexes for ledger paging + reference lookup
  - Xác nhận index phục vụ phân trang ledger và truy vết theo reference_id
  - Spec: DB(schema.sql)

### BE – Wallet API

- [ ] **P1-WALLET-BE-1.1** (0.5 PD) – API GET `/wallet/balance`
  - Trả đúng gold/diamond/frozen; format response ổn định
  - Spec: BR(Phase-1.2)

- [ ] **P1-WALLET-BE-1.2** (1.0 PD) – API GET `/wallet/ledger` paging + filters
  - Phân trang + filter; thứ tự theo created_at desc
  - Spec: BR(Phase-1.2)

- [ ] **P1-WALLET-BE-1.3** (0.75 PD) – Guard: cấm update balance trực tiếp
  - Bắt buộc mọi credit/debit đi qua `proc_wallet_*`
  - Spec: DB(DESIGN_DECISIONS)

### FE – Wallet Frontend

- [ ] **P1-WALLET-FE-1.1** (0.5 PD) – Wallet balances widget
  - UI hiển thị số dư và trạng thái loading/error
  - Spec: UX(2.6)

- [ ] **P1-WALLET-FE-1.2** (1.0 PD) – Ledger list UI (paging)
  - UI lịch sử giao dịch: paging, empty state, error state
  - Spec: UX(2.6)

### QA & Ops – Wallet

- [ ] **P1-WALLET-QA-1.1** (0.75 PD) – API tests: ledger invariant holds
  - Test ledger invariants cho các giao dịch cốt lõi
  - Spec: DB(DESIGN_DECISIONS)

- [ ] **P1-RECON-BE-1.1** (0.75 PD) – Reconciliation query `v_user_ledger_balance` vs `users`
  - Viết query/command đối soát và phát signal alert khi mismatch
  - Spec: DB(DESIGN_DECISIONS)

- [ ] **P1-RECON-OPS-1.1** (0.75 PD) – Schedule reconciliation + alert channel
  - Chạy theo lịch, đẩy metric/log cảnh báo đúng kênh
  - Spec: OPS(5.Observability)

- [ ] **P1-RECON-QA-1.1** (0.5 PD) – Mismatch simulation test
  - Chủ động tạo mismatch trong test env để chắc chắn cảnh báo hoạt động
  - Spec: OPS(5.Observability)

---

## 1.3–1.4 Reading Core – Daily 1 + Spread 3/5/10 (BR Phase 1.3–1.4)

### BE – RNG

- [ ] **P1-RNG-BE-1.1** (0.5 PD) – session_nonce (CSPRNG)
  - Sinh nonce bằng CSPRNG để đảm bảo công bằng
  - Spec: ARCH(4.4.2)

- [ ] **P1-RNG-BE-1.2** (0.75 PD) – seed_digest = HMAC(...)
  - Tạo seed_digest theo công thức HMAC versioned secret
  - Spec: ARCH(4.4.2)

- [ ] **P1-RNG-BE-1.3** (1.0 PD) – Fisher-Yates deterministic shuffle
  - Shuffle deterministic theo seed, đảm bảo không trùng lá, replay được
  - Spec: ARCH(4.4.2), BR(11)

- [ ] **P1-RNG-BE-1.4** (1.0 PD) – Persist RNG audit package
  - Lưu audit package đầy đủ + hash thứ tự bài cho tranh chấp
  - Spec: ARCH(4.4.2)

- [ ] **P1-RNG-DB-1.1** (0.5 PD) – RNG audit retention (>=24m)
  - Xác nhận chính sách giữ bằng chứng RNG >= 24 tháng, không TTL
  - Spec: OPS(4.13.4)

### BE – Reading Session

- [ ] **P1-READ-BE-1.1** (0.75 PD) – Create `reading_sessions` base
  - Tạo document reading session chuẩn, có spread_type, question, timestamps
  - Spec: DB(mongodb-schema)

- [ ] **P1-READ-BE-1.2** (0.75 PD) – Store drawn_cards (incl. level)
  - Lưu drawn_cards gồm card_id/position/is_reversed/level cho free follow-up slots
  - Spec: DB(mongodb-schema), UX(4.4.1), UX(4.4.5)

- [ ] **P1-READ-BE-1.3** (0.75 PD) – Enforce daily_1 limit (UTC)
  - Chặn rút daily_1 nếu đã rút trong business date UTC
  - Spec: BR(Phase-1.3)

- [ ] **P1-READ-BE-2.1** (0.75 PD) – Pricing config per spread
  - Định nghĩa bảng giá theo spread type và currency
  - Spec: BR(Phase-1.4)

- [ ] **P1-READ-BE-2.2** (0.75 PD) – Charge via `proc_wallet_debit`
  - Trừ ví bằng stored proc, reference_id trỏ về reading_session
  - Spec: DB(DESIGN_DECISIONS)

### BE – Card Collection

- [ ] **P1-CARD-BE-1.1** (1.0 PD) – Upsert `user_collections` on draw
  - Khi rút bài, tạo/cập nhật entry trong `user_collections`: tăng stats, cập nhật last_drawn_at
  - Spec: DB(mongodb-schema), UX(4.5.1)

- [ ] **P1-CARD-BE-1.2** (0.75 PD) – Credit EXP per card based on currency type
  - Tính EXP cho lá bài theo loại tiền dùng; cập nhật exp + tính lại level
  - Spec: UX(4.5.1), DB(mongodb-schema)

### FE – Reading Frontend

- [ ] **P1-READ-FE-1.1** (1.0 PD) – Spread selector + question form
  - UI chọn spread và nhập câu hỏi (optional), validate UX states
  - Spec: UX(4.4.1)

- [ ] **P1-READ-FE-1.2** (1.5 PD) – Card reveal animations
  - Hiệu ứng reveal/lật bài mượt; đảm bảo không hiển thị duplicate
  - Spec: UX(4.4.1)

- [ ] **P1-READ-FE-1.3** (1.0 PD) – Cost + insufficient balance UX
  - Hiển thị cost, và thông điệp khi thiếu gold/diamond
  - Spec: UX(4.15.3)

### QA – Reading Tests

- [ ] **P1-READ-QA-1.1** (1.0 PD) – RNG replay property test
  - Test property: cùng audit input → deck order giống nhau 100%
  - Spec: ARCH(4.4.2)

- [ ] **P1-READ-QA-1.2** (1.0 PD) – daily_1 limit test + error mapping
  - Test blocked đúng sau 1/day, FE/BE trả error code đúng
  - Spec: BR(Phase-1.3), UX(4.15.3)

- [ ] **P1-CARD-QA-1.1** (1.0 PD) – Card collection update on draw
  - Test rút bài tạo/update đúng `user_collections`, EXP tăng, level đúng
  - Spec: DB(mongodb-schema)

---

## 1.5 AI Streaming + Refund (BR Phase 1.5)

### BE – AI Provider & SSE

- [ ] **P1-AI-BE-1.1** (1.0 PD) – Provider adapter interface + basic implementation (Grok/ChatGPT)
  - Tạo interface adapter + implement tối thiểu để gọi provider, trả token stream
  - Spec: ARCH(1.1)

- [ ] **P1-AI-BE-1.2** (1.25 PD) – SSE endpoint wiring (EventSource-compatible) + heartbeat
  - Endpoint SSE stream token; có heartbeat/keep-alive giảm disconnect
  - Spec: ARCH(4.4.3)

- [ ] **P1-AI-BE-1.3** (1.25 PD) – Prompt templates + `prompt_version` hook
  - Tách system/developer/user prompt, ghi `prompt_version` để audit
  - Spec: ARCH(4.18), ARCH(4.4.7)

### BE – AI State Machine

- [ ] **P1-AI-BE-2.1** (1.0 PD) – Create `ai_requests` row (`requested`) + idempotency
  - Persist request state để refund/idempotent
  - Spec: DB(schema.sql), ARCH(4.4.3)

- [ ] **P1-AI-BE-2.2** (0.75 PD) – Persist `first_token_received`
  - Ghi mốc nhận token đầu để phân loại fail_before vs fail_after
  - Spec: ARCH(4.4.3)

- [ ] **P1-AI-BE-2.3** (1.5 PD) – Terminal transitions (completed/failed_*)
  - Áp rules terminal theo BR-18 (timeout/refund) và persist reason
  - Spec: BR(18), ARCH(4.4.3)

- [ ] **P1-AI-BE-2.4** (1.5 PD) – Refund & quota rollback idempotent
  - Refund Diamond + rollback quota đúng 1 lần theo `ai_request_id`
  - Spec: BR(18)

### BE – AI Guards

- [ ] **P1-AI-BE-3.1** (0.5 PD) – Guard step 1: follow-up cap check
  - Thực thi cap follow-up/session trước khi gọi AI
  - Spec: BR(3.2)

- [ ] **P1-AI-BE-3.2** (1.5 PD) – Guard step 2: daily quota reserve atomic
  - Reserve quota nguyên tử; fail các bước sau phải release reserve
  - Spec: ARCH(4.4.3)

- [ ] **P1-AI-BE-3.3** (0.75 PD) – Guard step 3: rate limit enforcement
  - Áp policy rate limit AI endpoints; trả error code rõ
  - Spec: ARCH(Rate-limit-matrix)

- [ ] **P1-AI-BE-3.4** (0.75 PD) – Guard step 4: charge Diamond after pass 1-3
  - Chỉ trừ Diamond sau khi pass cap/quota/rate-limit
  - Spec: BR(17)

### BE – AI Safety & Locale

- [ ] **P1-AI-BE-4.1** (1.5 PD) – Moderation pre/post (allow/soft/hard) scaffolding
  - Safety layer: pre-check chặn hard_block → không gọi model/không charge
  - Spec: UX(4.4.4)

- [ ] **P1-AI-BE-4.2** (1.5 PD) – De-identification/redaction for prompt logs
  - Redact PII trước khi lưu, tuân retention window
  - Spec: UX(4.4.4), OPS(4.13.4)

- [ ] **P1-AI-BE-5.1** (1.0 PD) – Localization control fields
  - Ghi `requested_locale/returned_locale/fallback_reason`
  - Spec: UX(4.4.6)

### DevOps – AI Ops

- [ ] **P1-AI-OPS-1.1** (0.75 PD) – Provider API keys secure + rotation note
  - Spec: OPS(5.Security)

- [ ] **P1-AI-OPS-1.2** (0.75 PD) – Dashboards: timeout/refund/latency
  - Spec: OPS(5.Observability)

### FE – AI Frontend

- [ ] **P1-AI-FE-1.1** (1.25 PD) – SSE client: start/stop/reconnect
  - Spec: ARCH(4.4.3)

- [ ] **P1-AI-FE-1.2** (1.25 PD) – Streaming UI states + copy mapping
  - UI streaming: loading/streaming/completed/error; map error code→copy
  - Spec: UX(4.15.3)

### QA – AI Tests

- [ ] **P1-AI-QA-1.1** (1.5 PD) – Integration: fail_before → refund once + rollback
  - Mô phỏng timeout trước token đầu
  - Spec: BR(18)

- [ ] **P1-AI-QA-1.2** (1.5 PD) – Integration: fail_after → refund once + rollback
  - Mô phỏng fail sau token đầu
  - Spec: BR(18)

---

## 1.6 Follow-up + History (BR Phase 1.6)

### BE – Follow-up Logic

- [ ] **P1-FUP-BE-1.1** (0.75 PD) – Compute highest card level in session
  - Tính level cao nhất trong session để xác định free slots
  - Spec: UX(4.4.5)

- [ ] **P1-FUP-BE-1.2** (0.5 PD) – Compute free slots by highest card level
  - `>=6→1 free, >=11→2 free, >=16→3 free`
  - Spec: UX(4.4.5)

- [ ] **P1-FUP-BE-1.3** (0.75 PD) – Paid tier progression
  - Free slots dùng trước, paid tiếp từ vị trí tương ứng trong chuỗi [1,2,4,8,16]
  - Spec: BR(3-item-2), UX(4.4.5)

- [ ] **P1-FUP-BE-1.4** (0.75 PD) – Enforce hard cap 5 follow-ups per session
  - Spec: UX(4.4.5)

- [ ] **P1-FUP-BE-2.1** (0.75 PD) – Persist followup record into `reading_sessions.followups`
  - Spec: DB(mongodb-schema)

### BE – History API

- [ ] **P1-HIST-BE-1.1** (0.75 PD) – API list reading_sessions by user_id (paging)
  - Spec: UX(2.6)

- [ ] **P1-HIST-BE-1.2** (0.5 PD) – API get reading_session detail
  - Spec: UX(2.6)

### FE – Follow-up & History Frontend

- [ ] **P1-FUP-FE-1.1** (1.0 PD) – Follow-up list UI + composer + cost badge
  - Spec: UX(4.4.5)

- [ ] **P1-FUP-FE-1.2** (0.5 PD) – Copy "Miễn phí Diamond, vẫn tiêu tốn AI quota ngày" when free-slot
  - Spec: UX(4.4.5)

- [ ] **P1-FUP-FE-1.3** (0.75 PD) – Cap reached UX + CTA upsell/quota exhausted
  - Spec: UX(4.15.3)

- [ ] **P1-HIST-FE-1.1** (0.75 PD) – History list screen
  - Spec: UX(2.6)

- [ ] **P1-HIST-FE-1.2** (0.75 PD) – History detail screen
  - Spec: UX(2.6)

### QA – Follow-up Tests

- [ ] **P1-FUP-QA-1.1** (1.0 PD) – Unit tests: free-slot + paid tiers precedence
  - Spec: BR(3.2)

- [ ] **P1-FUP-QA-1.2** (1.0 PD) – Integration: cap 5 enforced + correct error mapping
  - Spec: UX(4.4.5)

---

## 1.7 Legal + Profile + Deposit 1 kênh (BR Phase 1.7)

### FE – Legal Pages

- [ ] **P1-LEGAL-FE-1.1** (1.0 PD) – Pages: TOS/Privacy/AI disclaimer (public routes)
  - Spec: OPS(4.13.1)

- [ ] **P1-LEGAL-FE-1.2** (1.0 PD) – SEO basics for public pages
  - Spec: ARCH(1.1)

### BE – Consent

- [ ] **P1-LEGAL-BE-1.1** (0.75 PD) – Persist consent events to `user_consents` with version
  - Spec: OPS(4.13.1)

- [ ] **P1-LEGAL-BE-1.2** (0.75 PD) – Enforce consent present for account completion
  - Spec: OPS(4.13.1)

- [ ] **P1-LEGAL-BE-1.3** (0.75 PD) – Enforce re-consent when legal document version changes
  - Spec: OPS(4.13.1)

### BE – Profile

- [ ] **P1-PROFILE-BE-1.1** (0.75 PD) – Update display_name + avatar_url endpoints
  - Spec: UX(4.2.1)

- [ ] **P1-PROFILE-BE-1.2** (0.75 PD) – Update DOB + recalc zodiac/numerology
  - Spec: UX(4.2.1)

### FE – Profile

- [ ] **P1-PROFILE-FE-1.1** (0.75 PD) – Profile edit form (display name + avatar)
  - Spec: UX(4.2.1)

- [ ] **P1-PROFILE-FE-1.2** (0.75 PD) – DOB edit + show zodiac/numerology
  - Spec: UX(4.2.1)

### BE – Deposit

- [ ] **P1-DEP-BE-1.1** (1.0 PD) – Create deposit order (pending)
  - Spec: BR(Phase-1.7), BR(4.3.2)

- [ ] **P1-DEP-BE-1.2** (1.5 PD) – Webhook signature verification (provider)
  - Spec: BR(4.3.2), OPS(5.Security)

- [ ] **P1-DEP-BE-1.3** (1.25 PD) – Webhook idempotency: prevent double-credit
  - Spec: BR(4.3.2)

- [ ] **P1-DEP-BE-1.4** (1.0 PD) – On success: credit Diamond via `proc_wallet_credit`
  - Spec: DB(DESIGN_DECISIONS)

- [ ] **P1-DEP-BE-1.5** (0.75 PD) – FX snapshot storage path (if non-VND)
  - Spec: BR(21)

### BE – Promotions

- [ ] **P1-PROMO-BE-1.1** (1.0 PD) – Admin CRUD `deposit_promotions`
  - Spec: BR(4.3.2), DB(schema.sql)

- [ ] **P1-PROMO-BE-1.2** (1.0 PD) – Auto-apply promotion on deposit
  - Spec: BR(4.3.2)

### FE – Promotions

- [ ] **P1-PROMO-FE-1.1** (1.0 PD) – Admin UI deposit promotions
  - Spec: BR(4.3.2), OPS(4.10)

### DevOps – Deposit Ops

- [ ] **P1-DEP-OPS-1.1** (0.75 PD) – WAF/allowlist baseline for webhook
  - Spec: OPS(5.Security)

- [ ] **P1-DEP-OPS-1.2** (0.75 PD) – Retry/DLQ policy doc + alert hooks
  - Spec: OPS(5.Reliability)

### QA – Deposit Tests

- [ ] **P1-DEP-QA-1.1** (1.25 PD) – Webhook double-send → only one credit
  - Spec: BR(4.3.2)

- [ ] **P1-DEP-QA-1.2** (1.0 PD) – Signature invalid → reject + no side effects
  - Spec: BR(4.3.2)

- [ ] **P1-DEP-QA-1.3** (0.75 PD) – Reconciliation mismatch path captured
  - Spec: BR(4.3.2)

---

## Minimal Admin (Phase 1 scope)

### BE – Admin

- [ ] **P1-ADMIN-BE-1.1** (0.75 PD) – RBAC policy admin-only + route protection
  - Spec: OPS(4.10)

- [ ] **P1-ADMIN-BE-1.2** (1.0 PD) – Admin: list users + lock/unlock endpoint
  - Spec: OPS(4.10)

- [ ] **P1-ADMIN-BE-1.3** (0.75 PD) – Admin: list deposit orders + filters
  - Spec: OPS(4.10)

### FE – Admin

- [ ] **P1-ADMIN-FE-1.1** (0.75 PD) – Admin shell layout + nav
  - Spec: OPS(4.10)

- [ ] **P1-ADMIN-FE-1.2** (1.0 PD) – Users table + lock/unlock actions
  - Spec: OPS(4.10)

- [ ] **P1-ADMIN-FE-1.3** (1.0 PD) – Deposit table + filters
  - Spec: OPS(4.10)

### QA – Admin Tests

- [ ] **P1-ADMIN-QA-1.1** (0.75 PD) – RBAC E2E: non-admin cannot access
  - Spec: OPS(4.10)

- [ ] **P1-ADMIN-QA-1.2** (0.75 PD) – E2E: lock/unlock changes reflected
  - Spec: OPS(4.10)

---

## Tổng kết Phase 1

| Workstream | Số task | Tổng PD |
|---|---|---:|
| BE | 51 | ~45 |
| FE | 18 | ~17 |
| DB | 2 | 1.0 |
| DevOps | 4 | 3.0 |
| QA | 13 | ~13 |
| **Tổng** | **88** | **~79** |

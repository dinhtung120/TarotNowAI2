# Phase 1 – Test / Verification Checklist

**Mục đích:** Kiểm tra tất cả các bước trong Phase 1 (MVP) đã hoàn thành đúng chưa.  
**Cách dùng:** Chạy từng test case, đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Auth – Register (P1-AUTH-BE-1.1 → 1.2)

### Test 1.1 – Register thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@test.com","username":"testuser","password":"Test@1234","displayName":"Tester","dateOfBirth":"2000-01-01","consents":{"tos":true,"privacy":true,"ai_disclaimer":true}}' \
  | python3 -m json.tool
```
- [ ] Trả 201 Created (hoặc 200 OK)
- [ ] Không trả password/hash trong response
- [ ] user_id được tạo

### Test 1.2 – Trùng email/username bị chặn
```bash
# Gọi register lần 2 cùng email
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -d '{"email":"test@test.com","username":"testuser2","password":"Test@1234","displayName":"Tester2","dateOfBirth":"2000-01-01","consents":{"tos":true,"privacy":true,"ai_disclaimer":true}}' \
  -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Trả 409 Conflict hoặc ProblemDetails error code rõ
- [ ] Error code phân biệt: trùng email vs trùng username

### Test 1.3 – Password policy
```bash
# Mật khẩu yếu
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -d '{"email":"weak@test.com","username":"weakuser","password":"123","displayName":"Weak","dateOfBirth":"2000-01-01","consents":{"tos":true,"privacy":true,"ai_disclaimer":true}}' \
  -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Mật khẩu yếu bị reject (400 Bad Request)
- [ ] Error message chỉ rõ policy vi phạm

### Test 1.4 – Age gate 18+
```bash
# DOB < 18 tuổi
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -d '{"email":"young@test.com","username":"younguser","password":"Test@1234","displayName":"Young","dateOfBirth":"2015-01-01","consents":{"tos":true,"privacy":true,"ai_disclaimer":true}}' \
  -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] DOB < 18 bị chặn
- [ ] Error code rõ (age_gate_failed hoặc tương đương)

### Test 1.5 – Consent bắt buộc
```bash
# Thiếu consent
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -d '{"email":"noconsent@test.com","username":"nocon","password":"Test@1234","displayName":"NoCon","dateOfBirth":"2000-01-01","consents":{"tos":false,"privacy":true,"ai_disclaimer":true}}' \
  -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Thiếu consent TOS/Privacy/AI disclaimer bị reject
- [ ] Error chỉ rõ field nào thiếu

---

## 2. Auth – OTP & Email Verify (P1-AUTH-BE-2.1 → 2.3)

### Test 2.1 – OTP được tạo
```sql
SELECT * FROM email_otps WHERE email = 'test@test.com' ORDER BY created_at DESC LIMIT 1;
```
- [ ] OTP 6 số tồn tại
- [ ] `expires_at` = created_at + 30 phút
- [ ] `is_used = false`

### Test 2.2 – Verify OTP thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/verify-email \
  -H "Content-Type: application/json" \
  -d '{"email":"test@test.com","otp":"XXXXXX"}' | python3 -m json.tool
```
- [ ] Trả 200 OK
- [ ] `email_verified_at` được set trong DB

### Test 2.3 – Verify → +5 Gold
```sql
-- Kiểm tra sau verify
SELECT gold_balance FROM users WHERE email = 'test@test.com';
SELECT * FROM wallet_transactions WHERE user_id = '<user_id>' AND type = 'credit' AND reference_type = 'register_bonus';
```
- [ ] `gold_balance = 5`
- [ ] Có 1 dòng ledger `+5 Gold` với reference đúng

### Test 2.4 – OTP dùng lại bị chặn
```bash
# Gọi verify lần 2 cùng OTP
curl -s -X POST http://localhost:5000/api/v1/auth/verify-email \
  -d '{"email":"test@test.com","otp":"XXXXXX"}' \
  -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] OTP đã dùng bị reject
- [ ] Verify lần 2 KHÔNG cộng Gold thêm (idempotent)

### Test 2.5 – OTP hết hạn bị chặn
- [ ] OTP quá 30 phút bị reject với error code rõ

---

## 3. Auth – Login & JWT (P1-AUTH-BE-1.3 → 1.5)

### Test 3.1 – Login thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"login":"test@test.com","password":"Test@1234"}' -c cookies.txt | python3 -m json.tool
```
- [ ] Trả access token (JWT) trong body
- [ ] Refresh token set trong httpOnly cookie
- [ ] Cookie flags: `HttpOnly=true, Secure=true, SameSite=Strict/Lax`

### Test 3.2 – Login sai password
- [ ] Trả 401 Unauthorized
- [ ] Không leak thông tin user tồn tại hay không

### Test 3.3 – Refresh token rotation
```bash
# Dùng refresh token để lấy access mới
curl -s -X POST http://localhost:5000/api/v1/auth/refresh -b cookies.txt -c cookies.txt | python3 -m json.tool
```
- [ ] Access token mới được trả
- [ ] Refresh token cookie mới (rotated)
- [ ] Refresh token cũ bị invalidate

### Test 3.4 – Reuse detection → revoke chain
```bash
# Dùng refresh token CŨ (đã rotated)
curl -s -X POST http://localhost:5000/api/v1/auth/refresh -b old_cookies.txt | python3 -m json.tool
```
- [ ] Reject + revoke toàn bộ chain
- [ ] User bị buộc login lại

### Test 3.5 – Revoke session
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/revoke-all \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Tất cả refresh tokens bị revoke
- [ ] Access token cũ không dùng được (sau expiry)

---

## 4. Auth – Password Reset (P1-AUTH-BE-3.1 → 3.2)

### Test 4.1 – Forgot password
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '{"email":"test@test.com"}' | python3 -m json.tool
```
- [ ] Trả 200 (dù email tồn tại hay không — tránh enumeration)
- [ ] Token reset tạo trong DB (TTL 30 phút)

### Test 4.2 – Reset password thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/reset-password \
  -H "Content-Type: application/json" \
  -d '{"token":"<reset_token>","newPassword":"NewPass@5678"}' | python3 -m json.tool
```
- [ ] Password thay đổi thành công
- [ ] Tất cả sessions cũ bị revoke
- [ ] Token reset bị invalidate (dùng 1 lần)

---

## 5. Wallet (P1-WALLET)

### Test 5.1 – GET balance
```bash
curl -s http://localhost:5000/api/v1/wallet/balance \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Trả: `gold_balance`, `diamond_balance`, `frozen_diamond_balance`
- [ ] Giá trị khớp DB

### Test 5.2 – GET ledger
```bash
curl -s "http://localhost:5000/api/v1/wallet/ledger?page=1&limit=20" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Paging hoạt động
- [ ] Có dòng register bonus +5 Gold
- [ ] Thứ tự: created_at DESC

### Test 5.3 – Ledger invariant
```sql
-- Đối soát balance vs ledger
SELECT u.id, u.gold_balance, u.diamond_balance, v.ledger_gold, v.ledger_diamond
FROM users u
LEFT JOIN v_user_ledger_balance v ON u.id = v.user_id
WHERE u.gold_balance != v.ledger_gold OR u.diamond_balance != v.ledger_diamond;
```
- [ ] Query trả **0 rows** (không có mismatch)

### Test 5.4 – Guard: cấm UPDATE balance trực tiếp
```sql
-- Thử update trực tiếp (nếu có trigger/policy ngăn)
UPDATE users SET gold_balance = 999 WHERE email = 'test@test.com';
```
- [ ] Có mechanism ngăn chặn hoặc tài liệu quy ước rõ (code review gate)

---

## 6. Reading – RNG (P1-RNG-BE)

### Test 6.1 – RNG audit package lưu đúng
```sql
SELECT * FROM reading_rng_audits ORDER BY created_at DESC LIMIT 1;
```
- [ ] Có: `algorithm_version`, `secret_version`, `session_nonce`, `seed_digest`, `deck_order_hash`
- [ ] **Không** lưu `server_secret`
- [ ] `created_at` có giá trị

### Test 6.2 – Replay deterministic
```
# Cùng (session_nonce + user_id + timestamp + secret_version) → cùng deck order
```
- [ ] Replay cho ra cùng kết quả 100%
- [ ] Không trùng lá trong 1 phiên (unique card IDs)

### Test 6.3 – Retention >= 24 tháng
- [ ] `reading_rng_audits` KHÔNG có TTL tự xóa
- [ ] Có document/policy ghi rõ retention >= 24 tháng

---

## 7. Reading – Session (P1-READ-BE)

### Test 7.1 – Tạo reading session (daily_1)
```bash
curl -s -X POST http://localhost:5000/api/v1/readings \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"spreadType":"daily_1"}' | python3 -m json.tool
```
- [ ] Tạo `reading_sessions` document trong MongoDB
- [ ] `spread_type = "daily_1"`, `drawn_cards` có 1 lá
- [ ] `drawn_cards[0]` có: card_id, position, is_reversed, level

### Test 7.2 – Daily limit (1/ngày UTC)
```bash
# Gọi lại daily_1 cùng ngày
curl -s -X POST http://localhost:5000/api/v1/readings \
  -d '{"spreadType":"daily_1"}' \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Lần 2 trong cùng ngày UTC bị chặn
- [ ] Error code rõ (daily_limit_reached hoặc tương đương)

### Test 7.3 – Spread 3/5/10 + charge
```bash
curl -s -X POST http://localhost:5000/api/v1/readings \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"spreadType":"spread_3","question":"Tình yêu hôm nay?"}' | python3 -m json.tool
```
- [ ] `drawn_cards` có đúng 3 lá (spread_3), 5 lá (spread_5), 10 lá (spread_10)
- [ ] Không trùng card_id trong cùng 1 phiên
- [ ] Balance trừ đúng giá (wallet_transactions có ledger entry)
- [ ] Nếu thiếu balance → error chỉ rõ cost + CTA nạp

### Test 7.4 – Card collection update
```javascript
// Sau khi rút bài
db.user_collections.find({user_id: "<user_id>"}).pretty();
```
- [ ] Lá bài rút được có entry trong `user_collections`
- [ ] `total_draws` tăng, `exp` tăng, `last_drawn_at` cập nhật
- [ ] Level tính đúng theo `card_exp_levels`

---

## 8. AI Streaming (P1-AI-BE)

### Test 8.1 – SSE stream hoạt động
```bash
curl -N http://localhost:5000/api/v1/readings/<session_id>/interpret \
  -H "Authorization: Bearer <token>" -H "Accept: text/event-stream"
```
- [ ] Response là SSE format (`data: ...`)
- [ ] Tokens stream liên tục
- [ ] Kết thúc bằng event `done` hoặc tương đương

### Test 8.2 – AI request state machine
```sql
SELECT id, status, first_token_at, completed_at, charge_diamond, prompt_version
FROM ai_requests WHERE reading_session_ref = '<session_id>';
```
- [ ] `status` = `completed` sau khi stream xong
- [ ] `first_token_at` có giá trị
- [ ] `prompt_version` được ghi

### Test 8.3 – Guard order: quota → rate-limit → balance
```bash
# Gọi AI liên tục vượt quota
for i in {1..5}; do
  curl -s -X POST http://localhost:5000/api/v1/readings/<session_id>/interpret \
    -H "Authorization: Bearer <token>" | head -1
done
```
- [ ] Sau khi hết daily quota (mặc định 3) → reject với error rõ
- [ ] Không charge Diamond khi guard fail
- [ ] Không gọi AI provider khi guard fail

### Test 8.4 – In-flight cap (max 2)
- [ ] Gọi 3 AI requests đồng thời → request thứ 3 bị reject
- [ ] Error code: `ai_in_flight_limit` hoặc tương đương

### Test 8.5 – Timeout + refund
```sql
-- Simulate timeout (30s không có first token)
SELECT * FROM ai_requests WHERE status = 'failed_before_first_token';
SELECT * FROM wallet_transactions WHERE reference_type = 'ai_refund';
```
- [ ] Status = `failed_before_first_token` sau timeout 30s
- [ ] Diamond refund đúng số đã charge
- [ ] Quota rollback 1 unit
- [ ] Refund idempotent (gọi lại không tạo thêm ledger)

### Test 8.6 – AI safety / moderation
- [ ] Input vi phạm `hard_block` → không gọi model, không charge
- [ ] Response chứa nội dung nguy hiểm → chuyển sang safe template
- [ ] Prompt/response logs được redact PII

### Test 8.7 – Locale
```sql
SELECT requested_locale, returned_locale, fallback_reason FROM ai_requests LIMIT 5;
```
- [ ] `requested_locale` ghi đúng locale user (vi/en/zh-Hans)
- [ ] Nếu fallback → `fallback_reason` có giá trị

---

## 9. Follow-up (P1-FUP-BE)

### Test 9.1 – Free slots theo card level
```
Level cao nhất trong session = 6  → 1 free slot
Level cao nhất trong session = 11 → 2 free slots
Level cao nhất trong session = 16 → 3 free slots
Level cao nhất trong session < 6  → 0 free slots
```
- [ ] Free slot count đúng theo level cao nhất
- [ ] Follow-up miễn phí KHÔNG charge Diamond
- [ ] Follow-up miễn phí VẪN trừ AI quota

### Test 9.2 – Paid tiers progression
```
Ví dụ level 6 (1 free slot):
  Lần 1: FREE (0 Diamond)
  Lần 2: 2 Diamond
  Lần 3: 4 Diamond
  Lần 4: 8 Diamond
  Lần 5: 16 Diamond
```
- [ ] Free slot dùng trước → paid bắt đầu ở tier tiếp theo
- [ ] Giá đúng progression [1, 2, 4, 8, 16]

### Test 9.3 – Hard cap 5
```bash
# Follow-up lần 6
curl -s -X POST http://localhost:5000/api/v1/readings/<session_id>/followup \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"question":"test"}' | python3 -m json.tool
```
- [ ] Follow-up lần 6 bị chặn
- [ ] Error code rõ (followup_cap_reached)

### Test 9.4 – UX copy cho free slot
- [ ] Khi free slot: UI hiển thị "Miễn phí Diamond, vẫn tiêu tốn AI quota ngày"

---

## 10. Reading History (P1-HIST)

### Test 10.1 – List sessions
```bash
curl -s "http://localhost:5000/api/v1/readings?page=1&limit=10" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Trả list reading sessions của user
- [ ] Paging hoạt động
- [ ] Có spread_type, created_at, card count

### Test 10.2 – Session detail
```bash
curl -s "http://localhost:5000/api/v1/readings/<session_id>" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Trả đủ: drawn_cards, question, AI interpretation, followups
- [ ] Chỉ trả data của chính user (không truy cập session người khác)

---

## 11. Legal & Consent (P1-LEGAL)

### Test 11.1 – Consent persist
```sql
SELECT * FROM user_consents WHERE user_id = '<user_id>';
```
- [ ] Có consent cho TOS, Privacy, AI disclaimer
- [ ] Mỗi consent có `version`, `consented_at`

### Test 11.2 – Re-consent khi version thay đổi
```sql
-- Simulate version change
UPDATE system_configs SET value = '2.0' WHERE key = 'tos_version';
```
- [ ] User bị yêu cầu re-consent ở request tiếp theo
- [ ] Không cho truy cập features cho đến khi consent lại

### Test 11.3 – TOS/Privacy pages accessible
```bash
curl -s http://localhost:3000/terms | head -5
curl -s http://localhost:3000/privacy | head -5
```
- [ ] Trang TOS truy cập được (public route)
- [ ] Trang Privacy truy cập được
- [ ] Có SEO basics (title, meta description)

---

## 12. Profile (P1-PROFILE)

### Test 12.1 – Update display name + avatar
```bash
curl -s -X PATCH http://localhost:5000/api/v1/profile \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"displayName":"Tên Mới","avatarUrl":"https://example.com/avatar.jpg"}' | python3 -m json.tool
```
- [ ] Display name cập nhật thành công
- [ ] Avatar URL lưu đúng

### Test 12.2 – Update DOB → auto-calc zodiac/numerology
```bash
curl -s -X PATCH http://localhost:5000/api/v1/profile \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"dateOfBirth":"1995-07-15"}' | python3 -m json.tool
```
- [ ] DOB cập nhật
- [ ] Zodiac tính đúng (Cancer cho July 15)
- [ ] Numerology number tính đúng

---

## 13. Deposit (P1-DEP)

### Test 13.1 – Tạo deposit order
```bash
curl -s -X POST http://localhost:5000/api/v1/deposits \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"amount":50000,"method":"vietqr"}' | python3 -m json.tool
```
- [ ] Tạo `deposit_orders` row (status = pending)
- [ ] Trả payment URL/QR code

### Test 13.2 – Webhook signature verify
```bash
# Gửi webhook với signature sai
curl -s -X POST http://localhost:5000/api/v1/webhooks/payment \
  -H "X-Signature: invalid_sig" \
  -d '{"orderId":"xxx","status":"success"}' | python3 -m json.tool
```
- [ ] Signature sai → reject (400/401)
- [ ] Không credit Diamond
- [ ] Không side effects

### Test 13.3 – Webhook → credit Diamond
```bash
# Gửi webhook hợp lệ (signature đúng)
```
- [ ] `deposit_orders.status` = completed
- [ ] Diamond credit đúng (qua `proc_wallet_credit`)
- [ ] Ledger entry tồn tại với reference_id = deposit_order.id
- [ ] `provider_amount`, `provider_currency`, `fx_rate_snapshot`, `diamond_credited` lưu đầy đủ

### Test 13.4 – Double-send idempotent
```bash
# Gửi webhook 2 lần cùng order_id
```
- [ ] Chỉ credit 1 lần
- [ ] Lần 2 trả 200 OK (hoặc 409) nhưng KHÔNG credit thêm
- [ ] Ledger chỉ có 1 dòng

### Test 13.5 – Promotions auto-apply
```sql
-- Tạo promotion active
INSERT INTO deposit_promotions (name, min_amount, bonus_diamond, starts_at, ends_at, is_active)
VALUES ('Test Promo', 50000, 10, NOW() - INTERVAL '1 hour', NOW() + INTERVAL '1 day', true);
```
- [ ] Deposit >= min_amount → tự động apply bonus
- [ ] Diamond credited = base + bonus
- [ ] Ledger ghi rõ base vs bonus

---

## 14. Admin (P1-ADMIN)

### Test 14.1 – RBAC enforcement
```bash
# Non-admin truy cập admin endpoint
curl -s http://localhost:5000/api/v1/admin/users \
  -H "Authorization: Bearer <user_token>" | python3 -m json.tool
```
- [ ] Non-admin → 403 Forbidden
- [ ] Admin → 200 OK + list users

### Test 14.2 – Lock/unlock user
```bash
curl -s -X POST http://localhost:5000/api/v1/admin/users/<user_id>/lock \
  -H "Authorization: Bearer <admin_token>" | python3 -m json.tool
```
- [ ] User bị lock → không login được
- [ ] Unlock → login được lại
- [ ] Audit trail ghi `admin_actions`

### Test 14.3 – List deposit orders
```bash
curl -s "http://localhost:5000/api/v1/admin/deposits?status=completed&page=1" \
  -H "Authorization: Bearer <admin_token>" | python3 -m json.tool
```
- [ ] Filters hoạt động (status, date range)
- [ ] Paging hoạt động

---

## 15. FE – Auth UI (P1-AUTH-FE)

- [ ] Register form: validate realtime (email format, password strength, DOB)
- [ ] Verify OTP: 6-digit input + resend button + countdown
- [ ] Login: error messages thân thiện (không leak info)
- [ ] Forgot/reset: flow hoàn chỉnh, token expired state handled

---

## 16. FE – Wallet & Reading UI (P1-WALLET-FE, P1-READ-FE)

- [ ] Wallet widget hiển thị Gold/Diamond/Frozen
- [ ] Ledger list: paging, empty state, loading state
- [ ] Spread selector: 1/3/5/10 cards with cost display
- [ ] Card reveal: animation mượt, không blank/flicker
- [ ] Insufficient balance → error message + CTA nạp tiền

---

## 17. FE – AI & Follow-up UI (P1-AI-FE, P1-FUP-FE)

- [ ] SSE streaming: loading → streaming text → completed
- [ ] Error state: timeout/fail → hiện thông báo + ghi chú refund
- [ ] Follow-up list: hiện cost badge (FREE / N Diamond)
- [ ] Cap reached: hiện thông báo + disable composer
- [ ] Free slot copy: "Miễn phí Diamond, vẫn tiêu tốn AI quota ngày"

---

## 18. FE – Admin UI (P1-ADMIN-FE)

- [ ] Admin layout: sidebar + navigation
- [ ] Users table: list + search + lock/unlock actions
- [ ] Deposit table: list + filters (status, date)
- [ ] Promotion CRUD: create/edit/delete promotions

---

## Tổng kết Phase 1 Test

| # | Nhóm | Số test case | Kết quả |
|---|---|---:|---|
| 1 | Register | 5 | |
| 2 | OTP & Verify | 5 | |
| 3 | Login & JWT | 5 | |
| 4 | Password Reset | 2 | |
| 5 | Wallet | 4 | |
| 6 | RNG | 3 | |
| 7 | Reading Session | 4 | |
| 8 | AI Streaming | 7 | |
| 9 | Follow-up | 4 | |
| 10 | History | 2 | |
| 11 | Legal & Consent | 3 | |
| 12 | Profile | 2 | |
| 13 | Deposit | 5 | |
| 14 | Admin | 3 | |
| 15–18 | FE UI (Auth/Wallet/AI/Admin) | 15 | |
| **Tổng** | | **69** | |

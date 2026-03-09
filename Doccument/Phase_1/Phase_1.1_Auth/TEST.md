# Phase 1.1 – Auth Test Checklist

**Cách dùng:** Đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Register

### Test 1.1 – Register thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@test.com","username":"testuser","password":"Test@1234","displayName":"Tester","dateOfBirth":"2000-01-01","consents":{"tos":true,"privacy":true,"ai_disclaimer":true}}' | python3 -m json.tool
```
- [ ] Trả 201 Created
- [ ] Không trả password/hash trong response

### Test 1.2 – Trùng email/username
- [ ] Trùng email → 409 + error code rõ
- [ ] Trùng username → 409 + error code phân biệt

### Test 1.3 – Password policy
- [ ] Mật khẩu yếu ("123") → reject 400
- [ ] Error message chỉ rõ policy vi phạm

### Test 1.4 – Age gate 18+
- [ ] DOB < 18 tuổi → reject
- [ ] Error code: `age_gate_failed`

### Test 1.5 – Consent bắt buộc
- [ ] Thiếu TOS/Privacy/AI disclaimer → reject
- [ ] Error chỉ rõ field thiếu

---

## 2. OTP & Email Verify

### Test 2.1 – OTP tạo đúng
```sql
SELECT * FROM email_otps WHERE email = 'test@test.com' ORDER BY created_at DESC LIMIT 1;
```
- [ ] OTP 6 số, `expires_at` = +30 phút, `is_used = false`

### Test 2.2 – Verify OTP → +5 Gold
```sql
SELECT gold_balance FROM users WHERE email = 'test@test.com';
SELECT * FROM wallet_transactions WHERE type = 'credit' AND reference_type = 'register_bonus';
```
- [ ] `gold_balance = 5`
- [ ] Có ledger entry +5 Gold

### Test 2.3 – OTP dùng lại / hết hạn
- [ ] OTP đã dùng → reject
- [ ] OTP quá 30 phút → reject
- [ ] Verify lần 2 KHÔNG cộng Gold thêm (idempotent)

---

## 3. Login & JWT

### Test 3.1 – Login thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"login":"test@test.com","password":"Test@1234"}' -c cookies.txt | python3 -m json.tool
```
- [ ] Access token JWT trong body
- [ ] Refresh token trong httpOnly cookie
- [ ] Cookie: `HttpOnly=true, Secure=true, SameSite=Strict/Lax`

### Test 3.2 – Login sai → không leak info
- [ ] 401 Unauthorized, không nói user tồn tại hay không

### Test 3.3 – Refresh rotation
- [ ] Access mới trả về, refresh mới set cookie
- [ ] Refresh cũ bị invalidate

### Test 3.4 – Reuse detection → revoke chain
- [ ] Dùng refresh cũ → reject + revoke toàn bộ chain

### Test 3.5 – Revoke all sessions
- [ ] Tất cả refresh tokens bị revoke

---

## 4. Password Reset

### Test 4.1 – Forgot password
- [ ] Trả 200 dù email tồn tại hay không (chống enumeration)
- [ ] Token reset TTL 30 phút

### Test 4.2 – Reset thành công
- [ ] Password đổi, sessions cũ revoke, token invalidate

---

## Tổng kết: **17 test cases**

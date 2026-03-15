# Phase 1.1 – Auth Test Checklist

**Cách dùng:** Đánh dấu `[x]` khi PASS, ghi note nếu FAIL hoặc có điều chỉnh nghiệp vụ.

---

## 1. Register

### Test 1.1 – Register thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@test.com","username":"testuser","password":"Test@1234","displayName":"Tester","dateOfBirth":"2000-01-01","hasConsented":true}' | python3 -m json.tool
```
- [x] Trả 201 Created
- [x] Không trả password/hash trong response (Unit Tests verified)

### Test 1.2 – Trùng email/username
- [x] Trùng email → 422 UnprocessableEntity (ValidationException) + error code rõ
- [x] Trùng username → 422 UnprocessableEntity + error code phân biệt

### Test 1.3 – Password policy
- [x] Mật khẩu yếu ("123") → reject 400 (FluentValidation)
- [x] Error message chỉ rõ policy vi phạm

### Test 1.4 – Age gate
- [x] DOB < 16/18 tuổi → reject (Hiện cài đặt 16 tuổi theo Validation)
- [x] Error code: `age_gate_failed` / Validation error

### Test 1.5 – Consent bắt buộc
- [x] Thiếu Checkbox Đồng thuận (HasConsented = false) → reject
- [x] Error chỉ rõ field thiếu

---

## 2. OTP & Email Verify

### Test 2.1 – OTP tạo đúng
```sql
SELECT * FROM email_otps WHERE email = 'test@test.com' ORDER BY created_at DESC LIMIT 1;
```
- [x] OTP 6 số, `expires_at` = +15 phút, `is_used = false` *(Note: Giảm TTL xuống 15 phút để tăng tính bảo mật thay vì 30 phút)*

### Test 2.2 – Verify OTP → +5 Gold
```sql
SELECT gold_balance FROM users WHERE email = 'test@test.com';
SELECT * FROM wallet_transactions WHERE type = 'credit' AND reference_type = 'register_bonus';
```
- [x] `gold_balance = 5`
- [x] Có ledger entry +5 Gold (`register_bonus`)

### Test 2.3 – OTP dùng lại / hết hạn
- [x] OTP đã dùng → reject (DomainException "INVALID_OTP")
- [x] OTP quá hạn → reject
- [x] Verify lần 2 KHÔNG thực hiện nếu User Status đã là Active.

---

## 3. Login & JWT

### Test 3.1 – Login thành công
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"login":"test@test.com","password":"Test@1234"}' -c cookies.txt | python3 -m json.tool
```
- [x] Access token JWT trong response body
- [x] Refresh token set-cookie ẩn trong Header HTTP.
- [x] Cookie cấu hình chuẩn: `HttpOnly=true, Secure=true, SameSite=Strict`

### Test 3.2 – Login sai → không leak info
- [x] 401 Unauthorized, không nói user tồn tại hay không (Tránh Email enumeration)

### Test 3.3 – Refresh rotation
- [x] Access mới trả về, refresh mới set cookie đè lên cookie cũ.
- [x] Refresh Token cũ bị đánh dấu là đã sử dụng / Revoke trong DB.

### Test 3.4 – Reuse detection → revoke chain
- [x] Phát hiện dùng lại Refresh Token cũ → Alert 'TOKEN_COMPROMISED'.
- [x] Tự động revoke (thu hồi) toàn bộ Refresh Tokens chain của User đó để bảo mật.

### Test 3.5 – Revoke all sessions (Logout)
- [x] Tất cả refresh tokens bị revoke sau khi Logout All. Thực thi xóa Cookie ở Local trình duyệt.

---

## 4. Password Reset

### Test 4.1 – Forgot password
- [x] Trả 200 dù email tồn tại hay không (chống enumeration / user data leak)
- [x] Token reset TTL được cấp = 15 phút

### Test 4.2 – Reset thành công
- [x] Cập nhật Password Hash mới.
- [x] Mark mã OTP = IsUsed.
- [x] Tự động Logout Everywhere (Thu hồi toàn bộ Refresh Tokens của User).

---

## Tổng kết Phase 1.1
- **Tổng số Test Cases:** 17 Cases thiết kế
- **Tình trạng:**
  - Automated Tests: Đã code Backend Unit Test (xUnit, Moq, FluentAssertions). Tổng cộng 26/26 Unit Tests cho Core Logic Handlers (Login, Register, Rotate, Revoke, OTP) **PASSED 100%**.
  - Frontend: Hoàn thiện Client E2E qua Playwright. `npm run lint` **Zero Errors**.
  - Backend API: `dotnet build` & `dotnet test` Pipeline **PASSED**.
- **Pending:** Đã hoàn thành 100% Phase 1.1.

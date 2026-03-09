# Phase 3 – Test / Verification Checklist

**Mục đích:** Kiểm tra tất cả các bước trong Phase 3 (Mobile Parity) đã hoàn thành đúng chưa.  
**Cách dùng:** Chạy từng test case, đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Mobile Scaffold (P3-MOB-FE-1.1)

### Test 1.1 – Expo project build
```bash
cd mobile/  # hoặc đường dẫn mobile project
npx expo prebuild
npx expo run:ios    # hoặc run:android
```
- [ ] Build iOS thành công (hoặc simulator)
- [ ] Build Android thành công (hoặc emulator)
- [ ] App khởi động không crash

### Test 1.2 – Navigation skeleton
- [ ] Stack navigation hoạt động
- [ ] Có routes chính: Auth, Home, Reading, Chat, Wallet, Profile
- [ ] Bottom tab hoặc drawer navigation hoạt động

---

## 2. Secure Token Storage (P3-MOB-FE-1.2)

### Test 2.1 – Refresh token lưu an toàn
- [ ] iOS: Token lưu trong Keychain (không AsyncStorage)
- [ ] Android: Token lưu trong Keystore (không SharedPreferences plain)
- [ ] Token không hiện trong app logs
- [ ] Device binding basic hoạt động

### Test 2.2 – Token rotation trên mobile
- [ ] Refresh token rotation hoạt động giống web
- [ ] Token mới lưu đúng secure storage
- [ ] Reuse detection → revoke + buộc login lại

---

## 3. Auth Mobile Parity (P3-MOB-FE-1.3)

### Test 3.1 – Register flow
- [ ] Form register: username, email, password, DOB, consents
- [ ] Validation realtime (email format, password strength)
- [ ] Error mapping: ProblemDetails → thông điệp thân thiện

### Test 3.2 – Verify OTP flow
- [ ] Input 6 ký tự OTP
- [ ] Resend button + countdown
- [ ] Success → navigate to main screen

### Test 3.3 – Login flow
- [ ] Login email/username + password
- [ ] Token lưu secure storage
- [ ] Auto-login khi mở app (nếu token còn hạn)

### Test 3.4 – Error states
- [ ] Network error → retry UX
- [ ] Server error → thông báo rõ
- [ ] Session expired → buộc login lại

---

## 4. Wallet Parity (P3-MOB-FE-2.1)

### Test 4.1 – Balance display
- [ ] Hiển thị Gold/Diamond/Frozen Balance
- [ ] Pull-to-refresh cập nhật

### Test 4.2 – Ledger view
- [ ] List giao dịch có paging
- [ ] Empty state khi chưa có giao dịch
- [ ] Loading state mượt

---

## 5. Reading Parity (P3-MOB-FE-2.2)

### Test 5.1 – Spread selector
- [ ] Chọn 1/3/5/10 lá hoạt động
- [ ] Hiển thị cost trước khi confirm
- [ ] Question input (optional)

### Test 5.2 – Card reveal
- [ ] Animation reveal/lật bài cơ bản hoạt động
- [ ] Cards hiển thị đúng: tên, hình ảnh, reversed state
- [ ] Không blank/flicker

### Test 5.3 – Daily limit
- [ ] Daily_1 chỉ cho rút 1 lần/ngày
- [ ] Error state khi đã rút rồi

---

## 6. SSE Streaming Parity (P3-MOB-FE-2.3)

### Test 6.1 – AI streaming trên mobile
- [ ] SSE client connect + nhận tokens
- [ ] Hiển thị streaming text chuẩn
- [ ] Completion detection (stream end)

### Test 6.2 – Reconnect khi network unstable
- [ ] Mất mạng → reconnect tự động
- [ ] Hiển thị trạng thái: connecting/connected/error
- [ ] Không mất data đã nhận

---

## 7. Chat Parity (P3-MOB-FE-3.1 → 3.2)

### Test 7.1 – SignalR trên mobile
- [ ] Connect/disconnect/reconnect
- [ ] App vào background → disconnect graceful
- [ ] App trở lại foreground → reconnect + sync messages mới
- [ ] Nhận messages real-time

### Test 7.2 – Chat escrow UI
- [ ] Offer/accept/reject → render đúng
- [ ] Timer countdown hoạt động
- [ ] Status badges: pending/accepted/replied/completed/disputed
- [ ] Dispute CTA hiển thị trong window

---

## 8. Push Notifications (P3-MOB-OPS-1.1 → 1.2)

### Test 8.1 – Push notification delivery
- [ ] FCM (Android): nhận push khi app background
- [ ] APNs (iOS): nhận push khi app background
- [ ] Notification payload không chứa sensitive data
- [ ] Tapping notification → open app

### Test 8.2 – Deep-link routing
- [ ] Push "tin nhắn mới" → mở đúng conversation
- [ ] Push "reading completed" → mở đúng reading detail
- [ ] Push "wallet update" → mở wallet screen
- [ ] Deep-link khi app killed → launch + navigate đúng

---

## 9. Cross-Platform Smoke Tests (P3-MOB-QA)

### Test 9.1 – Parity checklist
- [ ] Auth flow: register → verify → login (parity web)
- [ ] Reading flow: select spread → draw → AI stream → follow-up (parity web)
- [ ] Chat flow: create conversation → message → escrow (parity web)
- [ ] Wallet: balance + ledger hiển thị đúng (parity web)

### Test 9.2 – Edge cases
- [ ] Offline mode → error handling graceful
- [ ] App kill + reopen → session restored
- [ ] Rotate device → layout responsive
- [ ] Low memory → no crash

---

## Tổng kết Phase 3 Test

| # | Nhóm | Số test case | Kết quả |
|---|---|---:|---|
| 1 | Mobile Scaffold | 2 | |
| 2 | Secure Storage | 2 | |
| 3 | Auth Parity | 4 | |
| 4 | Wallet Parity | 2 | |
| 5 | Reading Parity | 3 | |
| 6 | SSE Parity | 2 | |
| 7 | Chat Parity | 2 | |
| 8 | Push + Deep-link | 2 | |
| 9 | Cross-Platform Smoke | 2 | |
| **Tổng** | | **21** | |

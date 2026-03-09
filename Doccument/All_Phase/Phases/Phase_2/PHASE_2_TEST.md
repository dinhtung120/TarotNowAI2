# Phase 2 – Test / Verification Checklist

**Mục đích:** Kiểm tra tất cả các bước trong Phase 2 (Reader Marketplace + Chat + Escrow) đã hoàn thành đúng chưa.  
**Cách dùng:** Chạy từng test case, đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Reader Listing & Approval (P2-READER)

### Test 1.1 – Submit reader request
```bash
curl -s -X POST http://localhost:5000/api/v1/reader-requests \
  -H "Authorization: Bearer <user_token>" -H "Content-Type: application/json" \
  -d '{"bio":"Tarot reader 5 năm","specialties":["love","career"],"pricePerQuestion":10}' | python3 -m json.tool
```
- [ ] Tạo `reader_requests` document trong MongoDB
- [ ] Status = `pending`
- [ ] User chưa có quyền reader

### Test 1.2 – Admin approve reader
```bash
curl -s -X POST http://localhost:5000/api/v1/admin/reader-requests/<id>/approve \
  -H "Authorization: Bearer <admin_token>" | python3 -m json.tool
```
- [ ] `reader_requests.status` = `approved`
- [ ] `users.role` = `tarot_reader`
- [ ] `reader_profiles` document tạo trong MongoDB
- [ ] Audit trail ghi `admin_actions`

### Test 1.3 – Admin reject reader
- [ ] `reader_requests.status` = `rejected`
- [ ] User vẫn role `user`
- [ ] Có reason trong rejection

### Test 1.4 – Reader directory listing
```bash
curl -s "http://localhost:5000/api/v1/readers?page=1&sortBy=rating" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Chỉ hiện readers approved
- [ ] Filters: giá, đánh giá, chuyên môn hoạt động
- [ ] Paging hoạt động

### Test 1.5 – Gate: chỉ accepting_questions mới nhận chat
```bash
# Reader KHÔNG accepting_questions
curl -s -X POST http://localhost:5000/api/v1/conversations \
  -d '{"readerId":"<offline_reader_id>"}' \
  -H "Authorization: Bearer <user_token>" -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Reader offline/not accepting → reject với error rõ
- [ ] Reader approved + accepting_questions → OK

---

## 2. Chat 1-1 SignalR (P2-CHAT)

### Test 2.1 – Tạo conversation
```bash
curl -s -X POST http://localhost:5000/api/v1/conversations \
  -d '{"readerId":"<reader_id>","question":"Tình duyên?","offerAmount":10}' \
  -H "Authorization: Bearer <user_token>" -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Tạo `conversations` document trong MongoDB
- [ ] Status = `pending`
- [ ] `offer_expires_at` = created_at + 12h (hoặc config)

### Test 2.2 – SignalR connect/send/receive
- [ ] User và Reader connect được vào hub
- [ ] Gửi tin nhắn → đối phương nhận real-time
- [ ] Messages persist vào `chat_messages` collection

### Test 2.3 – Message types
```javascript
db.chat_messages.find({conversation_id: "<conv_id>"}).sort({created_at: 1});
```
- [ ] Type `text`: tin nhắn thường lưu đúng
- [ ] Type `system`: system messages lưu đúng
- [ ] Type `payment_offer`, `payment_accept`: escrow messages lưu đúng

### Test 2.4 – Read state + unread count
- [ ] Gửi messages → unread count tăng cho đối phương
- [ ] Mở chat → mark read → unread count giảm về 0
- [ ] Read receipts hiển thị đúng

### Test 2.5 – Report
```bash
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/report \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"reason":"spam","details":"Quảng cáo liên tục"}' | python3 -m json.tool
```
- [ ] Report tạo trong `reports` collection
- [ ] Admin queue nhận report

---

## 3. Escrow Core (P2-ESCROW)

### Test 3.1 – Freeze on accept
```bash
# Reader accept offer
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/accept \
  -H "Authorization: Bearer <reader_token>" | python3 -m json.tool
```
- [ ] `chat_finance_sessions` row tạo trong PostgreSQL
- [ ] `chat_question_items` row tạo (status = accepted)
- [ ] Diamond frozen via `proc_wallet_freeze`
- [ ] `users.frozen_diamond_balance` tăng
- [ ] `users.diamond_balance` giảm tương ứng

### Test 3.2 – Offer timeout → auto-cancel
```sql
-- Tạo offer với offer_expires_at trong quá khứ
-- Chạy timer job
```
- [ ] Offer quá hạn → auto-cancel
- [ ] Nếu đã freeze → auto-refund
- [ ] `conversation.status` = `cancelled`

### Test 3.3 – Reader reply → set auto_release
```bash
# Reader gửi reply
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/reply \
  -H "Authorization: Bearer <reader_token>" -H "Content-Type: application/json" \
  -d '{"message":"Kết quả reading..."}' | python3 -m json.tool
```
- [ ] `chat_question_items.status` = `replied`
- [ ] `auto_release_at` = replied_at + 24h
- [ ] Message lưu trong `chat_messages`

### Test 3.4 – User confirm → release
```bash
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/confirm \
  -H "Authorization: Bearer <user_token>" | python3 -m json.tool
```
- [ ] `proc_wallet_release` called
- [ ] Reader nhận Diamond (frozen → reader balance)
- [ ] Platform fee 10% khấu trừ
- [ ] Ledger entries tạo cho cả payer và receiver
- [ ] `conversation.status` = `completed`

### Test 3.5 – Auto-release (no dispute in 24h)
- [ ] Sau 24h không confirm/dispute → auto-release via job
- [ ] Diamond giải phóng cho reader
- [ ] Job idempotent (chạy lại không release 2 lần)

### Test 3.6 – No-reply auto-refund (24h)
- [ ] Reader không reply trong 24h → auto-refund
- [ ] `proc_wallet_refund` called
- [ ] User nhận lại frozen Diamond
- [ ] `conversation.status` = `cancelled`

### Test 3.7 – Open dispute
```bash
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/dispute \
  -H "Authorization: Bearer <user_token>" -H "Content-Type: application/json" \
  -d '{"reason":"Không trả lời đúng câu hỏi"}' | python3 -m json.tool
```
- [ ] `chat_question_items.status` = `disputed`
- [ ] Dispute window: trong 24h sau release/auto_release
- [ ] Dispute ngoài window → reject

### Test 3.8 – Add-question (escrow cộng dồn)
```bash
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/add-question \
  -H "Authorization: Bearer <user_token>" -H "Content-Type: application/json" \
  -d '{"question":"Câu hỏi thêm","amount":10}' | python3 -m json.tool
```
- [ ] Tạo thêm `chat_question_items` row
- [ ] Freeze thêm Diamond
- [ ] Timer riêng cho question mới

### Test 3.9 – Idempotency tests
```bash
# Double-freeze: gửi accept 2 lần
# Double-release: gửi confirm 2 lần
# Double-refund: trigger refund 2 lần
```
- [ ] Double-freeze bị chặn
- [ ] Double-release bị chặn (chỉ release 1 lần)
- [ ] Double-refund bị chặn

### Test 3.10 – Frozen balance reconciliation
```sql
SELECT u.id, u.frozen_diamond_balance, v.ledger_frozen
FROM users u
LEFT JOIN v_user_frozen_ledger_balance v ON u.id = v.user_id
WHERE u.frozen_diamond_balance != v.ledger_frozen;
```
- [ ] Query trả **0 rows** (không mismatch)

---

## 4. Withdrawal & Payout (P2-WITHDRAW)

### Test 4.1 – Create withdrawal request
```bash
curl -s -X POST http://localhost:5000/api/v1/withdrawals \
  -H "Authorization: Bearer <reader_token>" -H "Content-Type: application/json" \
  -d '{"amount":50}' | python3 -m json.tool
```
- [ ] Min 50 Diamond
- [ ] Max 1 request/ngày (UTC business date)
- [ ] KYC required

### Test 4.2 – Guards
- [ ] Amount < 50 → reject
- [ ] 2nd request cùng ngày → reject
- [ ] KYC chưa đạt → reject
- [ ] Account đang chargeback/disputed → reject

### Test 4.3 – Fee calculation
```sql
SELECT amount, fee_amount, net_amount FROM withdrawal_requests WHERE id = '<id>';
```
- [ ] `fee_amount` = amount * 10%
- [ ] `net_amount` = amount - fee_amount
- [ ] Debit Diamond via `proc_wallet_debit`

### Test 4.4 – Admin approve/reject payout
```bash
curl -s -X POST http://localhost:5000/api/v1/admin/withdrawals/<id>/approve \
  -H "Authorization: Bearer <admin_token>" | python3 -m json.tool
```
- [ ] Approve → status = completed
- [ ] Reject → status = rejected, Diamond refund to reader
- [ ] Audit trail có approver, timestamp, reason

---

## 5. MFA (P2-MFA)

### Test 5.1 – TOTP setup
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/mfa/setup \
  -H "Authorization: Bearer <reader_token>" | python3 -m json.tool
```
- [ ] Trả QR code URI + backup codes
- [ ] Backup codes lưu an toàn (hashed)

### Test 5.2 – MFA verify
```bash
curl -s -X POST http://localhost:5000/api/v1/auth/mfa/verify \
  -d '{"code":"123456"}' \
  -H "Authorization: Bearer <reader_token>" -H "Content-Type: application/json" | python3 -m json.tool
```
- [ ] Code đúng → MFA enabled
- [ ] Code sai → reject

### Test 5.3 – MFA gate enforcement
- [ ] Payout request khi MFA chưa bật → reject
- [ ] Admin action khi MFA chưa bật → reject
- [ ] Sau MFA verify → cho thực hiện

---

## 6. FE – Chat & Escrow UI

- [ ] Inbox list: conversations sorted + unread badges
- [ ] Chat screen: messages real-time, typing indicator
- [ ] Escrow offer: hiện giá + accept/reject buttons
- [ ] Timer countdown: offer expires, response due, auto-release
- [ ] Dispute CTA: hiện trong dispute window
- [ ] Reader directory: filters, profile pages, message button gating

---

## 7. FE – Admin Phase 2

- [ ] Dispute queue: list + detail + resolve actions (release/refund)
- [ ] Payout queue: list + approve/reject actions
- [ ] Reader approval queue: pending requests + approve/reject

---

## Tổng kết Phase 2 Test

| # | Nhóm | Số test case | Kết quả |
|---|---|---:|---|
| 1 | Reader Listing | 5 | |
| 2 | Chat SignalR | 5 | |
| 3 | Escrow Core | 10 | |
| 4 | Withdrawal | 4 | |
| 5 | MFA | 3 | |
| 6 | FE Chat/Escrow | 6 | |
| 7 | FE Admin | 3 | |
| **Tổng** | | **36** | |

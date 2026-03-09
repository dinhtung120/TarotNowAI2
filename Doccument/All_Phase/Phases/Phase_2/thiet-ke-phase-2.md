# Phase 2 – Tài liệu thiết kế liên quan

**Mục đích:** Trích các phần thiết kế cần đọc khi làm Phase 2 (Reader Marketplace + Chat + Escrow).  
**Nguồn gốc:** Trích từ `01-business-rules.md`, `02-product-ux-specs.md`, `03-tech-architecture.md`, `04-ops-security-compliance.md`

---

## 1. Quy tắc kinh doanh Phase 2 (từ BR)

### 2.1 Reader Listing (BR Phase 2.1)
- Danh bạ reader: tìm kiếm, lọc giá/đánh giá/chuyên môn
- Hồ sơ reader: avatar, bio, giá, trạng thái online
- Luồng duyệt reader: user submit → admin approve/reject
- Approved + accepting_questions mới nhận chat mới
- Phase 1 dùng KYC thủ công bởi admin; KYC chính thức triển khai Phase 2

### 2.2 Chat 1-1 (BR Phase 2.2)
- SignalR realtime messaging
- Luồng tin nhắn + trạng thái đã đọc (read state + unread_count)
- Kiểm duyệt chat cơ bản: spam, báo cáo
- Message types: text, system, card_share, payment_offer, payment_accept, payment_reject, system_refund, system_release, system_dispute

### 2.3 Escrow Core (BR Phase 2.3 + BR-3, BR-12, BR-19)
- **1 conversation = 1 finance session**
- Diamond bị freeze theo tổng câu hỏi accepted (main + add-question)

**Máy trạng thái escrow:**
```
pending → accepted (freeze) → replied → released / refunded / disputed
```

**Timer anchors (BR-19):**
- `offer_expires_at` = created_at + offer_timeout_hours (mặc định **12h**, configurable)
- `reader_response_due_at` = accepted_at + **24h**
- `auto_release_at` = replied_at + **24h** (nếu không confirm/dispute)
- `dispute_window_end` = dispute_window_start + **24h**
- Tất cả timer dùng UTC

**Dispute window anchor:**
- User confirm/release sớm: `dispute_window_start = release_at`
- Auto release: `dispute_window_start = auto_release_at`
- No-response auto refund: `dispute_window_start = auto_refund_at`

**Finance invariants (BR-12):**
- Mọi freeze/release/refund **bắt buộc idempotent**
- `SERIALIZABLE` hoặc `SELECT FOR UPDATE` + distributed lock
- Không double-freeze / double-release / double-refund
- Sử dụng `proc_wallet_freeze`, `proc_wallet_release`, `proc_wallet_refund`

### 2.4 Withdrawal (BR 4.3.3)
- Reader rút tiền: admin duyệt thủ công
- Phí nền tảng: **10%**
- Tối thiểu: **50 Diamond/request**, max **1 request/ngày** (UTC business date)
- KYC đạt chuẩn trước khi nhận payout
- Enhanced KYC khi tổng rút vượt ngưỡng
- Chặn payout nếu account đang chargeback/disputed hold
- SLA payout thủ công: **<= 24h**

### 2.5 MFA (BR Phase 2)
- Reader và Admin **bắt buộc** MFA trước payout/admin actions
- User thường có thể bật MFA tự nguyện
- TOTP setup + verify
- Audit log cho bật/tắt/reset MFA

---

## 2. Đặc tả UX Phase 2 (từ UX)

### 2.1 Reader Directory UX (UX 4.6.1)
- Reader listing page: filters (giá, đánh giá, chuyên môn)
- Reader profile page: avatar, bio, giá, chuyên môn, đánh giá
- Gating: nút message chỉ hiện khi reader approved + accepting_questions

### 2.2 Chat UX (UX 4.6.2)
- Inbox list: conversations grouped by status
- Chat screen: real-time messages, typing indicator (optional)
- Read receipts + unread badges
- Escrow UI in-chat: offer, accept/reject, timer countdown, status badges

### 2.3 Report UX (UX 4.6.5)
- Report button trong chat
- Reason codes selection
- Admin moderation queue

### 2.4 MFA UX (UX 4.1.2)
- MFA setup: QR code + backup codes
- MFA challenge: trước payout/admin actions

---

## 3. Kiến trúc kỹ thuật Phase 2 (từ ARCH)

### 3.1 SignalR Hub (ARCH 1.1)
- Auth + connect/disconnect lifecycle
- Message persistence to MongoDB
- Reconnect handling
- Health checks + metrics

### 3.2 Escrow State Machine (ARCH 4.6.4) – LOCKED
- `chat_finance_sessions` (PostgreSQL) + `conversations` (MongoDB) cross-reference
- `chat_question_items` per-question escrow tracking
- Timer-based jobs: auto-refund (overdue), auto-release (replied + no dispute)
- Jobs phải idempotent + retry-safe

### 3.3 Frozen Balance Reconciliation
- View `v_user_frozen_ledger_balance` cho audit
- Reconciliation: SUM(freeze) - SUM(refund) - SUM(released items) vs `users.frozen_diamond_balance`

---

## 4. Ops & Security Phase 2 (từ OPS)

### 4.1 KYC (OPS 4.13.2)
- KYC cho reader trước payout
- Enhanced KYC theo ngưỡng rút cộng dồn
- KYC/PII mã hóa at-rest (KMS managed key)
- Truy cập theo RBAC + access audit trail

### 4.2 Dispute Evidence (OPS 4.13.4)
- Giữ dispute evidence **>= 24 tháng**
- Quyền truy cập evidence: chỉ role được ủy quyền
- Export evidence cho legal/internal review

### 4.3 External Pricing Data (OPS 4.20.1)
- Pricing data sources: system_configs
- FX rate snapshot tại thời điểm capture

---

## 5. Database Schema liên quan Phase 2

### PostgreSQL - Bảng mới/quan trọng
- `chat_finance_sessions` – phiên tài chính 1-1
- `chat_question_items` – escrow từng câu hỏi (status: pending→accepted→released/refunded/disputed)
- `withdrawal_requests` – yêu cầu rút tiền
- `reader_payout_profiles` – thông tin bank reader

### MongoDB - Collections mới/quan trọng
- `reader_profiles` – hồ sơ reader (status: online/offline/accepting_questions)
- `reader_requests` – đơn xin làm reader
- `conversations` – conversations (status: pending/active/completed/cancelled/disputed)
- `chat_messages` – tin nhắn (type: text/system/card_share/payment_*/system_*)
- `reports` – báo cáo vi phạm

### Stored Procedures (bắt buộc cho escrow)
- `proc_wallet_freeze` – freeze Diamond (available → frozen)
- `proc_wallet_release` – release escrow (frozen payer → credit receiver)
- `proc_wallet_refund` – refund escrow (frozen → available)
- `proc_wallet_debit` – trừ Diamond cho withdrawal

---

## Tham chiếu đầy đủ

| Tài liệu | Sections liên quan |
|---|---|
| [01-business-rules.md](../All_Phase/tai-lieu-thiet-ke/01-business-rules.md) | Phase 2 (2.1–2.4), BR-3, 12, 15, 19, 21, 4.3.3 |
| [02-product-ux-specs.md](../All_Phase/tai-lieu-thiet-ke/02-product-ux-specs.md) | 4.1.2, 4.1.4, 4.2.2, 4.6.1–4.6.5 |
| [03-tech-architecture.md](../All_Phase/tai-lieu-thiet-ke/03-tech-architecture.md) | 4.6.3, 4.6.4 |
| [04-ops-security-compliance.md](../All_Phase/tai-lieu-thiet-ke/04-ops-security-compliance.md) | 4.13.2, 4.13.4, 4.20.1 |
| [database/DESIGN_DECISIONS.md](../../database/DESIGN_DECISIONS.md) | Escrow status flow, frozen balance, proc_wallet_* |

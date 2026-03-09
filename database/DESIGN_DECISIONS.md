# TarotWeb – Quyết định thiết kế Database

Tài liệu ghi lại các quyết định thiết kế quan trọng, invariants, và hướng dẫn triển khai.

---

## 1. Double-entry (Ghi sổ kép)

**Mô hình:** `wallet_transactions` lưu 1 row per balance-change với `user_id`. Mỗi row có `amount` (+ credit, - debit), `balance_before`, `balance_after`.

**Chuyển khoản (transfer payer → receiver):**
- Insert **2 rows** với cùng `reference_id`:
  - Row 1: payer, `amount` âm (debit)
  - Row 2: receiver, `amount` dương (credit)
- Đảm bảo audit trail và đối soát.

**Operations không chuyển khoản (deposit, cost, refund, ...):**
- 1 row per event. `reference_source` + `reference_id` trỏ tới nguồn (deposit_orders, ai_requests, ...).

**Invariant:** `balance_after = balance_before + amount` (CHECK constraint).

---

## 2. Balance consistency: users vs ledger

**Vấn đề:** `users.gold_balance`, `users.diamond_balance` phải khớp với balance cuối trong `wallet_transactions`.

**Giải pháp:**
1. **Stored procedures:** Mọi credit/debit PHẢI gọi `proc_wallet_credit` hoặc `proc_wallet_debit`. Mọi freeze/release/refund escrow PHẢI gọi `proc_wallet_freeze`, `proc_wallet_release`, `proc_wallet_refund`. Không cho app code UPDATE users + INSERT ledger rời rạc.
2. **Reconciliation view:** `v_user_ledger_balance` – balance cuối từ ledger theo (user_id, currency).
3. **Reconciliation job:** Chạy định kỳ (nightly hoặc real-time), so sánh `users.<balance>` với `v_user_ledger_balance.ledger_balance`. Alert nếu mismatch (OPS-4.19).

---

## 3. ON DELETE semantics

**Nguyên tắc:** Không dùng `ON DELETE CASCADE` cho bảng tham chiếu `users`. Dùng soft delete (`users.is_deleted`).

**Các FK critical:** `ON DELETE RESTRICT` – chặn xóa user khi còn dữ liệu con (wallet_transactions, deposit_orders, withdrawal_requests, chat_finance_sessions, ai_requests, reading_rng_audits).

**Ngoại lệ:** `email_otps`, `password_reset_tokens` – `ON DELETE CASCADE` vì dữ liệu phụ thuộc chặt, xóa user thì purge luôn.

> **L1 fix:** `user_consents` dùng `ON DELETE RESTRICT` (không CASCADE) để giữ consent history cho compliance audit (OPS-4.13.1). Không xóa consent khi xóa user.

---

## 4. Idempotency

**Partial unique index:** `idempotency_key` dùng `CREATE UNIQUE INDEX ... WHERE idempotency_key IS NOT NULL` – explicit, cho phép nhiều NULL.

**Bảng có idempotency_key:** wallet_transactions, chat_question_items, ai_requests, entitlement_consumes, deposit_orders, gacha_reward_logs.

**Format:** Khuyến nghị UUID hoặc `{domain}_{id}` (vd: `escrow_freeze_{question_item_id}`).

---

## 4.1 Escrow status flow (C6 fix)

**Trạng thái `escrow_status`:** `pending` → `accepted` → `released` | `refunded` | `disputed`

- **pending:** Câu hỏi đã tạo, chờ reader accept. Offer có thể hết hạn (`offer_expires_at`).
- **accepted:** Reader đã accept, Diamond đã freeze. Timer `reader_response_due_at` bắt đầu.
- **released:** Diamond đã giải phóng cho reader.
- **refunded:** Diamond đã trả lại payer.
- **disputed:** Đang tranh chấp, chờ admin resolve.

**Lưu ý:** Index `idx_chat_question_timers` filter `WHERE status = 'accepted'` thay vì `pending` để chỉ track items đã accept chờ reply.

---

## 5. Cross-DB references (PostgreSQL ↔ MongoDB)

**Format chuẩn:**
- **MongoDB ObjectId:** 24 hex chars, CHECK `~ '^[0-9a-f]{24}$'`. Dùng cho: `conversation_ref`, `reading_session_ref`, `proposal_message_ref`.
- **PostgreSQL UUID:** 36 chars với dấu gạch. Dùng cho: `finance_session_ref` (trong MongoDB conversations).

**Lưu ý:** Code phải parse đúng format khi join/lookup. Không nhầm ObjectId với UUID.

---

## 6. Partitioning (wallet_transactions)

**Khi nào:** DAU Tier M (~50k) hoặc L (~100k+). BR-4.17.

**Cách:** `RANGE PARTITION BY (created_at)` theo tháng. Giữ partition gần hot, partition cũ có thể archive/cold storage.

**Hiện tại:** Chưa partition. Thêm comment trong schema để chuẩn bị.

---

## 7. Isolation level cho finance

**Yêu cầu (ARCH-4.6.4):** `SERIALIZABLE` hoặc `SELECT ... FOR UPDATE` + distributed lock Redis cho các command: freeze, add_freeze, release, refund.

**Stored procedures:** `proc_wallet_credit`/`proc_wallet_debit` dùng `SELECT ... FOR UPDATE` trên user row. Gọi từ transaction với isolation phù hợp.

---

## 7.1 Frozen balance audit trail

**Vấn đề:** `proc_wallet_release` giảm `frozen_diamond_balance` của payer nhưng **không ghi ledger row cho payer** – vì `diamond_balance` payer không đổi, nếu ghi ledger với `amount = -X` sẽ vi phạm `chk_wallet_balance_consistency` (`balance_after = balance_before + amount`).

**Giải pháp:**
- **Freeze:** Ghi ledger row payer (type=`escrow_freeze`, amount=-X) → `diamond_balance` giảm, `frozen_diamond_balance` tăng.
- **Release:** Ghi ledger row **receiver** (type=`escrow_release`, amount=+X) → `diamond_balance` receiver tăng. Payer frozen giảm nhưng không ghi ledger row.
- **Refund:** Ghi ledger row payer (type=`escrow_refund`, amount=+X) → `diamond_balance` tăng, `frozen_diamond_balance` giảm.

> **C3 fix – Ngoại lệ chính thức cho double-entry:** Việc `proc_wallet_release` không ghi ledger row cho payer là **quyết định có chủ đích**, không phải lỗi thiếu. Lý do: `diamond_balance` payer không đổi khi release (chỉ `frozen_diamond_balance` giảm), nên ghi ledger row sẽ vi phạm constraint `chk_wallet_balance_consistency`. **BR-4.3.1 ("mọi biến động số dư phải có dòng sổ cái") áp dụng cho `diamond_balance`/`gold_balance` nhưng không áp cho `frozen_diamond_balance`.** Frozen balance được audit qua `v_user_frozen_ledger_balance` view – đây là source of truth cho frozen audit.

**Reconciliation frozen balance:** View `v_user_frozen_ledger_balance` tính:
- `SUM(freeze amounts)` từ `wallet_transactions` (payer)
- `-SUM(refund amounts)` từ `wallet_transactions` (payer)
- `-SUM(released amounts)` từ `chat_question_items` (payer_id, status='released')

So sánh kết quả với `users.frozen_diamond_balance`. Alert nếu mismatch.

---

## 8. Retention & compliance

| Dữ liệu | Retention |
|---------|-----------|
| RNG audit (reading_rng_audits) | ≥ 24 tháng (dispute evidence) |
| AI prompt metadata (ai_provider_logs) | 90 ngày TTL | L4 fix: đổi tên từ grok_logs |
| Notifications | 30 ngày TTL |
| Gacha logs | 180 ngày TTL |
| Dispute/chargeback evidence | 24 tháng |

**RNG secret rotation:** Secret cũ giữ ≥ 24 tháng để replay tranh chấp.

**reading_rng_audits:** Không dùng TTL (evidence tranh chấp). Cần scheduled job purge records > 24 tháng nếu chính sách cho phép archive.

---

## 8.1 Deposit Orders – Không soft delete (có chủ đích)

**Vấn đề:** `deposit_orders` không có `is_deleted/deleted_at` (khác `deposit_promotions` có soft delete).

**Lý do:** Deposit orders là dữ liệu tài chính critical, phải giữ nguyên vẹn cho đối soát, dispute evidence, và audit trail. Không cho phép soft delete hay hard delete.

---

## 9. MongoDB

**reading_chains:** Unique `(host_user_id, guest_user_id, business_date)` – cho phép mời lại cùng cặp host-guest vào ngày khác (L3 fix). Mỗi business date chỉ 1 chain per host-guest pair.

**reading_sessions:** Schema validator – `spread_type` enum, `drawn_cards` maxItems 10. validationAction: warn (không chặn insert cũ).

**Sharding (Tier L):** `chat_messages` shard key `{conversation_id: "hashed"}`. Cần `enableSharding` trước.

**TTL:** notifications 30d, ai_provider_logs 90d, gacha_logs 180d. Không TTL cho chat_messages (retention theo policy).

---

## 10. transaction_type ENUM

**Mở rộng:** PostgreSQL 10+ hỗ trợ `ALTER TYPE transaction_type ADD VALUE 'new_type'` không cần rewrite bảng. Nếu thêm type mới thường xuyên (Phase 4+), cân nhắc bảng `transaction_types` thay ENUM.

---

## 11. Ánh xạ trạng thái AI (MongoDB ↔ PostgreSQL)

**MongoDB** (`reading_sessions.ai_status`): `pending` \| `streaming` \| `completed` \| `timeout` \| `failed`

**PostgreSQL** (`ai_requests.status`): `requested` \| `first_token_received` \| `completed` \| `failed_before_first_token` \| `failed_after_first_token`

| MongoDB | PostgreSQL (tương đương) |
|---------|---------------------------|
| pending | requested |
| streaming | first_token_received |
| completed | completed |
| timeout | failed_before_first_token hoặc failed_after_first_token (tùy thời điểm) |
| failed | failed_before_first_token hoặc failed_after_first_token |

**Lưu ý:** Khi trace log cross-DB (vd: reading_session_ref ↔ ai_requests), map theo bảng trên để tránh nhầm lẫn.

---

## 12. Data rights (OPS-4.13.7)

**Bảng `data_rights_requests`:** Lưu yêu cầu truy cập/xuất (access_export), chỉnh sửa (correction), xóa (deletion) theo GDPR/CCPA-like. Mỗi request có status, sla_deadline_at, result_summary. Audit trail qua completed_by, completed_at.

---

## 13. Share reward anti-abuse (M3 note)

**Vấn đề:** BR-4.7.1 yêu cầu heuristic anti-abuse cho share reward (device fingerprint, IP velocity, cooldown). Hiện chưa có bảng/collection riêng lưu share claims.

**Giải pháp đề xuất:**
- Dùng `wallet_transactions` với `type = 'share_reward'` + `metadata_json` chứa `{device_fingerprint, ip, platform, share_url}` làm audit trail.
- Velocity check: query `wallet_transactions WHERE type='share_reward' AND user_id=? AND created_at > ? GROUP BY ...`.
- Nếu cần pipeline phức tạp hơn (Phase 5+), tạo collection MongoDB `share_claims` riêng.

---

## 14. Daily AI quota tracking (M4 note)

**Vấn đề:** ARCH-4.4.3 yêu cầu daily quota tracking per user per tier. Không có bảng riêng đếm quota ngày.

**Giải pháp được chọn:**
- **Redis counter:** Key format `ai_quota:{user_id}:{business_date_utc}`, TTL 48h. Atomic increment (`INCR`), compare với giới hạn từ `system_configs` hoặc subscription entitlement.
- **Rollback khi AI fail:** `DECR` counter (atomic). Nếu Redis unavailable, fallback query `ai_requests WHERE user_id=? AND DATE(created_at)=? AND status NOT IN ('failed_before_first_token')`.
- **Đối soát:** Reconciliation job so sánh Redis counter với `COUNT(ai_requests)` theo ngày.

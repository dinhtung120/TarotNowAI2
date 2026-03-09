# Phase 2.3 – Escrow Test Checklist

---

## 1. Freeze
- [ ] Accept → `proc_wallet_freeze`, frozen_balance tăng, diamond_balance giảm
- [ ] `chat_finance_sessions` + `chat_question_items` row tạo

## 2. Timers
- [ ] Offer timeout → auto-cancel (+ refund nếu đã freeze)
- [ ] No-reply 24h → auto-refund
- [ ] Auto-release (replied + no dispute 24h)

## 3. Release
- [ ] User confirm → reader nhận Diamond, fee 10% khấu trừ
- [ ] Ledger entries cho cả payer và receiver

## 4. Dispute
- [ ] Dispute trong window → status=disputed
- [ ] Dispute ngoài window → reject

## 5. Add-question
- [ ] Tạo thêm `chat_question_items`, freeze thêm Diamond

## 6. Idempotency
- [ ] Double-freeze → chặn
- [ ] Double-release → chặn
- [ ] Double-refund → chặn

## 7. Reconciliation
```sql
SELECT u.id, u.frozen_diamond_balance, v.ledger_frozen
FROM users u LEFT JOIN v_user_frozen_ledger_balance v ON u.id = v.user_id
WHERE u.frozen_diamond_balance != v.ledger_frozen;
```
- [ ] Trả **0 rows**

## Tổng kết: **13 test cases**

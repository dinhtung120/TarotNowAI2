# Phase 1.2 – Wallet Test Checklist

---

## 1. Balance API
```bash
curl -s http://localhost:5000/api/v1/wallet/balance -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Trả: `gold_balance`, `diamond_balance`, `frozen_diamond_balance`
- [ ] Giá trị khớp DB

## 2. Ledger API
```bash
curl -s "http://localhost:5000/api/v1/wallet/ledger?page=1&limit=20" -H "Authorization: Bearer <token>"
```
- [ ] Paging hoạt động
- [ ] Có dòng register bonus +5 Gold
- [ ] Thứ tự: created_at DESC

## 3. Ledger Invariant
```sql
SELECT u.id, u.gold_balance, v.ledger_gold FROM users u
LEFT JOIN v_user_ledger_balance v ON u.id = v.user_id
WHERE u.gold_balance != v.ledger_gold OR u.diamond_balance != v.ledger_diamond;
```
- [ ] Trả **0 rows**

## 4. Guard proc_wallet_*
- [ ] Mọi credit/debit đi qua `proc_wallet_*` (code review)
- [ ] Không có raw UPDATE balance trong codebase

## 5. Reconciliation
- [ ] Job chạy theo schedule
- [ ] Alert khi phát hiện mismatch
- [ ] Test: tạo mismatch → cảnh báo hoạt động

---

## Tổng kết: **8 test cases**

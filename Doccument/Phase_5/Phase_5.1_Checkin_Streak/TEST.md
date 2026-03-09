# Phase 5.1 – Check-in + Streak Test

---

- [ ] Check-in → Gold credit + ledger, idempotent (lần 2 reject)
- [ ] Streak +1 khi rút bài hợp lệ (1/ngày)
- [ ] Không rút → streak reset, pre_break lưu
- [ ] EXP multiplier: streak 10 → ×1.10
- [ ] Freeze price: pre_break=7 → 1D, pre_break=11 → 2D, pre_break=0 → không mua
- [ ] Mua freeze: debit + restore, idempotent, ngoài 24h → reject

## Tổng kết: **6 test cases**

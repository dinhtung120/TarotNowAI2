# Phase 1.5 – Follow-up + History Test Checklist

---

## 1. Free Slots
- [ ] Level cao nhất >= 6 → 1 free slot
- [ ] Level >= 11 → 2 free slots
- [ ] Level >= 16 → 3 free slots
- [ ] Level < 6 → 0 free slots
- [ ] Free slot KHÔNG charge Diamond, VẪN trừ AI quota

## 2. Paid Tiers
- [ ] Free dùng trước → paid bắt đầu tier tiếp
- [ ] Giá progression: [1, 2, 4, 8, 16] Diamond

## 3. Hard Cap 5
- [ ] Follow-up lần 6 → reject (followup_cap_reached)

## 4. UX Copy
- [ ] Free slot: hiện "Miễn phí Diamond, vẫn tiêu tốn AI quota ngày"

## 5. History
```bash
curl -s "http://localhost:5000/api/v1/readings?page=1" -H "Authorization: Bearer <token>"
curl -s "http://localhost:5000/api/v1/readings/<id>" -H "Authorization: Bearer <token>"
```
- [ ] List: paging, có spread_type, created_at
- [ ] Detail: drawn_cards, question, AI interpretation, followups
- [ ] Chỉ trả data của chính user

---

## Tổng kết: **10 test cases**

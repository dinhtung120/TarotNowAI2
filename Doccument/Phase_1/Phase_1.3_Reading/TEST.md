# Phase 1.3 – Reading Test Checklist

---

## 1. RNG
```sql
SELECT * FROM reading_rng_audits ORDER BY created_at DESC LIMIT 1;
```
- [ ] Có: `algorithm_version`, `secret_version`, `session_nonce`, `seed_digest`, `deck_order_hash`
- [ ] **Không** lưu `server_secret`
- [ ] Replay deterministic: cùng input → cùng deck order 100%
- [ ] Không trùng lá trong 1 phiên
- [ ] Retention >= 24 tháng (không TTL)

## 2. Daily 1 Card
```bash
curl -s -X POST http://localhost:5000/api/v1/readings \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"spreadType":"daily_1"}' | python3 -m json.tool
```
- [ ] Tạo reading_sessions, drawn_cards có 1 lá
- [ ] Lần 2 cùng ngày UTC → reject (daily_limit_reached)

## 3. Spread 3/5/10 + Charge
```bash
curl -s -X POST http://localhost:5000/api/v1/readings \
  -d '{"spreadType":"spread_3","question":"Tình yêu?"}' \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json"
```
- [ ] drawn_cards đúng số lượng (3/5/10)
- [ ] Không trùng card_id
- [ ] Balance trừ đúng giá + ledger entry
- [ ] Thiếu balance → error + CTA nạp

## 4. Card Collection
```javascript
db.user_collections.find({user_id: "<user_id>"});
```
- [ ] Lá rút có entry: total_draws tăng, exp tăng, last_drawn_at cập nhật
- [ ] Level tính đúng theo card_exp_levels

---

## Tổng kết: **12 test cases**

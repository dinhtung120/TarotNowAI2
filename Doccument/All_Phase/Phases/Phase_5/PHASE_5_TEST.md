# Phase 5+ – Test / Verification Checklist

**Mục đích:** Kiểm tra tất cả các bước trong Phase 5+ (Retention, Monetization, Gamification, Hardening, Expansion) đã hoàn thành đúng chưa.  
**Cách dùng:** Chạy từng test case, đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Daily Check-in (P5-CHECKIN)

### Test 1.1 – Check-in endpoint
```bash
curl -s -X POST http://localhost:5000/api/v1/checkin \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Tạo `daily_checkins` document trong MongoDB
- [ ] Unique per user per business_date
- [ ] Credit Gold via `proc_wallet_credit` (mặc định 1 Gold)
- [ ] Ledger entry tồn tại

### Test 1.2 – Idempotent check-in
```bash
# Gọi check-in lần 2 cùng ngày
```
- [ ] Lần 2 bị reject hoặc trả result trùng (không credit thêm)
- [ ] Chỉ 1 dòng ledger per ngày

---

## 2. Streak (P5-STREAK)

### Test 2.1 – Streak increment
```sql
SELECT current_streak, last_streak_date, pre_break_streak FROM users WHERE id = '<user_id>';
```
- [ ] Rút bài hợp lệ → `current_streak` +1
- [ ] `last_streak_date` = business_date hôm nay
- [ ] Chỉ tăng 1 lần per ngày (dù rút bài nhiều lần)

### Test 2.2 – Streak break
- [ ] Không rút bài 1 ngày → `current_streak` reset về 0
- [ ] `pre_break_streak` lưu giá trị trước khi gãy

### Test 2.3 – EXP multiplier
```
Streak 10 ngày → EXP_final = EXP_base × (1 + 10/100) = EXP_base × 1.10
```
- [ ] EXP nhận khi rút bài tăng đúng theo streak %
- [ ] EXP cuối cùng lưu trong user_collections

---

## 3. Streak Freeze (P5-FREEZE)

### Test 3.1 – Freeze price calculation
```
pre_break_streak = 7  → price = ceil(7/10) = 1 Diamond
pre_break_streak = 11 → price = ceil(11/10) = 2 Diamond
pre_break_streak = 0  → không hiển thị / không cho mua
```
- [ ] Giá tính đúng công thức `ceil(pre_break/10)`
- [ ] `pre_break_streak = 0` → không cho mua

### Test 3.2 – Purchase freeze
```bash
curl -s -X POST http://localhost:5000/api/v1/streak/freeze \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Diamond debit đúng giá (via `proc_wallet_debit`)
- [ ] Streak restored = pre_break_streak
- [ ] Ledger entry cho Diamond charge
- [ ] Idempotent (mua lần 2 cùng window → reject)

### Test 3.3 – Freeze window (24h)
- [ ] Ngoài 24h sau ngày gãy → không cho mua freeze
- [ ] Trong 24h → OK

---

## 4. Notifications (P5-NOTIF)

### Test 4.1 – Notification created
```javascript
db.notifications.find({user_id: "<user_id>"}).sort({created_at: -1}).limit(5);
```
- [ ] Daily habit notifications được schedule
- [ ] Win-back triggers (3/7/14 ngày im lặng)
- [ ] Có: title, body, type, action_url

### Test 4.2 – API list/mark-read/delete
```bash
curl -s "http://localhost:5000/api/v1/notifications?page=1" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] List hoạt động + paging
- [ ] Mark read → `read_at` set
- [ ] Delete → xóa document

### Test 4.3 – TTL 30 ngày
```javascript
db.notifications.getIndexes(); // TTL 2592000s
```
- [ ] Notifications tự xóa sau 30 ngày

---

## 5. Subscription & Entitlement (P5-SUB, P5-ENT)

### Test 5.1 – Multiple subscriptions active
```sql
SELECT * FROM user_subscriptions WHERE user_id = '<user_id>' AND status = 'active';
SELECT * FROM subscription_entitlement_buckets WHERE user_subscription_id IN (...);
```
- [ ] User có thể có nhiều subscription active
- [ ] Mỗi subscription có entitlement buckets riêng
- [ ] Entitlements reset theo UTC business date

### Test 5.2 – Earliest-expiry-first consume
```sql
-- User có 2 subscriptions với cùng entitlement key, expiry khác nhau
SELECT * FROM entitlement_consumes WHERE user_id = '<user_id>' ORDER BY created_at DESC;
```
- [ ] Consume từ bucket hết hạn sớm hơn
- [ ] Tie-break: subscription_id ASC (deterministic)
- [ ] Không double-consume

### Test 5.3 – Concurrent consume (no double-consume)
```bash
# Gửi 5 requests đồng thời cùng user cùng entitlement key
```
- [ ] Chỉ consume đúng số lượng available
- [ ] Không vượt quá quota
- [ ] Row locking hoạt động

### Test 5.4 – Cross-key mapping
```sql
SELECT * FROM entitlement_mapping_rules WHERE is_active = true;
```
- [ ] Mapping OFF (mặc định) → không auto cross-key
- [ ] Mapping ON + rule explicit → consume theo rule
- [ ] Usage log ghi `mapping_rule_id`

### Test 5.5 – Daily reset
- [ ] Quota reset đúng tại UTC midnight
- [ ] Entitlements từ expired subscriptions không còn available

---

## 6. Event Packs (P5-EVENT)

### Test 6.1 – Purchase within window
```bash
curl -s -X POST http://localhost:5000/api/v1/event-packs/<id>/purchase \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Chỉ mua được trong event window
- [ ] Ngoài window → reject
- [ ] Idempotent purchase

### Test 6.2 – Benefits expiry
- [ ] Benefits (bonus Diamond, EXP multiplier) hết hạn đúng
- [ ] Không ghi đè base entitlements

---

## 7. Gamification – Quests (P5-QUEST)

### Test 7.1 – Quest progress tracking
```javascript
db.quest_progress.find({user_id: "<user_id>", quest_code: "daily_draw"});
```
- [ ] Progress tăng khi hoàn thành action
- [ ] Unique per (user_id, quest_code, period_key)

### Test 7.2 – Claim rewards idempotent
```bash
curl -s -X POST http://localhost:5000/api/v1/quests/<quest_code>/claim \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Claim first → credit rewards + ledger
- [ ] Claim second → reject (đã claim)

---

## 8. Gamification – Achievements & Titles (P5-ACHV)

### Test 8.1 – Achievement unlock
```javascript
db.user_achievements.find({user_id: "<user_id>"});
```
- [ ] One-time unlock per user per achievement
- [ ] Unlock hook trigger tự động

### Test 8.2 – Title selection
```bash
curl -s -X POST http://localhost:5000/api/v1/titles/<title_id>/activate \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Active title thay đổi
- [ ] Profile hiển thị active title
- [ ] Chỉ select owned titles

---

## 9. Gamification – Leaderboard (P5-LB)

### Test 9.1 – Score tracks
```javascript
db.leaderboard_snapshots.find({period_type: "daily", period_key: "2026-03-08"});
```
- [ ] 3 tracks hoạt động: daily_rank_score, monthly_rank_score, lifetime_score
- [ ] Snapshot unique per period_key

### Test 9.2 – Recalculation job
- [ ] Job chạy đúng schedule
- [ ] Kết quả auditable
- [ ] Idempotent (chạy lại không duplicate)

---

## 10. Rate Limiting (P5-RL)

### Test 10.1 – Auth rate limit
```bash
# Brute force login
for i in {1..20}; do
  curl -s -X POST http://localhost:5000/api/v1/auth/login \
    -d '{"login":"test@test.com","password":"wrong"}' \
    -H "Content-Type: application/json" | head -1
done
```
- [ ] Sau N attempts → 429 Too Many Requests
- [ ] Escalation policy (tăng block time)

### Test 10.2 – AI rate limit
- [ ] In-flight cap 2 per user
- [ ] Per-endpoint rate limit hoạt động

### Test 10.3 – Payment rate limit
- [ ] Deposit/withdrawal rate limits hoạt động
- [ ] Error response rõ ràng

---

## 11. Share & Referral (P5-SHARE, P5-REF)

### Test 11.1 – Share reward
```bash
curl -s -X POST http://localhost:5000/api/v1/share/claim \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"readingId":"<id>","network":"facebook"}' | python3 -m json.tool
```
- [ ] First share/ngày/mạng → +2 Gold
- [ ] Second share cùng mạng cùng ngày → reject
- [ ] Link signed + TTL

### Test 11.2 – Referral
```sql
SELECT * FROM users WHERE referred_by_id IS NOT NULL;
```
- [ ] Referral code generated on register
- [ ] Track referral signup
- [ ] Credit rewards on milestones (idempotent)
- [ ] Self-referral prevention

---

## 12. Gacha (P5-GACHA)

### Test 12.1 – Gacha spin
```bash
curl -s -X POST http://localhost:5000/api/v1/gacha/spin \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Debit 5 Diamond (configurable)
- [ ] RNG weighted selection hoạt động
- [ ] Reward credited
- [ ] Idempotent (double-spin → chỉ 1 charge)

### Test 12.2 – Odds disclosure
```bash
curl -s "http://localhost:5000/api/v1/gacha/odds" | python3 -m json.tool
```
- [ ] Odds public API trả version + probabilities
- [ ] Tổng probabilities = 100%

### Test 12.3 – Pity timer (nếu bật)
- [ ] Sau N spins không ra high rarity → xác suất tăng
- [ ] Pity reset sau khi hit

### Test 12.4 – Logs retention
```sql
SELECT * FROM gacha_reward_logs ORDER BY created_at DESC LIMIT 5;
```
```javascript
db.gacha_logs.find().sort({created_at: -1}).limit(5);
```
- [ ] PG: reward_logs lưu (idempotency_key, spent_diamond)
- [ ] Mongo: gacha_logs lưu (TTL 180 ngày)

---

## 13. GDPR / Data Rights (P5-GDPR)

### Test 13.1 – Submit data rights request
```bash
curl -s -X POST http://localhost:5000/api/v1/data-rights \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"type":"access_export"}' | python3 -m json.tool
```
- [ ] Tạo `data_rights_requests` row
- [ ] Status = pending

### Test 13.2 – Export data
- [ ] Export job aggregate PG + Mongo
- [ ] Output: machine-readable + human summary
- [ ] Chứa tất cả user data

### Test 13.3 – Delete data
- [ ] Deletion: soft-delete + purge PII
- [ ] **Giữ ledger** (financial records không xóa)
- [ ] Response liệt kê: data đã xóa + data giữ lại + lý do

---

## 14. Geo Compliance (P5-GEO)

### Test 14.1 – Geo feature gating
```sql
SELECT * FROM user_geo_signals WHERE user_id = '<user_id>';
```
- [ ] Gacha/RNG monetization features bị gate theo country
- [ ] Server-side API enforcement (không chỉ UI hide)
- [ ] Multi-signal decision hoạt động

### Test 14.2 – Restricted review
- [ ] Tín hiệu mâu thuẫn / VPN → `restricted_review`
- [ ] Auto-resolve <= 24h nếu tín hiệu mới OK
- [ ] Quá SLA → escalate manual review

---

## 15. Friend Chain (P5-CHAIN)

### Test 15.1 – Chain invite + join
```bash
curl -s -X POST http://localhost:5000/api/v1/friend-chain/join \
  -H "Authorization: Bearer <guest_token>" -H "Content-Type: application/json" \
  -d '{"chainToken":"<token>"}' | python3 -m json.tool
```
- [ ] Tạo `reading_chains` document
- [ ] Unique: (host_user_id, guest_user_id, business_date)
- [ ] Reward cả host + guest (3 Gold each)
- [ ] Daily cap: 3 lần/ngày

### Test 15.2 – Idempotent + cap
- [ ] Join lần 2 cùng cặp cùng ngày → reject
- [ ] Lần 4+ cùng ngày → reject (cap 3)

---

## 16. Card Stories / Ascension (P5-STORY)

### Test 16.1 – Level-up trigger
```javascript
db.card_stories.find({user_id: "<user_id>"});
```
- [ ] Card level up đến milestone (6/11/16/20) → trigger story
- [ ] AI generate story
- [ ] Persist trong `card_stories` collection
- [ ] Unique: (user_id, card_id, level_trigger)

---

## 17. Observability (P5-OTEL)

- [ ] OpenTelemetry traces: API → DB → AI provider
- [ ] Structured logs với trace IDs
- [ ] `idempotency_key` + `correlation_id` searchable cho finance transitions
- [ ] Dashboard: latency, error rates, AI costs

---

## Tổng kết Phase 5+ Test

| # | Nhóm | Số test case | Kết quả |
|---|---|---:|---|
| 1 | Daily Check-in | 2 | |
| 2 | Streak | 3 | |
| 3 | Streak Freeze | 3 | |
| 4 | Notifications | 3 | |
| 5 | Subscription | 5 | |
| 6 | Event Packs | 2 | |
| 7 | Quests | 2 | |
| 8 | Achievements | 2 | |
| 9 | Leaderboard | 2 | |
| 10 | Rate Limiting | 3 | |
| 11 | Share & Referral | 2 | |
| 12 | Gacha | 4 | |
| 13 | GDPR | 3 | |
| 14 | Geo Compliance | 2 | |
| 15 | Friend Chain | 2 | |
| 16 | Card Stories | 1 | |
| 17 | Observability | 1 | |
| **Tổng** | | **42** | |

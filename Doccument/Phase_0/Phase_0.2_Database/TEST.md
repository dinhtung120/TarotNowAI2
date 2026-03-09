# Phase 0.2 – Database Test Checklist

---

## 1. PostgreSQL Schema
```bash
psql -h localhost -U postgres -d tarotweb -f database/postgresql/schema.sql
```
- [ ] Không lỗi SQL syntax, tất cả ENUMs tạo thành công

```sql
SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';
```
- [ ] Có đủ **25+ bảng**

### Stored Procedures
```sql
SELECT proname FROM pg_proc WHERE proname LIKE 'proc_wallet_%';
```
- [ ] `proc_wallet_credit`, `proc_wallet_debit`, `proc_wallet_freeze`, `proc_wallet_release`, `proc_wallet_refund` tồn tại

### Views
```sql
SELECT viewname FROM pg_views WHERE schemaname = 'public';
```
- [ ] `v_user_ledger_balance` + `v_user_frozen_ledger_balance` tồn tại

### Triggers
```sql
SELECT trigger_name FROM information_schema.triggers WHERE trigger_schema = 'public';
```
- [ ] Trigger `updated_at` cho các bảng chính

### System Accounts
```sql
SELECT id, email, role FROM users WHERE role = 'system';
```
- [ ] `00000000-...-000000000001` (system_platform) tồn tại
- [ ] `00000000-...-000000000002` (system_escrow) tồn tại

### CHECK Constraints
```sql
INSERT INTO users (email, username, password_hash, display_name, date_of_birth, gold_balance)
VALUES ('test_neg@test.com', 'test_neg', 'hash', 'Test', '2000-01-01', -1);
```
- [ ] Balance âm bị chặn bởi CHECK constraint

---

## 2. MongoDB Init
```bash
mongosh mongodb://localhost:27017/tarotweb < database/mongodb/init.js
```
- [ ] Không lỗi, output "init completed"

```javascript
db.getCollectionNames().sort();
```
- [ ] Có đủ **29 collections**

### Unique Indexes
```javascript
db.cards_catalog.getIndexes();
db.reading_chains.getIndexes();
```
- [ ] `cards_catalog` unique trên `code`
- [ ] `reading_chains` unique trên `(host_user_id, guest_user_id, business_date)`

### TTL Indexes
- [ ] `notifications` TTL 30 ngày (2592000s)
- [ ] `ai_provider_logs` TTL 90 ngày (7776000s)
- [ ] `gacha_logs` TTL 180 ngày (15552000s)

### Schema Validators
- [ ] `reading_sessions` validator: spread_type enum, drawn_cards maxItems 10
- [ ] `chat_messages` validator: type enum
- [ ] `conversations`, `reader_profiles`, `call_sessions` validators

---

## 3. Seed: cards_catalog
```javascript
db.cards_catalog.countDocuments();
```
- [ ] Đúng 78 documents
- [ ] Tra cứu theo `_id` (1-78) + `code` hoạt động
- [ ] Mỗi lá có: name.vi, name.en, name.zh, arcana, suit, meanings
- [ ] Insert duplicate code → fail (unique index)

## 4. Seed: system_configs
```sql
SELECT key, value FROM system_configs ORDER BY key;
```
- [ ] Có đủ configs: diamond_vnd_rate, daily_checkin_gold, register_bonus_gold, platform_fee_percent, ai_daily_quota_free/premium, ...
- [ ] Chạy schema.sql lần 2 → không lỗi (ON CONFLICT DO NOTHING)

## 5. Seed: EXP levels
```sql
SELECT * FROM user_exp_levels ORDER BY level;
SELECT * FROM card_exp_levels ORDER BY level;
```
- [ ] Có dữ liệu, level + min_exp tăng dần

---

## Tổng kết: **24 test cases**

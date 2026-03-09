# Phase 0 – Test / Verification Checklist

**Mục đích:** Kiểm tra tất cả các bước trong Phase 0 đã hoàn thành đúng chưa.  
**Cách dùng:** Chạy từng test case, đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Repo & Naming Conventions (P0-REPO-0.1)

### Test 1.1 – Cấu trúc thư mục tồn tại đúng
```bash
# Kiểm tra các thư mục chính tồn tại
ls -d src/          # Source code chính
ls -d docs/         # Tài liệu
ls -d database/     # Database schemas
ls -d tests/        # Test projects (nếu tách riêng)
```
- [ ] Thư mục gốc có cấu trúc rõ ràng, tách biệt BE/FE/DB/docs
- [ ] Naming conventions nhất quán (kebab-case hoặc PascalCase theo quy ước)
- [ ] Không có thư mục/file thừa, rác

### Test 1.2 – `.gitignore` đúng
```bash
cat .gitignore
```
- [ ] Có ignore: `node_modules/`, `bin/`, `obj/`, `.env`, `.env.local`
- [ ] Có ignore: `*.log`, `.DS_Store`, `.vs/`, `.idea/`
- [ ] KHÔNG ignore: `.env.example`, `appsettings.json` (template)

---

## 2. Config Files (P0-REPO-0.2)

### Test 2.1 – `.env.example` tồn tại + đầy đủ key
```bash
cat .env.example
```
- [ ] File tồn tại
- [ ] Có key cho Database: `POSTGRES_HOST`, `POSTGRES_PORT`, `POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD`
- [ ] Có key cho MongoDB: `MONGO_URI` hoặc `MONGO_HOST/PORT/DB`
- [ ] Có key cho Redis: `REDIS_URL` hoặc `REDIS_HOST/PORT`
- [ ] Có key cho AI Provider: `AI_API_KEY`, `AI_MODEL` hoặc tương tự
- [ ] Có key cho Payment: `PAYMENT_WEBHOOK_SECRET` hoặc tương tự
- [ ] Có key cho JWT: `JWT_SECRET`, `JWT_ISSUER`, `JWT_AUDIENCE` hoặc tương tự
- [ ] Tất cả giá trị mẫu KHÔNG phải secret thật (dùng placeholder)

### Test 2.2 – appsettings skeleton (.NET)
```bash
cat src/api/appsettings.json          # hoặc đường dẫn BE project
cat src/api/appsettings.Development.json
```
- [ ] File tồn tại
- [ ] Có section: `ConnectionStrings`, `Jwt`, `Redis` (hoặc tương đương)
- [ ] Development config trỏ localhost, không hardcode production values

---

## 3. Secrets Strategy (P0-REPO-0.3)

### Test 3.1 – Không có secret trong repo
```bash
# Tìm các pattern nguy hiểm trong source code
grep -rn "password\s*=" --include="*.json" --include="*.yaml" --include="*.yml" . | grep -v "example" | grep -v "template" | grep -v "node_modules"
grep -rn "sk-" --include="*.ts" --include="*.cs" --include="*.json" . | grep -v "node_modules"  # API keys pattern
grep -rn "PRIVATE.KEY" --include="*.ts" --include="*.cs" . | grep -v "node_modules"
```
- [ ] Không có secret/password/key thật trong source code
- [ ] Không có file `.env` (chỉ có `.env.example`)
- [ ] Có tài liệu mô tả cách inject secrets (README hoặc docs riêng)

---

## 4. CI Build API – .NET (P0-CICD-0.1)

### Test 4.1 – Build .NET thành công
```bash
cd src/api  # hoặc đường dẫn BE project
dotnet restore
dotnet build --no-restore
```
- [ ] `dotnet restore` không lỗi
- [ ] `dotnet build` compile thành công, không warning critical

### Test 4.2 – CI pipeline file tồn tại
```bash
ls .github/workflows/*.yml  # GitHub Actions
# hoặc
ls .gitlab-ci.yml           # GitLab CI
# hoặc
ls azure-pipelines.yml      # Azure DevOps
```
- [ ] File CI tồn tại
- [ ] Có step build backend
- [ ] Fail-fast: compile error sẽ khiến pipeline fail

---

## 5. CI Build Web – Next.js (P0-CICD-0.2)

### Test 5.1 – Build Next.js thành công
```bash
cd src/web  # hoặc đường dẫn FE project
npm install
npm run build
```
- [ ] `npm install` không lỗi
- [ ] `npm run build` thành công
- [ ] Không có TypeScript errors

### Test 5.2 – CI pipeline bao gồm frontend
- [ ] File CI có step build frontend
- [ ] Có step typecheck (`npx tsc --noEmit` hoặc tương đương)

---

## 6. CI Unit Tests (P0-CICD-0.3)

### Test 6.1 – Unit test chạy được
```bash
# Backend
cd src/api
dotnet test --verbosity normal

# Frontend (nếu có)
cd src/web
npm run test -- --run  # hoặc npm test
```
- [ ] BE tests chạy được (0 test OK = vẫn pass, miễn không lỗi framework)
- [ ] FE tests chạy được (nếu đã setup)
- [ ] CI pipeline có step chạy test

---

## 7. CI Cache + Artifacts (P0-CICD-0.4)

### Test 7.1 – CI config có cache
- [ ] CI file cấu hình cache cho NuGet packages (`~/.nuget/packages`)
- [ ] CI file cấu hình cache cho npm (`~/.npm` hoặc `node_modules`)
- [ ] CI có upload build artifacts (nếu cần debug)

---

## 8. PostgreSQL Schema (P0-DB-0.1)

### Test 8.1 – Schema apply thành công
```bash
psql -h localhost -U postgres -d tarotweb -f database/postgresql/schema.sql
# hoặc dùng migration tool
```
- [ ] Không lỗi SQL syntax
- [ ] Tất cả ENUMs tạo thành công

### Test 8.2 – Kiểm tra bảng và constraints
```sql
-- Chạy trong psql hoặc tool SQL
-- Đếm số bảng
SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';

-- Kiểm tra bảng quan trọng
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'users');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'wallet_transactions');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'chat_finance_sessions');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'chat_question_items');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'ai_requests');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'deposit_orders');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'reading_rng_audits');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'refresh_tokens');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'subscription_plans');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'entitlement_mapping_rules');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'data_rights_requests');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'admin_actions');
SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'system_configs');
```
- [ ] Có đủ **25 bảng** (users, user_consents, email_otps, password_reset_tokens, refresh_tokens, deposit_promotions, deposit_orders, wallet_transactions, chat_finance_sessions, chat_question_items, withdrawal_requests, reader_payout_profiles, subscription_plans, user_subscriptions, subscription_entitlement_buckets, entitlement_consumes, ai_requests, reading_rng_audits, gacha_odds_versions, gacha_reward_logs, user_exp_levels, card_exp_levels, user_geo_signals, system_configs, entitlement_mapping_rules, data_rights_requests, admin_actions)

### Test 8.3 – Stored procedures tồn tại
```sql
SELECT proname FROM pg_proc WHERE proname LIKE 'proc_wallet_%';
```
- [ ] `proc_wallet_credit` tồn tại
- [ ] `proc_wallet_debit` tồn tại
- [ ] `proc_wallet_freeze` tồn tại
- [ ] `proc_wallet_release` tồn tại
- [ ] `proc_wallet_refund` tồn tại

### Test 8.4 – Views tồn tại
```sql
SELECT viewname FROM pg_views WHERE schemaname = 'public';
```
- [ ] `v_user_ledger_balance` tồn tại
- [ ] `v_user_frozen_ledger_balance` tồn tại

### Test 8.5 – Triggers tồn tại
```sql
SELECT trigger_name FROM information_schema.triggers WHERE trigger_schema = 'public';
```
- [ ] Có trigger `updated_at` cho: users, chat_finance_sessions, withdrawal_requests, deposit_orders, subscription_plans, user_subscriptions, subscription_entitlement_buckets, reader_payout_profiles, entitlement_mapping_rules, deposit_promotions, ai_requests, user_geo_signals, system_configs, data_rights_requests, chat_question_items

### Test 8.6 – System accounts đã seed
```sql
SELECT id, email, role FROM users WHERE role = 'system';
```
- [ ] `00000000-0000-0000-0000-000000000001` (system_platform) tồn tại
- [ ] `00000000-0000-0000-0000-000000000002` (system_escrow) tồn tại

### Test 8.7 – CHECK constraints hoạt động
```sql
-- Test balance không âm
INSERT INTO users (email, username, password_hash, display_name, date_of_birth, gold_balance)
VALUES ('test_neg@test.com', 'test_neg', 'hash', 'Test', '2000-01-01', -1);
-- Phải FAIL với CHECK constraint violation
```
- [ ] Insert balance âm bị chặn bởi CHECK constraint

---

## 9. MongoDB Init (P0-DB-0.2)

### Test 9.1 – Script chạy thành công
```bash
mongosh mongodb://localhost:27017/tarotweb < database/mongodb/init.js
```
- [ ] Không lỗi
- [ ] Output: "TarotWeb MongoDB init completed."

### Test 9.2 – Kiểm tra collections
```javascript
// Trong mongosh
use tarotweb;
db.getCollectionNames().sort();
```
- [ ] Có đủ **29 collections**: cards_catalog, card_stories, user_collections, reading_sessions, reader_profiles, reader_requests, conversations, chat_messages, reviews, reports, referrals, quests, quest_progress, achievements, user_achievements, titles, user_titles, reading_chains, events_config, notifications, daily_checkins, ai_provider_logs, admin_logs, gacha_logs, leaderboard_snapshots, community_posts, community_reactions, call_sessions, share_claims

### Test 9.3 – Kiểm tra unique indexes
```javascript
// Kiểm tra vài index quan trọng
db.cards_catalog.getIndexes();  // Phải có unique: code
db.reading_chains.getIndexes(); // Phải có unique: (host_user_id, guest_user_id, business_date)
db.quest_progress.getIndexes(); // Phải có unique: (user_id, quest_code, period_key)
```
- [ ] `cards_catalog` có unique index trên `code`
- [ ] `reading_chains` có unique index trên `(host_user_id, guest_user_id, business_date)`

### Test 9.4 – Kiểm tra TTL indexes
```javascript
db.notifications.getIndexes();     // TTL 30 ngày (2592000s)
db.ai_provider_logs.getIndexes();  // TTL 90 ngày (7776000s)
db.gacha_logs.getIndexes();        // TTL 180 ngày (15552000s)
```
- [ ] `notifications` có TTL index 2592000s (30 ngày)
- [ ] `ai_provider_logs` có TTL index 7776000s (90 ngày)
- [ ] `gacha_logs` có TTL index 15552000s (180 ngày)

### Test 9.5 – Kiểm tra schema validators
```javascript
db.getCollectionInfos({name: "reading_sessions"})[0].options.validator;
db.getCollectionInfos({name: "chat_messages"})[0].options.validator;
```
- [ ] `reading_sessions` có validator: spread_type enum, drawn_cards maxItems 10
- [ ] `chat_messages` có validator: type enum (text, system, card_share, etc.)
- [ ] `conversations` có validator: status enum
- [ ] `reader_profiles` có validator: status enum
- [ ] `call_sessions` có validator: status enum

---

## 10. Seed: cards_catalog (P0-DB-0.3)

### Test 10.1 – Đủ 78 lá bài
```javascript
db.cards_catalog.countDocuments();  // Phải = 78
```
- [ ] Đúng 78 documents

### Test 10.2 – Tra cứu theo _id và code
```javascript
db.cards_catalog.findOne({_id: 1});          // The Fool
db.cards_catalog.findOne({code: "the_fool"}); // Phải tìm được
db.cards_catalog.findOne({_id: 78});          // Lá cuối
```
- [ ] Tra cứu theo `_id` (Int32 1-78) hoạt động
- [ ] Tra cứu theo `code` hoạt động
- [ ] Mỗi lá có: name.vi, name.en, name.zh, arcana, suit, meanings

### Test 10.3 – Unique constraint
```javascript
// Insert duplicate code phải fail
db.cards_catalog.insertOne({_id: 999, code: "the_fool", name: {vi: "x", en: "x", zh: "x"}});
// Phải lỗi duplicate key
```
- [ ] Insert duplicate `code` bị chặn bởi unique index

---

## 11. Seed: system_configs (P0-DB-0.4)

### Test 11.1 – Configs tồn tại
```sql
SELECT key, value FROM system_configs ORDER BY key;
```
- [ ] `diamond_vnd_rate` = '1000'
- [ ] `daily_checkin_gold` = '1'
- [ ] `register_bonus_gold` = '5'
- [ ] `platform_fee_percent` = '10'
- [ ] `min_withdrawal_diamond` = '50'
- [ ] `ai_error_timeout_seconds` = '30'
- [ ] `ai_daily_quota_free` = '3'
- [ ] `ai_daily_quota_premium` = '30'
- [ ] `share_reward_gold` = '2'
- [ ] `ai_max_retry_per_request` = '1'
- [ ] `ai_timeout_before_token_seconds` = '30'
- [ ] `ai_in_flight_cap` = '2'
- [ ] `streak_freeze_window_hours` = '24'
- [ ] `friend_chain_reward_gold` = '3'
- [ ] `friend_chain_daily_cap` = '3'
- [ ] `gacha_cost_diamond` = '5'
- [ ] `offer_timeout_hours` = '12'

### Test 11.2 – Idempotent seed
```bash
# Chạy schema.sql lần 2
psql -h localhost -U postgres -d tarotweb -f database/postgresql/schema.sql
```
- [ ] Không lỗi duplicate (ON CONFLICT DO NOTHING)

---

## 12. Seed: EXP levels (P0-DB-0.5)

### Test 12.1 – user_exp_levels có dữ liệu
```sql
SELECT * FROM user_exp_levels ORDER BY level;
```
- [ ] Có dữ liệu, level tăng dần
- [ ] `min_exp` tăng dần theo level

### Test 12.2 – card_exp_levels có dữ liệu
```sql
SELECT * FROM card_exp_levels ORDER BY level;
```
- [ ] Có dữ liệu cho level 1–20 (hoặc hơn)
- [ ] `min_exp` tăng dần theo level

---

## 13. API Scaffold (P0-API-0.1)

### Test 13.1 – API chạy được
```bash
cd src/api
dotnet run &
sleep 5
curl -s http://localhost:5000/api/v1/health  # hoặc port configured
```
- [ ] Server khởi động không lỗi
- [ ] Health endpoint trả 200 OK

### Test 13.2 – Swagger hoạt động
```bash
curl -s http://localhost:5000/swagger/index.html | head -5
# hoặc mở browser http://localhost:5000/swagger
```
- [ ] Swagger UI truy cập được
- [ ] Hiển thị API versioning `/api/v1`

---

## 14. ProblemDetails Error Contract (P0-API-0.2)

### Test 14.1 – Error response format chuẩn
```bash
# Gọi endpoint không tồn tại
curl -s http://localhost:5000/api/v1/nonexistent | python3 -m json.tool
```
- [ ] Response trả JSON format ProblemDetails:
  ```json
  {
    "type": "...",
    "title": "...",
    "status": 404,
    "detail": "...",
    "instance": "..."
  }
  ```
- [ ] Có `Content-Type: application/problem+json`

---

## 15. Refresh Token Cookie (P0-AUTH-COOKIE-0.1)

### Test 15.1 – Cookie config tồn tại
- [ ] Có middleware/config cho httpOnly cookie
- [ ] Cookie flags: `HttpOnly=true`, `Secure=true`, `SameSite=Strict` (hoặc Lax)
- [ ] Có tài liệu/note CSRF nếu cần

---

## 16. Next.js Scaffold (P0-WEB-0.1)

### Test 16.1 – Dev server chạy được
```bash
cd src/web
npm run dev &
sleep 5
curl -s http://localhost:3000 | head -10
```
- [ ] Dev server khởi động không lỗi
- [ ] Trang chủ trả HTML

### Test 16.2 – Routing hoạt động
- [ ] Có layout chung (header/footer hoặc sidebar)
- [ ] Có ít nhất 1 route ngoài `/`

---

## 17. i18n Scaffold (P0-WEB-0.2)

### Test 17.1 – Locale files tồn tại
```bash
ls src/web/locales/         # hoặc messages/ hoặc i18n/
# hoặc
ls src/web/public/locales/
```
- [ ] Có file cho `vi` (tiếng Việt)
- [ ] Có file cho `en` (tiếng Anh)
- [ ] Có file cho `zh` hoặc `zh-Hans` (tiếng Trung giản thể)

### Test 17.2 – Fallback hoạt động
- [ ] Khi thiếu key trong `vi`, fallback sang `en`
- [ ] Locale switcher hoặc mechanism chuyển ngôn ngữ tồn tại

---

## 18. xUnit + Testcontainers (P0-QA-0.1)

### Test 18.1 – Test project tồn tại + chạy được
```bash
ls tests/  # hoặc src/*.Tests/
dotnet test tests/ --verbosity normal
```
- [ ] Project test tồn tại
- [ ] `dotnet test` chạy không lỗi framework
- [ ] Có ít nhất 1 test skeleton (có thể empty/pass)

### Test 18.2 – Testcontainers configured
- [ ] Có NuGet package `Testcontainers` hoặc `Testcontainers.PostgreSql`/`Testcontainers.MongoDb`
- [ ] Có base class hoặc fixture cho Postgres container
- [ ] Có base class hoặc fixture cho MongoDB container

---

## 19. Playwright Smoke (P0-QA-0.2)

### Test 19.1 – Playwright project tồn tại
```bash
ls tests/e2e/           # hoặc e2e/ hoặc playwright/
npx playwright --version
```
- [ ] Project Playwright tồn tại
- [ ] Dependencies cài được

### Test 19.2 – Smoke test chạy được
```bash
npx playwright test --reporter=list
```
- [ ] Có ít nhất 1 smoke test skeleton
- [ ] Test runner không crash

---

## Tổng kết Phase 0 Test

| # | Nhóm | Số test case | Kết quả |
|---|---|---:|---|
| 1–3 | Repo & Config & Secrets | 7 | |
| 4–7 | CI/CD | 6 | |
| 8 | PostgreSQL Schema | 7 | |
| 9 | MongoDB Init | 5 | |
| 10 | Seed cards_catalog | 3 | |
| 11 | Seed system_configs | 2 | |
| 12 | Seed EXP levels | 2 | |
| 13–14 | API Scaffold | 3 | |
| 15 | Auth Cookie | 1 | |
| 16–17 | Web Scaffold + i18n | 4 | |
| 18–19 | Test Framework | 4 | |
| **Tổng** | | **44** | |

> **Ghi chú:** Đường dẫn `src/api`, `src/web`, `tests/` trong file này là placeholder. Thay bằng đường dẫn thực tế trong project.

# Phase 5+ – Tài liệu thiết kế liên quan

**Mục đích:** Trích các phần thiết kế cần đọc khi làm Phase 5+ (Retention, Monetization, Gamification, Hardening, Expansion).  
**Nguồn gốc:** Trích từ `01-business-rules.md`, `02-product-ux-specs.md`, `03-tech-architecture.md`, `04-ops-security-compliance.md`

---

## 5.1 Retention (Giữ chân)

### Quy tắc kinh doanh (BR 5.1, BR-7, 4.8.4, 4.11)

**Daily Check-in:**
- Mỗi ngày check-in: tạo record `daily_checkins` (unique per user per business_date)
- Phần thưởng: Gold (configurable qua `system_configs.daily_checkin_gold`)
- Credit qua `proc_wallet_credit`, idempotent

**Daily Streak Model (BR-7):**
- Mỗi ngày có ít nhất 1 lần rút bài hợp lệ → streak +1
- Streak multiplier: `+1% EXP` per streak day
- Công thức: `EXP_final = EXP_base × (1 + streak_days / 100)`
- Tính theo UTC business date, mỗi ngày chỉ cộng 1 lần

**Streak Freeze:**
- Giá: `ceil(pre_break_streak_days / 10)` Diamond
- Khi `pre_break_streak_days = 0` → không hiển thị/không cho mua
- Cửa sổ dùng: **24h** sau ngày bị ngắt (configurable: `streak_freeze_window_hours`)
- Mua freeze → debit Diamond + restore streak, idempotent
- Ví dụ: streak 7 → 1 Diamond, streak 11 → 2 Diamond

**Retention Loops (BR 4.11):**
- **D0 Value Loop**: verify email → free draw + AI stream + gợi ý follow-up
- **Daily Habit Loop**: nhắc daily card → EXP + streak
- **Streak Rescue Loop**: sắp gãy streak → đề xuất freeze + countdown
- **Session Completion Loop**: AI gợi ý follow-up cụ thể
- **Collection Progress Loop**: "còn X EXP để unlock hiệu ứng mới"
- **Reader Relationship Loop**: "Reader quen thuộc đang online"
- **Quest Cadence Loop**: daily quest + weekly quest
- **Win-back Loop**: im lặng 3/7/14 ngày → ưu đãi phục hồi

**Notifications:**
- MongoDB `notifications` collection (TTL 30 ngày)
- API: list/mark-read/delete
- Template pipeline vi/en/zh + Unicode checks

### Database liên quan
- **PostgreSQL**: `users.current_streak`, `users.last_streak_date`, `users.pre_break_streak`
- **MongoDB**: `daily_checkins` (unique: user_id + business_date)

---

## 5.2 Monetization Nâng cao (Subscription + Entitlement + Event Packs)

### Subscription Model (BR-6, 4.3.4)
- 1 user có thể có **nhiều subscription active** đồng thời
- Mỗi subscription đóng góp entitlement riêng
- Entitlement theo key (vd: `free_spread_3_daily`, `free_spread_5_daily`)
- Reset theo **UTC business date**

### Entitlement Algorithm (BR-16, ARCH 4.3.4) – LOCKED
- Consume theo **earliest-expiry-first**
- Tie-break: `subscription_id` tăng dần (deterministic)
- **Không** auto cross-key deduction (trừ khi có mapping rule explicit + ON)
- Mọi consume ghi usage log → chống double-count
- Source of truth = **DB write path** (không phải cache)
- Atomic consume: transaction + row lock

**Mapping rules:**
- Ví dụ: `free_spread_5_daily → free_spread_3_daily` (ratio 1:1)
- Mặc định **OFF** → tránh tiêu thụ ngoài ý định
- Bảng `entitlement_mapping_rules` quản lý

**Cache invalidation:**
- Cache key: `user_id + business_date`
- Invalidate qua events: SubscriptionActivated, Renewed, Expired, EntitlementAdjusted
- Cache không phải source of truth cho quyết định tài chính

### Event Packs (BR 4.3.5)
- Full Moon Pack + event packs theo cửa sổ thời gian
- Benefits có thời hạn (bonus Diamond, EXP multiplier trong N ngày)
- Chỉ mua trong event window hợp lệ
- Idempotent như deposit order

### Monetization Orchestration (BR 4.12)
- First Purchase: hiện gói nạp nhỏ sau 2-3 phiên high completion
- Intent-based Offer: thiếu Diamond → gói "vừa đủ"
- Stacked Subscription: chỉ hiện gói còn thiếu entitlement
- Streak Protection: chỉ đẩy freeze khi nguy cơ gãy thật
- Event Pack: chỉ hiện cho nhóm active + chi tiêu
- Escrow Upsell: chỉ khi reader trả lời chất lượng cao
- Refund Recovery: không upsell ngay sau timeout/refund
- High-value Segment: ưu đãi dài hạn, hạn chế giảm giá ngắn

### Database liên quan
- **PostgreSQL**: `subscription_plans`, `user_subscriptions`, `subscription_entitlement_buckets`, `entitlement_consumes`, `entitlement_mapping_rules`

---

## 5.3 Gamification (Quests/Achievements/Titles/Leaderboards)

### Quests (BR 4.8.1)
- Daily/weekly/monthly/seasonal quests
- Definition tách riêng, progress records theo user + period
- Claim rewards idempotent + ledger

### Achievements & Titles (BR 4.8.2)
- Achievement unlock: one-time per user
- Title ownership + active title selection (sync to `users.active_title_ref`)

### Leaderboards (BR 4.8.3, BR-5)
- 3 score tracks song song: `daily_rank_score`, `monthly_rank_score`, `lifetime_score`
- Snapshot job per period key (unique)
- Recalculation job + auditability
- Types mở rộng tương lai: `achievement_points`, `diamond_spend`, `top_readers`

### Database liên quan
- **MongoDB**: `quests`, `quest_progress`, `achievements`, `user_achievements`, `titles`, `user_titles`, `leaderboard_snapshots`

---

## 5.4 Platform Hardening

### Rate Limiting (ARCH Rate-limit-matrix)
- Auth endpoints: escalation policy
- Chat endpoints: per user per window
- Payment endpoints: strict limits
- AI endpoints: in-flight cap (**max 2** concurrent per user)
- Storage: Redis

### Observability (OPS 5)
- OpenTelemetry: traces/metrics/logs correlation
- Structured logs với trace IDs
- `idempotency_key` + `correlation_id` + `trace_id` cho mọi finance transition
- Cảnh báo SLI/SLO nội bộ

### Background Jobs
- Settlement/refund/release jobs gia cố
- Idempotent + retry giới hạn + dead-letter queue
- Instrument với OTel

---

## 5.5 Non-core Expansion

### Share Reward (BR 4.7.1)
- First share/ngày/mạng → **+2 Gold**
- Anti-abuse heuristic: reward cap, cooldown, device fingerprint, IP velocity
- Risk scoring: auto approve / soft-block / hard-block
- Proof-share module (advanced) ở Phase 5.5
- Share link: signed + TTL
- MongoDB: `share_claims` (cross-DB ref: `wallet_tx_ref → wallet_transactions.id`)

### Reviews & Reports (UX 4.6.1, 4.6.5)
- Reviews: write/list APIs + moderation flags
- Reports: queue + admin actions
- Audit trail cho moderation

### Referrals (BR 4.7.3)
- Referral code on register
- Track referral signup (`referred_by_id`)
- Credit rewards on milestones (idempotent)
- MongoDB: `referrals` (unique: inviter_id + invited_user_id)

---

## 5.6 Gacha System

### Gacha Rules (BR-23, UX 4.5.4)
- Công bố tỷ lệ (odds) + version rõ ràng trước mua
- Mua + gán thưởng: **idempotent + auditable**
- Pity rule (configurable): nếu bật thì deterministic
- Public disclosure contract: JSON schema có version
- RNG dùng cùng fairness framework (versioned seed + audit package)
- Chi phí: **5 Diamond/spin** (configurable: `gacha_cost_diamond`)
- Giữ reward logs >= dispute retention period

### Database liên quan
- **PostgreSQL**: `gacha_odds_versions` (odds + effective_from/to), `gacha_reward_logs` (idempotency_key, spent_diamond)
- **MongoDB**: `gacha_logs` (TTL 180 ngày)

---

## 5.7 GDPR / Data Rights

### Data Rights (OPS 4.13.7)
- Request types: `access_export`, `correction`, `deletion`
- Status flow: `pending → processing → completed/rejected`
- SLA xử lý nội bộ
- Export job: aggregate PG + Mongo → machine-readable + human summary
- Deletion: soft-delete + purge PII, nhưng **giữ ledger** (financial records)

### Database liên quan
- **PostgreSQL**: `data_rights_requests`

---

## 5.8 Geo Compliance

### Geo Feature Gating (BR-22, OPS 4.13.5)
- RNG monetization features (Gacha, event RNG): country/region gate
- Server-side API enforcement bắt buộc
- Multi-signal decision: `account_jurisdiction`, KYC country, payment country, IP geolocation
- Tín hiệu mâu thuẫn / VPN risk → `restricted_review`
- `restricted_review` SLA: auto-resolve **<= 24h**, quá hạn → escalate manual review
- Auto-resolve conditions: `kyc_verified=true`, `payment_country=account_jurisdiction`, `ip_geo_consistency_score >= 0.95`, no `vpn_proxy_risk`

### Database liên quan
- **PostgreSQL**: `user_geo_signals`

---

## 5.9 Friend Chain

### Friend Tarot Chain (BR 4.7.2)
- Mời bạn cùng rút bài → cả hai thưởng
- Reward: **3 Gold** mỗi chain (configurable: `friend_chain_reward_gold`)
- Daily cap: **3 lần/ngày** (configurable: `friend_chain_daily_cap`)
- Credit idempotent cho cả host + guest

### Database liên quan
- **MongoDB**: `reading_chains` (unique: host_user_id + guest_user_id + business_date)

---

## 5.10 Card Stories / Ascension

### Ascension Stories (UX 4.5.2)
- Card level milestones (6, 11, 16, 20+) trigger Ascension story
- AI generate story + persist
- Mỗi lá có story riêng theo level milestone

### Database liên quan
- **MongoDB**: `card_stories` (unique: user_id + card_id + level_trigger)

---

## Tham chiếu đầy đủ

| Tài liệu | Sections liên quan |
|---|---|
| [01-business-rules.md](../All_Phase/tai-lieu-thiet-ke/01-business-rules.md) | Phase 5+ (5.1–5.5), BR-5,6,7,22,23, 4.3.4, 4.3.5, 4.7, 4.8, 4.11, 4.12 |
| [02-product-ux-specs.md](../All_Phase/tai-lieu-thiet-ke/02-product-ux-specs.md) | 4.5.1–4.5.4, 4.6.1, 4.6.5, 4.8, 4.9, 4.15 |
| [03-tech-architecture.md](../All_Phase/tai-lieu-thiet-ke/03-tech-architecture.md) | 4.3.4, Rate-limit-matrix, 4.14.1 |
| [04-ops-security-compliance.md](../All_Phase/tai-lieu-thiet-ke/04-ops-security-compliance.md) | 4.13.5, 4.13.7, 5 (all NFRs) |
| [database/DESIGN_DECISIONS.md](../../database/DESIGN_DECISIONS.md) | Retention, Partitioning, proc_wallet_* |

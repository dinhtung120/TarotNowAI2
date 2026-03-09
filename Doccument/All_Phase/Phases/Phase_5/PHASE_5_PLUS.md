# Phase 5+ – Retention, Monetization, Gamification, Hardening & Expansion

**Nguồn:** `CODING_PLAN.md` Sections 8.1–8.10  
**Mục tiêu:** Track dài sau mobile – giữ chân người dùng, kiếm tiền nâng cao, gamification đầy đủ, gia cố nền tảng, và mở rộng tính năng.

---

## Quy ước

- **PD** = person-days | Spec: `BR` = business-rules, `UX` = product-ux, `ARCH` = tech-architecture, `OPS` = ops-security, `DB` = database/

---

## 5.1 Retention (Daily Habit + Streak Rescue + Win-back)

### BE – Streak

- [ ] **P5-STREAK-BE-1.1** (1.0 PD) – Track daily activity marker (valid draw)
  - Spec: BR(7), BR(4.11.4)

- [ ] **P5-STREAK-BE-1.2** (1.0 PD) – Increment `current_streak` once/day
  - Spec: BR(7)

- [ ] **P5-STREAK-BE-1.3** (1.0 PD) – Maintain `pre_break_streak` anchor
  - Spec: BR(7)

### BE – Streak Freeze

- [ ] **P5-FREEZE-BE-1.1** (0.75 PD) – Compute Streak Freeze price + eligibility
  - `ceil(pre_break/10)` Diamond | Spec: BR(7)

- [ ] **P5-FREEZE-BE-1.2** (1.5 PD) – Purchase freeze: charge Diamond + restore
  - Spec: BR(7), DB(DESIGN_DECISIONS)

### BE – Daily Check-in

- [ ] **P5-CHECKIN-BE-1.1** (1.0 PD) – Daily check-in endpoint
  - Tạo record `daily_checkins`, unique per user per business_date
  - Spec: BR(7), DB(mongodb-schema)

- [ ] **P5-CHECKIN-BE-1.2** (0.75 PD) – Credit Gold for check-in
  - Via `proc_wallet_credit`, type=daily_checkin, idempotent
  - Spec: DB(DESIGN_DECISIONS)

- [ ] **P5-CHECKIN-BE-1.3** (0.75 PD) – Compute streak from check-in
  - Spec: BR(7)

### BE – Notifications

- [ ] **P5-NOTIF-BE-1.1** (1.5 PD) – Schedule daily habit notification
  - Spec: BR(4.11.2)

- [ ] **P5-NOTIF-BE-1.2** (1.5 PD) – Win-back triggers 3/7/14 days
  - Spec: BR(4.11.8)

- [ ] **P5-NOTIF-BE-1.3** (1.0 PD) – API list/mark-read/delete notifications
  - Spec: DB(mongodb-schema), UX(4.9)

### FE – Retention

- [ ] **P5-STREAK-FE-1.1** (1.0 PD) – Streak UI: current streak + last active
  - Spec: BR(7)

- [ ] **P5-STREAK-FE-1.2** (1.0 PD) – Freeze CTA + price display + gating
  - Spec: BR(7)

- [ ] **P5-STREAK-FE-1.3** (1.0 PD) – Countdown window UI + error mapping
  - Spec: UX(4.15.3)

- [ ] **P5-CHECKIN-FE-1.1** (1.0 PD) – Daily check-in UI + streak display
  - Spec: BR(7)

- [ ] **P5-NOTIF-FE-1.1** (1.5 PD) – Notification bell + list UI + mark read
  - Spec: UX(4.9)

### DevOps – Notifications

- [ ] **P5-NOTIF-OPS-1.1** (1.5 PD) – Notification provider setup + env
  - Spec: OPS(5.Security)

- [ ] **P5-NOTIF-OPS-1.2** (1.5 PD) – Template pipeline vi/en/zh + Unicode checks
  - Spec: UX(4.9), OPS(5.Observability)

### QA – Retention

- [ ] **P5-RET-QA-1.1** (1.0 PD) – Regression: streak increments once/day
  - Spec: BR(7)

- [ ] **P5-RET-QA-1.2** (1.0 PD) – Regression: freeze price rounding + charge
  - Spec: BR(7)

- [ ] **P5-RET-QA-1.3** (2.0 PD) – Notification i18n + delivery smoke
  - Spec: UX(4.9)

- [ ] **P5-CHECKIN-QA-1.1** (1.0 PD) – Check-in idempotency + Gold credit
  - Spec: BR(7)

---

## 5.2 Monetization Nâng cao (Subscription + Entitlement + Event Packs)

### DB

- [ ] **P5-SUB-DB-1.1** (1.0 PD) – Review schema constraints/indexes for subscription
  - Spec: DB(schema.sql)

- [ ] **P5-SUB-DB-1.2** (1.0 PD) – Validate entitlement buckets reset keys by UTC business date
  - Spec: BR(6), ARCH(4.3.4)

- [ ] **P5-SUB-DB-1.3** (1.0 PD) – Verify `entitlement_consumes` idempotency unique index
  - Spec: DB(DESIGN_DECISIONS)

### BE – Entitlement

- [ ] **P5-ENT-BE-1.1** (1.0 PD) – Data model read: active subscriptions + entitlements
  - Spec: BR(6)

- [ ] **P5-ENT-BE-1.2** (1.5 PD) – Earliest-expiry-first + deterministic tie-break
  - Spec: BR(16), ARCH(4.3.4)

- [ ] **P5-ENT-BE-1.3** (2.0 PD) – Atomic consume transaction + row locking + usage log
  - Spec: ARCH(4.3.4)

- [ ] **P5-ENT-BE-1.4** (1.5 PD) – Prevent double-consume with idempotency
  - Spec: ARCH(4.3.4)

- [ ] **P5-ENT-BE-1.5** (1.0 PD) – Daily reset logic (UTC rollover)
  - Spec: BR(6)

### BE – Event Packs

- [ ] **P5-EVENT-BE-1.1** (1.5 PD) – Event pack catalog + window validation
  - Spec: BR(5.2), BR(4.3.5)

- [ ] **P5-EVENT-BE-1.2** (2.0 PD) – Purchase event pack idempotent + ledger entries
  - Spec: BR(4.3.5), DB(DESIGN_DECISIONS)

- [ ] **P5-EVENT-BE-1.3** (1.5 PD) – Entitlement expiry scheduling for event benefits
  - Spec: BR(4.3.5)

### DevOps – Entitlement

- [ ] **P5-ENT-OPS-1.1** (1.5 PD) – Entitlement cache key strategy + TTL + invalidation
  - Spec: ARCH(4.3.4)

- [ ] **P5-ENT-OPS-1.2** (1.5 PD) – Outbox wiring plan for cache invalidation reliability
  - Spec: ARCH(4.14.1)

### FE – Subscription & Events

- [ ] **P5-SUB-FE-1.1** (1.5 PD) – Subscription list UI + active status
  - Spec: BR(6)

- [ ] **P5-SUB-FE-1.2** (1.5 PD) – Quota display per entitlement key
  - Spec: BR(6)

- [ ] **P5-SUB-FE-1.3** (1.5 PD) – Upsell surfaces when quota exhausted
  - Spec: BR(6)

- [ ] **P5-SUB-FE-1.4** (1.5 PD) – Usage history UI (optional)
  - Spec: ARCH(4.3.4)

- [ ] **P5-EVENT-FE-1.1** (1.5 PD) – Event store UI + countdown window
  - Spec: BR(4.3.5)

- [ ] **P5-EVENT-FE-1.2** (1.5 PD) – Purchase flow UI + success/failure
  - Spec: UX(4.15.3)

- [ ] **P5-EVENT-FE-1.3** (1.0 PD) – Benefit display + expiry indicator
  - Spec: BR(4.3.5)

### QA – Monetization

- [ ] **P5-MON-QA-1.1** (2.0 PD) – Concurrency burst consume → no double-consume
  - Spec: ARCH(Testing-strategy)

- [ ] **P5-MON-QA-1.2** (1.5 PD) – Same-expiry tie-break deterministic case
  - Spec: BR(16)

- [ ] **P5-MON-QA-1.3** (1.0 PD) – Reset correctness at UTC boundary
  - Spec: BR(6)

---

## 5.3 Gamification (Quests/Achievements/Titles/Leaderboards)

### BE

- [ ] **P5-QUEST-BE-1.1** (2.0 PD) – Quest definitions CRUD + publish active
  - Spec: BR(4.8.1)

- [ ] **P5-QUEST-BE-1.2** (2.0 PD) – Progress tracking per period key
  - Spec: BR(4.8.1)

- [ ] **P5-QUEST-BE-1.3** (2.0 PD) – Claim rewards idempotent + ledger
  - Spec: BR(4.8.1), DB(DESIGN_DECISIONS)

- [ ] **P5-QUEST-BE-1.4** (1.0 PD) – Anti-abuse baseline counters
  - Spec: OPS(5.Anti-fraud)

- [ ] **P5-ACHV-BE-1.1** (2.0 PD) – Achievements definitions + unlock hooks
  - Spec: BR(4.8.2)

- [ ] **P5-ACHV-BE-1.2** (2.0 PD) – Titles ownership + active title sync
  - Spec: BR(4.8.2), DB(DATABASE_OVERVIEW)

- [ ] **P5-ACHV-BE-1.3** (2.0 PD) – One-time unlock enforcement + idempotency
  - Spec: BR(4.8.2)

- [ ] **P5-LB-BE-1.1** (2.0 PD) – Score tracks: daily/monthly/lifetime
  - Spec: BR(4.8.3)

- [ ] **P5-LB-BE-1.2** (2.0 PD) – Snapshot job per period key unique
  - Spec: BR(4.8.3)

- [ ] **P5-LB-BE-1.3** (3.0 PD) – Recalculation job + auditability
  - Spec: BR(4.8.3)

### FE

- [ ] **P5-GAME-FE-1.1** (2.5 PD) – Quests list/progress UI + claim
  - Spec: BR(4.8.1)

- [ ] **P5-GAME-FE-1.2** (2.0 PD) – Achievements UI
  - Spec: BR(4.8.2)

- [ ] **P5-GAME-FE-1.3** (1.5 PD) – Titles picker UI + active display
  - Spec: BR(4.8.2)

- [ ] **P5-GAME-FE-1.4** (2.0 PD) – Leaderboard UI (3 tabs)
  - Spec: BR(4.8.3)

### QA

- [ ] **P5-GAME-QA-1.1** (2.0 PD) – E2E: quest claim idempotency
  - Spec: BR(4.8.1)

- [ ] **P5-GAME-QA-1.2** (2.0 PD) – E2E: leaderboard snapshot uniqueness
  - Spec: BR(4.8.3)

- [ ] **P5-GAME-QA-1.3** (2.0 PD) – Regression: title selection in profile
  - Spec: BR(4.8.2)

> **Note:** Leaderboard `type` values `achievement_points`, `diamond_spend`, `top_readers` là **future extension**. Chỉ implement 3 types cốt lõi: `daily_rank_score`, `monthly_rank_score`, `lifetime_score`.

---

## 5.4 Platform Hardening (Rate Limit + OTel + Jobs)

### DevOps

- [ ] **P5-RL-OPS-1.1** (2.0 PD) – Configure rate limiter storage (Redis)
  - Spec: ARCH(Rate-limit-matrix)

- [ ] **P5-RL-OPS-1.2** (2.0 PD) – Dashboards + alerts for violations
  - Spec: OPS(5.Observability)

- [ ] **P5-OTEL-OPS-1.1** (2.5 PD) – OTel collector/exporters per env
  - Spec: OPS(5.Observability)

- [ ] **P5-OTEL-OPS-1.2** (2.5 PD) – Trace/log correlation standard
  - Spec: OPS(5.Observability)

### BE

- [ ] **P5-RL-BE-1.1** (1.5 PD) – Auth endpoints rate limit + escalation
  - Spec: ARCH(Rate-limit-matrix)

- [ ] **P5-RL-BE-1.2** (1.0 PD) – Chat endpoints rate limit
  - Spec: ARCH(Rate-limit-matrix)

- [ ] **P5-RL-BE-1.3** (1.0 PD) – Payment endpoints rate limit
  - Spec: ARCH(Rate-limit-matrix)

- [ ] **P5-RL-BE-1.4** (1.5 PD) – AI endpoints rate limit + in-flight cap
  - Spec: ARCH(4.4.3)

- [ ] **P5-OTEL-BE-1.1** (2.0 PD) – Instrument finance transitions logs
  - Spec: OPS(5.Observability)

- [ ] **P5-OTEL-BE-1.2** (2.0 PD) – Instrument jobs with idempotency
  - Spec: OPS(5.Reliability)

### QA

- [ ] **P5-HARD-QA-1.1** (2.0 PD) – Load smoke: SSE + SignalR + API mix
  - Spec: ARCH(Testing-strategy)

- [ ] **P5-HARD-QA-1.2** (3.0 PD) – Failure injection: queue lag + webhook storms
  - Spec: OPS(5.Reliability)

---

## 5.5 Non-core Expansion (Share, Reviews/Reports, Referrals)

### BE – Share & Proof

- [ ] **P5-SHARE-BE-1.1** (2.0 PD) – Signed share deep-link + TTL
  - Spec: BR(4.7.1)

- [ ] **P5-SHARE-BE-1.2** (3.0 PD) – Proof-share verification + risk score
  - Spec: BR(4.7.1), OPS(5.Anti-fraud)

- [ ] **P5-SHARE-BE-1.3** (3.0 PD) – Reward caps + cooldown + ledger credit
  - Spec: BR(4.7.1), DB(DESIGN_DECISIONS)

### BE – Reviews & Reports

- [ ] **P5-REP-BE-1.1** (2.0 PD) – Reviews write/list APIs + moderation flags
  - Spec: UX(4.6.1)

- [ ] **P5-REP-BE-1.2** (2.0 PD) – Reports queue + admin actions
  - Spec: DB(mongodb-schema)

- [ ] **P5-REP-BE-1.3** (2.0 PD) – Audit trail for moderation actions
  - Spec: OPS(4.10)

### BE – Referrals

- [ ] **P5-REF-BE-1.1** (0.75 PD) – Generate referral code on register
  - Spec: BR(4.7.3), DB(schema.sql)

- [ ] **P5-REF-BE-1.2** (1.0 PD) – Track referral signup + set `referred_by_id`
  - Spec: BR(4.7.3), DB(mongodb-schema)

- [ ] **P5-REF-BE-1.3** (1.5 PD) – Credit referral rewards on milestones
  - Spec: BR(4.7.3), DB(DESIGN_DECISIONS)

### FE

- [ ] **P5-NON-FE-1.1** (2.5 PD) – Share proof UI + claim reward UX
  - Spec: BR(4.7.1)

- [ ] **P5-NON-FE-1.2** (2.5 PD) – Review UI + report UI (user-facing)
  - Spec: UX(4.6.1), UX(4.6.5)

- [ ] **P5-NON-FE-1.3** (2.0 PD) – Admin moderation screens
  - Spec: OPS(4.10)

- [ ] **P5-REF-FE-1.1** (1.5 PD) – Referral UI (share code + history)
  - Spec: BR(4.7.3)

### QA

- [ ] **P5-NON-QA-1.1** (2.0 PD) – Abuse matrix: share claim velocity
  - Spec: BR(4.7.1)

- [ ] **P5-NON-QA-1.2** (3.0 PD) – Moderation workflows E2E
  - Spec: UX(4.6.5)

- [ ] **P5-REF-QA-1.1** (1.5 PD) – E2E referral + self-referral prevention
  - Spec: BR(4.7.3)

---

## 5.6 Gacha System

### DB

- [ ] **P5-GACHA-DB-1.1** (0.5 PD) – Verify gacha indexes
  - Spec: DB(schema.sql), DB(mongodb-schema)

### BE

- [ ] **P5-GACHA-BE-1.1** (1.5 PD) – Gacha odds catalog API + active version lookup
  - Spec: BR(5.2), UX(4.5.4)

- [ ] **P5-GACHA-BE-1.2** (2.0 PD) – Gacha spin: RNG + weighted selection
  - Spec: BR(5.2), ARCH(4.4.2)

- [ ] **P5-GACHA-BE-1.3** (2.0 PD) – Charge Diamond + credit rewards idempotent
  - Spec: DB(DESIGN_DECISIONS)

- [ ] **P5-GACHA-BE-1.4** (1.5 PD) – Log results to PG + Mongo
  - Spec: DB(schema.sql), DB(mongodb-schema)

- [ ] **P5-GACHA-BE-1.5** (2.0 PD) – Pity timer / guaranteed rarity
  - Spec: BR(5.2)

### FE

- [ ] **P5-GACHA-FE-1.1** (2.5 PD) – Gacha spin UI + animation
  - Spec: UX(4.5.4)

- [ ] **P5-GACHA-FE-1.2** (1.5 PD) – Gacha odds disclosure UI
  - Spec: UX(4.5.4), OPS(4.13.1)

- [ ] **P5-GACHA-FE-1.3** (1.5 PD) – Gacha history + rewards earned
  - Spec: UX(4.5.4)

### QA

- [ ] **P5-GACHA-QA-1.1** (2.0 PD) – RNG fairness property test
  - Spec: ARCH(4.4.2)

- [ ] **P5-GACHA-QA-1.2** (1.5 PD) – Idempotency: double-spin prevention
  - Spec: DB(DESIGN_DECISIONS)

---

## 5.7 Data Rights / GDPR Compliance

### BE

- [ ] **P5-GDPR-BE-1.1** (1.5 PD) – Submit data rights request API
  - Spec: OPS(4.13.7)

- [ ] **P5-GDPR-BE-1.2** (1.5 PD) – Admin process request + status transitions
  - Spec: OPS(4.13.7)

- [ ] **P5-GDPR-BE-1.3** (3.0 PD) – Data export job: aggregate PG + Mongo
  - Spec: OPS(4.13.7)

- [ ] **P5-GDPR-BE-1.4** (3.0 PD) – Data deletion job: soft-delete + purge PII
  - Spec: OPS(4.13.7)

### FE

- [ ] **P5-GDPR-FE-1.1** (1.5 PD) – User data rights request UI
  - Spec: OPS(4.13.7)

- [ ] **P5-GDPR-FE-1.2** (1.5 PD) – Admin data rights queue UI
  - Spec: OPS(4.13.7)

### QA

- [ ] **P5-GDPR-QA-1.1** (2.0 PD) – E2E: export returns complete user data
  - Spec: OPS(4.13.7)

- [ ] **P5-GDPR-QA-1.2** (2.0 PD) – E2E: deletion purges PII but preserves ledger
  - Spec: OPS(4.13.7)

---

## 5.8 Geo Compliance

### BE

- [ ] **P5-GEO-BE-1.1** (2.0 PD) – Geo decision engine middleware
  - Spec: OPS(4.13.5)

- [ ] **P5-GEO-BE-1.2** (2.0 PD) – Legal matrix CRUD + versioned flags
  - Spec: OPS(4.13.5)

- [ ] **P5-GEO-BE-1.3** (1.5 PD) – Server-side gate enforcement
  - Spec: OPS(4.13.5)

- [ ] **P5-GEO-BE-1.4** (2.0 PD) – Auto-resolve restricted_review job (SLA 24h)
  - Spec: OPS(4.13.5)

### FE

- [ ] **P5-GEO-FE-1.1** (1.5 PD) – Admin geo review queue UI
  - Spec: OPS(4.13.5)

- [ ] **P5-GEO-FE-1.2** (1.0 PD) – User-facing: feature hidden + reason
  - Spec: OPS(4.13.5)

### QA

- [ ] **P5-GEO-QA-1.1** (2.0 PD) – E2E: geo block + restricted + auto-resolve
  - Spec: OPS(4.13.5)

---

## 5.9 Friend Chain / Reading Chain

### BE

- [ ] **P5-CHAIN-BE-1.1** (1.5 PD) – Generate friend chain share link + token
  - Spec: BR(4.7.2)

- [ ] **P5-CHAIN-BE-1.2** (1.5 PD) – Join chain: create reading_chain + validate quota
  - Spec: BR(4.7.2), DB(mongodb-schema)

- [ ] **P5-CHAIN-BE-1.3** (1.5 PD) – Credit rewards for both parties (idempotent)
  - Spec: BR(4.7.2), DB(DESIGN_DECISIONS)

### FE

- [ ] **P5-CHAIN-FE-1.1** (1.5 PD) – Friend chain invite + join UI
  - Spec: BR(4.7.2)

### QA

- [ ] **P5-CHAIN-QA-1.1** (1.5 PD) – E2E: chain + reward + daily cap
  - Spec: BR(4.7.2)

---

## 5.10 Card Stories / Ascension

### BE

- [ ] **P5-STORY-BE-1.1** (1.0 PD) – Detect card level-up to milestone (6-20)
  - Spec: UX(4.5.2), DB(mongodb-schema)

- [ ] **P5-STORY-BE-1.2** (2.0 PD) – AI generate Ascension story + persist
  - Spec: UX(4.5.2), DB(mongodb-schema)

### FE

- [ ] **P5-STORY-FE-1.1** (1.5 PD) – Card story display UI
  - Spec: UX(4.5.2)

### QA

- [ ] **P5-STORY-QA-1.1** (1.5 PD) – E2E: level-up trigger + story creation
  - Spec: UX(4.5.2)

---

## Tổng kết Phase 5+

| Sub-phase | Số task | Tổng PD |
|---|---|---:|
| 5.1 Retention | 23 | ~24.5 |
| 5.2 Monetization | 22 | ~33 |
| 5.3 Gamification | 17 | ~32 |
| 5.4 Hardening | 12 | ~23 |
| 5.5 Non-core | 16 | ~30 |
| 5.6 Gacha | 11 | ~18.5 |
| 5.7 GDPR | 8 | ~16 |
| 5.8 Geo | 7 | ~12 |
| 5.9 Friend Chain | 5 | ~7.5 |
| 5.10 Card Stories | 4 | ~6 |
| **Tổng** | **125** | **~202.5** |

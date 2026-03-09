# Phase 2 – Reader Marketplace + Chat + Escrow

**Nguồn:** `CODING_PLAN.md` Sections 5.1–5.6  
**Mục tiêu:** Reader listing + approval, chat 1-1 realtime (SignalR), escrow core (freeze→reply→release/refund), dispute + admin, withdrawal + payout, MFA cho Reader/Admin.

---

## Quy ước

- **PD** = person-days | Spec: `BR` = business-rules, `UX` = product-ux, `ARCH` = tech-architecture, `OPS` = ops-security, `DB` = database/

---

## 2.1 Reader Listing + Approval

### DB

- [ ] **P2-READER-DB-1.1** (0.5 PD) – Confirm indexes for `reader_profiles`
  - Rà soát index: unique user_id, status+updated_at | Spec: DB(mongodb-schema)

- [ ] **P2-READER-DB-1.2** (0.5 PD) – Confirm indexes for `reader_requests` + `reviews`
  - Spec: DB(mongodb-schema)

### BE

- [ ] **P2-READER-BE-1.1** (0.75 PD) – Create reader request (user submits)
  - Spec: UX(4.1.4)

- [ ] **P2-READER-BE-1.2** (1.0 PD) – Admin approve/reject reader request + audit
  - Spec: OPS(4.10)

- [ ] **P2-READER-BE-1.3** (0.75 PD) – On approve: set role + reader_status
  - Cập nhật `users.role=tarot_reader` | Spec: UX(4.1.4)

- [ ] **P2-READER-BE-1.4** (1.0 PD) – Create/update Mongo `reader_profiles`
  - Spec: DB(mongodb-schema)

- [ ] **P2-READER-BE-1.5** (0.75 PD) – Gate: approved + accepting_questions required
  - Spec: UX(4.1.4)

### FE

- [ ] **P2-READER-FE-1.1** (1.0 PD) – Reader directory page UI
  - Spec: UX(4.6.1)

- [ ] **P2-READER-FE-1.2** (1.0 PD) – Filters UI (giá/đánh giá/chuyên môn)
  - Spec: UX(4.6.1)

- [ ] **P2-READER-FE-1.3** (1.0 PD) – Reader profile page + gating state
  - Spec: UX(4.6.1)

- [ ] **P2-READER-FE-1.4** (1.0 PD) – Admin UI: approval queue
  - Spec: OPS(4.10)

### QA

- [ ] **P2-READER-QA-1.1** (1.5 PD) – E2E: request → approve → listed
  - Spec: UX(4.6.1)

- [ ] **P2-READER-QA-1.2** (1.0 PD) – Test gating message button
  - Spec: UX(4.6.1)

---

## 2.2 Chat 1-1 (SignalR)

### BE

- [ ] **P2-CHAT-BE-1.1** (1.0 PD) – Create conversation + index lookups
  - Spec: ARCH(4.6.3), DB(mongodb-schema)

- [ ] **P2-CHAT-BE-1.2** (1.0 PD) – SignalR Hub auth + connect/disconnect
  - Spec: ARCH(1.1)

- [ ] **P2-CHAT-BE-1.3** (1.5 PD) – Persist messages to `chat_messages` with types
  - Spec: DB(mongodb-schema)

- [ ] **P2-CHAT-BE-1.4** (1.0 PD) – Read state + unread_count update strategy
  - Spec: UX(4.6.2)

- [ ] **P2-CHAT-BE-1.5** (1.0 PD) – Moderation hook + report endpoint
  - Spec: UX(4.6.5)

### FE

- [ ] **P2-CHAT-FE-1.1** (1.0 PD) – Inbox list UI
  - Spec: UX(4.6.2)

- [ ] **P2-CHAT-FE-1.2** (1.5 PD) – Chat screen + sending
  - Spec: UX(4.6.2)

- [ ] **P2-CHAT-FE-1.3** (1.5 PD) – SignalR client connect/reconnect
  - Spec: UX(2.7)

- [ ] **P2-CHAT-FE-1.4** (1.0 PD) – Read receipts UI + unread badges
  - Spec: UX(4.6.2)

- [ ] **P2-CHAT-FE-1.5** (1.0 PD) – Report UI + reason codes
  - Spec: UX(4.6.5)

### DevOps

- [ ] **P2-CHAT-OPS-1.1** (1.0 PD) – Health checks + metrics
  - Spec: OPS(5.Observability)

- [ ] **P2-CHAT-OPS-1.2** (1.0 PD) – Load test harness + thresholds doc
  - Spec: OPS(5.Performance)

### QA

- [ ] **P2-CHAT-QA-1.1** (1.5 PD) – E2E ordering send/receive
  - Spec: ARCH(4.6.3)

- [ ] **P2-CHAT-QA-1.2** (1.5 PD) – E2E unread + read receipts
  - Spec: UX(4.6.2)

---

## 2.3 Escrow Core (finance invariants locked)

### BE – Escrow Logic

- [ ] **P2-ESCROW-BE-1.1** (1.0 PD) – Create `chat_finance_sessions` + link to conversation
  - Spec: BR(3.3), DB(DATABASE_OVERVIEW)

- [ ] **P2-ESCROW-BE-1.2** (1.5 PD) – Create `chat_question_items` (main_question) + deadlines
  - Timer: `offer_expires_at`, khi accept: `reader_response_due_at` = accepted_at + 24h
  - Spec: ARCH(4.6.4)

- [ ] **P2-ESCROW-BE-1.3** (1.5 PD) – Freeze via `proc_wallet_freeze` + idempotency
  - Spec: DB(DESIGN_DECISIONS), ARCH(4.6.4)

- [ ] **P2-ESCROW-BE-1.4** (1.5 PD) – Add-question item + add_freeze
  - Spec: BR(3.3), ARCH(4.6.4)

- [ ] **P2-ESCROW-BE-1.5** (1.0 PD) – Record reply + set auto_release anchor
  - Spec: BR(19)

- [ ] **P2-ESCROW-BE-1.6** (1.5 PD) – Confirm release + `proc_wallet_release` idempotent
  - Spec: ARCH(4.6.4)

- [ ] **P2-ESCROW-BE-1.7** (1.0 PD) – Open dispute + transition disputed
  - Spec: BR(19)

### BE – Timer Jobs

- [ ] **P2-ESCROW-JOB-BE-1.1** (1.5 PD) – Job auto-refund overdue (no reply)
  - Spec: ARCH(4.6.4)

- [ ] **P2-ESCROW-JOB-BE-1.2** (1.5 PD) – Job auto-release after reply (no dispute)
  - Spec: ARCH(4.6.4)

- [ ] **P2-ESCROW-JOB-BE-1.3** (1.0 PD) – Idempotent job keys + retry-safe transitions
  - Spec: ARCH(4.6.4)

### DevOps

- [ ] **P2-ESCROW-OPS-1.1** (1.0 PD) – Deploy job runner + schedule/backoff
  - Spec: OPS(5.Reliability)

- [ ] **P2-ESCROW-OPS-1.2** (1.5 PD) – Alerts + dead-letter runbook
  - Spec: OPS(4.19), OPS(5.Reliability)

### FE

- [ ] **P2-ESCROW-FE-1.1** (1.5 PD) – Offer + accept/reject components
  - Spec: UX(4.6.2)

- [ ] **P2-ESCROW-FE-1.2** (1.0 PD) – Timer countdown UI
  - Spec: BR(19)

- [ ] **P2-ESCROW-FE-1.3** (1.0 PD) – Item status UI
  - Spec: ARCH(4.6.4)

- [ ] **P2-ESCROW-FE-1.4** (0.75 PD) – Dispute CTA + status banner
  - Spec: BR(19)

### QA

- [ ] **P2-ESCROW-QA-1.1** (2.0 PD) – Concurrency test add-question accept
  - Spec: ARCH(Testing-strategy)

- [ ] **P2-ESCROW-QA-1.2** (1.5 PD) – Idempotency test freeze/release/refund
  - Spec: DB(DESIGN_DECISIONS)

- [ ] **P2-ESCROW-QA-1.3** (1.5 PD) – Timer jobs exactly-once
  - Spec: ARCH(4.6.4)

---

## 2.4 Minimal Admin (Phase 2)

### BE

- [ ] **P2-ADMIN-BE-1.1** (1.0 PD) – Dispute queue endpoint + filters
  - Spec: OPS(4.10)

- [ ] **P2-ADMIN-BE-1.2** (1.5 PD) – Dispute resolve endpoint + audit trail
  - Spec: OPS(4.10)

- [ ] **P2-ADMIN-BE-1.3** (1.5 PD) – Payout queue: list withdrawals + approve/reject
  - Spec: BR(4.3.3), OPS(4.13.2)

### FE

- [ ] **P2-ADMIN-FE-1.1** (1.5 PD) – Admin dispute queue UI
  - Spec: OPS(4.10)

- [ ] **P2-ADMIN-FE-1.2** (1.5 PD) – Admin dispute detail + actions UI
  - Spec: OPS(4.10)

- [ ] **P2-ADMIN-FE-1.3** (1.5 PD) – Admin payout queue UI
  - Spec: BR(4.3.3)

### QA

- [ ] **P2-ADMIN-QA-1.1** (1.0 PD) – E2E dispute resolve (release)
  - Spec: BR(19)

- [ ] **P2-ADMIN-QA-1.2** (1.0 PD) – E2E dispute resolve (refund)
  - Spec: BR(19)

- [ ] **P2-ADMIN-QA-1.3** (1.0 PD) – E2E payout approve/reject + audit
  - Spec: OPS(4.10)

---

## 2.5 Withdrawal + Payout Profile

### BE

- [ ] **P2-WITHDRAW-BE-1.1** (1.5 PD) – Create withdrawal request endpoint
  - Validate: min 50 Diamond, max 1/ngày, KYC, chargeback/dispute hold
  - Spec: BR(4.3.3), DB(schema.sql)

- [ ] **P2-WITHDRAW-BE-1.2** (1.5 PD) – Debit Diamond via `proc_wallet_debit` + calculate fee 10%
  - Spec: DB(DESIGN_DECISIONS), BR(4.3.3)

- [ ] **P2-WITHDRAW-BE-1.3** (1.0 PD) – Withdrawal status transitions + admin hooks
  - Spec: BR(4.3.3)

- [ ] **P2-PAYOUT-BE-1.1** (1.0 PD) – Reader update bank info endpoint
  - Spec: UX(4.2.2), OPS(4.13.2)

- [ ] **P2-PAYOUT-BE-1.2** (0.75 PD) – Reader KYC status + enforcement
  - Spec: OPS(4.13.2)

### FE

- [ ] **P2-WITHDRAW-FE-1.1** (1.0 PD) – Reader withdrawal request UI
  - Spec: BR(4.3.3)

- [ ] **P2-WITHDRAW-FE-1.2** (0.75 PD) – Reader withdrawal history UI
  - Spec: BR(4.3.3)

- [ ] **P2-PAYOUT-FE-1.1** (1.0 PD) – Reader payout profile management UI
  - Spec: UX(4.2.2)

### QA

- [ ] **P2-WITHDRAW-QA-1.1** (1.5 PD) – E2E: min 50 Diamond, 1/day, KYC gate
  - Spec: BR(4.3.3)

- [ ] **P2-WITHDRAW-QA-1.2** (1.0 PD) – E2E: fee calculation + net_amount
  - Spec: BR(4.3.3)

---

## 2.6 MFA cho Reader/Admin

### BE

- [ ] **P2-MFA-BE-1.1** (1.5 PD) – TOTP setup + verify endpoint
  - Spec: UX(4.1.2), OPS(4.13.2)

- [ ] **P2-MFA-BE-1.2** (1.0 PD) – Enforce MFA gate cho payout/admin actions
  - Spec: UX(4.1.2), OPS(4.13.2)

### FE

- [ ] **P2-MFA-FE-1.1** (1.5 PD) – MFA setup UI (QR + backup codes)
  - Spec: UX(4.1.2)

- [ ] **P2-MFA-FE-1.2** (1.0 PD) – MFA challenge UI (trước payout/admin)
  - Spec: UX(4.1.2)

### QA

- [ ] **P2-MFA-QA-1.1** (1.0 PD) – E2E MFA setup + enforce test
  - Spec: UX(4.1.2)

---

## Tổng kết Phase 2

| Workstream | Số task | Tổng PD |
|---|---|---:|
| BE | 30 | ~33 |
| FE | 18 | ~19 |
| DB | 2 | 1.0 |
| DevOps | 4 | 4.5 |
| QA | 13 | ~17 |
| **Tổng** | **67** | **~74.5** |

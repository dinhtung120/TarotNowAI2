# Phase 1.3 – Reading Core (Daily 1 + Spread 3/5/10)

**Scope:** RNG, Reading Session, Card Collection, EXP

---

## BE – RNG

- [ ] **P1-RNG-BE-1.1** (0.5 PD) – session_nonce (CSPRNG)
- [ ] **P1-RNG-BE-1.2** (0.75 PD) – seed_digest = HMAC(...)
- [ ] **P1-RNG-BE-1.3** (1.0 PD) – Fisher-Yates deterministic shuffle
- [ ] **P1-RNG-BE-1.4** (1.0 PD) – Persist RNG audit package
- [ ] **P1-RNG-DB-1.1** (0.5 PD) – RNG audit retention (>=24m)

## BE – Reading Session

- [ ] **P1-READ-BE-1.1** (0.75 PD) – Create `reading_sessions` base
- [ ] **P1-READ-BE-1.2** (0.75 PD) – Store drawn_cards (incl. level)
- [ ] **P1-READ-BE-1.3** (0.75 PD) – Enforce daily_1 limit (UTC)
- [ ] **P1-READ-BE-2.1** (0.75 PD) – Pricing config per spread
- [ ] **P1-READ-BE-2.2** (0.75 PD) – Charge via `proc_wallet_debit`

## BE – Card Collection

- [ ] **P1-CARD-BE-1.1** (1.0 PD) – Upsert `user_collections` on draw
- [ ] **P1-CARD-BE-1.2** (0.75 PD) – Credit EXP per card based on currency type

## FE – Reading Frontend

- [ ] **P1-READ-FE-1.1** (1.0 PD) – Spread selector + question form
- [ ] **P1-READ-FE-1.2** (1.5 PD) – Card reveal animations
- [ ] **P1-READ-FE-1.3** (1.0 PD) – Cost + insufficient balance UX

## QA – Reading Tests

- [ ] **P1-READ-QA-1.1** (1.0 PD) – RNG replay property test
- [ ] **P1-READ-QA-1.2** (1.0 PD) – daily_1 limit test + error mapping
- [ ] **P1-CARD-QA-1.1** (1.0 PD) – Card collection update on draw

---

| Workstream | Tasks | PD |
|---|---:|---:|
| BE | 12 | ~8.25 |
| FE | 3 | ~3.5 |
| QA | 3 | ~3.0 |
| **Tổng** | **18** | **~14.75** |

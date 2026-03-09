# Phase 5.1 – Daily Check-in + Streak + Freeze

---

- [ ] **P5-CHK-1.1** (0.75 PD) – Check-in endpoint + idempotent
- [ ] **P5-CHK-1.2** (0.75 PD) – Credit Gold via proc + ledger
- [ ] **P5-STR-1.1** (1.0 PD) – Streak increment on valid draw (1/day UTC)
- [ ] **P5-STR-1.2** (0.75 PD) – Streak break → reset, save pre_break
- [ ] **P5-STR-1.3** (0.75 PD) – EXP multiplier: +1% per streak day
- [ ] **P5-FRZ-1.1** (1.0 PD) – Freeze price: ceil(pre_break/10) Diamond
- [ ] **P5-FRZ-1.2** (1.0 PD) – Purchase freeze: debit + restore streak
- [ ] **P5-FRZ-1.3** (0.5 PD) – Freeze window 24h enforcement
- [ ] **P5-FE-1.1** (1.5 PD) – Check-in + streak UI
- [ ] **P5-QA-1.1** (1.0 PD) – E2E: check-in → streak → break → freeze

| Tasks | PD |
|---:|---:|
| **10** | **~9.0** |

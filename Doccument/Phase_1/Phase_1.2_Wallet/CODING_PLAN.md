# Phase 1.2 – Wallet + Ledger

**Scope:** Balance API, Ledger API, Guard proc_wallet_*, Reconciliation

---

## DB – Wallet Verify

- [ ] **P1-WALLET-DB-1.1** (0.5 PD) – Verify CHECK constraints: non-negative balances
- [ ] **P1-WALLET-DB-1.2** (0.5 PD) – Verify indexes for ledger paging + reference lookup

## BE – Wallet API

- [ ] **P1-WALLET-BE-1.1** (0.5 PD) – API GET `/wallet/balance`
- [ ] **P1-WALLET-BE-1.2** (1.0 PD) – API GET `/wallet/ledger` paging + filters
- [ ] **P1-WALLET-BE-1.3** (0.75 PD) – Guard: cấm update balance trực tiếp

## FE – Wallet Frontend

- [ ] **P1-WALLET-FE-1.1** (0.5 PD) – Wallet balances widget
- [ ] **P1-WALLET-FE-1.2** (1.0 PD) – Ledger list UI (paging)

## QA & Ops

- [ ] **P1-WALLET-QA-1.1** (0.75 PD) – API tests: ledger invariant holds
- [ ] **P1-RECON-BE-1.1** (0.75 PD) – Reconciliation query `v_user_ledger_balance` vs `users`
- [ ] **P1-RECON-OPS-1.1** (0.75 PD) – Schedule reconciliation + alert channel
- [ ] **P1-RECON-QA-1.1** (0.5 PD) – Mismatch simulation test

---

| Workstream | Tasks | PD |
|---|---:|---:|
| DB | 2 | 1.0 |
| BE | 4 | ~3.0 |
| FE | 2 | ~1.5 |
| QA/Ops | 3 | ~2.0 |
| **Tổng** | **11** | **~7.5** |

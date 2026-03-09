# Phase 2.3 – Escrow Core

**Scope:** Freeze/Release/Refund, Timer jobs, Dispute, Add-question, Idempotency, Reconciliation

---

- [ ] **P2-ESC-BE-1.1** (2.0 PD) – Accept offer → freeze via `proc_wallet_freeze`
- [ ] **P2-ESC-BE-1.2** (1.5 PD) – Offer timeout job → auto-cancel/refund
- [ ] **P2-ESC-BE-1.3** (1.5 PD) – Reader reply → set auto_release_at
- [ ] **P2-ESC-BE-1.4** (1.5 PD) – User confirm → release via `proc_wallet_release` + platform fee
- [ ] **P2-ESC-BE-1.5** (1.5 PD) – Auto-release job (no dispute in 24h)
- [ ] **P2-ESC-BE-1.6** (1.5 PD) – No-reply auto-refund job (24h)
- [ ] **P2-ESC-BE-1.7** (1.5 PD) – Open dispute endpoint + window validation
- [ ] **P2-ESC-BE-1.8** (1.0 PD) – Add-question (escrow cộng dồn)
- [ ] **P2-ESC-BE-1.9** (2.0 PD) – Idempotency: double-freeze/release/refund prevention
- [ ] **P2-ESC-BE-1.10** (1.0 PD) – Frozen balance reconciliation
- [ ] **P2-ESC-FE-1.1** (2.0 PD) – In-chat escrow UI: offer/accept/timer/status
- [ ] **P2-ESC-FE-1.2** (1.0 PD) – Dispute CTA + window countdown
- [ ] **P2-ADMIN-ESC-1.1** (1.5 PD) – Admin dispute queue: resolve (release/refund)
- [ ] **P2-ESC-QA-1.1** (2.0 PD) – E2E: full escrow lifecycle
- [ ] **P2-ESC-QA-1.2** (1.5 PD) – Idempotency stress tests

---

| Tasks | PD |
|---:|---:|
| **15** | **~23.0** |

# Phase 1.6 – Legal + Profile + Deposit + Admin

**Scope:** Legal pages, Consent, Profile, Deposit 1 kênh, Promotions, Minimal Admin

---

## FE – Legal Pages

- [ ] **P1-LEGAL-FE-1.1** (1.0 PD) – TOS/Privacy/AI disclaimer pages
- [ ] **P1-LEGAL-FE-1.2** (1.0 PD) – SEO basics

## BE – Consent

- [ ] **P1-LEGAL-BE-1.1** (0.75 PD) – Persist consent events with version
- [ ] **P1-LEGAL-BE-1.2** (0.75 PD) – Enforce consent for account completion
- [ ] **P1-LEGAL-BE-1.3** (0.75 PD) – Enforce re-consent when version changes

## BE – Profile

- [ ] **P1-PROFILE-BE-1.1** (0.75 PD) – Update display_name + avatar_url
- [ ] **P1-PROFILE-BE-1.2** (0.75 PD) – Update DOB + recalc zodiac/numerology

## FE – Profile

- [ ] **P1-PROFILE-FE-1.1** (0.75 PD) – Profile edit form
- [ ] **P1-PROFILE-FE-1.2** (0.75 PD) – DOB edit + show zodiac/numerology

## BE – Deposit

- [ ] **P1-DEP-BE-1.1** (1.0 PD) – Create deposit order (pending)
- [ ] **P1-DEP-BE-1.2** (1.5 PD) – Webhook signature verification
- [ ] **P1-DEP-BE-1.3** (1.25 PD) – Webhook idempotency: prevent double-credit
- [ ] **P1-DEP-BE-1.4** (1.0 PD) – On success: credit Diamond via `proc_wallet_credit`
- [ ] **P1-DEP-BE-1.5** (0.75 PD) – FX snapshot storage (if non-VND)

## BE – Promotions

- [ ] **P1-PROMO-BE-1.1** (1.0 PD) – Admin CRUD `deposit_promotions`
- [ ] **P1-PROMO-BE-1.2** (1.0 PD) – Auto-apply promotion on deposit

## FE – Promotions

- [ ] **P1-PROMO-FE-1.1** (1.0 PD) – Admin UI deposit promotions

## DevOps – Deposit

- [ ] **P1-DEP-OPS-1.1** (0.75 PD) – WAF/allowlist for webhook
- [ ] **P1-DEP-OPS-1.2** (0.75 PD) – Retry/DLQ policy + alert

## QA – Deposit

- [ ] **P1-DEP-QA-1.1** (1.25 PD) – Webhook double-send → only one credit
- [ ] **P1-DEP-QA-1.2** (1.0 PD) – Signature invalid → reject
- [ ] **P1-DEP-QA-1.3** (0.75 PD) – Reconciliation mismatch path

## BE – Admin

- [ ] **P1-ADMIN-BE-1.1** (0.75 PD) – RBAC policy admin-only
- [ ] **P1-ADMIN-BE-1.2** (1.0 PD) – Admin: list users + lock/unlock
- [ ] **P1-ADMIN-BE-1.3** (0.75 PD) – Admin: list deposit orders

## FE – Admin

- [ ] **P1-ADMIN-FE-1.1** (0.75 PD) – Admin shell layout
- [ ] **P1-ADMIN-FE-1.2** (1.0 PD) – Users table + lock/unlock
- [ ] **P1-ADMIN-FE-1.3** (1.0 PD) – Deposit table + filters

## QA – Admin

- [ ] **P1-ADMIN-QA-1.1** (0.75 PD) – RBAC E2E: non-admin blocked
- [ ] **P1-ADMIN-QA-1.2** (0.75 PD) – E2E: lock/unlock reflected

---

| Workstream | Tasks | PD |
|---|---:|---:|
| BE | 15 | ~13.75 |
| FE | 7 | ~6.25 |
| DevOps | 2 | ~1.5 |
| QA | 5 | ~4.5 |
| **Tổng** | **29** | **~26.0** |

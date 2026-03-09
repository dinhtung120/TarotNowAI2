# Phase 1.1 – Auth Baseline

**Nguồn:** PHASE_1_MVP.md Section 1.1  
**Scope:** Register, Login, JWT, OTP, Password Reset, Consent, Age Gate

---

## BE – Auth Backend

- [ ] **P1-AUTH-BE-1.1** (1.0 PD) – Endpoint register: password policy + unique email/username
  - Spec: UX(4.1.1)

- [ ] **P1-AUTH-BE-1.2** (0.75 PD) – Password hashing Argon2id + verify
  - Spec: UX(4.1.1)

- [ ] **P1-AUTH-BE-1.3** (1.25 PD) – Issue JWT access + refresh rotation (happy path)
  - Spec: BR(Phase-1.1), ARCH(4.1.5)

- [ ] **P1-AUTH-BE-1.4** (1.5 PD) – Refresh rotation + reuse detection → revoke chain
  - Spec: UX(4.1.1)

- [ ] **P1-AUTH-BE-1.5** (1.0 PD) – Revoke session + revoke-all
  - Spec: UX(4.1.1)

## BE – OTP & Email Verify

- [ ] **P1-AUTH-BE-2.1** (1.0 PD) – Create OTP + store `email_otps` + email send hook
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-BE-2.2** (1.0 PD) – Verify OTP (expiry + one-time)
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-BE-2.3** (0.75 PD) – Verify success → credit +5 Gold via proc
  - Spec: BR(3.1), DB(DESIGN_DECISIONS)

## BE – Password Reset

- [ ] **P1-AUTH-BE-3.1** (0.75 PD) – Forgot password: generate reset token
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-BE-3.2** (1.0 PD) – Reset password: validate token + invalidate sessions
  - Spec: UX(4.1.3)

## BE – Consent & Age Gate

- [ ] **P1-AUTH-BE-4.1** (0.75 PD) – Consent enforcement gate
  - Spec: OPS(4.13.1)

- [ ] **P1-AUTH-BE-4.2** (0.5 PD) – Age gate 18+
  - Spec: OPS(4.13.1), UX(4.2.1)

## FE – Auth Frontend

- [ ] **P1-AUTH-FE-1.1** (1.5 PD) – Register UI: consent + DOB
  - Spec: OPS(4.13.1), UX(4.1.1)

- [ ] **P1-AUTH-FE-1.2** (1.0 PD) – Verify OTP UI
  - Spec: UX(4.1.3)

- [ ] **P1-AUTH-FE-1.3** (1.0 PD) – Login UI + error mapping
  - Spec: UX(4.15.3)

- [ ] **P1-AUTH-FE-1.4** (1.0 PD) – Forgot/reset password UI
  - Spec: UX(4.1.3)

## QA – Auth Tests

- [ ] **P1-AUTH-QA-1.1** (1.0 PD) – E2E: register → verify → +5 Gold
  - Spec: BR(3.1)

- [ ] **P1-AUTH-QA-1.2** (1.0 PD) – E2E: refresh reuse → revoke chain
  - Spec: UX(4.1.1)

---

| Workstream | Tasks | PD |
|---|---:|---:|
| BE | 12 | ~10.25 |
| FE | 4 | ~4.5 |
| QA | 2 | ~2.0 |
| **Tổng** | **18** | **~16.75** |

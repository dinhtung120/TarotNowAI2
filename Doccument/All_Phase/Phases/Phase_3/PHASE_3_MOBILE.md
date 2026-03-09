# Phase 3 – Mobile Parity (React Native / Expo)

**Nguồn:** `CODING_PLAN.md` Section 6  
**Mục tiêu:** Ứng dụng mobile parity web: auth, wallet, reading, chat, notifications, deep-link.

---

## Quy ước

- **PD** = person-days | Spec: `BR` = business-rules, `UX` = product-ux, `ARCH` = tech-architecture, `OPS` = ops-security

---

## Task Checklist

### FE – Mobile Scaffold & Auth

- [ ] **P3-MOB-FE-1.1** (2.0 PD) – Expo scaffold + navigation skeleton
  - Dựng app RN/Expo + navigation routes nền
  - Spec: BR(Phase-3.1)

- [ ] **P3-MOB-FE-1.2** (2.0 PD) – Secure storage refresh token + device binding
  - Lưu refresh token vào Keychain/Keystore + device binding cơ bản
  - Spec: ARCH(4.1.5)

- [ ] **P3-MOB-FE-1.3** (3.0 PD) – Auth screens parity
  - Màn login/register/verify OTP parity web; error mapping thống nhất
  - Spec: UX(2.7)

### FE – Wallet & Reading Parity

- [ ] **P3-MOB-FE-2.1** (2.5 PD) – Wallet parity (balance + ledger)
  - Spec: BR(Phase-3.2)

- [ ] **P3-MOB-FE-2.2** (3.0 PD) – Reading parity + animations baseline
  - Spec: BR(Phase-3.2)

- [ ] **P3-MOB-FE-2.3** (2.5 PD) – SSE streaming client parity
  - Client SSE stream token AI, xử lý reconnect và completion
  - Spec: BR(Phase-3.2), ARCH(4.4.3)

### FE – Chat Parity

- [ ] **P3-MOB-FE-3.1** (4.0 PD) – SignalR client parity + reconnect lifecycle
  - Xử lý app background/foreground reconnect
  - Spec: BR(Phase-3.3)

- [ ] **P3-MOB-FE-3.2** (4.0 PD) – Chat escrow UI parity
  - UI escrow parity web: offer/accept/timers/status/dispute
  - Spec: BR(Phase-3.3)

### DevOps – Push Notifications

- [ ] **P3-MOB-OPS-1.1** (3.0 PD) – Push notifications setup + envs
  - FCM/APNs + cấu hình env + keys
  - Spec: BR(Phase-3.4), OPS(5.Security)

- [ ] **P3-MOB-OPS-1.2** (2.0 PD) – Deep-link routing from push
  - Mở đúng màn (chat/reading/wallet) từ push
  - Spec: BR(Phase-3.4)

### QA – Mobile Tests

- [ ] **P3-MOB-QA-1.1** (3.0 PD) – Mobile parity checklist + smoke
  - Checklist + smoke flow auth/wallet/reading/chat trên iOS/Android
  - Spec: UX(2.7)

- [ ] **P3-MOB-QA-1.2** (3.0 PD) – Regression reconnect + refresh edge cases
  - Test reconnect + refresh token rotation edge cases trên mobile
  - Spec: ARCH(4.1.5)

---

## Tổng kết Phase 3

| Workstream | Số task | Tổng PD |
|---|---|---:|
| FE | 8 | 23.0 |
| DevOps | 2 | 5.0 |
| QA | 2 | 6.0 |
| **Tổng** | **12** | **34.0** |

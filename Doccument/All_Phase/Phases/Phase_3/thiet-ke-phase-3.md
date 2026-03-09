# Phase 3 – Tài liệu thiết kế liên quan

**Mục đích:** Trích các phần thiết kế cần đọc khi làm Phase 3 (Mobile Parity).  
**Nguồn gốc:** Trích từ `01-business-rules.md`, `02-product-ux-specs.md`, `03-tech-architecture.md`

---

## 1. Quy tắc kinh doanh Phase 3 (từ BR)

### Nguyên tắc chính
- **Ra mobile nhanh**: chỉ parity cốt lõi, không mở rộng tính năng mới
- Cốt lõi trước mobile: Auth, Ví, Reading AI, Chat escrow, Nạp tiền, Pháp lý

### 3.1 Mobile Scaffold + Auth (BR Phase 3.1)
- React Native (Expo) + TypeScript
- Auth tương đương web: JWT, refresh rotation, secure storage
- Device binding cho refresh token

### 3.2 Wallet + Reading Parity (BR Phase 3.2)
- Xem số dư ví, sổ cái
- Trải bài 1/3/5/10 lá + AI streaming (SSE)

### 3.3 Chat Parity (BR Phase 3.3)
- Chat 1-1 với reader
- Escrow trong chat
- SignalR reconnect lifecycle (app background/foreground)

### 3.4 Notifications + Deep-link (BR Phase 3.4)
- Push notification cơ bản (FCM/APNs)
- Deep-link từ push → mở đúng màn (chat/reading/wallet)

---

## 2. Đặc tả UX Mobile (từ UX)

### 2.1 Mobile-first Priorities (UX 2.7)
- Deep-link từ push notification
- App lifecycle reconnect cho SignalR/SSE
- Mọi API dùng ở mobile phải có versioned contract
- Backward compatibility test

### 2.2 Auth UX Mobile
- Secure storage: Keychain (iOS) / Keystore (Android)
- Biometric login (optional future)
- Login/register/verify OTP parity web

### 2.3 Reading UX Mobile
- Card reveal animations baseline
- SSE streaming client parity
- Error state handling khi network unstable

### 2.4 Chat UX Mobile
- SignalR reconnect khi app ra/vào background
- Push notification khi có tin nhắn mới
- Read receipts + unread badges parity

---

## 3. Kiến trúc kỹ thuật Mobile (từ ARCH)

### 3.1 Mobile Stack
- React Native + Expo + TypeScript
- Shared API contract với web

### 3.2 Secure Token Storage
- Refresh token lưu Keychain/Keystore (không AsyncStorage)
- Device binding basic

### 3.3 Realtime Connections
- SignalR client: handle connect/disconnect/reconnect
- SSE client: handle streaming + completion detection
- Network state awareness: auto-reconnect khi online

### 3.4 Push Notifications
- FCM (Android) + APNs (iOS)
- Deep-link routing: chat, reading, wallet
- Notification payload: không chứa sensitive data

---

## 4. Không cần đọc trong Phase 3

> Phase 3 là **parity** — mọi business logic, escrow, wallet invariants đã implement ở Phase 1-2. Mobile chỉ cần implement UI và transport layer tương ứng.

---

## Tham chiếu đầy đủ

| Tài liệu | Sections liên quan |
|---|---|
| [01-business-rules.md](../All_Phase/tai-lieu-thiet-ke/01-business-rules.md) | Phase 3 (3.1–3.4) |
| [02-product-ux-specs.md](../All_Phase/tai-lieu-thiet-ke/02-product-ux-specs.md) | 2.7 (Mobile parity) |
| [03-tech-architecture.md](../All_Phase/tai-lieu-thiet-ke/03-tech-architecture.md) | 4.1.5 (Auth mobile), Stack |

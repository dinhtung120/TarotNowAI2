# 📋 Tổng Hợp API & Trang Web — TarotNowAI2

> **Cập nhật lần cuối:** 2026-03-21  
> **Mục đích:** Liệt kê toàn bộ API endpoints (Backend) và trang web (Frontend), kèm trạng thái tích hợp.

**Chú thích:**
- ✅ = API đã được gọi từ Frontend
- ❌ = API **chưa** được gọi từ Frontend
- 📍 = Nơi gọi API trong Frontend (file action hoặc component)

---

## 1. 🔐 AuthController — `api/v1/auth`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 1 | `POST` | `/auth/register` | Đăng ký tài khoản mới | ✅ | 📍 [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) |
| 2 | `POST` | `/auth/login` | Đăng nhập, lấy JWT + Refresh Token | ✅ | 📍 [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) |
| 3 | `POST` | `/auth/refresh` | Xoay vòng Refresh Token | ❌ | — |
| 4 | `POST` | `/auth/logout` | Đăng xuất, thu hồi Refresh Token | ✅ | 📍 [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) |
| 5 | `POST` | `/auth/send-verification-email` | Gửi OTP xác thực email | ❌ | — |
| 6 | `POST` | `/auth/verify-email` | Xác thực OTP kích hoạt tài khoản | ✅ | 📍 [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) |
| 7 | `POST` | `/auth/forgot-password` | Gửi OTP đặt lại mật khẩu | ✅ | 📍 [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) |
| 8 | `POST` | `/auth/reset-password` | Đặt lại mật khẩu mới | ✅ | 📍 [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) |

---

## 2. 👤 ProfileController — `api/v1/profile`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 9 | `GET` | `/profile` | Lấy thông tin hồ sơ cá nhân | ✅ | 📍 [profileActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/profileActions.ts) |
| 10| `PATCH` | `/profile` | Cập nhật hồ sơ (tên, avatar, ngày sinh) | ✅ | 📍 [profileActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/profileActions.ts) |

---

## 3. 💰 WalletController — `api/v1/wallet`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 11| `GET` | `/Wallet/balance` | Lấy số dư ví (Gold, Diamond) | ✅ | 📍 [walletActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/walletActions.ts) |
| 12| `GET` | `/Wallet/ledger` | Lịch sử giao dịch ví (phân trang) | ✅ | 📍 [walletActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/walletActions.ts) |

---

## 4. 💳 DepositController — `api/v1/deposits`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 13| `POST` | `/deposits/orders` | Tạo đơn nạp tiền | ✅ | 📍 [depositActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/depositActions.ts) |
| 14| `POST` | `/deposits/webhook/vnpay` | Webhook nhận callback từ payment gateway | ❌ | Server-to-server (không gọi từ FE) |

---

## 5. 🃏 TarotController — `api/v1/reading`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 15| `POST` | `/reading/init` | Khởi tạo phiên rút bài Tarot | ✅ | 📍 [readingActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readingActions.ts) |
| 16| `POST` | `/reading/reveal` | Lật bài (chốt RNG, trả kết quả) | ✅ | 📍 [readingActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readingActions.ts) |
| 17| `GET` | `/reading/collection` | Xem bộ sưu tập bài đã thu thập | ✅ | 📍 [collectionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/collectionActions.ts) |

---

## 6. 🤖 AiController — `api/v1/sessions`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 18| `GET` | `/sessions/{sessionId}/stream` | SSE stream kết quả AI (Typewriter) | ✅ | 📍 [AiInterpretationStream.tsx](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/components/AiInterpretationStream.tsx) (EventSource) |

---

## 7. 📜 HistoryController — `api/v1/history`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 19| `GET` | `/history/sessions` | Danh sách lịch sử bốc bài (phân trang) | ✅ | 📍 [historyActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/historyActions.ts) |
| 20| `GET` | `/history/sessions/{id}` | Chi tiết phiên bốc bài + chat AI | ✅ | 📍 [historyActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/historyActions.ts) |
| 21| `GET` | `/History/admin/all-sessions` | [Admin] Tất cả lịch sử bốc bài | ✅ | 📍 [historyActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/historyActions.ts) |

---

## 8. 📖 ReaderController — `api/v1/reader`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 22| `POST` | `/reader/apply` | Gửi đơn xin làm Reader | ✅ | 📍 [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) |
| 23| `GET` | `/reader/my-request` | Xem trạng thái đơn xin Reader | ✅ | 📍 [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) |
| 24| `GET` | `/reader/profile/{userId}` | Xem hồ sơ Reader (public) | ✅ | 📍 [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) |
| 25| `PATCH` | `/reader/profile` | Reader cập nhật hồ sơ | ✅ | 📍 [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) |
| 26| `PATCH` | `/reader/status` | Reader chuyển trạng thái online/offline | ✅ | 📍 [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) |
| 27| `GET` | `/readers` | Danh sách Reader công khai (directory) | ✅ | 📍 [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) |

---

## 9. 💬 ConversationController — `api/v1/conversations`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 28| `POST` | `/conversations` | Tạo conversation mới | ✅ | 📍 [chatActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/chatActions.ts) |
| 29| `GET` | `/conversations` | Inbox — danh sách conversations | ✅ | 📍 [chatActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/chatActions.ts) |
| 30| `GET` | `/conversations/{id}/messages` | Lịch sử tin nhắn | ✅ | 📍 [chatActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/chatActions.ts) |

> **Lưu ý:** Realtime messaging qua **SignalR Hub** tại `/api/v1/chat` — kết nối trong [chat/[id]/page.tsx](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/%5Blocale%5D/chat/%5Bid%5D/page.tsx)

---

## 10. 🔒 EscrowController — `api/v1/escrow`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 31| `POST` | `/escrow/accept` | Accept offer — freeze diamond | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |
| 32| `POST` | `/escrow/reply` | Reader trả lời → set auto_release | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |
| 33| `POST` | `/escrow/confirm` | User confirm → release diamond | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |
| 34| `POST` | `/escrow/dispute` | Mở tranh chấp | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |
| 35| `POST` | `/escrow/add-question` | Thêm câu hỏi — freeze thêm diamond | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |
| 36| `GET` | `/escrow/{conversationId}` | Lấy trạng thái escrow | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |

---

## 11. 🔑 MfaController — `api/v1/mfa`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 37| `POST` | `/mfa/setup` | Lấy thông tin thiết lập MFA (URI+Secret) | ✅ | 📍 [mfaActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/mfaActions.ts) |
| 38| `POST` | `/mfa/verify` | Verify TOTP để bật MFA | ✅ | 📍 [mfaActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/mfaActions.ts) |
| 39| `POST` | `/mfa/challenge` | Xác thực MFA cho hành động nhạy cảm | ✅ | 📍 [mfaActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/mfaActions.ts) |
| 40| `GET` | `/mfa/status` | Lấy trạng thái MFA | ✅ | 📍 [mfaActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/mfaActions.ts) |

---

## 12. ⚖️ LegalController — `api/v1/legal`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 41| `GET` | `/legal/consent-status` | Kiểm tra trạng thái đồng ý pháp lý | ✅ | 📍 [legalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/legalActions.ts) |
| 42| `POST` | `/legal/consent` | Ghi nhận đồng ý pháp lý | ✅ | 📍 [legalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/legalActions.ts) |

---

## 13. 🚨 ReportController — `api/v1/reports`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 43| `POST` | `/reports` | Tạo báo cáo vi phạm | ✅ | 📍 [chatActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/chatActions.ts) |

---

## 14. 💸 WithdrawalController — `api/v1/withdrawal`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 44| `POST` | `/withdrawal/create` | Reader tạo yêu cầu rút tiền | ✅ | 📍 [withdrawalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/withdrawalActions.ts) |
| 45| `GET` | `/withdrawal/my` | Reader xem lịch sử rút tiền | ✅ | 📍 [withdrawalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/withdrawalActions.ts) |

---

## 15. 🛡️ AdminController — `api/v1/admin`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 46| `GET` | `/admin/reconciliation/wallet` | Kiểm tra bất đồng bộ sổ cái | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 47| `GET` | `/admin/users` | Danh sách users (phân trang) | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 48| `PATCH` | `/admin/users` | Cập nhật thông tin user | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 48.1| `PATCH`| `/admin/users/lock`| Khóa/Mở khóa user | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 49| `GET` | `/admin/deposits` | Danh sách đơn nạp tiền | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts), [depositActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/depositActions.ts) |
| 50| `POST` | `/admin/users/add-balance` | Cộng tiền cho user | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 51| `PATCH` | `/admin/deposits/process` | Phê duyệt/Từ chối đơn nạp tiền | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 52| `GET` | `/admin/reader-requests` | Danh sách đơn xin Reader | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 53| `PATCH` | `/admin/reader-requests/process` | Phê duyệt/Từ chối đơn xin Reader | ✅ | 📍 [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) |
| 54| `POST` | `/admin/escrow/resolve-dispute` | Admin giải quyết tranh chấp escrow | ✅ | 📍 [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) |
| 55| `GET` | `/admin/withdrawals/queue` | Danh sách yêu cầu rút tiền pending | ✅ | 📍 [withdrawalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/withdrawalActions.ts) |
| 56| `POST` | `/admin/withdrawals/process` | Admin approve/reject rút tiền | ✅ | 📍 [withdrawalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/withdrawalActions.ts) |

---

## 16. 🎁 PromotionsController — `api/v1/admin/promotions`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 57| `GET` | `/admin/promotions` | Danh sách khuyến mãi | ✅ | 📍 [promotionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/promotionActions.ts), [promotions/page.tsx](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/%5Blocale%5D/admin/promotions/page.tsx) |
| 58| `POST` | `/admin/promotions` | Tạo khuyến mãi mới | ✅ | 📍 [promotionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/promotionActions.ts), [promotions/page.tsx](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/%5Blocale%5D/admin/promotions/page.tsx) |
| 59| `PUT` | `/admin/promotions/{id}` | Cập nhật khuyến mãi | ✅ | 📍 [promotionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/promotionActions.ts), [promotions/page.tsx](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/%5Blocale%5D/admin/promotions/page.tsx) |
| 60| `DELETE` | `/admin/promotions/{id}` | Xóa khuyến mãi | ✅ | 📍 [promotionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/promotionActions.ts), [promotions/page.tsx](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/%5Blocale%5D/admin/promotions/page.tsx) |

---

## 17. 🏥 HealthController — `api/v1/health`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 61| `GET` | `/health` | Health check | ❌ | Dùng cho monitoring, không cần FE |
| 62| `GET` | `/health/error-test` | Test ProblemDetails error handling | ❌ | Endpoint debug, không cần FE |

---

## 18. 🔧 DiagController — `api/v1/diag`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 63| `POST` | `/diag/wipe` | Xóa dữ liệu (dev only) | ❌ | Endpoint dev/debug |
| 64| `GET` | `/diag/seed-admin` | Tạo SuperAdmin | ❌ | Endpoint dev/debug |
| 65| `GET` | `/diag/stats` | Thống kê MongoDB | ❌ | Endpoint dev/debug |

---

## 19. 🔔 NotificationController — `api/v1/notification`

| # | Method | Endpoint | Mô tả | Tích hợp | Nơi gọi |
|---|--------|----------|--------|----------|---------|
| 66| `GET` | `/notification` | Lấy danh sách thông báo | ✅ | 📍 [notificationActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/notificationActions.ts) |
| 67| `GET` | `/notification/unread-count` | Lấy số lượng thông báo chưa đọc | ✅ | 📍 [notificationActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/notificationActions.ts) |
| 68| `PATCH` | `/notification/{id}/read` | Đánh dấu thông báo đã đọc | ✅ | 📍 [notificationActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/notificationActions.ts) |

---

## 📊 Thống Kê Tích Hợp

| Metric | Số lượng |
|--------|----------|
| **Tổng số API endpoints** | **69** |
| ✅ Đã tích hợp Frontend | **62** |
| ❌ Chưa tích hợp | **7** |
| **Tỷ lệ tích hợp** | **89.8%** |

### API Chưa Tích Hợp (Chi tiết)

| # | API | Lý do |
|---|-----|-------|
| 1 | `POST /auth/refresh` | ⚠️ **Cần tích hợp** — Cần cho việc tự động làm mới token khi hết hạn |
| 2 | `POST /auth/send-verification-email` | ⚠️ **Cần tích hợp** — Trang verify-email cần nút "Gửi lại OTP" |
| 3 | `POST /deposits/webhook/vnpay` | ✔️ OK — Server-to-server webhook, không cần gọi từ FE |
| 4 | `GET /health` | ✔️ OK — Endpoint monitoring/DevOps |
| 5 | `GET /health/error-test` | ✔️ OK — Endpoint debug |
| 6 | `POST /diag/wipe` | ✔️ OK — Endpoint dev/debug |
| 7 | `GET /diag/seed-admin` | ✔️ OK — Endpoint dev/debug |
| 8 | `GET /diag/stats` | ✔️ OK — Endpoint dev/debug |

> ⚠️ **Lưu ý quan trọng:** 2 API auth (`refresh` và `send-verification-email`) nên được tích hợp vào Frontend để hoàn thiện trải nghiệm người dùng.

---

## 🌐 Danh Sách Trang Web (Frontend — Next.js App Router)

### Trang Công Khai (Public)

| # | Route | Mô tả | API Sử Dụng |
|---|-------|--------|-------------|
| 1 | `/[locale]/` | Trang chủ (Home) | — |
| 2 | `/[locale]/readers` | Danh sách Reader | `GET /readers` |
| 3 | `/[locale]/readers/[id]` | Hồ sơ Reader chi tiết | `GET /reader/profile/{userId}` |
| 4 | `/[locale]/legal/tos` | Điều khoản sử dụng | — (static) |
| 5 | `/[locale]/legal/privacy` | Chính sách bảo mật | — (static) |
| 6 | `/[locale]/legal/ai-disclaimer` | Tuyên bố miễn trừ AI | — (static) |

### Trang Xác Thực (Auth)

| # | Route | Mô tả | API Sử Dụng |
|---|-------|--------|-------------|
| 7 | `/[locale]/login` | Đăng nhập | `POST /auth/login` |
| 8 | `/[locale]/register` | Đăng ký | `POST /auth/register` |
| 9 | `/[locale]/verify-email` | Xác thực email (OTP) | `POST /auth/verify-email` |
| 10| `/[locale]/forgot-password` | Quên mật khẩu | `POST /auth/forgot-password` |
| 11| `/[locale]/reset-password` | Đặt lại mật khẩu | `POST /auth/reset-password` |

### Trang Người Dùng (Authenticated)

| # | Route | Mô tả | API Sử Dụng |
|---|-------|--------|-------------|
| 12| `/[locale]/profile` | Hồ sơ cá nhân | `GET/PATCH /profile` |
| 13| `/[locale]/profile/mfa` | Quản lý MFA | `GET /mfa/status`, `POST /mfa/setup`, `POST /mfa/verify` |
| 14| `/[locale]/profile/reader` | Hồ sơ Reader (của mình) | `PATCH /reader/profile`, `PATCH /reader/status` |
| 15| `/[locale]/reader/apply` | Đăng ký làm Reader | `POST /reader/apply`, `GET /reader/my-request` |
| 16| `/[locale]/reading` | Rút bài Tarot | `POST /reading/init`, `POST /reading/reveal` |
| 17| `/[locale]/reading/session/[id]` | Phiên bốc bài + AI stream | `GET /sessions/{id}/stream` (SSE) |
| 18| `/[locale]/reading/history` | Lịch sử bốc bài | `GET /history/sessions` |
| 19| `/[locale]/reading/history/[id]` | Chi tiết phiên bốc bài | `GET /history/sessions/{id}` |
| 20| `/[locale]/collection` | Bộ sưu tập bài Tarot | `GET /reading/collection` |
| 21| `/[locale]/wallet` | Ví người dùng | `GET /Wallet/balance`, `GET /Wallet/ledger` |
| 22| `/[locale]/wallet/deposit` | Nạp tiền | `POST /deposits/orders`, `GET /legal/consent-status`, `POST /legal/consent` |
| 23| `/[locale]/wallet/withdraw` | Rút tiền | `POST /withdrawal/create`, `GET /withdrawal/my` |
| 24| `/[locale]/chat` | Inbox (danh sách cuộc hội thoại) | `GET /conversations` |
| 25| `/[locale]/chat/[id]` | Phòng chat (SignalR realtime) | `GET /conversations/{id}/messages`, SignalR Hub |
| 26| `/[locale]/notifications` | Danh sách thông báo | `GET /notification`, `PATCH /notification/{id}/read` |

### Trang Admin

| # | Route | Mô tả | API Sử Dụng |
|---|-------|--------|-------------|
| 27| `/[locale]/admin` | Dashboard Admin | — |
| 28| `/[locale]/admin/users` | Quản lý người dùng | `GET /admin/users`, `PATCH /admin/users/lock`, `PATCH /admin/users`, `POST /admin/users/add-balance` |
| 29| `/[locale]/admin/deposits` | Quản lý nạp tiền | `GET /admin/deposits`, `PATCH /admin/deposits/process` |
| 30| `/[locale]/admin/reader-requests` | Quản lý đơn xin Reader | `GET /admin/reader-requests`, `PATCH /admin/reader-requests/process` |
| 31| `/[locale]/admin/readings` | Lịch sử bốc bài (all users) | `GET /History/admin/all-sessions` |
| 32| `/[locale]/admin/promotions` | Quản lý khuyến mãi | `GET/POST/PUT/DELETE /admin/promotions` |
| 33| `/[locale]/admin/withdrawals` | Quản lý rút tiền | `GET /admin/withdrawals/queue`, `POST /admin/withdrawals/process` |
| 34| `/[locale]/admin/disputes` | Quản lý tranh chấp | `POST /admin/escrow/resolve-dispute` |

---

## 📁 Cấu Trúc Frontend Actions

| File | Mô tả | Số API Calls |
|------|--------|-------------|
| [authActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/authActions.ts) | Xác thực (login, register, logout...) | 6 |
| [profileActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/profileActions.ts) | Hồ sơ cá nhân | 2 |
| [walletActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/walletActions.ts) | Ví (số dư, ledger) | 2 |
| [depositActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/depositActions.ts) | Nạp tiền | 2 |
| [readingActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readingActions.ts) | Rút bài Tarot | 2 |
| [collectionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/collectionActions.ts) | Bộ sưu tập bài | 1 |
| [historyActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/historyActions.ts) | Lịch sử bốc bài | 3 |
| [readerActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/readerActions.ts) | Tính năng Reader | 6 |
| [chatActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/chatActions.ts) | Chat + Report | 4 |
| [escrowActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/escrowActions.ts) | Escrow + Dispute | 7 |
| [mfaActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/mfaActions.ts) | Xác thực 2 lớp MFA | 4 |
| [legalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/legalActions.ts) | Pháp lý (consent) | 2 |
| [notificationActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/notificationActions.ts) | Thông báo (in-app) | 3 |
| [promotionActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/promotionActions.ts) | Khuyến mãi | 4 |
| [withdrawalActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/withdrawalActions.ts) | Rút tiền | 4 |
| [adminActions.ts](file:///Users/lucifer/Desktop/TarotNowAI2/Frontend/src/actions/adminActions.ts) | Admin (users, deposits, reader-requests) | 9 |

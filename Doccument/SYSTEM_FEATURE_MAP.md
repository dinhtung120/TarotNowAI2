# 🗺️ TarotNowAI2 — BẢN ĐỒ HỆ THỐNG THEO TÍNH NĂNG

> **Cập nhật:** 2026-03-19  
> **Mục đích:** Tài liệu tổng hợp duy nhất — đọc file này là nắm toàn bộ dự án. Khi lỗi ở đâu → khoanh vùng ngay.

---

## 📐 KIẾN TRÚC TỔNG QUAN

```
┌─────────────────────────────────────────────────────────────────────┐
│  FRONTEND (Next.js 15 App Router + TypeScript)                      │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌────────┐  ┌────────┐ │
│  │  Pages    │→ │Components│→ │ Actions  │→ │  Lib   │→ │ Store  │ │
│  │ (Route)   │  │ (UI)     │  │ (API call)│  │(api.ts)│  │(Zustand)│ │
│  └──────────┘  └──────────┘  └──────────┘  └────────┘  └────────┘ │
└────────────────────────────────┬────────────────────────────────────┘
                                 │ HTTP / SSE / SignalR
┌────────────────────────────────▼────────────────────────────────────┐
│  BACKEND (.NET 8 Clean Architecture)                                │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────┐  ┌────────────┐ │
│  │Controllers│→ │ Application  │→ │ Domain       │  │Infrastructure│
│  │ (API)     │  │ (CQRS/MediatR)│  │ (Entities)   │  │(EF/Mongo/  │ │
│  │ +Hub      │  │ +Validators  │  │ +Enums       │  │ Redis/AI)  │ │
│  └──────────┘  └──────────────┘  └──────────────┘  └────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
         │                    │                    │
    PostgreSQL            MongoDB              Redis
   (ACID/Wallet)    (Documents/Chat)      (Cache/Rate Limit)
```

### Quy ước đọc tài liệu

| Ký hiệu | Ý nghĩa |
|----------|----------|
| `→` | Luồng dữ liệu đi từ trái sang phải |
| **BE:** | File thuộc Backend |
| **FE:** | File thuộc Frontend |
| 🟢 | Đã tích hợp FE↔BE |
| 🔴 | Chưa tích hợp |

---

## 1. 🔐 XÁC THỰC & TÀI KHOẢN (Authentication)

### 1.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 1 | POST | `/auth/register` | Đăng ký tài khoản | 🟢 |
| 2 | POST | `/auth/login` | Đăng nhập, JWT + Refresh Token | 🟢 |
| 3 | POST | `/auth/refresh` | Xoay vòng Refresh Token | 🔴 |
| 4 | POST | `/auth/logout` | Đăng xuất, thu hồi token | 🟢 |
| 5 | POST | `/auth/send-verification-email` | Gửi OTP xác thực email | 🔴 |
| 6 | POST | `/auth/verify-email` | Xác thực OTP kích hoạt | 🟢 |
| 7 | POST | `/auth/forgot-password` | Gửi OTP đặt lại mật khẩu | 🟢 |
| 8 | POST | `/auth/reset-password` | Đặt lại mật khẩu mới | 🟢 |

### 1.2 Luồng xử lý

```
ĐĂNG KÝ:
FE: register/page.tsx → authActions.ts → lib/api.ts
    → BE: AuthController.Register → RegisterCommand → RegisterCommandHandler
    → UserRepository → User entity (PostgreSQL)
    → EmailOtpRepository → EmailOtp entity → MockEmailSender

ĐĂNG NHẬP:
FE: login/page.tsx → authActions.ts → lib/api.ts
    → BE: AuthController.Login → LoginCommand → LoginCommandHandler
    → UserRepository (xác minh) → PasswordHasher (Argon2id) → TokenService (JWT)
    → RefreshTokenRepository → RefreshToken entity
    ← AuthResponse (accessToken + refreshToken)
FE: authStore.ts lưu token → AuthSessionManager.tsx quản lý session

XÁC THỰC EMAIL:
FE: verify-email/page.tsx → authActions.ts
    → BE: AuthController.VerifyEmail → VerifyEmailCommand → VerifyEmailCommandHandler
    → EmailOtpRepository (kiểm OTP) → UserRepository (cập nhật EmailVerified)

QUÊN MẬT KHẨU:
FE: forgot-password/page.tsx → authActions.ts
    → BE: AuthController.ForgotPassword → ForgotPasswordCommand → ForgotPasswordCommandHandler
    → UserRepository → EmailOtpRepository → MockEmailSender

ĐẶT LẠI MẬT KHẨU:
FE: reset-password/page.tsx → authActions.ts
    → BE: AuthController.ResetPassword → ResetPasswordCommand → ResetPasswordCommandHandler
    → EmailOtpRepository (verify OTP) → UserRepository + PasswordHasher (update password)
```

### 1.3 File liên quan

**Backend:**
| File | Layer | Tác dụng |
|------|-------|----------|
| `Controllers/AuthController.cs` | API | Routing 8 endpoints xác thực |
| `Features/Auth/Commands/Register/*` | Application | Command + Handler + Validator đăng ký |
| `Features/Auth/Commands/Login/*` | Application | Command + Handler + Validator + AuthResponse đăng nhập |
| `Features/Auth/Commands/RefreshToken/*` | Application | Command + Handler xoay token |
| `Features/Auth/Commands/RevokeToken/*` | Application | Command + Handler thu hồi token |
| `Features/Auth/Commands/SendEmailVerificationOtp/*` | Application | Command + Handler gửi OTP email |
| `Features/Auth/Commands/VerifyEmail/*` | Application | Command + Handler xác thực email |
| `Features/Auth/Commands/ForgotPassword/*` | Application | Command + Handler quên mật khẩu |
| `Features/Auth/Commands/ResetPassword/*` | Application | Command + Handler đặt lại mật khẩu |
| `Interfaces/IUserRepository.cs` | Application | Interface truy cập User |
| `Interfaces/IRefreshTokenRepository.cs` | Application | Interface truy cập RefreshToken |
| `Interfaces/IEmailOtpRepository.cs` | Application | Interface truy cập EmailOtp |
| `Interfaces/IPasswordHasher.cs` | Application | Interface hash mật khẩu |
| `Interfaces/ITokenService.cs` | Application | Interface tạo/xác minh JWT |
| `Interfaces/IEmailSender.cs` | Application | Interface gửi email |
| `Domain/Entities/User.cs` | Domain | Entity người dùng |
| `Domain/Entities/RefreshToken.cs` | Domain | Entity refresh token |
| `Domain/Entities/EmailOtp.cs` | Domain | Entity OTP email |
| `Domain/Enums/UserRole.cs` | Domain | Enum vai trò |
| `Domain/Enums/UserStatus.cs` | Domain | Enum trạng thái user |
| `Domain/Enums/OtpType.cs` | Domain | Enum loại OTP |
| `Persistence/Repositories/UserRepository.cs` | Infrastructure | Truy cập DB User (PostgreSQL) |
| `Persistence/Repositories/RefreshTokenRepository.cs` | Infrastructure | Truy cập DB RefreshToken |
| `Persistence/Repositories/EmailOtpRepository.cs` | Infrastructure | Truy cập DB EmailOtp |
| `Security/Argon2idPasswordHasher.cs` | Infrastructure | Hash Argon2id |
| `Security/JwtTokenService.cs` | Infrastructure | Tạo JWT |
| `Services/MockEmailSender.cs` | Infrastructure | Gửi email giả lập |
| `Persistence/Configurations/UserConfiguration.cs` | Infrastructure | EF Config User |
| `Persistence/Configurations/RefreshTokenConfiguration.cs` | Infrastructure | EF Config RefreshToken |
| `Persistence/Configurations/EmailOtpConfiguration.cs` | Infrastructure | EF Config EmailOtp |

**Frontend:**
| File | Layer | Tác dụng |
|------|-------|----------|
| `app/[locale]/(auth)/login/page.tsx` | Page | Trang đăng nhập |
| `app/[locale]/(auth)/register/page.tsx` | Page | Trang đăng ký |
| `app/[locale]/(auth)/verify-email/page.tsx` | Page | Trang xác thực email |
| `app/[locale]/(auth)/forgot-password/page.tsx` | Page | Trang quên mật khẩu |
| `app/[locale]/(auth)/reset-password/page.tsx` | Page | Trang đặt lại mật khẩu |
| `actions/authActions.ts` | Action | 6 hàm gọi API xác thực |
| `store/authStore.ts` | Store | Zustand store quản lý auth state |
| `components/auth/AuthSessionManager.tsx` | Component | Quản lý session/token client |
| `components/auth/MfaChallengeModal.tsx` | Component | Modal MFA challenge |
| `components/layout/AuthLayout.tsx` | Component | Layout trang xác thực |
| `lib/api.ts` | Lib | HTTP client gọi BE |
| `lib/auth-client.ts` | Lib | Tiện ích auth client |
| `lib/jwt.ts` | Lib | Parse/kiểm tra JWT |
| `types/auth.ts` | Types | TypeScript types cho auth |

### 1.4 Khu vực khoanh lỗi

| Triệu chứng | Kiểm tra |
|-------------|----------|
| Không đăng nhập được | `LoginCommandHandler` → `PasswordHasher` → `UserRepository` |
| Token hết hạn không refresh | `RefreshTokenCommandHandler` → `JwtTokenService` → API `/auth/refresh` chưa tích hợp FE |
| Không nhận OTP email | `MockEmailSender` (dev) → kiểm tra log server |
| Lỗi validate form | `RegisterCommandValidator` / `LoginCommandValidator` |

---

## 2. 👤 HỒ SƠ CÁ NHÂN (Profile)

### 2.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 9 | GET | `/profile` | Lấy thông tin hồ sơ | 🟢 |
| 10 | PATCH | `/profile` | Cập nhật hồ sơ | 🟢 |

### 2.2 Luồng xử lý

```
XEM HỒ SƠ:
FE: profile/page.tsx → profileActions.ts → lib/api.ts
    → BE: ProfileController.Get → GetProfileQuery → GetProfileQueryHandler
    → UserRepository → User entity

CẬP NHẬT HỒ SƠ:
FE: profile/page.tsx → profileActions.ts → lib/api.ts
    → BE: ProfileController.Update → UpdateProfileCommand → UpdateProfileCommandValidator
    → UpdateProfileCommandHandler → UserRepository → User entity
```

### 2.3 File liên quan

**Backend:** `Controllers/ProfileController.cs`, `Features/Profile/Commands/UpdateProfile/*`, `Features/Profile/Queries/GetProfile/*`, `Interfaces/IUserRepository.cs`, `Domain/Entities/User.cs`, `Domain/Helpers/ProfileHelper.cs`

**Frontend:** `app/[locale]/(user)/profile/page.tsx`, `actions/profileActions.ts`

### 2.4 Khu vực khoanh lỗi

| Triệu chứng | Kiểm tra |
|-------------|----------|
| Profile không load | `GetProfileQueryHandler` → `UserRepository` → DB connection |
| Cập nhật thất bại | `UpdateProfileCommandValidator` → validation rules |

---

## 3. 🔑 BẢO MẬT HAI LỚP (MFA)

### 3.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 37 | POST | `/mfa/setup` | Lấy URI + Secret thiết lập MFA | 🟢 |
| 38 | POST | `/mfa/verify` | Verify TOTP để bật MFA | 🟢 |
| 39 | POST | `/mfa/challenge` | Xác thực MFA cho hành động nhạy cảm | 🟢 |
| 40 | GET | `/mfa/status` | Lấy trạng thái MFA | 🟢 |

### 3.2 Luồng xử lý

```
THIẾT LẬP MFA:
FE: profile/mfa/page.tsx → mfaActions.ts → lib/api.ts
    → BE: MfaController.Setup → MfaSetupCommand → Handler → TotpMfaService
    ← URI + Secret để hiển thị QR code

XÁC THỰC MFA:
FE: MfaChallengeModal.tsx → mfaActions.ts
    → BE: MfaController.Challenge → MfaChallengeCommand → Handler → TotpMfaService
```

### 3.3 File liên quan

**Backend:** `Controllers/MfaController.cs`, `Features/Mfa/Commands/MfaSetup/*`, `Features/Mfa/Commands/MfaVerify/*`, `Features/Mfa/Commands/MfaChallenge/*`, `Features/Mfa/Queries/GetMfaStatus/*`, `Interfaces/IMfaService.cs`, `Services/TotpMfaService.cs`

**Frontend:** `app/[locale]/(user)/profile/mfa/page.tsx`, `actions/mfaActions.ts`, `components/auth/MfaChallengeModal.tsx`, `types/mfa.ts`

---

## 4. 🃏 RÚT BÀI TAROT (Reading)

### 4.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 15 | POST | `/reading/init` | Khởi tạo phiên rút bài | 🟢 |
| 16 | POST | `/reading/reveal` | Lật bài (RNG + kết quả) | 🟢 |
| 17 | GET | `/reading/collection` | Bộ sưu tập bài đã thu thập | 🟢 |

### 4.2 Luồng xử lý

```
KHỞI TẠO PHIÊN:
FE: reading/page.tsx → readingActions.ts → lib/api.ts
    → BE: TarotController.Init → InitReadingSessionCommand → InitReadingSessionCommandHandler
    → ReadingSessionRepository + RngService → ReadingSession entity
    → MongoReadingSessionRepository (lưu document)
    ← sessionId + metadata

LẬT BÀI:
FE: reading/page.tsx → readingActions.ts
    → BE: TarotController.Reveal → RevealReadingSessionCommand → RevealReadingSessionCommandHandler
    → ReadingSessionRepository → RngService (chốt kết quả random)
    → CardsCatalogRepository (lấy info lá bài) → UserCollectionRepository (cập nhật BST)
    ← Danh sách lá bài đã lật + ý nghĩa

BỘ SƯU TẬP:
FE: collection/page.tsx → collectionActions.ts
    → BE: TarotController.Collection → GetUserCollectionQuery → GetUserCollectionQueryHandler
    → UserCollectionRepository (MongoDB) → UserCollectionDocument
```

### 4.3 File liên quan

**Backend:** `Controllers/TarotController.cs`, `Features/Reading/Commands/InitSession/*`, `Features/Reading/Commands/RevealSession/*`, `Features/Reading/Commands/StreamReading/*`, `Features/Reading/Commands/CompleteAiStream/*`, `Features/Reading/Queries/GetCollection/*`, `Interfaces/IReadingSessionRepository.cs`, `Interfaces/ICardsCatalogRepository.cs`, `Interfaces/IUserCollectionRepository.cs`, `Interfaces/IRngService.cs`, `Domain/Entities/ReadingSession.cs`, `Domain/Entities/UserCollection.cs`, `Domain/Enums/SpreadType.cs`, `Persistence/MongoDocuments/ReadingSessionDocument.cs`, `Persistence/MongoDocuments/CardCatalogDocument.cs`, `Persistence/MongoDocuments/UserCollectionDocument.cs`, `Persistence/Repositories/MongoReadingSessionRepository.cs`, `Persistence/Repositories/MongoCardsCatalogRepository.cs`, `Persistence/Repositories/MongoUserCollectionRepository.cs`, `Services/RngService.cs`

**Frontend:** `app/[locale]/(user)/reading/page.tsx`, `app/[locale]/(user)/collection/page.tsx`, `actions/readingActions.ts`, `actions/collectionActions.ts`, `lib/tarotData.ts`

### 4.4 Khu vực khoanh lỗi

| Triệu chứng | Kiểm tra |
|-------------|----------|
| Init thất bại | `InitReadingSessionCommandHandler` → MongoDB connection |
| Reveal sai bài | `RngService` → `RevealReadingSessionCommandHandler` |
| BST thiếu bài | `MongoUserCollectionRepository` → `UserCollectionDocument` |

---

## 5. 🤖 AI INTERPRETATION (Streaming)

### 5.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 18 | GET | `/sessions/{sessionId}/stream` | SSE stream kết quả AI | 🟢 |

### 5.2 Luồng xử lý

```
AI STREAM:
FE: reading/session/[id]/page.tsx → AiInterpretationStream.tsx (EventSource SSE)
    → BE: AiController.Stream → StreamReadingCommand
    → AiRequestRepository (reserve quota) → OpenAiProvider (gọi AI)
    → SSE chunks → CompleteAiStreamCommand → AiProviderLogRepository (MongoDB log)
    ← Nội dung AI trả về dạng typewriter effect

FOLLOW-UP (trong stream):
FE: AiInterpretationStream.tsx → readingActions.ts (follow-up)
    → BE: FollowupPricingService (tính phí) → WalletRepository (trừ quota)
    → OpenAiProvider (gọi AI tiếp) → SSE chunks
```

### 5.3 File liên quan

**Backend:** `Controllers/AiController.cs`, `Features/Reading/Commands/StreamReading/*`, `Features/Reading/Commands/CompleteAiStream/*`, `Interfaces/IAiProvider.cs`, `Interfaces/IAiRequestRepository.cs`, `Interfaces/IAiProviderLogRepository.cs`, `Domain/Entities/AiRequest.cs`, `Domain/Enums/AiRequestStatus.cs`, `Domain/Services/FollowupPricingService.cs`, `Services/Ai/OpenAiProvider.cs`, `Persistence/MongoDocuments/AiProviderLogDocument.cs`, `Persistence/Repositories/AiRequestRepository.cs`, `Persistence/Repositories/MongoAiProviderLogRepository.cs`

**Frontend:** `app/[locale]/reading/session/[id]/page.tsx`, `components/AiInterpretationStream.tsx`

### 5.4 Khu vực khoanh lỗi

| Triệu chứng | Kiểm tra |
|-------------|----------|
| Stream không chạy | `AiController` → SSE headers → `OpenAiProvider` API key/connection |
| AI trả lỗi | `OpenAiProvider` → retry logic → `AiRequestRepository` quota |
| Follow-up tính sai phí | `FollowupPricingService` → `EconomyConstants` |

---

## 6. 📜 LỊCH SỬ ĐỌC BÀI (History)

### 6.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 19 | GET | `/history/sessions` | Danh sách lịch sử bốc bài | 🟢 |
| 20 | GET | `/history/sessions/{id}` | Chi tiết phiên bốc bài | 🟢 |
| 21 | GET | `/History/admin/all-sessions` | [Admin] Tất cả lịch sử | 🟢 |

### 6.2 Luồng xử lý

```
DANH SÁCH LỊCH SỬ:
FE: reading/history/page.tsx → historyActions.ts → lib/api.ts
    → BE: HistoryController.List → GetReadingHistoryQuery → Handler
    → MongoReadingSessionRepository → ReadingSessionDocument (phân trang)

CHI TIẾT PHIÊN:
FE: reading/history/[id]/page.tsx → historyActions.ts
    → BE: HistoryController.Detail → GetReadingDetailQuery → Handler
    → MongoReadingSessionRepository → ReadingSessionDocument + AI data
```

### 6.3 File liên quan

**Backend:** `Controllers/HistoryController.cs`, `Features/History/Queries/GetReadingHistory/*`, `Features/History/Queries/GetReadingDetail/*`, `Features/History/Queries/GetAllReadings/*`

**Frontend:** `app/[locale]/(user)/reading/history/page.tsx`, `app/[locale]/(user)/reading/history/[id]/page.tsx`, `actions/historyActions.ts`

---

## 7. 💰 VÍ & SỔ CÁI (Wallet & Ledger)

### 7.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 11 | GET | `/Wallet/balance` | Số dư ví (Gold, Diamond) | 🟢 |
| 12 | GET | `/Wallet/ledger` | Lịch sử giao dịch ví | 🟢 |

### 7.2 Luồng xử lý

```
XEM SỐ DƯ:
FE: wallet/page.tsx → walletActions.ts → lib/api.ts
    → BE: WalletController.Balance → GetWalletBalanceQuery → Handler
    → WalletRepository → UserWallet entity (PostgreSQL)
FE: walletStore.ts cập nhật state → WalletWidget.tsx hiển thị

LỊCH SỬ GIAO DỊCH:
FE: wallet/page.tsx → walletActions.ts
    → BE: WalletController.Ledger → GetLedgerListQuery → Handler
    → LedgerRepository → WalletTransaction entities (phân trang)
```

### 7.3 File liên quan

**Backend:** `Controllers/WalletController.cs`, `Features/Wallet/Queries/GetWalletBalance/*`, `Features/Wallet/Queries/GetLedgerList/*`, `Features/Wallet/Queries/WalletDtos.cs`, `Interfaces/IWalletRepository.cs`, `Interfaces/ILedgerRepository.cs`, `Domain/Entities/UserWallet.cs`, `Domain/Entities/WalletTransaction.cs`, `Domain/Enums/TransactionType.cs`, `Domain/Enums/CurrencyType.cs`, `Domain/Constants/EconomyConstants.cs`, `Persistence/Repositories/WalletRepository.cs`, `Persistence/Repositories/LedgerRepository.cs`, `Persistence/Configurations/WalletTransactionConfiguration.cs`

**Frontend:** `app/[locale]/(user)/wallet/page.tsx`, `actions/walletActions.ts`, `store/walletStore.ts`, `components/common/WalletWidget.tsx`, `types/wallet.ts`

### 7.4 Khu vực khoanh lỗi

| Triệu chứng | Kiểm tra |
|-------------|----------|
| Số dư sai | `WalletRepository` → `UserWallet` entity → ACID transaction |
| Ledger thiếu bản ghi | `LedgerRepository` → `WalletTransaction` → `TransactionCoordinator` |
| Double-spend | `TransactionCoordinator` → idempotency key → DB constraints |

---

## 8. 💳 NẠP TIỀN (Deposit)

### 8.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 13 | POST | `/deposits/orders` | Tạo đơn nạp tiền | 🟢 |
| 14 | POST | `/deposits/webhook/vnpay` | Webhook payment gateway | 🔴 (S2S) |

### 8.2 Luồng xử lý

```
TẠO ĐƠN NẠP:
FE: wallet/deposit/page.tsx → depositActions.ts → lib/api.ts
    → (kiểm consent) legalActions.ts → BE: LegalController
    → BE: DepositController.Create → CreateDepositOrderCommand → Validator → Handler
    → DepositOrderRepository → DepositOrder entity
    → PaymentGatewayService (tạo payment URL) → DepositPromotionRepository (áp KM)
    ← Payment URL redirect

WEBHOOK XỬ LÝ:
Payment Gateway → BE: DepositController.Webhook → ProcessDepositWebhookCommand → Handler
    → DepositOrderRepository (cập nhật status) → HmacPaymentGatewayService (verify)
    → WalletRepository (cộng tiền) → TransactionCoordinator (ACID)
```

### 8.3 File liên quan

**Backend:** `Controllers/DepositController.cs`, `Features/Deposit/Commands/CreateDepositOrder/*`, `Features/Deposit/Commands/ProcessDepositWebhook/*`, `Interfaces/IDepositOrderRepository.cs`, `Interfaces/IDepositPromotionRepository.cs`, `Interfaces/IPaymentGatewayService.cs`, `Domain/Entities/DepositOrder.cs`, `Domain/Entities/DepositPromotion.cs`, `Persistence/Repositories/DepositOrderRepository.cs`, `Persistence/Repositories/DepositPromotionRepository.cs`, `Persistence/Configurations/DepositOrderConfiguration.cs`, `Persistence/Configurations/DepositPromotionConfiguration.cs`, `Services/HmacPaymentGatewayService.cs`, `Persistence/TransactionCoordinator.cs`

**Frontend:** `app/[locale]/(user)/wallet/deposit/page.tsx`, `actions/depositActions.ts`, `actions/legalActions.ts`

---

## 9. 💸 RÚT TIỀN (Withdrawal)

### 9.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 44 | POST | `/withdrawal/create` | Reader tạo yêu cầu rút tiền | 🟢 |
| 45 | GET | `/withdrawal/my` | Reader xem lịch sử rút tiền | 🟢 |

### 9.2 Luồng xử lý

```
TẠO YÊU CẦU RÚT:
FE: wallet/withdraw/page.tsx → withdrawalActions.ts → lib/api.ts
    → BE: WithdrawalController.Create → CreateWithdrawalCommand → Handler
    → WalletRepository (kiểm tra số dư) → WithdrawalRepository → WithdrawalRequest entity
    → TransactionCoordinator (freeze tiền)

ADMIN DUYỆT:
FE: admin/withdrawals/page.tsx → withdrawalActions.ts
    → BE: AdminController.ProcessWithdrawal → ProcessWithdrawalCommand → Handler
    → WithdrawalRepository → WalletRepository (chuyển tiền) → TransactionCoordinator
```

### 9.3 File liên quan

**Backend:** `Controllers/WithdrawalController.cs`, `Features/Withdrawal/Commands/CreateWithdrawal/*`, `Features/Withdrawal/Commands/ProcessWithdrawal/*`, `Features/Withdrawal/Queries/ListWithdrawals/*`, `Interfaces/IWithdrawalRepository.cs`, `Domain/Entities/WithdrawalRequest.cs`, `Repositories/WithdrawalRepository.cs`, `Persistence/Configurations/WithdrawalRequestConfiguration.cs`

**Frontend:** `app/[locale]/(user)/wallet/withdraw/page.tsx`, `app/[locale]/admin/withdrawals/page.tsx`, `actions/withdrawalActions.ts`, `types/withdrawal.ts`

---

## 10. 📖 READER (Đăng ký & Quản lý Reader)

### 10.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 22 | POST | `/reader/apply` | Gửi đơn xin làm Reader | 🟢 |
| 23 | GET | `/reader/my-request` | Xem trạng thái đơn | 🟢 |
| 24 | GET | `/reader/profile/{userId}` | Xem hồ sơ Reader (public) | 🟢 |
| 25 | PATCH | `/reader/profile` | Reader cập nhật hồ sơ | 🟢 |
| 26 | PATCH | `/reader/status` | Chuyển online/offline | 🟢 |
| 27 | GET | `/readers` | Danh sách Reader công khai | 🟢 |

### 10.2 Luồng xử lý

```
ĐĂNG KÝ READER:
FE: reader/apply/page.tsx → readerActions.ts → lib/api.ts
    → BE: ReaderController.Apply → SubmitReaderRequestCommand → Validator → Handler
    → ReaderRequestRepository (MongoDB) → ReaderRequestDocument

XEM DANH SÁCH READER:
FE: readers/page.tsx → readerActions.ts
    → BE: ReaderController.List → ListReadersQuery → Handler
    → ReaderProfileRepository (MongoDB) → ReaderProfileDocument

CẬP NHẬT PROFILE READER:
FE: profile/reader/page.tsx → readerActions.ts
    → BE: ReaderController.UpdateProfile → UpdateReaderProfileCommand → Handler
    → ReaderProfileRepository → ReaderProfileDocument

ĐỔI TRẠNG THÁI:
FE: profile/reader/page.tsx → readerActions.ts
    → BE: ReaderController.UpdateStatus → UpdateReaderStatusCommand → Handler
    → ReaderProfileRepository
```

### 10.3 File liên quan

**Backend:** `Controllers/ReaderController.cs`, `Features/Reader/Commands/SubmitReaderRequest/*`, `Features/Reader/Commands/UpdateReaderProfile/*`, `Features/Reader/Commands/UpdateReaderStatus/*`, `Features/Reader/Queries/GetMyReaderRequest/*`, `Features/Reader/Queries/GetReaderProfile/*`, `Features/Reader/Queries/ListReaders/*`, `Interfaces/IReaderProfileRepository.cs`, `Interfaces/IReaderRequestRepository.cs`, `Domain/Enums/ReaderApprovalStatus.cs`, `Domain/Enums/ReaderOnlineStatus.cs`, `Persistence/MongoDocuments/ReaderProfileDocument.cs`, `Persistence/MongoDocuments/ReaderRequestDocument.cs`, `Persistence/Repositories/MongoReaderProfileRepository.cs`, `Persistence/Repositories/MongoReaderRequestRepository.cs`, `Common/ReaderDtos.cs`

**Frontend:** `app/[locale]/(user)/reader/apply/page.tsx`, `app/[locale]/(user)/readers/page.tsx`, `app/[locale]/(user)/readers/[id]/page.tsx`, `app/[locale]/(user)/profile/reader/page.tsx`, `actions/readerActions.ts`, `types/reader.ts`

---

## 11. 💬 CHAT & REALTIME (Conversation + SignalR)

### 11.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 28 | POST | `/conversations` | Tạo conversation mới | 🟢 |
| 29 | GET | `/conversations` | Inbox danh sách conversations | 🟢 |
| 30 | GET | `/conversations/{id}/messages` | Lịch sử tin nhắn | 🟢 |
| — | SignalR | `/api/v1/chat` | Realtime messaging | 🟢 |

### 11.2 Luồng xử lý

```
TẠO CONVERSATION:
FE: readers/[id]/page.tsx hoặc chat/page.tsx → chatActions.ts
    → BE: ConversationController.Create → CreateConversationCommand → Handler
    → ConversationRepository (MongoDB) → ConversationDocument

XEM INBOX:
FE: chat/page.tsx → chatActions.ts
    → BE: ConversationController.List → ListConversationsQuery → Handler
    → ConversationRepository → ConversationDocument (phân trang)

CHAT REALTIME:
FE: chat/[id]/page.tsx → SignalR Hub connection (HubConnectionBuilder)
    → BE: ChatHub.cs (SignalR) → SendMessageCommand → Handler
    → ChatMessageRepository (MongoDB) → ChatMessageDocument
    → SignalR broadcast → FE: nhận message realtime

ĐÁNH DẤU ĐÃ ĐỌC:
FE: chat/[id]/page.tsx → chatActions.ts
    → BE: ConversationController → MarkMessagesReadCommand → Handler
```

### 11.3 File liên quan

**Backend:** `Controllers/ConversationController.cs`, `Hubs/ChatHub.cs`, `Features/Chat/Commands/CreateConversation/*`, `Features/Chat/Commands/SendMessage/*`, `Features/Chat/Commands/MarkMessagesRead/*`, `Features/Chat/Queries/ListConversations/*`, `Features/Chat/Queries/ListMessages/*`, `Interfaces/IConversationRepository.cs`, `Interfaces/IChatMessageRepository.cs`, `Domain/Enums/ChatMessageType.cs`, `Domain/Enums/ConversationStatus.cs`, `Persistence/MongoDocuments/ConversationDocument.cs`, `Persistence/MongoDocuments/ChatMessageDocument.cs`, `Persistence/Repositories/MongoConversationRepository.cs`, `Persistence/Repositories/MongoChatMessageRepository.cs`, `Common/ChatDtos.cs`

**Frontend:** `app/[locale]/(user)/chat/page.tsx`, `app/[locale]/(user)/chat/[id]/page.tsx`, `actions/chatActions.ts`, `types/chat.ts`

---

## 12. 🔒 ESCROW (Giữ tiền giao dịch)

### 12.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 31 | POST | `/escrow/accept` | Accept offer — freeze diamond | 🟢 |
| 32 | POST | `/escrow/reply` | Reader trả lời | 🟢 |
| 33 | POST | `/escrow/confirm` | User confirm → release | 🟢 |
| 34 | POST | `/escrow/dispute` | Mở tranh chấp | 🟢 |
| 35 | POST | `/escrow/add-question` | Thêm câu hỏi | 🟢 |
| 36 | GET | `/escrow/{conversationId}` | Lấy trạng thái escrow | 🟢 |

### 12.2 Luồng xử lý

```
ACCEPT OFFER (User → Reader):
FE: chat/[id]/page.tsx → EscrowPanel.tsx → escrowActions.ts
    → BE: EscrowController.Accept → AcceptOfferCommand → Handler
    → WalletRepository (freeze diamond) → ChatFinanceRepository
    → ChatFinanceSession entity → ChatQuestionItem entity

READER TRẢ LỜI:
FE: chat/[id]/page.tsx → EscrowPanel.tsx → escrowActions.ts
    → BE: EscrowController.Reply → ReaderReplyCommand → Handler
    → ChatFinanceRepository → set auto_release timer
    → EscrowTimerService (background job kiểm tra timeout)

USER CONFIRM:
FE: chat/[id]/page.tsx → EscrowPanel.tsx → escrowActions.ts
    → BE: EscrowController.Confirm → ConfirmReleaseCommand → Handler
    → WalletRepository (release diamond → Reader) → TransactionCoordinator

TRANH CHẤP:
FE: chat/[id]/page.tsx → DisputeButton.tsx → escrowActions.ts
    → BE: EscrowController.Dispute → OpenDisputeCommand → Handler
    → ChatFinanceRepository (đánh dấu disputed)
```

### 12.3 File liên quan

**Backend:** `Controllers/EscrowController.cs`, `Features/Escrow/Commands/AcceptOffer/*`, `Features/Escrow/Commands/AddQuestion/*`, `Features/Escrow/Commands/ConfirmRelease/*`, `Features/Escrow/Commands/OpenDispute/*`, `Features/Escrow/Commands/ReaderReply/*`, `Features/Escrow/Queries/GetEscrowStatus/*`, `Interfaces/IChatFinanceRepository.cs`, `Domain/Entities/ChatFinanceSession.cs`, `Domain/Entities/ChatQuestionItem.cs`, `Domain/Enums/QuestionItemStatus.cs`, `Domain/Enums/QuestionItemType.cs`, `Persistence/Configurations/ChatFinanceSessionConfiguration.cs`, `Persistence/Configurations/ChatQuestionItemConfiguration.cs`, `Repositories/ChatFinanceRepository.cs`, `BackgroundJobs/EscrowTimerService.cs`

**Frontend:** `actions/escrowActions.ts`, `components/chat/EscrowPanel.tsx`, `components/chat/DisputeButton.tsx`, `types/escrow.ts`

### 12.4 Khu vực khoanh lỗi

| Triệu chứng | Kiểm tra |
|-------------|----------|
| Không freeze được tiền | `AcceptOfferCommand` → `WalletRepository` → balance check |
| Auto-release không chạy | `EscrowTimerService` (background job) → timer config |
| Tranh chấp không xử lý | `OpenDisputeCommand` → `ChatFinanceRepository` |

---

## 13. 🚨 BÁO CÁO VI PHẠM (Report)

### 13.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 43 | POST | `/reports` | Tạo báo cáo vi phạm | 🟢 |

### 13.2 Luồng xử lý

```
FE: chat/[id]/page.tsx → ReportModal.tsx → chatActions.ts
    → BE: ReportController.Create → CreateReportCommand → Handler
    → ReportRepository (MongoDB) → ReportDocument
```

### 13.3 File liên quan

**Backend:** `Controllers/ReportController.cs`, `Features/Chat/Commands/CreateReport/*`, `Interfaces/IReportRepository.cs`, `Persistence/MongoDocuments/ReportDocument.cs`, `Persistence/Repositories/MongoReportRepository.cs`

**Frontend:** `components/chat/ReportModal.tsx`, `actions/chatActions.ts`

---

## 14. ⚖️ PHÁP LÝ (Legal / Consent)

### 14.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 41 | GET | `/legal/consent-status` | Kiểm tra trạng thái đồng ý | 🟢 |
| 42 | POST | `/legal/consent` | Ghi nhận đồng ý pháp lý | 🟢 |

### 14.2 Luồng xử lý

```
FE: wallet/deposit/page.tsx (trước khi nạp tiền) → legalActions.ts
    → BE: LegalController.Check → CheckConsentQuery → Handler → UserConsentRepository
    → (nếu chưa consent) → FE hiển thị modal → legalActions.ts
    → BE: LegalController.Record → RecordConsentCommand → Validator → Handler
    → UserConsentRepository → UserConsent entity
```

### 14.3 File liên quan

**Backend:** `Controllers/LegalController.cs`, `Features/Legal/Commands/RecordConsent/*`, `Features/Legal/Queries/CheckConsent/*`, `Interfaces/IUserConsentRepository.cs`, `Domain/Entities/UserConsent.cs`, `Persistence/Configurations/UserConsentConfiguration.cs`, `Persistence/Repositories/UserConsentRepository.cs`

**Frontend:** `app/[locale]/legal/tos/page.tsx`, `app/[locale]/legal/privacy/page.tsx`, `app/[locale]/legal/ai-disclaimer/page.tsx`, `actions/legalActions.ts`

---

## 15. 🎁 KHUYẾN MÃI (Promotions)

### 15.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 57 | GET | `/admin/promotions` | Danh sách khuyến mãi | 🟢 |
| 58 | POST | `/admin/promotions` | Tạo khuyến mãi mới | 🟢 |
| 59 | PUT | `/admin/promotions/{id}` | Cập nhật khuyến mãi | 🟢 |
| 60 | DELETE | `/admin/promotions/{id}` | Xóa khuyến mãi | 🟢 |

### 15.2 Luồng xử lý

```
CRUD KHUYẾN MÃI (Admin):
FE: admin/promotions/page.tsx → promotions-client.tsx → promotionActions.ts
    → BE: PromotionsController → CreatePromotionCommand/UpdatePromotionCommand/DeletePromotionCommand
    → Validator → Handler → DepositPromotionRepository → DepositPromotion entity
```

### 15.3 File liên quan

**Backend:** `Controllers/PromotionsController.cs`, `Features/Promotions/Commands/CreatePromotion/*`, `Features/Promotions/Commands/UpdatePromotion/*`, `Features/Promotions/Commands/DeletePromotion/*`, `Features/Promotions/Queries/ListPromotions/*`, `Domain/Entities/DepositPromotion.cs`

**Frontend:** `app/[locale]/admin/promotions/page.tsx`, `app/[locale]/admin/promotions/promotions-client.tsx`, `actions/promotionActions.ts`

---

## 16. 🛡️ QUẢN TRỊ HỆ THỐNG (Admin)

### 16.1 API Endpoints

| # | Method | Endpoint | Mô tả | Trạng thái |
|---|--------|----------|--------|------------|
| 46 | GET | `/admin/reconciliation/wallet` | Đối soát sổ cái | 🟢 |
| 47 | GET | `/admin/users` | Danh sách users | 🟢 |
| 48 | PATCH | `/admin/users/lock` | Khóa/mở khóa user | 🟢 |
| 49 | GET | `/admin/deposits` | Danh sách đơn nạp | 🟢 |
| 50 | POST | `/admin/users/add-balance` | Cộng tiền cho user | 🟢 |
| 51 | PATCH | `/admin/deposits/process` | Phê duyệt đơn nạp | 🟢 |
| 52 | GET | `/admin/reader-requests` | Đơn xin Reader | 🟢 |
| 53 | PATCH | `/admin/reader-requests/process` | Duyệt đơn Reader | 🟢 |
| 54 | POST | `/admin/escrow/resolve-dispute` | Giải quyết tranh chấp | 🟢 |
| 55 | GET | `/admin/withdrawals/queue` | DS yêu cầu rút tiền | 🟢 |
| 56 | POST | `/admin/withdrawals/process` | Duyệt rút tiền | 🟢 |

### 16.2 Luồng xử lý

```
QUẢN LÝ USER:
FE: admin/users/page.tsx → adminActions.ts
    → BE: AdminController → ListUsersQuery / ToggleUserLockCommand / AddUserBalanceCommand

DUYỆT NẠP TIỀN:
FE: admin/deposits/page.tsx → adminActions.ts
    → BE: AdminController → ListDepositsQuery / ProcessDepositCommand

DUYỆT READER:
FE: admin/reader-requests/page.tsx → adminActions.ts
    → BE: AdminController → ListReaderRequestsQuery / ApproveReaderCommand

GIẢI QUYẾT TRANH CHẤP:
FE: admin/disputes/page.tsx → escrowActions.ts
    → BE: AdminController → ResolveDisputeCommand

DUYỆT RÚT TIỀN:
FE: admin/withdrawals/page.tsx → withdrawalActions.ts
    → BE: AdminController → ProcessWithdrawalCommand

ĐỐI SOÁT SỔ CÁI:
FE: admin/page.tsx → adminActions.ts
    → BE: AdminController → GetLedgerMismatchQuery → Handler → AdminRepository
```

### 16.3 File liên quan

**Backend:** `Controllers/AdminController.cs`, `Features/Admin/Commands/*` (AddUserBalance, ApproveReader, ProcessDeposit, ResolveDispute, ToggleUserLock), `Features/Admin/Queries/*` (GetLedgerMismatch, ListDeposits, ListReaderRequests, ListUsers), `Interfaces/IAdminRepository.cs`, `Persistence/Repositories/AdminRepository.cs`

**Frontend:** `app/[locale]/admin/page.tsx`, `app/[locale]/admin/users/page.tsx`, `app/[locale]/admin/deposits/page.tsx`, `app/[locale]/admin/reader-requests/page.tsx`, `app/[locale]/admin/readings/page.tsx`, `app/[locale]/admin/withdrawals/page.tsx`, `app/[locale]/admin/disputes/page.tsx`, `app/[locale]/admin/layout.tsx`, `actions/adminActions.ts`

---

## 17. 🏥 HẠ TẦNG CHUNG (Infrastructure / Cross-cutting)

### 17.1 Hệ thống DevOps / Debug

| # | Endpoint | Mô tả | Trạng thái |
|---|----------|--------|------------|
| 61 | `GET /health` | Health check | 🔴 (monitoring) |
| 62 | `GET /health/error-test` | Test error handling | 🔴 (debug) |
| 63 | `POST /diag/wipe` | Xóa dữ liệu dev | 🔴 (debug) |
| 64 | `GET /diag/seed-admin` | Tạo SuperAdmin | 🔴 (debug) |
| 65 | `GET /diag/stats` | Thống kê MongoDB | 🔴 (debug) |

### 17.2 File hạ tầng BE

| File | Tác dụng |
|------|----------|
| `Program.cs` | Khởi động API, DI, middleware, CORS, Swagger |
| `Middlewares/GlobalExceptionHandler.cs` | Bắt lỗi → ProblemDetails |
| `Behaviors/ValidationBehavior.cs` | MediatR pipeline: FluentValidation |
| `Application/DependencyInjection.cs` | DI Application layer |
| `Infrastructure/DependencyInjection.cs` | DI Infrastructure layer |
| `Persistence/ApplicationDbContext.cs` | EF Core DbContext (PostgreSQL) |
| `Persistence/ApplicationDbContextFactory.cs` | Design-time DB factory |
| `Persistence/MongoDbContext.cs` | MongoDB context |
| `Persistence/TransactionCoordinator.cs` | Unit of Work pattern |
| `Services/RedisCacheService.cs` | Cache Redis |
| `Services/CacheBackendState.cs` | Theo dõi trạng thái cache |
| `BackgroundJobs/CacheBackendStartupLogger.cs` | Log khởi động cache |
| `BackgroundJobs/EscrowTimerService.cs` | Timer auto-release escrow |
| `Exceptions/*` | BadRequest, NotFound, Validation, DomainException |

### 17.3 File hạ tầng FE

| File | Tác dụng |
|------|----------|
| `lib/api.ts` | HTTP client wrapper chung |
| `lib/auth-client.ts` | Auth utilities |
| `lib/jwt.ts` | JWT parser |
| `proxy.ts` | API proxy helper |
| `store/authStore.ts` | Auth state (Zustand) |
| `store/walletStore.ts` | Wallet state (Zustand) |
| `i18n/request.ts` | i18n config |
| `i18n/routing.ts` | i18n routing |
| `messages/*.json` | Bản dịch VI/EN/ZH |
| `components/common/Navbar.tsx` | Thanh điều hướng |
| `components/common/LanguageSwitcher.tsx` | Chuyển ngôn ngữ |
| `components/common/ThemeSwitcher.tsx` | Đổi theme |
| `components/layout/*` | Layout components |
| `components/ui/*` | UI primitives (Button, Input, Modal, Badge...) |

---

## 📊 THỐNG KÊ TỔNG HỢP

| Metric | Backend | Frontend |
|--------|---------|----------|
| Tổng API endpoints | 65 | — |
| Đã tích hợp FE↔BE | 58 (89.2%) | — |
| Controllers | 18 | — |
| Trang web (routes) | — | 33 |
| Action files | — | 15 |
| Component files | — | 28 |
| Type definition files | — | 7 |
| Store files | — | 2 |
| Domain Entities | 15 | — |
| Domain Enums | 13 | — |
| Repository Interfaces | 26 | — |
| Repository Implementations | 20 | — |
| EF Configurations | 10 | — |
| MongoDB Documents | 10 | — |
| Unit Tests | 33 files | — |
| Integration Tests | 10 files | — |
| E2E Tests (Playwright) | — | 2 files |

---

## ⚠️ CÁC ĐIỂM CẦN LƯU Ý

### API Chưa Tích Hợp FE (Cần Xử Lý)

| API | Ưu tiên | Ghi chú |
|-----|---------|---------|
| `POST /auth/refresh` | ⚠️ CAO | Cần cho auto-refresh token khi hết hạn |
| `POST /auth/send-verification-email` | ⚠️ TRUNG BÌNH | Cần nút "Gửi lại OTP" ở verify-email |

### Checklist Bảo Trì Nhanh

| Vấn đề | Bước 1 | Bước 2 | Bước 3 |
|--------|--------|--------|--------|
| Lỗi API 500 | Kiểm tra `GlobalExceptionHandler` log | Xác định Controller → Handler | Debug Handler → Repository |
| Lỗi UI không hiển thị | Kiểm tra `page.tsx` nào bị lỗi | Kiểm tra `actions/*.ts` tương ứng | Kiểm tra `lib/api.ts` response |
| Lỗi thanh toán | `DepositController` → `PaymentGatewayService` | `HmacPaymentGatewayService` verify | `TransactionCoordinator` ACID |
| Lỗi chat realtime | `ChatHub.cs` → SignalR connection | `chat/[id]/page.tsx` HubConnection | Network/CORS config |
| Lỗi escrow | `EscrowController` → Command Handler | `ChatFinanceRepository` → DB state | `EscrowTimerService` background |
| Lỗi AI stream | `AiController` → SSE headers | `OpenAiProvider` → API key/quota | `AiInterpretationStream.tsx` EventSource |
| Số dư sai | `WalletRepository` → `UserWallet` | `LedgerRepository` → `WalletTransaction` | `GetLedgerMismatchQuery` đối soát |

# 📌 GHI CHÚ: API CHƯA GỌI & FILE CHƯA CÓ TRONG BẢN ĐỒ TÍNH NĂNG

> **Cập nhật:** 2026-03-19  
> **Nguồn so sánh:**  
> - [BE_FE_FILE_LIST_SUPER_DETAIL.md](file:///Users/lucifer/Desktop/TarotNowAI2/BE_FE_FILE_LIST_SUPER_DETAIL.md) — 464 file  
> - [SYSTEM_FEATURE_MAP.md](file:///Users/lucifer/Desktop/TarotNowAI2/Doccument/SYSTEM_FEATURE_MAP.md) — Bản đồ tính năng  
> - [API_AND_PAGES_SUMMARY.md](file:///Users/lucifier/Desktop/TarotNowAI2/Doccument/API_AND_PAGES_SUMMARY.md) — Tổng hợp API

---

## 1. 🔴 API CHƯA ĐƯỢC GỌI TỪ FRONTEND

Tổng cộng **7 API** chưa được gọi từ Frontend (trong tổng số 65 endpoints).

### 1.1 ⚠️ CẦN TÍCH HỢP (Ảnh hưởng trải nghiệm người dùng)

| # | Method | Endpoint | Controller | Mô tả | Ưu tiên | Lý do cần tích hợp |
|---|--------|----------|------------|--------|---------|---------------------|
| 1 | `POST` | `/auth/refresh` | `AuthController` | Xoay vòng Refresh Token | 🔴 **CAO** | Khi JWT access token hết hạn, FE không có cơ chế tự động refresh → user bị đá ra ngoài phải đăng nhập lại |
| 2 | `POST` | `/auth/send-verification-email` | `AuthController` | Gửi lại OTP xác thực email | 🟡 **TRUNG BÌNH** | Ở trang `verify-email/page.tsx` không có nút "Gửi lại mã OTP" → user không nhận được OTP sẽ bị kẹt |

**File BE liên quan:**

| API | Command | Handler | Validator |
|-----|---------|---------|-----------|
| `/auth/refresh` | `RefreshTokenCommand.cs` | `RefreshTokenCommandHandler.cs` | — |
| `/auth/send-verification-email` | `SendEmailVerificationOtpCommand.cs` | `SendEmailVerificationOtpCommandHandler.cs` | — |

**File FE cần sửa để tích hợp:**

| API | File cần thêm hàm gọi | File page cần cập nhật UI |
|-----|------------------------|--------------------------|
| `/auth/refresh` | `actions/authActions.ts` | `components/auth/AuthSessionManager.tsx` (thêm auto-refresh logic) |
| `/auth/send-verification-email` | `actions/authActions.ts` | `app/[locale]/(auth)/verify-email/page.tsx` (thêm nút "Gửi lại") |

### 1.2 ✅ KHÔNG CẦN TÍCH HỢP FE (Hợp lệ)

| # | Method | Endpoint | Controller | Mô tả | Lý do không cần FE |
|---|--------|----------|------------|--------|---------------------|
| 3 | `POST` | `/deposits/webhook/vnpay` | `DepositController` | Webhook thanh toán VNPay | Server-to-server — Payment Gateway gọi trực tiếp BE |
| 4 | `GET` | `/health` | `HealthController` | Health check | Dùng cho monitoring/DevOps (Uptime Robot, Load Balancer...) |
| 5 | `GET` | `/health/error-test` | `HealthController` | Test ProblemDetails | Endpoint debug cho dev, không cần FE |
| 6 | `POST` | `/diag/wipe` | `DiagController` | Xóa toàn bộ dữ liệu | Endpoint dev-only, KHÔNG BAO GIỜ gọi từ FE |
| 7 | `GET` | `/diag/seed-admin` | `DiagController` | Tạo SuperAdmin seed | Endpoint dev-only, chạy 1 lần khi setup |
| 8 | `GET` | `/diag/stats` | `DiagController` | Thống kê MongoDB | Endpoint dev/debug |

---

## 2. 📁 FILE BACKEND CHƯA CÓ TRONG BẢN ĐỒ TÍNH NĂNG

### 2.1 File Cấu Hình & Vận Hành Dự Án (ProjectOps)

> Đây là các file cấu hình, build, tài liệu nội bộ — **không thuộc tính năng cụ thể** nào.

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `BE_AUDIT_FINDINGS_2026-03-18.md` | ProjectOps | Báo cáo audit backend | Tài liệu nội bộ |
| `BE_FINAL_AUDIT_REPORT.md` | ProjectOps | Báo cáo audit tổng hợp | Tài liệu nội bộ |
| `TarotNow.slnx` | ProjectOps | Solution file .NET | File build hệ thống |
| `cookies.txt` | ProjectOps | Cookie test API thủ công | File debug local |
| `test-api.js` | ProjectOps | Script test nhanh API | Script test (Node.js) |
| `test-paid.js` | ProjectOps | Script test luồng thanh toán | Script test (Node.js) |
| `test-reveal.js` | ProjectOps | Script test luồng reveal bài | Script test (Node.js) |

### 2.2 File Cấu Hình API Layer

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `Contracts/RequestDtos.cs` | API | Tập hợp DTO đầu vào chung | Dùng chung cho nhiều controller |
| `TarotNow.Api.csproj` | API | Project file API | File build config |
| `TarotNow.Api.http` | API | Bộ request mẫu (REST Client) | File test API từ IDE |
| `appsettings.json` | API | Config mặc định | Config runtime |
| `appsettings.Development.json` | API | Config môi trường Dev | Config runtime |
| `Properties/launchSettings.json` | API | Config profile chạy/debug | Config IDE |

### 2.3 File Application Layer (Dùng Chung)

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `Common/Models/PaginatedList.cs` | Application | Model phân trang dùng chung | Dùng bởi mọi Query có phân trang |
| `TarotNow.Application.csproj` | Application | Project file Application | File build config |
| `Interfaces/ICacheService.cs` | Application | Interface cache Redis | Dùng bởi nhiều handler |
| `Interfaces/INotificationRepository.cs` | Application | Interface notification | Chưa sử dụng nhiều (dự phòng) |
| `Interfaces/ITransactionCoordinator.cs` | Application | Interface orchestrate ACID transaction | Dùng bởi tất cả luồng tài chính |

### 2.4 File Domain Layer

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `Domain/Entities/MismatchRecord.cs` | Domain | Entity ghi nhận bất đồng bộ sổ cái | Dùng bởi `GetLedgerMismatchQuery` (Admin) |
| `Domain/Exceptions/DomainException.cs` | Domain | Base exception cho domain violations | Dùng bởi tất cả domain logic |
| `TarotNow.Domain.csproj` | Domain | Project file Domain | File build config |

### 2.5 File Infrastructure — Controller Debug/Health

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `Controllers/HealthController.cs` | API | Health check endpoints | Không thuộc tính năng FE |
| `Controllers/DiagController.cs` | API | Diagnostic endpoints (dev) | Không thuộc tính năng FE |

### 2.6 File Infrastructure — Migration (EF Core)

> Migration files sinh tự động, **không thuộc tính năng cụ thể** — áp dụng cho toàn bộ schema.

| File | Tác dụng |
|------|----------|
| `Migrations/20260315124405_ResetSchema.cs` | Migration reset schema ban đầu |
| `Migrations/20260315124405_ResetSchema.Designer.cs` | Designer metadata |
| `Migrations/20260317211223_AlignSchemaWithMongoAndSecurity.cs` | Migration alignment schema |
| `Migrations/20260317211223_AlignSchemaWithMongoAndSecurity.Designer.cs` | Designer metadata |
| `Migrations/20260318163000_AddWithdrawalOnePerDayActiveIndex.cs` | Migration thêm index withdrawal |
| `Migrations/20260318180118_AddEscrowUniquenessIndexes.cs` | Migration thêm index escrow |
| `Migrations/20260318180118_AddEscrowUniquenessIndexes.Designer.cs` | Designer metadata |
| `Migrations/ApplicationDbContextModelSnapshot.cs` | Snapshot schema hiện tại |

### 2.7 File Infrastructure — Notification (Chưa Hoàn Thiện)

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `Persistence/MongoDocuments/NotificationDocument.cs` | Infrastructure | Schema notification MongoDB | ⚠️ Có document nhưng chưa có trang FE hiển thị |
| `Persistence/Repositories/MongoNotificationRepository.cs` | Infrastructure | Repository notification | ⚠️ Đã implement nhưng chưa kết nối FE |

### 2.8 File Infrastructure — Project Config

| File | Layer | Tác dụng |
|------|-------|----------|
| `TarotNow.Infrastructure.csproj` | Infrastructure | Project file Infrastructure |

---

## 3. 📁 FILE FRONTEND CHƯA CÓ TRONG BẢN ĐỒ TÍNH NĂNG

### 3.1 File Cấu Hình & Tooling

> Các file này thuộc **config build/lint/test** — không thuộc tính năng cụ thể.

| File | Layer | Tác dụng |
|------|-------|----------|
| `.gitignore` | Tooling | Danh sách file bỏ qua Git |
| `README.md` | Tooling | Hướng dẫn setup/chạy Frontend |
| `eslint.config.mjs` | Tooling | Quy tắc lint JS/TS |
| `next.config.ts` | Tooling | Cấu hình Next.js |
| `package.json` | Tooling | Dependencies & scripts |
| `package-lock.json` | Tooling | Lock file NPM |
| `postcss.config.mjs` | Tooling | Cấu hình PostCSS |
| `tsconfig.json` | Tooling | Cấu hình TypeScript |
| `playwright.config.ts` | Testing | Cấu hình Playwright E2E |
| `.github/workflows/playwright.yml` | CI/CD | Workflow CI chạy E2E tests |

### 3.2 File Tài Nguyên Tĩnh (Public Assets)

| File | Layer | Tác dụng |
|------|-------|----------|
| `public/file.svg` | PublicAssets | Icon file |
| `public/globe.svg` | PublicAssets | Icon globe |
| `public/next.svg` | PublicAssets | Icon Next.js |
| `public/vercel.svg` | PublicAssets | Icon Vercel |
| `public/window.svg` | PublicAssets | Icon window |

### 3.3 File i18n (Bản Dịch)

| File | Layer | Tác dụng |
|------|-------|----------|
| `messages/en.json` | i18n | Bản dịch tiếng Anh (next-intl) |
| `messages/vi.json` | i18n | Bản dịch tiếng Việt |
| `messages/zh.json` | i18n | Bản dịch tiếng Trung |
| `public/locales/en/common.json` | i18n | Từ điển EN (client) |
| `public/locales/vi/common.json` | i18n | Từ điển VI (client) |

### 3.4 File Layout & Loading (Chung cho nhiều route)

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `app/[locale]/layout.tsx` | AppRouter | Layout gốc theo locale | Bao tất cả các route |
| `app/[locale]/(user)/layout.tsx` | AppRouter | Layout user đã đăng nhập | Bao tất cả route `(user)` |
| `app/[locale]/(user)/loading.tsx` | AppRouter | Loading skeleton user | Suspense fallback |
| `app/[locale]/loading.tsx` | AppRouter | Loading skeleton gốc | Suspense fallback |
| `app/favicon.ico` | AppRouter | Favicon trình duyệt | Static |
| `app/globals.css` | AppRouter | CSS global (root) | Style toàn cục |
| `app/[locale]/globals.css` | AppRouter | CSS global (locale) | Style theo locale |

### 3.5 Component Layout (Chung, không gắn tính năng cụ thể)

| File | Layer | Tác dụng | Ghi chú |
|------|-------|----------|---------|
| `components/layout/AstralBackground.tsx` | Component | Nền visual chủ đề tarot | Trang trí, không logic |
| `components/layout/BottomTabBar.tsx` | Component | Thanh điều hướng dưới (mobile) | Navigation chung |
| `components/layout/Footer.tsx` | Component | Chân trang | Layout chung |
| `components/layout/RoutePrefetcher.tsx` | Component | Prefetch route | Performance chung |
| `components/layout/UserLayout.tsx` | Component | Layout khung user | Wrapper layout |
| `components/layout/UserSidebar.tsx` | Component | Sidebar menu user | Navigation chung |

### 3.6 Component UI Primitives (Tái sử dụng)

> Các component UI primitive **được sử dụng khắp nơi** — không gắn vào tính năng cụ thể.

| File | Layer | Tác dụng |
|------|-------|----------|
| `components/ui/Badge.tsx` | UI | Badge hiển thị nhãn |
| `components/ui/Button.tsx` | UI | Button chung |
| `components/ui/EmptyState.tsx` | UI | Trạng thái rỗng |
| `components/ui/GlassCard.tsx` | UI | Card kiểu glassmorphism |
| `components/ui/Input.tsx` | UI | Input field chung |
| `components/ui/LoadingSpinner.tsx` | UI | Spinner loading |
| `components/ui/Modal.tsx` | UI | Modal dialog |
| `components/ui/Pagination.tsx` | UI | Phân trang |
| `components/ui/SectionHeader.tsx` | UI | Header section |
| `components/ui/SkeletonLoader.tsx` | UI | Skeleton loading |
| `components/ui/index.ts` | UI | Barrel export |

---

## 4. 🧪 FILE TEST CHƯA CÓ TRONG BẢN ĐỒ TÍNH NĂNG

> Tất cả file test **đều có trong dự án** nhưng không liệt kê trong bản đồ tính năng vì chúng là **cross-cutting**.

### 4.1 Unit Tests — Application Layer

| File | Module test | Tính năng liên quan |
|------|-------------|---------------------|
| `GetLedgerMismatchQueryHandlerTests.cs` | Admin | Đối soát sổ cái |
| `ApproveReaderCommandHandlerTests.cs` | Admin | Duyệt Reader |
| `ProcessDepositCommandHandlerTests.cs` | Admin | Xử lý nạp tiền |
| `ForgotPasswordCommandHandlerTests.cs` | Auth | Quên mật khẩu |
| `LoginCommandHandlerTests.cs` | Auth | Đăng nhập |
| `RegisterCommandHandlerTests.cs` | Auth | Đăng ký |
| `ResetPasswordCommandHandlerTests.cs` | Auth | Đặt lại mật khẩu |
| `SendEmailVerificationOtpCommandHandlerTests.cs` | Auth | Gửi OTP email |
| `VerifyEmailCommandHandlerTests.cs` | Auth | Xác thực email |
| `CreateConversationCommandHandlerTests.cs` | Chat | Tạo conversation |
| `CreateReportCommandHandlerTests.cs` | Chat | Báo cáo vi phạm |
| `MarkMessagesReadCommandHandlerTests.cs` | Chat | Đánh dấu đã đọc |
| `SendMessageCommandHandlerTests.cs` | Chat | Gửi tin nhắn |
| `AcceptOfferCommandHandlerTests.cs` | Escrow | Accept offer |
| `AddQuestionCommandHandlerTests.cs` | Escrow | Thêm câu hỏi |
| `ConfirmReleaseCommandHandlerTests.cs` | Escrow | Xác nhận giải phóng |
| `OpenDisputeCommandHandlerTests.cs` | Escrow | Mở tranh chấp |
| `ReaderReplyCommandHandlerTests.cs` | Escrow | Reader trả lời |
| `GetReadingDetailQueryHandlerTests.cs` | History | Chi tiết phiên |
| `GetReadingHistoryQueryHandlerTests.cs` | History | Lịch sử phiên |
| `MfaChallengeCommandHandlerTests.cs` | MFA | Challenge MFA |
| `MfaSetupCommandHandlerTests.cs` | MFA | Setup MFA |
| `MfaVerifyCommandHandlerTests.cs` | MFA | Verify MFA |
| `SubmitReaderRequestCommandHandlerTests.cs` | Reader | Nộp đơn Reader |
| `UpdateReaderProfileCommandHandlerTests.cs` | Reader | Cập nhật profile |
| `UpdateReaderStatusCommandHandlerTests.cs` | Reader | Đổi trạng thái |
| `CreateWithdrawalCommandHandlerTests.cs` | Withdrawal | Tạo yêu cầu rút |
| `ProcessWithdrawalCommandHandlerTests.cs` | Withdrawal | Xử lý rút tiền |
| `InitReadingSessionCommandHandlerTests.cs` | Reading | Khởi tạo phiên |
| `RevealReadingSessionCommandHandlerTests.cs` | Reading | Lật bài |
| `GetLedgerListQueryHandlerTests.cs` | Wallet | Danh sách ledger |
| `GetWalletBalanceQueryHandlerTests.cs` | Wallet | Số dư ví |

### 4.2 Integration Tests

| File | Module test |
|------|-------------|
| `AdminRbacIntegrationTests.cs` | Admin RBAC |
| `AiStreamingTests.cs` | AI Streaming |
| `CustomWebApplicationFactory.cs` | Test host factory |
| `DepositWebhookIntegrationTests.cs` | Deposit Webhook |
| `FollowupCapIntegrationTests.cs` | Follow-up cap |
| `LegalIntegrationTests.cs` | Legal/consent |
| `ProfileIntegrationTests.cs` | Profile |
| `PromotionIntegrationTests.cs` | Promotion |
| `TestAuthHandler.cs` | Auth test helper |
| `Argon2idPasswordHasherTests.cs` | Password hashing |

### 4.3 Infrastructure Unit Tests

| File | Module test |
|------|-------------|
| `EscrowTimerServiceTests.cs` | Escrow timer job |
| `RngServiceTests.cs` | Random number service |

### 4.4 Test Project Files

| File | Tác dụng |
|------|----------|
| `TarotNow.Api.IntegrationTests.csproj` | Project file integration tests |
| `TarotNow.Application.UnitTests.csproj` | Project file unit tests |
| `TarotNow.Domain.UnitTests.csproj` | Project file domain tests |
| `TarotNow.Infrastructure.IntegrationTests.csproj` | Project file infra tests |
| `TarotNow.Infrastructure.UnitTests.csproj` | Project file infra unit tests |

### 4.5 Frontend E2E Tests

| File | Tác dụng |
|------|----------|
| `tests/example.spec.ts` | Playwright test mẫu |
| `tests/viewport-qa.spec.ts` | Test đa viewport |

---

## 📊 THỐNG KÊ TỔNG HỢP

| Hạng mục | Số lượng |
|----------|----------|
| **API chưa gọi từ FE** | **7** (2 cần tích hợp, 5 hợp lệ) |
| **File BE chưa trong bản đồ** | **30** (config/migration/test/notification) |
| **File FE chưa trong bản đồ** | **35** (tooling/assets/i18n/layout/UI/test) |
| **File test chưa trong bản đồ** | **39** (unit + integration + e2e) |

### Phân loại lý do không có trong bản đồ

| Lý do | Số file | Giải thích |
|-------|---------|------------|
| Config & build (`.csproj`, `tsconfig`, `package.json`...) | 18 | Không thuộc tính năng nghiệp vụ |
| Migration files (EF Core sinh tự động) | 8 | Cross-cutting, schema chung |
| Test files | 39 | Cross-cutting, kiểm thử toàn hệ thống |
| Static assets (SVG, favicon) | 6 | Tài nguyên tĩnh |
| i18n files (JSON bản dịch) | 5 | Đa ngôn ngữ chung |
| UI primitives (Button, Input, Modal...) | 11 | Tái sử dụng khắp nơi |
| Layout/Loading components | 7 | Khung chung, không logic nghiệp vụ |
| Notification (chưa hoàn thiện) | 2 | ⚠️ Đã code BE nhưng chưa có FE |

---

## ⚠️ HÀNH ĐỘNG CẦN LÀM

### Ưu tiên CAO

1. **Tích hợp `POST /auth/refresh`** vào `authActions.ts` + `AuthSessionManager.tsx`
   - Thêm logic auto-refresh khi token sắp hết hạn
   - Intercept 401 response trong `lib/api.ts` → gọi refresh → retry request

### Ưu tiên TRUNG BÌNH

2. **Tích hợp `POST /auth/send-verification-email`** vào `verify-email/page.tsx`
   - Thêm nút "Gửi lại mã OTP" với countdown 60s

### Ưu tiên THẤP (Tùy chọn)

3. **Hoàn thiện Notification module** — đã có `NotificationDocument.cs` + `MongoNotificationRepository.cs` + `INotificationRepository.cs` nhưng chưa có:
   - Controller endpoint để FE gọi
   - Trang FE hiển thị thông báo
   - Realtime push notification (SignalR hoặc SSE)

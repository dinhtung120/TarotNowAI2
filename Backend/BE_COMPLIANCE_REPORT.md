# Backend Compliance Report (2026-03-26)

## 1) Mục tiêu
Rà soát và chỉnh sửa toàn bộ backend theo checklist kiến trúc/maintainability bạn yêu cầu (Clean Architecture, DI, CQRS, testability, error handling, observability, versioning, config/options, OpenAPI/XML docs, complexity budget).

## 2) Kết quả tổng quan
- Đã hoàn tất audit + refactor các điểm chưa đạt.
- Đã bổ sung rule kiến trúc tự động để khóa chuẩn lâu dài.
- Toàn bộ test đang **PASS**.

## 3) Các hạng mục đã sửa

### A. Chuẩn hóa API Versioning + giảm magic string route
- Thêm constants dùng chung:
  - `src/TarotNow.Api/Constants/ApiVersions.cs`
  - `src/TarotNow.Api/Constants/ApiRoutes.cs`
  - `src/TarotNow.Api/Constants/FeatureFlags.cs`
  - `src/TarotNow.Api/GlobalUsings.cs`
- Chuyển route hard-code `api/v1/...` trong controller sang constants (`ApiRoutes.*`).
- Bổ sung `[ApiVersion(ApiVersions.V1)]` cho API controllers.
- Cấu hình version reader theo URL segment trong startup (`UrlSegmentApiVersionReader`).
- Đồng bộ SignalR route qua `ApiRoutes.ChatHub`.
- Đồng bộ path check JWT stream/chat qua constants nội bộ Infrastructure.

### B. Chuẩn hóa cấu hình sang IOptions (thay IConfiguration trực tiếp trong runtime service)
- Thêm options classes:
  - `src/TarotNow.Infrastructure/Options/Argon2Options.cs`
  - `src/TarotNow.Infrastructure/Options/SecurityOptions.cs`
  - `src/TarotNow.Infrastructure/Options/SmtpOptions.cs`
  - `src/TarotNow.Infrastructure/Options/LegalSettingsOptions.cs`
  - `src/TarotNow.Infrastructure/Options/DiagnosticsOptions.cs`
- Bind vào DI tại `src/TarotNow.Infrastructure/DependencyInjection.cs`.
- Refactor các service sang `IOptions<T>`:
  - `Argon2idPasswordHasher`
  - `TotpMfaService`
  - `LegalVersionSettings`
  - `DiagnosticsService`
  - `SmtpEmailSender`

### C. DI/Testability
- Loại bỏ tạo `new SmtpClient()` trong service logic.
- Đăng ký SMTP client qua DI container:
  - `services.AddScoped<ISmtpClient, SmtpClient>();`
- `SmtpEmailSender` dùng constructor injection đầy đủ, tăng khả năng mock/test.

### D. OpenAPI + XML Comments cho public endpoints
- Giữ Generate XML doc và include XML comments trong Swagger.
- Bổ sung XML `<summary>` cho các action public còn thiếu (nhóm Admin/Auth/Escrow/AI).
- Cập nhật pipeline để `MapOpenApi()` luôn được map.

### E. Rule kiến trúc mới (tự động kiểm soát)
Thêm file:
- `tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`

Các rule mới đã bật:
1. API controllers phải có version metadata (`[ApiVersion]`/`[ApiVersionNeutral]`).
2. Không hard-code `api/v1` trong route attributes.
3. Runtime services Infrastructure không dùng `IConfiguration` trực tiếp (trừ composition root allowlist).
4. Không `new SmtpClient()` ngoài composition root.
5. Feature flag key phải dùng constants (không string literal trực tiếp).
6. Mọi HTTP action phải có XML summary comment.

## 4) Kết quả test xác nhận
Chạy:
- `dotnet test --nologo`

Kết quả:
- `TarotNow.ArchitectureTests`: Passed
- `TarotNow.Domain.UnitTests`: Passed
- `TarotNow.Application.UnitTests`: Passed
- `TarotNow.Infrastructure.IntegrationTests`: Passed
- `TarotNow.Api.IntegrationTests`: Passed

## 5) Trạng thái theo checklist bạn đưa
- Clean Architecture 4 layer + dependency direction: **Đạt** (đã có test kiến trúc).
- DI/constructor injection: **Đạt** (đã xử lý thêm các điểm dùng config/new trực tiếp).
- CQRS + MediatR + pipeline behaviors: **Đạt**.
- Global exception + custom exceptions + FluentValidation: **Đạt**.
- Structured logging + observability (logs/metrics/tracing): **Đạt**.
- API versioning: **Đạt** (route + metadata + URL segment reader).
- Config qua `.env` + `IOptions`: **Đạt** (phần runtime service đã chuẩn hóa).
- OpenAPI + XML comments public HTTP actions: **Đạt**.
- Complexity/line/method/param budgets: **Đạt** (architecture tests pass).
- Bonus CQRS pipeline behaviors + domain events + feature flags: **Đạt**.
- Bonus contract testing: **N/A** trong phạm vi hiện tại (chưa tách microservices).
- Bonus EF Core migration order + OpenAPI exposure: **Đạt**.

## 6) Ghi chú
- Các tiêu chí mang tính thiết kế định tính như SRP/Feature Envy/Inappropriate Intimacy đã được giảm rủi ro bằng quy tắc kiến trúc tự động + review code theo feature slice; không phát hiện blocker mới trong phạm vi code hiện tại.

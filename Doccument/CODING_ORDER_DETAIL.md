# 🔧 HƯỚNG DẪN CHI TIẾT: THỨ TỰ CODE TỪNG FILE

> **File này bổ sung chi tiết cho `CLONE_PROJECT_GUIDE.md`.**  
> Mỗi file ghi rõ: **cần chứa gì bên trong**, **tại sao phải code trước file khác**, và **cách kiểm tra**.

---

## GIAI ĐOẠN 0: HẠ TẦNG CƠ SỞ

> **Mục tiêu:** Chạy `dotnet run` không lỗi + `npm run dev` hiện trang trắng.  
> **Tại sao phải code trước?** Tất cả tính năng đều phụ thuộc vào khung này.

---

### BƯỚC 0.1 — Domain Enums (Hằng số dùng chung)

> **Tại sao code Enum trước?** Vì Entity (bước sau) cần dùng các giá trị này. Nếu chưa có Enum → Entity sẽ lỗi biên dịch.

#### File 1: `Domain/Enums/UserRole.cs`

```
Mục đích: Định nghĩa 3 vai trò người dùng
Nội dung cần code:
  - 3 hằng số string: "user", "tarot_reader", "admin"
  - Dùng static class với const string (không dùng C# enum vì lưu DB là text)
Ví dụ:
  public static class UserRole
  {
      public const string User = "user";
      public const string TarotReader = "tarot_reader";
      public const string Admin = "admin";
  }
Phụ thuộc: Không phụ thuộc file nào
```

#### File 2: `Domain/Enums/UserStatus.cs`

```
Mục đích: Định nghĩa 3 trạng thái tài khoản
Nội dung cần code:
  - "pending" (chờ xác thực email)
  - "active" (đã kích hoạt)
  - "locked" (bị khóa bởi admin)
Phụ thuộc: Không
```

#### File 3: `Domain/Enums/CurrencyType.cs`

```
Mục đích: Định nghĩa 2 loại tiền tệ trong hệ thống
Nội dung cần code:
  - "gold" — tiền miễn phí, cày được
  - "diamond" — tiền nạp bằng tiền thật
Phụ thuộc: Không
```

#### File 4: `Domain/Enums/TransactionType.cs`

```
Mục đích: Định nghĩa các loại giao dịch ví
Nội dung cần code:
  - "deposit" — nạp tiền
  - "spend" — chi tiêu (AI, escrow)
  - "release" — giải phóng escrow cho Reader
  - "refund" — hoàn tiền
  - "admin_add" — admin cộng tiền thủ công
Phụ thuộc: Không
```

#### File 5: `Domain/Enums/ReaderApprovalStatus.cs`

```
Mục đích: Trạng thái đơn xin Reader
Nội dung: "pending", "approved", "rejected"
Phụ thuộc: Không
```

#### File 6: `Domain/Constants/EconomyConstants.cs`

```
Mục đích: Hằng số giá cả, tỉ giá
Nội dung cần code:
  - AiReadingCostGold = 5 (giá 1 lần AI đọc bài bằng Gold)
  - AiReadingCostDiamond = 2 (giá bằng Diamond)
  - DiamondToVndRate = 1000 (1 diamond = 1000 VNĐ)
  - WithdrawalFeePercent = 10 (phí rút 10%)
  - MinWithdrawalDiamond = 50 (rút tối thiểu 50 diamond)
  - FollowupBaseCost = 1 (giá follow-up AI đầu tiên)
Phụ thuộc: Không
```

#### File 7: `Domain/Exceptions/DomainException.cs`

```
Mục đích: Exception base khi vi phạm quy tắc nghiệp vụ
Nội dung: class DomainException kế thừa Exception
  - Constructor nhận message
  - Dùng khi: số dư không đủ, trùng email, v.v.
Phụ thuộc: Không
```

---

### BƯỚC 0.2 — Domain Entities (Cấu trúc dữ liệu)

> **Tại sao code Entity sau Enum?** Entity dùng Enum để gán giá trị mặc định (VD: `Role = UserRole.User`).

#### File 8: `Domain/Entities/UserWallet.cs`

```
Mục đích: Value Object quản lý tiền của user (owned entity — nằm cùng bảng users)
Nội dung cần code:
  - 4 property: GoldBalance, DiamondBalance, FrozenDiamondBalance, TotalDiamondsPurchased
  - Tất cả kiểu long, default 0, có private set
  - 6 method:
    • Credit(currency, amount, type) — cộng tiền
    • Debit(currency, amount) — trừ tiền (throw nếu không đủ)
    • FreezeDiamond(amount) — đóng băng cho escrow
    • ReleaseFrozenDiamond(amount) — giải phóng (chuyển cho Reader)
    • RefundFrozenDiamond(amount) — hoàn lại cho user
    • ConsumeFrozenDiamond(amount) — tiêu hao (trả phí AI)
  - Factory method: CreateDefault() trả về wallet rỗng
  - Protected constructor cho EF Core
Phụ thuộc: CurrencyType, TransactionType (Enum)
Tại sao code trước User? Vì User chứa UserWallet → User.cs import UserWallet
```

#### File 9: `Domain/Entities/User.cs`

```
Mục đích: Entity trung tâm — bảng users trong PostgreSQL
Nội dung cần code:
  - 18+ property (xem schema bảng users ở CLONE_PROJECT_GUIDE.md)
  - Owned property: Wallet (kiểu UserWallet)
  - Backward compatibility: GoldBalance => Wallet.GoldBalance (proxy)
  - Constructor: nhận email, username, passwordHash, displayName, dateOfBirth, hasConsented
    → gán default: Status=pending, Role=user, Wallet=CreateDefault()
  - 12 method:
    • AddExp, UpdatePassword, UpdateProfile, Activate, Lock, Unlock
    • PromoteToAdmin, ApproveAsReader, RejectReaderRequest
    • Credit, Debit, FreezeDiamond (proxy tới Wallet)
  - Protected constructor cho EF Core
Phụ thuộc: UserWallet, UserRole, UserStatus, ReaderApprovalStatus
```

---

### BƯỚC 0.3 — Infrastructure: Kết nối Database

> **Tại sao code Infrastructure sau Entity?** DbContext cần biết Entity nào để tạo bảng.

#### File 10: `Infrastructure/Persistence/ApplicationDbContext.cs`

```
Mục đích: EF Core DbContext — cầu nối giữa C# và PostgreSQL
Nội dung cần code:
  - Kế thừa DbContext
  - Khai báo DbSet cho mỗi bảng:
    • DbSet<User> Users
    • (Sau này thêm: RefreshTokens, EmailOtps, WalletTransactions, v.v.)
  - Override OnModelCreating → gọi ApplyConfigurationsFromAssembly
    (tự động tìm tất cả file Configuration trong project)
Phụ thuộc: User entity
Tại sao cần? Không có DbContext → không kết nối được PostgreSQL
```

#### File 11: `Infrastructure/Persistence/Configurations/UserConfiguration.cs`

```
Mục đích: Cấu hình chi tiết bảng users cho EF Core
Nội dung cần code:
  - Implement IEntityTypeConfiguration<User>
  - Chỉ định: tên bảng "users", primary key, unique constraints (email, username)
  - Cấu hình Owned Entity:
    builder.OwnsOne(u => u.Wallet, w => {
        w.Property(x => x.GoldBalance).HasColumnName("wallet_gold_balance");
        w.Property(x => x.DiamondBalance).HasColumnName("wallet_diamond_balance");
        // ... 2 cột nữa
    });
  - HasMany<UserConsent> (thêm sau)
Phụ thuộc: User entity
Tại sao cần? Không có Configuration → EF Core tạo bảng với tên/cấu trúc mặc định (sai)
```

#### File 12: `Infrastructure/Persistence/MongoDbContext.cs`

```
Mục đích: Quản lý kết nối MongoDB, đặt tên collection
Nội dung cần code:
  - Constructor nhận IMongoDatabase
  - Property trả về IMongoCollection cho mỗi loại document:
    • ReadingSessions → "reading_sessions"
    • CardCatalogs → "card_catalog"
    • UserCollections → "user_collections"
    • (Sau này thêm: Conversations, ChatMessages, v.v.)
Phụ thuộc: MongoDB.Driver package
```

#### File 13: `Infrastructure/Persistence/ApplicationDbContextFactory.cs`

```
Mục đích: Cho phép chạy lệnh EF migration từ terminal (design-time factory)
Nội dung cần code:
  - Implement IDesignTimeDbContextFactory<ApplicationDbContext>
  - Đọc appsettings.json → lấy connection string → tạo DbContext
Phụ thuộc: ApplicationDbContext
Tại sao cần? Không có file này → lệnh "dotnet ef migrations add" sẽ lỗi
```

---

### BƯỚC 0.4 — Application: Pipeline chung

> **Tại sao code sau Infrastructure?** Application layer định nghĩa interface, nhưng DependencyInjection cần đăng ký cả 2 layer cùng lúc.

#### File 14: `Application/Interfaces/ICacheService.cs`

```
Mục đích: Interface cho cache (Redis hoặc memory fallback)
Nội dung:
  - GetAsync<T>(key)
  - SetAsync<T>(key, value, expiry)
  - RemoveAsync(key)
Phụ thuộc: Không
```

#### File 15: `Application/Interfaces/ITransactionCoordinator.cs`

```
Mục đích: Interface Unit of Work — đảm bảo nhiều thao tác DB trong 1 transaction
Nội dung:
  - BeginTransactionAsync()
  - CommitAsync()
  - RollbackAsync()
Phụ thuộc: Không
```

#### File 16: `Application/Common/Models/PaginatedList.cs`

```
Mục đích: Model phân trang dùng chung cho tất cả Query có phân trang
Nội dung:
  - Generic class PaginatedList<T>
  - Property: Items (List<T>), TotalCount, PageIndex, PageSize, TotalPages
  - Static method: CreateAsync(source, pageIndex, pageSize) — truy vấn DB với Skip/Take
Phụ thuộc: Không
```

#### File 17: `Application/Behaviors/ValidationBehavior.cs`

```
Mục đích: MediatR pipeline — tự động validate mọi Command/Query trước khi xử lý
Nội dung:
  - Implement IPipelineBehavior<TRequest, TResponse>
  - Constructor nhận IEnumerable<IValidator<TRequest>> (DI inject tất cả validator)
  - Logic: chạy tất cả validator → nếu có lỗi → throw ValidationException → nếu OK → gọi next()
Phụ thuộc: FluentValidation, MediatR
Tại sao cần? Nếu không có → phải validate thủ công trong mỗi Handler (vi phạm DRY)
```

#### File 18: `Application/DependencyInjection.cs`

```
Mục đích: Đăng ký tất cả service Application vào DI container
Nội dung:
  - Extension method: AddApplicationServices(this IServiceCollection)
  - Đăng ký MediatR (auto-scan Handler từ assembly)
  - Đăng ký ValidationBehavior vào MediatR pipeline
  - Đăng ký FluentValidation (auto-scan Validator từ assembly)
Phụ thuộc: ValidationBehavior, MediatR, FluentValidation
Tại sao cần? Program.cs gọi services.AddApplicationServices() → nếu thiếu file này → lỗi biên dịch
```

---

### BƯỚC 0.5 — Infrastructure: DI + Services

#### File 19: `Infrastructure/Services/RedisCacheService.cs`

```
Mục đích: Implement ICacheService bằng Redis (fallback Memory nếu Redis tắt)
Nội dung:
  - Implement ICacheService
  - Constructor nhận IDistributedCache
  - Serialize/Deserialize dùng System.Text.Json
Phụ thuộc: ICacheService (Application interface)
```

#### File 20: `Infrastructure/Persistence/TransactionCoordinator.cs`

```
Mục đích: Implement ITransactionCoordinator — quản lý ACID transaction PostgreSQL
Nội dung:
  - Constructor nhận ApplicationDbContext
  - BeginTransactionAsync → DbContext.Database.BeginTransactionAsync()
  - CommitAsync → SaveChanges + Commit
  - RollbackAsync → Rollback
Phụ thuộc: ApplicationDbContext, ITransactionCoordinator
```

#### File 21: `Infrastructure/Services/CacheBackendState.cs`

```
Mục đích: Lưu trạng thái cache đang dùng Redis hay Memory
Nội dung: record CacheBackendState(bool UsesRedis)
Phụ thuộc: Không
```

#### File 22: `Infrastructure/DependencyInjection.cs`

```
Mục đích: Đăng ký TẤT CẢ service Infrastructure vào DI container
Nội dung:
  - Extension method: AddInfrastructureServices(this IServiceCollection, IConfiguration)
  - Đăng ký: PostgreSQL DbContext, MongoDB Client+Database, Redis
  - Đăng ký: Tất cả Repository (ban đầu chỉ cần UserRepository)
  - Đăng ký: JWT Authentication
  - Có TryCreateRedisMultiplexer fallback Memory nếu Redis tắt
  - Có ValidateJwtSecret chống dùng key yếu
Phụ thuộc: TẤT CẢ file Infrastructure + Application interfaces
Lưu ý: File này sẽ được CẬP NHẬT liên tục mỗi khi thêm tính năng mới
  → Thêm repo → thêm dòng services.AddScoped<IXxxRepo, XxxRepo>()
```

---

### BƯỚC 0.6 — API Layer: Khung chạy

#### File 23: `Api/appsettings.json`

```
Mục đích: File cấu hình chính
Nội dung cần có:
  - ConnectionStrings: PostgreSQL, MongoDB, Redis
  - Jwt: SecretKey, Issuer, Audience, ExpiryMinutes, RefreshExpiryDays
  - AiProvider: ApiKey, BaseUrl, Model, TimeoutSeconds, MaxRetries
  - Redis: ConnectionString, InstanceName
  - Cors: AllowedOrigins (http://localhost:3000)
  - SystemConfig: DailyFreeGold, DailyAiQuota, InFlightAiCap, ReadingRateLimitSeconds
  - Serilog: config logging
Phụ thuộc: Không (file JSON tĩnh)
```

#### File 24: `Api/appsettings.Development.json`

```
Mục đích: Override config cho môi trường dev (password dev, debug log)
Nội dung: Connection strings local, JWT dev key, Serilog Debug level
Phụ thuộc: Không
```

#### File 25: `Api/Middlewares/GlobalExceptionHandler.cs`

```
Mục đích: Bắt TẤT CẢ exception → trả ProblemDetails JSON thay vì crash
Nội dung:
  - Implement IExceptionHandler (ASP.NET 8)
  - Switch theo loại exception:
    • ValidationException → 400 + danh sách lỗi
    • DomainException → 400 + message
    • NotFoundException → 404
    • Còn lại → 500 Internal Server Error
Phụ thuộc: DomainException
Tại sao cần? Nếu không có → lỗi server trả HTML thay vì JSON → FE không parse được
```

#### File 26: `Api/Program.cs`

```
Mục đích: ĐIỂM KHỞI ĐỘNG — nơi mọi thứ được lắp ghép
Nội dung cần code:
  1. builder.Services.AddApplicationServices()
  2. builder.Services.AddInfrastructureServices(builder.Configuration)
  3. builder.Services.AddControllers()
  4. builder.Services.AddEndpointsApiExplorer() + AddSwaggerGen()
  5. builder.Services.AddSignalR() (cho chat realtime sau này)
  6. builder.Services.AddExceptionHandler<GlobalExceptionHandler>()
  7. Cấu hình CORS cho http://localhost:3000
  8. app.UseAuthentication() + app.UseAuthorization()
  9. app.MapControllers()
  10. app.MapHub<ChatHub>("/api/v1/chat") (thêm sau)
  11. app.UseSwagger() + app.UseSwaggerUI()
Phụ thuộc: DependencyInjection (Application + Infrastructure), GlobalExceptionHandler
GHI NHỚ: File này cũng sẽ CẬP NHẬT khi thêm tính năng
```

#### File 27: `Api/Controllers/HealthController.cs`

```
Mục đích: Endpoint kiểm tra server có sống không
Nội dung:
  - [Route("api/v1/health")]
  - GET "/" → trả { status: "ok", time: DateTime.UtcNow }
Phụ thuộc: Không
```

**✅ KIỂM TRA GIAI ĐOẠN 0:**
```bash
cd Backend/src/TarotNow.Api
dotnet build        # Phải "Build succeeded"
dotnet run          # Phải hiện "Now listening on: http://localhost:5037"
# Mở browser: http://localhost:5037/api/v1/health → thấy JSON
# Mở browser: http://localhost:5037/swagger → thấy Swagger UI
```

---

### BƯỚC 0.7 — Frontend: Khung chạy

#### File 28: `src/lib/api.ts`

```
Mục đích: HTTP client wrapper — TẤT CẢ lệnh gọi API đều đi qua file này
Nội dung:
  - Hằng số API_BASE_URL đọc từ process.env.NEXT_PUBLIC_API_URL
  - Hàm apiGet<T>(path) — gọi GET, trả data hoặc throw
  - Hàm apiPost<T>(path, body) — gọi POST
  - Hàm apiPatch<T>(path, body) — gọi PATCH
  - Hàm apiDelete(path) — gọi DELETE
  - Tự đính kèm header Authorization: Bearer {token} nếu có
  - Tự parse JSON response
  - Nếu status 401 → xóa token, redirect login
Phụ thuộc: Không
Tại sao code trước? Tất cả actions/*.ts đều import api.ts
```

#### File 29-30: `src/i18n/request.ts` + `src/i18n/routing.ts`

```
Mục đích: Cấu hình đa ngôn ngữ (vi, en, zh)
Nội dung:
  - request.ts: getRequestConfig trả messages theo locale
  - routing.ts: defineRouting với locales ['vi','en','zh'], defaultLocale 'vi'
Phụ thuộc: next-intl package
```

#### File 31-32: `messages/vi.json` + `messages/en.json`

```
Mục đích: File bản dịch giao diện
Nội dung ban đầu: { "common": { "appName": "TarotNowAI", "login": "Đăng nhập", ... } }
Phụ thuộc: Không (file JSON tĩnh)
```

#### File 33: `src/app/[locale]/layout.tsx`

```
Mục đích: Root layout — bao bọc tất cả trang
Nội dung:
  - Import NextIntlClientProvider
  - Import font (Inter hoặc Playfair Display)
  - Truyền messages locale xuống children
  - Thêm <html lang={locale}>
Phụ thuộc: i18n config, messages
```

#### File 34: `src/app/[locale]/page.tsx`

```
Mục đích: Trang chủ
Nội dung ban đầu: Hero section + tiêu đề dự án
Phụ thuộc: layout.tsx
```

#### File 35-38: `src/components/ui/Button.tsx`, `Input.tsx`, `Modal.tsx`, `LoadingSpinner.tsx`

```
Mục đích: UI primitives — component cơ bản tái sử dụng
Nội dung:
  - Button: nhận variant (primary/secondary/danger), disabled, loading, onClick
  - Input: nhận label, error, type, placeholder, register (react-hook-form)
  - Modal: nhận isOpen, onClose, title, children
  - LoadingSpinner: animated spinner SVG
Phụ thuộc: Không
Tại sao code trước? Tất cả trang đăng nhập/đăng ký đều dùng Button + Input
```

#### File 39: `src/components/common/Navbar.tsx`

```
Mục đích: Thanh điều hướng hiển thị trên mọi trang
Nội dung: Logo, link Home, link Login/Register (nếu chưa đăng nhập), avatar + dropdown (nếu đã đăng nhập)
Phụ thuộc: authStore (thêm sau), LanguageSwitcher
```

#### File 40: `.env.local`

```
Mục đích: Biến môi trường cho Frontend
Nội dung: NEXT_PUBLIC_API_URL=http://localhost:5037/api/v1
```

**✅ KIỂM TRA:**
```bash
cd Frontend && npm run dev   # http://localhost:3000 → hiện trang chủ
```

---

## GIAI ĐOẠN 1: ĐĂNG KÝ & ĐĂNG NHẬP

> **Mục tiêu:** User đăng ký → nhận OTP → xác thực email → đăng nhập → duy trì session.

---

### BƯỚC 1.1 — BE Domain: Entities mới

#### File 41: `Domain/Enums/OtpType.cs`

```
Nội dung: "email_verification", "reset_password"
Phụ thuộc: Không
```

#### File 42: `Domain/Entities/RefreshToken.cs`

```
Mục đích: Entity lưu token làm mới phiên đăng nhập
Nội dung:
  - Property: Id, UserId, Token (SHA256 hash), ExpiresAt, CreatedAt, CreatedByIp, RevokedAt
  - Computed: IsExpired, IsRevoked, IsActive
  - Constructor: nhận userId, rawToken → hash bằng SHA256 trước khi lưu
  - Method: Revoke() — đặt RevokedAt = now
  - Method: MatchesToken(rawToken) — so sánh hash với FixedTimeEquals (chống timing attack)
  - Static: HashToken(token) — SHA256 hash
Phụ thuộc: User (navigation property)
```

#### File 43: `Domain/Entities/EmailOtp.cs`

```
Mục đích: Entity lưu mã OTP 6 số để xác thực email
Nội dung:
  - Property: Id, UserId, OtpCode (SHA256 hash), Type, ExpiresAt, IsUsed, CreatedAt
  - Computed: IsExpired
  - Constructor: nhận userId, rawOtpCode (6 số) → hash SHA256
  - Method: MarkAsUsed() — đặt IsUsed = true
  - Method: VerifyCode(code) — so sánh hash + FixedTimeEquals
Phụ thuộc: OtpType, User
```

### BƯỚC 1.2 — BE Application: Interfaces

> **Tại sao code interface trước handler?** Handler cần inject interface. Interface định nghĩa "cần gì", chưa cần biết "làm thế nào".

#### File 44: `Application/Interfaces/IUserRepository.cs`

```
Nội dung:
  - FindByIdAsync(Guid id) → User?
  - FindByEmailAsync(string email) → User?
  - FindByUsernameAsync(string username) → User?
  - ExistsByEmailAsync(string email) → bool
  - ExistsByUsernameAsync(string username) → bool
  - AddAsync(User user)
  - UpdateAsync(User user)
Phụ thuộc: User entity
```

#### File 45: `Application/Interfaces/IRefreshTokenRepository.cs`

```
Nội dung:
  - FindByTokenHashAsync(string hash) → RefreshToken?
  - AddAsync(RefreshToken token)
  - RevokeAllByUserIdAsync(Guid userId)
Phụ thuộc: RefreshToken entity
```

#### File 46: `Application/Interfaces/IEmailOtpRepository.cs`

```
Nội dung:
  - FindLatestOtpAsync(Guid userId, string type) → EmailOtp?
  - AddAsync(EmailOtp otp)
Phụ thuộc: EmailOtp entity
```

#### File 47-49: `IPasswordHasher.cs`, `ITokenService.cs`, `IEmailSender.cs`

```
IPasswordHasher:
  - HashPassword(string password) → string
  - VerifyPassword(string hash, string password) → bool

ITokenService:
  - GenerateAccessToken(User user) → string (JWT)
  - GenerateRefreshToken() → string (random 64 chars)

IEmailSender:
  - SendOtpAsync(string email, string otpCode, string type) → Task
```

### BƯỚC 1.3 — BE Application: Command Handlers

> **Tại sao code Handler sau Interface?** Handler inject interface qua constructor. Nếu interface chưa có → lỗi biên dịch.

#### File 50-52: `Features/Auth/Commands/Register/` (3 file)

```
RegisterCommand.cs:
  - Implement IRequest<Guid> (trả userId)
  - Property: Email, Username, Password, DisplayName, DateOfBirth

RegisterCommandValidator.cs:
  - Implement AbstractValidator<RegisterCommand>
  - Rules: Email format, Username 3-30 chars, Password 8+ chars có chữ hoa + số

RegisterCommandHandler.cs:
  - Implement IRequestHandler<RegisterCommand, Guid>
  - Constructor: inject IUserRepository, IPasswordHasher, IEmailOtpRepository, IEmailSender
  - Logic:
    1. Check trùng email → throw nếu trùng
    2. Check trùng username → throw nếu trùng
    3. Hash password bằng IPasswordHasher
    4. Tạo User entity mới
    5. Tạo EmailOtp (random 6 số)
    6. Gửi OTP qua IEmailSender
    7. Lưu User + OTP vào DB
    8. Trả userId
```

#### File 53-55: `Features/Auth/Commands/Login/` (3 file)

```
LoginCommand.cs:
  - Property: Email, Password
  - Implement IRequest<AuthResponse>

AuthResponse.cs:
  - Property: AccessToken, RefreshToken, UserId, DisplayName, Role

LoginCommandHandler.cs:
  - Constructor: inject IUserRepository, IPasswordHasher, ITokenService, IRefreshTokenRepository
  - Logic:
    1. Tìm user bằng email → throw nếu không tìm thấy
    2. Kiểm tra password hash → throw nếu sai
    3. Kiểm tra status != locked → throw nếu bị khóa
    4. Tạo access token (JWT, 60 phút)
    5. Tạo refresh token (random, 7 ngày)
    6. Lưu refresh token vào DB (hash SHA256)
    7. Trả AuthResponse
```

#### File 56-57: `Features/Auth/Commands/VerifyEmail/`

```
VerifyEmailCommand.cs: Property: UserId, OtpCode
VerifyEmailCommandHandler.cs:
  - Tìm OTP mới nhất → kiểm tra hết hạn, đã dùng → kiểm tra mã khớp
  - Nếu OK → user.Activate() + otp.MarkAsUsed()
```

#### File 58-61: `ForgotPassword/`, `ResetPassword/`, `RevokeToken/`, `RefreshToken/`

```
Tương tự pattern: Command + Handler (+ Validator nếu cần)
Mỗi file:
  - ForgotPassword: tìm user by email → tạo OTP type=reset_password → gửi email
  - ResetPassword: verify OTP → user.UpdatePassword(newHash)
  - RevokeToken: tìm refresh token → token.Revoke()
  - RefreshToken: verify old token → tạo token mới → revoke token cũ
```

### BƯỚC 1.4 — BE Infrastructure: Repositories + Security

#### File 62: `Persistence/Repositories/UserRepository.cs`

```
Mục đích: Implement IUserRepository bằng EF Core
Nội dung:
  - Constructor nhận ApplicationDbContext
  - Mỗi method = LINQ query trên _context.Users
  - FindByEmailAsync: _context.Users.FirstOrDefaultAsync(u => u.Email == email)
  - AddAsync: _context.Users.Add(user) + SaveChangesAsync
Phụ thuộc: ApplicationDbContext, IUserRepository
```

#### File 63-64: `RefreshTokenRepository.cs`, `EmailOtpRepository.cs` — tương tự pattern

#### File 65: `Persistence/Configurations/RefreshTokenConfiguration.cs`

```
EF config: tên bảng "refresh_tokens", foreign key UserId → users
```

#### File 66: `Persistence/Configurations/EmailOtpConfiguration.cs`

```
EF config: tên bảng "email_otps", foreign key UserId → users
```

#### File 67: `Security/Argon2idPasswordHasher.cs`

```
Mục đích: Hash mật khẩu bằng Argon2id (an toàn hơn bcrypt)
Nội dung: Implement IPasswordHasher, dùng Konscious.Security.Cryptography
```

#### File 68: `Security/JwtTokenService.cs`

```
Mục đích: Tạo JWT access token
Nội dung:
  - Constructor nhận IConfiguration (đọc Jwt:SecretKey, Issuer, Audience, ExpiryMinutes)
  - GenerateAccessToken: tạo JwtSecurityToken với claims (userId, email, role)
  - GenerateRefreshToken: RandomNumberGenerator 64 bytes → Base64
```

#### File 69: `Services/MockEmailSender.cs`

```
Mục đích: "Gửi" email giả lập — chỉ log ra console
Nội dung: Implement IEmailSender → Console.WriteLine($"OTP for {email}: {otpCode}")
Tại sao Mock? Chưa tích hợp SMTP thật. Xem OTP trong terminal output.
```

### BƯỚC 1.5 — BE API: Controller

#### File 70: `Controllers/AuthController.cs`

```
Mục đích: 8 endpoints xác thực
Nội dung:
  - [Route("api/v1/auth")]
  - Mỗi endpoint:
    1. POST "register" → Send(new RegisterCommand {...})
    2. POST "login" → Send(new LoginCommand {...})
    3. POST "logout" → Send(new RevokeTokenCommand {...})
    4. POST "refresh" → Send(new RefreshTokenCommand {...})
    5. POST "verify-email" → Send(new VerifyEmailCommand {...})
    6. POST "forgot-password" → Send(new ForgotPasswordCommand {...})
    7. POST "reset-password" → Send(new ResetPasswordCommand {...})
    8. POST "send-verification-email" → Send(new SendEmailVerificationOtpCommand {...})
  - Controller chỉ forward request cho MediatR, không chứa logic
Phụ thuộc: MediatR (IMediator)
```

> ⚠️ **Đừng quên:** Cập nhật `Infrastructure/DependencyInjection.cs` để đăng ký các repository mới!

### BƯỚC 1.6 — FE: Trang đăng ký/đăng nhập

#### File 71: `types/auth.ts`

```
Nội dung: TypeScript interface cho LoginRequest, RegisterRequest, AuthResponse, User
```

#### File 72: `actions/authActions.ts`

```
Nội dung: 6 hàm async gọi api.ts:
  - registerUser(data) → apiPost('/auth/register', data)
  - loginUser(data) → apiPost('/auth/login', data)
  - logoutUser() → apiPost('/auth/logout')
  - verifyEmail(data) → apiPost('/auth/verify-email', data)
  - forgotPassword(data) → apiPost('/auth/forgot-password', data)
  - resetPassword(data) → apiPost('/auth/reset-password', data)
Phụ thuộc: lib/api.ts, types/auth.ts
```

#### File 73: `store/authStore.ts`

```
Nội dung: Zustand store
  - State: user (info), accessToken, isAuthenticated
  - Actions: setAuth(response) — lưu token + user, clearAuth() — xóa, getToken() — đọc
  - Persist: lưu vào localStorage
Phụ thuộc: zustand
```

#### File 74: `components/auth/AuthSessionManager.tsx`

```
Mục đích: Component (client) chạy khi app load → kiểm tra token còn valid không
Nội dung: useEffect kiểm tra token expiry → nếu hết hạn → clearAuth → redirect login
Phụ thuộc: authStore
```

#### File 75: `components/layout/AuthLayout.tsx`

```
Mục đích: Layout cho trang auth (login, register...) — không có sidebar, chỉ có logo + form
```

#### File 76-80: 5 trang auth

```
login/page.tsx:
  - Form: email + password (react-hook-form + zod)
  - Submit → loginUser → setAuth → redirect home
  - Link: "Quên mật khẩu?", "Chưa có tài khoản? Đăng ký"

register/page.tsx:
  - Form: email + username + password + displayName + dateOfBirth
  - Submit → registerUser → redirect verify-email

verify-email/page.tsx:
  - Form: nhập mã OTP 6 số
  - Submit → verifyEmail → redirect login

forgot-password/page.tsx:
  - Form: nhập email
  - Submit → forgotPassword → hiện thông báo "Đã gửi OTP"

reset-password/page.tsx:
  - Form: OTP + mật khẩu mới + xác nhận mật khẩu
  - Submit → resetPassword → redirect login
```

**✅ KIỂM TRA GIAI ĐOẠN 1:**
```
1. Chạy migration: dotnet ef migrations add Auth → dotnet ef database update
2. Mở http://localhost:3000/vi/register → Đăng ký
3. Xem terminal BE → thấy OTP in ra (MockEmailSender)
4. Mở /vi/verify-email → Nhập OTP
5. Mở /vi/login → Đăng nhập → Thấy trang chủ (đã đăng nhập)
6. Mở DevTools → Application → localStorage → thấy token
```

---

## GIAI ĐOẠN 2: HỒ SƠ CÁ NHÂN (PROFILE)

> **Mục tiêu:** User đăng nhập → xem hồ sơ → sửa tên hiển thị, avatar, ngày sinh → lưu → reload vẫn đúng.  
> **Tính năng nhỏ nhất** — chỉ cần 2 handler + 1 controller + 1 trang FE.

---

### BƯỚC 2.1 — BE Application: Handlers

> **Không cần Enum/Entity mới** — dùng lại User entity đã có từ Giai đoạn 0.

#### File 81: `Application/Features/Profile/Queries/GetProfile/GetProfileQuery.cs`

```
Mục đích: Query lấy thông tin profile theo userId
Nội dung:
  - Implement IRequest<ProfileDto>
  - Property: UserId (Guid) — lấy từ JWT claims
Phụ thuộc: Không
```

#### File 82: `Application/Features/Profile/Queries/GetProfile/ProfileDto.cs`

```
Mục đích: DTO trả về cho Frontend (KHÔNG trả passwordHash)
Nội dung:
  - Property: Id, Email, Username, DisplayName, AvatarUrl, DateOfBirth,
    Level, Exp, Role, Status, GoldBalance, DiamondBalance, CreatedAt
Phụ thuộc: Không
```

#### File 83: `Application/Features/Profile/Queries/GetProfile/GetProfileQueryHandler.cs`

```
Mục đích: Handler xử lý query → trả ProfileDto
Nội dung:
  - Constructor: inject IUserRepository
  - Logic:
    1. Tìm user bằng userId → throw NotFoundException nếu không tìm thấy
    2. Map User entity → ProfileDto (thủ công hoặc dùng AutoMapper)
    3. Trả ProfileDto
Phụ thuộc: IUserRepository, ProfileDto
```

#### File 84: `Application/Features/Profile/Commands/UpdateProfile/UpdateProfileCommand.cs`

```
Nội dung:
  - Implement IRequest<Unit> (không trả data)
  - Property: UserId, DisplayName, AvatarUrl (nullable), DateOfBirth
Phụ thuộc: Không
```

#### File 85: `Application/Features/Profile/Commands/UpdateProfile/UpdateProfileCommandValidator.cs`

```
Nội dung:
  - Rule: DisplayName 2-50 ký tự, DateOfBirth phải > 13 tuổi
Phụ thuộc: UpdateProfileCommand
```

#### File 86: `Application/Features/Profile/Commands/UpdateProfile/UpdateProfileCommandHandler.cs`

```
Nội dung:
  - Constructor: inject IUserRepository
  - Logic:
    1. Tìm user → throw nếu không có
    2. Gọi user.UpdateProfile(displayName, avatarUrl, dateOfBirth)
    3. SaveChanges
Phụ thuộc: IUserRepository
```

### BƯỚC 2.2 — BE API: Controller

#### File 87: `Api/Controllers/ProfileController.cs`

```
Nội dung:
  - [Route("api/v1/profile")]
  - [Authorize] (yêu cầu đăng nhập)
  - GET "/" → Send(new GetProfileQuery { UserId = lấy từ JWT })
  - PATCH "/" → Send(new UpdateProfileCommand { ... })
  - Lấy UserId từ HttpContext.User.Claims
Phụ thuộc: MediatR
```

### BƯỚC 2.3 — FE: Trang Profile

#### File 88: `actions/profileActions.ts`

```
Nội dung:
  - getProfile() → apiGet('/profile')
  - updateProfile(data) → apiPatch('/profile', data)
Phụ thuộc: lib/api.ts
```

#### File 89: `app/[locale]/(user)/profile/page.tsx`

```
Nội dung:
  - useEffect → getProfile() → hiển thị thông tin
  - Form (react-hook-form): displayName, avatarUrl, dateOfBirth
  - Submit → updateProfile() → toast thành công → reload data
  - Hiển thị: Level, Exp, GoldBalance, DiamondBalance (read-only)
Phụ thuộc: profileActions, authStore, UI components
```

> ⚠️ **Cập nhật DI:** Không cần — UserRepository đã đăng ký từ Giai đoạn 1.

**✅ KIỂM TRA GIAI ĐOẠN 2:**
```
1. Đăng nhập → Mở /vi/profile
2. Thấy tên, email, level, số dư
3. Sửa tên hiển thị → Lưu → Reload → Tên phải thay đổi
4. Test Swagger: GET /api/v1/profile (gắn JWT header)
```

---

## GIAI ĐOẠN 3: VÍ & SỔ CÁI (WALLET)

> **Mục tiêu:** Xem số dư + lịch sử giao dịch. Chuẩn bị nền tảng tài chính cho AI + Escrow.

---

### BƯỚC 3.1 — BE Domain: Entity mới

#### File 90: `Domain/Entities/WalletTransaction.cs`

```
Mục đích: Entity sổ cái — ghi lại MỌI dao dịch tiền. Append-only (chỉ thêm, KHÔNG sửa/xóa)
Nội dung:
  - 13 property: Id, UserId, Currency, Type, Amount, BalanceBefore, BalanceAfter,
    ReferenceSource, ReferenceId, Description, MetadataJson, IdempotencyKey, CreatedAt
  - Constructor: private (chống tạo bừa từ bên ngoài)
  - Factory method: static Create(...) — điểm tạo duy nhất
  - Protected constructor cho EF Core
Phụ thuộc: Không (dùng string cho currency, type — không cần import Enum)
Tại sao cần? Ghi vết mọi thay đổi tiền → audit trail → chống gian lận
```

### BƯỚC 3.2 — BE Application: Interfaces + Handlers

#### File 91: `Application/Interfaces/IWalletRepository.cs`

```
Nội dung:
  - GetUserWithWalletAsync(Guid userId) → User? (Include Wallet)
  - UpdateWalletAsync(User user)
Phụ thuộc: User entity
```

#### File 92: `Application/Interfaces/ILedgerRepository.cs`

```
Nội dung:
  - AddTransactionAsync(WalletTransaction tx)
  - GetTransactionsAsync(Guid userId, int page, int size) → PaginatedList<WalletTransaction>
  - GetBalanceSumAsync(Guid userId) → (long gold, long diamond) (để đối soát)
Phụ thuộc: WalletTransaction entity, PaginatedList
```

#### File 93-94: `Features/Wallet/Queries/GetWalletBalance/` (Query + Handler)

```
GetWalletBalanceQuery: Property UserId
GetWalletBalanceQueryHandler:
  - Logic: Tìm user → trả WalletBalanceDto { GoldBalance, DiamondBalance, FrozenDiamondBalance }
```

#### File 95-96: `Features/Wallet/Queries/GetLedgerList/` (Query + Handler)

```
GetLedgerListQuery: Property UserId, Page, PageSize
GetLedgerListQueryHandler:
  - Logic: Gọi ILedgerRepository.GetTransactionsAsync → trả PaginatedList<LedgerDto>
  - LedgerDto: Id, Currency, Type, Amount, BalanceBefore, BalanceAfter, Description, CreatedAt
```

### BƯỚC 3.3 — BE Infrastructure: Repositories + Config

#### File 97: `Persistence/Configurations/WalletTransactionConfiguration.cs`

```
EF config: tên bảng "wallet_transactions", FK userId → users, unique IdempotencyKey
```

#### File 98: `Persistence/Repositories/WalletRepository.cs`

```
Implement IWalletRepository:
  - GetUserWithWalletAsync: _context.Users.Include(u => u.Wallet).FirstOrDefault(...)
```

#### File 99: `Persistence/Repositories/LedgerRepository.cs`

```
Implement ILedgerRepository:
  - GetTransactionsAsync: Query OrderByDescending(CreatedAt).Skip().Take()
  - AddTransactionAsync: _context.WalletTransactions.Add(tx)
```

> ⚠️ **Cập nhật file:**
> - `ApplicationDbContext.cs` → thêm `DbSet<WalletTransaction> WalletTransactions`
> - `Infrastructure/DependencyInjection.cs` → thêm `services.AddScoped<IWalletRepository, WalletRepository>()` + `services.AddScoped<ILedgerRepository, LedgerRepository>()`

### BƯỚC 3.4 — BE API

#### File 100: `Controllers/WalletController.cs`

```
- [Route("api/v1/wallet")] [Authorize]
- GET "balance" → GetWalletBalance
- GET "ledger?page=1&size=20" → GetLedgerList
```

### BƯỚC 3.5 — FE

#### File 101: `types/wallet.ts`

```
Interface: WalletBalance, LedgerItem, PaginatedResponse<T>
```

#### File 102: `actions/walletActions.ts`

```
- getWalletBalance() → apiGet('/wallet/balance')
- getLedgerList(page, size) → apiGet('/wallet/ledger?page=...&size=...')
```

#### File 103: `store/walletStore.ts`

```
Zustand: state goldBalance, diamondBalance, frozenDiamondBalance
Actions: fetchBalance() → gọi API + cập nhật state
```

#### File 104: `components/common/WalletWidget.tsx`

```
Mục đích: Widget hiển thị số dư ở Navbar (Gold icon + số, Diamond icon + số)
Click → redirect /wallet
```

#### File 105: `app/[locale]/(user)/wallet/page.tsx`

```
Nội dung:
  - Hiển thị 3 ô: Gold, Diamond, Frozen Diamond
  - Bảng lịch sử giao dịch có phân trang
  - Nút "Nạp tiền" (link đến deposit — code sau)
```

**✅ KIỂM TRA GIAI ĐOẠN 3:**
```
1. Đăng nhập → Navbar thấy "0 Gold, 0 Diamond"
2. Mở /vi/wallet → Thấy 3 ô số dư + bảng lịch sử trống
3. Test Swagger: GET /api/v1/wallet/balance
4. Test Swagger: GET /api/v1/wallet/ledger
```

---

## GIAI ĐOẠN 4: RÚT BÀI TAROT (READING)

> **Mục tiêu:** User rút bài → lật bài → xem ý nghĩa → bộ sưu tập tăng dần.  
> **Tính năng đầu tiên dùng MongoDB** — phiên rút bài lưu dạng document.

---

### BƯỚC 4.1 — BE Domain

#### File 106: `Domain/Enums/SpreadType.cs`

```
Nội dung: "single" (1 lá), "three_card" (3 lá), "celtic_cross" (10 lá)
```

#### File 107: `Domain/Enums/ReadingSessionStatus.cs`

```
Nội dung: "initialized" (đã rút, chưa lật), "revealed" (đã lật), "streaming" (đang AI), "completed"
```

### BƯỚC 4.2 — BE Infrastructure: MongoDB Documents

> **Tại sao code MongoDB document trước Interface?** Interface cần biết return type.

#### File 108: `Persistence/MongoDocuments/ReadingSessionDocument.cs`

```
Mục đích: Schema phiên rút bài lưu MongoDB
Nội dung:
  - [BsonId] Id (string ObjectId)
  - UserId, SpreadType, Question
  - Cards: List<CardDrawn> (cardId, code, position, isReversed, name)
  - AiInterpretation (string — ghi sau khi AI stream xong)
  - FollowupMessages: List<ChatMessage> (role, content)
  - Status, Locale, CreatedAt, CompletedAt
Phụ thuộc: MongoDB.Driver, BsonElement attributes
```

#### File 109: `Persistence/MongoDocuments/CardCatalogDocument.cs`

```
Mục đích: Lá bài Tarot — 78 document tĩnh (seed 1 lần)
Nội dung:
  - [BsonId] Id (int: 0-77)
  - Code, Name (LocalizedName: vi/en/zh), Arcana, Suit, Number, Element
  - Meanings: CardMeanings { Upright (LocalizedText), Reversed (LocalizedText) }
```

#### File 110: `Persistence/MongoDocuments/UserCollectionDocument.cs`

```
Mục đích: Bộ sưu tập bài đã thu thập
Nội dung: Id, UserId, CardId, Code, FirstObtainedAt, Count
```

### BƯỚC 4.3 — BE Application: Interfaces

#### File 111: `Application/Interfaces/IReadingSessionRepository.cs`

```
- CreateAsync(ReadingSessionDocument doc) → string (id)
- GetByIdAsync(string id) → ReadingSessionDocument?
- UpdateAsync(ReadingSessionDocument doc)
- GetByUserIdAsync(string userId, int page, int size) → List<ReadingSessionDocument>
```

#### File 112: `Application/Interfaces/ICardsCatalogRepository.cs`

```
- GetAllAsync() → List<CardCatalogDocument>
- GetByIdAsync(int cardId) → CardCatalogDocument?
```

#### File 113: `Application/Interfaces/IUserCollectionRepository.cs`

```
- UpsertCardAsync(string userId, int cardId, string code)
- GetCollectionAsync(string userId) → List<UserCollectionDocument>
```

#### File 114: `Application/Interfaces/IRngService.cs`

```
- DrawCards(int count, List<int> allCardIds) → List<DrawnCard>
  (mỗi DrawnCard: CardId, IsReversed — random cryptographic)
```

### BƯỚC 4.4 — BE Application: Handlers

#### File 115-116: `Features/Reading/Commands/InitSession/` (Command + Handler)

```
InitSessionCommand: UserId, SpreadType, Question, Locale
InitSessionCommandHandler:
  - Logic:
    1. Xác định số lá theo SpreadType (single=1, three_card=3, celtic_cross=10)
    2. Lấy danh sách 78 lá từ ICardsCatalogRepository
    3. Gọi IRngService.DrawCards(count) → random lá + upright/reversed
    4. Tạo ReadingSessionDocument { status = "initialized", cards = [...] }
    5. Lưu MongoDB
    6. Trả sessionId
```

#### File 117-118: `Features/Reading/Commands/RevealSession/` (Command + Handler)

```
RevealSessionCommand: SessionId, UserId
RevealSessionCommandHandler:
  - Logic:
    1. Tìm session → kiểm tra thuộc đúng user
    2. Kiểm tra status == "initialized"
    3. Cập nhật status = "revealed"
    4. Upsert mỗi lá vào UserCollection (bộ sưu tập)
    5. Trả danh sách lá bài + ý nghĩa
```

#### File 119-120: `Features/Reading/Queries/GetCollection/` (Query + Handler)

```
GetCollectionQuery: UserId
Handler: Gọi IUserCollectionRepository.GetCollectionAsync → trả list
```

### BƯỚC 4.5 — BE Infrastructure: Repositories

#### File 121: `Persistence/Repositories/MongoReadingSessionRepository.cs`

```
Implement IReadingSessionRepository bằng MongoDbContext:
  - _collection = mongoDbContext.ReadingSessions
  - CreateAsync: InsertOneAsync(doc)
  - GetByIdAsync: Find(x => x.Id == id).FirstOrDefault()
```

#### File 122-123: `MongoCardsCatalogRepository.cs`, `MongoUserCollectionRepository.cs` — tương tự

#### File 124: `Services/RngService.cs`

```
Implement IRngService:
  - Dùng RandomNumberGenerator (cryptographic) — KHÔNG dùng Random()
  - Shuffle Fisher-Yates → lấy N lá đầu
  - Mỗi lá random bool isReversed (50/50)
```

> ⚠️ **Cập nhật:**
> - `MongoDbContext.cs` → thêm 3 collection mới
> - `Infrastructure/DependencyInjection.cs` → thêm 3 Mongo repos + RngService

### BƯỚC 4.6 — BE API

#### File 125: `Controllers/TarotController.cs`

```
- [Route("api/v1/sessions")] [Authorize]
- POST "/" → InitSession
- PATCH "{id}/reveal" → RevealSession
- GET "/collection" → GetCollection
```

### BƯỚC 4.7 — FE

#### File 126: `lib/tarotData.ts`

```
Mục đích: Dữ liệu 78 lá bài phía client (ảnh, tên, mô tả ngắn)
Nội dung: Mảng 78 object { id, code, name, image, description }
Tại sao ở client? Để hiển thị animation rút bài offline, không cần gọi API
```

#### File 127: `types/reading.ts`

```
Interface: ReadingSession, DrawnCard, SpreadType
```

#### File 128: `actions/readingActions.ts`

```
- initSession(spreadType, question, locale) → apiPost('/sessions', data)
- revealSession(sessionId) → apiPatch('/sessions/{id}/reveal')
```

#### File 129: `actions/collectionActions.ts`

```
- getCollection() → apiGet('/sessions/collection')
```

#### File 130: `app/[locale]/(user)/reading/page.tsx`

```
Nội dung:
  - Chọn kiểu trải bài (single/three_card/celtic_cross)
  - Nhập câu hỏi
  - Nhấn "Rút bài" → animation xáo bài → hiện lá úp
  - Nhấn "Lật bài" → hiện lá ngửa + ý nghĩa + trạng thái upright/reversed
  - Nút "Giải bài AI" (link đến AI stream — Giai đoạn 5)
```

#### File 131: `app/[locale]/(user)/collection/page.tsx`

```
Nội dung:
  - Grid 78 lá: đã sưu tập → màu + hover effect, chưa có → xám mờ
  - Hiển thị count (rút được bao nhiêu lần) + ngày đầu tiên thu thập
  - Badge: "12/78 lá" thanh progress bar
```

**✅ KIỂM TRA GIAI ĐOẠN 4:**
```
1. Mở /vi/reading → Chọn "3 lá" → Nhập câu hỏi → Rút bài
2. Thấy 3 lá úp → Nhấn "Lật" → Thấy tên + ý nghĩa
3. Mở /vi/collection → Thấy 3 lá vừa rút đã sáng, 75 lá còn xám
4. Rút bài lần 2 → Collection cập nhật count
```

---

## GIAI ĐOẠN 5: AI GIẢI BÀI + LỊCH SỬ

> **Mục tiêu:** Click "Giải bài AI" → stream text SSE → follow-up chat → lưu lịch sử.  
> **Tính năng phức tạp nhất ở BE** — cần atomicity (trừ tiền trước, hoàn nếu lỗi).

---

### BƯỚC 5.1 — BE Domain

#### File 132: `Domain/Enums/AiRequestStatus.cs`

```
Nội dung: "requested", "completed", "failed"
```

#### File 133: `Domain/Entities/AiRequest.cs`

```
Mục đích: Entity SQL theo dõi mỗi lần gọi AI (tracking chi phí + trạng thái)
Nội dung (20 property):
  - Id, UserId, ReadingSessionRef (trỏ MongoDB)
  - FollowupSequence (null=lần đầu, 1-5=follow-up)
  - Status, FirstTokenAt, CompletionMarkerAt, FinishReason
  - RetryCount, PromptVersion, PolicyVersion
  - CorrelationId, TraceId
  - ChargeGold, ChargeDiamond (ghi giá lúc tạo)
  - RequestedLocale, ReturnedLocale, FallbackReason
  - IdempotencyKey, CreatedAt, UpdatedAt
Phụ thuộc: AiRequestStatus
```

#### File 134: `Domain/Services/FollowupPricingService.cs`

```
Mục đích: Tính giá follow-up theo thứ tự (lần 1 = 1 diamond, lần 2 = 2, ...)
Nội dung: static method Calculate(short sequence) → long cost = sequence
Phụ thuộc: EconomyConstants
```

### BƯỚC 5.2 — BE Application: Interfaces

#### File 135: `Application/Interfaces/IAiProvider.cs`

```
Nội dung:
  - StreamReadingInterpretationAsync(prompt, onChunk, cancellationToken) → AiResult
    - onChunk: Action<string> callback mỗi khi nhận 1 đoạn text từ AI
    - AiResult: { Success, FinishReason, Error }
Phụ thuộc: Không
```

#### File 136: `Application/Interfaces/IAiRequestRepository.cs`

```
- AddAsync(AiRequest req)
- UpdateAsync(AiRequest req)
- GetBySessionRefAsync(string ref) → List<AiRequest>
- CountInFlightAsync(Guid userId) → int (đếm request đang chạy)
```

#### File 137: `Application/Interfaces/IAiProviderLogRepository.cs`

```
- AddLogAsync(AiProviderLogDocument log) — lưu MongoDB
```

### BƯỚC 5.3 — BE Application: Handlers

#### File 138-139: `Features/Reading/Commands/StreamReading/` (Command + Handler)

```
StreamReadingCommand: SessionId, UserId, Locale, HttpResponse (để stream SSE)
StreamReadingCommandHandler — LOGIC PHỨC TẠP NHẤT:
  1. Tìm session MongoDB → kiểm tra status = "revealed"
  2. Kiểm tra rate limit (Redis: 1 request mỗi 30 giây)
  3. Kiểm tra in-flight cap (tối đa 2 AI request cùng lúc)
  4. Tính giá: Gold hoặc Diamond? Đủ tiền không?
  5. TRỪ TIỀN TRƯỚC (atomic):
     - user.Debit(currency, amount) hoặc user.FreezeDiamond(amount)
     - Ghi WalletTransaction
  6. Tạo AiRequest entity (status = "requested")
  7. Lưu DB (Commit transaction)
  8. Gọi IAiProvider.StreamReadingInterpretationAsync:
     - Mỗi chunk → ghi SSE "data: {text}\n\n" vào HttpResponse
     - Nếu thành công → cập nhật status = "completed", ghi interpretation vào MongoDB
     - Nếu lỗi/timeout → status = "failed" → HOÀN TIỀN (user.Credit hoặc RefundFrozenDiamond)
  9. Ghi AiProviderLog MongoDB (token count, latency)
```

#### File 140-141: `Features/Reading/Commands/FollowupReading/` (Command + Handler)

```
Tương tự StreamReading nhưng:
  - Kiểm tra followupSequence ≤ 5
  - Tính giá theo FollowupPricingService
  - Prompt bao gồm lịch sử chat trước đó
```

#### File 142-144: `Features/History/Queries/` (3 Query + Handler)

```
GetHistoryList: Lấy danh sách phiên theo userId, phân trang
GetHistoryDetail: Lấy chi tiết 1 phiên (cards + AI interpretation + followups)
GetHistoryStats: Đếm tổng phiên, tổng lá đã rút, AI calls
```

### BƯỚC 5.4 — BE Infrastructure

#### File 145: `Services/Ai/OpenAiProvider.cs`

```
Mục đích: Gọi API OpenAI (GPT-4o-mini) stream SSE
Nội dung:
  - Implement IAiProvider
  - Constructor: inject IHttpClientFactory, IConfiguration
  - Build prompt: system message (role tarot reader) + user message (cards + question)
  - Gọi POST /v1/chat/completions với stream=true
  - Parse SSE response → gọi callback onChunk cho mỗi đoạn
  - Retry: tối đa 3 lần, backoff 1s-2s-4s
  - Timeout: 120 giây
Phụ thuộc: HttpClient, IConfiguration (đọc ApiKey, Model)
```

#### File 146: `Persistence/MongoDocuments/AiProviderLogDocument.cs`

```
Schema: Id, UserId, ReadingRef, AiRequestRef, Model, Tokens (in/out), LatencyMs,
  PromptVersion, PolicyVersion, Status, ErrorCode, TraceId, CreatedAt
```

#### File 147: `Persistence/Repositories/MongoAiProviderLogRepository.cs` + `AiRequestRepository.cs`

> ⚠️ **Cập nhật:**
> - `ApplicationDbContext.cs` → thêm `DbSet<AiRequest>`
> - `Infrastructure/DependencyInjection.cs` → thêm repos + OpenAiProvider

### BƯỚC 5.5 — BE API

#### File 148: `Controllers/AiController.cs`

```
- [Route("api/v1/sessions")] [Authorize]
- GET "{sessionId}/stream?access_token=..." → SSE endpoint
  - Set Response.ContentType = "text/event-stream"
  - Gọi StreamReadingCommand
  - Hỗ trợ access_token từ query string (vì EventSource không gắn header được)
```

#### File 149: `Controllers/HistoryController.cs`

```
- [Route("api/v1/history")] [Authorize]
- GET "/?page=1&size=20" → danh sách
- GET "/{id}" → chi tiết
- GET "/stats" → thống kê
```

### BƯỚC 5.6 — FE

#### File 150: `components/reading/AiInterpretationStream.tsx`

```
Mục đích: Component SSE — hiển thị text streaming từ AI
Nội dung:
  - Tạo EventSource kết nối /sessions/{id}/stream?access_token=...
  - onmessage → append text vào state → render markdown
  - oncomplete → hiện nút "Hỏi tiếp" (follow-up)
  - onerror → thông báo lỗi + retry
  - Follow-up form: input + submit → gọi API follow-up → stream lại
Phụ thuộc: authStore (lấy token), react-markdown (render)
```

#### File 151: `actions/historyActions.ts`

```
- getHistoryList(page, size) → apiGet('/history?...')
- getHistoryDetail(id) → apiGet('/history/{id}')
```

#### File 152: Cập nhật `reading/page.tsx` — thêm nút "Giải bài AI" sau khi lật bài

#### File 153: `app/[locale]/reading/session/[id]/page.tsx`

```
Trang hiển thị AI stream — mount <AiInterpretationStream sessionId={id} />
```

#### File 154-155: `app/[locale]/(user)/reading/history/page.tsx` + `[id]/page.tsx`

```
Danh sách lịch sử: Mỗi item hiện ngày, câu hỏi, kiểu trải, số lá
Chi tiết: Hiện lá bài + AI interpretation + follow-up messages
```

**✅ KIỂM TRA GIAI ĐOẠN 5:**
```
1. Rút bài → Lật → Nhấn "Giải bài AI"
2. Text AI xuất hiện dần dần (streaming)
3. Nhấn "Hỏi tiếp" → Nhập câu follow-up → Stream tiếp
4. Mở /vi/reading/history → Thấy phiên vừa xong
5. Kiểm tra ví: Gold hoặc Diamond đã bị trừ
6. Kiểm tra ledger: có giao dịch "spend"
```

---

## GIAI ĐOẠN 6: NẠP TIỀN + PHÁP LÝ + KHUYẾN MÃI

> **Mục tiêu:** Nạp Diamond bằng VNĐ → webhook xác nhận → cộng ví. Consent check.

---

### BƯỚC 6.1 — BE Domain: 3 Entities mới

#### File 156: `Domain/Entities/DepositOrder.cs`

```
9 property: Id, UserId, AmountVnd, DiamondAmount, Status, TransactionId, FxSnapshot, CreatedAt, ProcessedAt
Methods: MarkAsSuccess(transactionId), MarkAsFailed(transactionId)
```

#### File 157: `Domain/Entities/DepositPromotion.cs`

```
5 property: Id, MinAmountVnd, BonusDiamond, IsActive, CreatedAt
Methods: Update(min, bonus, active), SetActive(bool)
```

#### File 158: `Domain/Entities/UserConsent.cs`

```
7 property: Id, UserId, DocumentType, Version, ConsentedAt, IpAddress, UserAgent
```

### BƯỚC 6.2 — BE Application

#### File 159-160: `Interfaces/IDepositOrderRepository.cs` + `IDepositPromotionRepository.cs` + `IUserConsentRepository.cs`

#### File 161: `Interfaces/IPaymentGatewayService.cs`

```
- VerifyWebhookSignature(payload, signature) → bool
```

#### File 162-163: `Features/Deposit/Commands/CreateDepositOrder/` (Command + Handler)

```
Logic:
  1. Nhận amountVnd → tính diamondAmount (÷ 1000)
  2. Tìm promotion active (bonus nếu đủ ngưỡng)
  3. Tạo DepositOrder { status = "Pending" }
  4. Trả orderId + paymentUrl (redirect đến cổng thanh toán)
```

#### File 164-165: `Features/Deposit/Commands/ProcessDepositWebhook/` (Command + Handler)

```
Logic:
  1. Verify HMAC signature từ payment gateway
  2. Tìm order → kiểm tra chưa xử lý
  3. Success: order.MarkAsSuccess → user.Credit("diamond", amount, "deposit") → ghi Ledger
  4. Failed: order.MarkAsFailed
```

#### File 166-167: `Features/Legal/Commands/RecordConsent/` + `Queries/CheckConsent/`

```
RecordConsent: Lưu UserConsent (type, version, IP, userAgent)
CheckConsent: Kiểm tra user đã consent loại nào chưa
```

#### File 168-169: `Features/Promotions/Queries/GetActivePromotions/` + `Commands/ManagePromotion/`

### BƯỚC 6.3 — BE Infrastructure

#### File 170-172: 3 EF Configurations + 3 Repositories

#### File 173: `Services/HmacPaymentGatewayService.cs`

```
Implement IPaymentGatewayService: verify HMAC-SHA256 signature
```

> ⚠️ Cập nhật: DbContext (3 DbSet mới), DI (3 repos + PaymentGateway)

### BƯỚC 6.4 — BE API

#### File 174: `Controllers/DepositController.cs` — 2 endpoints: createOrder + webhook (AllowAnonymous)
#### File 175: `Controllers/LegalController.cs` — 2 endpoints: check + record
#### File 176: `Controllers/PromotionsController.cs` — 4 endpoints: list, create, update, toggle

### BƯỚC 6.5 — FE

#### File 177-179: `actions/depositActions.ts`, `legalActions.ts`, `promotionActions.ts`
#### File 180: `components/legal/ConsentModal.tsx` — Modal bắt đồng ý TOS trước khi nạp
#### File 181: `app/[locale]/(user)/wallet/deposit/page.tsx` — Form chọn gói nạp + hiện khuyến mãi
#### File 182-184: `app/[locale]/legal/tos/page.tsx`, `privacy/page.tsx`, `ai-disclaimer/page.tsx`

**✅ KIỂM TRA GIAI ĐOẠN 6:**
```
1. Mở /vi/wallet/deposit → Chọn gói 50,000đ
2. Consent modal hiện → Đồng ý
3. (Dev) Test webhook qua Swagger → truyền orderId + signature
4. Kiểm tra ví: Diamond đã tăng + bonus khuyến mãi
5. Kiểm tra ledger: có giao dịch "deposit"
```

---

## GIAI ĐOẠN 7: READER + CHAT + ESCROW + RÚT TIỀN + MFA

> **Mục tiêu:** Hệ thống thuê Reader qua chat, escrow giữ tiền, rút tiền.  
> **Giai đoạn lớn nhất** — chia thành 5 phần con.

---

### PHẦN 7A: ĐƠN XIN READER

#### File 185: `Persistence/MongoDocuments/ReaderRequestDocument.cs`

```
Schema: Id, UserId, DisplayName, Bio, Specialties, Experience, Status, AdminNote, CreatedAt
```

#### File 186: `Persistence/MongoDocuments/ReaderProfileDocument.cs`

```
Schema: Id, UserId, DisplayName, AvatarUrl, Status (online/offline), Bio (localized),
  Specialties, Pricing { DiamondPerQuestion }, Stats { AvgRating, TotalReviews },
  Badges, IsDeleted, CreatedAt
```

#### File 187-188: `Interfaces/IReaderRequestRepository.cs` + `IReaderProfileRepository.cs`

#### File 189-190: `Features/Reader/Commands/SubmitReaderRequest/` (Command + Handler)

```
Logic: Kiểm tra chưa có đơn pending → Tạo MongoDB document → Cập nhật user.ReaderStatus = "pending"
```

#### File 191-192: `Features/Reader/Queries/GetReaderList/` + `GetReaderDetail/`

#### File 193-194: `Features/Reader/Commands/UpdateReaderProfile/` + `ToggleReaderStatus/`

#### File 195: `Controllers/ReaderController.cs`

```
6 endpoints: submitRequest, getReaders, getReaderDetail, updateProfile, toggleOnline, getMyProfile
```

#### FE (File 196-200):
- `types/reader.ts`, `actions/readerActions.ts`  
- `app/[locale]/(user)/readers/page.tsx` — Danh sách Reader
- `app/[locale]/(user)/readers/[id]/page.tsx` — Chi tiết Reader
- `app/[locale]/(user)/reader/apply/page.tsx` — Form đăng ký Reader
- `app/[locale]/(user)/profile/reader/page.tsx` — Quản lý profile Reader

### PHẦN 7B: CHAT REALTIME (SignalR)

#### File 201: `Persistence/MongoDocuments/ConversationDocument.cs`

```
Schema: Id, Participants[], Type, Status, LastMessageAt, LastMessagePreview, UnreadCounts{}, CreatedAt
```

#### File 202: `Persistence/MongoDocuments/ChatMessageDocument.cs`

```
Schema: Id, ConversationId, SenderId, Type (text/offer/system), Content, Metadata{}, ReadBy[], CreatedAt
```

#### File 203-204: `Interfaces/IConversationRepository.cs` + `IChatMessageRepository.cs`

#### File 205-207: `Features/Chat/Commands/CreateConversation/`, `SendMessage/`, `Queries/GetConversations/`

```
CreateConversation: Kiểm tra 2 user hợp lệ + không trùng → tạo Conversation MongoDB
SendMessage: Lưu message MongoDB → broadcast qua SignalR
GetConversations: Trả danh sách + unread count
```

#### File 208: `Hubs/ChatHub.cs`

```
Mục đích: SignalR Hub cho chat realtime
Nội dung:
  - OnConnectedAsync: join group "user_{userId}"
  - SendMessage: validate + lưu DB + gọi Clients.Group("user_{receiverId}").SendAsync("ReceiveMessage", ...)
  - MarkAsRead: cập nhật unreadCount
Phụ thuộc: IChatMessageRepository, IConversationRepository
```

> ⚠️ **Cập nhật Program.cs:** `app.MapHub<ChatHub>("/api/v1/chat")`

#### File 209: `Controllers/ConversationController.cs`

```
3 endpoints: createConversation, getConversations, getMessages
```

#### FE (File 210-214):
- `types/chat.ts`, `actions/chatActions.ts`
- `lib/signalr.ts` — SignalR client connection
- `app/[locale]/(user)/chat/page.tsx` — Inbox
- `app/[locale]/(user)/chat/[id]/page.tsx` — Chat room

### PHẦN 7C: ESCROW (GIỮ TIỀN)

#### File 215-216: `Domain/Entities/ChatFinanceSession.cs` + `ChatQuestionItem.cs` — đã có Entity

#### File 217: `Domain/Enums/QuestionItemStatus.cs`

```
"pending", "accepted", "released", "refunded", "disputed"
```

#### File 218: `Interfaces/IChatFinanceRepository.cs`

```
- CreateSessionAsync, GetSessionAsync, CreateQuestionItemAsync
- UpdateQuestionItemStatusAsync, GetPendingExpiredItems
```

#### File 219-222: `Features/Escrow/Commands/`

```
CreateOffer: User gửi offer → trừ diamond → tạo QuestionItem (status=pending, offer_expires_at=+24h)
AcceptOffer: Reader chấp nhận → status=accepted, reader_response_due_at=+24h
ReleasePayment: User xác nhận OK → tiền chuy ển cho Reader (status=released)
RefundPayment: Timeout hoặc dispute → hoàn tiền cho User
```

#### File 223: `BackgroundJobs/EscrowTimerService.cs`

```
Mục đích: Background service chạy mỗi 60 giây, kiểm tra:
  - QuestionItem pending + offer_expires_at < now → auto refund
  - QuestionItem accepted + reader_response_due_at < now → auto refund
  - QuestionItem replied + auto_release_at < now → auto release cho Reader
Nội dung: Implement IHostedService (BackgroundService base)
```

#### File 224: `Controllers/EscrowController.cs` — 6 endpoints

#### FE (File 225-228):
- `types/escrow.ts`, `actions/escrowActions.ts`
- `components/chat/EscrowPanel.tsx` — Panel trong chat room hiển thị offer/status
- `components/chat/DisputeButton.tsx` — Nút mở dispute

### PHẦN 7D: RÚT TIỀN

#### File 229: Đã có `Domain/Entities/WithdrawalRequest.cs`

#### File 230: `Interfaces/IWithdrawalRepository.cs`

#### File 231-232: `Features/Withdrawal/Commands/CreateWithdrawal/`

```
Logic:
  1. Kiểm tra role = tarot_reader
  2. Kiểm tra amountDiamond ≥ 50
  3. Kiểm tra chưa rút hôm nay (business_date_utc unique per user)
  4. Tính: amountVnd = amount × 1000, feeVnd = 10%, netVnd = amountVnd - feeVnd
  5. Trừ Diamond → Ghi ledger → Tạo WithdrawalRequest status="pending"
```

#### File 233-234: `Features/Withdrawal/Queries/GetWithdrawalHistory/`
#### File 235: `Controllers/WithdrawalController.cs` — 2 endpoints: create, history

#### FE (File 236-237):
- `actions/withdrawalActions.ts`
- `app/[locale]/(user)/wallet/withdraw/page.tsx` — Form rút tiền (chỉ hiện cho Reader)

### PHẦN 7E: MFA (Xác thực 2 lớp)

#### File 238: `Application/Interfaces/IMfaService.cs`

```
- GenerateSecret() → MfaSetup { Secret, QrCodeUri }
- ValidateTotp(encryptedSecret, code) → bool
- GenerateBackupCodes() → List<string>
```

#### File 239: `Infrastructure/Services/TotpMfaService.cs`

```
TOTP 6 số, refresh mỗi 30 giây, dùng Otp.NET library
```

#### File 240-243: `Features/Mfa/Commands/` — EnableMfa, VerifyMfaSetup, DisableMfa, ValidateMfaLogin
#### File 244: `Controllers/MfaController.cs` — 4 endpoints

#### FE (File 245-247):
- `actions/mfaActions.ts`
- `app/[locale]/(user)/profile/mfa/page.tsx` — Trang bật/tắt MFA + QR code
- `components/auth/MfaChallengeModal.tsx` — Modal nhập mã 6 số khi login

### PHẦN 7F: REPORT

#### File 248: `Persistence/MongoDocuments/ReportDocument.cs`
#### File 249: `Interfaces/IReportRepository.cs`
#### File 250-251: `Features/Chat/Commands/CreateReport/`
#### File 252: `Controllers/ReportController.cs` — 1 endpoint

**✅ KIỂM TRA GIAI ĐOẠN 7:**
```
1. Tạo tài khoản A (user) + B (reader)
2. B submit reader request → Admin approve (tạm dùng Swagger)
3. A mở /readers → Thấy B → Click vào → "Nhắn tin"
4. Chat realtime: A gửi tin → B thấy ngay (2 tab browser)
5. A gửi offer 5 diamond → B accept → B reply → A confirm release
6. B mở /wallet/withdraw → Rút 50 diamond → Tạo đơn
7. Mở /profile/mfa → Bật MFA → Scan QR → Logout → Login lại → Phải nhập mã 6 số
```

---

## GIAI ĐOẠN 8: ADMIN PANEL

> **Mục tiêu:** Admin quản lý toàn bộ hệ thống qua dashboard.

---

### BƯỚC 8.1 — BE Application

#### File 253: `Interfaces/IAdminRepository.cs`

```
Nội dung:
  - GetAllUsersAsync(page, size, filters) → PaginatedList<User>
  - GetDepositOrdersAsync(filters) → PaginatedList<DepositOrder>
  - GetWithdrawalRequestsAsync(filters) → PaginatedList<WithdrawalRequest>
  - GetReaderRequestsAsync(status) → List<ReaderRequestDocument>
  - GetDisputedItemsAsync() → List<ChatQuestionItem>
  - RunLedgerMismatchCheckAsync() → List<MismatchReport>
```

#### File 254-258: `Features/Admin/Commands/`

```
AddBalanceCommand: Admin cộng Gold/Diamond cho user (+ ghi ledger)
  - Logic: Tìm user → user.Credit(currency, amount, "admin_add") → ghi WalletTransaction

ApproveReaderCommand: Duyệt đơn Reader
  - Logic: Tìm đơn MongoDB → approve → user.ApproveAsReader() → tạo ReaderProfile MongoDB

ProcessDepositCommand: Admin xác nhận nạp thủ công (trường hợp webhook không về)
  - Logic: Tìm order → markAsSuccess → user.Credit diamond

ResolveDisputeCommand: Giải quyết tranh chấp escrow
  - Logic: ChatQuestionItem → quyết release cho Reader HOẶC refund cho User

ToggleUserLockCommand: Khóa/mở khóa tài khoản
  - Logic: user.Lock() hoặc user.Unlock()

ProcessWithdrawalCommand: Admin duyệt + xử lý rút tiền
  - Logic: status → approved → paid hoặc rejected (nếu rejected → hoàn diamond)
```

#### File 259-262: `Features/Admin/Queries/`

```
ListUsersQuery: Phân trang + filter (role, status, search)
ListDepositsQuery: Filter theo status + date range
ListReaderRequestsQuery: Filter theo status
GetLedgerMismatchQuery: Đối soát — so tổng wallet vs tổng ledger per user
```

### BƯỚC 8.2 — BE Infrastructure

#### File 263: `Persistence/Repositories/AdminRepository.cs`

```
Implement IAdminRepository — các query EF Core phức tạp
LedgerMismatch: GROUP BY user_id SUM(amount) WHERE type=credit vs debit → so với wallet balance
```

> ⚠️ Cập nhật DI: `services.AddScoped<IAdminRepository, AdminRepository>()`

### BƯỚC 8.3 — BE API

#### File 264: `Controllers/AdminController.cs`

```
[Route("api/v1/admin")] [Authorize(Roles = "admin")]
11 endpoints:
  - GET "users?page=1&size=20&search=&role=" → ListUsers
  - POST "users/{id}/toggle-lock" → ToggleUserLock
  - POST "users/{id}/add-balance" → AddBalance
  - GET "deposits?status=&from=&to=" → ListDeposits
  - POST "deposits/{id}/process" → ProcessDeposit
  - GET "reader-requests?status=" → ListReaderRequests
  - POST "reader-requests/{id}/approve" → ApproveReader
  - POST "reader-requests/{id}/reject" → (dùng ApproveReaderCommand với flag reject)
  - GET "withdrawals?status=" → ListWithdrawals
  - POST "withdrawals/{id}/process" → ProcessWithdrawal
  - GET "ledger-check" → GetLedgerMismatch
```

### BƯỚC 8.4 — FE

#### File 265: `actions/adminActions.ts`

```
11 hàm gọi API admin (1 hàm cho mỗi endpoint)
```

#### File 266: `app/[locale]/admin/layout.tsx`

```
Layout admin: Sidebar (link đến 8 trang) + kiểm tra role=admin → redirect nếu không phải
```

#### File 267-274: 8 trang admin

```
page.tsx (Dashboard):
  - Card: tổng user, tổng deposits hôm nay, tổng reader, đơn rút pending
  - Chart: deposits theo ngày (7 ngày gần nhất)

users/page.tsx:
  - Bảng user có search + filter role/status
  - Action: Lock/Unlock, Add balance (modal)

deposits/page.tsx:
  - Bảng đơn nạp theo status
  - Action: Process deposit thủ công

reader-requests/page.tsx:
  - Bảng đơn xin Reader
  - Action: Approve + Reject (với ghi chú)

readings/page.tsx:
  - Bảng lịch sử AI readings (chỉ xem, tracking chi phí AI)

withdrawals/page.tsx:
  - Bảng đơn rút tiền
  - Action: Approve → Paid, hoặc Reject (hoàn diamond)

disputes/page.tsx:
  - Bảng tranh chấp escrow
  - Action: Release cho Reader hoặc Refund cho User

promotions/page.tsx:
  - Bảng khuyến mãi + Form tạo/sửa/toggle active
```

**✅ KIỂM TRA GIAI ĐOẠN 8 (KIỂM TRA CUỐI CÙNG TOÀN HỆ THỐNG):**
```
1. Seed tài khoản admin: INSERT vào DB hoặc dùng endpoint seed
2. Đăng nhập admin → /vi/admin → Thấy dashboard
3. Mở /admin/users → Tìm user → Add 100 gold → Kiểm tra ví user
4. Mở /admin/reader-requests → Approve 1 đơn → User đó thành Reader
5. Mở /admin/deposits → Xử lý 1 đơn nạp → Diamond user tăng
6. Mở /admin/withdrawals → Approve + Pay 1 đơn rút
7. Mở /admin/disputes → Resolve 1 dispute
8. Kiểm tra LedgerMismatch: GET /admin/ledger-check → phải trả mảng rỗng (= khớp)
9. Chạy FULL FLOW: Register → Verify → Login → Rút bài → AI → Nạp tiền → Chat Reader → Escrow → Rút tiền → Admin duyệt
```

---

## 📌 QUY TẮC VÀNG KHI CODE

| # | Quy tắc | Tại sao |
|---|---------|---------|
| 1 | Code Enum **trước** Entity | Entity dùng Enum gán giá trị mặc định |
| 2 | Code Entity **trước** DbContext | DbContext cần khai báo `DbSet<Entity>` |
| 3 | Code Interface **trước** Handler | Handler inject interface qua constructor |
| 4 | Code Handler **trước** Controller | Controller gọi MediatR send Command/Query |
| 5 | Code Repository **trước** DI | DI file đăng ký `services.AddScoped<IRepo, Repo>()` |
| 6 | Code `lib/api.ts` **trước** actions | Tất cả actions import api.ts |
| 7 | Code actions **trước** pages | Pages gọi hàm từ actions |
| 8 | Code store **trước** components | Components đọc state từ store |
| 9 | **Luôn chạy test** sau mỗi giai đoạn | Phát hiện bug sớm, dễ sửa |
| 10 | **Cập nhật DI** khi thêm repo/service | Quên đăng ký DI → runtime error |

---

## 📊 TỔNG KẾT SỐ LIỆU

| Giai đoạn | Tính năng | Số file BE | Số file FE | Tổng |
|-----------|-----------|------------|------------|------|
| 0 | Hạ tầng | 27 | 13 | 40 |
| 1 | Auth | 30 | 10 | 40 |
| 2 | Profile | 7 | 2 | 9 |
| 3 | Wallet | 10 | 5 | 15 |
| 4 | Reading | 20 | 6 | 26 |
| 5 | AI Stream + History | 17 | 6 | 23 |
| 6 | Deposit + Legal + Promo | 18 | 8 | 26 |
| 7 | Reader + Chat + Escrow + MFA | 50 | 18 | 68 |
| 8 | Admin | 12 | 10 | 22 |
| **TỔNG** | | **~191** | **~78** | **~269 file** |

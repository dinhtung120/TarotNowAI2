/*
 * ===================================================================
 * FILE: DependencyInjection.cs
 * NAMESPACE: TarotNow.Infrastructure
 * ===================================================================
 * MỤC ĐÍCH:
 *   Ổ Khóa Tâm Điểm Nối Sóng Thằng Infrastructure Ngầm Bọc Vào .NET Core Application Builder. Ở Đây Gọi Toàn Bộ Các Nguồn Nước Dòng Chảy DI (Dependency Injection) Chứa Hết SQL Mongo Cứu Lẽ Cấu Hình Bệnh Redis Bú Jwt Mắc Quái Ác Đi Trong 1 File Dọn Sạch Sự Nóng Program.cs Tới Nỗi Tởm Giết Web Nhanh.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Repositories;
using TarotNow.Infrastructure.Security;
using TarotNow.Infrastructure.Services;
using TarotNow.Infrastructure.Services.Ai;
using TarotNow.Infrastructure.Repositories;
using TarotNow.Infrastructure.BackgroundJobs;
using System;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure;

/// <summary>
/// Đại Công Xưởng Cài Đặt (IoC Container Setup).
/// Tích Gọi Tách Lớp Toàn Bộ Các `AddScoped`, `AddSingleton`, `AddDbContext` Nhắm Nhát Kẻ Trục Thức Rõ Đáo Bằng Cho Phép Thằng API Đứng Nhìn Ở Rìa Rất Gọn.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Method Bọc Rộng Nhét Đầu Phun Tất Cả Inject Cửa Đăng Kí Này Qua File Setup Extension Ném Sang Tầng Thằng `TarotNow.Api/Program.cs` Gắn Phịch Gọi `builder.Services.AddInfrastructureServices()`.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ======================================================================
        // POSTGRESQL (EF Core) — Write model, wallet, auth, deposits (Bảng Chính Tính Số Nghiêm Khắc Có Khoá Transaction Tránh Mất Máu Áp Oái Ra).
        // ======================================================================
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

        // ======================================================================
        // MONGODB — Document store cho collections Phase 1 Cuộn Phụ Rác Data To (Bực Nhóm Session Bói Log Lịch Sử Lớn Gắn Nhấp Tránh Nhờn DB Sóng Lớn).
        // Lifecycle: MongoClient Gọi Một Vợ Bọc Cố Thread-safe (Singleton).
        // ======================================================================
        var mongoConnectionString = configuration.GetConnectionString("MongoDB")
            ?? "mongodb://localhost:27017/tarotweb"; // Thòng Trượt Lỗi Áp Gọi Phá Test Local Cho Gọn Cục Database.
        
        // Đăng Ký Chảo MongoClient Tự Dâng Connection Pool Không Cần Mở Đi Tụ Nhá Mở.
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
        
        // Trát MongoDatabase Dính Máu Parse Cho Thường "tarotweb" Hái Át Cho Thật Rạch Tách Dày Mongo DB Đọc.
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var mongoUrl = new MongoUrl(mongoConnectionString);
            var databaseName = mongoUrl.DatabaseName ?? "tarotweb";
            return client.GetDatabase(databaseName);
        });
        
        // Giăng Khung Context Mongo Rạch Gọi Code Ném DB Ảo.
        services.AddSingleton<MongoDbContext>();

        // ======================================================================
        // REPOSITORIES — Lính SQL Vác DB PostgreSQL Độc Quyền Kiếm Transaction Ráp.
        // Dùng Scoped Tức Trả Yêu Cầu Theo Từng Vòng HTTP Gọi Sinh Ra (Kéo Tủ Đeo Ác Mọc Vứt DB Nuốt Trọn Không Sài Chung Dễ Dính Leak SQL Ngon Sòng Tắt).
        // ======================================================================
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IEmailOtpRepository, EmailOtpRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IAiRequestRepository, AiRequestRepository>();
        services.AddScoped<ILedgerRepository, LedgerRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IUserConsentRepository, UserConsentRepository>();
        services.AddScoped<IDepositOrderRepository, DepositOrderRepository>();
        services.AddScoped<IDepositPromotionRepository, DepositPromotionRepository>();
        services.AddScoped<ITransactionCoordinator, TransactionCoordinator>(); // Thằng Ngáo Trùm Gây Gọi Gom 2 Repo Trả Lên Cho DB Cùng Quay Hóa Commit Cuộn.

        // ======================================================================
        // REPOSITORIES — Cò MongoDB Nhặt 5 Cục Thạch DB JSON Mọc Chấm Rề Không Dính Kéo.
        // ======================================================================
        services.AddScoped<IReadingSessionRepository, MongoReadingSessionRepository>();
        services.AddScoped<IUserCollectionRepository, MongoUserCollectionRepository>();
        services.AddScoped<ICardsCatalogRepository, MongoCardsCatalogRepository>();
        services.AddScoped<IAiProviderLogRepository, MongoAiProviderLogRepository>();
        services.AddScoped<INotificationRepository, MongoNotificationRepository>();
        services.AddScoped<IReaderRequestRepository, MongoReaderRequestRepository>();
        services.AddScoped<IReaderProfileRepository, MongoReaderProfileRepository>();

        // Phase 2.2 — Hàng Mẫu Text Message Text Cho Cái Lò Chat.
        services.AddScoped<IConversationRepository, MongoConversationRepository>();
        services.AddScoped<IChatMessageRepository, MongoChatMessageRepository>();
        services.AddScoped<IReportRepository, MongoReportRepository>();

        // Phase 2.3 — Cửa Lính Escrow Cột Dao Dịch Transaction Náu Trói Vào Database Bọc Kèo DB Độc Cùng SQL Timer Refund Chạy Quát Gọi IHostedService (Background Run Thép Nhạc 60s Tịch Hát Có Background Lũy SQL Cả App). 
        services.AddScoped<IChatFinanceRepository, ChatFinanceRepository>();
        services.AddHostedService<EscrowTimerService>();

        // Phase 2.4 — Ổ Trút Withdrawal Rét Cắt Rút Ngân Hàng Kêu
        services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();

        // Phase 2.5 — Thập Tự MFA Mẫu Xác Nhận Email Ngắn Hạn Authenticator Totp 6 Số Biến Hoá 30s Cổng Xoạy API Auth.
        services.AddScoped<IMfaService, TotpMfaService>();

        // ======================================================================
        // EXTERNAL PLUGINS - Trải Đi Các Đống Đóng Sạn Dùng Singleton Ném Sống Bền Vững Độc Suốt Vòng Trì.
        // ======================================================================
        services.AddSingleton<IPasswordHasher, Argon2idPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        services.AddScoped<IEmailSender, MockEmailSender>(); // Móc Đóng Giả Gửi Mail Nhanh Chống Tắt Bật Tịt Giữa Trưởng Tịt.
        services.AddSingleton<IRngService, RngService>(); // Trò Máy Trộn Thóc Shuffle Bóc Bài Vững Gacha Bọc Khung Ngẫu Nhiên Hút Tiêu. 
        services.AddSingleton<IPaymentGatewayService, HmacPaymentGatewayService>();

        // Thêm HTTP Cắm Vào Chảo Dịch Gọi Stream Cho Vọng Bói Gọi Tóc OpenAI Khốn (Dựng HttpClient Bằng IHttpClientFactory Tiện Góp Thêm Đụng Trắng Socket Kiệt Vòi).
        services.AddHttpClient<IAiProvider, OpenAiProvider>();

        // ======================================================================
        // REDIS CACHE — Nút Giữ Lằn Nghẹt Rate Limiting Và Giữ Túi Cạn Nhá Đêm (Quota Của Máu Rút Bài Lỗ To). Fallback Khẽ Ngang Ở Không Dùng Memory (Giảm Chế Cache Rỏ Để Không Treo Web Lỡ Trượt Nạp Rút).
        // ======================================================================
        var redisConnectionString = configuration.GetConnectionString("Redis") 
            ?? "localhost:6379"; 

        var redisMultiplexer = TryCreateRedisMultiplexer(redisConnectionString);
        var usesRedisCache = redisMultiplexer != null;
        if (redisMultiplexer != null)
        {
            // Ép Dính Ngắt Khóa StackExchangeRedis Dùng Cùng Bữa Tầm "IDistributedCache" Microsoft Bọc (Chỗ Rate, OTP Sinh Nhét).
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "TarotNow:"; 
            });

            services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer); // Đẩy Gọi Cục Múi Khóa Dính Lock Cho Nháy Cache Gốc Giữ Bóp Rate Quả Gọi Redis Lõn.
        }
        else
        {
            // Trượt Rớt Bọc Trống RAM Máy Tính Kếu Cháy Cứu Thay Cho Code App Chạy Local Dev Tịt Thở Docker Ko Bật Lẫn Cọc Rớt.
            services.AddDistributedMemoryCache();
        }

        services.AddSingleton(new CacheBackendState(usesRedisCache)); 
        services.AddHostedService<CacheBackendStartupLogger>(); 

        // Rúc Xé Cột Dịch Vụ Ẩn Thức Cắn Cache Tiện Hóa Cao Lọc Hốc Dựa Tới.
        services.AddScoped<ICacheService, RedisCacheService>();

        // ======================================================================
        // ADD AUTHENTICATION (Bật Kẹp Token Xác Thực Cảo Nút Giao Thông Cửa Đầu Vào Cho Khách). 
        // JWT Bọc Sạn Gỡ Móc Token Phá Xoát Từ Client (Phải Cầm Chìa Khóa Nát API Báo Xoát Gửi).
        // ======================================================================
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"] 
            ?? throw new InvalidOperationException("JWT SecretKey chưa được cấu hình. Vui lòng thiết lập 'Jwt:SecretKey' trong appsettings.json hoặc biến môi trường.");
        
        ValidateJwtSecret(secretKey); // Gõ Đầu Kẻ Setup Xài Pass Dỏm Cắt Phá Bóp Chết Khỏi Mở Web Rút Gọi Web Đồ Ác Ý Chặn Quát Cấm Local Chạy Bịp Độc Không Deploy Được.
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                // Oái Kèm Khoen Mật Cục Chìa Khóa Phay (Chuyển Gọn Mảng Xoáy Chuỗi Xâu Nét Mã Gõ Tí Kẹp Vòng Đo Check HMAC Bảo Mật Mã Sạch Phát Mã).
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(secretKey))
            };
            
            // Xủi Cái Kẹo Token Được Gắn Đi Ở Quả Nắp Lỗ Header Nhập Nhằng Do Streaming Tạt Cát Dùng SSE Query URL ?access_token=... Từ Client Nuốt Dùng Web Gọi EventSource Chặn Auth Header (Hỗ Trợ Luồng Phase 1.4 API Phóng Và Websocket Chat Rích Socket Nhựa Rút Token Khang Query Vô Gắn Ngang Bụng Chứ Mạn Không Bọc Header Http Được).
            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    
                    var isChatHub = path.StartsWithSegments("/api/v1/chat"); // Pháo Tụt WebSocket Chạy Nối Token Náo
                    var isAiStreamEndpoint =
                        path.StartsWithSegments("/api/v1/sessions", out var remaining) &&
                        remaining.HasValue &&
                        remaining.Value.EndsWith("/stream", StringComparison.OrdinalIgnoreCase);

                    // Trích Thần Gắn Đi Ném Vô Đầu Thách Vòng Lỗi Phê Lọt Context Token Gấp Của Tụt 
                    if (!string.IsNullOrEmpty(accessToken) && (isChatHub || isAiStreamEndpoint))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
        
        return services;
    }

    /// <summary>Rắc Máy Bẻ Gãy Guard Validate Chống Thùng Nhầm Nhão Pass Key Thường "Rác Nhét Vào" Đánh Sập Lò Kím Hack Đọc Giấy Kệ App Web Đòi Chống Phân Trách Gốc Rễ Từ Rạch (Móc 32 Ký Tự Mép Vọt Tụt Mất Máu).</summary>
    private static void ValidateJwtSecret(string secretKey)
    {
        var normalized = secretKey.Trim();
        if (normalized.Length < 32)
            throw new InvalidOperationException("Jwt:SecretKey quá ngắn. Yêu cầu tối thiểu 32 ký tự.");

        if (normalized.Contains("REPLACE", StringComparison.OrdinalIgnoreCase) ||
            normalized.Contains("PHUONG_AN_B", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Jwt:SecretKey đang dùng placeholder. Vui lòng cấu hình secret thật.");
        }
    }

    /// <summary>Bẫy Lọc Rớt Lụi Trượt Kết Nối Lên Redis Chết Thác Không Ném Exception App Crash Chết Chặn Khối To Toàn Web Đi Phủi Cắn Exception Toàn App Trượt Ra Nát Lại Bằng Dùng FallBack Nhanh Sẵn (Graceful Fallback Lũy Phá Đáy Rắc Dùng Bọc).</summary>
    private static IConnectionMultiplexer? TryCreateRedisMultiplexer(string connectionString)
    {
        try
        {
            var options = ConfigurationOptions.Parse(connectionString);
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 2000;
            options.SyncTimeout = 2000;
            options.ConnectRetry = 1;

            var multiplexer = ConnectionMultiplexer.Connect(options);
            if (multiplexer.IsConnected) return multiplexer; // Conect Ngon Véo Trả 1.

            multiplexer.Dispose();
            return null;
        }
        catch
        {
            return null; // Vớt Cháy Cạn Nhờn Ổ Cắm Server Ép Ra Móc Null Quét Còi Cho Thằng Memory Nuốt.
        }
    }
}

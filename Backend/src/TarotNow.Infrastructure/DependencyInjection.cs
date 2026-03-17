using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Repositories;
using TarotNow.Infrastructure.Security;
using TarotNow.Infrastructure.Services;
using TarotNow.Infrastructure.Services.Ai;
using TarotNow.Infrastructure.Repositories;
using TarotNow.Infrastructure.BackgroundJobs;

namespace TarotNow.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký tất cả Infrastructure Services (DB, Redis, External APIs) vào DI container.
    /// Tách biệt phần cấu hình hạ tầng khỏi Application logic.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ======================================================================
        // POSTGRESQL (EF Core) — Write model, wallet, auth, deposits
        // ======================================================================
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

        // ======================================================================
        // MONGODB — Document store cho collections Phase 1
        // Lifecycle: MongoClient thread-safe → Singleton.
        // MongoDbContext tạo indexes tại khởi tạo → Singleton.
        // ======================================================================
        var mongoConnectionString = configuration.GetConnectionString("MongoDB")
            ?? "mongodb://localhost:27017/tarotweb"; // Fallback cho local dev
        
        // Đăng ký MongoClient — connection pool tự quản lý bởi driver
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
        
        // Đăng ký IMongoDatabase — parse database name từ connection string
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var mongoUrl = new MongoUrl(mongoConnectionString);
            // Dùng database name từ connection string, fallback "tarotweb"
            var databaseName = mongoUrl.DatabaseName ?? "tarotweb";
            return client.GetDatabase(databaseName);
        });
        
        // Đăng ký MongoDbContext — tạo indexes + expose collection properties
        services.AddSingleton<MongoDbContext>();

        // ======================================================================
        // REPOSITORIES — PostgreSQL-only repos
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

        // ======================================================================
        // REPOSITORIES — MongoDB repos (thay thế EF Core cho 5 collections)
        // ======================================================================
        services.AddScoped<IReadingSessionRepository, MongoReadingSessionRepository>();
        services.AddScoped<IUserCollectionRepository, MongoUserCollectionRepository>();
        services.AddScoped<ICardsCatalogRepository, MongoCardsCatalogRepository>();
        services.AddScoped<IAiProviderLogRepository, MongoAiProviderLogRepository>();
        services.AddScoped<INotificationRepository, MongoNotificationRepository>();
        services.AddScoped<IReaderRequestRepository, MongoReaderRequestRepository>();
        services.AddScoped<IReaderProfileRepository, MongoReaderProfileRepository>();

        // Phase 2.2 — Chat repositories
        services.AddScoped<IConversationRepository, MongoConversationRepository>();
        services.AddScoped<IChatMessageRepository, MongoChatMessageRepository>();
        services.AddScoped<IReportRepository, MongoReportRepository>();

        // Phase 2.3 — Escrow repository + background timer
        services.AddScoped<IChatFinanceRepository, ChatFinanceRepository>();
        services.AddHostedService<EscrowTimerService>();

        // Phase 2.4 — Withdrawal repository
        services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();

        // Phase 2.5 — MFA service
        services.AddScoped<IMfaService, TotpMfaService>();

        // Đăng ký Auth Services
        services.AddSingleton<IPasswordHasher, Argon2idPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        // Đăng ký Services Khác
        services.AddScoped<IEmailSender, MockEmailSender>();
        services.AddSingleton<IRngService, RngService>();
        services.AddSingleton<IPaymentGatewayService, MockPaymentGatewayService>();

        // Phase 1.4: Tích hợp Streaming Provider (OpenAI)
        services.AddHttpClient<IAiProvider, OpenAiProvider>();

        // ======================================================================
        // REDIS CACHE — Rate limiting, Daily Quota, In-flight Cap
        // ======================================================================
        var redisConnectionString = configuration.GetConnectionString("Redis") 
            ?? "localhost:6379"; // Fallback cho local dev

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "TarotNow:"; // Prefix cho các key trong Redis
        });

        // Đăng ký CacheService để Application layer sử dụng
        services.AddScoped<ICacheService, RedisCacheService>();

        // Đăng ký Authentication/JWT Services
        var jwtSettings = configuration.GetSection("Jwt");
        // Bắt buộc cấu hình SecretKey trong appsettings.json / biến môi trường.
        // Không dùng fallback hardcoded vì rủi ro bảo mật cao — nếu config thiếu, app phải dừng ngay.
        var secretKey = jwtSettings["SecretKey"] 
            ?? throw new InvalidOperationException("JWT SecretKey chưa được cấu hình. Vui lòng thiết lập 'Jwt:SecretKey' trong appsettings.json hoặc biến môi trường.");
        
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
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(secretKey))
            };
            
            // Allow token to be sent in query string for SSE (AiInterpretationStream in Phase 1.4)
            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && 
                        (path.StartsWithSegments("/api/v1/chat") || path.StartsWithSegments("/api/v1/sessions")))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
        
        return services;
    }
}

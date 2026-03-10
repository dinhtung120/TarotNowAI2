using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Interfaces;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Repositories;
using TarotNow.Infrastructure.Security;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký tất cả Infrastructure Services (DB, Redis, External APIs) vào DI container.
    /// Tách biệt phần cấu hình hạ tầng khỏi Application logic.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Đăng ký Entity Framework Core (PostgreSQL)
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))
                   .UseSnakeCaseNamingConvention());

        // Đăng ký Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IEmailOtpRepository, EmailOtpRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ILedgerRepository, LedgerRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IReadingSessionRepository, ReadingSessionRepository>();
        services.AddScoped<IUserCollectionRepository, UserCollectionRepository>();

        // Đăng ký Auth Services
        services.AddSingleton<IPasswordHasher, Argon2idPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        // Đăng ký Services Khác
        services.AddScoped<IEmailSender, MockEmailSender>();
        services.AddSingleton<IRngService, RngService>();

        // TODO: Đăng ký MongoDB MongoClient
        // var mongoConnectionString = configuration.GetConnectionString("MongoDB");

        // TODO: Đăng ký Redis Cache
        // services.AddStackExchangeRedisCache(options => ...);

        // Đăng ký Authentication/JWT Services
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"] ?? "TarotNow_SuperSecretKey_For_Development_Only!";
        
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
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/api/v1/chat"))
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

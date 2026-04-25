using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Constants;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Đăng ký xác thực JWT và cấu hình cơ chế lấy token cho HTTP/SignalR.
    /// Luồng xử lý: đọc cấu hình JWT đã validate, đăng ký Authentication và gắn JwtBearer options.
    /// </summary>
    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var (jwtOptions, secretKey) = GetValidatedJwtConfiguration(configuration);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                ConfigureJwtBearerOptions(options, jwtOptions, secretKey);
            });
    }

    /// <summary>
    /// Lấy và kiểm tra cấu hình JWT bắt buộc trước khi khởi tạo auth pipeline.
    /// Luồng xử lý: bind JwtOptions, kiểm tra SecretKey tồn tại, validate độ mạnh và trả tuple cấu hình.
    /// </summary>
    private static (JwtOptions Options, string SecretKey) GetValidatedJwtConfiguration(IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
        var secretKey = jwtOptions.SecretKey
            ?? throw new InvalidOperationException("JWT SecretKey chưa được cấu hình. Vui lòng thiết lập 'Jwt:SecretKey'.");

        ValidateJwtSecret(secretKey);
        return (jwtOptions, secretKey);
    }

    /// <summary>
    /// Cấu hình JwtBearerOptions gồm TokenValidationParameters và event resolve token.
    /// Luồng xử lý: set rule validate issuer/audience/lifetime/signature, gắn OnMessageReceived để lấy token.
    /// </summary>
    private static void ConfigureJwtBearerOptions(JwtBearerOptions options, JwtOptions jwtOptions, string secretKey)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ResolveBearerTokenAsync,
            OnTokenValidated = ResolveTokenValidationAsync
        };
    }

    /// <summary>
    /// Resolve bearer token từ cookie/query cho các endpoint realtime.
    /// Luồng xử lý: ưu tiên cookie; query token chỉ chấp nhận cho SignalR hub.
    /// </summary>
    private static Task ResolveBearerTokenAsync(MessageReceivedContext context)
    {
        var queryToken = context.Request.Query["access_token"].ToString();
        var cookieToken = context.Request.Cookies["accessToken"];
        var path = context.HttpContext.Request.Path;

        if (HasToken(cookieToken))
        {
            context.Token = cookieToken;
            // Ưu tiên access token qua cookie cho toàn bộ web endpoints để đồng bộ auth flow HttpOnly cookie.
        }
        else
        {
            var isHubEndpoint = IsHubEndpoint(path);
            if (HasToken(queryToken) && isHubEndpoint)
            {
                context.Token = queryToken;
                // Fallback query token chỉ cho SignalR khi client không gửi cookie/header thuận tiện.
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Kiểm tra path có phải endpoint hub realtime hay không.
    /// Luồng xử lý: đối chiếu path với các segment chat/presence hub.
    /// </summary>
    private static bool IsHubEndpoint(PathString path)
    {
        return path.StartsWithSegments(ApiPathConstants.ChatHub)
            || path.StartsWithSegments(ApiPathConstants.PresenceHub);
    }

    /// <summary>
    /// Kiểm tra token có giá trị hợp lệ để sử dụng hay không.
    /// Luồng xử lý: trả true khi token không null/empty/whitespace.
    /// </summary>
    private static bool HasToken(string? token)
    {
        return !string.IsNullOrWhiteSpace(token);
    }

    private static async Task ResolveTokenValidationAsync(TokenValidatedContext context)
    {
        var principal = context.Principal;
        if (principal is null)
        {
            context.Fail("Missing token principal.");
            return;
        }

        var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
        var authSessionRepository = context.HttpContext.RequestServices.GetService<IAuthSessionRepository>();

        var cancellationToken = context.HttpContext.RequestAborted;
        var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (cacheService is not null && HasToken(jti))
        {
            var isBlacklisted = await cacheService.GetAsync<string>(
                AuthCacheKeys.BuildAccessBlacklistKey(jti!.Trim()),
                cancellationToken);
            if (isBlacklisted is not null)
            {
                context.Fail("Access token has been revoked.");
                return;
            }
        }

        var sessionIdRaw = principal.FindFirst(AuthClaimConstants.SessionId)?.Value;
        if (!Guid.TryParse(sessionIdRaw, out var sessionId) || sessionId == Guid.Empty)
        {
            return;
        }

        if (cacheService is not null)
        {
            var isRevokedSession = await cacheService.GetAsync<string>(
                AuthCacheKeys.BuildSessionRevokedKey(sessionId),
                cancellationToken);
            if (isRevokedSession is not null)
            {
                context.Fail("Session has been revoked.");
                return;
            }
        }

        if (authSessionRepository is null)
        {
            return;
        }

        var activeSession = await authSessionRepository.GetActiveAsync(sessionId, cancellationToken);
        if (activeSession is null)
        {
            context.Fail("Session has been revoked.");
        }
    }

    /// <summary>
    /// Validate JWT secret để chặn cấu hình yếu hoặc placeholder.
    /// Luồng xử lý: trim secret, kiểm tra độ dài tối thiểu và phát hiện chuỗi placeholder nguy hiểm.
    /// </summary>
    private static void ValidateJwtSecret(string secretKey)
    {
        var normalized = secretKey.Trim();
        if (normalized.Length < 32)
        {
            // Business rule bảo mật: secret dưới 32 ký tự không đủ mạnh cho chữ ký JWT.
            throw new InvalidOperationException("Jwt:SecretKey quá ngắn. Yêu cầu tối thiểu 32 ký tự.");
        }

        if (normalized.Contains("REPLACE", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("PHUONG_AN_B", StringComparison.OrdinalIgnoreCase))
        {
            // Chặn cấu hình placeholder để tránh deploy với secret giả.
            throw new InvalidOperationException("Jwt:SecretKey đang dùng placeholder. Vui lòng cấu hình secret thật.");
        }
    }
}

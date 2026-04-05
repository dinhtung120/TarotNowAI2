using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TarotNow.Infrastructure.Constants;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
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

    private static (JwtOptions Options, string SecretKey) GetValidatedJwtConfiguration(IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
        var secretKey = jwtOptions.SecretKey
            ?? throw new InvalidOperationException("JWT SecretKey chưa được cấu hình. Vui lòng thiết lập 'Jwt:SecretKey'.");

        ValidateJwtSecret(secretKey);
        return (jwtOptions, secretKey);
    }

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
            OnMessageReceived = ResolveBearerTokenAsync
        };
    }

    private static Task ResolveBearerTokenAsync(MessageReceivedContext context)
    {
        var queryToken = context.Request.Query["access_token"].ToString();
        var cookieToken = context.Request.Cookies["accessToken"];
        var path = context.HttpContext.Request.Path;

        var isChatHub = path.StartsWithSegments(ApiPathConstants.ChatHub);
        var isPresenceHub = path.StartsWithSegments(ApiPathConstants.PresenceHub);
        /* FIX #23: Thêm CallHub vào danh sách endpoint được phép đọc token.
         * Trước đây thiếu dòng này → CallHub WebSocket gửi access_token qua query string
         * nhưng server bỏ qua → trả về 401 Unauthorized → "connection not found". */
        var isCallHub = path.StartsWithSegments(ApiPathConstants.CallHub);
        var isAiStream = path.StartsWithSegments(ApiPathConstants.Sessions, out var remaining)
                         && remaining.HasValue
                         && remaining.Value.EndsWith("/stream", StringComparison.OrdinalIgnoreCase);

        if ((isChatHub || isPresenceHub || isCallHub) && !string.IsNullOrWhiteSpace(cookieToken))
        {
            context.Token = cookieToken;
        }
        else if (!string.IsNullOrWhiteSpace(queryToken) && (isAiStream || isChatHub || isPresenceHub || isCallHub))
        {
            context.Token = queryToken;
        }

        return Task.CompletedTask;
    }

    private static void ValidateJwtSecret(string secretKey)
    {
        var normalized = secretKey.Trim();
        if (normalized.Length < 32)
        {
            throw new InvalidOperationException("Jwt:SecretKey quá ngắn. Yêu cầu tối thiểu 32 ký tự.");
        }

        if (normalized.Contains("REPLACE", StringComparison.OrdinalIgnoreCase)
            || normalized.Contains("PHUONG_AN_B", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Jwt:SecretKey đang dùng placeholder. Vui lòng cấu hình secret thật.");
        }
    }
}

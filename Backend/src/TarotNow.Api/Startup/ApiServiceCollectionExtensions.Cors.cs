using System.Net;

namespace TarotNow.Api.Startup;

// Cấu hình CORS theo danh sách origin cho phép lấy từ cấu hình môi trường.
public static class CorsServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký CORS policy mặc định dựa trên cấu hình và môi trường chạy.
    /// Luồng xử lý: đọc danh sách origin, validate cấu hình bắt buộc, sau đó rẽ nhánh rule dev/prod.
    /// </summary>
    public static IServiceCollection AddConfiguredCors(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var rawOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Select(origin => origin.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? Array.Empty<string>();

        if (rawOrigins.Length == 0)
        {
            // Rule bắt buộc: không có origin hợp lệ thì fail-fast để tránh mở nhầm CORS wildcard.
            throw new InvalidOperationException("Missing CORS configuration: Cors:AllowedOrigins must contain at least one origin.");
        }

        // Tự động mở rộng danh sách origin để hỗ trợ cả domain có và không có 'www'.
        // Điều này rất quan trọng để tránh lỗi CORS khi người dùng truy cập qua các biến thể subdomain khác nhau.
        var expandedOrigins = new HashSet<string>(rawOrigins, StringComparer.OrdinalIgnoreCase);
        foreach (var origin in rawOrigins)
        {
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
            {
                continue;
            }

            var scheme = uri.Scheme;
            var host = uri.Host;
            var port = uri.IsDefaultPort ? "" : $":{uri.Port}";

            if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                // Nếu cấu hình có www.example.com -> tự động cho phép cả example.com
                expandedOrigins.Add($"{scheme}://{host[4..]}{port}");
            }
            else if (!IPAddress.TryParse(host, out _) && host.Contains('.'))
            {
                // Nếu cấu hình có example.com (và không phải IP) -> tự động cho phép cả www.example.com
                expandedOrigins.Add($"{scheme}://www.{host}{port}");
            }
        }

        var allowedOrigins = expandedOrigins.ToArray();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    // Toàn bộ origin cho phép phải đi qua cấu hình, không hard-code theo môi trường chạy.
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}

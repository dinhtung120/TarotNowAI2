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
        IHostEnvironment _)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Select(origin => origin.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? Array.Empty<string>();

        if (allowedOrigins.Length == 0)
        {
            // Rule bắt buộc: không có origin hợp lệ thì fail-fast để tránh mở nhầm CORS wildcard.
            throw new InvalidOperationException("Missing CORS configuration: Cors:AllowedOrigins must contain at least one origin.");
        }

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

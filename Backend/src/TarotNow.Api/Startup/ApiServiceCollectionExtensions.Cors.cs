namespace TarotNow.Api.Startup;

// Cấu hình CORS theo danh sách origin cho phép lấy từ cấu hình môi trường.
public static class CorsServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký CORS policy mặc định dựa trên cấu hình origin.
    /// Luồng xử lý: đọc danh sách origin, validate định dạng và áp dụng policy credentials.
    /// </summary>
    public static IServiceCollection AddConfiguredCors(
        this IServiceCollection services,
        IConfiguration configuration)
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

        var invalidOrigins = rawOrigins
            .Where(origin => Uri.TryCreate(origin, UriKind.Absolute, out _) == false)
            .ToArray();
        if (invalidOrigins.Length > 0)
        {
            throw new InvalidOperationException(
                $"Invalid CORS origin values: {string.Join(", ", invalidOrigins)}");
        }

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(rawOrigins)
                    // Toàn bộ origin cho phép phải đi qua cấu hình, không hard-code theo môi trường chạy.
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}

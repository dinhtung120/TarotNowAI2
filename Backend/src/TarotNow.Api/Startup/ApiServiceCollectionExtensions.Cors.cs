using System.Net;
using System.Net.Sockets;

namespace TarotNow.Api.Startup;

// Cấu hình CORS theo danh sách origin cho phép, đồng thời nới rule hợp lý khi chạy development.
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

        var allowedOriginsSet = new HashSet<string>(allowedOrigins, StringComparer.OrdinalIgnoreCase);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (environment.IsDevelopment())
                {
                    // Nhánh dev: cho phép localhost/private network để hỗ trợ mobile emulator và máy test nội bộ.
                    policy.SetIsOriginAllowed(origin => IsDevelopmentOriginAllowed(origin, allowedOriginsSet));
                }
                else
                {
                    // Nhánh non-dev: chỉ cho phép đúng danh sách cấu hình để đảm bảo bảo mật.
                    policy.WithOrigins(allowedOrigins);
                }

                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    /// <summary>
    /// Kiểm tra một origin có được phép trong môi trường development hay không.
    /// Luồng xử lý: ưu tiên danh sách cấu hình, sau đó parse URI và cho phép localhost/private IPv4.
    /// </summary>
    private static bool IsDevelopmentOriginAllowed(string origin, ISet<string> configuredOrigins)
    {
        if (configuredOrigins.Contains(origin))
        {
            // Origin nằm trong cấu hình chính thức luôn được chấp nhận.
            return true;
        }

        if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
        {
            // Edge case chuỗi origin sai định dạng URI thì từ chối ngay.
            return false;
        }

        if (!string.Equals(uri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            // Chỉ chấp nhận HTTP/HTTPS để tránh protocol không mong muốn.
            return false;
        }

        if (string.Equals(uri.Host, "localhost", StringComparison.OrdinalIgnoreCase))
        {
            // Hỗ trợ dev local nhiều port khác nhau cho frontend tooling.
            return true;
        }

        if (!IPAddress.TryParse(uri.Host, out var address))
        {
            // Host không phải IP cũng không phải localhost thì không nằm trong rule mở rộng dev.
            return false;
        }

        return IsLoopbackOrPrivateNetwork(address);
    }

    /// <summary>
    /// Kiểm tra địa chỉ IP thuộc loopback hoặc dải private network hay không.
    /// Luồng xử lý: ưu tiên loopback, sau đó chỉ chấp nhận IPv4 private ranges thông dụng.
    /// </summary>
    private static bool IsLoopbackOrPrivateNetwork(IPAddress address)
    {
        if (IPAddress.IsLoopback(address))
        {
            // Loopback luôn hợp lệ cho kịch bản chạy local.
            return true;
        }

        if (address.AddressFamily != AddressFamily.InterNetwork)
        {
            // Chưa mở rộng rule cho IPv6 private ranges để tránh nới quá rộng ngoài nhu cầu hiện tại.
            return false;
        }

        var bytes = address.GetAddressBytes();
        // Rule mạng private RFC1918 cho môi trường dev nội bộ.
        return bytes[0] == 10
            || (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
            || (bytes[0] == 192 && bytes[1] == 168);
    }
}

using TarotNow.Api.Hubs;
using TarotNow.Api.Middlewares;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

namespace TarotNow.Api.Startup;

// Chuẩn hóa cách ghép pipeline middleware/endpoint của API để giữ thứ tự thực thi nhất quán.
public static class ApiApplicationBuilderExtensions
{
    // Tên thư mục upload public nằm dưới wwwroot.
    private const string UploadsFolderName = "uploads";
    // Tên thư mục web root mặc định của ứng dụng.
    private const string WebRootFolderName = "wwwroot";
    // Endpoint hub presence dùng đường dẫn cố định để client subscription ổn định.
    private const string PresenceHubPath = "/api/v1/presence";
    // Extension cần map MIME thủ công cho static file provider.
    private const string AvifExtension = ".avif";
    // MIME type chuẩn cho ảnh AVIF.
    private const string AvifMimeType = "image/avif";
    // Extension cần map MIME thủ công cho static file provider.
    private const string WebpExtension = ".webp";
    // MIME type chuẩn cho ảnh WEBP.
    private const string WebpMimeType = "image/webp";

    /// <summary>
    /// Ghép toàn bộ pipeline middleware và map endpoint cho API.
    /// Luồng xử lý: cấu hình lỗi/request, swagger, https, static files, security, realtime rồi map controllers.
    /// </summary>
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        // Giữ thứ tự middleware cố định để tránh regression do thay đổi pipeline ngầm.
        ConfigureErrorAndRequestMiddlewares(app);
        ConfigureSwagger(app);
        ConfigureHttps(app);
        ConfigureStaticFiles(app);
        ConfigureSecurityPipeline(app);
        MapRealtimeEndpoints(app);
        app.MapControllers();

        return app;
    }

    /// <summary>
    /// Cấu hình nhóm middleware lõi cho xử lý lỗi và nhận diện request.
    /// Luồng xử lý: bật exception handler, correlation id và feature gate chat theo đúng thứ tự.
    /// </summary>
    private static void ConfigureErrorAndRequestMiddlewares(WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ChatFeatureGateMiddleware>();
    }

    /// <summary>
    /// Cấu hình OpenAPI/Swagger cho tài liệu API.
    /// Luồng xử lý: luôn map OpenAPI endpoint, chỉ bật UI swagger khi chạy môi trường development.
    /// </summary>
    private static void ConfigureSwagger(WebApplication app)
    {
        app.MapOpenApi();

        if (!app.Environment.IsDevelopment())
        {
            // Nhánh production: không mở swagger UI để giảm bề mặt lộ metadata API công khai.
            return;
        }

        // Nhánh development: bật swagger để developer kiểm thử và khám phá endpoint nhanh.
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    /// <summary>
    /// Bật chuyển hướng HTTPS cho môi trường không phải development.
    /// Luồng xử lý: bỏ qua dev local để tránh ảnh hưởng trải nghiệm debug khi chưa có TLS.
    /// </summary>
    private static void ConfigureHttps(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Edge case local dev: giữ HTTP để giảm cấu hình hạ tầng phát triển.
            return;
        }

        // Môi trường ngoài dev bắt buộc chuyển hướng sang HTTPS để đảm bảo an toàn truyền tải.
        app.UseHttpsRedirection();
    }

    /// <summary>
    /// Cấu hình phục vụ static file cho web root mặc định và thư mục uploads.
    /// Luồng xử lý: bật static files mặc định, đảm bảo thư mục uploads tồn tại, map MIME type đặc thù.
    /// </summary>
    private static void ConfigureStaticFiles(WebApplication app)
    {
        app.UseStaticFiles();

        var uploadsPath = EnsureUploadsDirectory(app);
        var contentTypeProvider = CreateUploadsContentTypeProvider();

        // Khai báo static file source riêng cho uploads để client truy cập tài nguyên người dùng upload.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/" + UploadsFolderName,
            ContentTypeProvider = contentTypeProvider,
            ServeUnknownFileTypes = true,
        });
    }

    /// <summary>
    /// Đảm bảo thư mục uploads tồn tại trước khi static file middleware hoạt động.
    /// Luồng xử lý: dựng path tuyệt đối theo content root, tạo thư mục nếu chưa có, trả path kết quả.
    /// </summary>
    private static string EnsureUploadsDirectory(WebApplication app)
    {
        var uploadsPath = Path.Combine(app.Environment.ContentRootPath, WebRootFolderName, UploadsFolderName);
        if (Directory.Exists(uploadsPath))
        {
            // Nhánh đã tồn tại: tái sử dụng trực tiếp để tránh thao tác I/O dư thừa.
            return uploadsPath;
        }

        // Nhánh chưa tồn tại: tạo thư mục để tránh 404 khi ứng dụng vừa deploy lần đầu.
        Directory.CreateDirectory(uploadsPath);
        return uploadsPath;
    }

    /// <summary>
    /// Tạo content type provider cho thư mục uploads, bổ sung MIME map thiếu sẵn trong mặc định.
    /// Luồng xử lý: khởi tạo provider rồi gắn thêm map AVIF/WEBP.
    /// </summary>
    private static FileExtensionContentTypeProvider CreateUploadsContentTypeProvider()
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        // Bổ sung MIME map để browser/client xử lý đúng định dạng ảnh hiện đại.
        contentTypeProvider.Mappings[AvifExtension] = AvifMimeType;
        contentTypeProvider.Mappings[WebpExtension] = WebpMimeType;
        return contentTypeProvider;
    }

    /// <summary>
    /// Cấu hình chuỗi middleware bảo mật cho request pipeline.
    /// Luồng xử lý: CORS trước, sau đó rate limit, rồi authentication và authorization.
    /// </summary>
    private static void ConfigureSecurityPipeline(WebApplication app)
    {
        app.UseCors();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
    }

    /// <summary>
    /// Map các SignalR hub endpoint dùng cho chat, presence và call.
    /// Luồng xử lý: đăng ký từng hub vào route tương ứng để client kết nối realtime đúng kênh.
    /// </summary>
    private static void MapRealtimeEndpoints(WebApplication app)
    {
        app.MapHub<ChatHub>("/" + ApiRoutes.ChatHub);
        app.MapHub<PresenceHub>(PresenceHubPath);
        app.MapHub<CallHub>("/" + ApiRoutes.CallHub);
    }
}

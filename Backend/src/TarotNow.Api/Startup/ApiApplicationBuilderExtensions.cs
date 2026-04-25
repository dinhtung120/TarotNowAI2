using TarotNow.Api.Hubs;
using TarotNow.Api.Middlewares;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.HttpOverrides;

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
    // MIME type cho ảnh JPEG.
    private const string JpegMimeType = "image/jpeg";
    // MIME type cho ảnh PNG.
    private const string PngMimeType = "image/png";
    // MIME type cho ảnh GIF.
    private const string GifMimeType = "image/gif";
    // MIME type cho ảnh HEIC.
    private const string HeicMimeType = "image/heic";
    // MIME type cho ảnh HEIF.
    private const string HeifMimeType = "image/heif";
    // MIME type cho âm thanh MP3.
    private const string Mp3MimeType = "audio/mpeg";
    // MIME type cho âm thanh WAV.
    private const string WavMimeType = "audio/wav";
    // MIME type cho âm thanh OGG.
    private const string OggMimeType = "audio/ogg";
    // MIME type cho âm thanh WEBM.
    private const string WebmMimeType = "audio/webm";
    // MIME type cho âm thanh M4A.
    private const string M4aMimeType = "audio/mp4";

    /// <summary>
    /// Ghép toàn bộ pipeline middleware và map endpoint cho API.
    /// Luồng xử lý: cấu hình lỗi/request, swagger, https, static files, security, realtime rồi map controllers.
    /// </summary>
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        // Giữ thứ tự middleware cố định để tránh regression do thay đổi pipeline ngầm.
        ConfigureForwardedHeaders(app);
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
    /// Bật forwarded headers khi chạy sau reverse proxy đáng tin cậy.
    /// Luồng xử lý: kiểm tra cờ cấu hình rồi gọi middleware để chuẩn hóa scheme/ip trước các middleware khác.
    /// </summary>
    private static void ConfigureForwardedHeaders(WebApplication app)
    {
        if (!app.Configuration.GetValue<bool>("ForwardedHeaders:Enabled"))
        {
            return;
        }

        app.UseForwardedHeaders();
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
        if (!app.Environment.IsDevelopment())
        {
            // Nhánh production: không mở OpenAPI endpoint để giảm bề mặt lộ metadata API công khai.
            return;
        }

        // Nhánh development: bật swagger để developer kiểm thử và khám phá endpoint nhanh.
        app.MapOpenApi();
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

        // Kiểm tra xem có bật chuyển hướng HTTPS từ cấu hình không.
        // Mặc định là true nếu không cấu hình, nhưng có thể tắt qua env: SECURITY__HTTPSREDIRECTIONENABLED
        if (!app.Configuration.GetValue<bool>("Security:HttpsRedirectionEnabled", true))
        {
            return;
        }

        // Bỏ redirect cho health endpoint nội bộ để container healthcheck HTTP không bị false-negative.
        app.UseWhen(
            context => !context.Request.Path.StartsWithSegments("/api/v1/health", StringComparison.OrdinalIgnoreCase),
            branch => branch.UseHttpsRedirection());
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
            ServeUnknownFileTypes = false,
        });
    }

    /// <summary>
    /// Đảm bảo thư mục uploads tồn tại trước khi static file middleware hoạt động.
    /// Luồng xử lý: dựng path tuyệt đối theo content root, tạo thư mục nếu chưa có, trả path kết quả.
    /// </summary>
    private static string EnsureUploadsDirectory(WebApplication app)
    {
        var configuredStorageRoot = app.Configuration["FileStorage:RootPath"]?.Trim();
        var storageRoot = string.IsNullOrWhiteSpace(configuredStorageRoot)
            ? Path.Combine(app.Environment.ContentRootPath, WebRootFolderName)
            : configuredStorageRoot;
        var uploadsPath = Path.Combine(storageRoot, UploadsFolderName);
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
        // Chỉ cho phép serve tập extension an toàn đã được hệ thống upload hỗ trợ.
        contentTypeProvider.Mappings.Clear();
        contentTypeProvider.Mappings[".jpg"] = JpegMimeType;
        contentTypeProvider.Mappings[".jpeg"] = JpegMimeType;
        contentTypeProvider.Mappings[".png"] = PngMimeType;
        contentTypeProvider.Mappings[".gif"] = GifMimeType;
        contentTypeProvider.Mappings[AvifExtension] = AvifMimeType;
        contentTypeProvider.Mappings[WebpExtension] = WebpMimeType;
        contentTypeProvider.Mappings[".heic"] = HeicMimeType;
        contentTypeProvider.Mappings[".heif"] = HeifMimeType;
        contentTypeProvider.Mappings[".mp3"] = Mp3MimeType;
        contentTypeProvider.Mappings[".wav"] = WavMimeType;
        contentTypeProvider.Mappings[".ogg"] = OggMimeType;
        contentTypeProvider.Mappings[".webm"] = WebmMimeType;
        contentTypeProvider.Mappings[".m4a"] = M4aMimeType;
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
    /// Map các SignalR hub endpoint dùng cho chat và presence.
    /// Luồng xử lý: đăng ký từng hub vào route tương ứng để client kết nối realtime đúng kênh.
    /// </summary>
    private static void MapRealtimeEndpoints(WebApplication app)
    {
        app.MapHub<ChatHub>("/" + ApiRoutes.ChatHub);
        app.MapHub<PresenceHub>(PresenceHubPath);
    }
}

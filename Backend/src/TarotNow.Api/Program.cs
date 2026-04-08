using TarotNow.Api.Startup;
using Serilog;

// Nạp biến môi trường sớm để mọi cấu hình phía sau đọc được đầy đủ giá trị runtime.
EnvLoader.Load();

// Khởi tạo host builder trung tâm cho toàn bộ ứng dụng API.
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    // Dùng cấu hình + DI enrichers để log đồng bộ giữa local/dev/prod.
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// Đăng ký service presentation + CORS trước khi build app.
builder.Services
    .AddApiPresentationServices(builder.Configuration)
    .AddConfiguredCors(builder.Configuration, builder.Environment);

var app = builder.Build();

// Ghép middleware pipeline và endpoint mapping theo cấu hình startup chuẩn hóa.
app.UseApiPipeline();
app.Run();

public partial class Program { }

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TarotNow.Api.Startup;
using Serilog;
using TarotNow.Infrastructure.Persistence;

// Nạp biến môi trường sớm để mọi cấu hình phía sau đọc được đầy đủ giá trị runtime.
EnvLoader.Load();

// Hỗ trợ mode one-shot để apply EF migrations trong quy trình bootstrap production.
if (args.Any(arg => string.Equals(arg, "--migrate", StringComparison.OrdinalIgnoreCase)))
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    var connectionString = configuration.GetConnectionString("PostgreSQL")
        ?? configuration["ConnectionStrings:PostgreSQL"];
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Missing required configuration ConnectionStrings:PostgreSQL (env: CONNECTIONSTRINGS__POSTGRESQL).");
    }

    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    optionsBuilder.UseNpgsql(connectionString, options =>
        options.MigrationsHistoryTable("__ef_migrations_history"));

    await using var dbContext = new ApplicationDbContext(optionsBuilder.Options);
    await dbContext.Database.MigrateAsync();
    return;
}

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

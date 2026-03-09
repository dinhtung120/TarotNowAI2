using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.MongoDb;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

/// <summary>
/// Fixture dùng chung cho các Integration Tests của tầng API.
/// Khởi tạo và quản lý vòng đời của Docker containers (PostgreSQL, MongoDB) thông qua Testcontainers.
/// </summary>
public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("tarotweb_test")
        .WithUsername("postgres")
        .WithPassword("postgres_test_password")
        .Build();

    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:7.0-jammy")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // TODO: Ở đây, chúng ta sẽ remove kết nối DB thật (nếu có đăng ký ở Program.cs)
            // và thay thế bằng connection string lấy từ _dbContainer và _mongoContainer
            // var postgresConnectionString = _dbContainer.GetConnectionString();
            // var mongoConnectionString = _mongoContainer.GetConnectionString();
            
            // Ví dụ:
            // services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            // services.AddDbContext<ApplicationDbContext>(options => ...);
        });

        // Đặt environment để load appsettings phù hợp nếu cần
        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        // Khởi động các Docker containers trước khi chạy test
        await _dbContainer.StartAsync();
        await _mongoContainer.StartAsync();

        // TODO: Cấu hình chạy DB Migration và Seed data (đọc file SQL/JS) ngay tại đây
    }

    public new async Task DisposeAsync()
    {
        // Dọn dẹp containers sau khi test xong
        await _dbContainer.DisposeAsync().AsTask();
        await _mongoContainer.DisposeAsync().AsTask();
    }
}



using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace TarotNow.Infrastructure.Persistence;

// Factory tạo DbContext thời gian thiết kế cho lệnh EF migrations.
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Tạo ApplicationDbContext dùng cho tooling EF ở design-time.
    /// Luồng xử lý: xác định environment, nạp cấu hình appsettings, resolve connection string và dựng DbContextOptions.
    /// </summary>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var currentDirectory = Directory.GetCurrentDirectory();
        var apiProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "TarotNow.Api"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQL")
            ?? configuration["ConnectionStrings:PostgreSQL"]
            ?? "Host=localhost;Port=5432;Database=tarotnow;Username=postgres;Password=postgres";
        // Fallback chuỗi kết nối mặc định để tooling vẫn chạy được khi thiếu cấu hình cục bộ.

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString, options =>
            options.MigrationsHistoryTable("__ef_migrations_history"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

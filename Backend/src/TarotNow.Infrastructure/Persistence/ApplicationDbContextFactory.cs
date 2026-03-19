/*
 * ===================================================================
 * FILE: ApplicationDbContextFactory.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence
 * ===================================================================
 * MỤC ĐÍCH:
 *   File mồi mài công cụ giúp thằng Entity Framework Core (EF Core) biết đường kết nối 
 *   vào DB PostgreSQL trong lúc gõ lệnh cmd chay (như `dotnet ef migrations add`).
 *   Không ảnh hưởng lúc Runtime hệ thống.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// Máy Kéo Quên Tạo Kết Nối Cho Chảo Migrations Chạy.
/// Trả Lời: "Tao Muốn Add Migrations Ngay Máy Tool Ngoài CMD Thì Tao Đọc Cấu Hình Mật Khẩu Kết Nối Của DB Ở Đầu Gì Đâu File API Đang Tắt Cơ Mà?"
/// Nó Trỏ Nhờ File Này Chỉ Thẳng Mũi Này Xuống Nhánh TarotNow.Api Tìm JSON Của DB.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Kiếm Env Cội Là File Gì (Thường Lấy Development Tát)
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        
        // Quăng Dò Tầm Hướng Ngược Ra Lấy Đường Dẫn Vị Trí Cha Từ Chỗ Go "Backend/src/TarotNow.Infrastructure" → Mò Về Lỗ "TarotNow.Api" Ôm JSON
        var currentDirectory = Directory.GetCurrentDirectory();
        var apiProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "TarotNow.Api"));

        // Build Khớp File Cấu Hình Dò Chìa appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Móc Cái Chuỗi Connection String "Host=...;" Ra, Gãy Đi Kế Phá Dành Tí Backup Default Dỏm Bọc Chặn Đã.
        var connectionString = configuration.GetConnectionString("PostgreSQL")
            ?? configuration["ConnectionStrings:PostgreSQL"]
            ?? "Host=localhost;Port=5432;Database=tarotnow;Username=postgres;Password=postgres";

        // Tọng Thông Số Chuỗi Cứng Trả Cho Builder Để Khởi Ném Factory Sinh Instance DbContext Nấu Cơm Dóng Entity Kịp Báo Migrations Tới DB Test.
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString, options =>
            options.MigrationsHistoryTable("__ef_migrations_history"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

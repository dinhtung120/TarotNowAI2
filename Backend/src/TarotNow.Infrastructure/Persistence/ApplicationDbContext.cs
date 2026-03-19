/*
 * ===================================================================
 * FILE: ApplicationDbContext.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence
 * ===================================================================
 * MỤC ĐÍCH:
 *   Nút Thắt Trung Tâm Đi Cửa Của Toàn Bộ Dữ Liệu SQL Bóng (PostgreSQL).
 *   Sợi Dây EF Core Này Trói Tất Cả Các Bảng Vàng Chặt Chẽ Lại, Áp Luật Đổi Tên CamelCase Sang snake_case Tự Động Để Ném Xọt Xuống DB Khỏi Lỗi Tên.
 * ===================================================================
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.Configurations;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// Thùng Chứa Cấu Phức Trung Tâm EF Core Giao Tiếp Database Quan Hệ (PostgreSQL - Lưu Tiền Ví, Điểm Tài Khoản Bệnh Khách).
/// 
/// Tại Sao Bọn ReadingSession/UserCollection Không Nằm Ở Đây Nữa?
/// → Mới Móc Rút Chuyển Mảng Quăng Sang Ruột MongoDB (Do Data Rác To JSON Quá Khủng) Để Giữ Nhẹ Bụng Cho Thằng Postgres Chuyên Làm Việc Ví Tiền Thanh Toán Cho Mượt (Tránh Lock DB).
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>Nạy Vung Đáy DB Trét Mỡ Gọi Cửa Migrations Gắn Đầu Cột.</summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Chỉnh Mảng Cục Bảng Ghi Dấu Tích Lịch Sử Migrations '__ef_migrations_history' Tuân Thủ Định Dạng Chữ Thường snake_case Hoàn Toàn Chống Rối OS Hệ Linux Phân Biệt Chữ Hoa/Thường.
        optionsBuilder.UseNpgsql(o => o.MigrationsHistoryTable("__ef_migrations_history"));
    }

    // ======================================================================
    // CỘT MỐC LẬP DANH SÁCH BẢNG (DBSETS) ẢNH XẠ TRỰC TIẾP TỪ ENTITIES LÊN SQL
    // ======================================================================
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<EmailOtp> EmailOtps { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<AiRequest> AiRequests { get; set; } = null!;
    public DbSet<UserConsent> UserConsents { get; set; } = null!;
    public DbSet<DepositOrder> DepositOrders { get; set; } = null!;
    public DbSet<DepositPromotion> DepositPromotions { get; set; } = null!;

    // Phase 2.3 Escrow — Khung Chặn Tiền Cầm Đồ Ví Chat Lấy Kim Cương
    public DbSet<ChatFinanceSession> ChatFinanceSessions { get; set; } = null!;
    public DbSet<ChatQuestionItem> ChatQuestionItems { get; set; } = null!;

    // Phase 2.4 Withdrawal — Giấy Viết Tờ Đơn Xin Chuyển Tiền Trắng Về ATM
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; } = null!;

    // ======================================================================
    // NẶN KHUNG SCHEMA SQL TRƯỚC KHI MIGRATION ỤP XUỐNG CƠ SỞ DỮ LIỆU
    // ======================================================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 1. Quét Sạch Mọi Đống Rule Cắt Chỉnh Của Từng Entities Rời Rạc (VD: File Cục AiRequestConfiguration Chứa Quy Tắc Nằm Đó) 
        // Phải Ném Gắn Rule Đầu Tiên Trước Khi Đi Ép snake_case Ở Dưới Để Tụi Nó Còn Cãi Kịp Nhau.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Đống Nhọ Phế Legacy 1 Thuở Gọi Bỏ Án Gắn Đợi Gãy Do Vứt Ánh Chuyển Qua Mongo (Ignore Hủy Không Migrations Lữa Kẻo Sinh Bảng Rác DB PostgreSQL Xưa).
        modelBuilder.Ignore<ReadingSession>();
        modelBuilder.Ignore<UserCollection>();

        // 2. Hàm Thần Sầu Tự Động Kẻ Regex Quất Lùa Chữ Tên Bảng, Tên Cột, Khóa Ngoại Xuống Standard Thường snake_case Đi Chợ (UserId -> user_id).
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Úp Mảng Tên Bảng Chữ Cái Thường Kẹp Gạch Dưới (Trừ Của Tuổi Đứa Thuộc Value Object Sở Hữu Thuê OwnedType).
            if (!entity.IsOwned())
            {
                var tableName = entity.GetTableName();
                if (tableName != null) entity.SetTableName(ToSnakeCase(tableName));
            }

            // Quét Cột Biến Hóa Dấu Gạch 
            foreach (var property in entity.GetProperties())
            {
                // Riêng Lĩnh Thằng Value Object Ví Dụ UserWallet Chứa Lòng Trong Bảng User, Thì Cột Khoá ID Trả Gắn Là "id" Chống Nổi Trôi Lỗi Cột Văng Entity PK Chết.
                if (entity.IsOwned() && property.IsPrimaryKey())
                {
                    property.SetColumnName("id");
                    continue;
                }

                // Cột Khác Chưa Ai Định Danh Ném Tên Phân Gạch Ra (Tránh Thằng Nào Configuration Cứu Được Override Thì Không Đập Nó Nhé).
                var colName = property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema()));
                if (string.IsNullOrEmpty(colName) || colName == property.Name)
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }
            }

            // Mò Đuổi Mép Khóa Chính PK
            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (keyName != null) key.SetName(ToSnakeCase(keyName));
            }

            // Mò Sửa Lại Tên Cột Tham Chiếu Khóa Ngoại FK (Tránh Nét PK Báo Lỗi EF SQL Đánh Tên Gắt Lệnh FK_User_Role Loằng Ngoằng)
            foreach (var fk in entity.GetForeignKeys())
            {
                var fkName = fk.GetConstraintName();
                if (fkName != null) fk.SetConstraintName(ToSnakeCase(fkName));
            }

            // Bẻ Nắn Cột Indexes Truy Xuất Tức Thì.
            foreach (var index in entity.GetIndexes())
            {
                var idxName = index.GetDatabaseName();
                if (idxName != null) index.SetDatabaseName(ToSnakeCase(idxName));
            }
        }
    }

    /// <summary>
    /// Thuật Toán Text Quấn Cột Chữ PascalCase "ChatFinanceSession" → Xắt Ra Xương "chat_finance_session".
    /// Tiện Ích Đỉnh Bóp Tròn Trữ Tên DB Tuân Thủ Cú Pháp Linux PostgreSQL Thường.
    /// </summary>
    private string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }
        // Biến Regex Tìm Giữa Chữ Cụm Kí Tự Chuyển Chữ Hoa Ra Gạch Dưới Ghép Rồi Kéo Thành ToLower Nhẹ.
        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}

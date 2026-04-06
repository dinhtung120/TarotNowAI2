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
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.Configurations;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// Thùng Chứa Cấu Phức Trung Tâm EF Core Giao Tiếp Database Quan Hệ (PostgreSQL - Lưu Tiền Ví, Điểm Tài Khoản Bệnh Khách).
/// 
/// Tại Sao Bọn ReadingSession/UserCollection Không Nằm Ở Đây Nữa?
/// → Mới Móc Rút Chuyển Mảng Quăng Sang Ruột MongoDB (Do Data Rác To JSON Quá Khủng) Để Giữ Nhẹ Bụng Cho Thằng Postgres Chuyên Làm Việc Ví Tiền Thanh Toán Cho Mượt (Tránh Lock DB).
/// </summary>
public partial class ApplicationDbContext : DbContext
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
    public DbSet<ChatFinanceSession> ChatFinanceSessions { get; set; } = null!;
    public DbSet<ChatQuestionItem> ChatQuestionItems { get; set; } = null!;

    // Phase 5.2 - Subscriptions & Entitlements
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;
    public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
    public DbSet<SubscriptionEntitlementBucket> SubscriptionEntitlementBuckets { get; set; } = null!;
    public DbSet<EntitlementConsume> EntitlementConsumes { get; set; } = null!;
    public DbSet<EntitlementMappingRule> EntitlementMappingRules { get; set; } = null!;

    // Phase 2.4 Withdrawal — Giấy Viết Tờ Đơn Xin Chuyển Tiền Trắng Về ATM
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; } = null!;

    // Phase 5.6 Gacha - Vòng Quay
    public DbSet<GachaBanner> GachaBanners { get; set; } = null!;
    public DbSet<GachaBannerItem> GachaBannerItems { get; set; } = null!;
    public DbSet<GachaRewardLog> GachaRewardLogs { get; set; } = null!;

    // ======================================================================
    // NẶN KHUNG SCHEMA SQL TRƯỚC KHI MIGRATION ỤP XUỐNG CƠ SỞ DỮ LIỆU
    // ======================================================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyEntityConfigurations(modelBuilder);
        ApplySnakeCaseConventions(modelBuilder);
    }
}

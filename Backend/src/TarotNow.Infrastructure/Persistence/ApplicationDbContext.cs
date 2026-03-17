using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.Configurations;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// DbContext chính của ứng dụng kết nối với PostgreSQL.
/// 
/// Lưu ý: ReadingSession và UserCollection đã được chuyển sang MongoDB
/// (xem MongoDbContext). Chỉ còn các entity thuần PostgreSQL ở đây.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Đồng bộ bảng history về snake_case để tránh conflict case-sensitivity
        optionsBuilder.UseNpgsql(o => o.MigrationsHistoryTable("__ef_migrations_history"));
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<EmailOtp> EmailOtps { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<AiRequest> AiRequests { get; set; } = null!;
    public DbSet<UserConsent> UserConsents { get; set; } = null!;
    public DbSet<DepositOrder> DepositOrders { get; set; } = null!;
    public DbSet<DepositPromotion> DepositPromotions { get; set; } = null!;

    // Phase 2.3 Escrow — EF Core entities cho PostgreSQL tables
    public DbSet<ChatFinanceSession> ChatFinanceSessions { get; set; } = null!;
    public DbSet<ChatQuestionItem> ChatQuestionItems { get; set; } = null!;

    // Phase 2.4 Withdrawal — EF Core entity cho PostgreSQL
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 1. Áp dụng các cấu hình tường minh từ Assembly (VD: UserConfiguration)
        // Cần làm điều này TRƯỚC khi chạy loop đặt tên snake_case để các HasColumnName được ưu tiên.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // 2. Tự động chuyển đổi tên bảng và cột sang snake_case cho PostgreSQL
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Tên bảng -> snake_case (Chỉ dành cho các bảng thực, không phải Owned Types)
            if (!entity.IsOwned())
            {
                var tableName = entity.GetTableName();
                if (tableName != null) entity.SetTableName(ToSnakeCase(tableName));
            }

            foreach (var property in entity.GetProperties())
            {
                // Quy tắc cho Owned Types: Giữ PK trùng với Owner 
                if (entity.IsOwned() && property.IsPrimaryKey())
                {
                    property.SetColumnName("id");
                    continue;
                }

                // Chỉ đặt tên snake_case nếu property đó chưa có Column Name tường minh hoặc Column Name đang trùng với Property Name
                var colName = property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema()));
                if (string.IsNullOrEmpty(colName) || colName == property.Name)
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (keyName != null) key.SetName(ToSnakeCase(keyName));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                var fkName = fk.GetConstraintName();
                if (fkName != null) fk.SetConstraintName(ToSnakeCase(fkName));
            }

            foreach (var index in entity.GetIndexes())
            {
                var idxName = index.GetDatabaseName();
                if (idxName != null) index.SetDatabaseName(ToSnakeCase(idxName));
            }
        }
    }

    private string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }
        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}

using Microsoft.EntityFrameworkCore;
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

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<EmailOtp> EmailOtps { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<AiRequest> AiRequests { get; set; } = null!;
    public DbSet<UserConsent> UserConsents { get; set; } = null!;
    public DbSet<DepositOrder> DepositOrders { get; set; } = null!;
    public DbSet<DepositPromotion> DepositPromotions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Tên bảng -> snake_case
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
                    // Thường là shadow property "Id" hoặc "UserId"
                    // Ta map nó về "id" (hoặc tên PK của owner)
                    property.SetColumnName("id");
                    continue;
                }

                var colName = property.GetColumnName();
                if (colName != null) property.SetColumnName(ToSnakeCase(colName));
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

        // Scan tất cả EntityTypeConfiguration trong asembly hiện tại chứa DbContext này
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }
        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}

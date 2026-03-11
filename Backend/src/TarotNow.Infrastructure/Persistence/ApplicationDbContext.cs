using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.Configurations;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// DbContext chính của ứng dụng kết nối với PostgreSQL.
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
    public DbSet<ReadingSession> ReadingSessions { get; set; } = null!;
    public DbSet<UserCollection> UserCollections { get; set; } = null!;
    public DbSet<AiRequest> AiRequests { get; set; } = null!;
    public DbSet<UserConsent> UserConsents { get; set; } = null!;
    public DbSet<DepositOrder> DepositOrders { get; set; } = null!;
    public DbSet<DepositPromotion> DepositPromotions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Cấu hình quy ước chuẩn Snake Case (tương thích PostgreSQL schema.sql)
        // Để mapping các field tự nhiên như "PasswordHash" -> "password_hash"
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null)
                entity.SetTableName(ToSnakeCase(tableName));

            foreach (var property in entity.GetProperties())
            {
                // Chỉ translate column name nếu config kia chưa explicit set HasColumnName()
                var mappedColumn = property.GetColumnName();
                property.SetColumnName(ToSnakeCase(mappedColumn));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()));
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
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

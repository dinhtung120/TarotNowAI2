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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Scan tất cả EntityTypeConfiguration trong asembly hiện tại chứa DbContext này
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

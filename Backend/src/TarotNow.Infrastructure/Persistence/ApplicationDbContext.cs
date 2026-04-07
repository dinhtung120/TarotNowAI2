

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.Configurations;

namespace TarotNow.Infrastructure.Persistence;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
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
    public DbSet<ChatFinanceSession> ChatFinanceSessions { get; set; } = null!;
    public DbSet<ChatQuestionItem> ChatQuestionItems { get; set; } = null!;

    
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;
    public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
    public DbSet<SubscriptionEntitlementBucket> SubscriptionEntitlementBuckets { get; set; } = null!;
    public DbSet<EntitlementConsume> EntitlementConsumes { get; set; } = null!;
    public DbSet<EntitlementMappingRule> EntitlementMappingRules { get; set; } = null!;

    
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; } = null!;

    
    public DbSet<GachaBanner> GachaBanners { get; set; } = null!;
    public DbSet<GachaBannerItem> GachaBannerItems { get; set; } = null!;
    public DbSet<GachaRewardLog> GachaRewardLogs { get; set; } = null!;

    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyEntityConfigurations(modelBuilder);
        ApplySnakeCaseConventions(modelBuilder);
    }
}

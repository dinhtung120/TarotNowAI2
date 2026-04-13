

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.Outbox;
using TarotNow.Infrastructure.Persistence.Configurations;

namespace TarotNow.Infrastructure.Persistence;

public partial class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Khởi tạo DbContext với options đã được cấu hình từ DI.
    /// Luồng xử lý: truyền options cho DbContext base để dùng trong runtime.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Cấu hình bổ sung cho DbContext khi build model.
    /// Luồng xử lý: chỉ định tên bảng migration history để đồng bộ chuẩn đặt tên.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(o => o.MigrationsHistoryTable("__ef_migrations_history"));
        // Ép tên bảng migration history cố định để tránh lệch giữa môi trường.
    }

    // DbSet bảng users.
    public DbSet<User> Users { get; set; } = null!;

    // DbSet bảng refresh_tokens.
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    // DbSet bảng email_otps.
    public DbSet<EmailOtp> EmailOtps { get; set; }

    // DbSet bảng wallet_transactions.
    public DbSet<WalletTransaction> WalletTransactions { get; set; }

    // DbSet bảng ai_requests.
    public DbSet<AiRequest> AiRequests { get; set; } = null!;

    // DbSet bảng user_consents.
    public DbSet<UserConsent> UserConsents { get; set; } = null!;

    // DbSet bảng deposit_orders.
    public DbSet<DepositOrder> DepositOrders { get; set; } = null!;

    // DbSet bảng deposit_promotions.
    public DbSet<DepositPromotion> DepositPromotions { get; set; } = null!;

    // DbSet bảng chat_finance_sessions.
    public DbSet<ChatFinanceSession> ChatFinanceSessions { get; set; } = null!;

    // DbSet bảng chat_question_items.
    public DbSet<ChatQuestionItem> ChatQuestionItems { get; set; } = null!;

    // DbSet bảng subscription_plans.
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;

    // DbSet bảng user_subscriptions.
    public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;

    // DbSet bảng subscription_entitlement_buckets.
    public DbSet<SubscriptionEntitlementBucket> SubscriptionEntitlementBuckets { get; set; } = null!;

    // DbSet bảng entitlement_consumes.
    public DbSet<EntitlementConsume> EntitlementConsumes { get; set; } = null!;

    // DbSet bảng entitlement_mapping_rules.
    public DbSet<EntitlementMappingRule> EntitlementMappingRules { get; set; } = null!;

    // DbSet bảng withdrawal_requests.
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; } = null!;

    // DbSet bảng gacha_banners.
    public DbSet<GachaBanner> GachaBanners { get; set; } = null!;

    // DbSet bảng gacha_banner_items.
    public DbSet<GachaBannerItem> GachaBannerItems { get; set; } = null!;

    // DbSet bảng gacha_reward_logs.
    public DbSet<GachaRewardLog> GachaRewardLogs { get; set; } = null!;

    // DbSet bảng outbox_messages.
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    // DbSet bảng outbox_handler_states.
    public DbSet<OutboxHandlerState> OutboxHandlerStates { get; set; } = null!;

    /// <summary>
    /// Xây dựng model mapping cho DbContext.
    /// Luồng xử lý: gọi base.OnModelCreating, áp cấu hình entity từ assembly và chuẩn hóa naming snake_case.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyEntityConfigurations(modelBuilder);
        ApplySnakeCaseConventions(modelBuilder);
    }
}



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

    // DbSet bảng auth_sessions.
    public DbSet<AuthSession> AuthSessions { get; set; } = null!;

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

    // DbSet bảng system_configs.
    public DbSet<SystemConfig> SystemConfigs { get; set; } = null!;

    // DbSet bảng chat_finance_sessions.
    public DbSet<ChatFinanceSession> ChatFinanceSessions { get; set; } = null!;

    // DbSet bảng chat_question_items.
    public DbSet<ChatQuestionItem> ChatQuestionItems { get; set; } = null!;

    // DbSet bảng withdrawal_requests.
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; } = null!;

    // DbSet bảng gacha_pools.
    public DbSet<GachaPool> GachaPools { get; set; } = null!;

    // DbSet bảng gacha_pool_reward_rates.
    public DbSet<GachaPoolRewardRate> GachaPoolRewardRates { get; set; } = null!;

    // DbSet bảng gacha_pull_operations.
    public DbSet<GachaPullOperation> GachaPullOperations { get; set; } = null!;

    // DbSet bảng gacha_pull_reward_logs.
    public DbSet<GachaPullRewardLog> GachaPullRewardLogs { get; set; } = null!;

    // DbSet bảng user_gacha_pities.
    public DbSet<UserGachaPity> UserGachaPities { get; set; } = null!;

    // DbSet bảng gacha_history_entries.
    public DbSet<GachaHistoryEntry> GachaHistoryEntries { get; set; } = null!;

    // DbSet bảng item_definitions.
    public DbSet<ItemDefinition> ItemDefinitions { get; set; } = null!;

    // DbSet bảng user_items.
    public DbSet<UserItem> UserItems { get; set; } = null!;

    // DbSet bảng inventory_item_use_operations.
    public DbSet<InventoryItemUseOperation> InventoryItemUseOperations { get; set; } = null!;

    // DbSet bảng free_draw_credits.
    public DbSet<FreeDrawCredit> FreeDrawCredits { get; set; } = null!;

    // DbSet bảng inventory_luck_effects.
    public DbSet<InventoryLuckEffect> InventoryLuckEffects { get; set; } = null!;

    // DbSet bảng outbox_messages.
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    // DbSet bảng outbox_handler_states.
    public DbSet<OutboxHandlerState> OutboxHandlerStates { get; set; } = null!;

    // DbSet bảng outbox_inline_handler_states.
    public DbSet<OutboxInlineHandlerState> OutboxInlineHandlerStates { get; set; } = null!;

    // DbSet bảng reading_reveal_saga_states.
    public DbSet<ReadingRevealSagaState> ReadingRevealSagaStates { get; set; } = null!;

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

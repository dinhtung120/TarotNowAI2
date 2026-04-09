using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.BackgroundJobs;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Repositories;
using TarotNow.Infrastructure.Repositories;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure;

public static partial class DependencyInjection
{
    /// <summary>
    /// Đăng ký toàn bộ repository/worker liên quan dữ liệu và xử lý nền.
    /// Luồng xử lý: chia nhóm đăng ký core, mongo, gamification, gacha, support và hosted workers.
    /// </summary>
    private static void AddRepositories(IServiceCollection services)
    {
        AddCoreRepositories(services);
        AddMongoRepositories(services);
        AddGamificationRepositories(services);
        AddGachaRepositories(services);
        AddSupportRepositories(services);
        AddHostedWorkers(services);
    }

    /// <summary>
    /// Đăng ký repository lõi dùng cho auth, wallet, consent, deposit và subscription.
    /// Luồng xử lý: bind interface -> implementation dạng scoped theo request scope.
    /// </summary>
    private static void AddCoreRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IEmailOtpRepository, EmailOtpRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
        services.AddScoped<ICommunityReactionRepository, CommunityReactionRepository>();
        services.AddScoped<ICommunityCommentRepository, CommunityCommentRepository>();
        services.AddScoped<IAiRequestRepository, AiRequestRepository>();
        services.AddScoped<ILedgerRepository, LedgerRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IUserConsentRepository, UserConsentRepository>();
        services.AddScoped<IDepositOrderRepository, DepositOrderRepository>();
        services.AddScoped<IDepositPromotionRepository, DepositPromotionRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ITransactionCoordinator, TransactionCoordinator>();
    }

    /// <summary>
    /// Đăng ký repository Mongo và orchestrator cho các module chat/reading/report/gamification phụ trợ.
    /// Luồng xử lý: bind các repository Mongo dạng scoped và service streak liên quan.
    /// </summary>
    private static void AddMongoRepositories(IServiceCollection services)
    {
        services.AddScoped<IReadingSessionRepository, MongoReadingSessionRepository>();
        services.AddScoped<IReadingSessionOrchestrator, ReadingSessionOrchestrator>();
        services.AddScoped<IUserCollectionRepository, MongoUserCollectionRepository>();
        services.AddScoped<ICardsCatalogRepository, MongoCardsCatalogRepository>();
        services.AddScoped<IAiProviderLogRepository, MongoAiProviderLogRepository>();
        services.AddScoped<INotificationRepository, MongoNotificationRepository>();
        services.AddScoped<IReaderRequestRepository, MongoReaderRequestRepository>();
        services.AddScoped<IReaderProfileRepository, MongoReaderProfileRepository>();
        services.AddScoped<IConversationRepository, MongoConversationRepository>();
        services.AddScoped<IChatMessageRepository, MongoChatMessageRepository>();
        services.AddScoped<IReportRepository, MongoReportRepository>();
        services.AddScoped<IDailyCheckinRepository, MongoDailyCheckinRepository>();
        services.AddScoped<IStreakService, StreakService>();
    }

    /// <summary>
    /// Đăng ký repository/service của nhóm gamification.
    /// Luồng xử lý: bind quest/achievement/title/leaderboard repositories và service push/tính điểm.
    /// </summary>
    private static void AddGamificationRepositories(IServiceCollection services)
    {
        services.AddScoped<IQuestRepository, MongoQuestRepository>();
        services.AddScoped<IAchievementRepository, MongoAchievementRepository>();
        services.AddScoped<ITitleRepository, MongoTitleRepository>();
        services.AddScoped<ILeaderboardRepository, MongoLeaderboardRepository>();
        services.AddScoped<IGamificationService, GamificationService>();
        services.AddScoped<IGamificationPushService, GamificationPushService>();
    }

    /// <summary>
    /// Đăng ký repository nhóm gacha.
    /// Luồng xử lý: bind repository banner/quay thưởng và log gacha.
    /// </summary>
    private static void AddGachaRepositories(IServiceCollection services)
    {
        services.AddScoped<IGachaRepository, GachaRepository>();
        services.AddScoped<IGachaLogRepository, GachaLogRepository>();
    }

    /// <summary>
    /// Đăng ký repository/service hỗ trợ khác (chat finance, withdrawal, MFA, moderation queue).
    /// Luồng xử lý: bind service scoped và singleton queue moderation dùng chung toàn app.
    /// </summary>
    private static void AddSupportRepositories(IServiceCollection services)
    {
        services.AddScoped<IChatFinanceRepository, ChatFinanceRepository>();
        services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
        services.AddScoped<IMfaService, TotpMfaService>();

        services.AddSingleton<ChatModerationQueue>();
        services.AddSingleton<IChatModerationQueue>(sp => sp.GetRequiredService<ChatModerationQueue>());
        // Dùng một queue singleton để mọi producer/consumer moderation chia sẻ cùng kênh dữ liệu.
    }

    /// <summary>
    /// Đăng ký các hosted worker chạy nền.
    /// Luồng xử lý: thêm hosted service cho escrow/moderation/streak/entitlement/subscription jobs.
    /// </summary>
    private static void AddHostedWorkers(IServiceCollection services)
    {
        services.AddHostedService<EscrowTimerService>();
        services.AddHostedService<ChatModerationWorker>();
        services.AddHostedService<StreakBreakBackgroundJob>();
        services.AddHostedService<EntitlementDailyResetJob>();
        services.AddHostedService<SubscriptionExpiryJob>();
    }
}

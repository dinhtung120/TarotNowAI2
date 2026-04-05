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
    private static void AddRepositories(IServiceCollection services)
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
        services.AddScoped<ITransactionCoordinator, TransactionCoordinator>();

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

        services.AddScoped<IChatFinanceRepository, ChatFinanceRepository>();
        services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
        services.AddScoped<IMfaService, TotpMfaService>();

        services.AddSingleton<ChatModerationQueue>();
        services.AddSingleton<IChatModerationQueue>(sp => sp.GetRequiredService<ChatModerationQueue>());

        services.AddHostedService<EscrowTimerService>();
        services.AddHostedService<ChatModerationWorker>();
    }
}

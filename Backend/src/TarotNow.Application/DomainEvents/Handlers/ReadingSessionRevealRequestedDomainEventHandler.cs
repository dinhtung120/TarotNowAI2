using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler write-side cho reveal reading session.
/// </summary>
public sealed partial class ReadingSessionRevealRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionRevealRequestedDomainEvent>
{
    private readonly record struct ReadingChargeSnapshot(
        bool Debited,
        string Currency,
        string ChangeType,
        long Amount,
        string ReferenceId);

    private readonly IReadingSessionRepository _readingSessionRepository;
    private readonly IReadingRevealSagaStateRepository _readingRevealSagaStateRepository;
    private readonly IUserCollectionRepository _userCollectionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IFreeDrawCreditRepository _freeDrawCreditRepository;
    private readonly IRngService _rngService;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo handler reveal-requested.
    /// </summary>
    public ReadingSessionRevealRequestedDomainEventHandler(
        IReadingSessionRepository readingSessionRepository,
        IReadingRevealSagaStateRepository readingRevealSagaStateRepository,
        IUserCollectionRepository userCollectionRepository,
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        IFreeDrawCreditRepository freeDrawCreditRepository,
        IRngService rngService,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings,
        ITransactionCoordinator transactionCoordinator,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readingSessionRepository = readingSessionRepository;
        _readingRevealSagaStateRepository = readingRevealSagaStateRepository;
        _userCollectionRepository = userCollectionRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _freeDrawCreditRepository = freeDrawCreditRepository;
        _rngService = rngService;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
        _transactionCoordinator = transactionCoordinator;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var session = await GetSessionAsync(domainEvent, cancellationToken);
        var saga = await _readingRevealSagaStateRepository.GetOrCreateAsync(
            domainEvent.SessionId,
            domainEvent.UserId,
            domainEvent.Language,
            cancellationToken);

        if (await TryHandleCompletedSessionReplayAsync(domainEvent, session, saga, cancellationToken))
        {
            return;
        }

        var revealedCards = await ExecuteSagaWorkflowAsync(
            domainEvent,
            session,
            saga,
            cancellationToken);

        domainEvent.RevealedCards = revealedCards;
        domainEvent.IsIdempotentReplay = false;
    }

    private async Task<ReadingSession> GetSessionAsync(
        ReadingSessionRevealRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var session = await _readingSessionRepository.GetByIdAsync(domainEvent.SessionId, cancellationToken)
            ?? throw new NotFoundException("Session not found");

        if (session.UserId != domainEvent.UserId.ToString())
        {
            throw new ForbiddenException("Reading session not found or access denied");
        }

        return session;
    }
}

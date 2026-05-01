

using MediatR;
using System;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<SendMessageCommandHandlerRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IReaderProfileRepository _readerProfileRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IUploadSessionRepository _uploadSessionRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;
    private readonly ICacheService _cacheService;
    private readonly IChatRealtimeFastLanePublisher _chatRealtimeFastLanePublisher;

    public SendMessageCommandHandlerRequestedDomainEventHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo,
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        IReaderProfileRepository readerProfileRepo,
        ITransactionCoordinator transactionCoordinator,
        IUploadSessionRepository uploadSessionRepository,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings,
        ICacheService cacheService,
        IChatRealtimeFastLanePublisher chatRealtimeFastLanePublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _readerProfileRepo = readerProfileRepo;
        _transactionCoordinator = transactionCoordinator;
        _uploadSessionRepository = uploadSessionRepository;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
        _cacheService = cacheService;
        _chatRealtimeFastLanePublisher = chatRealtimeFastLanePublisher;
    }

    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        var conversation = await LoadConversationAsync(request, cancellationToken);
        var senderId = request.SenderId.ToString();

        ValidateSender(conversation, senderId);
        ValidateConversationForSend(conversation, senderId, request.Type);
        var existingMessage = await ResolveExistingClientMessageAsync(request, cancellationToken);
        if (existingMessage is not null)
        {
            return existingMessage;
        }

        await ValidateAndConsumeMediaUploadSessionAsync(request, conversation, cancellationToken);
        var message = BuildMessage(request, senderId);

        // Chỉ trigger freeze khi là tin nhắn đầu tiên của user ở trạng thái Pending.
        var firstMessageFreeze = await TryFreezeMainQuestionOnFirstUserMessageAsync(
            conversation,
            senderId,
            message.Id,
            cancellationToken);
        ApplyConversationStateTransition(conversation, senderId, firstMessageFreeze.OfferExpiresAtUtc);

        return await PersistMessageFlowAsync(
            new PersistMessageFlowContext(
                request,
                conversation,
                senderId,
                message,
                firstMessageFreeze),
            cancellationToken);
    }

    private async Task<ChatMessageDto?> ResolveExistingClientMessageAsync(
        SendMessageCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedClientMessageId = NormalizeClientMessageId(request.ClientMessageId);
        if (string.IsNullOrWhiteSpace(normalizedClientMessageId))
        {
            return null;
        }

        request.ClientMessageId = normalizedClientMessageId;
        return await _messageRepo.GetByConversationAndClientMessageIdAsync(
            request.ConversationId,
            normalizedClientMessageId,
            cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        SendMessageCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }

    private readonly record struct PersistMessageFlowContext(
        SendMessageCommand Request,
        ConversationDto Conversation,
        string SenderId,
        ChatMessageDto Message,
        FirstMessageFreezeResult FirstMessageFreeze);
}

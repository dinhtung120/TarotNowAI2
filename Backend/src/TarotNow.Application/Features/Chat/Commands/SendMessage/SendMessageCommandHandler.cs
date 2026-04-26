

using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

// Handler điều phối toàn bộ luồng gửi tin nhắn trong conversation.
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

    /// <summary>
    /// Khởi tạo handler gửi tin nhắn.
    /// Luồng xử lý: nhận các repository/service cần cho validate, media processing, freeze tài chính và push số dư.
    /// </summary>
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
    }

    /// <summary>
    /// Xử lý gửi message vào conversation.
    /// Luồng xử lý: validate request, xử lý media, kiểm tra quyền/trạng thái conversation, freeze main question nếu là tin nhắn đầu, lưu message và cập nhật state liên quan.
    /// </summary>
    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        var conversation = await LoadConversationAsync(request, cancellationToken);
        var senderId = request.SenderId.ToString();

        ValidateSender(conversation, senderId);
        ValidateConversationForSend(conversation, senderId, request.Type);
        await ValidateAndConsumeMediaUploadSessionAsync(request, conversation, cancellationToken);

        // Chỉ trigger freeze khi là tin nhắn đầu tiên của user ở trạng thái Pending.
        var firstMessageFreeze = await TryFreezeMainQuestionOnFirstUserMessageAsync(
            conversation,
            senderId,
            request.Type,
            cancellationToken);
        ApplyConversationStateTransition(conversation, senderId, firstMessageFreeze.OfferExpiresAtUtc);

        var message = BuildMessage(request, senderId);
        await _messageRepo.AddAsync(message, cancellationToken);
        // Nếu reader vừa trả lời message đủ điều kiện, cập nhật mốc replied cho item Accepted.
        await TryMarkReaderRepliedAsync(conversation, senderId, request.Type, cancellationToken);

        IncrementUnreadCounter(conversation, senderId);

        if (firstMessageFreeze.IsTriggered)
        {
            var systemMessage = BuildSystemMessage(
                conversation.Id,
                senderId,
                $"Đã đóng băng {firstMessageFreeze.AmountDiamond} 💎 cho cuộc chat này. Đang chờ Reader phản hồi.");
            await _messageRepo.AddAsync(systemMessage, cancellationToken);
            conversation.LastMessageAt = systemMessage.CreatedAt;
            conversation.UpdatedAt = systemMessage.CreatedAt;
        }
        else
        {
            // Luồng gửi thông thường: cập nhật timeline theo message vừa gửi.
            conversation.LastMessageAt = message.CreatedAt;
            conversation.UpdatedAt = message.CreatedAt;
        }

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);
        await PublishFreezeEventsAsync(request.SenderId, conversation, firstMessageFreeze, cancellationToken);
        await PublishRealtimeEventsAsync(conversation, message, cancellationToken);

        return message;
    }

    private async Task PublishFreezeEventsAsync(
        Guid senderId,
        ConversationDto conversation,
        FirstMessageFreezeResult firstMessageFreeze,
        CancellationToken cancellationToken)
    {
        if (firstMessageFreeze.IsTriggered)
        {
            await _domainEventPublisher.PublishAsync(new Domain.Events.MoneyChangedDomainEvent
            {
                UserId = senderId,
                Currency = Domain.Enums.CurrencyType.Diamond,
                ChangeType = Domain.Enums.TransactionType.EscrowFreeze,
                DeltaAmount = -firstMessageFreeze.AmountDiamond,
                ReferenceId = conversation.Id
            }, cancellationToken);

            // Chuyển string ReaderId thành Guid an toàn hoặc bỏ qua luồng event nếu sai định dạng.
            if (Guid.TryParse(conversation.ReaderId, out var readerGuid))
            {
                // Phát DomainEvent thông báo Reader cần phê duyệt câu hỏi (ngầm định kích hoạt email)
                await _domainEventPublisher.PublishAsync(new Domain.Events.ChatOfferReceivedDomainEvent
                {
                    ConversationId = conversation.Id,
                    ReaderId = readerGuid,
                    UserId = senderId,
                    OfferExpiresAtUtc = firstMessageFreeze.OfferExpiresAtUtc!.Value
                }, cancellationToken);
            }
        }
    }

    private async Task PublishRealtimeEventsAsync(
        ConversationDto conversation,
        ChatMessageDto message,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ChatMessageCreatedDomainEvent
            {
                ConversationId = conversation.Id,
                MessageId = message.Id,
                SenderId = message.SenderId,
                MessageType = message.Type,
                OccurredAtUtc = message.CreatedAt
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "message_created", message.CreatedAt),
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.UnreadCountChangedDomainEvent
            {
                ConversationId = conversation.Id,
                UserId = conversation.UserId,
                ReaderId = conversation.ReaderId,
                OccurredAtUtc = message.CreatedAt
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ChatModerationRequestedDomainEvent
            {
                MessageId = message.Id,
                ConversationId = conversation.Id,
                SenderId = message.SenderId,
                MessageType = message.Type,
                Content = message.Content,
                CreatedAtUtc = message.CreatedAt
            },
            cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        SendMessageCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

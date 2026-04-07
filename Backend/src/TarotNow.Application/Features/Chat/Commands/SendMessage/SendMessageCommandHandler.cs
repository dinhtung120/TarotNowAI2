

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ChatMessageDto>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IReaderProfileRepository _readerProfileRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IMediaProcessorService _mediaProcessor;
    private readonly IWalletPushService _walletPushService;

    public SendMessageCommandHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo,
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        IReaderProfileRepository readerProfileRepo,
        ITransactionCoordinator transactionCoordinator,
        IMediaProcessorService mediaProcessor,
        IWalletPushService walletPushService)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _readerProfileRepo = readerProfileRepo;
        _transactionCoordinator = transactionCoordinator;
        _mediaProcessor = mediaProcessor;
        _walletPushService = walletPushService;
    }

    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        await ProcessMediaRequestAsync(request, cancellationToken);
        var conversation = await LoadConversationAsync(request, cancellationToken);
        var senderId = request.SenderId.ToString();

        ValidateSender(conversation, senderId);
        ValidateConversationForSend(conversation, senderId, request.Type);

        var firstMessageFreeze = await TryFreezeMainQuestionOnFirstUserMessageAsync(
            conversation,
            senderId,
            request.Type,
            cancellationToken);
        ApplyConversationStateTransition(conversation, senderId, firstMessageFreeze.OfferExpiresAtUtc);

        var message = BuildMessage(request, senderId);
        await _messageRepo.AddAsync(message, cancellationToken);
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
            conversation.LastMessageAt = message.CreatedAt;
            conversation.UpdatedAt = message.CreatedAt;
        }

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);

        
        if (firstMessageFreeze.IsTriggered)
        {
            await _walletPushService.PushBalanceChangedAsync(request.SenderId, cancellationToken);
        }

        return message;
    }

}

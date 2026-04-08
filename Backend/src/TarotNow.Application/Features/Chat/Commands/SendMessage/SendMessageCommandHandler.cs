

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

// Handler điều phối toàn bộ luồng gửi tin nhắn trong conversation.
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

    /// <summary>
    /// Khởi tạo handler gửi tin nhắn.
    /// Luồng xử lý: nhận các repository/service cần cho validate, media processing, freeze tài chính và push số dư.
    /// </summary>
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

    /// <summary>
    /// Xử lý gửi message vào conversation.
    /// Luồng xử lý: validate request, xử lý media, kiểm tra quyền/trạng thái conversation, freeze main question nếu là tin nhắn đầu, lưu message và cập nhật state liên quan.
    /// </summary>
    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        // Nén/chuyển đổi media data-uri trước khi lưu để tối ưu kích thước payload.
        await ProcessMediaRequestAsync(request, cancellationToken);
        var conversation = await LoadConversationAsync(request, cancellationToken);
        var senderId = request.SenderId.ToString();

        ValidateSender(conversation, senderId);
        ValidateConversationForSend(conversation, senderId, request.Type);

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

        if (firstMessageFreeze.IsTriggered)
        {
            // Khi có freeze, đẩy cập nhật số dư realtime để user thấy thay đổi ngay.
            await _walletPushService.PushBalanceChangedAsync(request.SenderId, cancellationToken);
        }

        return message;
    }

}

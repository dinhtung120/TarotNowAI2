using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

// Handler điều phối luồng tạo đề nghị cộng thêm tiền trong conversation.
public partial class RequestConversationAddMoneyCommandExecutor
    : ICommandExecutionExecutor<RequestConversationAddMoneyCommand, ConversationAddMoneyRequestResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IMediator _mediator;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler request conversation add money.
    /// Luồng xử lý: nhận repository conversation/message, mediator gửi message và publisher phát event cập nhật conversation.
    /// </summary>
    public RequestConversationAddMoneyCommandExecutor(
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        IMediator mediator,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings)
    {
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
        _mediator = mediator;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Xử lý command đề nghị cộng thêm tiền.
    /// Luồng xử lý: validate input, tải conversation, xử lý case complete-request, chặn pending offer trùng, gửi offer message và publish event.
    /// </summary>
    public async Task<ConversationAddMoneyRequestResult> Handle(
        RequestConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        var conversation = await LoadConversationAsync(request, cancellationToken);

        if (await TrySendCancelledByCompleteMessageAsync(request, conversation, cancellationToken) is { } cancelledResult)
        {
            // Nếu đang có flow complete request, chỉ gửi thông báo hủy offer và kết thúc sớm.
            return cancelledResult;
        }

        await EnsureNoPendingOfferAsync(request.ConversationId, cancellationToken);
        // Tạo payment offer message cho đề nghị cộng tiền mới.
        var message = await SendOfferMessageAsync(request, cancellationToken);

        // Phát event để UI realtime cập nhật trạng thái conversation.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(request.ConversationId, "add_money_requested", DateTime.UtcNow),
            cancellationToken);

        return new ConversationAddMoneyRequestResult { MessageId = message.Id };
    }
}

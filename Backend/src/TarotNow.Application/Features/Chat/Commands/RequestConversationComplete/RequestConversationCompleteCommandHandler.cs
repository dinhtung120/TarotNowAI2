using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

// Handler điều phối luồng yêu cầu hoàn thành conversation.
public partial class RequestConversationCompleteCommandHandler
    : IRequestHandler<RequestConversationCompleteCommand, ConversationActionResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IEscrowSettlementService _escrowSettlementService;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler request conversation complete.
    /// Luồng xử lý: nhận repository finance/conversation/message, settlement service và transaction coordinator.
    /// </summary>
    public RequestConversationCompleteCommandHandler(
        IConversationRepository conversationRepository,
        IChatFinanceRepository financeRepository,
        IEscrowSettlementService escrowSettlementService,
        ITransactionCoordinator transactionCoordinator,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _financeRepository = financeRepository;
        _escrowSettlementService = escrowSettlementService;
        _transactionCoordinator = transactionCoordinator;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command yêu cầu hoàn thành conversation.
    /// Luồng xử lý: dựng request context, xử lý first-request nếu cần, ghi xác nhận requester, nếu đủ 2 bên thì settle complete, nếu chưa thì lưu pending confirm.
    /// </summary>
    public async Task<ConversationActionResult> Handle(
        RequestConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var context = await BuildContextAsync(request, cancellationToken);
        if (IsAlreadyConfirmedByRequester(context))
        {
            // Requester đã xác nhận trước đó, trả trạng thái hiện tại để tránh tạo side-effect lặp.
            return new ConversationActionResult { Status = context.Conversation.Status };
        }

        var lastMessageAt = await HandleFirstRequestIfNeededAsync(context, cancellationToken);
        // Ghi dấu thời điểm xác nhận của phía requester.
        ApplyRequesterConfirmation(context);

        if (HasBothSidesConfirmed(context.Conversation))
        {
            // Đủ xác nhận hai bên: chốt complete + settlement ngay.
            return await CompleteConversationAsync(context, cancellationToken);
        }

        // Chưa đủ xác nhận: persist trạng thái chờ bên còn lại.
        await PersistPendingConversationAsync(context.Conversation, context.Now, lastMessageAt, cancellationToken);

        // Publish update để UI đối phương nhận thông báo yêu cầu hoàn thành.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(context.Conversation.Id, "complete_requested", context.Now),
            cancellationToken);

        return new ConversationActionResult { Status = context.Conversation.Status };
    }
}

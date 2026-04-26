using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

// Handler điều phối luồng phản hồi yêu cầu hoàn thành conversation.
public partial class RespondConversationCompleteCommandExecutor
    : ICommandExecutionExecutor<RespondConversationCompleteCommand, ConversationCompleteRespondResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IEscrowSettlementService _escrowSettlementService;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler respond conversation complete.
    /// Luồng xử lý: nhận repository conversation/finance/message, settlement service và publisher để xử lý đầy đủ nhánh accept/reject.
    /// </summary>
    public RespondConversationCompleteCommandExecutor(
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
    /// Xử lý phản hồi yêu cầu hoàn thành conversation.
    /// Luồng xử lý: dựng context hợp lệ, nếu reject thì hủy request complete; nếu accept thì ghi xác nhận responder và chốt complete khi đủ hai phía.
    /// </summary>
    public async Task<ConversationCompleteRespondResult> Handle(
        RespondConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var context = await BuildContextAsync(request, cancellationToken);
        if (request.Accept == false)
        {
            // Nhánh từ chối: xóa trạng thái confirm pending và ghi message hệ thống.
            return await RejectCompletionRequestAsync(context, cancellationToken);
        }

        // Nhánh chấp thuận: ghi mốc xác nhận của phía responder.
        ApplyResponderConfirmation(context);
        if (HasBothSidesConfirmed(context.Conversation))
        {
            // Khi đủ xác nhận hai bên thì chốt complete + settlement ngay.
            return await CompleteConversationAsync(context, cancellationToken);
        }

        context.Conversation.UpdatedAt = context.Now;
        // Persist trạng thái đã phản hồi để phía còn lại thấy tiến trình mới nhất.
        await _conversationRepository.UpdateAsync(context.Conversation, cancellationToken);

        // Publish để realtime cập nhật trạng thái phản hồi complete request.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(context.Conversation.Id, "complete_responded", context.Now),
            cancellationToken);

        return new ConversationCompleteRespondResult { Status = context.Conversation.Status, Accepted = true };
    }
}

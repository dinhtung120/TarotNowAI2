using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;

// Command để user phản hồi đề nghị cộng tiền trong conversation.
public class RespondConversationAddMoneyCommand : IRequest<ConversationAddMoneyRespondResult>
{
    // Định danh conversation chứa đề nghị cộng tiền.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh user đưa ra phản hồi.
    public Guid UserId { get; set; }

    // Cờ cho biết user chấp nhận hay từ chối đề nghị.
    public bool Accept { get; set; }

    // Định danh payment-offer message được phản hồi.
    public string OfferMessageId { get; set; } = string.Empty;

    // Lý do từ chối (chỉ dùng khi Accept = false).
    public string? RejectReason { get; set; }
}

// Handler điều phối luồng phản hồi đề nghị cộng tiền.
public partial class RespondConversationAddMoneyCommandHandler
    : IRequestHandler<RespondConversationAddMoneyCommand, ConversationAddMoneyRespondResult>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IMediator _mediator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler respond conversation add money.
    /// Luồng xử lý: nhận repository conversation/message, mediator xử lý command liên quan và publisher phát event đồng bộ conversation.
    /// </summary>
    public RespondConversationAddMoneyCommandHandler(
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        IChatFinanceRepository financeRepository,
        IWalletRepository walletRepository,
        ITransactionCoordinator transactionCoordinator,
        IMediator mediator,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
        _financeRepository = financeRepository;
        _walletRepository = walletRepository;
        _transactionCoordinator = transactionCoordinator;
        _mediator = mediator;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý phản hồi cho đề nghị cộng tiền.
    /// Luồng xử lý: kiểm tra quyền, lấy offer hợp lệ, chặn phản hồi trùng; nếu reject thì gửi payment reject, nếu accept thì freeze tiền và gửi payment accept.
    /// </summary>
    public async Task<ConversationAddMoneyRespondResult> Handle(
        RespondConversationAddMoneyCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await GetConversationAsync(request, cancellationToken);
        ValidateRespondPermission(conversation, request.UserId);
        var offer = await GetOfferMessageAsync(request, conversation, cancellationToken);
        // Rule nghiệp vụ: mỗi offer chỉ được xử lý một lần để tránh freeze trùng.
        await EnsureOfferNotHandledAsync(request.ConversationId, offer.Id, cancellationToken);

        if (request.Accept == false)
        {
            // Nhánh từ chối: chỉ phát payment reject và dừng flow.
            return await RejectOfferAsync(request, offer, cancellationToken);
        }

        var readerId = ValidateAndParseReaderId(conversation, request);
        // Nhánh chấp nhận: freeze thêm tiền theo payload offer trước khi gửi message xác nhận.
        var itemId = await FreezeOfferAsync(request, offer, readerId, cancellationToken);
        ChatMessageDto acceptMessage;
        try
        {
            acceptMessage = await SendAcceptMessageAsync(request, offer, cancellationToken);
        }
        catch
        {
            await CompensateOfferFreezeAsync(request, itemId, cancellationToken);
            throw;
        }

        // Publish để UI hai phía nhận trạng thái offer đã được phản hồi.
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(request.ConversationId, "add_money_responded", DateTime.UtcNow),
            cancellationToken);

        return new ConversationAddMoneyRespondResult
        {
            Accepted = true,
            ItemId = itemId,
            MessageId = acceptMessage.Id
        };
    }
}

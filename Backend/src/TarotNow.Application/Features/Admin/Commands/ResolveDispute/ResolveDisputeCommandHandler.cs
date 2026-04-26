using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

// Handler điều phối toàn bộ luồng xử lý tranh chấp question item.
public partial class ResolveDisputeCommandExecutor : ICommandExecutionExecutor<ResolveDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler resolve dispute.
    /// Luồng xử lý: nhận các repository tài chính/chat và transaction coordinator để xử lý atomically.
    /// </summary>
    public ResolveDisputeCommandExecutor(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        IReaderProfileRepository readerProfileRepository,
        ITransactionCoordinator transactionCoordinator,
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _readerProfileRepository = readerProfileRepository;
        _transactionCoordinator = transactionCoordinator;
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Xử lý command resolve dispute theo action release/refund/split.
    /// Luồng xử lý: validate action và split percent, dựng audit metadata, chạy toàn bộ settlement trong transaction.
    /// </summary>
    public async Task<bool> Handle(ResolveDisputeCommand request, CancellationToken cancellationToken)
    {
        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "release" && action != "refund" && action != "split")
        {
            // Rule business: chỉ hỗ trợ ba action settle dispute đã định nghĩa.
            throw new BadRequestException("Action phải là 'release', 'refund' hoặc 'split'.");
        }

        if (action == "split")
        {
            if (request.SplitPercentToReader is null or <= 0 or >= 100)
            {
                // Rule split: phần cho reader phải nằm trong khoảng mở (0,100).
                throw new BadRequestException("SplitPercentToReader phải nằm trong khoảng 1-99.");
            }
        }

        // Metadata audit đi kèm mọi giao dịch để thuận tiện truy vết quyết định xử lý của admin.
        var auditMetadata = BuildResolveAuditMetadata(request.AdminId, action, request.AdminNote);

        // Bọc toàn bộ settlement trong transaction để tránh trạng thái nửa chừng khi phát sinh lỗi.
        await _transactionCoordinator.ExecuteAsync(
            transactionCt => ResolveDisputeAsync(request, action, auditMetadata, transactionCt),
            cancellationToken);

        return true;
    }
}

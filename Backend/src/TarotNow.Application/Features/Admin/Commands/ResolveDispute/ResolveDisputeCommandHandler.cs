using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler : IRequestHandler<ResolveDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IConversationRepository _conversationRepository;
    private readonly IChatMessageRepository _chatMessageRepository;

    public ResolveDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        IReaderProfileRepository readerProfileRepository,
        ITransactionCoordinator transactionCoordinator,
        IConversationRepository conversationRepository,
        IChatMessageRepository chatMessageRepository)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _readerProfileRepository = readerProfileRepository;
        _transactionCoordinator = transactionCoordinator;
        _conversationRepository = conversationRepository;
        _chatMessageRepository = chatMessageRepository;
    }

    public async Task<bool> Handle(ResolveDisputeCommand request, CancellationToken cancellationToken)
    {
        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "release" && action != "refund" && action != "split")
        {
            throw new BadRequestException("Action phải là 'release', 'refund' hoặc 'split'.");
        }

        if (action == "split")
        {
            if (request.SplitPercentToReader is null or <= 0 or >= 100)
            {
                throw new BadRequestException("SplitPercentToReader phải nằm trong khoảng 1-99.");
            }
        }

        var auditMetadata = BuildResolveAuditMetadata(request.AdminId, action, request.AdminNote);

        await _transactionCoordinator.ExecuteAsync(
            transactionCt => ResolveDisputeAsync(request, action, auditMetadata, transactionCt),
            cancellationToken);

        return true;
    }
}

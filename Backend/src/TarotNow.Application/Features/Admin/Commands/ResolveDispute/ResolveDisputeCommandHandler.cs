using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler : IRequestHandler<ResolveDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public ResolveDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator,
        IDomainEventPublisher domainEventPublisher)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<bool> Handle(ResolveDisputeCommand request, CancellationToken cancellationToken)
    {
        var action = request.Action?.Trim().ToLowerInvariant();
        if (action != "release" && action != "refund")
        {
            throw new BadRequestException("Action phải là 'release' hoặc 'refund'.");
        }

        var auditMetadata = BuildResolveAuditMetadata(request.AdminId, action, request.AdminNote);

        await _transactionCoordinator.ExecuteAsync(
            transactionCt => ResolveDisputeAsync(request, action, auditMetadata, transactionCt),
            cancellationToken);

        return true;
    }
}

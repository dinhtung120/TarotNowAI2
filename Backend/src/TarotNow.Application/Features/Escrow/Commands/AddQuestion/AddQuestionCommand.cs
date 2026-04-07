

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

public class AddQuestionCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    
        public string ConversationRef { get; set; } = string.Empty;
    
        public long AmountDiamond { get; set; }
    
    public string? ProposalMessageRef { get; set; }
    
        public string IdempotencyKey { get; set; } = string.Empty;
}

public partial class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public AddQuestionCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<Guid> Handle(AddQuestionCommand req, CancellationToken ct)
    {
        var idempotencyKey = ValidateIdempotencyKey(req.IdempotencyKey);
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null)
        {
            return existing.Id;
        }

        return await ExecuteAddQuestionAsync(req, idempotencyKey, ct);
    }
}

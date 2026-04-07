using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

public class CreateWithdrawalCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public long AmountDiamond { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankAccountName { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
    public string MfaCode { get; set; } = string.Empty; 
}

public partial class CreateWithdrawalCommandHandler : IRequestHandler<CreateWithdrawalCommand, Guid>
{
    private readonly record struct WithdrawalPlan(long AmountVnd, long FeeVnd, long NetAmountVnd);

    private readonly IWithdrawalRepository _withdrawalRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public CreateWithdrawalCommandHandler(
        IWithdrawalRepository withdrawalRepo,
        IWalletRepository walletRepo,
        IUserRepository userRepo,
        IMfaService mfaService,
        ITransactionCoordinator transactionCoordinator)
    {
        _withdrawalRepo = withdrawalRepo;
        _walletRepo = walletRepo;
        _userRepo = userRepo;
        _mfaService = mfaService;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<Guid> Handle(CreateWithdrawalCommand req, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var user = await GetUserOrThrowAsync(req.UserId, ct);
        await ValidateRequestAsync(req, user, today, ct);

        var plan = BuildWithdrawalPlan(req.AmountDiamond);
        var createdRequest = await ExecuteWithdrawalAsync(req, today, plan, ct);
        return createdRequest.Id;
    }
}

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Constants;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;

/// <summary>
/// Command: Reader tạo yêu cầu rút tiền.
///
/// Guards (BE-1.2):
/// → Min 50 Diamond
/// → Max 1 request/ngày (business_date_utc)
/// → Reader status phải là "approved" (KYC đạt)
/// → Số dư khả dụng đủ (diamond_balance >= amount)
///
/// Fee (BE-1.3):
/// → 10% tính bằng VND → net = gross - fee
/// → Quy đổi: 1 Diamond = 1000 VND
/// → Debit diamond ngay khi tạo request (đóng băng tiền)
/// </summary>
public class CreateWithdrawalCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public long AmountDiamond { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankAccountName { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
    public string MfaCode { get; set; } = string.Empty; // Mã TOTP từ app MFA
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

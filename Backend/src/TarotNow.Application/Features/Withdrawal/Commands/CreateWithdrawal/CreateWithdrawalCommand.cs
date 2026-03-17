using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
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

public class CreateWithdrawalCommandHandler : IRequestHandler<CreateWithdrawalCommand, Guid>
{
    private readonly IWithdrawalRepository _withdrawalRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public CreateWithdrawalCommandHandler(
        IWithdrawalRepository withdrawalRepo,
        IWalletRepository walletRepo,
        IUserRepository userRepo,
        IMfaService mfaService)
    {
        _withdrawalRepo = withdrawalRepo;
        _walletRepo = walletRepo;
        _userRepo = userRepo;
        _mfaService = mfaService;
    }

    public async Task<Guid> Handle(CreateWithdrawalCommand req, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(req.UserId, ct)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        // Guard 0: MFA Verification
        if (!user.MfaEnabled || string.IsNullOrEmpty(user.MfaSecretEncrypted))
            throw new BadRequestException("Bạn phải bật bảo mật 2 lớp (MFA) trước khi rút tiền.");

        var plainSecret = _mfaService.DecryptSecret(user.MfaSecretEncrypted);
        if (!_mfaService.VerifyCode(plainSecret, req.MfaCode))
            throw new BadRequestException("Mã xác thực MFA không hợp lệ hoặc đã hết hạn.");

        // Guard 1: Min 50 Diamond
        if (req.AmountDiamond < 50)
            throw new BadRequestException("Số lượng rút tối thiểu là 50 Diamond.");

        // Guard 2: Kiểm tra user — phải là reader đã approved (KYC)
        if (user.ReaderStatus != "approved")
            throw new BadRequestException("Bạn cần hoàn tất xác minh Reader (KYC) trước khi rút tiền.");

        // Guard 3: Số dư khả dụng
        if (user.DiamondBalance < req.AmountDiamond)
            throw new BadRequestException($"Số dư không đủ. Hiện có {user.DiamondBalance}💎, cần {req.AmountDiamond}💎.");


        // Guard 4: Max 1 request/ngày
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var hasPending = await _withdrawalRepo.HasPendingRequestTodayAsync(req.UserId, today, ct);
        if (hasPending)
            throw new BadRequestException("Bạn đã có yêu cầu rút tiền hôm nay. Vui lòng thử lại ngày mai.");

        // Guard 5: Bank info validation
        if (string.IsNullOrWhiteSpace(req.BankName) ||
            string.IsNullOrWhiteSpace(req.BankAccountName) ||
            string.IsNullOrWhiteSpace(req.BankAccountNumber))
            throw new BadRequestException("Thông tin ngân hàng không hợp lệ.");

        // Fee calculation: 1 Diamond = 1000 VND, fee 10%
        var amountVnd = req.AmountDiamond * 1000;
        var feeVnd = (long)Math.Ceiling(amountVnd * 0.10);
        var netAmountVnd = amountVnd - feeVnd;

        // Debit diamond — trừ ngay khi tạo request
        await _walletRepo.DebitAsync(
            req.UserId, "diamond", "withdrawal", req.AmountDiamond,
            referenceSource: "withdrawal_request",
            description: $"Rút {req.AmountDiamond}💎 (= {netAmountVnd:N0} VND sau phí 10%)",
            cancellationToken: ct);

        // Tạo withdrawal request
        var request = new WithdrawalRequest
        {
            UserId = req.UserId,
            BusinessDateUtc = today,
            AmountDiamond = req.AmountDiamond,
            AmountVnd = amountVnd,
            FeeVnd = feeVnd,
            NetAmountVnd = netAmountVnd,
            BankName = req.BankName,
            BankAccountName = req.BankAccountName,
            BankAccountNumber = req.BankAccountNumber,
            Status = "pending",
        };

        await _withdrawalRepo.AddAsync(request, ct);
        await _withdrawalRepo.SaveChangesAsync(ct);

        return request.Id;
    }
}

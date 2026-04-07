using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

public class ProcessWithdrawalCommand : IRequest<bool>
{
    public Guid RequestId { get; set; }
    public Guid AdminId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
    public string MfaCode { get; set; } = string.Empty;
}

public class ProcessWithdrawalCommandHandler : IRequestHandler<ProcessWithdrawalCommand, bool>
{
    private const string ApproveAction = "approve";
    private const string RejectAction = "reject";
    private const string PendingStatus = "pending";
    private const string ApprovedStatus = "approved";
    private const string RejectedStatus = "rejected";
    private const string DiamondCurrency = "diamond";
    private const string WithdrawalRefundTransactionType = "withdrawal_refund";
    private const string WithdrawalReferenceSource = "withdrawal_request";

    private readonly IWithdrawalRepository _withdrawalRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    public ProcessWithdrawalCommandHandler(
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

    public async Task<bool> Handle(ProcessWithdrawalCommand req, CancellationToken ct)
    {
        ValidateAction(req.Action);

        var request = await _withdrawalRepo.GetByIdAsync(req.RequestId, ct)
            ?? throw new NotFoundException("Không tìm thấy yêu cầu rút tiền.");

        var admin = await _userRepo.GetByIdAsync(req.AdminId, ct)
            ?? throw new NotFoundException("Không tìm thấy admin.");

        
        if (!admin.MfaEnabled || string.IsNullOrEmpty(admin.MfaSecretEncrypted))
            throw new BadRequestException("Admin phải cấu hình MFA trước khi thực hiện hành động này.");

        var decSecret = _mfaService.DecryptSecret(admin.MfaSecretEncrypted);
        if (!_mfaService.VerifyCode(decSecret, req.MfaCode))
            throw new BadRequestException("Mã xác thực MFA của admin bị sai hoặc hết hạn.");

        if (request.Status != PendingStatus)
            throw new BadRequestException($"Yêu cầu ở trạng thái '{request.Status}', không thể xử lý.");

        await ProcessByActionAsync(req, request, ct);
        SetAuditFields(req, request);
        await PersistWithdrawalAsync(request, ct);
        return true;
    }

    private static void ValidateAction(string action)
    {
        if (action == ApproveAction || action == RejectAction) return;
        throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");
    }

    private async Task ProcessByActionAsync(
        ProcessWithdrawalCommand req,
        dynamic request,
        CancellationToken cancellationToken)
    {
        if (req.Action == ApproveAction)
        {
            request.Status = ApprovedStatus;
            return;
        }

        await _walletRepo.CreditAsync(
            request.UserId,
            DiamondCurrency,
            WithdrawalRefundTransactionType,
            request.AmountDiamond,
            referenceSource: WithdrawalReferenceSource,
            referenceId: request.Id.ToString(),
            description: $"Refund {request.AmountDiamond}💎 — yêu cầu rút tiền bị từ chối",
            idempotencyKey: $"wd_refund_{request.Id}",
            cancellationToken: cancellationToken);

        request.Status = RejectedStatus;
    }

    private static void SetAuditFields(ProcessWithdrawalCommand req, dynamic request)
    {
        request.AdminId = req.AdminId;
        request.AdminNote = req.AdminNote;
        request.ProcessedAt = DateTime.UtcNow;
    }

    private async Task PersistWithdrawalAsync(dynamic request, CancellationToken cancellationToken)
    {
        await _withdrawalRepo.UpdateAsync(request, cancellationToken);
        await _withdrawalRepo.SaveChangesAsync(cancellationToken);
    }
}

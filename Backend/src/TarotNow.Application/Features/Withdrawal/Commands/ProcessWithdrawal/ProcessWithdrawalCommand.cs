using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

/// <summary>
/// Command: Admin approve hoặc reject yêu cầu rút tiền.
///
/// Approve → status = approved (→ paid khi chuyển khoản xong, tùy flow).
/// Reject → status = rejected + refund diamond cho reader.
///
/// Audit trail: admin_id, admin_note, processed_at.
/// </summary>
public class ProcessWithdrawalCommand : IRequest<bool>
{
    public Guid RequestId { get; set; }
    public Guid AdminId { get; set; }
    /// <summary>"approve" | "reject"</summary>
    public string Action { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
    public string MfaCode { get; set; } = string.Empty;
}

public class ProcessWithdrawalCommandHandler : IRequestHandler<ProcessWithdrawalCommand, bool>
{
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
        if (req.Action != "approve" && req.Action != "reject")
            throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");

        var request = await _withdrawalRepo.GetByIdAsync(req.RequestId, ct)
            ?? throw new NotFoundException("Không tìm thấy yêu cầu rút tiền.");

        var admin = await _userRepo.GetByIdAsync(req.AdminId, ct)
            ?? throw new NotFoundException("Không tìm thấy admin.");

        // Guard: Admin phải có MFA và code đúng
        if (!admin.MfaEnabled || string.IsNullOrEmpty(admin.MfaSecretEncrypted))
            throw new BadRequestException("Admin phải cấu hình MFA trước khi thực hiện hành động này.");

        var decSecret = _mfaService.DecryptSecret(admin.MfaSecretEncrypted);
        if (!_mfaService.VerifyCode(decSecret, req.MfaCode))
            throw new BadRequestException("Mã xác thực MFA của admin bị sai hoặc hết hạn.");

        if (request.Status != "pending")
            throw new BadRequestException($"Yêu cầu ở trạng thái '{request.Status}', không thể xử lý.");

        var now = DateTime.UtcNow;

        if (req.Action == "approve")
        {
            // Approve — diamond đã debit khi tạo request, chỉ cần đổi status
            request.Status = "approved";
        }
        else
        {
            // Reject → refund diamond cho reader
            await _walletRepo.CreditAsync(
                request.UserId, "diamond", "withdrawal_refund", request.AmountDiamond,
                referenceSource: "withdrawal_request",
                referenceId: request.Id.ToString(),
                description: $"Refund {request.AmountDiamond}💎 — yêu cầu rút tiền bị từ chối",
                idempotencyKey: $"wd_refund_{request.Id}",
                cancellationToken: ct);

            request.Status = "rejected";
        }

        // Audit trail
        request.AdminId = req.AdminId;
        request.AdminNote = req.AdminNote;
        request.ProcessedAt = now;

        await _withdrawalRepo.UpdateAsync(request, ct);
        await _withdrawalRepo.SaveChangesAsync(ct);

        return true;
    }
}

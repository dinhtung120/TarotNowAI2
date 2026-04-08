using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;

// Command để admin duyệt hoặc từ chối yêu cầu rút tiền.
public class ProcessWithdrawalCommand : IRequest<bool>
{
    // Định danh yêu cầu rút cần xử lý.
    public Guid RequestId { get; set; }

    // Định danh admin thực hiện thao tác.
    public Guid AdminId { get; set; }

    // Hành động xử lý (approve/reject).
    public string Action { get; set; } = string.Empty;

    // Ghi chú admin khi xử lý (tùy chọn).
    public string? AdminNote { get; set; }

    // Mã MFA của admin để xác thực thao tác nhạy cảm.
    public string MfaCode { get; set; } = string.Empty;
}

// Handler xử lý duyệt/từ chối withdrawal request.
public class ProcessWithdrawalCommandHandler : IRequestHandler<ProcessWithdrawalCommand, bool>
{
    // Action duyệt yêu cầu rút.
    private const string ApproveAction = "approve";

    // Action từ chối yêu cầu rút.
    private const string RejectAction = "reject";

    // Trạng thái pending của withdrawal request.
    private const string PendingStatus = "pending";

    // Trạng thái approved của withdrawal request.
    private const string ApprovedStatus = "approved";

    // Trạng thái rejected của withdrawal request.
    private const string RejectedStatus = "rejected";

    // Currency dùng cho nghiệp vụ refund khi reject.
    private const string DiamondCurrency = "diamond";

    // Loại giao dịch ví khi hoàn trả rút tiền bị từ chối.
    private const string WithdrawalRefundTransactionType = "withdrawal_refund";

    // Reference source chung cho giao dịch liên quan withdrawal request.
    private const string WithdrawalReferenceSource = "withdrawal_request";

    private readonly IWithdrawalRepository _withdrawalRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMfaService _mfaService;

    /// <summary>
    /// Khởi tạo handler xử lý withdrawal.
    /// Luồng xử lý: nhận repository/service để tải request, xác thực admin MFA, cập nhật trạng thái và hoàn tiền khi reject.
    /// </summary>
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

    /// <summary>
    /// Xử lý command duyệt/từ chối withdrawal.
    /// Luồng xử lý: validate action, tải request/admin, xác thực MFA admin, xử lý theo action, gắn audit fields và lưu thay đổi.
    /// </summary>
    public async Task<bool> Handle(ProcessWithdrawalCommand requestCommand, CancellationToken cancellationToken)
    {
        ValidateAction(requestCommand.Action);

        var request = await _withdrawalRepo.GetByIdAsync(requestCommand.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy yêu cầu rút tiền.");

        var admin = await _userRepo.GetByIdAsync(requestCommand.AdminId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy admin.");

        if (!admin.MfaEnabled || string.IsNullOrEmpty(admin.MfaSecretEncrypted))
        {
            // Admin bắt buộc bật MFA trước khi thao tác duyệt/từ chối giao dịch tài chính.
            throw new BadRequestException("Admin phải cấu hình MFA trước khi thực hiện hành động này.");
        }

        var decSecret = _mfaService.DecryptSecret(admin.MfaSecretEncrypted);
        if (!_mfaService.VerifyCode(decSecret, requestCommand.MfaCode))
        {
            // Mã MFA admin sai/hết hạn thì chặn xử lý để bảo mật thao tác.
            throw new BadRequestException("Mã xác thực MFA của admin bị sai hoặc hết hạn.");
        }

        if (request.Status != PendingStatus)
        {
            // Chỉ request pending mới được xử lý để tránh đổi trạng thái vòng lặp.
            throw new BadRequestException($"Yêu cầu ở trạng thái '{request.Status}', không thể xử lý.");
        }

        await ProcessByActionAsync(requestCommand, request, cancellationToken);
        SetAuditFields(requestCommand, request);
        await PersistWithdrawalAsync(request, cancellationToken);
        // Lưu trạng thái cuối cùng và audit trail sau khi action đã xử lý xong.

        return true;
    }

    /// <summary>
    /// Kiểm tra action đầu vào có hợp lệ không.
    /// Luồng xử lý: chỉ chấp nhận approve/reject, giá trị khác sẽ bị chặn.
    /// </summary>
    private static void ValidateAction(string action)
    {
        if (action == ApproveAction || action == RejectAction)
        {
            return;
        }

        throw new BadRequestException("Action phải là 'approve' hoặc 'reject'.");
    }

    /// <summary>
    /// Xử lý nghiệp vụ theo action admin.
    /// Luồng xử lý: approve chỉ cập nhật trạng thái; reject thực hiện refund ví rồi cập nhật trạng thái rejected.
    /// </summary>
    private async Task ProcessByActionAsync(
        ProcessWithdrawalCommand requestCommand,
        dynamic request,
        CancellationToken cancellationToken)
    {
        if (requestCommand.Action == ApproveAction)
        {
            request.Status = ApprovedStatus;
            // Duyệt thành công thì giữ nguyên kết quả debit đã thực hiện lúc tạo request rút.
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
        // Từ chối yêu cầu thì hoàn trả toàn bộ diamond đã giữ khi tạo request rút.

        request.Status = RejectedStatus;
    }

    /// <summary>
    /// Gắn thông tin audit khi xử lý request.
    /// Luồng xử lý: lưu admin id, ghi chú và thời điểm xử lý để phục vụ đối soát.
    /// </summary>
    private static void SetAuditFields(ProcessWithdrawalCommand requestCommand, dynamic request)
    {
        request.AdminId = requestCommand.AdminId;
        request.AdminNote = requestCommand.AdminNote;
        request.ProcessedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Lưu trạng thái request sau xử lý.
    /// Luồng xử lý: update entity và save changes xuống persistence.
    /// </summary>
    private async Task PersistWithdrawalAsync(dynamic request, CancellationToken cancellationToken)
    {
        await _withdrawalRepo.UpdateAsync(request, cancellationToken);
        await _withdrawalRepo.SaveChangesAsync(cancellationToken);
    }
}

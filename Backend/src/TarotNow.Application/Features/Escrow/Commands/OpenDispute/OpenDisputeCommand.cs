

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.OpenDispute;

// Command mở tranh chấp cho một item thanh toán trong escrow.
public class OpenDisputeCommand : IRequest<bool>
{
    // Định danh item cần mở tranh chấp.
    public Guid ItemId { get; set; }

    // Định danh người gửi tranh chấp.
    public Guid UserId { get; set; }

    // Lý do tranh chấp do người dùng cung cấp.
    public string Reason { get; set; } = string.Empty;
}

// Handler xử lý mở tranh chấp.
public class OpenDisputeCommandHandler : IRequestHandler<OpenDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler open dispute.
    /// Luồng xử lý: nhận finance repository và transaction coordinator để cập nhật item/session an toàn trong transaction.
    /// </summary>
    public OpenDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        ITransactionCoordinator transactionCoordinator,
        ISystemConfigSettings systemConfigSettings)
    {
        _financeRepo = financeRepo;
        _transactionCoordinator = transactionCoordinator;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Xử lý command mở tranh chấp.
    /// Luồng xử lý: khóa item, kiểm tra quyền + điều kiện trạng thái + lý do, chuyển item sang Disputed, cập nhật session và lưu thay đổi.
    /// </summary>
    public async Task<bool> Handle(OpenDisputeCommand req, CancellationToken ct)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            EnsureUserCanOpenDispute(item, req.UserId);
            if (item.Status == QuestionItemStatus.Disputed)
            {
                // Idempotent: item đã ở trạng thái tranh chấp thì bỏ qua cập nhật lặp.
                return;
            }

            EnsureItemCanOpenDispute(item.Status);
            EnsureReasonIsValid(req.Reason);

            var now = DateTime.UtcNow;
            var disputeWindowHours = Math.Max(1, _systemConfigSettings.EscrowDisputeWindowHours);
            item.Status = QuestionItemStatus.Disputed;
            item.AutoReleaseAt = null;
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.AddHours(disputeWindowHours);
            item.DisputeReason = req.Reason.Trim();
            item.UpdatedAt = now;

            await _financeRepo.UpdateItemAsync(item, transactionCt);
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                // Đồng bộ session sang trạng thái disputed để luồng xử lý sau nhận biết tranh chấp.
                await UpdateSessionStatusAsync(session, transactionCt);
            }

            await _financeRepo.SaveChangesAsync(transactionCt);
        }, ct);

        return true;
    }

    /// <summary>
    /// Kiểm tra user có quyền mở tranh chấp cho item hay không.
    /// Luồng xử lý: chỉ cho phép payer hoặc receiver của item mở tranh chấp.
    /// </summary>
    private static void EnsureUserCanOpenDispute(ChatQuestionItem item, Guid userId)
    {
        if (item.PayerId == userId || item.ReceiverId == userId)
        {
            return;
        }

        throw new BadRequestException("Bạn không có quyền mở tranh chấp cho mục thanh toán này.");
    }

    /// <summary>
    /// Kiểm tra trạng thái item có được phép mở tranh chấp hay không.
    /// Luồng xử lý: chỉ item Accepted mới được chuyển sang Disputed.
    /// </summary>
    private static void EnsureItemCanOpenDispute(string status)
    {
        if (status == QuestionItemStatus.Accepted)
        {
            return;
        }

        throw new BadRequestException($"Câu hỏi ở trạng thái {status}, không thể mở tranh chấp.");
    }

    /// <summary>
    /// Kiểm tra lý do tranh chấp hợp lệ.
    /// Luồng xử lý: bắt buộc reason có nội dung và đạt độ dài tối thiểu.
    /// </summary>
    private void EnsureReasonIsValid(string reason)
    {
        var minReasonLength = Math.Max(1, _systemConfigSettings.EscrowDisputeMinReasonLength);
        if (!string.IsNullOrWhiteSpace(reason) && reason.Trim().Length >= minReasonLength)
        {
            return;
        }

        throw new BadRequestException($"Lý do tranh chấp phải có ít nhất {minReasonLength} ký tự.");
    }

    /// <summary>
    /// Cập nhật trạng thái session sang disputed.
    /// Luồng xử lý: gán status mới và persist session qua repository.
    /// </summary>
    private async Task UpdateSessionStatusAsync(ChatFinanceSession session, CancellationToken cancellationToken)
    {
        session.Status = ChatFinanceSessionStatus.Disputed;
        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }
}

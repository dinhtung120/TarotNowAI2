

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
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
    // Độ dài tối thiểu của lý do tranh chấp.
    private const int MinReasonLength = 10;

    // Khoảng thời gian cửa sổ tranh chấp kể từ lúc mở.
    private static readonly TimeSpan DisputeWindowDuration = TimeSpan.FromHours(48);

    // Trạng thái session khi có tranh chấp.
    private const string SessionDisputedStatus = "disputed";

    private readonly IChatFinanceRepository _financeRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo handler open dispute.
    /// Luồng xử lý: nhận finance repository và transaction coordinator để cập nhật item/session an toàn trong transaction.
    /// </summary>
    public OpenDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _transactionCoordinator = transactionCoordinator;
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
            item.Status = QuestionItemStatus.Disputed;
            item.AutoReleaseAt = null;
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.Add(DisputeWindowDuration);
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
    private static void EnsureUserCanOpenDispute(dynamic item, Guid userId)
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
    private static void EnsureReasonIsValid(string reason)
    {
        if (!string.IsNullOrWhiteSpace(reason) && reason.Length >= MinReasonLength)
        {
            return;
        }

        throw new BadRequestException($"Lý do tranh chấp phải có ít nhất {MinReasonLength} ký tự.");
    }

    /// <summary>
    /// Cập nhật trạng thái session sang disputed.
    /// Luồng xử lý: gán status mới và persist session qua repository.
    /// </summary>
    private async Task UpdateSessionStatusAsync(dynamic session, CancellationToken cancellationToken)
    {
        session.Status = SessionDisputedStatus;
        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
    }
}

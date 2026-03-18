using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.OpenDispute;

/// <summary>
/// Command: User mở tranh chấp — validate dispute window.
///
/// Business rules:
/// → Chỉ mở dispute khi item đang accepted (tiền còn frozen).
/// → Reader phải đã reply, và còn trong cửa sổ xử lý trước auto-release.
/// → Khi dispute → dừng auto-release để admin xử lý.
/// </summary>
public class OpenDisputeCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class OpenDisputeCommandHandler : IRequestHandler<OpenDisputeCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public OpenDisputeCommandHandler(
        IChatFinanceRepository financeRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(OpenDisputeCommand req, CancellationToken ct)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            // Chỉ payer mới mở dispute
            if (item.PayerId != req.UserId)
                throw new BadRequestException("Chỉ người đặt câu hỏi mới được mở tranh chấp.");

            // Validate status — chỉ accepted items (vẫn đang giữ frozen)
            if (item.Status != QuestionItemStatus.Accepted)
                throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể mở tranh chấp.");

            if (item.RepliedAt == null)
                throw new BadRequestException("Reader chưa trả lời, chưa thể mở tranh chấp.");

            // Validate dispute window trước khi auto-release
            var now = DateTime.UtcNow;
            if (item.AutoReleaseAt != null && now > item.AutoReleaseAt)
                throw new BadRequestException("Đã quá thời hạn mở tranh chấp.");

            if (string.IsNullOrWhiteSpace(req.Reason) || req.Reason.Length < 10)
                throw new BadRequestException("Lý do tranh chấp phải có ít nhất 10 ký tự.");

            // Chuyển status → disputed
            item.Status = QuestionItemStatus.Disputed;
            item.AutoReleaseAt = null;
            item.UpdatedAt = now;

            await _financeRepo.UpdateItemAsync(item, transactionCt);

            // Cập nhật session status
            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.Status = "disputed";
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            await _financeRepo.SaveChangesAsync(transactionCt);
        }, ct);

        return true;
    }
}

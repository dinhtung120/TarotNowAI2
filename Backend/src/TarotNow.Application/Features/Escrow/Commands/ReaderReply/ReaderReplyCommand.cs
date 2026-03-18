using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.ReaderReply;

/// <summary>
/// Command: Reader reply → set replied_at + auto_release_at = +24h.
/// Sau 24h nếu user không dispute → auto-release.
/// </summary>
public class ReaderReplyCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    public Guid ReaderId { get; set; }
}

public class ReaderReplyCommandHandler : IRequestHandler<ReaderReplyCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly ITransactionCoordinator _transactionCoordinator;

    public ReaderReplyCommandHandler(
        IChatFinanceRepository financeRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<bool> Handle(ReaderReplyCommand req, CancellationToken ct)
    {
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemForUpdateAsync(req.ItemId, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

            // Chỉ reader nhận mới được reply
            if (item.ReceiverId != req.ReaderId)
                throw new BadRequestException("Bạn không phải reader của câu hỏi này.");

            // Chỉ accepted items mới reply được
            if (item.Status != QuestionItemStatus.Accepted)
                throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể reply.");

            // Đã reply rồi
            if (item.RepliedAt != null)
                throw new BadRequestException("Câu hỏi đã được trả lời.");

            var now = DateTime.UtcNow;
            item.RepliedAt = now;
            item.AutoReleaseAt = now.AddHours(24);
            // Xóa auto_refund timer vì đã reply
            item.AutoRefundAt = null;

            await _financeRepo.UpdateItemAsync(item, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);
        }, ct);

        return true;
    }
}

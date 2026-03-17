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

    public ReaderReplyCommandHandler(IChatFinanceRepository financeRepo)
    {
        _financeRepo = financeRepo;
    }

    public async Task<bool> Handle(ReaderReplyCommand req, CancellationToken ct)
    {
        var item = await _financeRepo.GetItemByIdAsync(req.ItemId, ct)
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

        await _financeRepo.UpdateItemAsync(item, ct);
        await _financeRepo.SaveChangesAsync(ct);
        return true;
    }
}

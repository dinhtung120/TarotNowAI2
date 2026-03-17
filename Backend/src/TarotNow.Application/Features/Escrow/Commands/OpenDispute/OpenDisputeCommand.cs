using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.OpenDispute;

/// <summary>
/// Command: User mở tranh chấp — validate dispute window.
///
/// Business rules:
/// → Chỉ mở dispute trong dispute_window (24h từ release/refund).
/// → Item phải ở status = released (hoặc refunded — tùy policy).
/// → Khi dispute → freeze lại tiền (nếu đã release) — admin quyết định.
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

    public OpenDisputeCommandHandler(IChatFinanceRepository financeRepo)
    {
        _financeRepo = financeRepo;
    }

    public async Task<bool> Handle(OpenDisputeCommand req, CancellationToken ct)
    {
        var item = await _financeRepo.GetItemByIdAsync(req.ItemId, ct)
            ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

        // Chỉ payer mới mở dispute
        if (item.PayerId != req.UserId)
            throw new BadRequestException("Chỉ người đặt câu hỏi mới được mở tranh chấp.");

        // Validate status — chỉ released items
        if (item.Status != QuestionItemStatus.Released)
            throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể mở tranh chấp.");

        // Validate dispute window
        var now = DateTime.UtcNow;
        if (item.DisputeWindowEnd == null || now > item.DisputeWindowEnd)
            throw new BadRequestException("Đã hết thời hạn mở tranh chấp (24 giờ sau release).");

        if (string.IsNullOrWhiteSpace(req.Reason) || req.Reason.Length < 10)
            throw new BadRequestException("Lý do tranh chấp phải có ít nhất 10 ký tự.");

        // Chuyển status → disputed
        item.Status = QuestionItemStatus.Disputed;

        await _financeRepo.UpdateItemAsync(item, ct);

        // Cập nhật session status
        var session = await _financeRepo.GetSessionByIdAsync(item.FinanceSessionId, ct);
        if (session != null)
        {
            session.Status = "disputed";
            await _financeRepo.UpdateSessionAsync(session, ct);
        }

        await _financeRepo.SaveChangesAsync(ct);
        return true;
    }
}

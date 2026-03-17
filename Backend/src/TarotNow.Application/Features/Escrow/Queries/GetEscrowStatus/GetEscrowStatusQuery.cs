using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Escrow.Queries.GetEscrowStatus;

/// <summary>
/// Query lấy trạng thái escrow của conversation.
/// Trả về finance session + tất cả question items.
/// </summary>
public class GetEscrowStatusQuery : IRequest<EscrowStatusResult?>
{
    public string ConversationRef { get; set; } = string.Empty;
    public Guid RequesterId { get; set; }
}

public class EscrowStatusResult
{
    public Guid SessionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public long TotalFrozen { get; set; }
    public List<QuestionItemResult> Items { get; set; } = new();
}

public class QuestionItemResult
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public long AmountDiamond { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? AcceptedAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public DateTime? AutoRefundAt { get; set; }
    public DateTime? AutoReleaseAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public DateTime? DisputeWindowEnd { get; set; }
}

public class GetEscrowStatusQueryHandler : IRequestHandler<GetEscrowStatusQuery, EscrowStatusResult?>
{
    private readonly IChatFinanceRepository _financeRepo;

    public GetEscrowStatusQueryHandler(IChatFinanceRepository financeRepo) => _financeRepo = financeRepo;

    public async Task<EscrowStatusResult?> Handle(GetEscrowStatusQuery req, CancellationToken ct)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, ct);
        if (session == null) return null;

        // Kiểm tra quyền
        if (session.UserId != req.RequesterId && session.ReaderId != req.RequesterId)
            return null;

        var items = await _financeRepo.GetItemsBySessionIdAsync(session.Id, ct);

        return new EscrowStatusResult
        {
            SessionId = session.Id,
            Status = session.Status,
            TotalFrozen = session.TotalFrozen,
            Items = items.Select(i => new QuestionItemResult
            {
                Id = i.Id,
                Type = i.Type,
                AmountDiamond = i.AmountDiamond,
                Status = i.Status,
                AcceptedAt = i.AcceptedAt,
                RepliedAt = i.RepliedAt,
                AutoRefundAt = i.AutoRefundAt,
                AutoReleaseAt = i.AutoReleaseAt,
                ReleasedAt = i.ReleasedAt,
                DisputeWindowEnd = i.DisputeWindowEnd,
            }).ToList()
        };
    }
}

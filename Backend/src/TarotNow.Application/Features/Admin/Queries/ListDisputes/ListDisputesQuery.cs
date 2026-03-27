using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDisputes;

public class ListDisputesQuery : IRequest<ListDisputesResult>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}

public class ListDisputesResult
{
    public IReadOnlyList<DisputeItemDto> Items { get; set; } = Array.Empty<DisputeItemDto>();

    public long TotalCount { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}

public class DisputeItemDto
{
    public Guid Id { get; set; }

    public Guid FinanceSessionId { get; set; }

    public Guid PayerId { get; set; }

    public Guid ReceiverId { get; set; }

    public long AmountDiamond { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class ListDisputesQueryHandler : IRequestHandler<ListDisputesQuery, ListDisputesResult>
{
    private readonly IChatFinanceRepository _financeRepository;

    public ListDisputesQueryHandler(IChatFinanceRepository financeRepository)
    {
        _financeRepository = financeRepository;
    }

    public async Task<ListDisputesResult> Handle(ListDisputesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _financeRepository.GetDisputedItemsPaginatedAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        return new ListDisputesResult
        {
            Items = items.Select(item => new DisputeItemDto
            {
                Id = item.Id,
                FinanceSessionId = item.FinanceSessionId,
                PayerId = item.PayerId,
                ReceiverId = item.ReceiverId,
                AmountDiamond = item.AmountDiamond,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}

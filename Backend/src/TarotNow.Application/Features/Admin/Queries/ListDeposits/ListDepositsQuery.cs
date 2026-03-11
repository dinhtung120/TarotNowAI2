using MediatR;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

public class ListDepositsQuery : IRequest<ListDepositsResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Status { get; set; }
}

public class ListDepositsResponse
{
    public IEnumerable<DepositDto> Deposits { get; set; } = new List<DepositDto>();
    public int TotalCount { get; set; }
}

public class DepositDto
{
    public System.Guid Id { get; set; }
    public System.Guid UserId { get; set; }
    public long AmountVnd { get; set; }
    public long DiamondAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public System.DateTime CreatedAt { get; set; }
}

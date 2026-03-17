using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals;

/// <summary>
/// Query: Lấy danh sách yêu cầu rút tiền.
/// Dùng cho cả reader (my-list) và admin (pending queue).
/// </summary>
public class ListWithdrawalsQuery : IRequest<List<WithdrawalResult>>
{
    /// <summary>Nếu có → lấy theo user. Nếu null → admin lấy pending.</summary>
    public Guid? UserId { get; set; }
    public bool PendingOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class WithdrawalResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public long AmountDiamond { get; set; }
    public long AmountVnd { get; set; }
    public long FeeVnd { get; set; }
    public long NetAmountVnd { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankAccountName { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ListWithdrawalsQueryHandler : IRequestHandler<ListWithdrawalsQuery, List<WithdrawalResult>>
{
    private readonly IWithdrawalRepository _repo;

    public ListWithdrawalsQueryHandler(IWithdrawalRepository repo) => _repo = repo;

    public async Task<List<WithdrawalResult>> Handle(ListWithdrawalsQuery req, CancellationToken ct)
    {
        List<WithdrawalRequest> items;

        if (req.PendingOnly)
        {
            items = await _repo.ListPendingAsync(req.Page, req.PageSize, ct);
        }
        else if (req.UserId.HasValue)
        {
            items = await _repo.ListByUserAsync(req.UserId.Value, req.Page, req.PageSize, ct);
        }
        else
        {
            items = await _repo.ListPendingAsync(req.Page, req.PageSize, ct);
        }

        return items.Select(r => new WithdrawalResult
        {
            Id = r.Id,
            UserId = r.UserId,
            AmountDiamond = r.AmountDiamond,
            AmountVnd = r.AmountVnd,
            FeeVnd = r.FeeVnd,
            NetAmountVnd = r.NetAmountVnd,
            BankName = r.BankName,
            BankAccountName = r.BankAccountName,
            BankAccountNumber = r.BankAccountNumber,
            Status = r.Status,
            AdminNote = r.AdminNote,
            ProcessedAt = r.ProcessedAt,
            CreatedAt = r.CreatedAt,
        }).ToList();
    }
}

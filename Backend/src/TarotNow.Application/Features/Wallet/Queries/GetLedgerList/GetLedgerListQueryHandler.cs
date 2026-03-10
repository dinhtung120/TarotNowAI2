using MediatR;
using TarotNow.Application.Common.Models;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Application.Features.Wallet.Queries.GetLedgerList;

public class GetLedgerListQueryHandler : IRequestHandler<GetLedgerListQuery, PaginatedList<WalletTransactionDto>>
{
    private readonly ILedgerRepository _ledgerRepository;

    public GetLedgerListQueryHandler(ILedgerRepository ledgerRepository)
    {
        _ledgerRepository = ledgerRepository;
    }

    public async Task<PaginatedList<WalletTransactionDto>> Handle(GetLedgerListQuery request, CancellationToken cancellationToken)
    {
        // 1. Lấy tổng số lượng để tính phân trang
        var totalCount = await _ledgerRepository.GetTotalCountAsync(request.UserId, cancellationToken);

        // 2. Chặn truy vấn nếu số lượng = 0
        if (totalCount == 0)
        {
            return new PaginatedList<WalletTransactionDto>(new List<WalletTransactionDto>(), 0, request.Page, request.Limit);
        }

        // 3. Lấy dữ liệu
        var transactions = await _ledgerRepository.GetTransactionsAsync(request.UserId, request.Page, request.Limit, cancellationToken);

        // 4. Map Entity sang Dto
        var dtos = transactions.Select(t => new WalletTransactionDto
        {
            Id = t.Id,
            Currency = t.Currency.ToString(),
            Type = t.Type.ToString(),
            Amount = t.Amount,
            BalanceBefore = t.BalanceBefore,
            BalanceAfter = t.BalanceAfter,
            Description = t.Description,
            CreatedAt = t.CreatedAt
        }).ToList();

        // 5. Trả về PaginatedList
        return new PaginatedList<WalletTransactionDto>(dtos, totalCount, request.Page, request.Limit);
    }
}

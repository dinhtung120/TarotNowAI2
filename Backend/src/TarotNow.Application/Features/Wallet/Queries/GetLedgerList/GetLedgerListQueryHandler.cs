

using MediatR;
using TarotNow.Application.Common.Models;
using TarotNow.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        
        var totalCount = await _ledgerRepository.GetTotalCountAsync(request.UserId, cancellationToken);

        
        if (totalCount == 0)
        {
            return new PaginatedList<WalletTransactionDto>(new List<WalletTransactionDto>(), 0, request.Page, request.Limit);
        }

        
        var transactions = await _ledgerRepository.GetTransactionsAsync(request.UserId, request.Page, request.Limit, cancellationToken);

        
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

        
        return new PaginatedList<WalletTransactionDto>(dtos, totalCount, request.Page, request.Limit);
    }
}

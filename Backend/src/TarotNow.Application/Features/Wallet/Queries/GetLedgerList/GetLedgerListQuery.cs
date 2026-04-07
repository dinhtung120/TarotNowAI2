

using MediatR;
using TarotNow.Application.Common.Models;
using System;

namespace TarotNow.Application.Features.Wallet.Queries.GetLedgerList;

public class GetLedgerListQuery : IRequest<PaginatedList<WalletTransactionDto>>
{
    public Guid UserId { get; set; }
    
        public int Page { get; set; } = 1;
    
        public int Limit { get; set; } = 20;
    
    public GetLedgerListQuery(Guid userId, int page = 1, int limit = 20)
    {
        UserId = userId;
        
        Page = page < 1 ? 1 : page;
        
        Limit = limit < 1 ? 20 : limit > 100 ? 100 : limit;
    }
}

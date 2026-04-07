

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface ILedgerRepository
{
        Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default);
    
        Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default);
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IDailyCheckinRepository
{
        Task<bool> HasCheckedInAsync(string userId, string businessDate, CancellationToken cancellationToken = default);

        Task InsertAsync(string userId, string businessDate, long goldReward, CancellationToken cancellationToken = default);
    
        Task<int> GetCheckinCountAsync(string userId, int recentDays, CancellationToken cancellationToken = default);
}

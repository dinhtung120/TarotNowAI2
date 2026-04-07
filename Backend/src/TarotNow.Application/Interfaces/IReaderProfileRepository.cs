

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IReaderProfileRepository
{
        Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

        Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<ReaderProfileDto>> GetByUserIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);

        Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

        Task DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize,
        string? specialty = null, string? status = null, string? searchTerm = null,
        CancellationToken cancellationToken = default);
}

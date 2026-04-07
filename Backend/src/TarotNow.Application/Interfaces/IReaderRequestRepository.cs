

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IReaderRequestRepository
{
        Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);

        Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default);

        Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);
}

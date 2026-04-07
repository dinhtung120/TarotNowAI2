

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IReportRepository
{
        Task AddAsync(ReportDto report, CancellationToken cancellationToken = default);

        Task<ReportDto?> GetByIdAsync(string reportId, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null, string? targetType = null,
        CancellationToken cancellationToken = default);

        Task<bool> ResolveAsync(
        string reportId,
        string status,
        string result,
        string resolvedBy,
        string? adminNote,
        CancellationToken cancellationToken = default);
}

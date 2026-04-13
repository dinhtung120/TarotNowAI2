using TarotNow.Application.Features.Admin.Queries.GetOutboxDashboard;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract đọc dữ liệu vận hành outbox cho dashboard admin.
/// </summary>
public interface IOutboxMonitoringRepository
{
    /// <summary>
    /// Lấy snapshot dashboard outbox tại thời điểm hiện tại.
    /// </summary>
    Task<OutboxDashboardDto> GetDashboardAsync(
        int top,
        DateTime nowUtc,
        CancellationToken cancellationToken = default);
}

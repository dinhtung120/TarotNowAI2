using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.GetOutboxDashboard;

/// <summary>
/// Handler truy vấn dashboard vận hành outbox.
/// </summary>
public sealed class GetOutboxDashboardQueryHandler : IRequestHandler<GetOutboxDashboardQuery, OutboxDashboardDto>
{
    private const int DefaultTop = 20;
    private const int MaxTop = 100;

    private readonly IOutboxMonitoringRepository _outboxMonitoringRepository;

    /// <summary>
    /// Khởi tạo query handler dashboard outbox.
    /// </summary>
    public GetOutboxDashboardQueryHandler(IOutboxMonitoringRepository outboxMonitoringRepository)
    {
        _outboxMonitoringRepository = outboxMonitoringRepository;
    }

    /// <inheritdoc />
    public Task<OutboxDashboardDto> Handle(GetOutboxDashboardQuery request, CancellationToken cancellationToken)
    {
        var top = NormalizeTop(request.Top);
        return _outboxMonitoringRepository.GetDashboardAsync(top, DateTime.UtcNow, cancellationToken);
    }

    private static int NormalizeTop(int top)
    {
        if (top <= 0)
        {
            return DefaultTop;
        }

        return top > MaxTop ? MaxTop : top;
    }
}

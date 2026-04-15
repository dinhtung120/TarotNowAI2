using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

// Handler khởi tạo reading session theo Rule 0: chỉ publish domain event.
public partial class InitReadingSessionCommandHandler : IRequestHandler<InitReadingSessionCommand, InitReadingSessionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler init reading session.
    /// Luồng xử lý: nhận inline dispatcher để phát domain event và để handler chuyên trách xử lý side-effects.
    /// </summary>
    public InitReadingSessionCommandHandler(
        IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command khởi tạo reading session.
    /// Luồng xử lý: chỉ publish domain event init-requested và trả dữ liệu đã được handler xử lý.
    /// </summary>
    public async Task<InitReadingSessionResult> Handle(
        InitReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var domainEvent = new ReadingSessionInitRequestedDomainEvent
        {
            UserId = request.UserId,
            SpreadType = request.SpreadType,
            Question = request.Question,
            Currency = request.Currency
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);

        return new InitReadingSessionResult
        {
            SessionId = domainEvent.SessionId,
            CostGold = domainEvent.CostGold,
            CostDiamond = domainEvent.CostDiamond
        };
    }
}

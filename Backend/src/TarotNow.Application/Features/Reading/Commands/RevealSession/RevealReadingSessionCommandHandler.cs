using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

// Handler reveal reading session theo Rule 0: chỉ publish domain event.
public partial class RevealReadingSessionCommandHandler : IRequestHandler<RevealReadingSessionCommand, RevealReadingSessionResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler reveal reading session.
    /// Luồng xử lý: nhận inline dispatcher để phát domain event reveal-requested.
    /// </summary>
    public RevealReadingSessionCommandHandler(
        IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command reveal session.
    /// Luồng xử lý: chỉ publish domain event reveal-requested và trả snapshot từ event sau khi handler hoàn tất.
    /// </summary>
    public async Task<RevealReadingSessionResult> Handle(
        RevealReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var domainEvent = new ReadingSessionRevealRequestedDomainEvent
        {
            UserId = request.UserId,
            SessionId = request.SessionId,
            Language = NormalizeLanguage(request.Language)
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);

        return new RevealReadingSessionResult
        {
            Cards = domainEvent.RevealedCards.ToArray()
        };
    }

    private static string NormalizeLanguage(string? language)
    {
        return language?.Trim().ToLowerInvariant() switch
        {
            "en" => "en",
            "zh" => "zh",
            _ => "vi"
        };
    }
}

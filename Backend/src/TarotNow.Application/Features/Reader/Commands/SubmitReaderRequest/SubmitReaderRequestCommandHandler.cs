using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// Handler gửi đơn Reader theo Rule 0: chỉ publish domain event.
/// </summary>
public sealed class SubmitReaderRequestCommandHandler : IRequestHandler<SubmitReaderRequestCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler gửi đơn Reader.
    /// </summary>
    public SubmitReaderRequestCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command bằng cách publish domain event và trả kết quả hydrate từ handler.
    /// </summary>
    public async Task<bool> Handle(SubmitReaderRequestCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ReaderRequestSubmitRequestedDomainEvent
        {
            UserId = request.UserId,
            Bio = request.Bio,
            Specialties = request.Specialties,
            YearsOfExperience = request.YearsOfExperience,
            FacebookUrl = request.FacebookUrl,
            InstagramUrl = request.InstagramUrl,
            TikTokUrl = request.TikTokUrl,
            DiamondPerQuestion = request.DiamondPerQuestion,
            ProofDocuments = request.ProofDocuments
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Submitted;
    }
}

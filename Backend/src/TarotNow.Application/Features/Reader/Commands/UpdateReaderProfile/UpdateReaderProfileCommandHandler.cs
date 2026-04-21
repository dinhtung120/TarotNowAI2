using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

/// <summary>
/// Handler cập nhật hồ sơ Reader theo Rule 0: chỉ publish domain event.
/// </summary>
public sealed class UpdateReaderProfileCommandHandler : IRequestHandler<UpdateReaderProfileCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler cập nhật hồ sơ Reader.
    /// </summary>
    public UpdateReaderProfileCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command bằng cách publish domain event và trả kết quả đã hydrate.
    /// </summary>
    public async Task<bool> Handle(UpdateReaderProfileCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ReaderProfileUpdateRequestedDomainEvent
        {
            UserId = request.UserId,
            BioVi = request.BioVi,
            BioEn = request.BioEn,
            BioZh = request.BioZh,
            DiamondPerQuestion = request.DiamondPerQuestion,
            Specialties = request.Specialties,
            YearsOfExperience = request.YearsOfExperience,
            FacebookUrl = request.FacebookUrl,
            InstagramUrl = request.InstagramUrl,
            TikTokUrl = request.TikTokUrl
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Updated;
    }
}

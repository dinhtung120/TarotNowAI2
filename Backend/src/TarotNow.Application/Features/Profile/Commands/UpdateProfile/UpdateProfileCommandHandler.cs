using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

// Handler cập nhật profile theo Rule 0: chỉ publish domain event.
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler cập nhật profile.
    /// Luồng xử lý: nhận dispatcher để publish domain event và tách side-effect sang event handlers.
    /// </summary>
    public UpdateProfileCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command cập nhật profile.
    /// Luồng xử lý: publish domain event và trả kết quả đã hydrate từ event handler.
    /// </summary>
    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new UserProfileUpdateRequestedDomainEvent
        {
            UserId = request.UserId,
            DisplayName = request.DisplayName,
            DateOfBirth = request.DateOfBirth,
            PayoutBankName = request.PayoutBankName,
            PayoutBankBin = request.PayoutBankBin,
            PayoutBankAccountNumber = request.PayoutBankAccountNumber,
            PayoutBankAccountHolder = request.PayoutBankAccountHolder
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Updated;
    }
}

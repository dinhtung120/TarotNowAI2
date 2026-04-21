using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

/// <summary>
/// Handler cập nhật trạng thái Reader theo Rule 0: chỉ publish domain event.
/// </summary>
public sealed class UpdateReaderStatusCommandHandler : IRequestHandler<UpdateReaderStatusCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler cập nhật trạng thái Reader.
    /// </summary>
    public UpdateReaderStatusCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command bằng cách publish domain event và trả kết quả đã hydrate.
    /// </summary>
    public async Task<bool> Handle(UpdateReaderStatusCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ReaderStatusUpdateRequestedDomainEvent
        {
            UserId = request.UserId,
            Status = request.Status
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Updated;
    }
}

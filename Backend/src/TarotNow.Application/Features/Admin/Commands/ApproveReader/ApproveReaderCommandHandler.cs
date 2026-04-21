using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

/// <summary>
/// Handler xử lý duyệt/từ chối Reader theo Rule 0: chỉ publish domain event.
/// </summary>
public sealed class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
{
    private const string ApproveAction = "approve";
    private const string RejectAction = "reject";
    private const string InvalidActionMessage = "Action không hợp lệ. Chỉ chấp nhận: approve, reject.";

    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler duyệt Reader.
    /// </summary>
    public ApproveReaderCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command bằng cách publish domain event và trả kết quả xử lý.
    /// </summary>
    public async Task<bool> Handle(ApproveReaderCommand request, CancellationToken cancellationToken)
    {
        var action = ValidateAndNormalizeAction(request.Action);
        var domainEvent = new ReaderRequestReviewRequestedDomainEvent
        {
            RequestId = request.RequestId,
            Action = action,
            AdminNote = request.AdminNote,
            AdminId = request.AdminId
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Processed;
    }

    /// <summary>
    /// Chuẩn hóa action và chặn giá trị ngoài approve/reject.
    /// </summary>
    private static string ValidateAndNormalizeAction(string action)
    {
        var normalizedAction = action.Trim().ToLowerInvariant();
        if (normalizedAction is ApproveAction or RejectAction)
        {
            return normalizedAction;
        }

        throw new BadRequestException(InvalidActionMessage);
    }
}

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.UpdateCallStatus;

public class UpdateCallStatusCommand : IRequest<bool>
{
    public string CallSessionId { get; set; } = string.Empty;
    public string NewStatus { get; set; } = "ended";
    public string? ExpectedPreviousStatus { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? EndReason { get; set; }
}

public class UpdateCallStatusCommandHandler : IRequestHandler<UpdateCallStatusCommand, bool>
{
    private readonly ICallSessionRepository _callSessionRepository;

    public UpdateCallStatusCommandHandler(ICallSessionRepository callSessionRepository)
    {
        _callSessionRepository = callSessionRepository;
    }

    public async Task<bool> Handle(UpdateCallStatusCommand request, CancellationToken cancellationToken)
    {
        var newStatus = ParseStatusOrThrow(request.NewStatus, nameof(request.NewStatus));
        CallSessionStatus? expectedPreviousStatus = null;

        if (string.IsNullOrWhiteSpace(request.ExpectedPreviousStatus) == false)
        {
            expectedPreviousStatus = ParseStatusOrThrow(
                request.ExpectedPreviousStatus!,
                nameof(request.ExpectedPreviousStatus));
        }

        return await _callSessionRepository.UpdateStatusAsync(
            request.CallSessionId,
            newStatus,
            startedAt: request.StartedAt,
            endedAt: request.EndedAt,
            endReason: request.EndReason,
            expectedPreviousStatus: expectedPreviousStatus,
            ct: cancellationToken);
    }

    private static CallSessionStatus ParseStatusOrThrow(string rawStatus, string fieldName)
    {
        var value = rawStatus.Trim().ToLowerInvariant();
        return value switch
        {
            "requested" => CallSessionStatus.Requested,
            "accepted" => CallSessionStatus.Accepted,
            "rejected" => CallSessionStatus.Rejected,
            "ended" => CallSessionStatus.Ended,
            _ => throw new BadRequestException($"{fieldName} không hợp lệ.")
        };
    }
}

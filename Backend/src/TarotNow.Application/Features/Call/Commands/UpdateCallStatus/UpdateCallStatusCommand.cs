using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.UpdateCallStatus;

// Command cập nhật trạng thái phiên gọi (dùng cho luồng hệ thống/realtime).
public class UpdateCallStatusCommand : IRequest<bool>
{
    // Định danh phiên gọi cần cập nhật.
    public string CallSessionId { get; set; } = string.Empty;
    // Trạng thái mới mong muốn.
    public string NewStatus { get; set; } = "ended";
    // Trạng thái kỳ vọng trước cập nhật để hỗ trợ optimistic concurrency.
    public string? ExpectedPreviousStatus { get; set; }
    // Thời điểm bắt đầu cuộc gọi (nếu cập nhật sang accepted).
    public DateTime? StartedAt { get; set; }
    // Thời điểm kết thúc cuộc gọi (nếu cập nhật sang ended).
    public DateTime? EndedAt { get; set; }
    // Lý do kết thúc cuộc gọi.
    public string? EndReason { get; set; }
}

// Handler cập nhật trạng thái call session theo command.
public class UpdateCallStatusCommandHandler : IRequestHandler<UpdateCallStatusCommand, bool>
{
    private readonly ICallSessionRepository _callSessionRepository;

    /// <summary>
    /// Khởi tạo handler update call status.
    /// Luồng xử lý: nhận call session repository để ghi thay đổi trạng thái.
    /// </summary>
    public UpdateCallStatusCommandHandler(ICallSessionRepository callSessionRepository)
    {
        _callSessionRepository = callSessionRepository;
    }

    /// <summary>
    /// Xử lý command cập nhật trạng thái cuộc gọi.
    /// Luồng xử lý: parse new status, parse expected previous status nếu có, gọi repository update status.
    /// </summary>
    public async Task<bool> Handle(UpdateCallStatusCommand request, CancellationToken cancellationToken)
    {
        var newStatus = ParseStatusOrThrow(request.NewStatus, nameof(request.NewStatus));
        CallSessionStatus? expectedPreviousStatus = null;

        if (string.IsNullOrWhiteSpace(request.ExpectedPreviousStatus) == false)
        {
            // Parse expected previous status để chống ghi đè trạng thái không mong muốn.
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

    /// <summary>
    /// Parse chuỗi trạng thái call sang enum domain hợp lệ.
    /// Luồng xử lý: trim+lower status, map theo switch, ném BadRequest khi không thuộc tập hỗ trợ.
    /// </summary>
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

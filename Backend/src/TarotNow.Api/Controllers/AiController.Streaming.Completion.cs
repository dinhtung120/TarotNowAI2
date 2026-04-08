using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Api.Controllers;

public partial class AiController
{
    // Ngữ cảnh tối thiểu để xử lý nhánh cancel/failure khi stream đang chạy.
    private readonly record struct StreamCompletionContext(
        Guid AiRequestId,
        Guid UserId,
        string SessionId,
        StreamExecutionState State);

    /// <summary>
    /// Xử lý nhánh stream bị hủy.
    /// Luồng xử lý: xác định final status theo số token, phân loại nguyên nhân hủy, finalize trạng thái và log khi cần.
    /// </summary>
    /// <param name="exception">Exception hủy stream.</param>
    /// <param name="context">Ngữ cảnh runtime dùng để finalize.</param>
    /// <param name="requestToken">Token hủy của request hiện tại.</param>
    private async Task HandleCanceledStreamAsync(
        OperationCanceledException exception,
        StreamCompletionContext context,
        CancellationToken requestToken)
    {
        // Phân nhánh status để phân biệt hủy trước token đầu tiên và hủy giữa chừng.
        var finalStatus = context.State.OutputTokens > 0
            ? AiStreamFinalStatuses.FailedAfterFirstToken
            : AiStreamFinalStatuses.FailedBeforeFirstToken;

        // Tách client disconnect khỏi upstream timeout để phục vụ phân tích vận hành.
        var clientDisconnected = requestToken.IsCancellationRequested
                                 || HttpContext.RequestAborted.IsCancellationRequested;
        var finishReason = clientDisconnected
            ? "Client disconnected"
            : "Upstream timeout/cancellation";

        await CompleteStreamAsync(new StreamCompletionDispatch(
            context.AiRequestId,
            context.UserId,
            FollowUpQuestion: null,
            context.State,
            finalStatus,
            finishReason,
            clientDisconnected,
            CancellationToken.None));

        if (!clientDisconnected)
        {
            // Chỉ log warning cho hủy từ upstream để giảm nhiễu log khi người dùng chủ động đóng kết nối.
            _logger.LogWarning(
                exception,
                "AI stream canceled by upstream for session {SessionId}, request {AiRequestId}.",
                context.SessionId,
                context.AiRequestId);
        }
    }

    /// <summary>
    /// Xử lý nhánh lỗi runtime không phải cancel.
    /// Luồng xử lý: xác định final status, finalize bản ghi lỗi, log error và phản hồi SSE nếu response chưa bắt đầu.
    /// </summary>
    /// <param name="exception">Exception runtime phát sinh trong khi stream.</param>
    /// <param name="context">Ngữ cảnh runtime dùng để finalize.</param>
    /// <param name="cancellationToken">Token hủy cho thao tác finalize.</param>
    private async Task HandleFailedStreamAsync(
        Exception exception,
        StreamCompletionContext context,
        CancellationToken cancellationToken)
    {
        // Phân loại lỗi theo trạng thái đã phát token hay chưa để analytics phản ánh đúng trải nghiệm.
        var finalStatus = context.State.OutputTokens > 0
            ? AiStreamFinalStatuses.FailedAfterFirstToken
            : AiStreamFinalStatuses.FailedBeforeFirstToken;

        await CompleteStreamAsync(new StreamCompletionDispatch(
            context.AiRequestId,
            context.UserId,
            FollowUpQuestion: null,
            context.State,
            finalStatus,
            exception.Message,
            IsClientDisconnect: false,
            cancellationToken));

        _logger.LogError(
            exception,
            "AI stream runtime error for session {SessionId}, request {AiRequestId}.",
            context.SessionId,
            context.AiRequestId);

        if (!Response.HasStarted)
        {
            // Chỉ ghi status và event lỗi khi response chưa bắt đầu để tránh phá luồng SSE đang mở.
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteServerEventAsync("Stream Error: Unable to process AI stream. Please try again.", CancellationToken.None);
        }
    }
}

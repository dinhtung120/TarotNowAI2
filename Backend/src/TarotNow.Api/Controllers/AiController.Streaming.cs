using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

namespace TarotNow.Api.Controllers;

public partial class AiController
{
    /// <summary>
    /// Khởi tạo stream AI từ command tầng ứng dụng.
    /// Luồng xử lý: gửi command, ánh xạ exception thành HTTP status + SSE message phù hợp.
    /// </summary>
    /// <param name="userId">Id người dùng yêu cầu stream.</param>
    /// <param name="streamStartRequest">Ngữ cảnh request stream đã gom tham số transport.</param>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Thông tin stream nếu khởi tạo thành công; ngược lại trả <c>null</c>.</returns>
    private async Task<StreamReadingResult?> TryStartStreamAsync(
        Guid userId,
        StreamStartRequest streamStartRequest,
        CancellationToken cancellationToken)
    {
        try
        {
            // Dùng fallback "vi" để tránh null language gây phân nhánh không cần thiết ở handler.
            return await _mediator.Send(new StreamReadingCommand
            {
                UserId = userId,
                ReadingSessionId = streamStartRequest.SessionId,
                FollowupQuestion = streamStartRequest.FollowUpQuestion,
                Language = streamStartRequest.Language ?? "vi",
                IdempotencyKey = streamStartRequest.IdempotencyKey
            }, cancellationToken);
        }
        catch (BadRequestException ex)
        {
            // Nhóm lỗi validation/business input được phản hồi 400 để client tự chỉnh request.
            _logger.LogInformation(ex, "Rejected AI stream request for session {SessionId}.", streamStartRequest.SessionId);
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteServerEventAsync("AI stream request is invalid.", cancellationToken);
            return null;
        }
        catch (NotFoundException ex)
        {
            // Session không tồn tại hoặc không truy cập được được ánh xạ về 404 nhất quán.
            _logger.LogInformation(ex, "AI stream session not found {SessionId}.", streamStartRequest.SessionId);
            Response.StatusCode = StatusCodes.Status404NotFound;
            await WriteServerEventAsync("Reading session was not found.", cancellationToken);
            return null;
        }
        catch (Exception ex)
        {
            // Lỗi hạ tầng không mong đợi được log cảnh báo và trả thông điệp an toàn cho client.
            _logger.LogWarning(ex, "Failed to initialize AI stream for session {SessionId}.", streamStartRequest.SessionId);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await WriteServerEventAsync("Unable to start AI stream. Please try again later.", cancellationToken);
            return null;
        }
    }

    private readonly record struct StreamStartRequest(
        string SessionId,
        string? FollowUpQuestion,
        string? Language,
        string? IdempotencyKey);

    /// <summary>
    /// Thiết lập header chuẩn cho response SSE.
    /// </summary>
    /// <param name="response">HTTP response cần cấu hình.</param>
    private static void ConfigureSseHeaders(HttpResponse response)
    {
        response.Headers.Append("Content-Type", "text/event-stream");
        response.Headers.Append("Cache-Control", "no-cache");
        response.Headers.Append("Connection", "keep-alive");
    }

    /// <summary>
    /// Thực thi vòng stream và finalize trạng thái AI request.
    /// Luồng xử lý: stream chunk, gửi DONE, hoàn tất thành công; nếu lỗi thì đi nhánh cancel/failure tương ứng.
    /// </summary>
    /// <param name="result">Kết quả khởi tạo stream.</param>
    /// <param name="userId">Id người dùng.</param>
    /// <param name="sessionId">Id phiên reading.</param>
    /// <param name="followUpQuestion">Câu hỏi follow-up tùy chọn.</param>
    /// <param name="requestToken">Token hủy request từ kết nối hiện tại.</param>
    private async Task StreamAndFinalizeAsync(
        StreamReadingResult result,
        Guid userId,
        string sessionId,
        string? followUpQuestion,
        CancellationToken requestToken)
    {
        // State gom toàn bộ thông tin runtime để đồng bộ giữa các bước finalize.
        var state = new StreamExecutionState();
        var completionContext = new StreamCompletionContext(
            result.AiRequestId,
            userId,
            sessionId,
            state);

        try
        {
            // Luồng thành công chuẩn: stream dữ liệu -> gửi DONE -> ghi nhận completion trạng thái Completed.
            await WriteStreamAsync(result.Stream, state, requestToken);
            await WriteDoneEventAsync(requestToken);

            await CompleteStreamAsync(new StreamCompletionDispatch(
                result.AiRequestId,
                userId,
                followUpQuestion,
                state,
                AiStreamFinalStatuses.Completed,
                ErrorMessage: null,
                IsClientDisconnect: false,
                CancellationToken.None));
        }
        catch (OperationCanceledException ex)
        {
            // Tách riêng nhánh cancel để phân biệt client disconnect và upstream timeout.
            await HandleCanceledStreamAsync(ex, completionContext, requestToken);
        }
        catch (Exception ex)
        {
            // Mọi lỗi runtime còn lại đi chung nhánh failed để đảm bảo luôn finalize trạng thái request.
            await HandleFailedStreamAsync(ex, completionContext, CancellationToken.None);
        }
    }
}

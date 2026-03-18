using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Services.Ai;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller xử lý AI Streaming qua Server-Sent Events (SSE).
/// Thiết kế "thin controller": chỉ xử lý HTTP concerns (headers, response stream),
/// toàn bộ logic nghiệp vụ (state machine, escrow, refund, quota) 
/// được ủy thác cho Application layer qua MediatR.
/// 
/// Refactored từ phiên bản cũ (CA-03): loại bỏ inject trực tiếp Repository,
/// xóa các private methods ProcessRefund/ProcessSuccessfulRelease/UpdateAiRequestState,
/// thay bằng CompleteAiStreamCommand duy nhất.
/// </summary>
[ApiController]
[Route("api/v1/sessions")]
[Authorize] // Bắt buộc đăng nhập
public class AiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AiController> _logger;

    /// <summary>
    /// Constructor chỉ nhận IMediator — đúng nguyên tắc Thin Controller.
    /// Trước đây inject thêm IAiRequestRepository + IWalletRepository (vi phạm CA).
    /// </summary>
    public AiController(IMediator mediator, ILogger<AiController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Bật kết nối Server-Sent Events (SSE) để stream kết quả AI Chat.
    /// Format trả về Chunk-by-Chunk cho Frontend render Typewriter Effect.
    /// 
    /// Flow: Controller nhận IAsyncEnumerable từ StreamReadingCommand,
    /// rồi iterate từng chunk gửi qua Response stream.
    /// Mọi xử lý state/escrow/refund gửi qua CompleteAiStreamCommand.
    /// </summary>
    [HttpGet("{sessionId}/stream")]
    public async Task StreamReading(string sessionId, [FromQuery] string? followUpQuestion, CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        StreamReadingResult result;
        try
        {
            // Bước 1: Khởi tạo AI stream qua Application layer (guards, quota, freeze đều ở đây)
            result = await _mediator.Send(new StreamReadingCommand
            {
                UserId = userId,
                ReadingSessionId = sessionId,
                FollowupQuestion = followUpQuestion
            }, cancellationToken);
        }
        catch (BadRequestException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync($"data: {ex.Message}\n\n", cancellationToken);
            return;
        }
        catch (NotFoundException ex)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            await Response.WriteAsync($"data: {ex.Message}\n\n", cancellationToken);
            return;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize AI stream for session {SessionId}.", sessionId);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await Response.WriteAsync("data: Unable to start AI stream. Please try again later.\n\n", cancellationToken);
            return;
        }

        // --- CẤU HÌNH HEADERS CỦA EVENT STREAM ---
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        var requestToken = cancellationToken;
        int tokenCounter = 0;
        DateTimeOffset? firstTokenAt = null;

        try
        {
            // Bước 2: Iterate stream từ AI Provider, gửi từng chunk xuống client
            await foreach (var chunk in result.Stream.WithCancellation(requestToken))
            {
                if (tokenCounter == 0)
                {
                    firstTokenAt = DateTimeOffset.UtcNow;
                }

                // Thay thế \n thành \\n để chuỗi SSE không bị đứt đoạn format
                var sanitizedChunk = chunk.Replace("\n", "\\n");

                await Response.WriteAsync($"data: {sanitizedChunk}\n\n", requestToken);
                await Response.Body.FlushAsync(requestToken);

                tokenCounter++;
            }

            // Bước 3: Gửi dấu hiệu hoàn tất cho client
            await Response.WriteAsync("data: [DONE]\n\n", requestToken);
            await Response.Body.FlushAsync(requestToken);

            // Bước 4: Xử lý nghiệp vụ SAU khi stream thành công — ủy thác cho Application layer
            // CompleteAiStreamCommand xử lý: consume escrow, update state → completed
            await _mediator.Send(new CompleteAiStreamCommand
            {
                AiRequestId = result.AiRequestId,
                UserId = userId,
                FinalStatus = AiRequestStatus.Completed,
                IsClientDisconnect = false,
                FirstTokenAt = firstTokenAt
            }, CancellationToken.None); // Dùng None vì client đã nhận xong, không nên cancel

            // Bước 5: Ghi log AI Telemetry vào MongoDB
            if (result.Provider is OpenAiProvider openAi)
            {
                var latency = firstTokenAt.HasValue ? (int)(DateTimeOffset.UtcNow - firstTokenAt.Value).TotalMilliseconds : 0;
                await openAi.LogRequestAsync(userId, sessionId, result.AiRequestId.ToString(), 
                    0, tokenCounter, latency, "completed"); 
            }
        }
        catch (OperationCanceledException ex)
        {
            var status = tokenCounter > 0
                ? AiRequestStatus.FailedAfterFirstToken
                : AiRequestStatus.FailedBeforeFirstToken;
            var clientDisconnected = cancellationToken.IsCancellationRequested
                                     || HttpContext.RequestAborted.IsCancellationRequested;
            var finishReason = clientDisconnected ? "Client disconnected" : "Upstream timeout/cancellation";

            await _mediator.Send(new CompleteAiStreamCommand
            {
                AiRequestId = result.AiRequestId,
                UserId = userId,
                FinalStatus = status,
                ErrorMessage = finishReason,
                IsClientDisconnect = clientDisconnected,
                FirstTokenAt = firstTokenAt
            }, CancellationToken.None);

            if (result.Provider is OpenAiProvider openAi)
            {
                await openAi.LogRequestAsync(
                    userId,
                    sessionId,
                    result.AiRequestId.ToString(),
                    0,
                    tokenCounter,
                    0,
                    "failed",
                    finishReason);
            }

            if (!clientDisconnected)
            {
                _logger.LogWarning(ex,
                    "AI stream canceled by upstream for session {SessionId}, request {AiRequestId}.",
                    sessionId,
                    result.AiRequestId);
            }
        }
        catch (Exception ex)
        {
            // Server-side error
            var status = tokenCounter > 0 
                ? AiRequestStatus.FailedAfterFirstToken 
                : AiRequestStatus.FailedBeforeFirstToken;

            await _mediator.Send(new CompleteAiStreamCommand
            {
                AiRequestId = result.AiRequestId,
                UserId = userId,
                FinalStatus = status,
                ErrorMessage = ex.Message,
                IsClientDisconnect = false,
                FirstTokenAt = firstTokenAt
            }, CancellationToken.None);

            if (result.Provider is OpenAiProvider openAi)
            {
                await openAi.LogRequestAsync(userId, sessionId, result.AiRequestId.ToString(), 
                    0, tokenCounter, 0, "failed", ex.Message);
            }

            _logger.LogError(ex,
                "AI stream runtime error for session {SessionId}, request {AiRequestId}.",
                sessionId,
                result.AiRequestId);

            if (!Response.HasStarted)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("data: Stream Error: Unable to process AI stream. Please try again.\n\n");
            }
        }
    }
}

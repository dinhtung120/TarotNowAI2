using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Reading.Commands.StreamReading;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/sessions")]
[Authorize] // Bắt buộc đăng nhập
public class AiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAiRequestRepository _aiRequestRepo;
    private readonly IWalletRepository _walletRepo;

    public AiController(IMediator mediator, IAiRequestRepository aiRequestRepo, IWalletRepository walletRepo)
    {
        _mediator = mediator;
        _aiRequestRepo = aiRequestRepo;
        _walletRepo = walletRepo;
    }

    /// <summary>
    /// Bật kết nối Server-Sent Events (SSE) để stream kết quả AI Chat.
    /// Format trả về Chunk-by-Chunk cho Frontend render Typwriter Effect.
    /// </summary>
    [HttpGet("{sessionId}/stream")]
    public async Task StreamReading(string sessionId, [FromQuery] string? followUpQuestion, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        StreamReadingResult result;
        try
        {
            result = await _mediator.Send(new StreamReadingCommand
            {
                UserId = userId,
                ReadingSessionId = sessionId,
                FollowupQuestion = followUpQuestion
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            // Trả lỗi Text nếu xảy ra Exception (hết tiền, phiên bị khóa, v.v) chưa kịp Stream
            Response.StatusCode = 400; // BadRequest
            var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            await Response.WriteAsync($"data: {errorMsg}\n\n", cancellationToken);
            return;
        }

        // --- CẤU HÌNH HEADERS CỦA EVENT STREAM ---
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        var requestToken = cancellationToken;
        int tokenCounter = 0;
        
        try
        {
            // Fetch Data từ IAsyncEnumerable và dội ngược về Response Stream
            await foreach (var chunk in result.Stream.WithCancellation(requestToken))
            {
                if (tokenCounter == 0)
                {
                    // Update State: First Token Received (Chỉ ghi khi thực sự lấy được chữ đầu tiên)
                    await UpdateAiRequestState(result.AiRequestId, AiRequestStatus.FirstTokenReceived, null, requestToken);
                }

                // Thay thế \n thành \\n (hoặc encoded Json) để chuỗi JSON SSE không bị đứt đoạn sai format do xuống dòng
                var sanitizedChunk = chunk.Replace("\n", "\\n");

                await Response.WriteAsync($"data: {sanitizedChunk}\n\n", requestToken);
                await Response.Body.FlushAsync(requestToken);
                
                tokenCounter++;
            }

            // Gửi Dấu Hiệu Hoàn Tất 
            await Response.WriteAsync("data: [DONE]\n\n", requestToken);
            await Response.Body.FlushAsync(requestToken);

            // Xử lý Giải trí Quỹ Escrow (Chuyển tiền từ Freeze sang ví Admin vì dịch vụ đã hoàn tất)
            await ProcessSuccessfulRelease(result.AiRequestId, userId, CancellationToken.None);

            // Update State: Completed
            await UpdateAiRequestState(result.AiRequestId, AiRequestStatus.Completed, null, CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            // Rất quan trọng: Client đột ngột ngắt kết nối trình duyệt hoặc Request Timeout
            var status = tokenCounter > 0 ? AiRequestStatus.FailedAfterFirstToken : AiRequestStatus.FailedBeforeFirstToken;
            
            if (tokenCounter > 0)
            {
                // TEST.md: Client disconnect sau token đầu -> backend vẫn track, KHÔNG auto-refund
                await UpdateAiRequestState(result.AiRequestId, status, "Client disconnected midway", CancellationToken.None);
            }
            else
            {
                // TEST.md: Timeout (hoặc ngắt) trước token đầu -> Refund
                await ProcessRefund(result.AiRequestId, userId, status, "Client disconnected midway", CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            // Lỗi từ phía Provider API sập
            var status = tokenCounter > 0 ? AiRequestStatus.FailedAfterFirstToken : AiRequestStatus.FailedBeforeFirstToken;
            
            await ProcessRefund(result.AiRequestId, userId, status, ex.Message, CancellationToken.None);
            
            // Gửi báo lỗi cuối cho UI nếu Browser vẫn sống
            if (!Response.HasStarted)
            {
                 Response.StatusCode = 500;
                 await Response.WriteAsync($"data: Stream Error: {ex.Message}\n\n");
            }
        }
    }

    /// <summary>
    /// Helper cập nhật state lưu vô Postgres bảng ai_requests
    /// </summary>
    private async Task UpdateAiRequestState(Guid aiReqId, string status, string? reason, CancellationToken ct)
    {
        var record = await _aiRequestRepo.GetByIdAsync(aiReqId, ct);
        if (record != null)
        {
            record.Status = status;
            record.FinishReason = reason;

            if (status == AiRequestStatus.FirstTokenReceived && !record.FirstTokenAt.HasValue)
                 record.FirstTokenAt = DateTimeOffset.UtcNow;
            
            if (status == AiRequestStatus.Completed)
                 record.CompletionMarkerAt = DateTimeOffset.UtcNow;
                 
            await _aiRequestRepo.UpdateAsync(record, ct);
        }
    }

    /// <summary>
    /// Helper xử lý Auto Refund cho Client khi bị đứt mạng hoặc Provider sập.
    /// Tính Idempotency đảm bảo chỉ gọi Stored Procedure refund 1 lần.
    /// </summary>
    private async Task ProcessRefund(Guid aiReqId, Guid userId, string finalStatus, string errorMessage, CancellationToken backupCt)
    {
        var record = await _aiRequestRepo.GetByIdAsync(aiReqId, backupCt);
        if (record != null)
        {
            await UpdateAiRequestState(aiReqId, finalStatus, errorMessage, backupCt);

            // Refund qua procedure (Idempotent: refund_ai_reqID)
            await _walletRepo.RefundAsync(
                userId: userId,
                amount: record.ChargeDiamond,
                referenceSource: "AiRequestAutoRefund",
                referenceId: record.Id.ToString(),
                description: $"Auto Refund for aborting AI Streaming ({finalStatus})",
                idempotencyKey: $"refund_{record.Id}",
                cancellationToken: backupCt
            );
        }
    }

    /// <summary>
    /// Helper xử lý Release Quỹ (Chuyển từ tài khoản bị đóng băng sang tài khoản Thực thi/Admin)
    /// </summary>
    private async Task ProcessSuccessfulRelease(Guid aiReqId, Guid userId, CancellationToken backupCt)
    {
        var record = await _aiRequestRepo.GetByIdAsync(aiReqId, backupCt);
        if (record != null)
        {
            // Payer: userId
            // Receiver: Admin giả lập / App Wallet (thường sẽ có ví Master, ở đây mô phỏng dùng chính user hoặc hạch toán riêng lẻ)
            // Thiết kế gốc: Giữ ID Master trong config. Tạm thời dùng ID giả lập 000 để đại diện System 
            var systemAdminId = Guid.Empty; // System Master Account

            try 
            {
                await _walletRepo.ReleaseAsync(
                    payerId: userId,
                    receiverId: systemAdminId, // Demo Master Wallet
                    amount: record.ChargeDiamond,
                    referenceSource: "AiRequestCompletedRelease",
                    referenceId: record.Id.ToString(),
                    description: "Escrow Release for fully completed AI Stream",
                    idempotencyKey: $"release_{record.Id}",
                    cancellationToken: backupCt
                );
            }
            catch(Exception ex) 
            {
                // Warning log: Escrow Failed. Admin có thể reconcile sau
            }
        }
    }
}

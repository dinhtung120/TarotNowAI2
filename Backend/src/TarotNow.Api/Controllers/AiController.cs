/*
 * ===================================================================
 * FILE: AiController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller này xử lý việc STREAM (phát trực tiếp) kết quả phân tích
 *   bài tarot từ AI (trí tuệ nhân tạo) xuống cho client (trình duyệt).
 *
 * CÁCH HOẠT ĐỘNG (đơn giản hóa):
 *   1. User rút bài tarot và ấn "Xem giải nghĩa AI"
 *   2. Client gọi GET /api/v1/sessions/{sessionId}/stream
 *   3. Server kết nối đến OpenAI (ChatGPT) để xin phân tích
 *   4. OpenAI trả về từng đoạn text nhỏ (chunk) liên tục
 *   5. Server chuyển tiếp từng chunk xuống client theo thời gian thực
 *   6. Client hiển thị text từng chữ một (hiệu ứng "đang gõ" - typewriter effect)
 *   7. Khi xong, server cập nhật trạng thái và xử lý tài chính (trừ quota/escrow)
 *
 * CÔNG NGHỆ SỬ DỤNG:
 *   - SSE (Server-Sent Events): giao thức cho phép server gửi dữ liệu
 *     liên tục xuống client qua một kết nối HTTP duy nhất.
 *   - Khác với WebSocket (2 chiều), SSE chỉ 1 chiều (server → client),
 *     đơn giản hơn và phù hợp cho việc stream text.
 *
 * KIẾN TRÚC:
 *   Tuân theo nguyên tắc "Thin Controller" - controller chỉ lo:
 *   - Thiết lập headers SSE
 *   - Iterate stream và gửi xuống client
 *   - Gọi CompleteAiStreamCommand để Application layer xử lý nghiệp vụ
 *   Mọi logic phức tạp (kiểm tra quota, freeze tiền, refund) đều ở Application layer.
 * ===================================================================
 */

// Import các thư viện cần thiết
using System.Security.Claims;   // Để đọc thông tin user từ JWT token
using MediatR;                   // Thư viện trung gian gửi Command/Query đến Handler
using Microsoft.AspNetCore.Authorization; // Attribute [Authorize] kiểm soát quyền truy cập
using Microsoft.AspNetCore.Mvc;  // Nền tảng xây dựng API controller

// Import các exception tùy chỉnh của Application layer
// Dùng để bắt lỗi cụ thể và trả mã HTTP phù hợp (400, 404...)
using TarotNow.Application.Exceptions;

// Import Command để khởi tạo và hoàn tất AI stream
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

// Namespace - "địa chỉ" logic của file
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
/*
 * [ApiController]: Bật tính năng mặc định cho API (auto-validate model, v.v.)
 * [Route("api/v1/sessions")]: URL gốc = /api/v1/sessions (không dùng [controller] vì tên "Ai" không phù hợp)
 * [Authorize]: Bắt buộc đăng nhập (có JWT token hợp lệ) mới được gọi
 */
[ApiController]
[Route("api/v1/sessions")]
[Authorize] // Bắt buộc đăng nhập
public class AiController : ControllerBase
{
    /*
     * _mediator: Đối tượng trung gian MediatR để gửi Command/Query.
     * _logger: Đối tượng ghi log (nhật ký) để theo dõi lỗi và hoạt động.
     *   Logger rất quan trọng trong hệ thống production để debug khi có sự cố.
     *   ILogger<AiController> gắn log với class AiController → dễ tìm kiếm trong log.
     */
    private readonly IMediator _mediator;
    private readonly ILogger<AiController> _logger;

    /// <summary>
    /// Constructor chỉ nhận IMediator và ILogger — đúng nguyên tắc Thin Controller.
    /// Trước đây (phiên bản cũ) inject thêm IAiRequestRepository + IWalletRepository
    /// → vi phạm Clean Architecture vì controller KHÔNG nên biết về repository.
    /// </summary>
    public AiController(IMediator mediator, ILogger<AiController> logger)
    {
        _mediator = mediator; // Lưu mediator để gửi commands
        _logger = logger;     // Lưu logger để ghi log
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/sessions/{sessionId}/stream?followUpQuestion=...
    /// 
    /// MỤC ĐÍCH: Bật kết nối SSE để stream kết quả phân tích AI tarot.
    /// 
    /// THAM SỐ:
    ///   - sessionId (từ URL): ID phiên đọc bài (ví dụ: "abc-123-def")
    ///   - followUpQuestion (query string, tùy chọn): Câu hỏi bổ sung
    ///     Ví dụ: "Lá bài này có ý nghĩa gì trong tình yêu?"
    ///   - cancellationToken: Token để hủy thao tác nếu client ngắt kết nối
    /// 
    /// CÁCH HOẠT ĐỘNG CHI TIẾT:
    ///   Bước 1: Xác thực user, khởi tạo stream qua Application layer
    ///   Bước 2: Thiết lập headers SSE
    ///   Bước 3: Lặp qua từng chunk text từ AI, gửi xuống client
    ///   Bước 4: Gửi "[DONE]" báo hiệu hoàn tất
    ///   Bước 5: Xử lý nghiệp vụ hậu stream (cập nhật trạng thái, escrow)
    ///   Bước 6: Ghi log telemetry vào MongoDB
    /// </summary>
    [HttpGet("{sessionId}/stream")]
    public async Task StreamReading(string sessionId, [FromQuery] string? followUpQuestion, [FromQuery] string? language, CancellationToken cancellationToken)
    {
        // ========================================
        // BƯỚC 0: XÁC THỰC NGƯỜI DÙNG
        // ========================================

        /*
         * Lấy User ID từ JWT token (giống AdminController).
         * Nếu không hợp lệ → trả 401 Unauthorized và kết thúc.
         */
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return; // Kết thúc - không stream gì cả
        }

        // ========================================
        // BƯỚC 1: KHỞI TẠO AI STREAM
        // ========================================

        /*
         * StreamReadingResult: kết quả từ Application layer, chứa:
         * - Stream: IAsyncEnumerable<string> - luồng text từ AI
         * - AiRequestId: ID bản ghi AI request trong database
         */
        StreamReadingResult result;
        try
        {
            /*
             * Gửi StreamReadingCommand qua MediatR → Handler sẽ:
             * 1. Kiểm tra phiên đọc bài có tồn tại không
             * 2. Kiểm tra user có đủ quota (hạn mức) không
             * 3. Freeze (đóng băng) tiền trong escrow
             * 4. Tạo bản ghi AiRequest trong database
             * 5. Kết nối đến OpenAI và trả về stream
             */
            result = await _mediator.Send(new StreamReadingCommand
            {
                UserId = userId,                     // ID người dùng đang yêu cầu
                ReadingSessionId = sessionId,        // ID phiên đọc bài
                FollowupQuestion = followUpQuestion, // Câu hỏi bổ sung (nếu có)
                Language = language ?? "vi"          // Ngôn ngữ hệ thống (mặc định vi)
            }, cancellationToken);
        }
        catch (BadRequestException ex)
        {
            // Lỗi do dữ liệu không hợp lệ (ví dụ: phiên chưa reveal bài)
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync($"data: {ex.Message}\n\n", cancellationToken);
            return;
        }
        catch (NotFoundException ex)
        {
            // Không tìm thấy phiên đọc bài (sessionId sai hoặc đã bị xóa)
            Response.StatusCode = StatusCodes.Status404NotFound;
            await Response.WriteAsync($"data: {ex.Message}\n\n", cancellationToken);
            return;
        }
        catch (Exception ex)
        {
            // Lỗi không xác định (database sập, kết nối OpenAI thất bại, v.v.)
            _logger.LogWarning(ex, "Failed to initialize AI stream for session {SessionId}.", sessionId);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await Response.WriteAsync("data: Unable to start AI stream. Please try again later.\n\n", cancellationToken);
            return;
        }

        // ========================================
        // BƯỚC 2: CẤU HÌNH HEADERS SSE
        // ========================================

        /*
         * Server-Sent Events (SSE) yêu cầu 3 headers đặc biệt:
         * 
         * 1. Content-Type = "text/event-stream"
         *    → Báo cho trình duyệt biết đây là luồng sự kiện, không phải JSON/HTML thông thường.
         *    → Trình duyệt sẽ dùng EventSource API để đọc.
         *
         * 2. Cache-Control = "no-cache"
         *    → Cấm trình duyệt lưu cache (bộ nhớ tạm).
         *    → Vì dữ liệu stream luôn mới, cache sẽ gây hiển thị sai.
         *
         * 3. Connection = "keep-alive"
         *    → Giữ kết nối TCP mở liên tục, không đóng sau mỗi chunk.
         *    → Cần thiết để server gửi nhiều chunk qua cùng một kết nối.
         */
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        var requestToken = cancellationToken;

        // Biến đếm số lượng token (chunk text) đã gửi thành công
        int tokenCounter = 0;

        // Thời điểm nhận được token đầu tiên từ AI
        // Dùng để tính "Time to First Token" - chỉ số đo hiệu năng AI
        DateTimeOffset? firstTokenAt = null;
        
        // Dùng để hứng lại toàn bộ câu trả lời của AI hòng lưu vào DB (Cái này trước đó đang thiếu, làm Lịch Sử Trải Bài chả hiện gì cả)
        var fullResponseBuilder = new System.Text.StringBuilder();

        try
        {
            // ========================================
            // BƯỚC 3: STREAM TỪNG CHUNK TEXT TỪ AI
            // ========================================
            await foreach (var chunk in result.Stream.WithCancellation(requestToken))
            {
                if (tokenCounter == 0)
                {
                    firstTokenAt = DateTimeOffset.UtcNow;
                }

                // Ghi nhận chunk vào bộ nhớ để hồi sau lưu Text
                fullResponseBuilder.Append(chunk);

                var sanitizedChunk = chunk.Replace("\n", "\\n");
                await Response.WriteAsync($"data: {sanitizedChunk}\n\n", requestToken);
                await Response.Body.FlushAsync(requestToken);

                tokenCounter++; 
            }

            // ========================================
            // BƯỚC 4: BÁO HIỆU HOÀN TẤT
            // ========================================
            await Response.WriteAsync("data: [DONE]\n\n", requestToken);
            await Response.Body.FlushAsync(requestToken);

            // ========================================
            // BƯỚC 5: XỬ LÝ NGHIỆP VỤ SAU STREAM
            // ========================================
            var latency = firstTokenAt.HasValue ? (int)(DateTimeOffset.UtcNow - firstTokenAt.Value).TotalMilliseconds : 0;
            await _mediator.Send(new CompleteAiStreamCommand
            {
                AiRequestId = result.AiRequestId,
                UserId = userId,
                FinalStatus = AiStreamFinalStatuses.Completed,
                IsClientDisconnect = false,
                FirstTokenAt = firstTokenAt,
                OutputTokens = tokenCounter,
                LatencyMs = latency,
                FullResponse = fullResponseBuilder.ToString(),
                FollowupQuestion = followUpQuestion
            }, CancellationToken.None);
        }
        catch (OperationCanceledException ex)
        {
            // ========================================
            // XỬ LÝ KHI BỊ HỦY (CLIENT NGẮT HOẶC TIMEOUT)
            // ========================================

            /*
             * OperationCanceledException xảy ra khi:
             * 1. Client đóng tab/ngắt kết nối giữa chừng (phổ biến nhất)
             * 2. Upstream (OpenAI) timeout hoặc bị hủy
             * 
             * Trạng thái phụ thuộc vào đã nhận được token đầu tiên chưa:
             * - Chưa có token nào → FailedBeforeFirstToken (có thể full refund)
             * - Đã có token → FailedAfterFirstToken (có thể partial refund)
             */
            var status = tokenCounter > 0
                ? AiStreamFinalStatuses.FailedAfterFirstToken   // Đã nhận một số token rồi mới lỗi
                : AiStreamFinalStatuses.FailedBeforeFirstToken; // Chưa nhận token nào đã lỗi

            /*
             * Phân biệt nguyên nhân hủy:
             * - cancellationToken.IsCancellationRequested: client ngắt HTTP request
             * - HttpContext.RequestAborted.IsCancellationRequested: client ngắt kết nối TCP
             * 
             * Nếu client tự ngắt → "Client disconnected" (bình thường, user đóng tab)
             * Nếu không → "Upstream timeout/cancellation" (lỗi phía OpenAI, cần điều tra)
             */
            var clientDisconnected = cancellationToken.IsCancellationRequested
                                     || HttpContext.RequestAborted.IsCancellationRequested;
            var finishReason = clientDisconnected ? "Client disconnected" : "Upstream timeout/cancellation";

            // Gửi command hoàn tất (dù thất bại) để Application layer xử lý refund nếu cần
            await _mediator.Send(new CompleteAiStreamCommand
            {
                AiRequestId = result.AiRequestId,
                UserId = userId,
                FinalStatus = status,
                ErrorMessage = finishReason,
                IsClientDisconnect = clientDisconnected,
                FirstTokenAt = firstTokenAt,
                OutputTokens = tokenCounter,
                LatencyMs = 0
            }, CancellationToken.None); // Dùng None vì phải hoàn tất nghiệp vụ dù đang cancel

            // Chỉ ghi warning log nếu lỗi từ upstream (không phải client tự ngắt)
            // Vì client ngắt kết nối là bình thường (user đóng tab), không cần cảnh báo
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
            // ========================================
            // XỬ LÝ LỖI KHÔNG MONG ĐỢI (SERVER ERROR)
            // ========================================

            /*
             * Bắt tất cả exception khác (lỗi mạng, lỗi serialize, lỗi logic...).
             * Đây là "catch-all" cuối cùng để đảm bảo:
             * 1. AiRequest luôn được cập nhật trạng thái (không bao giờ "treo")
             * 2. Tài chính được xử lý đúng (refund nếu cần)
             * 3. Log được ghi lại để dev điều tra
             */
            var status = tokenCounter > 0 
                ? AiStreamFinalStatuses.FailedAfterFirstToken 
                : AiStreamFinalStatuses.FailedBeforeFirstToken;

            // Hoàn tất AI request với trạng thái failed
            await _mediator.Send(new CompleteAiStreamCommand
            {
                AiRequestId = result.AiRequestId,
                UserId = userId,
                FinalStatus = status,
                ErrorMessage = ex.Message,      // Lưu message lỗi để debug
                IsClientDisconnect = false,     // Lỗi server, không phải client ngắt
                FirstTokenAt = firstTokenAt,
                OutputTokens = tokenCounter,
                LatencyMs = 0
            }, CancellationToken.None);

            // Ghi log Error (mức cao nhất) vì đây là lỗi server cần điều tra
            _logger.LogError(ex,
                "AI stream runtime error for session {SessionId}, request {AiRequestId}.",
                sessionId,
                result.AiRequestId);

            /*
             * Response.HasStarted: kiểm tra xem đã bắt đầu gửi response chưa.
             * - Nếu CHƯA gửi gì: có thể set status code 500 và gửi thông báo lỗi.
             * - Nếu ĐÃ gửi (đã stream vài chunk): KHÔNG THỂ thay đổi status code,
             *   vì HTTP protocol chỉ cho phép gửi status code MỘT LẦN ở đầu response.
             *   Trường hợp này client sẽ thấy stream bị đứt giữa chừng.
             */
            if (!Response.HasStarted)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("data: Stream Error: Unable to process AI stream. Please try again.\n\n");
            }
        }
    }
}

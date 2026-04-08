using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Sessions)]
[Authorize]
// API streaming AI cho reading session.
// Luồng chính: kiểm tra feature flag + quyền truy cập, khởi tạo stream, rồi đẩy SSE cho client.
public partial class AiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AiController> _logger;
    private readonly IFeatureManagerSnapshot _featureManager;

    /// <summary>
    /// Khởi tạo controller streaming AI.
    /// </summary>
    /// <param name="mediator">MediatR dùng để gọi command/query.</param>
    /// <param name="logger">Logger phục vụ quan sát lỗi stream.</param>
    /// <param name="featureManager">Feature manager để bật/tắt streaming theo cấu hình.</param>
    public AiController(
        IMediator mediator,
        ILogger<AiController> logger,
        IFeatureManagerSnapshot featureManager)
    {
        _mediator = mediator;
        _logger = logger;
        _featureManager = featureManager;
    }

    /// <summary>
    /// Stream kết quả đọc bài theo chuẩn SSE.
    /// Luồng xử lý: gate bằng feature flag, xác thực user, khởi tạo stream, phát chunk và finalize trạng thái.
    /// </summary>
    /// <param name="sessionId">Id phiên reading cần stream kết quả.</param>
    /// <param name="followUpQuestion">Câu hỏi follow-up tùy chọn.</param>
    /// <param name="language">Ngôn ngữ mong muốn của phản hồi AI.</param>
    /// <param name="cancellationToken">Token hủy request từ client hoặc server.</param>
    /// <returns>Không trả body JSON; response được ghi trực tiếp theo SSE.</returns>
    [HttpGet("{sessionId}/stream")]
    public async Task StreamReading(
        string sessionId,
        [FromQuery] string? followUpQuestion,
        [FromQuery] string? language,
        CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AiStreamingEnabled))
        {
            // Edge case vận hành: feature bị tắt thì dừng sớm và trả thông điệp SSE để client xử lý đồng nhất.
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await WriteServerEventAsync("AI streaming is temporarily disabled by feature flag.", cancellationToken);
            return;
        }

        if (!User.TryGetUserId(out var userId))
        {
            // Chặn request không có danh tính hợp lệ trước khi gọi nghiệp vụ tốn tài nguyên.
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var streamResult = await TryStartStreamAsync(userId, sessionId, followUpQuestion, language, cancellationToken);
        if (streamResult == null)
        {
            // Nhánh null biểu thị lỗi đã được xử lý trong bước khởi tạo nên không xử lý lặp ở đây.
            return;
        }

        // Chỉ cấu hình header sau khi khởi tạo stream thành công để tránh mở SSE cho request lỗi.
        ConfigureSseHeaders(Response);
        await StreamAndFinalizeAsync(streamResult, userId, sessionId, followUpQuestion, cancellationToken);
    }
}

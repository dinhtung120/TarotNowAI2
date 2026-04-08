

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Gacha.Commands.SpinGacha;
using TarotNow.Application.Features.Gacha.Queries.GetActiveBanners;
using TarotNow.Application.Features.Gacha.Queries.GetBannerOdds;
using TarotNow.Application.Features.Gacha.Queries.GetGachaHistory;

namespace TarotNow.Api.Controllers;

[ApiController]
[Authorize] 
[ApiVersion(ApiVersions.V1)]
[Produces("application/json")]
[EnableRateLimiting("auth-session")]
// API tính năng gacha.
// Luồng chính: lấy banner/odds, xem lịch sử quay và thực hiện spin với idempotency key.
public class GachaController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller gacha.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query/command gacha.</param>
    public GachaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách banner đang hoạt động.
    /// </summary>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Danh sách banner có thể quay ở thời điểm hiện tại.</returns>
    [HttpGet(ApiRoutes.Gacha + "/banners")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBanners(CancellationToken ct)
    {
        // UserId là tùy chọn để backend có thể cá nhân hóa dữ liệu banner khi cần.
        var userId = User.GetUserIdOrNull();
        var result = await _mediator.Send(new GetActiveBannersQuery { UserId = userId }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Lấy bảng tỉ lệ quay của một banner.
    /// </summary>
    /// <param name="bannerCode">Mã banner cần xem odds.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Thông tin odds của banner.</returns>
    [HttpGet(ApiRoutes.Gacha + "/banners/{bannerCode}/odds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBannerOdds([FromRoute] string bannerCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBannerOddsQuery(bannerCode), ct);
        return Ok(result);
    }

    /// <summary>
    /// Lấy lịch sử quay gacha của người dùng hiện tại.
    /// </summary>
    /// <param name="limit">Giới hạn số bản ghi trả về.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Lịch sử quay gacha theo giới hạn.</returns>
    [HttpGet(ApiRoutes.Gacha + "/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGachaHistory([FromQuery] int limit = 50, CancellationToken ct = default)
    {
        // Bắt buộc user đã xác thực để bảo vệ lịch sử quay cá nhân.
        var userId = User.GetUserIdOrNull() ?? throw new System.UnauthorizedAccessException();
        var result = await _mediator.Send(new GetGachaHistoryQuery(userId, limit), ct);
        return Ok(result);
    }

    /// <summary>
    /// Thực hiện quay gacha.
    /// Luồng xử lý: xác thực user, dựng command từ request + idempotency key, dispatch spin.
    /// </summary>
    /// <param name="idempotencyKey">Khóa chống quay trùng khi client retry.</param>
    /// <param name="request">Payload quay gacha.</param>
    /// <param name="ct">Token hủy request.</param>
    /// <returns>Kết quả quay gacha.</returns>
    [HttpPost(ApiRoutes.Gacha + "/spin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Spin(
        [FromHeader(Name = "X-Idempotency-Key")][Required] string idempotencyKey,
        [FromBody] SpinGachaRequestDto request,
        CancellationToken ct)
    {
        // Giao dịch quay gacha bắt buộc có user id hợp lệ để hạch toán ví chính xác.
        var userId = User.GetUserIdOrNull() ?? throw new System.UnauthorizedAccessException();

        // Mapping đầy đủ request sang command để handler áp dụng rule chi phí và random.
        var command = new SpinGachaCommand
        {
            UserId = userId,
            BannerCode = request.BannerCode,
            IdempotencyKey = idempotencyKey,
            Count = request.Count
        };

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}

// Payload yêu cầu quay gacha.
public class SpinGachaRequestDto
{
    // Mã banner mục tiêu cần quay.
    [Required]
    public string BannerCode { get; set; } = string.Empty;

    // Số lượt quay trong một request, bị chặn để bảo vệ tài nguyên và rule kinh tế.
    [Range(1, 10)]
    public int Count { get; set; } = 1;
}

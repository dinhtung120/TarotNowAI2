/*
 * ===================================================================
 * FILE: GachaController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH:
 *   API Endpoints cho tính năng Gacha Phase 5.6
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
[Authorize] // Tất cả API Gacha yêu cầu đăng nhập
[Produces("application/json")]
public class GachaController : ControllerBase
{
    private readonly IMediator _mediator;

    public GachaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách các Banner Gacha đang hoạt động.
    /// Khách tải trang Gacha sẽ gọi cái này đầu tiên.
    /// </summary>
    [HttpGet(ApiRoutes.Gacha + "/banners")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBanners(CancellationToken ct)
    {
        var userId = User.GetUserIdOrNull();
        var result = await _mediator.Send(new GetActiveBannersQuery { UserId = userId }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Niêm yết công khai tỉ lệ trúng thưởng (Odds Disclosure) của 1 banner.
    /// Bắt buộc về mặt pháp lý (Google Play / Apple Store).
    /// </summary>
    [HttpGet(ApiRoutes.Gacha + "/banners/{bannerCode}/odds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBannerOdds([FromRoute] string bannerCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBannerOddsQuery(bannerCode), ct);
        return Ok(result);
    }

    /// <summary>
    /// Lịch sử quay Gacha của người dùng.
    /// Cho phép khách tự đối soát xem mình đã quay được gì, có bị lỗi không.
    /// </summary>
    [HttpGet(ApiRoutes.Gacha + "/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGachaHistory([FromQuery] int limit = 50, CancellationToken ct = default)
    {
        var userId = User.GetUserIdOrNull() ?? throw new System.UnauthorizedAccessException();
        var result = await _mediator.Send(new GetGachaHistoryQuery(userId, limit), ct);
        return Ok(result);
    }

    /// <summary>
    /// Trọng tâm: Thực hiện Quay Gacha (Play/Spin).
    /// Yêu cầu Idempotency Key để chống lặp lệnh do lag mạng (double spend).
    /// </summary>
    [HttpPost(ApiRoutes.Gacha + "/spin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Spin(
        [FromHeader(Name = "X-Idempotency-Key")][Required] string idempotencyKey,
        [FromBody] SpinGachaRequestDto request,
        CancellationToken ct)
    {
        var userId = User.GetUserIdOrNull() ?? throw new System.UnauthorizedAccessException();
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

public class SpinGachaRequestDto
{
    [Required]
    public string BannerCode { get; set; } = string.Empty;

    /// <summary>
    /// Số lượng vòng quay (1 hoặc 10)
    /// </summary>
    [Range(1, 10)]
    public int Count { get; set; } = 1;
}

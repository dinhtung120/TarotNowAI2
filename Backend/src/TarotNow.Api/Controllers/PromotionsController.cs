using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading;
using TarotNow.Api.Contracts;
using TarotNow.Application.Features.Promotions.Commands.CreatePromotion;
using TarotNow.Application.Features.Promotions.Commands.DeletePromotion;
using TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;
using TarotNow.Application.Features.Promotions.Queries.ListPromotions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Admin + "/promotions")]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị khuyến mãi.
public class PromotionsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị khuyến mãi.
    /// </summary>
    public PromotionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách khuyến mãi.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListPromotions([FromQuery] bool onlyActive = false, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new ListPromotionsQuery { OnlyActive = onlyActive }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới chương trình khuyến mãi.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand command, CancellationToken cancellationToken)
    {
        var success = await _mediator.SendWithRequestCancellation(HttpContext, command, cancellationToken);
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot create promotion",
                detail: "Không thể tạo chương trình khuyến mãi.");
    }

    /// <summary>
    /// Cập nhật chương trình khuyến mãi theo id.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] UpdatePromotionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePromotionCommand
        {
            Id = id,
            MinAmountVnd = request.MinAmountVnd,
            BonusGold = request.BonusGold,
            IsActive = request.IsActive
        };

        var success = await _mediator.SendWithRequestCancellation(HttpContext, command, cancellationToken);
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update promotion",
                detail: "Không thể cập nhật chương trình khuyến mãi.");
    }

    /// <summary>
    /// Xóa một chương trình khuyến mãi.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.SendWithRequestCancellation(HttpContext, new DeletePromotionCommand { Id = id }, cancellationToken);
        if (!success)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot delete promotion",
                detail: "Xóa khuyến mãi thất bại.");
        }

        return Ok();
    }
}

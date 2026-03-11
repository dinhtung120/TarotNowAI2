using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TarotNow.Application.Features.Promotions.Commands.CreatePromotion;
using TarotNow.Application.Features.Promotions.Commands.DeletePromotion;
using TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;
using TarotNow.Application.Features.Promotions.Queries.ListPromotions;

namespace TarotNow.Api.Controllers;

[Route("api/v1/admin/promotions")]
[ApiController]
[Authorize(Roles = "admin")] // Yêu cầu role admin
public class PromotionsController : ControllerBase
{
    private readonly IMediator Mediator;

    public PromotionsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous] // AllowAnonymous nếu client cũng cần list này hiển thị thông báo, hoặc Admin thì mới gọi được
    // Ta tách riêng /api/v1/promotions cho client và /api/v1/admin/promotions cho admin nếu cần
    // Cứ tạm để ở đây và check role
    public async Task<IActionResult> ListPromotions([FromQuery] bool onlyActive = false)
    {
        var result = await Mediator.Send(new ListPromotionsQuery { OnlyActive = onlyActive });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand command)
    {
        var success = await Mediator.Send(command);
        return success ? Ok() : BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] UpdatePromotionRequest request)
    {
        var command = new UpdatePromotionCommand
        {
            Id = id,
            MinAmountVnd = request.MinAmountVnd,
            BonusDiamond = request.BonusDiamond,
            IsActive = request.IsActive
        };
        var success = await Mediator.Send(command);
        return success ? Ok() : BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion(Guid id)
    {
        var success = await Mediator.Send(new DeletePromotionCommand { Id = id });
        return success ? Ok() : BadRequest();
    }
}

public class UpdatePromotionRequest
{
    public long MinAmountVnd { get; set; }
    public long BonusDiamond { get; set; }
    public bool IsActive { get; set; }
}



using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
public class PromotionsController : ControllerBase
{
    private readonly IMediator Mediator;

    public PromotionsController(IMediator mediator)
    {
        Mediator = mediator;
    }

        [HttpGet]
    public async Task<IActionResult> ListPromotions([FromQuery] bool onlyActive = false)
    {
        var result = await Mediator.Send(new ListPromotionsQuery { OnlyActive = onlyActive });
        return Ok(result);
    }

        [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand command)
    {
        var success = await Mediator.Send(command);
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot create promotion",
                detail: "Không thể tạo chương trình khuyến mãi.");
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
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update promotion",
                detail: "Không thể cập nhật chương trình khuyến mãi.");
    }

        [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion([FromRoute] Guid id)
    {
        var success = await Mediator.Send(new DeletePromotionCommand { Id = id });
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

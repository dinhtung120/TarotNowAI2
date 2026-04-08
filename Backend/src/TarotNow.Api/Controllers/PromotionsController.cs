

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
// API quản trị khuyến mãi.
// Luồng chính: liệt kê, tạo, cập nhật và xóa chương trình khuyến mãi.
public class PromotionsController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Khởi tạo controller quản trị khuyến mãi.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query khuyến mãi.</param>
    public PromotionsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách khuyến mãi.
    /// </summary>
    /// <param name="onlyActive">Bộ lọc chỉ lấy khuyến mãi đang hoạt động.</param>
    /// <returns>Danh sách chương trình khuyến mãi.</returns>
    [HttpGet]
    public async Task<IActionResult> ListPromotions([FromQuery] bool onlyActive = false)
    {
        var result = await Mediator.Send(new ListPromotionsQuery { OnlyActive = onlyActive });
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới chương trình khuyến mãi.
    /// Luồng xử lý: dispatch command tạo, rẽ nhánh lỗi khi không đạt rule nghiệp vụ.
    /// </summary>
    /// <param name="command">Command tạo promotion.</param>
    /// <returns>Kết quả tạo khuyến mãi.</returns>
    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand command)
    {
        var success = await Mediator.Send(command);
        // Tách nhánh lỗi để dashboard admin nhận thông điệp nghiệp vụ rõ ràng.
        return success
            ? Ok()
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot create promotion",
                detail: "Không thể tạo chương trình khuyến mãi.");
    }

    /// <summary>
    /// Cập nhật chương trình khuyến mãi theo id.
    /// Luồng xử lý: map DTO sang command cập nhật, dispatch và trả nhánh kết quả tương ứng.
    /// </summary>
    /// <param name="id">Id promotion cần cập nhật.</param>
    /// <param name="request">Payload dữ liệu cập nhật.</param>
    /// <returns>Kết quả cập nhật khuyến mãi.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] UpdatePromotionRequest request)
    {
        // Map tường minh để tránh sai lệch giữa route id và dữ liệu cập nhật.
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

    /// <summary>
    /// Xóa một chương trình khuyến mãi.
    /// </summary>
    /// <param name="id">Id promotion cần xóa.</param>
    /// <returns>Kết quả xóa khuyến mãi.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion([FromRoute] Guid id)
    {
        var success = await Mediator.Send(new DeletePromotionCommand { Id = id });
        if (!success)
        {
            // Trả lỗi nghiệp vụ khi không thể xóa (ví dụ promotion đang bị ràng buộc).
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot delete promotion",
                detail: "Xóa khuyến mãi thất bại.");
        }

        return Ok();
    }
}

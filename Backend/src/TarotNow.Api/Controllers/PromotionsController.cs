/*
 * ===================================================================
 * FILE: PromotionsController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller CRUD (Create, Read, Update, Delete) cho CHƯƠNG TRÌNH KHUYẾN MÃI.
 *   Chỉ dành cho Admin — quản lý khuyến mãi nạp tiền.
 *
 * CHƯƠNG TRÌNH KHUYẾN MÃI LÀ GÌ?
 *   Khi user nạp tiền, nếu đạt ngưỡng tối thiểu → được tặng thêm diamond.
 *   Ví dụ: "Nạp từ 200.000 VND trở lên, được tặng 100 diamond bonus."
 *   
 * CÁC THAO TÁC:
 *   GET    / → Xem danh sách khuyến mãi (có thể filter chỉ active)
 *   POST   / → Tạo khuyến mãi mới
 *   PUT    /{id} → Cập nhật khuyến mãi
 *   DELETE /{id} → Xóa khuyến mãi
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

using TarotNow.Api.Contracts; // UpdatePromotionRequest DTO
using TarotNow.Application.Features.Promotions.Commands.CreatePromotion;
using TarotNow.Application.Features.Promotions.Commands.DeletePromotion;
using TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;
using TarotNow.Application.Features.Promotions.Queries.ListPromotions;

namespace TarotNow.Api.Controllers;

/*
 * [Route(ApiRoutes.Admin + "/promotions")]: URL nằm trong nhóm /admin/
 *   vì chỉ admin mới quản lý khuyến mãi.
 * [Authorize(Roles = "admin")]: Bắt buộc có role admin.
 */
[Route(ApiRoutes.Admin + "/promotions")]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize(Roles = "admin")] // Yêu cầu role admin
public class PromotionsController : ControllerBase
{
    private readonly IMediator Mediator;

    public PromotionsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/admin/promotions?onlyActive=false
    /// MỤC ĐÍCH: Lấy danh sách tất cả chương trình khuyến mãi.
    ///
    /// onlyActive = true → chỉ lấy khuyến mãi đang hoạt động
    /// onlyActive = false → lấy tất cả (cả đã tắt)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListPromotions([FromQuery] bool onlyActive = false)
    {
        var result = await Mediator.Send(new ListPromotionsQuery { OnlyActive = onlyActive });
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/admin/promotions
    /// MỤC ĐÍCH: Tạo chương trình khuyến mãi mới.
    ///
    /// [HttpPost] KHÔNG CÓ tên route → dùng URL gốc (/api/v1/admin/promotions).
    /// Body: { minAmountVnd: 100000, bonusDiamond: 50, isActive: true }
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand command)
    {
        var success = await Mediator.Send(command);
        return success ? Ok() : BadRequest();
    }

    /// <summary>
    /// ENDPOINT: PUT /api/v1/admin/promotions/{id}
    /// MỤC ĐÍCH: Cập nhật thông tin chương trình khuyến mãi.
    ///
    /// [HttpPut("{id}")]: id lấy từ URL path.
    ///   Ví dụ: PUT /api/v1/admin/promotions/abc-123-def
    ///   → id = abc-123-def
    ///
    /// Dùng PUT (thay vì PATCH) vì cập nhật TẤT CẢ trường cùng lúc.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] UpdatePromotionRequest request)
    {
        // Kết hợp id từ route và dữ liệu từ body
        var command = new UpdatePromotionCommand
        {
            Id = id,                            // ID từ URL path
            MinAmountVnd = request.MinAmountVnd, // Ngưỡng nạp tối thiểu
            BonusDiamond = request.BonusDiamond, // Số diamond thưởng
            IsActive = request.IsActive          // Trạng thái bật/tắt
        };
        var success = await Mediator.Send(command);
        return success ? Ok() : BadRequest();
    }

    /// <summary>
    /// ENDPOINT: DELETE /api/v1/admin/promotions/{id}
    /// MỤC ĐÍCH: Xóa chương trình khuyến mãi.
    ///
    /// [FromRoute]: id lấy từ URL path.
    /// [HttpDelete]: HTTP DELETE method - xóa resource.
    /// 
    /// LƯU Ý: Thường nên dùng "soft delete" (đánh dấu xóa, không xóa thật)
    /// để giữ lại lịch sử kiểm toán. Nhưng ở đây là hard delete.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion([FromRoute] Guid id)
    {
        var success = await Mediator.Send(new DeletePromotionCommand { Id = id });
        if (!success) return BadRequest(new { message = "Xóa khuyến mãi thất bại." });
        return Ok();
    }
}

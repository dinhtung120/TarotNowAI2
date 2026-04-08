using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
// API thông báo người dùng.
// Luồng chính: truy vấn danh sách/unread count và cập nhật trạng thái đã đọc qua các partial controller.
public partial class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller thông báo.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query notification.</param>
    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }
}

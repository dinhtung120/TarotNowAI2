using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Conversations)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
[EnableRateLimiting("auth-session")]
// API điều phối toàn bộ luồng hội thoại user-reader.
// Luồng chính: cung cấp helper xác thực user và điều phối command/query qua MediatR.
public partial class ConversationController : ControllerBase
{
    protected readonly IMediator Mediator;

    /// <summary>
    /// Khởi tạo controller hội thoại.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query hội thoại.</param>
    public ConversationController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Thử lấy user id từ context xác thực hiện tại.
    /// </summary>
    /// <param name="userId">User id đầu ra nếu lấy thành công.</param>
    /// <returns><c>true</c> nếu lấy được user id; ngược lại <c>false</c>.</returns>
    protected bool TryGetUserId(out Guid userId)
    {
        return User.TryGetUserId(out userId);
    }
}

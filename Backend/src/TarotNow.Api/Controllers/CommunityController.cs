using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Community)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize(Policy = ApiAuthorizationPolicies.AuthenticatedUser)]
// API cộng đồng cho thao tác bài viết, bình luận, reaction và báo cáo.
// Luồng chính: lấy user hiện tại làm ngữ cảnh nghiệp vụ cho các partial controller liên quan.
public partial class CommunityController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller cộng đồng.
    /// </summary>
    /// <param name="mediator">MediatR dùng để dispatch command/query cộng đồng.</param>
    public CommunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy user id bắt buộc từ context xác thực.
    /// Luồng xử lý: đọc claim user id, ném lỗi unauthorized nếu không tồn tại.
    /// </summary>
    /// <returns>User id hợp lệ của request hiện tại.</returns>
    private Guid GetRequiredUserId()
    {
        // Chặn toàn bộ thao tác cộng đồng ghi dữ liệu khi không xác định được chủ thể thực hiện.
        return User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
    }
}

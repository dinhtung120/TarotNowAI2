using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Reader)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
// API nghiệp vụ reader.
// Luồng chính: tra cứu directory/profile reader và xử lý luồng đăng ký/cập nhật trạng thái reader.
public partial class ReaderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserPresenceTracker _presenceTracker;

    /// <summary>
    /// Khởi tạo controller reader.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query reader.</param>
    /// <param name="presenceTracker">Tracker trạng thái online realtime của user.</param>
    public ReaderController(IMediator mediator, IUserPresenceTracker presenceTracker)
    {
        _mediator = mediator;
        _presenceTracker = presenceTracker;
    }

    /// <summary>
    /// Áp dụng trạng thái online thực tế từ presence tracker lên hồ sơ reader.
    /// Luồng xử lý: nếu user đang online và status hiện tại là offline thì nâng lên online.
    /// </summary>
    /// <param name="profile">Hồ sơ reader cần đồng bộ trạng thái hiển thị.</param>
    private void ApplyPresenceStatus(ReaderProfileDto profile)
    {
        if (_presenceTracker.IsOnline(profile.UserId) == false)
        {
            // Khi user offline theo tracker thì giữ nguyên trạng thái hiện tại từ dữ liệu hồ sơ.
            return;
        }

        if (string.Equals(
                ApiReaderStatusConstants.NormalizeOrDefault(profile.Status),
                ApiReaderStatusConstants.Offline,
                StringComparison.OrdinalIgnoreCase))
        {
            // Ưu tiên trạng thái online realtime để UI phản ánh đúng khả năng phản hồi của reader.
            profile.Status = ApiReaderStatusConstants.Online;
        }
    }
}

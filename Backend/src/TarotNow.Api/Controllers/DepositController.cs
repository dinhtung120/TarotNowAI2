using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Deposits)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
// API nạp tiền.
// Luồng chính: tạo đơn nạp và nhận webhook thanh toán qua các partial controller chuyên biệt.
public partial class DepositController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DepositController> _logger;

    /// <summary>
    /// Khởi tạo controller nạp tiền.
    /// </summary>
    /// <param name="mediator">MediatR điều phối nghiệp vụ nạp tiền.</param>
    /// <param name="logger">Logger theo dõi lỗi webhook và vận hành.</param>
    public DepositController(IMediator mediator, ILogger<DepositController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}

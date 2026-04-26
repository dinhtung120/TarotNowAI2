

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reading.Commands.InitSession;

namespace TarotNow.Api.Controllers;


[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Reading)]
[Authorize] 
[EnableRateLimiting("auth-session")]
// API reading tarot.
// Luồng chính: khởi tạo phiên, reveal kết quả, lấy catalog lá bài và bộ sưu tập người dùng.
public class TarotController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller reading tarot.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query reading.</param>
    public TarotController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Khởi tạo reading session mới.
    /// Luồng xử lý: xác thực user id, gắn vào command init, dispatch và trả kết quả phiên.
    /// </summary>
    /// <param name="command">Command khởi tạo session.</param>
    /// <returns>Thông tin session đã khởi tạo.</returns>
    [HttpPost("init")]
    public async Task<IActionResult> InitSession([FromBody] InitReadingSessionCommand command)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn init session khi danh tính user không hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Luôn ghi đè UserId từ token để tránh giả mạo chủ thể trong payload.
        command.UserId = userId; 

        var result = await _mediator.Send(command);
        return Ok(result); 
    }

    /// <summary>
    /// Reveal bài cho reading session hiện tại.
    /// Luồng xử lý: xác thực user id, gắn vào command reveal, dispatch và trả kết quả.
    /// </summary>
    /// <param name="command">Command reveal lá bài/session.</param>
    /// <returns>Kết quả reveal của session.</returns>
    [HttpPost("reveal")]
    public async Task<IActionResult> RevealCards([FromBody] TarotNow.Application.Features.Reading.Commands.RevealSession.RevealReadingSessionCommand command)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn reveal khi request không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Gắn user id từ context để handler kiểm tra quyền truy cập session.
        command.UserId = userId; 

        var result = await _mediator.Send(command);
        return Ok(result); 
    }

    /// <summary>
    /// Lấy catalog lá bài tarot công khai.
    /// </summary>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Danh mục lá bài dùng cho client hiển thị.</returns>
    [HttpGet("cards-catalog")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalog(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new TarotNow.Application.Features.Reading.Queries.GetCardsCatalog.GetCardsCatalogQuery(),
            cancellationToken
        );
        return Ok(result);
    }

    /// <summary>
    /// Lấy bộ sưu tập lá bài của người dùng hiện tại.
    /// </summary>
    /// <returns>Dữ liệu collection của user.</returns>
    [HttpGet("collection")]
    public async Task<IActionResult> GetCollection()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn collection khi không xác định được user.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(
            new TarotNow.Application.Features.Reading.Queries.GetCollection.GetUserCollectionQuery { UserId = userId }
        );
        return Ok(result);
    }
}

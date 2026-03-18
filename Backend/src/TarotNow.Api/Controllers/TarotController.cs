using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Reading.Commands.InitSession;
using System.Security.Claims;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/reading")]
[Authorize] // Bắt buộc đăng nhập
public class TarotController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarotController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Bước 1: Khởi tạo phiên Rút bài Tarot, trừ tiền và giấu Client/Server Seed.
    /// Trả về SessionId để chạy Animation lật bài.
    /// </summary>
    [HttpPost("init")]
    public async Task<IActionResult> InitSession([FromBody] InitReadingSessionCommand command)
    {
        // Gắn UserId từ Token
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();
        command.UserId = userId;

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Bước 2: Lật bài từ Session đã khởi tạo. Cần truyền đúng SessionId của phiên hợp lệ.
    /// Server sẽ chốt RNG, trả bài và cộng EXP cho UserCollection.
    /// </summary>
    [HttpPost("reveal")]
    public async Task<IActionResult> RevealCards([FromBody] TarotNow.Application.Features.Reading.Commands.RevealSession.RevealReadingSessionCommand command)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();
        command.UserId = userId;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Bước 3: Xem danh sách Thẻ Bài Tarot đã thu thập (Kho đồ)
    /// </summary>
    [HttpGet("collection")]
    public async Task<IActionResult> GetCollection()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();
        var result = await _mediator.Send(new TarotNow.Application.Features.Reading.Queries.GetCollection.GetUserCollectionQuery { UserId = userId });
        return Ok(result);
    }
}

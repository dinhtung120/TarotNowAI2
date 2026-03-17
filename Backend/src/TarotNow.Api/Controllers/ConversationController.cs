using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Chat.Queries.ListMessages;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý conversations — REST endpoints.
///
/// SignalR Hub xử lý realtime (send/receive messages).
/// Controller này xử lý CRUD + history (non-realtime).
///
/// Endpoints:
/// - POST /conversations: Tạo conversation mới.
/// - GET /conversations: Inbox (danh sách conversations).
/// - GET /conversations/{id}/messages: Lịch sử chat.
/// </summary>
[Route("api/v1/conversations")]
[ApiController]
[Authorize]
public class ConversationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tạo conversation mới giữa user hiện tại và reader.
    /// Nếu đã có conversation active → trả về conversation đó (idempotent).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConversationBody body)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var command = new CreateConversationCommand
        {
            UserId = userId,
            ReaderId = body.ReaderId
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Inbox — danh sách conversations của user hiện tại.
    /// Hỗ trợ cả user role và reader role.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string role = "user")
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var query = new ListConversationsQuery
        {
            UserId = userId,
            Role = role,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Lịch sử tin nhắn trong conversation.
    /// Phân trang — sort DESC (frontend reverse).
    /// </summary>
    [HttpGet("{id}/messages")]
    public async Task<IActionResult> Messages(
        string id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var query = new ListMessagesQuery
        {
            ConversationId = id,
            RequesterId = userId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

/// <summary>Body cho POST /conversations.</summary>
public class CreateConversationBody
{
    /// <summary>UUID reader muốn chat.</summary>
    public Guid ReaderId { get; set; }
}

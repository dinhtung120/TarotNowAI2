using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Application.Features.Escrow.Commands.AcceptOffer;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Features.Escrow.Commands.ConfirmRelease;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Features.Escrow.Commands.ReaderReply;
using TarotNow.Application.Features.Escrow.Queries.GetEscrowStatus;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý Escrow — freeze/release/refund, dispute, add-question.
///
/// Tất cả endpoints yêu cầu authentication (JWT).
/// Wallet operations đi qua stored procedures (ACID transactions).
/// </summary>
[Route("api/v1/escrow")]
[ApiController]
[Authorize]
public class EscrowController : ControllerBase
{
    private readonly IMediator _mediator;

    public EscrowController(IMediator mediator) => _mediator = mediator;

    private Guid? GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(str, out var id) ? id : null;
    }

    /// <summary>
    /// Accept offer → freeze diamond, tạo escrow session + question item.
    /// Idempotent: double-call trả về item đã tạo.
    /// </summary>
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptOffer([FromBody] AcceptOfferBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new AcceptOfferCommand
        {
            UserId = userId.Value,
            ReaderId = body.ReaderId,
            ConversationRef = body.ConversationRef,
            AmountDiamond = body.AmountDiamond,
            ProposalMessageRef = body.ProposalMessageRef,
            IdempotencyKey = body.IdempotencyKey,
        };

        var itemId = await _mediator.Send(command);
        return Ok(new { success = true, itemId });
    }

    /// <summary>Reader đã trả lời → set auto_release_at = +24h.</summary>
    [HttpPost("reply")]
    public async Task<IActionResult> ReaderReply([FromBody] ReaderReplyBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new ReaderReplyCommand { ItemId = body.ItemId, ReaderId = userId.Value };
        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    /// <summary>User confirm → release diamond cho reader (- 10% fee).</summary>
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmRelease([FromBody] ConfirmReleaseBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new ConfirmReleaseCommand { ItemId = body.ItemId, UserId = userId.Value };
        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    /// <summary>Mở tranh chấp — validate dispute window.</summary>
    [HttpPost("dispute")]
    public async Task<IActionResult> OpenDispute([FromBody] OpenDisputeBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new OpenDisputeCommand
        {
            ItemId = body.ItemId,
            UserId = userId.Value,
            Reason = body.Reason,
        };
        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    /// <summary>Thêm câu hỏi → freeze thêm diamond (cộng dồn).</summary>
    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new AddQuestionCommand
        {
            UserId = userId.Value,
            ConversationRef = body.ConversationRef,
            AmountDiamond = body.AmountDiamond,
            ProposalMessageRef = body.ProposalMessageRef,
            IdempotencyKey = body.IdempotencyKey,
        };

        var itemId = await _mediator.Send(command);
        return Ok(new { success = true, itemId });
    }

    /// <summary>Lấy trạng thái escrow của conversation.</summary>
    [HttpGet("{conversationId}")]
    public async Task<IActionResult> GetStatus(string conversationId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var query = new GetEscrowStatusQuery
        {
            ConversationRef = conversationId,
            RequesterId = userId.Value,
        };

        var result = await _mediator.Send(query);
        if (result == null) return NotFound();
        return Ok(result);
    }
}

// --- Request Bodies ---

public class AcceptOfferBody
{
    public Guid ReaderId { get; set; }
    public string ConversationRef { get; set; } = string.Empty;
    public long AmountDiamond { get; set; }
    public string? ProposalMessageRef { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
}

public class ReaderReplyBody { public Guid ItemId { get; set; } }

public class ConfirmReleaseBody { public Guid ItemId { get; set; } }

public class OpenDisputeBody
{
    public Guid ItemId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class AddQuestionBody
{
    public string ConversationRef { get; set; } = string.Empty;
    public long AmountDiamond { get; set; }
    public string? ProposalMessageRef { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
}

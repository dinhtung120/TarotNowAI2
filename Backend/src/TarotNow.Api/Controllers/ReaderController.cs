using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Features.Reader.Queries.GetReaderProfile;
using TarotNow.Application.Features.Reader.Queries.ListReaders;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý tính năng Reader — thin controller theo Clean Architecture.
///
/// Endpoints:
/// - POST /apply: User gửi đơn xin làm Reader.
/// - GET /my-request: User xem trạng thái đơn của mình.
/// - GET /profile/{userId}: Xem hồ sơ Reader (public).
/// - PATCH /profile: Reader tự cập nhật hồ sơ.
/// - PATCH /status: Reader chuyển đổi online/offline/accepting.
/// - GET /directory: Danh sách Reader công khai (public, phân trang).
///
/// Mọi business logic nằm trong MediatR handlers — controller chỉ:
/// 1. Extract userId từ JWT claims.
/// 2. Map request body/query sang Command/Query.
/// 3. Gửi qua MediatR.
/// 4. Trả response phù hợp.
/// </summary>
[Route("api/v1/reader")]
[ApiController]
public class ReaderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IReaderRequestRepository _readerRequestRepository;

    public ReaderController(IMediator mediator, IReaderRequestRepository readerRequestRepository)
    {
        _mediator = mediator;
        _readerRequestRepository = readerRequestRepository;
    }

    /// <summary>
    /// User gửi đơn xin trở thành Reader.
    ///
    /// Yêu cầu: đã đăng nhập (Authorize), role = user.
    /// Body: { introText: string, proofDocuments?: string[] }
    /// </summary>
    [HttpPost("apply")]
    [Authorize]
    public async Task<IActionResult> Apply([FromBody] SubmitReaderRequestBody body)
    {
        // Lấy userId từ JWT claims — không tin request body
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var command = new SubmitReaderRequestCommand
        {
            UserId = userId,
            IntroText = body.IntroText,
            ProofDocuments = body.ProofDocuments ?? new List<string>()
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true, message = "Đơn đã được gửi thành công. Vui lòng chờ admin duyệt." })
                      : BadRequest(new { message = "Không thể gửi đơn." });
    }

    /// <summary>
    /// User xem trạng thái đơn xin Reader của mình.
    /// Trả về null nếu chưa từng submit đơn.
    /// </summary>
    [HttpGet("my-request")]
    [Authorize]
    public async Task<IActionResult> GetMyRequest()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var request = await _readerRequestRepository.GetLatestByUserIdAsync(userId.ToString());

        if (request == null)
            return Ok(new { hasRequest = false });

        return Ok(new
        {
            hasRequest = true,
            status = request.Status,
            introText = request.IntroText,
            adminNote = request.AdminNote,
            createdAt = request.CreatedAt,
            reviewedAt = request.ReviewedAt
        });
    }

    /// <summary>
    /// Xem hồ sơ công khai Reader.
    /// Public — không cần đăng nhập.
    /// </summary>
    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var query = new GetReaderProfileQuery { UserId = userId };
        var profile = await _mediator.Send(query);

        if (profile == null)
            return NotFound(new { message = "Không tìm thấy hồ sơ Reader." });

        return Ok(profile);
    }

    /// <summary>
    /// Reader tự cập nhật hồ sơ (bio, pricing, specialties).
    /// Yêu cầu: role = tarot_reader.
    /// </summary>
    [HttpPatch("profile")]
    [Authorize(Roles = "tarot_reader")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateReaderProfileBody body)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var command = new UpdateReaderProfileCommand
        {
            UserId = userId,
            BioVi = body.BioVi,
            BioEn = body.BioEn,
            BioZh = body.BioZh,
            DiamondPerQuestion = body.DiamondPerQuestion,
            Specialties = body.Specialties
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest();
    }

    /// <summary>
    /// Reader chuyển đổi trạng thái online.
    /// Yêu cầu: role = tarot_reader.
    /// </summary>
    [HttpPatch("status")]
    [Authorize(Roles = "tarot_reader")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateReaderStatusBody body)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var command = new UpdateReaderStatusCommand
        {
            UserId = userId,
            Status = body.Status
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest();
    }

    /// <summary>
    /// Danh sách Reader công khai (directory listing).
    /// Public — không cần đăng nhập.
    /// Hỗ trợ filter: specialty, status, searchTerm.
    /// </summary>
    [HttpGet("/api/v1/readers")]
    public async Task<IActionResult> ListReaders([FromQuery] ListReadersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

// ======================================================================
// Request Body DTOs — tách riêng để không expose MediatR Command ra API.
// Tại sao dùng DTO thay vì Command trực tiếp?
// → Command có UserId lấy từ JWT, không phải từ body.
// → Tránh mass assignment attack (user gửi UserId giả).
// ======================================================================

/// <summary>Body cho POST /reader/apply</summary>
public class SubmitReaderRequestBody
{
    /// <summary>Lời giới thiệu bản thân.</summary>
    public string IntroText { get; set; } = string.Empty;

    /// <summary>Danh sách URL tài liệu chứng minh.</summary>
    public List<string>? ProofDocuments { get; set; }
}

/// <summary>Body cho PATCH /reader/profile</summary>
public class UpdateReaderProfileBody
{
    public string? BioVi { get; set; }
    public string? BioEn { get; set; }
    public string? BioZh { get; set; }
    public long? DiamondPerQuestion { get; set; }
    public List<string>? Specialties { get; set; }
}

/// <summary>Body cho PATCH /reader/status</summary>
public class UpdateReaderStatusBody
{
    /// <summary>Trạng thái mới: "online" | "offline" | "accepting_questions".</summary>
    public string Status { get; set; } = string.Empty;
}

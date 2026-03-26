/*
 * ===================================================================
 * FILE: ReaderController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý tính năng READER (người đọc bài tarot chuyên nghiệp).
 *
 * READER LÀ AI?
 *   Reader là người dùng đã nộp đơn và được admin phê duyệt để trở thành
 *   "người đọc bài" chuyên nghiệp. Họ có thể:
 *   - Nhận câu hỏi từ user và trả lời (qua chat, có thu phí)
 *   - Tự đặt giá cho mỗi câu hỏi (diamond per question)
 *   - Rút tiền về tài khoản ngân hàng
 *
 * CÁC ENDPOINT:
 *   POST  /apply         → User nộp đơn xin làm reader (cần đăng nhập)
 *   GET   /my-request    → User xem trạng thái đơn (cần đăng nhập)
 *   GET   /profile/{id}  → Xem hồ sơ reader (PUBLIC, không cần đăng nhập)
 *   PATCH /profile       → Reader tự cập nhật hồ sơ (cần role tarot_reader)
 *   PATCH /status        → Reader đổi trạng thái online (cần role tarot_reader)
 *   GET   /api/v1/readers → Danh sách reader (PUBLIC, phân trang)
 *
 * KIẾN TRÚC:
 *   Tuân theo Clean Architecture: controller KHÔNG chứa business logic.
 *   Chỉ: extract userId → map sang Command/Query → gửi MediatR → trả response.
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Import Command/Query cho Reader
using TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest;
using TarotNow.Application.Features.Reader.Queries.GetReaderProfile;
using TarotNow.Application.Features.Reader.Queries.ListReaders;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý tính năng Reader — thin controller theo Clean Architecture.
/// Endpoints có mix giữa public (không cần đăng nhập) và private (cần auth).
/// </summary>
[Route(ApiRoutes.Reader)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
public class ReaderController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReaderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/reader/apply
    /// MỤC ĐÍCH: User nộp đơn xin trở thành Reader.
    ///
    /// LUỒNG:
    ///   1. User điền form: giới thiệu bản thân + tải tài liệu chứng minh
    ///   2. Server tạo document "reader_request" trong MongoDB (trạng thái: pending)
    ///   3. Admin xem và duyệt/từ chối qua AdminController
    ///   4. Nếu được duyệt → user được cấp role "tarot_reader"
    ///
    /// BẢO MẬT: UserId lấy từ JWT, KHÔNG từ body → chống mass assignment attack.
    ///   Mass assignment attack: hacker gửi { userId: "admin-id" } trong body
    ///   để giả mạo đơn của admin. Lấy từ JWT ngăn chặn hoàn toàn.
    /// </summary>
    [HttpPost("apply")]
    [Authorize] // Phải đăng nhập
    public async Task<IActionResult> Apply([FromBody] SubmitReaderRequestBody body)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var command = new SubmitReaderRequestCommand
        {
            UserId = userId,
            IntroText = body.IntroText,
            /*
             * ProofDocuments: danh sách URL tài liệu.
             * "?? new List<string>()": nếu client không gửi → tạo list rỗng
             * thay vì null, tránh NullReferenceException trong handler.
             */
            ProofDocuments = body.ProofDocuments ?? new List<string>()
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true, message = "Đơn đã được gửi thành công. Vui lòng chờ admin duyệt." })
                      : BadRequest(new { message = "Không thể gửi đơn." });
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/reader/my-request
    /// MỤC ĐÍCH: User xem trạng thái đơn xin Reader của mình.
    ///
    /// TRẢ VỀ: Thông tin đơn và trạng thái:
    ///   - pending: đang chờ admin duyệt
    ///   - approved: đã được duyệt
    ///   - rejected: bị từ chối (kèm lý do)
    ///   - null: chưa từng gửi đơn
    /// </summary>
    [HttpGet("my-request")]
    [Authorize]
    public async Task<IActionResult> GetMyRequest()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetMyReaderRequestQuery { UserId = userId });
        return Ok(result); // null nếu chưa gửi đơn
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/reader/profile/{userId}
    /// MỤC ĐÍCH: Xem hồ sơ công khai của một Reader.
    ///
    /// PUBLIC: KHÔNG cần [Authorize] → ai cũng xem được.
    ///   User xem profile reader trước khi quyết định đặt câu hỏi.
    ///
    /// TRẢ VỀ: bio (giới thiệu), specialties (chuyên môn),
    ///   diamondPerQuestion (giá), rating, trạng thái online.
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
    /// ENDPOINT: PATCH /api/v1/reader/profile
    /// MỤC ĐÍCH: Reader tự cập nhật hồ sơ của mình.
    ///
    /// [Authorize(Roles = "tarot_reader")]: CHỈ user có role "tarot_reader"
    ///   mới gọi được. User thường (chưa được duyệt) sẽ bị 403 Forbidden.
    ///
    /// CÁC TRƯỜNG CÓ THỂ CẬP NHẬT:
    ///   - BioVi/BioEn/BioZh: giới thiệu bản thân (đa ngôn ngữ VI/EN/ZH)
    ///   - DiamondPerQuestion: giá cho mỗi câu hỏi
    ///   - Specialties: danh sách chuyên môn (["tình yêu", "sự nghiệp"])
    /// </summary>
    [HttpPatch("profile")]
    [Authorize(Roles = "tarot_reader")] // Chỉ reader đã được duyệt
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateReaderProfileBody body)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var command = new UpdateReaderProfileCommand
        {
            UserId = userId,
            BioVi = body.BioVi,                       // Giới thiệu tiếng Việt
            BioEn = body.BioEn,                       // Giới thiệu tiếng Anh
            BioZh = body.BioZh,                       // Giới thiệu tiếng Trung
            DiamondPerQuestion = body.DiamondPerQuestion, // Giá mỗi câu hỏi
            Specialties = body.Specialties             // Chuyên môn
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest();
    }

    /// <summary>
    /// ENDPOINT: PATCH /api/v1/reader/status
    /// MỤC ĐÍCH: Reader chuyển đổi trạng thái hoạt động.
    ///
    /// CÁC TRẠNG THÁI:
    ///   - "online": đang trực tuyến, sẵn sàng nhận câu hỏi
    ///   - "offline": không hoạt động
    ///   - "accepting_questions": đang trực tuyến VÀ chủ động nhận câu hỏi mới
    ///
    /// [Authorize(Roles = "tarot_reader")]: chỉ reader.
    /// </summary>
    [HttpPatch("status")]
    [Authorize(Roles = "tarot_reader")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateReaderStatusBody body)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var command = new UpdateReaderStatusCommand
        {
            UserId = userId,
            Status = body.Status // "online" | "offline" | "accepting_questions"
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest();
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/readers
    /// MỤC ĐÍCH: Danh sách Reader công khai (Directory listing).
    ///
    /// PUBLIC: KHÔNG cần đăng nhập → user duyệt danh sách reader trước khi chọn.
    ///
    /// LƯU Ý ROUTE ĐẶC BIỆT:
    ///   URL = "/api/v1/readers" (số nhiều, có "s")
    ///   KHÁC với route gốc "/api/v1/reader" (số ít)
    ///   Đây là quy ước REST: danh sách resource dùng số nhiều (readers),
    ///   thao tác trên 1 resource dùng số ít (reader/apply, reader/profile).
    ///
    /// HỖ TRỢ LỌC: specialty (chuyên môn), status (online/offline), search text.
    /// </summary>
    [HttpGet(ApiRoutes.ReadersAbsolute)] // Route tuyệt đối (override route class-level)
    public async Task<IActionResult> ListReaders([FromQuery] ListReadersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

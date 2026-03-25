/*
 * ===================================================================
 * FILE: AdminController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Đây là "bộ điều khiển" (Controller) dành riêng cho QUẢN TRỊ VIÊN (Admin).
 *   Nó cung cấp các endpoint (đường dẫn API) để admin thực hiện:
 *   1. Đối soát ví (reconciliation) - so sánh số dư thực tế và sổ cái
 *   2. Quản lý người dùng (xem danh sách, khóa/mở tài khoản)
 *   3. Quản lý nạp tiền (xem danh sách, xử lý đơn nạp)
 *   4. Cộng tiền thủ công cho người dùng
 *   5. Phê duyệt/từ chối đơn xin làm Reader
 *   6. Xử lý tranh chấp (dispute) trong giao dịch escrow
 *   7. Quản lý yêu cầu rút tiền
 *
 * KIẾN TRÚC:
 *   Controller này tuân theo nguyên tắc "Thin Controller" (controller mỏng):
 *   - Nó KHÔNG chứa logic nghiệp vụ phức tạp
 *   - Chỉ làm 3 việc: nhận request → gửi cho handler xử lý → trả response
 *   - Toàn bộ logic được ủy thác cho Application layer thông qua MediatR
 *
 * BẢO MẬT:
 *   - Attribute [Authorize(Roles = "admin")] đảm bảo CHỈ user có role "admin"
 *     mới truy cập được các endpoint trong controller này.
 *   - AdminId luôn lấy từ JWT token (không cho client tự gửi) để chống giả mạo.
 * ===================================================================
 */

// Các "using" bên dưới là khai báo import thư viện/namespace cần dùng
// Giống như "nhập khẩu công cụ" trước khi làm việc.
using MediatR; // MediatR: thư viện trung gian giúp gửi Command/Query đến Handler xử lý
using Microsoft.AspNetCore.Authorization; // Chứa [Authorize] để kiểm soát quyền truy cập
using Microsoft.AspNetCore.Mvc; // Chứa ControllerBase, [HttpGet], [FromBody]... - nền tảng xây API
using System.Security.Claims; // Chứa ClaimTypes để đọc thông tin từ JWT token (ai đang đăng nhập)
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch; // Import query đối soát sổ cái

// Khai báo namespace - "địa chỉ" logic của file này trong cấu trúc dự án
namespace TarotNow.Api.Controllers;

/*
 * [Route("api/v1/[controller]")]:
 *   - Định nghĩa đường dẫn URL gốc cho TẤT CẢ endpoint trong controller này.
 *   - [controller] sẽ tự động thay bằng tên class bỏ hậu tố "Controller" → "Admin"
 *   - Kết quả: URL gốc = "api/v1/Admin"
 *   - "v1" là phiên bản API, giúp sau này nâng cấp lên v2 mà không phá hỏng v1.
 *
 * [ApiController]:
 *   - Bật các tính năng mặc định cho API controller:
 *     + Tự động validate model (kiểm tra dữ liệu đầu vào)
 *     + Tự động trả lỗi 400 khi model không hợp lệ
 *     + Tự động đọc [FromBody] từ JSON request body
 *
 * [Authorize(Roles = "admin")]:
 *   - BẢO MẬT QUAN TRỌNG: Chỉ cho phép người dùng có vai trò "admin" truy cập.
 *   - Nếu user thường (role = "user") gọi bất kỳ endpoint nào → bị từ chối (403 Forbidden).
 *   - Thông tin role được lưu trong JWT token khi user đăng nhập.
 */
[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Roles = "admin")] // Chỉ dành cho Admin
public class AdminController : ControllerBase
{
    /*
     * _mediator: Đối tượng trung gian MediatR.
     * - Thay vì controller GỌI TRỰC TIẾP database hay service,
     *   nó gửi "yêu cầu" (Command/Query) qua _mediator.
     * - _mediator tìm đúng Handler đã đăng ký và chuyển yêu cầu đến đó.
     * - Ưu điểm: controller không cần biết ai xử lý, dễ test, dễ thay đổi logic.
     * - "readonly" nghĩa là biến này chỉ được gán 1 lần trong constructor, không đổi sau đó.
     */
    private readonly IMediator _mediator;

    /*
     * Constructor (hàm khởi tạo):
     * - Được gọi tự động khi có request đến controller.
     * - Tham số "mediator" được DI Container tự động truyền vào (Dependency Injection).
     * - DI Container là "nhà máy" quản lý việc tạo và cung cấp các đối tượng cần thiết.
     */
    public AdminController(IMediator mediator)
    {
        _mediator = mediator; // Lưu lại mediator để dùng trong các method bên dưới
    }

    /*
     * ENDPOINT: GET api/v1/Admin/reconciliation/wallet
     * MỤC ĐÍCH: Đối soát ví - kiểm tra xem số dư ví có khớp với tổng giao dịch trong sổ cái không.
     * 
     * TẠI SAO CẦN ĐỐI SOÁT?
     *   Trong hệ thống tài chính, có thể xảy ra sai lệch do lỗi phần mềm, crash giữa chừng,
     *   hoặc giao dịch bị treo. Đối soát giúp phát hiện và sửa chữa kịp thời.
     *
     * [HttpGet]: Chỉ chấp nhận HTTP GET method (yêu cầu ĐỌC dữ liệu, không thay đổi gì).
     * Task<IActionResult>: Hàm bất đồng bộ (async), trả về kết quả HTTP linh hoạt.
     */
    [HttpGet("reconciliation/wallet")]
    public async Task<IActionResult> GetWalletMismatches()
    {
        // Tạo đối tượng Query (câu hỏi) - "Hãy cho tôi danh sách sai lệch sổ cái"
        var query = new GetLedgerMismatchQuery();

        // Gửi query qua MediatR → Handler nhận và truy vấn database → trả kết quả
        var result = await _mediator.Send(query);

        // Ok(result): Trả về HTTP 200 (thành công) kèm dữ liệu kết quả dưới dạng JSON
        return Ok(result);
    }

    /*
     * ENDPOINT: GET api/v1/Admin/users?page=1&pageSize=20
     * MỤC ĐÍCH: Lấy danh sách người dùng, có hỗ trợ phân trang.
     *
     * [FromQuery]: Tham số được lấy từ URL query string (phần sau dấu "?").
     *   Ví dụ: /api/v1/Admin/users?page=2&pageSize=10
     *   → query.Page = 2, query.PageSize = 10
     */
    [HttpGet("users")]
    public async Task<IActionResult> ListUsers([FromQuery] TarotNow.Application.Features.Admin.Queries.ListUsers.ListUsersQuery query)
    {
        var result = await _mediator.Send(query); // Gửi query đến handler
        return Ok(result); // Trả danh sách user dạng JSON
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] TarotNow.Application.Features.Admin.Commands.CreateUser.CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    /*
     * ENDPOINT: PATCH api/v1/Admin/users/lock
     * MỤC ĐÍCH: Khóa hoặc mở khóa tài khoản người dùng.
     *
     * [HttpPatch]: HTTP PATCH method - dùng cho cập nhật MỘT PHẦN dữ liệu.
     *   (PATCH khác PUT: PUT thay THẾ TOÀN BỘ, PATCH chỉ sửa một vài trường)
     *
     * [FromBody]: Tham số được đọc từ body của HTTP request (phần JSON gửi kèm).
     *   Ví dụ body: { "userId": "abc-123", "isLocked": true }
     */
    [HttpPatch("users/lock")]
    public async Task<IActionResult> ToggleUserLock([FromBody] TarotNow.Application.Features.Admin.Commands.ToggleUserLock.ToggleUserLockCommand command)
    {
        // Gửi command "khóa/mở khóa user" qua MediatR
        var success = await _mediator.Send(command);

        // Nếu thành công → 200 OK, nếu thất bại → 400 Bad Request
        return success ? Ok() : BadRequest();
    }

    /*
     * ENDPOINT: PUT api/v1/Admin/users/{id}
     * MỤC ĐÍCH: Admin sửa đổi thông tin toàn diện của user (Role, Status, Balance).
     */
    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] TarotNow.Application.Features.Admin.Commands.UpdateUser.UpdateUserCommand command)
    {
        command.UserId = id;

        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            var headerKey = Request.Headers["X-Idempotency-Key"].ToString();
            command.IdempotencyKey = !string.IsNullOrWhiteSpace(headerKey) ? headerKey : Guid.NewGuid().ToString();
        }

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể cập nhật User." });
    }

    /*
     * ENDPOINT: GET api/v1/Admin/deposits
     * MỤC ĐÍCH: Lấy danh sách tất cả đơn nạp tiền (admin xem để duyệt hoặc theo dõi).
     */
    [HttpGet("deposits")]
    public async Task<IActionResult> ListDeposits([FromQuery] TarotNow.Application.Features.Admin.Queries.ListDeposits.ListDepositsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /*
     * ENDPOINT: POST api/v1/Admin/users/add-balance
     * MỤC ĐÍCH: Admin cộng tiền (diamond) thủ công cho người dùng.
     *   Dùng khi cần bồi thường, tặng thưởng, hay xử lý sự cố.
     *
     * IDEMPOTENCY KEY (khóa chống trùng lặp):
     *   Nếu admin ấn nút 2 lần do mạng chậm, server sẽ kiểm tra IdempotencyKey:
     *   - Lần 1: key = "abc123" → xử lý bình thường
     *   - Lần 2: key = "abc123" (trùng) → bỏ qua, tránh cộng tiền 2 lần
     *   Key có thể gửi trong body hoặc qua HTTP header "X-Idempotency-Key".
     */
    [HttpPost("users/add-balance")]
    public async Task<IActionResult> AddUserBalance([FromBody] TarotNow.Application.Features.Admin.Commands.AddUserBalance.AddUserBalanceCommand command)
    {
        // Nếu client chưa gửi IdempotencyKey trong body, thử lấy từ HTTP header
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            command.IdempotencyKey = Request.Headers["X-Idempotency-Key"].ToString();
        }

        // Gửi command cộng tiền và chờ kết quả
        var result = await _mediator.Send(command);

        // Trả về kết quả: thành công hoặc thất bại kèm thông báo
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể cộng tiền cho người dùng này." });
    }

    /*
     * ENDPOINT: PATCH api/v1/Admin/deposits/process
     * MỤC ĐÍCH: Admin xử lý (duyệt) một đơn nạp tiền.
     *   Sau khi xác nhận user đã chuyển khoản, admin duyệt đơn
     *   → hệ thống cộng diamond vào ví user.
     */
    [HttpPatch("deposits/process")]
    public async Task<IActionResult> ProcessDeposit([FromBody] TarotNow.Application.Features.Admin.Commands.ProcessDeposit.ProcessDepositCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể xử lý đơn nạp tiền này." });
    }

    // ======================================================================
    // Phase 2.1 — Quản lý đơn xin làm Reader (người đọc bài tarot)
    // ======================================================================

    /// <summary>
    /// ENDPOINT: GET api/v1/Admin/reader-requests
    /// MỤC ĐÍCH: Lấy danh sách đơn xin làm Reader có phân trang.
    /// Admin dùng trang này để xem ai đã nộp đơn, trạng thái đang chờ/đã duyệt/bị từ chối.
    /// Hỗ trợ filter theo status: pending (đang chờ), approved (đã duyệt), rejected (bị từ chối).
    /// </summary>
    [HttpGet("reader-requests")]
    public async Task<IActionResult> ListReaderRequests(
        [FromQuery] TarotNow.Application.Features.Admin.Queries.ListReaderRequests.ListReaderRequestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: PATCH api/v1/Admin/reader-requests/process
    /// MỤC ĐÍCH: Admin phê duyệt hoặc từ chối đơn xin Reader.
    ///
    /// CÁCH HOẠT ĐỘNG:
    ///   1. Client (trang admin) gửi body: { requestId: "...", action: "approve", adminNote: "..." }
    ///   2. Server lấy AdminId từ JWT token (KHÔNG cho client tự gửi AdminId → chống giả mạo)
    ///   3. Gửi command đến handler để cập nhật trạng thái đơn trong database
    ///
    /// ĐẶC BIỆT:
    ///   AdminId KHÔNG nằm trong body request mà được trích xuất từ JWT claims.
    ///   Điều này đảm bảo bảo mật - admin không thể giả mạo danh tính admin khác.
    /// </summary>
    [HttpPatch("reader-requests/process")]
    public async Task<IActionResult> ProcessReaderRequest([FromBody] ProcessReaderRequestBody body)
    {
        /*
         * User.FindFirstValue(ClaimTypes.NameIdentifier):
         *   - "User" là thông tin người đang đăng nhập, được giải mã từ JWT token.
         *   - ClaimTypes.NameIdentifier là "claim" chứa ID của user.
         *   - "Claim" giống như thẻ thông tin trên CMND: tên, tuổi, số ID...
         *     JWT token chứa nhiều claim như vậy.
         */
        var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Kiểm tra: nếu không có adminId hoặc không phải GUID hợp lệ → trả 401 Unauthorized
        if (string.IsNullOrEmpty(adminIdString) || !Guid.TryParse(adminIdString, out var adminId))
            return Unauthorized();

        // Tạo command với AdminId lấy từ JWT (an toàn), không từ body (không an toàn)
        var command = new TarotNow.Application.Features.Admin.Commands.ApproveReader.ApproveReaderCommand
        {
            RequestId = body.RequestId,   // ID đơn xin Reader (từ MongoDB)
            Action = body.Action,          // "approve" hoặc "reject"
            AdminNote = body.AdminNote,    // Ghi chú của admin (lý do duyệt/từ chối)
            AdminId = adminId              // ID admin từ JWT token (bảo mật)
        };

        // Gửi command qua MediatR và chờ kết quả
        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể xử lý đơn xin Reader." });
    }

    /// <summary>
    /// ENDPOINT: POST api/v1/Admin/escrow/resolve-dispute
    /// MỤC ĐÍCH: Admin xử lý tranh chấp (dispute) trong giao dịch escrow.
    ///
    /// ESCROW LÀ GÌ?
    ///   Khi user trả tiền cho reader để đọc bài, tiền được "giữ tạm" (escrow)
    ///   chứ không chuyển thẳng cho reader. Sau khi reader trả lời xong và user hài lòng,
    ///   tiền mới được "giải phóng" cho reader.
    ///   Nếu có tranh chấp (user không hài lòng), admin can thiệp:
    ///   - "release": xác nhận reader đã làm tốt → chuyển tiền cho reader
    ///   - "refund": xác nhận user đúng → hoàn tiền cho user
    /// </summary>
    [HttpPost("escrow/resolve-dispute")]
    public async Task<IActionResult> ResolveDispute([FromBody] ResolveDisputeBody body)
    {
        // Lấy AdminId từ JWT và kiểm tra hợp lệ
        var adminId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (Guid?)null;
        if (adminId == null) return Unauthorized();

        // Tạo command xử lý tranh chấp
        var command = new TarotNow.Application.Features.Admin.Commands.ResolveDispute.ResolveDisputeCommand
        {
            ItemId = body.ItemId,         // ID của câu hỏi đang bị tranh chấp
            Action = body.Action,          // "release" hoặc "refund"
            AdminNote = body.AdminNote,    // Ghi chú lý do quyết định
            AdminId = adminId.Value        // ID admin xử lý
        };

        await _mediator.Send(command);

        // Trả về kèm action đã thực hiện (đã trim và lowercase để chuẩn hóa)
        return Ok(new { success = true, action = body.Action?.Trim().ToLowerInvariant() });
    }

    // ======================================================================
    // Phase 2.4 — Quản lý yêu cầu rút tiền (Withdrawal)
    // ======================================================================

    /// <summary>
    /// ENDPOINT: GET api/v1/Admin/withdrawals/queue?page=1&pageSize=20
    /// MỤC ĐÍCH: Admin lấy danh sách yêu cầu rút tiền đang chờ duyệt (pending).
    /// Phân trang để tránh tải quá nhiều dữ liệu cùng lúc.
    /// </summary>
    [HttpGet("withdrawals/queue")]
    public async Task<IActionResult> WithdrawalQueue([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        // Tạo query chỉ lấy các yêu cầu đang chờ (PendingOnly = true)
        var query = new TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals.ListWithdrawalsQuery
        {
            PendingOnly = true,  // Chỉ lấy đơn đang chờ duyệt, không lấy đã xử lý
            Page = page,         // Trang hiện tại (mặc định = 1)
            PageSize = pageSize, // Số đơn mỗi trang (mặc định = 20)
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: POST api/v1/Admin/withdrawals/process
    /// MỤC ĐÍCH: Admin duyệt (approve) hoặc từ chối (reject) yêu cầu rút tiền.
    ///
    /// QUAN TRỌNG: Cần MFA code (mã xác thực 2 lớp) để đảm bảo
    /// chỉ admin thật sự mới có thể duyệt rút tiền (chống hack tài khoản admin).
    /// </summary>
    [HttpPost("withdrawals/process")]
    public async Task<IActionResult> ProcessWithdrawal([FromBody] ProcessWithdrawalBody body)
    {
        // Lấy AdminId từ JWT token
        var adminId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (Guid?)null;
        if (adminId == null) return Unauthorized();

        // Tạo command xử lý rút tiền
        var command = new TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal.ProcessWithdrawalCommand
        {
            RequestId = body.WithdrawalId, // ID yêu cầu rút tiền
            AdminId = adminId.Value,       // Admin đang xử lý (từ JWT)
            Action = body.Action,           // "approve" hoặc "reject"
            AdminNote = body.AdminNote,     // Ghi chú (lý do từ chối, v.v.)
            MfaCode = body.MfaCode,         // Mã xác thực 2 lớp (TOTP code)
        };

        await _mediator.Send(command);
        return Ok(new { success = true, action = body.Action });
    }
}

/*
 * ===================================================================
 * CÁC CLASS DTO BỔ SUNG (Data Transfer Object)
 * ===================================================================
 * Các class bên dưới là "khuôn mẫu" cho dữ liệu mà client gửi lên
 * trong body của HTTP request. Chúng được tách riêng khỏi Command
 * để AdminId luôn được lấy từ JWT token → bảo mật hơn.
 * ===================================================================
 */

/// <summary>
/// DTO cho endpoint PATCH /admin/reader-requests/process.
/// Chứa dữ liệu client gửi lên khi admin xử lý đơn xin Reader.
/// AdminId KHÔNG có trong class này vì lấy từ JWT (chống giả mạo).
/// </summary>
public class ProcessReaderRequestBody
{
    /// <summary>
    /// ObjectId (dạng string) của document "reader_requests" trong MongoDB.
    /// Mỗi đơn xin Reader có một ID duy nhất trong MongoDB.
    /// </summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Hành động admin muốn thực hiện: "approve" (duyệt) hoặc "reject" (từ chối).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú của admin (tùy chọn - không bắt buộc).
    /// Ví dụ: "Hồ sơ đầy đủ, kinh nghiệm 5 năm" hoặc "Thiếu chứng chỉ".
    /// Dấu "?" nghĩa là có thể null (không gửi cũng được).
    /// </summary>
    public string? AdminNote { get; set; }
}

/// <summary>
/// DTO cho endpoint POST /admin/escrow/resolve-dispute.
/// Chứa dữ liệu khi admin xử lý tranh chấp giữa user và reader.
/// </summary>
public class ResolveDisputeBody
{
    /// <summary>
    /// UUID (Universally Unique Identifier) của item đang bị tranh chấp.
    /// Đây là ID của bản ghi "chat_question_items" trong database PostgreSQL.
    /// Guid là kiểu dữ liệu 128-bit đảm bảo không trùng lặp trên toàn thế giới.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Hành động xử lý tranh chấp:
    /// - "release": reader đúng → chuyển tiền escrow cho reader
    /// - "refund": user đúng → hoàn tiền lại cho user
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú admin giải thích lý do quyết định (tùy chọn).
    /// Được lưu lại để audit (kiểm toán) sau này.
    /// </summary>
    public string? AdminNote { get; set; }
}

/// <summary>
/// DTO cho endpoint POST /admin/withdrawals/process.
/// Chứa dữ liệu khi admin duyệt/từ chối yêu cầu rút tiền.
/// </summary>
public class ProcessWithdrawalBody
{
    /// <summary>
    /// ID của yêu cầu rút tiền cần xử lý.
    /// </summary>
    public Guid WithdrawalId { get; set; }

    /// <summary>
    /// Hành động: "approve" (duyệt - chuyển tiền) hoặc "reject" (từ chối).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú admin (tùy chọn).
    /// Ví dụ lý do từ chối: "Thông tin ngân hàng không hợp lệ".
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// Mã xác thực 2 lớp (MFA/TOTP code).
    /// BẮT BUỘC khi duyệt rút tiền để đảm bảo an toàn tài chính.
    /// Đây là mã 6 số từ app Google Authenticator hoặc tương tự.
    /// </summary>
    public string MfaCode { get; set; } = string.Empty;
}

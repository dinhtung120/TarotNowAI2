/*
 * ===================================================================
 * FILE: ConversationController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý CÁC CUỘC TRÒ CHUYỆN (conversations) giữa
 *   người dùng (user) và người đọc bài (reader).
 *
 * PHÂN BIỆT VỚI SIGNALR:
 *   - Controller này xử lý các thao tác NON-REALTIME (không cần thời gian thực):
 *     + Tạo cuộc trò chuyện mới
 *     + Lấy danh sách hội thoại (inbox)
 *     + Lấy lịch sử tin nhắn
 *   - SignalR Hub (ChatHub.cs) xử lý REALTIME:
 *     + Gửi/nhận tin nhắn tức thì
 *     + Thông báo "đang gõ..."
 *     + Cập nhật trạng thái online
 *
 * TẠI SAO TÁCH RA?
 *   REST API (Controller) phù hợp cho: đọc dữ liệu, tạo/cập nhật resource.
 *   WebSocket/SignalR phù hợp cho: giao tiếp 2 chiều, tức thì, liên tục.
 *   Kết hợp cả hai cho trải nghiệm chat tốt nhất.
 * ===================================================================
 */

using MediatR;                 // MediatR: gửi Command/Query đến Handler
using Microsoft.AspNetCore.Authorization; // [Authorize] kiểm soát quyền
using Microsoft.AspNetCore.Mvc; // Nền tảng API controller
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

// Import các Command/Query cho chat
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
/*
 * [Route(ApiRoutes.Conversations)]: URL gốc cho tất cả endpoint trong controller.
 * [Authorize]: Tất cả endpoint yêu cầu đăng nhập (có JWT token hợp lệ).
 */
[Route(ApiRoutes.Conversations)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
public class ConversationController : ControllerBase
{
    // MediatR mediator để gửi commands/queries
    private readonly IMediator _mediator;

    public ConversationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/conversations
    /// MỤC ĐÍCH: Tạo cuộc trò chuyện mới giữa user hiện tại và một reader.
    ///
    /// TÍNH IDEMPOTENT:
    ///   Nếu đã có conversation đang active (chưa kết thúc) giữa 2 người này,
    ///   server sẽ TRẢ VỀ conversation đó thay vì tạo mới.
    ///   Điều này ngăn việc tạo trùng lặp khi:
    ///   - User ấn nút 2 lần do mạng chậm
    ///   - User quay lại trang chat với cùng reader
    ///   
    ///   "Idempotent" = gọi nhiều lần cho cùng kết quả, không tạo side effect thêm.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConversationBody body)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        // Tạo command tạo conversation
        var command = new CreateConversationCommand
        {
            UserId = userId,        // AI dùng userId từ JWT (bảo mật)
            ReaderId = body.ReaderId // Reader ID mà user muốn chat (từ body)
        };

        // Gửi command → handler kiểm tra có conversation sẵn không → tạo/trả về
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/conversations?page=1&amp;pageSize=20&amp;role=user
    /// MỤC ĐÍCH: Lấy danh sách conversations (inbox) của người dùng hiện tại.
    ///
    /// HỖ TRỢ CẢ 2 VAI TRÒ:
    ///   - role="user": hiển thị inbox khi đang dùng với tư cách người dùng
    ///   - role="reader": hiển thị inbox khi đang dùng với tư cách người đọc bài
    ///   
    ///   Một người có thể VỪA là user VỪA là reader, nên cần phân biệt vai trò.
    ///   
    /// PHÂN TRANG:
    ///   - page: trang hiện tại (mặc định 1)
    ///   - pageSize: số conversation mỗi trang (mặc định 20)
    ///   Phân trang giúp không tải quá nhiều dữ liệu cùng lúc → app nhanh hơn.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string role = "user")
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        // Tạo query với các tham số phân trang và vai trò
        var query = new ListConversationsQuery
        {
            UserId = userId,
            Role = role,         // "user" hoặc "reader"
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/conversations/{id}/messages?page=1&amp;pageSize=50
    /// MỤC ĐÍCH: Lấy lịch sử tin nhắn trong một cuộc trò chuyện cụ thể.
    ///
    /// THỨ TỰ SẮP XẾP:
    ///   Tin nhắn được sắp xếp theo thời gian GIẢM DẦN (mới nhất trước).
    ///   Frontend sẽ đảo ngược (reverse) để hiển thị tin cũ ở trên, tin mới ở dưới
    ///   (giống giao diện chat WhatsApp/Messenger).
    ///   
    ///   Lý do sort DESC ở server: khi phân trang, trang 1 lấy tin MỚI NHẤT
    ///   (user thường muốn xem tin gần đây, không phải tin từ tuần trước).
    ///
    /// BẢO MẬT:
    ///   Handler sẽ kiểm tra RequesterId có thuộc conversation này không.
    ///   User A không thể đọc tin nhắn của conversation giữa User B và Reader C.
    /// </summary>
    [HttpGet("{id}/messages")]
    public async Task<IActionResult> Messages(
        string id,                       // id của conversation (từ URL path)
        [FromQuery] int page = 1,        // trang (mặc định 1)
        [FromQuery] int pageSize = 50)   // số tin mỗi trang (mặc định 50, nhiều hơn vì cần ngữ cảnh)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var query = new ListMessagesQuery
        {
            ConversationId = id,      // ID conversation cần lấy tin nhắn
            RequesterId = userId,     // ID người yêu cầu (để kiểm tra quyền truy cập)
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

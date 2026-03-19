/*
 * ===================================================================
 * FILE: EscrowController.cs 
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý hệ thống ESCROW (giữ tiền trung gian) cho giao dịch
 *   giữa người dùng (user) và người đọc bài (reader).
 *
 * ESCROW LÀ GÌ? (Giải thích đơn giản)
 *   Giống như khi bạn mua hàng trên Shopee:
 *   1. Bạn trả tiền → Shopee GIỮ tiền (không chuyển thẳng cho người bán)
 *   2. Người bán giao hàng
 *   3. Bạn xác nhận đã nhận → Shopee MỚI chuyển tiền cho người bán
 *   4. Nếu có vấn đề → Shopee hoàn tiền cho bạn
 *
 *   Trong TarotNow:
 *   1. User trả diamond → Hệ thống ĐÓNG BĂNG diamond (freeze)
 *   2. Reader trả lời câu hỏi
 *   3. User hài lòng → xác nhận → Diamond được GIẢI PHÓNG cho Reader (trừ 10% phí)
 *   4. Không hài lòng → mở tranh chấp → Admin xử lý
 *
 * CÁC ENDPOINT:
 *   POST /accept       - User chấp nhận đề nghị → đóng băng diamond
 *   POST /reply        - Reader đã trả lời → bắt đầu đếm ngược 24h tự giải phóng
 *   POST /confirm      - User xác nhận hài lòng → giải phóng diamond cho reader
 *   POST /dispute      - User mở tranh chấp → admin can thiệp
 *   POST /add-question - User thêm câu hỏi → đóng băng thêm diamond
 *   GET  /{id}         - Xem trạng thái escrow hiện tại
 * ===================================================================
 */

using MediatR;                 // MediatR trung gian gửi Command/Query
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền truy cập
using Microsoft.AspNetCore.Mvc; // API controller base
using System.Security.Claims;   // Đọc thông tin user từ JWT

// Import tất cả Command/Query cho Escrow
using TarotNow.Application.Features.Escrow.Commands.AcceptOffer;    // Chấp nhận đề nghị
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;    // Thêm câu hỏi
using TarotNow.Application.Features.Escrow.Commands.ConfirmRelease; // Xác nhận giải phóng
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;    // Mở tranh chấp
using TarotNow.Application.Features.Escrow.Commands.ReaderReply;    // Reader trả lời
using TarotNow.Application.Features.Escrow.Queries.GetEscrowStatus; // Xem trạng thái

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý Escrow — freeze/release/refund, dispute, add-question.
/// Tất cả endpoints yêu cầu authentication (JWT).
/// Wallet operations đi qua stored procedures (ACID transactions)
/// để đảm bảo tính nguyên tử - không thể bị gián đoạn giữa chừng.
/// </summary>
[Route("api/v1/escrow")]
[ApiController]
[Authorize] // Tất cả endpoint yêu cầu đăng nhập
public class EscrowController : ControllerBase
{
    private readonly IMediator _mediator;

    /*
     * Constructor dùng "expression body" (=>):
     * Cú pháp ngắn gọn thay cho { _mediator = mediator; }
     * Kết quả hoàn toàn giống nhau, chỉ viết gọn hơn.
     */
    public EscrowController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Helper method dùng chung: lấy UserId từ JWT token.
    /// Trả về null nếu JWT không hợp lệ (user chưa đăng nhập hoặc token hết hạn).
    /// 
    /// Guid? (có dấu ?): kiểu nullable - có thể có giá trị hoặc null.
    /// Dùng null để biểu thị "không thành công" thay vì throw exception.
    /// </summary>
    private Guid? GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(str, out var id) ? id : null;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/escrow/accept
    /// MỤC ĐÍCH: User chấp nhận đề nghị (offer) từ reader.
    ///
    /// KHI GỌI ENDPOINT NÀY:
    ///   1. Hệ thống kiểm tra user có đủ diamond không
    ///   2. ĐÓNG BĂNG (freeze) số diamond vào escrow
    ///      → Diamond bị trừ khỏi "số dư khả dụng" nhưng chưa mất hẳn
    ///   3. Tạo ChatFinanceSession (phiên giao dịch) trong database
    ///   4. Tạo ChatQuestionItem (item câu hỏi) trạng thái "pending"
    ///
    /// TÍNH IDEMPOTENT:
    ///   Gọi 2 lần với cùng IdempotencyKey → KHÔNG tạo 2 item,
    ///   mà trả về item đã tạo từ lần 1. An toàn khi mạng chập chờn.
    /// </summary>
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptOffer([FromBody] AcceptOfferBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new AcceptOfferCommand
        {
            UserId = userId.Value,                      // User chấp nhận (từ JWT)
            ReaderId = body.ReaderId,                    // Reader được chấp nhận
            ConversationRef = body.ConversationRef,      // ID cuộc trò chuyện liên quan
            AmountDiamond = body.AmountDiamond,          // Số diamond cần đóng băng
            ProposalMessageRef = body.ProposalMessageRef, // ID tin nhắn đề nghị gốc
            IdempotencyKey = body.IdempotencyKey,         // Key chống trùng lặp
        };

        // Handler trả về itemId (UUID) của ChatQuestionItem vừa tạo
        var itemId = await _mediator.Send(command);
        return Ok(new { success = true, itemId });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/escrow/reply
    /// MỤC ĐÍCH: Reader đánh dấu đã trả lời câu hỏi.
    ///
    /// KHI GỌI ENDPOINT NÀY:
    ///   - Trạng thái item chuyển từ "Pending" → "Answered"
    ///   - Bắt đầu đếm ngược 24 giờ (auto_release_at = now + 24h)
    ///   - Sau 24h nếu user không xác nhận hoặc dispute → tự động release cho reader
    ///   
    ///   Đếm ngược 24h là cơ chế BẢO VỆ READER:
    ///   Nếu user bỏ mặc không xác nhận → reader vẫn nhận được tiền.
    /// </summary>
    [HttpPost("reply")]
    public async Task<IActionResult> ReaderReply([FromBody] ReaderReplyBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // userId.Value ở đây chính là ReaderId (reader đang đăng nhập và gọi endpoint)
        var command = new ReaderReplyCommand { ItemId = body.ItemId, ReaderId = userId.Value };
        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/escrow/confirm
    /// MỤC ĐÍCH: User xác nhận hài lòng với câu trả lời → giải phóng diamond cho reader.
    ///
    /// KHI GỌI ENDPOINT NÀY:
    ///   1. Diamond escrow được GIẢI PHÓNG (unfreeze)
    ///   2. TRỪ 10% PHÍ nền tảng (platform fee) → vào ví hệ thống
    ///   3. 90% CÒN LẠI → chuyển vào ví reader
    ///   4. Trạng thái item → "Released"
    ///   
    ///   Ví dụ: Escrow 100 diamond
    ///   → Reader nhận: 90 diamond
    ///   → Platform fee: 10 diamond
    /// </summary>
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmRelease([FromBody] ConfirmReleaseBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new ConfirmReleaseCommand { ItemId = body.ItemId, UserId = userId.Value };
        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/escrow/dispute
    /// MỤC ĐÍCH: User mở tranh chấp khi không hài lòng với câu trả lời.
    ///
    /// ĐIỀU KIỆN:
    ///   - Chỉ mở được trong "dispute window" (cửa sổ tranh chấp):
    ///     từ khi reader trả lời đến trước khi hết 24h tự giải phóng.
    ///   - Sau khi dispute: tiền bị giữ lại, admin sẽ xem xét và quyết định
    ///     release cho reader hoặc refund cho user.
    /// </summary>
    [HttpPost("dispute")]
    public async Task<IActionResult> OpenDispute([FromBody] OpenDisputeBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new OpenDisputeCommand
        {
            ItemId = body.ItemId,     // Item đang tranh chấp
            UserId = userId.Value,     // User mở tranh chấp (từ JWT)
            Reason = body.Reason,      // Lý do tranh chấp (bắt buộc)
        };
        await _mediator.Send(command);
        return Ok(new { success = true });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/escrow/add-question
    /// MỤC ĐÍCH: User thêm câu hỏi mới → đóng băng thêm diamond (cộng dồn).
    ///
    /// Trong một cuộc trò chuyện, user có thể hỏi NHIỀU CÂU.
    /// Mỗi câu hỏi = 1 giao dịch escrow riêng biệt.
    /// Ví dụ: câu 1 tốn 50 diamond, câu 2 tốn 30 diamond
    /// → Tổng đóng băng: 80 diamond, nhưng xử lý riêng từng item.
    /// </summary>
    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new AddQuestionCommand
        {
            UserId = userId.Value,
            ConversationRef = body.ConversationRef,       // ID cuộc trò chuyện
            AmountDiamond = body.AmountDiamond,            // Số diamond cho câu hỏi này
            ProposalMessageRef = body.ProposalMessageRef,  // ID tin nhắn đề nghị
            IdempotencyKey = body.IdempotencyKey,           // Key chống trùng
        };

        var itemId = await _mediator.Send(command);
        return Ok(new { success = true, itemId });
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/escrow/{conversationId}
    /// MỤC ĐÍCH: Xem trạng thái escrow hiện tại của một cuộc trò chuyện.
    ///
    /// TRẢ VỀ:
    ///   - Tổng số diamond đã escrow
    ///   - Danh sách các question items và trạng thái từng item
    ///   - Thông tin phiên giao dịch (session)
    ///
    /// BẢO MẬT: Handler kiểm tra RequesterId phải thuộc conversation.
    /// </summary>
    [HttpGet("{conversationId}")]
    public async Task<IActionResult> GetStatus(string conversationId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var query = new GetEscrowStatusQuery
        {
            ConversationRef = conversationId, // ID conversation cần xem
            RequesterId = userId.Value,       // Người yêu cầu (để kiểm tra quyền)
        };

        var result = await _mediator.Send(query);
        if (result == null) return NotFound(); // Không tìm thấy escrow cho conversation này
        return Ok(result);
    }
}

// ===================================================================
// CÁC CLASS DTO (Request Bodies)
// ===================================================================
// Mỗi class bên dưới là "khuôn mẫu" cho dữ liệu client gửi lên.

/// <summary>
/// DTO cho POST /escrow/accept - Chấp nhận đề nghị từ reader.
/// </summary>
public class AcceptOfferBody
{
    /// <summary>UUID của reader mà user muốn giao dịch.</summary>
    public Guid ReaderId { get; set; }

    /// <summary>ID cuộc trò chuyện (MongoDB ObjectId dạng string).</summary>
    public string ConversationRef { get; set; } = string.Empty;

    /// <summary>Số diamond cần đóng băng cho câu hỏi này.</summary>
    public long AmountDiamond { get; set; }

    /// <summary>ID tin nhắn chứa đề nghị giá (để tham chiếu lại). Có thể null.</summary>
    public string? ProposalMessageRef { get; set; }

    /// <summary>
    /// Key chống giao dịch trùng lặp (idempotency key).
    /// Client tạo UUID ngẫu nhiên cho mỗi lần giao dịch.
    /// Nếu gửi lại cùng key → server trả kết quả cũ thay vì tạo mới.
    /// </summary>
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>DTO cho POST /escrow/reply - Reader đánh dấu đã trả lời.</summary>
public class ReaderReplyBody 
{ 
    /// <summary>UUID của question item mà reader đã trả lời.</summary>
    public Guid ItemId { get; set; } 
}

/// <summary>DTO cho POST /escrow/confirm - User xác nhận giải phóng tiền.</summary>
public class ConfirmReleaseBody 
{ 
    /// <summary>UUID của question item cần xác nhận.</summary>
    public Guid ItemId { get; set; } 
}

/// <summary>DTO cho POST /escrow/dispute - User mở tranh chấp.</summary>
public class OpenDisputeBody
{
    /// <summary>UUID của question item đang tranh chấp.</summary>
    public Guid ItemId { get; set; }

    /// <summary>Lý do tranh chấp mà user nhập (bắt buộc).</summary>
    public string Reason { get; set; } = string.Empty;
}

/// <summary>DTO cho POST /escrow/add-question - Thêm câu hỏi mới.</summary>
public class AddQuestionBody
{
    /// <summary>ID cuộc trò chuyện (MongoDB ObjectId dạng string).</summary>
    public string ConversationRef { get; set; } = string.Empty;

    /// <summary>Số diamond cho câu hỏi bổ sung.</summary>
    public long AmountDiamond { get; set; }

    /// <summary>ID tin nhắn đề nghị (tùy chọn).</summary>
    public string? ProposalMessageRef { get; set; }

    /// <summary>Key chống trùng lặp giao dịch.</summary>
    public string IdempotencyKey { get; set; } = string.Empty;
}

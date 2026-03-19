/*
 * ===================================================================
 * FILE: DepositController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller xử lý luồng NẠP TIỀN vào ví người dùng.
 *   Có 2 endpoint chính:
 *   1. User tạo đơn nạp tiền (CreateOrder)
 *   2. Cổng thanh toán gọi webhook khi giao dịch hoàn tất (Webhook)
 *
 * LUỒNG NẠP TIỀN:
 *   ┌──────────┐    ┌──────────┐    ┌──────────────┐    ┌──────────┐
 *   │ User     │───→│ Server   │───→│ Cổng thanh   │───→│ Ngân hàng│
 *   │ (Client) │    │ (API)    │    │ toán (VNPay)  │    │          │
 *   └──────────┘    └──────────┘    └──────────────┘    └──────────┘
 *        │               │                 │
 *   1. Tạo đơn nạp  2. Tạo order    3. User thanh toán
 *                    trong DB         trên trang VNPay
 *                                          │
 *   ┌──────────┐    ┌──────────┐    ┌──────────────┐
 *   │ User ví  │←───│ Server   │←───│ Webhook call │
 *   │ +diamond │    │ cộng $ │    │ từ VNPay     │
 *   └──────────┘    └──────────┘    └──────────────┘
 *   5. Nhận tiền  4. Xử lý webhook  4. VNPay báo success
 * ===================================================================
 */

// Import các thư viện cần thiết
using MediatR;                 // MediatR: gửi Command/Query
using Microsoft.AspNetCore.Authorization; // Kiểm soát quyền truy cập
using Microsoft.AspNetCore.Mvc; // Nền tảng API
using System;                   // Guid, v.v.
using System.Security.Claims;   // Đọc JWT claims
using System.Threading.Tasks;   // async/await

// Import DTO từ Contracts
using TarotNow.Api.Contracts;

// Import Command cho nạp tiền
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

namespace TarotNow.Api.Controllers;

/*
 * [Route("api/v1/deposits")]: URL gốc = /api/v1/deposits
 * KHÔNG có [Authorize] ở cấp class vì endpoint webhook cần [AllowAnonymous]
 * (cổng thanh toán gọi webhook mà không có JWT token của user).
 */
[Route("api/v1/deposits")]
[ApiController]
public class DepositController : ControllerBase
{
    /*
     * Mediator: Trung gian MediatR (viết hoa "M" vì convention khác với controller khác,
     *   nhưng vẫn hoạt động tương tự - chỉ là naming style khác).
     * _logger: Ghi log để theo dõi lỗi và hoạt động nạp tiền.
     *   Đặc biệt quan trọng ở đây vì liên quan đến TÀI CHÍNH - mọi thao tác phải được ghi lại.
     */
    private readonly IMediator Mediator;
    private readonly ILogger<DepositController> _logger;

    public DepositController(IMediator mediator, ILogger<DepositController> logger)
    {
        Mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/deposits/orders
    /// MỤC ĐÍCH: User tạo đơn nạp tiền.
    ///
    /// LUỒNG:
    ///   1. User chọn số tiền muốn nạp (ví dụ: 100.000 VND)
    ///   2. Client gửi request với AmountVnd = 100000
    ///   3. Handler tạo bản ghi DepositOrder trong database (trạng thái: Pending)
    ///   4. Handler tạo URL thanh toán từ cổng thanh toán
    ///   5. Trả về URL cho client → client mở URL đó để user thanh toán
    ///
    /// [Authorize]: Chỉ user đã đăng nhập mới được tạo đơn nạp.
    /// </summary>
    [HttpPost("orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDepositOrderRequest request)
    {
        // Lấy userId từ JWT token (ai đang nạp tiền)
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        // Tạo command nạp tiền
        var command = new CreateDepositOrderCommand
        {
            UserId = userId,            // User nạp tiền (từ JWT, không cho client tự gửi)
            AmountVnd = request.AmountVnd // Số tiền VND muốn nạp
        };

        // Gửi command → handler tạo order, tạo URL thanh toán
        var response = await Mediator.Send(command);
        return Ok(response); // Trả về thông tin order + URL thanh toán
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/deposits/webhook/vnpay
    /// MỤC ĐÍCH: Nhận thông báo (webhook) từ cổng thanh toán khi giao dịch hoàn tất.
    ///
    /// WEBHOOK LÀ GÌ?
    ///   Khi user thanh toán xong trên trang VNPay, VNPay sẽ GỌI NGƯỢC lại
    ///   server của mình qua URL này để thông báo kết quả (thành công/thất bại).
    ///   Đây là giao tiếp SERVER-TO-SERVER (VNPay → TarotNow API).
    ///
    /// [AllowAnonymous]: Cho phép gọi mà KHÔNG cần JWT token.
    ///   Vì VNPay là server bên ngoài, nó không có JWT token của user.
    ///   Thay vào đó, bảo mật bằng SIGNATURE VERIFICATION:
    ///   VNPay ký chữ ký số (digital signature) lên payload,
    ///   server kiểm tra chữ ký đó để xác nhận request đến từ VNPay thật.
    ///
    /// BẢO MẬT:
    ///   Hacker không thể giả mạo webhook vì không có secret key để tạo chữ ký.
    /// </summary>
    [HttpPost("webhook/vnpay")] // URL mà VNPay gọi khi thanh toán xong
    [AllowAnonymous] // Webhook là server-to-server, bảo mật bằng signature, không cần JWT
    public async Task<IActionResult> Webhook()
    {
        // ========================================
        // BƯỚC 1: ĐỌC RAW PAYLOAD VÀ SIGNATURE
        // ========================================

        /*
         * Đọc TOÀN BỘ nội dung body request dưới dạng text (raw string).
         * Không dùng [FromBody] vì:
         * 1. Cần raw payload gốc để verify signature (chữ ký)
         * 2. [FromBody] có thể thay đổi format (reorder keys) → signature sai
         * 
         * StreamReader: công cụ đọc stream (luồng dữ liệu) thành string.
         * "using var": tự động giải phóng bộ nhớ khi đọc xong (Dispose pattern).
         */
        using var reader = new System.IO.StreamReader(Request.Body);
        var rawPayload = await reader.ReadToEndAsync();
        
        /*
         * Lấy chữ ký từ HTTP header "X-Webhook-Signature".
         * Mỗi cổng thanh toán dùng tên header khác nhau:
         * - VNPay: X-Webhook-Signature
         * - Stripe: Stripe-Signature
         * - PayPal: PAYPAL-AUTH-SIGNATURE
         * Tên header này cần đổi khi tích hợp cổng thanh toán khác.
         */
        var signature = Request.Headers["X-Webhook-Signature"].ToString() ?? string.Empty;

        // ========================================
        // BƯỚC 2: PARSE (PHÂN TÍCH) JSON PAYLOAD
        // ========================================

        /*
         * Chuyển đổi raw JSON string thành đối tượng C# (WebhookPayloadData).
         * PropertyNameCaseInsensitive = true: không phân biệt hoa/thường tên trường.
         *   Ví dụ: "orderId" và "OrderId" đều match được.
         *   Tránh lỗi khi cổng thanh toán dùng convention khác với C#.
         */
        WebhookPayloadData? payloadData;
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            payloadData = System.Text.Json.JsonSerializer.Deserialize<WebhookPayloadData>(rawPayload, options);
        }
        catch (System.Text.Json.JsonException ex)
        {
            // JSON không hợp lệ (bị hỏng, sai format) → ghi log cảnh báo
            _logger.LogWarning(ex, "Invalid webhook JSON payload.");
            return BadRequest(new { msg = "Invalid JSON payload" });
        }

        // Kiểm tra payload sau khi parse có null không
        if (payloadData == null)
        {
            _logger.LogWarning("Webhook payload is null after deserialization.");
            return BadRequest(new { msg = "Invalid JSON payload" });
        }

        // ========================================
        // BƯỚC 3: GỬI COMMAND XỬ LÝ WEBHOOK
        // ========================================

        /*
         * Gửi cả raw payload (để verify signature) và parsed data (để đọc thông tin).
         * Handler sẽ:
         * 1. Verify signature bằng secret key → xác nhận webhook từ VNPay thật
         * 2. Tìm DepositOrder tương ứng trong database
         * 3. Kiểm tra trạng thái (tránh xử lý trùng)
         * 4. Cộng diamond vào ví user
         * 5. Cập nhật order status → Completed
         */
        var command = new ProcessDepositWebhookCommand
        {
            RawPayload = rawPayload,     // Payload gốc (để verify)
            Signature = signature,        // Chữ ký số (để verify)
            PayloadData = payloadData     // Dữ liệu đã parse (để đọc)
        };

        var result = await Mediator.Send(command);

        // Trả OK cho VNPay biết đã nhận thành công
        // VNPay sẽ gọi lại nếu nhận được lỗi (retry mechanism)
        return result ? Ok(new { success = true }) : BadRequest();
    }
}

/*
 * ===================================================================
 * FILE: ProcessDepositWebhookCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh chuyên biệt dành cho CỔNG GIAO TẾP tự động (Webhook) từ Server Ngân hàng.
 *   
 * THÔNG TIN KẾT NỐI:
 *   Khi User chuyển khoản thành công bên MoMo/VNPay, hệ thống của họ 
 *   sẽ gọi ngược (Callback/Webhook) về Server TarotNow thông qua API này 
 *   để thông báo "Ê Tarot, khách chuyển tiền rồi nhé, cộng điểm cho khách đi".
 * ===================================================================
 */

using MediatR;
using System.Text.Json;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

/// <summary>
/// Gói hồ sơ báo cáo tình trạng chuyển khoản do Ngân Hàng gửi tới.
/// </summary>
public class ProcessDepositWebhookCommand : IRequest<bool>
{
    /// <summary>Văn bản Raw JSON thuần túy (Dùng để kiểm tra Chữ ký điện tử - Tránh hacker giả mạo MoMo).</summary>
    public string RawPayload { get; set; } = string.Empty;

    /// <summary>Chữ ký mã hoá xác thực đính kèm trong Header HTTP của Ngân Hàng.</summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>Hộp chứa dữ liệu đã được giải mã sẵn sàng cho Handler tiêu thụ.</summary>
    public WebhookPayloadData PayloadData { get; set; } = new();
}

/// <summary>
/// DTO chứa Ruột thông tin hoá đơn rớt từ Ngân Hàng về.
/// </summary>
public class WebhookPayloadData
{
    public string OrderId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public long Amount { get; set; }
    
    /// <summary>"SUCCESS" (MoMo trừ tiền thành công) hoặc "FAILED" (Thẻ khô máu).</summary>
    public string Status { get; set; } = string.Empty; 
    
    // Thuận tiện lưu trữ tỷ giá hoặc thông tin mở rộng (Mã Code đối soát)
    public string? FxSnapshot { get; set; } 
}

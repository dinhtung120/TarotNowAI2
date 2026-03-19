/*
 * ===================================================================
 * FILE: RecordConsentCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Legal.Commands.RecordConsent
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lưu giữ Bằng Chứng Nhấn Đồng Ý của hệ thống Pháp Lý (Chấp nhận Điều khoản).
 *   (Phục vụ GDPR/CCPA - Bảo vệ Công ty khỏi nguy cơ kiện tụng pháp lý).
 *
 * LUỒNG HOẠT ĐỘNG:
 *   Khi User Update App hoặc Lần Đầu Đăng Ký, Popup Terms of Service (TOS) hiện lên.
 *   User Bấm Nút "I Agree" -> Bắn Gói Lệnh này lưu Dấu Vân Tay (IP, Browser) vào Server.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

/// <summary>
/// Gói Lệnh: Thu Thập Bút Tích Pháp Lý của Khách hàng.
/// </summary>
public class RecordConsentCommand : IRequest<bool>
{
    /// <summary>Phải lưu danh tính người nhấn nút Đồng ý.</summary>
    public Guid UserId { get; set; }
    
    /// <summary>Tên Hợp Đồng: TOS (Điều khoản GD) hay Privacy_Policy (Chính sách Bảo mật).</summary>
    public string DocumentType { get; set; } = string.Empty;
    
    /// <summary>Đời Hợp Đồng: 1.0 (2024), 2.0 (2025). Sau này đổi Luật, có cớ ép User bấm Đồng Ý Lại.</summary>
    public string Version { get; set; } = string.Empty;
    
    // ===================================
    // KẾ THỪA AUDIT TRAIL LOG CỦA MỸ (GDPR)
    // ===================================
    
    /// <summary>Địa chỉ mạng (Dùng làm bằng chứng trước tòa User ở quốc gia nào đã nhấn nút).</summary>
    public string IpAddress { get; set; } = string.Empty;
    
    /// <summary>Trình duyệt nhấn (Safari trên iPhone hay Cốc Cốc trên máy tính).</summary>
    public string UserAgent { get; set; } = string.Empty;
}

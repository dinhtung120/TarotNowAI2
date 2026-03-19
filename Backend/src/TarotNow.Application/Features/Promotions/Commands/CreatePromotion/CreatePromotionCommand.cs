/*
 * ===================================================================
 * FILE: CreatePromotionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.CreatePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Quyền Năng Của Quản Trị Viên (Admin). Tạo ra các "Khuyến Mãi" mồi chài 
 *   kích thích Khách nạp tiền.
 *
 * CONCEPT KINH TẾ (ECONOMY/MARKETING):
 *   Để kích hoạt Khuyến Mãi này, User phải nạp đủ `MinAmountVnd`. 
 *   Nếu đạt mốc đó, User sẽ được Bank Hệ Thống thưởng rớt thêm `BonusDiamond` (Tiền ảo Máu).
 *   
 * VÍ DỤ: 
 *   Nạp trên 500,000 VNĐ -> Thưởng Nóng 50 Kim Cương Bonus.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

/// <summary>
/// Gói Lệnh Dành Cho Admin Dựng Chiến Dịch (Campaign) Khuyến Mãi Nạp Thẻ.
/// </summary>
public class CreatePromotionCommand : IRequest<bool>
{
    /// <summary>Mốc Hụt Hơi để hưởng Lộc: Nạp Tối Thiểu 1,000,000 VNĐ. Tiền thật bằng VNĐ.</summary>
    public long MinAmountVnd { get; set; }
    
    /// <summary>Số Tiền Rơi Vãi Khuyến Mãi (Trả bằng Đơn Vị Thạch Ánh Dương - Diamond Của App, Không Phát Tiền Mặt VNĐ NHA!).</summary>
    public long BonusDiamond { get; set; }
    
    /// <summary>Bật/Tắt Sự Kiện Event này (Ngày Thường Tắt, Trung Thu Bật Lên).</summary>
    public bool IsActive { get; set; }
}

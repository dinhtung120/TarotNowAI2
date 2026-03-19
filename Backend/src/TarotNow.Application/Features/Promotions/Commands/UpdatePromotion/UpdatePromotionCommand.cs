/*
 * ===================================================================
 * FILE: UpdatePromotionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.UpdatePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh cho phép Ghi Đè (Chỉnh sửa) các thông số của một Gói Khuyến Mãi cũ,
 *   thường dùng khi Admin gõ nhầm số 0 thành số O hoặc set lộn giá kim cương.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    
    /// <summary>Giá Trị Tiền Mặt Nạp Yêu Cầu Cần Đạt Thành Để Giật Giải.</summary>
    public long MinAmountVnd { get; set; }
    
    /// <summary>Quà Tặng Thưởng (Đơn vị nội tệ App).</summary>
    public long BonusDiamond { get; set; }
    
    /// <summary>Cờ Gạt Đóng/Mở sự kiện.</summary>
    public bool IsActive { get; set; }
}

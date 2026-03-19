/*
 * ===================================================================
 * FILE: ListPromotionsQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Queries.ListPromotions
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh Trích Xuất Danh Sách Sự kiện Khuyến Mãi.
 *   - Nếu là User: Chỉ trả về các Khuyến Mãi đang chạy (Active).
 *   - Nếu là Admin: Lấy ráo trọi cả Active lẫn Inactive để quản lý.
 * ===================================================================
 */

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Promotions.Queries.ListPromotions;

public class ListPromotionsQuery : IRequest<IEnumerable<PromotionResponse>>
{
    /// <summary>Cờ Gạt: True = Chỉ lấy cái đang bật (Dành cho User). False = Lấy hết (Dành cho Admin).</summary>
    public bool OnlyActive { get; set; }
}

public class PromotionResponse
{
    public Guid Id { get; set; }
    
    /// <summary>Giá Trị Tiền Mặt Nạp Yêu Cầu.</summary>
    public long MinAmountVnd { get; set; }
    
    /// <summary>Quà Tặng Thưởng.</summary>
    public long BonusDiamond { get; set; }
    
    public bool IsActive { get; set; }
    
    /// <summary>Thời gian tạo, để Frontend sort Khuyến mãi mới nhất lên trên cùng.</summary>
    public DateTime CreatedAt { get; set; }
}

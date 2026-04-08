using MediatR;
using System;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Command cập nhật thông tin một promotion hiện có.
public class UpdatePromotionCommand : IRequest<bool>
{
    // Định danh promotion cần cập nhật.
    public Guid Id { get; set; }

    // Mức tiền nạp tối thiểu (VND) để áp dụng khuyến mãi.
    public long MinAmountVnd { get; set; }

    // Số kim cương thưởng khi thỏa điều kiện.
    public long BonusDiamond { get; set; }

    // Trạng thái active/inactive của promotion sau cập nhật.
    public bool IsActive { get; set; }
}

using MediatR;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

// Command tạo mới một chính sách khuyến mãi nạp tiền.
public class CreatePromotionCommand : IRequest<bool>
{
    // Mức tiền nạp tối thiểu (VND) để áp dụng khuyến mãi.
    public long MinAmountVnd { get; set; }

    // Số Gold thưởng khi thỏa điều kiện nạp tiền.
    public long BonusGold { get; set; }

    // Cờ bật/tắt khuyến mãi ngay sau khi tạo.
    public bool IsActive { get; set; }
}

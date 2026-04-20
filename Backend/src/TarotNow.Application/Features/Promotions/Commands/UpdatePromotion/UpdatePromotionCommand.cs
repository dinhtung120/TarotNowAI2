using MediatR;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Command cập nhật một policy khuyến mãi nạp tiền.
public class UpdatePromotionCommand : IRequest<bool>
{
    // Định danh promotion cần cập nhật.
    public Guid Id { get; set; }

    // Mức nạp tối thiểu để áp dụng khuyến mãi.
    public long MinAmountVnd { get; set; }

    // Mức Gold thưởng khi đủ điều kiện.
    public long BonusGold { get; set; }

    // Trạng thái bật/tắt promotion.
    public bool IsActive { get; set; }
}

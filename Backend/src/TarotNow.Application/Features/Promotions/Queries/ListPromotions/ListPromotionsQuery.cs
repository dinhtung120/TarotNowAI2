using MediatR;

namespace TarotNow.Application.Features.Promotions.Queries.ListPromotions;

// Query lấy danh sách promotion để hiển thị quản trị.
public class ListPromotionsQuery : IRequest<IEnumerable<PromotionResponse>>
{
    // Cờ lọc chỉ lấy promotion đang active.
    public bool OnlyActive { get; set; }
}

// DTO thông tin promotion trả về cho client.
public class PromotionResponse
{
    // Định danh promotion.
    public Guid Id { get; set; }

    // Ngưỡng nạp tối thiểu (VND).
    public long MinAmountVnd { get; set; }

    // Số Gold thưởng.
    public long BonusGold { get; set; }

    // Trạng thái active/inactive.
    public bool IsActive { get; set; }

    // Thời điểm tạo promotion.
    public DateTime CreatedAt { get; set; }
}

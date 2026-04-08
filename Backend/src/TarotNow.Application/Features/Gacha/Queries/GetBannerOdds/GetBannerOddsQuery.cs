using MediatR;
using TarotNow.Application.Features.Gacha.Dtos;

namespace TarotNow.Application.Features.Gacha.Queries.GetBannerOdds;

// Query lấy bảng odds chi tiết của một banner gacha.
public class GetBannerOddsQuery : IRequest<GachaBannerOddsDto>
{
    // Mã banner cần truy vấn odds.
    public string BannerCode { get; set; } = string.Empty;

    /// <summary>
    /// Khởi tạo query với banner code cụ thể.
    /// Luồng xử lý: gán trực tiếp BannerCode từ tham số đầu vào.
    /// </summary>
    public GetBannerOddsQuery(string bannerCode)
    {
        BannerCode = bannerCode;
    }
}

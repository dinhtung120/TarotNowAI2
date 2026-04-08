using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetActiveBanners;

// Handler truy vấn danh sách banner gacha active.
public class GetActiveBannersQueryHandler : IRequestHandler<GetActiveBannersQuery, List<GachaBannerDto>>
{
    private readonly IGachaRepository _gachaRepository;
    private readonly ILogger<GetActiveBannersQueryHandler> _logger;

    /// <summary>
    /// Khởi tạo handler get active banners.
    /// Luồng xử lý: nhận gacha repository và logger để tải banner active và ghi log quan sát.
    /// </summary>
    public GetActiveBannersQueryHandler(IGachaRepository gachaRepository, ILogger<GetActiveBannersQueryHandler> logger)
    {
        _gachaRepository = gachaRepository;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý query lấy banner active.
    /// Luồng xử lý: tải toàn bộ banner active, nếu có UserId thì enrich pity count từng banner, rồi map kết quả DTO.
    /// </summary>
    public async Task<List<GachaBannerDto>> Handle(GetActiveBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _gachaRepository.GetAllActiveBannersAsync(cancellationToken);
        _logger.LogInformation("[GachaQuery] Found {Count} active banners in repository.", banners.Count);

        if (banners.Count == 0)
        {
            // Trả danh sách rỗng ngay khi không có banner active.
            return new List<GachaBannerDto>();
        }

        var result = new List<GachaBannerDto>();
        foreach (var b in banners)
        {
            int pity = 0;
            if (request.UserId.HasValue)
            {
                // Chỉ truy vấn pity khi có ngữ cảnh user cụ thể.
                pity = await _gachaRepository.GetUserPityCountAsync(request.UserId.Value, b.Id, cancellationToken);
            }

            result.Add(new GachaBannerDto
            {
                Code = b.Code,
                NameVi = b.NameVi,
                NameEn = b.NameEn,
                DescriptionVi = b.DescriptionVi,
                DescriptionEn = b.DescriptionEn,
                CostDiamond = b.CostDiamond,
                OddsVersion = b.OddsVersion,
                UserCurrentPity = pity
            });
        }

        return result;
    }
}

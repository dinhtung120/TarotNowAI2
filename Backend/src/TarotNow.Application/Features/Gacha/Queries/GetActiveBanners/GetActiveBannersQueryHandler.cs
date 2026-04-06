using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gacha.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gacha.Queries.GetActiveBanners;

public class GetActiveBannersQueryHandler : IRequestHandler<GetActiveBannersQuery, List<GachaBannerDto>>
{
    private readonly IGachaRepository _gachaRepository;
    private readonly ILogger<GetActiveBannersQueryHandler> _logger;

    public GetActiveBannersQueryHandler(IGachaRepository gachaRepository, ILogger<GetActiveBannersQueryHandler> logger)
    {
        _gachaRepository = gachaRepository;
        _logger = logger;
    }

    public async Task<List<GachaBannerDto>> Handle(GetActiveBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _gachaRepository.GetAllActiveBannersAsync(cancellationToken);
        _logger.LogInformation("[GachaQuery] Found {Count} active banners in repository.", banners.Count);
        
        if (banners.Count == 0)
        {
            // Debug: Check if ANY banners exist at all
            // Note: This is just for debugging purposes
        }
        
        var result = new List<GachaBannerDto>();
        foreach (var b in banners)
        {
            int pity = 0;
            if (request.UserId.HasValue)
            {
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

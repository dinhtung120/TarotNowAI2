using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Deposit.Queries.ListDepositPackages;

// Handler lấy danh sách gói nạp preset.
public class ListDepositPackagesQueryHandler : IRequestHandler<ListDepositPackagesQuery, IEnumerable<DepositPackageDto>>
{
    private readonly IDepositPackageCatalog _depositPackageCatalog;
    private readonly IDepositPromotionRepository _depositPromotionRepository;

    /// <summary>
    /// Khởi tạo handler lấy gói nạp.
    /// </summary>
    public ListDepositPackagesQueryHandler(
        IDepositPackageCatalog depositPackageCatalog,
        IDepositPromotionRepository depositPromotionRepository)
    {
        _depositPackageCatalog = depositPackageCatalog;
        _depositPromotionRepository = depositPromotionRepository;
    }

    /// <summary>
    /// Xử lý query lấy gói nạp.
    /// </summary>
    public async Task<IEnumerable<DepositPackageDto>> Handle(
        ListDepositPackagesQuery request,
        CancellationToken cancellationToken)
    {
        var activePromotions = (await _depositPromotionRepository.GetActivePromotionsAsync(cancellationToken)).ToList();
        var packages = _depositPackageCatalog.GetActivePackages();

        return packages
            .OrderBy(x => x.AmountVnd)
            .Select(package =>
            {
                var bonusGold = ResolveBonusGold(package.AmountVnd, activePromotions);

                return new DepositPackageDto
                {
                    Code = package.Code,
                    AmountVnd = package.AmountVnd,
                    BaseDiamondAmount = package.BaseDiamond,
                    BonusGoldAmount = bonusGold,
                    TotalDiamondAmount = package.BaseDiamond
                };
            })
            .ToArray();
    }

    private static long ResolveBonusGold(long amountVnd, IReadOnlyCollection<TarotNow.Domain.Entities.DepositPromotion> activePromotions)
    {
        foreach (var promotion in activePromotions)
        {
            if (amountVnd >= promotion.MinAmountVnd)
            {
                return promotion.BonusGold;
            }
        }

        return 0;
    }
}

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Inventory.Queries;

/// <summary>
/// Handler truy vấn kho đồ người dùng.
/// </summary>
public sealed class GetUserInventoryQueryHandler : IRequestHandler<GetUserInventoryQuery, GetUserInventoryResult>
{
    private readonly IUserItemRepository _userItemRepository;

    /// <summary>
    /// Khởi tạo handler query inventory.
    /// </summary>
    public GetUserInventoryQueryHandler(IUserItemRepository userItemRepository)
    {
        _userItemRepository = userItemRepository;
    }

    /// <summary>
    /// Xử lý query và map dữ liệu inventory trả về.
    /// </summary>
    public async Task<GetUserInventoryResult> Handle(GetUserInventoryQuery request, CancellationToken cancellationToken)
    {
        var views = await _userItemRepository.GetUserInventoryAsync(request.UserId, cancellationToken);

        var items = views
            .OrderByDescending(x => x.AcquiredAtUtc)
            .Select(x => new UserInventoryItemDto
            {
                ItemDefinitionId = x.ItemDefinitionId,
                ItemCode = x.ItemCode,
                ItemType = x.ItemType,
                EnhancementType = x.EnhancementType,
                Rarity = x.Rarity,
                IsConsumable = x.IsConsumable,
                IsPermanent = x.IsPermanent,
                EffectValue = x.EffectValue,
                SuccessRatePercent = x.SuccessRatePercent,
                NameVi = x.NameVi,
                NameEn = x.NameEn,
                NameZh = x.NameZh,
                DescriptionVi = x.DescriptionVi,
                DescriptionEn = x.DescriptionEn,
                DescriptionZh = x.DescriptionZh,
                IconUrl = x.IconUrl,
                Quantity = x.Quantity,
                AcquiredAtUtc = x.AcquiredAtUtc,
            })
            .ToArray();

        return new GetUserInventoryResult
        {
            Items = items,
        };
    }
}

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

// Handler tạo mới promotion nạp tiền.
public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler tạo promotion.
    /// Luồng xử lý: nhận promotion repository để khởi tạo và lưu bản ghi khuyến mãi mới.
    /// </summary>
    public CreatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý command tạo promotion.
    /// Luồng xử lý: dựng entity DepositPromotion từ request và persist vào repository.
    /// </summary>
    public async Task<bool> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = new DepositPromotion(
            request.MinAmountVnd,
            request.BonusDiamond,
            request.IsActive);
        // Tạo entity khuyến mãi mới theo ngưỡng nạp và mức thưởng được quản trị viên cấu hình.

        await _promotionRepository.AddAsync(promotion, cancellationToken);
        // Ghi bản ghi promotion mới vào persistence để có thể áp dụng trong luồng nạp tiền.

        return true;
    }
}

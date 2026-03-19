/*
 * ===================================================================
 * FILE: UpdatePromotionCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.UpdatePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Áp Dụng Bản Cập Nhật Khuyến Mãi vào Cơ Sở Dữ Liệu bằng cách gọi hàm Domain.
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandHandler : IRequestHandler<UpdatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    public UpdatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<bool> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        // Tra ID xem gói Khuyến mãi này có tồn tại hay không, tránh Sửa Lụi.
        var promotion = await _promotionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Promotion {request.Id} not found");

        // Nhờ Domain Method thay đổi. Nếu ta gán trực tiếp = thì Sạch sẽ phá vỡ quy tắc Encapsulation.
        promotion.Update(request.MinAmountVnd, request.BonusDiamond, request.IsActive);
        
        // Quá trình Commit vào EF Core hoặc MongoDb
        await _promotionRepository.UpdateAsync(promotion, cancellationToken);
        
        return true;
    }
}

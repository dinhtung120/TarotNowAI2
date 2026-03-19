/*
 * ===================================================================
 * FILE: CreatePromotionCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.CreatePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Nơi thực thi Lệnh Tạo Event Khuyến Mãi do Admin bấm nút trên Dashboard.
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    public CreatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<bool> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        // Khởi tạo một Gói Khuyến Mãi Mới từ bản vẽ Domain.
        var promotion = new DepositPromotion(request.MinAmountVnd, request.BonusDiamond, request.IsActive);
        
        // Quăng rổ vào Database.
        await _promotionRepository.AddAsync(promotion, cancellationToken);
        
        return true;
    }
}

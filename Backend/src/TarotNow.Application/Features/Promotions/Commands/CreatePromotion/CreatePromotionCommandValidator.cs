/*
 * ===================================================================
 * FILE: CreatePromotionCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Promotions.Commands.CreatePromotion
 * ===================================================================
 * MỤC ĐÍCH:
 *   Phòng vệ lúc Admin nhập liệu sai (hoặc Hacker gọi thẳng API truyền số Âm).
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        // Nạp min phải là số Dương. Nạp -100k VNĐ rồi bắt Web tặng Kim Cương là Lừa Đảo.
        RuleFor(x => x.MinAmountVnd).GreaterThan(0);
        
        // Kim cương tặng cũng phải > 0. (Tặng 0 thì khuyến mãi làm gì).
        RuleFor(x => x.BonusDiamond).GreaterThan(0);
    }
}

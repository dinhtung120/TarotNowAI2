using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.UpdatePromotion;

// Handler cập nhật promotion nạp tiền.
public class UpdatePromotionCommandHandler : IRequestHandler<UpdatePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật promotion.
    /// Luồng xử lý: nhận promotion repository để tải bản ghi hiện tại và lưu lại dữ liệu sau cập nhật.
    /// </summary>
    public UpdatePromotionCommandHandler(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý command cập nhật promotion.
    /// Luồng xử lý: lấy promotion theo id, ném lỗi nếu thiếu dữ liệu, rồi áp dụng thay đổi và persist.
    /// </summary>
    public async Task<bool> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Promotion {request.Id} not found");

        promotion.Update(request.MinAmountVnd, request.BonusDiamond, request.IsActive);
        // Áp dụng thay đổi nghiệp vụ vào entity để chuẩn hóa cập nhật qua domain method.

        await _promotionRepository.UpdateAsync(promotion, cancellationToken);
        // Persist state mới để promotion có hiệu lực tức thì ở luồng nạp tiền.

        return true;
    }
}

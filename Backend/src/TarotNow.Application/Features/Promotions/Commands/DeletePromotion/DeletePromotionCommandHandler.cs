using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

// Handler xóa promotion khỏi hệ thống.
public class DeletePromotionCommandExecutor : ICommandExecutionExecutor<DeletePromotionCommand, bool>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler xóa promotion.
    /// Luồng xử lý: nhận promotion repository để tải và xóa bản ghi khuyến mãi mục tiêu.
    /// </summary>
    public DeletePromotionCommandExecutor(IDepositPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    /// <summary>
    /// Xử lý command xóa promotion.
    /// Luồng xử lý: tìm promotion theo id, ném lỗi nếu không tồn tại, rồi thực hiện xóa vật lý ở repository.
    /// </summary>
    public async Task<bool> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Promotion {request.Id} not found");

        await _promotionRepository.DeleteAsync(promotion, cancellationToken);
        // Đổi state hệ thống: loại bỏ promotion khỏi kho cấu hình áp dụng nạp tiền.

        return true;
    }
}

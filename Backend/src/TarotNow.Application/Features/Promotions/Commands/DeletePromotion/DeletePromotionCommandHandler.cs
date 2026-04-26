using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Promotions.Commands.DeletePromotion;

// Handler xóa promotion khỏi hệ thống.
public class DeletePromotionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DeletePromotionCommandHandlerRequestedDomainEvent>
{
    private readonly IDepositPromotionRepository _promotionRepository;

    /// <summary>
    /// Khởi tạo handler xóa promotion.
    /// Luồng xử lý: nhận promotion repository để tải và xóa bản ghi khuyến mãi mục tiêu.
    /// </summary>
    public DeletePromotionCommandHandlerRequestedDomainEventHandler(
        IDepositPromotionRepository promotionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
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

    protected override async Task HandleDomainEventAsync(
        DeletePromotionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

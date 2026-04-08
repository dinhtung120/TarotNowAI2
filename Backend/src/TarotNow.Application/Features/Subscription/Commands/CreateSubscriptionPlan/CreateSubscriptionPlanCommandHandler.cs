using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

// Handler tạo mới subscription plan.
public class CreateSubscriptionPlanCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Guid>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    /// <summary>
    /// Khởi tạo handler tạo subscription plan.
    /// Luồng xử lý: nhận subscription repository để thêm plan mới và lưu thay đổi.
    /// </summary>
    public CreateSubscriptionPlanCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    /// <summary>
    /// Xử lý command tạo subscription plan.
    /// Luồng xử lý: dựng entity plan từ request, thêm vào repository, lưu transaction và trả plan id mới.
    /// </summary>
    public async Task<Guid> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new SubscriptionPlan(
            name: request.Name,
            description: request.Description,
            priceDiamond: request.PriceDiamond,
            durationDays: request.DurationDays,
            entitlementsJson: request.EntitlementsJson,
            displayOrder: request.DisplayOrder);
        // Khởi tạo plan từ input admin để chuẩn hóa dữ liệu qua domain entity.

        await _subscriptionRepository.AddPlanAsync(plan, cancellationToken);
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        // Persist plan mới trước khi trả id để tránh id trả ra nhưng dữ liệu chưa commit.

        return plan.Id;
    }
}

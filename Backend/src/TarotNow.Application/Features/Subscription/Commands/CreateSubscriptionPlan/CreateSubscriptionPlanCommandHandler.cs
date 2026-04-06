/*
 * ===================================================================
 * FILE: CreateSubscriptionPlanCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler thụ lý việc tạo Gói Mới Lưu Xuống Database.
 * ===================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Guid>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public CreateSubscriptionPlanCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Guid> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        // Khởi Tạo Khung Xương Từ Domain Entity
        var plan = new SubscriptionPlan(
            name: request.Name,
            description: request.Description,
            priceDiamond: request.PriceDiamond,
            durationDays: request.DurationDays,
            entitlementsJson: request.EntitlementsJson, // Dùng Payload JSON trực tiếp đỡ map.
            displayOrder: request.DisplayOrder
        );
        
        // Đút Của Vào Kho
        await _subscriptionRepository.AddPlanAsync(plan, cancellationToken);
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return plan.Id;
    }
}

/*
 * ===================================================================
 * FILE: CreateSubscriptionPlanCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lệnh dành riêng cho Quản Trị Viên (Admin) để tạo ra các gói bán hàng kỳ hạn mới.
 * ===================================================================
 */

using System;
using MediatR;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

public record CreateSubscriptionPlanCommand(
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    string EntitlementsJson,
    int DisplayOrder
) : IRequest<Guid>;

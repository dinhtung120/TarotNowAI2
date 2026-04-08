using MediatR;
using System.Collections.Generic;
using TarotNow.Application.Features.Subscription.Queries.Dtos;

namespace TarotNow.Application.Features.Subscription.Queries.GetSubscriptionPlans;

/// <summary>
/// Query lấy danh sách gói subscription đang active.
/// Luồng xử lý: handler trả danh sách SubscriptionPlanDto đã parse entitlement để client hiển thị trang mua gói.
/// </summary>
public record GetSubscriptionPlansQuery : IRequest<List<SubscriptionPlanDto>>;

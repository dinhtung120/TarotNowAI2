using MediatR;
using System;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

/// <summary>
/// Command tạo mới gói subscription.
/// Luồng xử lý: nhận cấu hình gói từ admin để handler dựng entity SubscriptionPlan và lưu vào persistence.
/// </summary>
/// <param name="Name">Tên gói subscription.</param>
/// <param name="Description">Mô tả ngắn của gói (tùy chọn).</param>
/// <param name="PriceDiamond">Giá gói theo diamond.</param>
/// <param name="DurationDays">Thời hạn gói theo số ngày.</param>
/// <param name="EntitlementsJson">JSON cấu hình entitlement đi kèm gói.</param>
/// <param name="DisplayOrder">Thứ tự hiển thị của gói trên UI.</param>
public record CreateSubscriptionPlanCommand(
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    string EntitlementsJson,
    int DisplayOrder
) : IRequest<Guid>;

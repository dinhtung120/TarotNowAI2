/*
 * ===================================================================
 * FILE: SubscriptionPlanDto.cs
 * ===================================================================
 * MỤC ĐÍCH:
 *   DTO trả về thông tin gói đăng ký cho Frontend hiển thị.
 *   Description là nullable vì Admin có thể không điền mô tả.
 * ===================================================================
 */

using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Subscription.Queries.Dtos;

public record SubscriptionPlanDto(
    Guid Id,
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    bool IsActive,
    Dictionary<string, int> Entitlements,
    DateTime CreatedAt
);

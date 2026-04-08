using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Subscription.Queries.Dtos;

/// <summary>
/// DTO thông tin gói subscription hiển thị cho client.
/// Luồng xử lý: query handler map dữ liệu plan từ persistence sang cấu trúc DTO này để trả về danh sách gói.
/// </summary>
/// <param name="Id">Định danh gói.</param>
/// <param name="Name">Tên gói.</param>
/// <param name="Description">Mô tả gói (tùy chọn).</param>
/// <param name="PriceDiamond">Giá gói theo diamond.</param>
/// <param name="DurationDays">Thời hạn gói (ngày).</param>
/// <param name="IsActive">Trạng thái gói còn bán hay không.</param>
/// <param name="Entitlements">Danh sách entitlement và quota mỗi ngày.</param>
/// <param name="CreatedAt">Thời điểm tạo gói.</param>
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

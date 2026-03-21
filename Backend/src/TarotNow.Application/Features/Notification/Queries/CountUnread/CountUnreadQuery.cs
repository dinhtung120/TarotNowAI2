/*
 * ===================================================================
 * FILE: CountUnreadQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Notification.Queries.CountUnread
 * ===================================================================
 * MỤC ĐÍCH:
 *   CQRS Query đếm số thông báo CHƯA ĐỌC của user.
 *   FE dùng kết quả để hiển thị badge count (số đỏ) trên icon chuông.
 *
 * TẠI SAO TÁCH RIÊNG QUERY NÀY?
 *   - GetNotificationsQuery trả danh sách đầy đủ → nặng, không phù hợp cho badge.
 *   - CountUnreadQuery chỉ trả 1 con số → nhẹ, gọi thường xuyên (polling).
 *   - FE có thể poll mỗi 30-60s mà không lo ảnh hưởng performance.
 *   - Single Responsibility: mỗi Query làm đúng 1 việc.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Notification.Queries.CountUnread;

/// <summary>
/// Query đếm số thông báo chưa đọc. Trả về long (số nguyên 64-bit).
/// </summary>
public class CountUnreadQuery : IRequest<long>
{
    /// <summary>ID user — controller gán từ JWT claim.</summary>
    public Guid UserId { get; set; }

    public CountUnreadQuery(Guid userId)
    {
        UserId = userId;
    }
}

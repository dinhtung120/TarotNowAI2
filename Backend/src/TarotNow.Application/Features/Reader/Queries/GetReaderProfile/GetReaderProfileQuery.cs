/*
 * ===================================================================
 * FILE: GetReaderProfileQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Queries.GetReaderProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Hỏi Thông Tin Chi Tiết (Tiểu Sử, Điểm Rating, Giá Cát Sê) 
 *   Của Duy Nhất 1 Thầy Bói. (Hiển thị ở trang Chi Tiết Thầy Bói).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

/// <summary>
/// Query lấy thông tin hồ sơ Reader theo userId.
/// Trả về null nếu reader chưa có profile (chưa được approve).
/// </summary>
public class GetReaderProfileQuery : IRequest<ReaderProfileDto?>
{
    /// <summary>ID của Ông Thầy Bói để Frontend biết kéo data của ai.</summary>
    public string UserId { get; set; } = string.Empty;
}

/*
 * ===================================================================
 * FILE: UpdateReaderProfileCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh Cập nhật trang cá nhân của Reader (Giá Tiền, Tiểu sử đa ngôn ngữ).
 *   Chỉ những ai "Đã Đậu" kỳ thi sát hạch lên Reader thì mới xài được API này.
 * ===================================================================
 */

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

public class UpdateReaderProfileCommand : IRequest<bool>
{
    /// <summary>UUID reader — lấy tự động từ Token để tránh Hacker sửa bậy Profile của Reader khác.</summary>
    public Guid UserId { get; set; }

    /// <summary>Cập nhật tiểu sử Tiếng Việt (Có thể null nếu họ không muốn sửa trường này).</summary>
    public string? BioVi { get; set; }

    /// <summary>Cập nhật tiểu sử Tiếng Anh.</summary>
    public string? BioEn { get; set; }

    /// <summary>Cập nhật tiểu sử Tiếng Trung.</summary>
    public string? BioZh { get; set; }

    /// <summary>Set giá Cát Sê mới cho mỗi câu hỏi cắc cớ từ Fan (Đơn vị Diamond).</summary>
    public long? DiamondPerQuestion { get; set; }

    /// <summary>Danh sách Tụ Bài/Chuyên Môn (Tình Yêu, Sự Nghiệp, Âm Phần...).</summary>
    public List<string>? Specialties { get; set; }
}

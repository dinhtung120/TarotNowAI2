using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

// Command cập nhật hồ sơ Reader.
public class UpdateReaderProfileCommand : IRequest<bool>
{
    // Định danh user của reader cần cập nhật hồ sơ.
    public Guid UserId { get; set; }

    // Mô tả reader tiếng Việt (tùy chọn).
    public string? BioVi { get; set; }

    // Mô tả reader tiếng Anh (tùy chọn).
    public string? BioEn { get; set; }

    // Mô tả reader tiếng Trung (tùy chọn).
    public string? BioZh { get; set; }

    // Mức giá mỗi câu hỏi (Diamond) nếu cần thay đổi.
    public long? DiamondPerQuestion { get; set; }

    // Danh sách chuyên môn reader (tùy chọn).
    public List<string>? Specialties { get; set; }
}

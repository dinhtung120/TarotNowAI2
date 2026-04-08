namespace TarotNow.Api.Contracts.Requests;

// Payload cập nhật hồ sơ công khai của reader.
public class UpdateReaderProfileBody
{
    // Mô tả profile bằng tiếng Việt.
    public string? BioVi { get; set; }

    // Mô tả profile bằng tiếng Anh.
    public string? BioEn { get; set; }

    // Mô tả profile bằng tiếng Trung.
    public string? BioZh { get; set; }

    // Giá theo kim cương cho mỗi câu hỏi reader nhận trả lời.
    public long? DiamondPerQuestion { get; set; }

    // Danh sách chuyên môn nổi bật của reader.
    public List<string>? Specialties { get; set; }
}

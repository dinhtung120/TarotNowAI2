/*
 * ===================================================================
 * FILE: ReaderDtos.cs
 * NAMESPACE: TarotNow.Application.Common
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Tập hợp các DTO cho tính năng READER (người đọc bài tarot chuyên nghiệp).
 *
 * CLEAN ARCHITECTURE BOUNDARY:
 *   - MongoDB lưu: ReaderRequestDocument, ReaderProfileDocument
 *   - Application layer dùng: ReaderRequestDto, ReaderProfileDto (file này)
 *   - API layer trả về: JSON từ DTO
 *   
 *   Application layer KHÔNG tham chiếu Infrastructure layer.
 *   Nó chỉ biết đến DTO, không biết Document hay MongoDB tồn tại.
 *   Đây là nguyên tắc "Dependency Inversion" trong Clean Architecture.
 *
 * CÁC DTO TRONG FILE NÀY:
 *   1. ReaderRequestDto: Đơn xin làm reader (pending/approved/rejected)
 *   2. ReaderProfileDto: Hồ sơ công khai reader (bio, giá, chuyên môn)
 * ===================================================================
 */

namespace TarotNow.Application.Common;

/// <summary>
/// DTO đại diện cho ĐƠN XIN TRỞ THÀNH READER.
///
/// Tại sao dùng DTO thay vì tham chiếu MongoDocument trực tiếp?
/// → Clean Architecture: Application layer KHÔNG được biết về Infrastructure.
/// → Application.csproj chỉ reference Domain.csproj.
/// → Repository (Infrastructure) sẽ map MongoDocument ↔ DTO.
///
/// POCO (Plain Old CLR Object): class đơn giản chỉ chứa data, không có logic.
/// Business logic nằm ở Domain layer (User entity có method PromoteToReader).
/// </summary>
public class ReaderRequestDto
{
    /// <summary>
    /// ID duy nhất - MongoDB ObjectId dạng string.
    /// Dùng để admin identify đơn khi approve/reject.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// UUID của user gửi đơn.
    /// Map với bảng users.id trong PostgreSQL.
    /// Dùng string vì MongoDB không hiểu kiểu Guid.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái đơn:
    ///   - "pending": đang chờ admin xem xét
    ///   - "approved": đã được phê duyệt → user thành reader
    ///   - "rejected": bị từ chối → user có thể gửi lại sau
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Lời giới thiệu bản thân do user viết khi nộp đơn.</summary>
    public string IntroText { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách URL tài liệu chứng minh (ảnh chứng chỉ, bằng cấp).
    /// "new()" là cú pháp C# 9+: tạo List rỗng mặc định (tránh null).
    /// </summary>
    public List<string> ProofDocuments { get; set; } = new();

    /// <summary>Ghi chú của admin khi duyệt/từ chối (lý do từ chối).</summary>
    public string? AdminNote { get; set; }

    /// <summary>UUID admin đã xem xét đơn.</summary>
    public string? ReviewedBy { get; set; }

    /// <summary>Thời điểm admin xem xét.</summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>Thời điểm user gửi đơn.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối.</summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO đại diện cho HỒ SƠ CÔNG KHAI READER.
/// Thông tin này được hiển thị trên trang profile reader (ai cũng xem được).
/// </summary>
public class ReaderProfileDto
{
    /// <summary>MongoDB ObjectId string.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// UUID user - mapping 1:1 với bảng users trong PostgreSQL.
    /// Mỗi user chỉ có tối đa 1 reader profile.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái hoạt động:
    ///   - "online": đang trực tuyến (tự động)
    ///   - "offline": không hoạt động hoặc timeout (tự động)
    ///   - "busy": đang bận rộn (chủ động)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Giá diamond cho MỖI câu hỏi. Ví dụ: 50 diamond/câu.</summary>
    public long DiamondPerQuestion { get; set; }

    /// <summary>Tiểu sử tiếng Việt (đa ngôn ngữ i18n: VI/EN/ZH).</summary>
    public string BioVi { get; set; } = string.Empty;

    /// <summary>Tiểu sử tiếng Anh.</summary>
    public string BioEn { get; set; } = string.Empty;

    /// <summary>Tiểu sử tiếng Trung.</summary>
    public string BioZh { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách chuyên môn. Ví dụ: ["tình yêu", "sự nghiệp", "tài chính"].
    /// Dùng để lọc reader theo chuyên môn trên trang danh sách.
    /// </summary>
    public List<string> Specialties { get; set; } = new();

    /// <summary>Điểm đánh giá trung bình (0.0 - 5.0) từ các lần review.</summary>
    public double AvgRating { get; set; }

    /// <summary>Tổng số lượt đánh giá.</summary>
    public int TotalReviews { get; set; }

    /// <summary>
    /// Tên hiển thị - DENORMALIZED (sao chép từ users table).
    /// "Denormalized" nghĩa là: dữ liệu bị lặp ở nhiều nơi.
    /// Lý do: tránh phải JOIN với PostgreSQL mỗi khi hiển thị reader profile.
    /// Nhược điểm: khi user đổi tên → phải cập nhật cả ở đây.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>URL ảnh đại diện (nullable vì không bắt buộc).</summary>
    public string? AvatarUrl { get; set; }

    /// <summary>Thời điểm tạo reader profile.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối.</summary>
    public DateTime? UpdatedAt { get; set; }
}

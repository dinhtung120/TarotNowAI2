namespace TarotNow.Application.Interfaces;

// Contract lưu và truy vấn log provider AI cho mục tiêu theo dõi và đối soát vận hành.
public interface IAiProviderLogRepository
{
    /// <summary>
    /// Tạo bản ghi log AI để lưu dấu vết token, độ trễ và trạng thái thực thi.
    /// Luồng xử lý: nhận DTO đã chuẩn hóa từ tầng ứng dụng và persist thành một record mới.
    /// </summary>
    Task CreateAsync(AiProviderLogCreateDto log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Truy vấn log AI theo người dùng để hỗ trợ theo dõi lịch sử và phân trang.
    /// Luồng xử lý: lọc theo userId, áp dụng page/pageSize, trả cả danh sách và tổng số bản ghi.
    /// </summary>
    Task<(IEnumerable<AiProviderLogDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
}

// DTO đầu vào khi tạo log provider AI.
public class AiProviderLogCreateDto
{
    // Định danh người dùng gửi request AI.
    public Guid UserId { get; set; }

    // Mã tham chiếu phiên đọc bài để truy vết theo ngữ cảnh.
    public string? ReadingRef { get; set; }

    // Mã tham chiếu request AI nội bộ.
    public string? AiRequestRef { get; set; }

    // Tên model thực tế đã xử lý request.
    public string Model { get; set; } = string.Empty;

    // Số token đầu vào.
    public int InputTokens { get; set; }

    // Số token đầu ra.
    public int OutputTokens { get; set; }

    // Độ trễ xử lý tính bằng mili giây.
    public int LatencyMs { get; set; }

    // Phiên bản prompt dùng tại thời điểm gọi AI.
    public string? PromptVersion { get; set; }

    // Trạng thái cuối của request AI.
    public string Status { get; set; } = "requested";

    // Mã lỗi nếu request gặp sự cố.
    public string? ErrorCode { get; set; }

    // Trace id để liên kết log ứng dụng và log hạ tầng.
    public string? TraceId { get; set; }
}

// DTO đầu ra log provider AI phục vụ hiển thị và tra soát.
public class AiProviderLogDto
{
    // Định danh bản ghi log.
    public string Id { get; set; } = string.Empty;

    // Định danh người dùng tương ứng.
    public Guid UserId { get; set; }

    // Model AI đã phục vụ request.
    public string Model { get; set; } = string.Empty;

    // Số token đầu vào đã sử dụng.
    public int InputTokens { get; set; }

    // Số token đầu ra đã sinh.
    public int OutputTokens { get; set; }

    // Độ trễ xử lý của request (ms).
    public int LatencyMs { get; set; }

    // Trạng thái request sau khi xử lý.
    public string Status { get; set; } = string.Empty;

    // Mã lỗi nếu có thất bại.
    public string? ErrorCode { get; set; }

    // Thời điểm log được tạo.
    public DateTime CreatedAt { get; set; }
}

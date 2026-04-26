

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract kết nối nhà cung cấp AI để chuẩn hóa cách stream, logging và nhận diện model.
public interface IAiProvider
{
    /// <summary>
    /// Stream phản hồi hội thoại từ AI để giảm độ trễ hiển thị cho người dùng.
    /// Luồng xử lý: gửi systemPrompt và userPrompt tới provider, sau đó trả luồng token theo thời gian thực.
    /// </summary>
    IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken);

    // Tên nhà cung cấp để phục vụ audit và routing theo cấu hình.
    string ProviderName { get; }

    // Tên model đang dùng để theo dõi hiệu năng và kiểm soát phiên bản prompt.
    string ModelName { get; }

    /// <summary>
    /// Ghi log request AI nhằm theo dõi token, độ trễ và trạng thái xử lý.
    /// Luồng xử lý: nhận payload log đã chuẩn hóa và ghi xuống kho lưu trữ tương ứng của provider.
    /// </summary>
    Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default);
}

// DTO log request để thống nhất dữ liệu giám sát giữa các provider.
public sealed class AiProviderRequestLog
{
    // Định danh người dùng tạo request AI.
    public Guid UserId { get; init; }

    // Mã phiên đọc bài để liên kết log theo ngữ cảnh phiên.
    public string? SessionId { get; init; }

    // Mã request nghiệp vụ để truy vết xuyên suốt luồng xử lý.
    public string? RequestId { get; init; }

    // Tổng token đầu vào đã gửi cho model.
    public int InputTokens { get; init; }

    // Tổng token đầu ra nhận từ model.
    public int OutputTokens { get; init; }

    // Thời gian phản hồi toàn phần của request (ms).
    public int LatencyMs { get; init; }

    // Trạng thái xử lý request để phân tích tỉ lệ thành công/thất bại.
    public string Status { get; init; } = "requested";

    // Mã lỗi chi tiết khi request thất bại hoặc bị từ chối.
    public string? ErrorCode { get; init; }

    // Phiên bản prompt áp dụng cho request này.
    public string? PromptVersion { get; init; }
}

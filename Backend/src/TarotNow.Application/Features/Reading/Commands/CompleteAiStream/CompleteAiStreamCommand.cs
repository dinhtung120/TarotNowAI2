using MediatR;
using System;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

// Command hoàn tất một AI stream và chốt nghiệp vụ billing/session theo trạng thái cuối.
public class CompleteAiStreamCommand : IRequest<bool>
{
    // Định danh bản ghi AI request cần chốt trạng thái.
    public Guid AiRequestId { get; set; }

    // Định danh user sở hữu request.
    public Guid UserId { get; set; }

    // Trạng thái kết thúc cuối cùng của stream.
    public string FinalStatus { get; set; } = AiStreamFinalStatuses.Completed;

    // Thông tin lỗi chi tiết khi request thất bại (nếu có).
    public string? ErrorMessage { get; set; }

    // Cờ cho biết stream dừng do client ngắt kết nối.
    public bool IsClientDisconnect { get; set; }

    // Mốc thời gian token đầu tiên được phát (nếu có).
    public DateTimeOffset? FirstTokenAt { get; set; }

    // Tổng output token đã sinh từ provider.
    public int OutputTokens { get; set; }

    // Tổng input token ước lượng đã gửi lên provider.
    public int InputTokens { get; set; }

    // Độ trễ tổng của request (ms).
    public int LatencyMs { get; set; }

    // Nội dung trả lời đầy đủ cuối cùng từ AI (nếu có).
    public string? FullResponse { get; set; }

    // Câu hỏi follow-up tương ứng với FullResponse (nếu có).
    public string? FollowupQuestion { get; set; }
}

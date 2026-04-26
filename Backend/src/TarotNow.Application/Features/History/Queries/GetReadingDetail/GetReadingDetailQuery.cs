using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.History.Queries.GetReadingDetail;

// Query lấy chi tiết một reading session của user.
public class GetReadingDetailQuery : IRequest<GetReadingDetailResponse?>
{
    // Định danh user yêu cầu dữ liệu.
    public Guid UserId { get; set; }

    // Định danh reading session cần lấy.
    public string SessionId { get; set; } = string.Empty;
}

// DTO chi tiết reading session.
public class GetReadingDetailResponse
{
    // Định danh session.
    public string Id { get; set; } = string.Empty;

    // Loại spread của reading.
    public string SpreadType { get; set; } = string.Empty;

    // Chuỗi cards đã rút (nếu có).
    public string? CardsDrawn { get; set; }

    // Danh sách cards đã rút kèm orientation.
    public IEnumerable<DrawnCardDto> DrawnCards { get; set; } = new List<DrawnCardDto>();

    // Cờ session đã hoàn thành hay chưa.
    public bool IsCompleted { get; set; }

    // Thời điểm tạo session.
    public DateTime CreatedAt { get; set; }

    // Thời điểm hoàn thành session (nếu có).
    public DateTime? CompletedAt { get; set; }

    // Tóm tắt AI của phiên đọc bài.
    public string? AiSummary { get; set; }

    // Danh sách follow-up hỏi đáp.
    public IEnumerable<FollowupDto> Followups { get; set; } = new List<FollowupDto>();

    // Danh sách tương tác AI theo timeline.
    public IEnumerable<AiRequestDto> AiInteractions { get; set; } = new List<AiRequestDto>();
}

// DTO một lá đã rút trong phiên đọc.
public class DrawnCardDto
{
    // Id lá bài trong bộ 78 lá.
    public int CardId { get; set; }

    // Vị trí của lá trong spread.
    public int Position { get; set; }

    // Orientation của lá (upright/reversed).
    public string Orientation { get; set; } = string.Empty;
}

// DTO follow-up trong reading session.
public class FollowupDto
{
    // Câu hỏi follow-up.
    public string Question { get; set; } = string.Empty;

    // Câu trả lời follow-up.
    public string Answer { get; set; } = string.Empty;
}

// DTO một request AI trong reading session.
public class AiRequestDto
{
    // Định danh request AI.
    public Guid Id { get; set; }

    // Trạng thái xử lý request.
    public string Status { get; set; } = string.Empty;

    // Lý do kết thúc request (nếu có).
    public string? FinishReason { get; set; }

    // Lượng kim cương đã tính phí cho request.
    public long ChargeDiamond { get; set; }

    // Thời điểm tạo request AI.
    public DateTimeOffset CreatedAt { get; set; }

    // Loại request suy ra từ idempotency key/cost.
    public string? RequestType { get; set; }
}

// Handler truy vấn chi tiết reading session.
public class GetReadingDetailQueryHandler : IRequestHandler<GetReadingDetailQuery, GetReadingDetailResponse?>
{
    private readonly IReadingSessionRepository _readingRepo;

    /// <summary>
    /// Khởi tạo handler get reading detail.
    /// Luồng xử lý: nhận reading repository để tải session và các AI request đi kèm.
    /// </summary>
    public GetReadingDetailQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    /// <summary>
    /// Xử lý query lấy chi tiết reading.
    /// Luồng xử lý: tải session kèm AI requests, kiểm tra quyền sở hữu user, rồi map dữ liệu chi tiết ra response.
    /// </summary>
    public async Task<GetReadingDetailResponse?> Handle(GetReadingDetailQuery request, CancellationToken cancellationToken)
    {
        var session = await _readingRepo.GetSessionWithAiRequestsAsync(request.SessionId, cancellationToken);

        if (!session.HasValue)
        {
            // Session không tồn tại thì trả null để caller quyết định phản hồi.
            return null;
        }

        if (session.Value.ReadingSession.UserId != request.UserId.ToString())
        {
            // Chặn truy cập trái phép reading session của user khác.
            throw new ForbiddenException("Reading session not found or access denied");
        }

        return new GetReadingDetailResponse
        {
            Id = session.Value.ReadingSession.Id,
            SpreadType = session.Value.ReadingSession.SpreadType,
            CardsDrawn = session.Value.ReadingSession.CardsDrawn,
            DrawnCards = ReadingDrawnCardCodec.Parse(session.Value.ReadingSession.CardsDrawn)
                .Select(card => new DrawnCardDto
                {
                    CardId = card.CardId,
                    Position = card.Position,
                    Orientation = card.Orientation
                })
                .ToList(),
            IsCompleted = session.Value.ReadingSession.IsCompleted,
            CreatedAt = session.Value.ReadingSession.CreatedAt,
            CompletedAt = session.Value.ReadingSession.CompletedAt,
            AiSummary = session.Value.ReadingSession.AiSummary,
            Followups = session.Value.ReadingSession.Followups.Select(f => new FollowupDto
            {
                Question = f.Question,
                Answer = f.Answer
            }).ToList(),
            AiInteractions = session.Value.AiRequests.Select(a => new AiRequestDto
            {
                Id = a.Id,
                Status = a.Status,
                FinishReason = a.FinishReason,
                ChargeDiamond = a.ChargeDiamond,
                CreatedAt = a.CreatedAt,
                RequestType = a.FollowupSequence.HasValue ? "Followup" : "InitialReading"
            }).OrderBy(x => x.CreatedAt)
        };
    }
}

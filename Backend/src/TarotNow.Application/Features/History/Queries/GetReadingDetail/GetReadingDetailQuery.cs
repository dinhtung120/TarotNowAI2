using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetReadingDetail;

public class GetReadingDetailQuery : IRequest<GetReadingDetailResponse>
{
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
}

public class GetReadingDetailResponse
{
    public Guid Id { get; set; }
    public string SpreadType { get; set; } = string.Empty;
    public string? CardsDrawn { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Thông tin các câu hỏi và trả lời AI
    // Trong thực tế, AI trả lời có thể lấy qua lịch sử chat lưu riêng, 
    // hoặc lấy metadata từ AiRequests. Cho MVP, ta gửi danh sách Request cơ bản.
    public IEnumerable<AiRequestDto> AiInteractions { get; set; } = new List<AiRequestDto>();
}

public class AiRequestDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FinishReason { get; set; }
    public long ChargeDiamond { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    // Thuộc tính tùy chọn để Frontend parse Prompt (nếu lưu trong IdempotencyKey cho debug)
    public string? RequestType { get; set; }
}

public class GetReadingDetailQueryHandler : IRequestHandler<GetReadingDetailQuery, GetReadingDetailResponse>
{
    private readonly IReadingSessionRepository _readingRepo;

    public GetReadingDetailQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    public async Task<GetReadingDetailResponse> Handle(GetReadingDetailQuery request, CancellationToken cancellationToken)
    {
        var session = await _readingRepo.GetSessionWithAiRequestsAsync(request.SessionId, cancellationToken);

        if (!session.HasValue)
            return null; // Return null as expected by the test: Handle_ShouldReturnNull_WhenSessionDoesNotExist

        if (session.Value.ReadingSession.UserId != request.UserId)
            throw new UnauthorizedAccessException("Reading session not found or access denied");

        return new GetReadingDetailResponse
        {
            Id = session.Value.ReadingSession.Id,
            SpreadType = session.Value.ReadingSession.SpreadType,
            CardsDrawn = session.Value.ReadingSession.CardsDrawn,
            IsCompleted = session.Value.ReadingSession.IsCompleted,
            CreatedAt = session.Value.ReadingSession.CreatedAt,
            CompletedAt = session.Value.ReadingSession.CompletedAt,
            AiInteractions = session.Value.AiRequests.Select(a => new AiRequestDto
            {
                Id = a.Id,
                Status = a.Status,
                FinishReason = a.FinishReason,
                ChargeDiamond = a.ChargeDiamond,
                CreatedAt = a.CreatedAt,
                RequestType = a.IdempotencyKey != null && a.IdempotencyKey.Contains("ai_stream") ? 
                              (a.ChargeDiamond > 5 ? "Followup" : "InitialReading") : "Unknown"
            }).OrderBy(x => x.CreatedAt)
        };
    }
}

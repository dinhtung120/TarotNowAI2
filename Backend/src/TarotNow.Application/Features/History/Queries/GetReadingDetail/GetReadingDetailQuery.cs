

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetReadingDetail;

public class GetReadingDetailQuery : IRequest<GetReadingDetailResponse?>
{
        public Guid UserId { get; set; }
    
        public string SessionId { get; set; } = string.Empty;
}

public class GetReadingDetailResponse
{
    public string Id { get; set; } = string.Empty;
    public string SpreadType { get; set; } = string.Empty;
    
        public string? CardsDrawn { get; set; }
    
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    
    
    
    
    public string? AiSummary { get; set; }
    public IEnumerable<FollowupDto> Followups { get; set; } = new List<FollowupDto>();

        public IEnumerable<AiRequestDto> AiInteractions { get; set; } = new List<AiRequestDto>();
}

public class FollowupDto
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}

public class AiRequestDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FinishReason { get; set; }
    
        public long ChargeDiamond { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
        public string? RequestType { get; set; }
}

public class GetReadingDetailQueryHandler : IRequestHandler<GetReadingDetailQuery, GetReadingDetailResponse?>
{
    private readonly IReadingSessionRepository _readingRepo;

    public GetReadingDetailQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    public async Task<GetReadingDetailResponse?> Handle(GetReadingDetailQuery request, CancellationToken cancellationToken)
    {
        
        var session = await _readingRepo.GetSessionWithAiRequestsAsync(request.SessionId, cancellationToken);

        
        if (!session.HasValue)
            return null; 

        
        
        
        
        if (session.Value.ReadingSession.UserId != request.UserId.ToString())
            throw new UnauthorizedAccessException("Reading session not found or access denied");

        
        return new GetReadingDetailResponse
        {
            Id = session.Value.ReadingSession.Id,
            SpreadType = session.Value.ReadingSession.SpreadType,
            CardsDrawn = session.Value.ReadingSession.CardsDrawn,
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
                
                
                
                RequestType = a.IdempotencyKey != null && a.IdempotencyKey.Contains("ai_stream") ? 
                              (a.ChargeDiamond > 5 ? "Followup" : "InitialReading") : "Unknown"
            }).OrderBy(x => x.CreatedAt) 
        };
    }
}

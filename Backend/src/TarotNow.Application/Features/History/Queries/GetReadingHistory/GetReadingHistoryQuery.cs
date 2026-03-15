using MediatR;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetReadingHistory;

public class GetReadingHistoryQuery : IRequest<GetReadingHistoryResponse>
{
    public Guid UserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetReadingHistoryResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<ReadingSessionDto> Items { get; set; } = new List<ReadingSessionDto>();
}

public class ReadingSessionDto
{
    public Guid Id { get; set; }
    public string SpreadType { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetReadingHistoryQueryHandler : IRequestHandler<GetReadingHistoryQuery, GetReadingHistoryResponse>
{
    private readonly IReadingSessionRepository _readingRepo;

    public GetReadingHistoryQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    public async Task<GetReadingHistoryResponse> Handle(GetReadingHistoryQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _readingRepo.GetSessionsByUserIdAsync(request.UserId, request.Page, request.PageSize, cancellationToken);

        var dtos = items.Select(s => new ReadingSessionDto
        {
            Id = s.Id,
            SpreadType = s.SpreadType,
            IsCompleted = s.IsCompleted,
            CreatedAt = s.CreatedAt
        });

        return new GetReadingHistoryResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = dtos
        };
    }
}

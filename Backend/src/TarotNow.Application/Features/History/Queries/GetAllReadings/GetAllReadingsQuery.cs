

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetAllReadings;

public class GetAllReadingsQuery : IRequest<GetAllReadingsResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    
    
    
    
        public string? Username { get; set; }
    
        public string? SpreadType { get; set; }
    
        public DateTime? StartDate { get; set; }
    
        public DateTime? EndDate { get; set; }
}

public class GetAllReadingsResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<AdminReadingDto> Items { get; set; } = new List<AdminReadingDto>();
}

public class AdminReadingDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    
        public string Username { get; set; } = string.Empty; 
    
    public string SpreadType { get; set; } = string.Empty;
    public string? Question { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

public partial class GetAllReadingsQueryHandler : IRequestHandler<GetAllReadingsQuery, GetAllReadingsResponse>
{
    private readonly IReadingSessionRepository _readingRepo;
    
    
    private readonly IUserRepository _userRepo; 

    public GetAllReadingsQueryHandler(IReadingSessionRepository readingRepo, IUserRepository userRepo)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
    }

    public async Task<GetAllReadingsResponse> Handle(GetAllReadingsQuery request, CancellationToken cancellationToken)
    {
        var filteredUserIds = await ResolveUserFilterAsync(request.Username, cancellationToken);
        if (filteredUserIds is { Count: 0 })
        {
            return BuildEmptyResponse(request);
        }

        var (items, totalCount) = await LoadReadingsAsync(request, filteredUserIds, cancellationToken);
        var dtos = await BuildDtosAsync(items, cancellationToken);
        return BuildResponse(request, totalCount, dtos);
    }
}

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetAllReadings;

public class GetAllReadingsQuery : IRequest<GetAllReadingsResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    // Filters
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
    public string Username { get; set; } = string.Empty; // Thêm username để hiển thị thay cho ID
    public string SpreadType { get; set; } = string.Empty;
    public string? Question { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetAllReadingsQueryHandler : IRequestHandler<GetAllReadingsQuery, GetAllReadingsResponse>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IUserRepository _userRepo; // Cần truy vấn thông tin user từ SQL

    public GetAllReadingsQueryHandler(IReadingSessionRepository readingRepo, IUserRepository userRepo)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
    }

    public async Task<GetAllReadingsResponse> Handle(GetAllReadingsQuery request, CancellationToken cancellationToken)
    {
        List<Guid>? filteredUserIds = null;

        // 1. Nếu có lọc theo Username, tìm IDs tương ứng từ SQL
        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            var usernameLower = request.Username.Trim().ToLowerInvariant();
            var users = await _userRepo.SearchUsersByUsernameAsync(usernameLower, cancellationToken);
            filteredUserIds = users.Select(u => u.Id).ToList();
            
            // Nếu lọc theo tên mà không thấy user nào => trả về rỗng luôn (Tránh MongoDB trả về hết)
            if (filteredUserIds.Count == 0)
            {
                return new GetAllReadingsResponse { Page = request.Page, PageSize = request.PageSize, TotalCount = 0, Items = new List<AdminReadingDto>() };
            }
        }

        // 2. Lấy dữ liệu từ MongoDB với các filter
        var (items, totalCount) = await _readingRepo.GetAllSessionsAsync(
            request.Page, 
            request.PageSize, 
            filteredUserIds?.Select(id => id.ToString()).ToList(),
            request.SpreadType,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        // 3. Thu thập tất cả UserIds trong kết quả để lấy Username hàng loạt (Batching)
        // Lưu ý: UserRepo vẫn nhận Guid nên cần parse ở bước này để query SQL
        var parsedItems = items
            .Select(s => new
            {
                Session = s,
                ParsedUserId = Guid.TryParse(s.UserId, out var guid) ? guid : (Guid?)null
            })
            .ToList();

        var userGuids = parsedItems
            .Where(x => x.ParsedUserId.HasValue)
            .Select(x => x.ParsedUserId!.Value)
            .Distinct()
            .ToList();
            
        var userMap = await _userRepo.GetUsernameMapAsync(userGuids, cancellationToken);

        var dtos = parsedItems.Select(x => {
            var foundUsername = x.ParsedUserId.HasValue && userMap.TryGetValue(x.ParsedUserId.Value, out var uname)
                ? uname
                : "Unknown";
            
            return new AdminReadingDto
            {
                Id = x.Session.Id,
                UserId = x.Session.UserId,
                Username = foundUsername,
                SpreadType = x.Session.SpreadType,
                Question = x.Session.Question,
                IsCompleted = x.Session.IsCompleted,
                CreatedAt = x.Session.CreatedAt
            };
        });

        return new GetAllReadingsResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = dtos
        };
    }
}

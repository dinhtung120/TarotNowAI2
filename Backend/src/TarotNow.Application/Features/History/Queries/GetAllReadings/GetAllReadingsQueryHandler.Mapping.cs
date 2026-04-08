namespace TarotNow.Application.Features.History.Queries.GetAllReadings;

public partial class GetAllReadingsQueryHandler
{
    /// <summary>
    /// Resolve bộ lọc user theo username.
    /// Luồng xử lý: nếu username rỗng thì không lọc; nếu có thì search user và trả danh sách user id phù hợp.
    /// </summary>
    private async Task<List<Guid>?> ResolveUserFilterAsync(string? username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            // Không truyền username thì bỏ qua filter user.
            return null;
        }

        var users = await _userRepo.SearchUsersByUsernameAsync(username.Trim().ToLowerInvariant(), cancellationToken);
        return users.Select(user => user.Id).ToList();
    }

    /// <summary>
    /// Tải danh sách reading sessions theo phân trang và bộ lọc.
    /// Luồng xử lý: gọi repository với page/pageSize, user filter, spread type và khoảng ngày.
    /// </summary>
    private async Task<(IEnumerable<Domain.Entities.ReadingSession> Items, int TotalCount)> LoadReadingsAsync(
        GetAllReadingsQuery request,
        List<Guid>? filteredUserIds,
        CancellationToken cancellationToken)
    {
        return await _readingRepo.GetAllSessionsAsync(
            request.Page,
            request.PageSize,
            filteredUserIds?.Select(id => id.ToString()).ToList(),
            request.SpreadType,
            request.StartDate,
            request.EndDate,
            cancellationToken);
    }

    /// <summary>
    /// Map danh sách reading session sang DTO hiển thị admin.
    /// Luồng xử lý: parse user id hợp lệ, tải username map theo batch rồi build DTO từng session.
    /// </summary>
    private async Task<IEnumerable<AdminReadingDto>> BuildDtosAsync(
        IEnumerable<Domain.Entities.ReadingSession> sessions,
        CancellationToken cancellationToken)
    {
        var parsedItems = sessions
            .Select(session => new
            {
                Session = session,
                ParsedUserId = Guid.TryParse(session.UserId, out var userId) ? userId : (Guid?)null
            })
            .ToList();

        var userGuids = parsedItems
            .Where(item => item.ParsedUserId.HasValue)
            .Select(item => item.ParsedUserId!.Value)
            .Distinct()
            .ToList();

        var userMap = await _userRepo.GetUsernameMapAsync(userGuids, cancellationToken);
        return parsedItems.Select(item => BuildDto(item.Session, item.ParsedUserId, userMap));
    }

    /// <summary>
    /// Build một AdminReadingDto từ entity reading session.
    /// Luồng xử lý: resolve username theo userMap, fallback "Unknown" khi thiếu mapping.
    /// </summary>
    private static AdminReadingDto BuildDto(
        Domain.Entities.ReadingSession session,
        Guid? userId,
        IReadOnlyDictionary<Guid, string> userMap)
    {
        var username = userId.HasValue && userMap.TryGetValue(userId.Value, out var mappedName)
            ? mappedName
            : "Unknown";

        return new AdminReadingDto
        {
            Id = session.Id,
            UserId = session.UserId,
            Username = username,
            SpreadType = session.SpreadType,
            Question = session.Question,
            IsCompleted = session.IsCompleted,
            CreatedAt = session.CreatedAt
        };
    }

    /// <summary>
    /// Dựng response rỗng cho truy vấn readings.
    /// Luồng xử lý: giữ thông tin page/pageSize đầu vào và trả tổng bằng 0.
    /// </summary>
    private static GetAllReadingsResponse BuildEmptyResponse(GetAllReadingsQuery request)
    {
        return new GetAllReadingsResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = 0,
            TotalPages = 0,
            Items = []
        };
    }

    /// <summary>
    /// Dựng response phân trang cho kết quả readings.
    /// Luồng xử lý: map metadata phân trang và danh sách item đã build DTO.
    /// </summary>
    private static GetAllReadingsResponse BuildResponse(
        GetAllReadingsQuery request,
        int totalCount,
        IEnumerable<AdminReadingDto> items)
    {
        return new GetAllReadingsResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = items
        };
    }
}

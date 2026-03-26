namespace TarotNow.Application.Features.History.Queries.GetAllReadings;

public partial class GetAllReadingsQueryHandler
{
    private async Task<List<Guid>?> ResolveUserFilterAsync(string? username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        var users = await _userRepo.SearchUsersByUsernameAsync(username.Trim().ToLowerInvariant(), cancellationToken);
        return users.Select(user => user.Id).ToList();
    }

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

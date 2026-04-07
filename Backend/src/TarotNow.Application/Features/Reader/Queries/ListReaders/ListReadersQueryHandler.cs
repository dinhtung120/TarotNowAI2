

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

public class ListReadersQueryHandler : IRequestHandler<ListReadersQuery, ListReadersResult>
{
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    public ListReadersQueryHandler(
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    public async Task<ListReadersResult> Handle(ListReadersQuery request, CancellationToken cancellationToken)
    {
        var (profiles, totalCount) = await _readerProfileRepository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            request.Specialty,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        
        
        var profileList = profiles.ToList();
        var needsEnrichment = profileList
            .Where(p => string.IsNullOrEmpty(p.AvatarUrl) && !string.IsNullOrEmpty(p.UserId))
            .ToList();

        if (needsEnrichment.Count > 0)
        {
            var userIds = needsEnrichment
                .Select(p => Guid.TryParse(p.UserId, out var g) ? g : (Guid?)null)
                .Where(g => g.HasValue)
                .Select(g => g!.Value);

            var userInfoMap = await _userRepository.GetUserBasicInfoMapAsync(userIds, cancellationToken);

            foreach (var profile in needsEnrichment)
            {
                if (Guid.TryParse(profile.UserId, out var guid) && userInfoMap.TryGetValue(guid, out var info))
                {
                    if (!string.IsNullOrEmpty(info.AvatarUrl))
                    {
                        profile.AvatarUrl = info.AvatarUrl;
                    }
                }
            }
        }

        return new ListReadersResult
        {
            Readers = profileList,
            TotalCount = totalCount
        };
    }
}

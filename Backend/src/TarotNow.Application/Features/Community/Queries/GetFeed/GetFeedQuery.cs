

using MediatR;
using AutoMapper;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Queries.GetFeed;

public class GetFeedQuery : IRequest<(IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)>
{
    public Guid ViewerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? AuthorFilter { get; set; }
    public string? VisibilityFilter { get; set; }
}

public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, (IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)>
{
    private const int MinPage = 1;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 50;

    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityReactionRepository _reactionRepo;
    private readonly IMapper _mapper;

    public GetFeedQueryHandler(
        ICommunityPostRepository postRepo,
        ICommunityReactionRepository reactionRepo,
        IMapper mapper)
    {
        _postRepo = postRepo;
        _reactionRepo = reactionRepo;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        NormalizePaging(request);

        var (posts, total) = await _postRepo.GetFeedAsync(
            request.Page,
            request.PageSize,
            request.ViewerId.ToString(),
            request.AuthorFilter,
            request.VisibilityFilter,
            cancellationToken);

        if (!posts.Any())
            return (Enumerable.Empty<CommunityPostFeedItemDto>(), total);

        var postIds = posts.Select(p => p.Id).ToList();
        var userReactions = await _reactionRepo.GetUserReactionsForPostsAsync(
            request.ViewerId.ToString(),
            postIds,
            cancellationToken);

        var feedItems = MapFeedItems(posts, userReactions);
        return (feedItems, total);
    }

    private static void NormalizePaging(GetFeedQuery request)
    {
        if (request.Page < MinPage) request.Page = MinPage;
        if (request.PageSize < MinPageSize || request.PageSize > MaxPageSize) request.PageSize = MaxPageSize;
    }

    private IEnumerable<CommunityPostFeedItemDto> MapFeedItems(
        IEnumerable<CommunityPostDto> posts,
        IReadOnlyDictionary<string, string> userReactions)
    {
        return posts.Select(post =>
        {
            var mapped = _mapper.Map<CommunityPostFeedItemDto>(post);
            var postId = post.Id;
            mapped.ViewerReaction = userReactions.ContainsKey(postId)
                ? userReactions[postId]
                : null;
            return mapped;
        });
    }
}

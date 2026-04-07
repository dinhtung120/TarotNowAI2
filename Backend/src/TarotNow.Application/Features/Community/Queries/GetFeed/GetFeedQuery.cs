

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
        
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1 || request.PageSize > 50) request.PageSize = 50;

        
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

        
        var feedItems = posts.Select(p =>
        {
            var mapped = _mapper.Map<CommunityPostFeedItemDto>(p);
            mapped.ViewerReaction = userReactions.GetValueOrDefault(p.Id);
            return mapped;
        });

        return (feedItems, total);
    }
}

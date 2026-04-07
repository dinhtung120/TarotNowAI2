

using MediatR;
using AutoMapper;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Queries.GetPostDetail;

public class GetPostDetailQuery : IRequest<CommunityPostFeedItemDto>
{
    public string PostId { get; set; } = string.Empty;
    public Guid ViewerId { get; set; }
}

public class GetPostDetailQueryHandler : IRequestHandler<GetPostDetailQuery, CommunityPostFeedItemDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityReactionRepository _reactionRepo;
    private readonly IMapper _mapper;

    public GetPostDetailQueryHandler(
        ICommunityPostRepository postRepo,
        ICommunityReactionRepository reactionRepo,
        IMapper mapper)
    {
        _postRepo = postRepo;
        _reactionRepo = reactionRepo;
        _mapper = mapper;
    }

    public async Task<CommunityPostFeedItemDto> Handle(GetPostDetailQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");

        var viewerId = request.ViewerId.ToString();
        if (post.Visibility == PostVisibility.Private && post.AuthorId != viewerId)
            throw new ForbiddenException("Bạn không có quyền xem bài viết riêng tư này.");

        var viewerReaction = await _reactionRepo.GetAsync(request.PostId, viewerId, cancellationToken);

        var mapped = _mapper.Map<CommunityPostFeedItemDto>(post);
        mapped.ViewerReaction = viewerReaction?.Type;
        return mapped;
    }
}

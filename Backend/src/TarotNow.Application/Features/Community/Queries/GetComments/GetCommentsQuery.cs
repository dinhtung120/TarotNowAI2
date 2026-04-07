using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Community.Queries.GetComments;

public class GetCommentsQuery : IRequest<(IEnumerable<CommunityCommentDto> Items, long TotalCount)>
{
    public required string PostId { get; set; }
    public Guid? ViewerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, (IEnumerable<CommunityCommentDto> Items, long TotalCount)>
{
    private readonly ICommunityCommentRepository _commentRepo;
    private readonly ICommunityPostRepository _postRepo;

    public GetCommentsQueryHandler(ICommunityCommentRepository commentRepo, ICommunityPostRepository postRepo)
    {
        _commentRepo = commentRepo;
        _postRepo = postRepo;
    }

    public async Task<(IEnumerable<CommunityCommentDto> Items, long TotalCount)> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1 || request.PageSize > 50) request.PageSize = 50;

        
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
            throw new NotFoundException("Bài viết không tồn tại.");

        if (post.Visibility == PostVisibility.Private 
            && post.AuthorId != request.ViewerId?.ToString())
            throw new ForbiddenException("Không có quyền xem bình luận bài viết riêng tư.");

        return await _commentRepo.GetByPostIdAsync(request.PostId, request.Page, request.PageSize, cancellationToken);
    }
}



using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.CreatePost;

// Command tạo bài viết community mới.
public class CreatePostCommand : IRequest<CommunityPostDto>
{
    // Định danh tác giả bài viết.
    public Guid AuthorId { get; set; }

    // Nội dung bài viết.
    public string Content { get; set; } = string.Empty;

    // Mức hiển thị của bài viết (public/private).
    public string Visibility { get; set; } = PostVisibility.Public;

    // Draft id để map asset upload trước khi post thật được tạo.
    public string? ContextDraftId { get; set; }
}

// Handler xử lý tạo community post.
public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CommunityPostDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private readonly IGamificationService _gamificationService;
    private readonly ICommunityMediaAttachmentService _communityMediaAttachmentService;

    /// <summary>
    /// Khởi tạo handler create post.
    /// Luồng xử lý: nhận repository bài viết/user và service gamification để tạo post + cộng tiến trình.
    /// </summary>
    public CreatePostCommandHandler(
        ICommunityPostRepository postRepo,
        IUserRepository userRepo,
        IGamificationService gamificationService,
        ICommunityMediaAttachmentService communityMediaAttachmentService)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _gamificationService = gamificationService;
        _communityMediaAttachmentService = communityMediaAttachmentService;
    }

    /// <summary>
    /// Xử lý command tạo bài viết.
    /// Luồng xử lý: validate content/visibility, kiểm tra user tồn tại, tạo DTO post mới, lưu repository và kích hoạt gamification.
    /// </summary>
    public async Task<CommunityPostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
        {
            // Rule nghiệp vụ: bài viết phải có nội dung và nằm trong giới hạn chiều dài.
            throw new BadRequestException("Nội dung bài viết không được trống và tối đa 5000 ký tự.");
        }

        var validVisibilities = new[] { PostVisibility.Public, PostVisibility.Private };
        if (!validVisibilities.Contains(request.Visibility))
        {
            // Chặn visibility ngoài tập hỗ trợ để tránh dữ liệu sai schema hiển thị.
            throw new BadRequestException("Quyền riêng tư không hợp lệ.");
        }

        var user = await _userRepo.GetByIdAsync(request.AuthorId, cancellationToken);
        if (user == null)
        {
            // Edge case: user không tồn tại.
            throw new NotFoundException("User không tồn tại.");
        }

        var newPost = new CommunityPostDto
        {
            AuthorId = request.AuthorId.ToString(),
            AuthorDisplayName = user.DisplayName,
            AuthorAvatarUrl = user.AvatarUrl,
            Content = request.Content.Trim(),
            Visibility = request.Visibility,
            CreatedAt = DateTime.UtcNow,
            ReactionsCount = new Dictionary<string, int>(),
            TotalReactions = 0,
            IsDeleted = false
        };

        // Lưu post mới trước để đảm bảo có dữ liệu nguồn cho các bước tiếp theo.
        var result = await _postRepo.CreateAsync(newPost, cancellationToken);

        // Cập nhật gamification sau khi tạo post thành công.
        await _gamificationService.OnPostCreatedAsync(request.AuthorId, cancellationToken);
        await _communityMediaAttachmentService.AttachForNewEntityAsync(
            request.AuthorId,
            "post",
            request.ContextDraftId,
            result.Id,
            result.Content,
            cancellationToken);

        return result;
    }
}

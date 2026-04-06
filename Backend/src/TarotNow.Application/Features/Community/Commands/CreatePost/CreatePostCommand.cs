/*
 * ===================================================================
 * FILE: CreatePostCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Commands.CreatePost
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý logic tạo bài viết cộng đồng mới.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.CreatePost;

public class CreatePostCommand : IRequest<CommunityPostDto>
{
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Visibility { get; set; } = PostVisibility.Public;
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CommunityPostDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private readonly IGamificationService _gamificationService;

    public CreatePostCommandHandler(
        ICommunityPostRepository postRepo, 
        IUserRepository userRepo,
        IGamificationService gamificationService)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _gamificationService = gamificationService;
    }

    public async Task<CommunityPostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Business Logic
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
            throw new BadRequestException("Nội dung bài viết không được trống và tối đa 5000 ký tự.");

        var validVisibilities = new[] { PostVisibility.Public, PostVisibility.Private };
        if (!validVisibilities.Contains(request.Visibility))
            throw new BadRequestException("Quyền riêng tư không hợp lệ.");

        // 2. Fetch User để lấy Name/Avatar (Denormalization)
        var user = await _userRepo.GetByIdAsync(request.AuthorId, cancellationToken);
        if (user == null)
            throw new NotFoundException("User không tồn tại.");

        // 3. Khởi tạo DTO
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

        // 4. Update Database
        var result = await _postRepo.CreateAsync(newPost, cancellationToken);

        // 5. Trigger Gamification
        await _gamificationService.OnPostCreatedAsync(request.AuthorId, cancellationToken);

        return result;
    }
}

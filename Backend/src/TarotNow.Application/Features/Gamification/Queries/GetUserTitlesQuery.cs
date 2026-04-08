using MediatR;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

// Query lấy danh sách title user sở hữu và title đang active.
public record GetUserTitlesQuery(Guid UserId) : IRequest<UserTitlesDto>;

// Handler truy vấn title của user.
public class GetUserTitlesQueryHandler : IRequestHandler<GetUserTitlesQuery, UserTitlesDto>
{
    private readonly ITitleRepository _titleRepo;
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Khởi tạo handler get user titles.
    /// Luồng xử lý: nhận title repository và user repository để tổng hợp definitions, unlocked titles và active title.
    /// </summary>
    public GetUserTitlesQueryHandler(ITitleRepository titleRepo, IUserRepository userRepo)
    {
        _titleRepo = titleRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Xử lý query lấy title của user.
    /// Luồng xử lý: tải thông tin user, tải toàn bộ title definitions, tải unlocked titles và map sang UserTitlesDto.
    /// </summary>
    public async Task<UserTitlesDto> Handle(GetUserTitlesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        var allTitles = await _titleRepo.GetAllTitlesAsync(cancellationToken);
        var userTitles = await _titleRepo.GetUserTitlesAsync(request.UserId, cancellationToken);

        return new UserTitlesDto
        {
            Definitions = allTitles.Select(t => new TitleDefinitionDto
            {
                Code = t.Code,
                NameVi = t.NameVi,
                NameEn = t.NameEn,
                NameZh = t.NameZh,
                DescriptionVi = t.DescriptionVi,
                DescriptionEn = t.DescriptionEn,
                DescriptionZh = t.DescriptionZh,
                Rarity = t.Rarity,
                IsActive = t.IsActive
            }).ToList(),
            UnlockedList = userTitles.Select(ut => new UserTitleDto
            {
                TitleCode = ut.TitleCode,
                GrantedAt = ut.GrantedAt
            }).ToList(),
            // Có thể null nếu user chưa chọn title active.
            ActiveTitleCode = user?.ActiveTitleRef
        };
    }
}

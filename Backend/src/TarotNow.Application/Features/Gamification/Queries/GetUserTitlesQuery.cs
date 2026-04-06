using MediatR;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

public record GetUserTitlesQuery(Guid UserId) : IRequest<UserTitlesDto>;

public class GetUserTitlesQueryHandler : IRequestHandler<GetUserTitlesQuery, UserTitlesDto>
{
    private readonly ITitleRepository _titleRepo;
    private readonly IUserRepository _userRepo;

    public GetUserTitlesQueryHandler(ITitleRepository titleRepo, IUserRepository userRepo)
    {
        _titleRepo = titleRepo;
        _userRepo = userRepo;
    }

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
            ActiveTitleCode = user?.ActiveTitleRef
        };
    }
}

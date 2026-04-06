using MediatR;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

public record GetUserAchievementsQuery(Guid UserId) : IRequest<UserAchievementsDto>;

public class GetUserAchievementsQueryHandler : IRequestHandler<GetUserAchievementsQuery, UserAchievementsDto>
{
    private readonly IAchievementRepository _achRepo;

    public GetUserAchievementsQueryHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    public async Task<UserAchievementsDto> Handle(GetUserAchievementsQuery request, CancellationToken cancellationToken)
    {
        var definitions = await _achRepo.GetAllAchievementsAsync(cancellationToken);
        var unlocked = await _achRepo.GetUserAchievementsAsync(request.UserId, cancellationToken);
        return new UserAchievementsDto { Definitions = definitions, UnlockedList = unlocked };
    }
}

using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;


public record GetAllQuestsAdminQuery : IRequest<List<QuestDefinitionDto>>;

public class GetAllQuestsAdminQueryHandler : IRequestHandler<GetAllQuestsAdminQuery, List<QuestDefinitionDto>>
{
    private readonly IQuestRepository _questRepo;

    public GetAllQuestsAdminQueryHandler(IQuestRepository questRepo)
    {
        _questRepo = questRepo;
    }

    public async Task<List<QuestDefinitionDto>> Handle(GetAllQuestsAdminQuery request, CancellationToken cancellationToken)
    {
        return await _questRepo.GetAllQuestsAsync(cancellationToken);
    }
}


public record GetAllAchievementsAdminQuery : IRequest<List<AchievementDefinitionDto>>;

public class GetAllAchievementsAdminQueryHandler : IRequestHandler<GetAllAchievementsAdminQuery, List<AchievementDefinitionDto>>
{
    private readonly IAchievementRepository _achRepo;

    public GetAllAchievementsAdminQueryHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    public async Task<List<AchievementDefinitionDto>> Handle(GetAllAchievementsAdminQuery request, CancellationToken cancellationToken)
    {
        return await _achRepo.GetAllAchievementsAsync(cancellationToken);
    }
}


public record GetAllTitlesAdminQuery : IRequest<List<TitleDefinitionDto>>;

public class GetAllTitlesAdminQueryHandler : IRequestHandler<GetAllTitlesAdminQuery, List<TitleDefinitionDto>>
{
    private readonly ITitleRepository _titleRepo;

    public GetAllTitlesAdminQueryHandler(ITitleRepository titleRepo)
    {
        _titleRepo = titleRepo;
    }

    public async Task<List<TitleDefinitionDto>> Handle(GetAllTitlesAdminQuery request, CancellationToken cancellationToken)
    {
        return await _titleRepo.GetAllTitlesAsync(cancellationToken);
    }
}

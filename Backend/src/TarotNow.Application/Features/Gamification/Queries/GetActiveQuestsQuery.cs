using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

public record GetActiveQuestsQuery(Guid UserId, string QuestType) : IRequest<List<QuestWithProgressDto>>;

public class GetActiveQuestsQueryHandler : IRequestHandler<GetActiveQuestsQuery, List<QuestWithProgressDto>>
{
    private readonly IQuestRepository _questRepo;
    private readonly ILogger<GetActiveQuestsQueryHandler> _logger;

    public GetActiveQuestsQueryHandler(IQuestRepository questRepo, ILogger<GetActiveQuestsQueryHandler> logger)
    {
        _questRepo = questRepo;
        _logger = logger;
    }

    public async Task<List<QuestWithProgressDto>> Handle(GetActiveQuestsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Gamification] Lấy danh sách nhiệm vụ {Type} cho User: {UserId}", request.QuestType, request.UserId);
        var activeQuests = await _questRepo.GetActiveQuestsAsync(request.QuestType, cancellationToken);
        var periodKey = PeriodKeyHelper.GetPeriodKey(request.QuestType);
        var currentProgress = await _questRepo.GetAllProgressAsync(request.UserId, request.QuestType, periodKey, cancellationToken);
        return BuildQuestProgressList(activeQuests, currentProgress);
    }

    private static List<QuestWithProgressDto> BuildQuestProgressList(
        List<QuestDefinitionDto> activeQuests,
        List<QuestProgressDto> currentProgress)
    {
        var result = new List<QuestWithProgressDto>(activeQuests.Count);
        foreach (var definition in activeQuests)
        {
            result.Add(new QuestWithProgressDto
            {
                Definition = definition,
                Progress = currentProgress.Find(p => p.QuestCode == definition.Code)
            });
        }

        return result;
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

// Query lấy danh sách quest active kèm tiến độ của user.
public record GetActiveQuestsQuery(Guid UserId, string QuestType) : IRequest<List<QuestWithProgressDto>>;

// Handler truy vấn quest active theo loại.
public class GetActiveQuestsQueryHandler : IRequestHandler<GetActiveQuestsQuery, List<QuestWithProgressDto>>
{
    private readonly IQuestRepository _questRepo;
    private readonly ILogger<GetActiveQuestsQueryHandler> _logger;

    /// <summary>
    /// Khởi tạo handler get active quests.
    /// Luồng xử lý: nhận quest repository và logger để tải định nghĩa quest active + tiến độ user.
    /// </summary>
    public GetActiveQuestsQueryHandler(IQuestRepository questRepo, ILogger<GetActiveQuestsQueryHandler> logger)
    {
        _questRepo = questRepo;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý query lấy quest active.
    /// Luồng xử lý: tải quest active theo quest type, tính period key, tải toàn bộ progress user trong kỳ, rồi map definition/progress.
    /// </summary>
    public async Task<List<QuestWithProgressDto>> Handle(GetActiveQuestsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Gamification] Lấy danh sách nhiệm vụ {Type} cho User: {UserId}", request.QuestType, request.UserId);
        var activeQuests = await _questRepo.GetActiveQuestsAsync(request.QuestType, cancellationToken);
        var periodKey = PeriodKeyHelper.GetPeriodKey(request.QuestType);
        var currentProgress = await _questRepo.GetAllProgressAsync(request.UserId, request.QuestType, periodKey, cancellationToken);
        return BuildQuestProgressList(activeQuests, currentProgress);
    }

    /// <summary>
    /// Ghép danh sách quest definition với progress tương ứng của user.
    /// Luồng xử lý: duyệt từng quest active và tìm progress cùng QuestCode trong danh sách tiến độ đã tải.
    /// </summary>
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

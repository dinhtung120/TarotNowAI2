using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

// Query admin lấy toàn bộ quest definitions.
public record GetAllQuestsAdminQuery : IRequest<List<QuestDefinitionDto>>;

// Handler query admin lấy danh sách quest.
public class GetAllQuestsAdminQueryHandler : IRequestHandler<GetAllQuestsAdminQuery, List<QuestDefinitionDto>>
{
    private readonly IQuestRepository _questRepo;

    /// <summary>
    /// Khởi tạo handler admin get all quests.
    /// Luồng xử lý: nhận quest repository để truy vấn toàn bộ định nghĩa quest.
    /// </summary>
    public GetAllQuestsAdminQueryHandler(IQuestRepository questRepo)
    {
        _questRepo = questRepo;
    }

    /// <summary>
    /// Xử lý query lấy toàn bộ quest definition cho admin.
    /// Luồng xử lý: gọi repository lấy danh sách quest hiện có.
    /// </summary>
    public async Task<List<QuestDefinitionDto>> Handle(GetAllQuestsAdminQuery request, CancellationToken cancellationToken)
    {
        return await _questRepo.GetAllQuestsAsync(cancellationToken);
    }
}

// Query admin lấy toàn bộ achievement definitions.
public record GetAllAchievementsAdminQuery : IRequest<List<AchievementDefinitionDto>>;

// Handler query admin lấy danh sách achievement.
public class GetAllAchievementsAdminQueryHandler : IRequestHandler<GetAllAchievementsAdminQuery, List<AchievementDefinitionDto>>
{
    private readonly IAchievementRepository _achRepo;

    /// <summary>
    /// Khởi tạo handler admin get all achievements.
    /// Luồng xử lý: nhận achievement repository để truy vấn toàn bộ định nghĩa achievement.
    /// </summary>
    public GetAllAchievementsAdminQueryHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    /// <summary>
    /// Xử lý query lấy toàn bộ achievement definition cho admin.
    /// Luồng xử lý: gọi repository lấy danh sách achievement hiện có.
    /// </summary>
    public async Task<List<AchievementDefinitionDto>> Handle(GetAllAchievementsAdminQuery request, CancellationToken cancellationToken)
    {
        return await _achRepo.GetAllAchievementsAsync(cancellationToken);
    }
}

// Query admin lấy toàn bộ title definitions.
public record GetAllTitlesAdminQuery : IRequest<List<TitleDefinitionDto>>;

// Handler query admin lấy danh sách title.
public class GetAllTitlesAdminQueryHandler : IRequestHandler<GetAllTitlesAdminQuery, List<TitleDefinitionDto>>
{
    private readonly ITitleRepository _titleRepo;

    /// <summary>
    /// Khởi tạo handler admin get all titles.
    /// Luồng xử lý: nhận title repository để truy vấn toàn bộ định nghĩa title.
    /// </summary>
    public GetAllTitlesAdminQueryHandler(ITitleRepository titleRepo)
    {
        _titleRepo = titleRepo;
    }

    /// <summary>
    /// Xử lý query lấy toàn bộ title definition cho admin.
    /// Luồng xử lý: gọi repository lấy danh sách title hiện có.
    /// </summary>
    public async Task<List<TitleDefinitionDto>> Handle(GetAllTitlesAdminQuery request, CancellationToken cancellationToken)
    {
        return await _titleRepo.GetAllTitlesAsync(cancellationToken);
    }
}

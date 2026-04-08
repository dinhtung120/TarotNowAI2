using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

// Command admin tạo/cập nhật định nghĩa quest.
public record UpsertQuestDefinitionCommand(QuestDefinitionDto Quest) : IRequest<bool>;

// Command admin xóa định nghĩa quest theo code.
public record DeleteQuestDefinitionCommand(string Code) : IRequest<bool>;

// Handler command admin cho định nghĩa quest.
public class AdminQuestCommandHandler :
    IRequestHandler<UpsertQuestDefinitionCommand, bool>,
    IRequestHandler<DeleteQuestDefinitionCommand, bool>
{
    private readonly IQuestRepository _questRepo;

    /// <summary>
    /// Khởi tạo handler admin quest.
    /// Luồng xử lý: nhận quest repository để upsert/xóa định nghĩa quest.
    /// </summary>
    public AdminQuestCommandHandler(IQuestRepository questRepo)
    {
        _questRepo = questRepo;
    }

    /// <summary>
    /// Xử lý command upsert quest definition.
    /// Luồng xử lý: gọi repository upsert và trả true khi hoàn tất.
    /// </summary>
    public async Task<bool> Handle(UpsertQuestDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _questRepo.UpsertQuestDefinitionAsync(request.Quest, cancellationToken);
        return true;
    }

    /// <summary>
    /// Xử lý command delete quest definition.
    /// Luồng xử lý: gọi repository xóa quest theo code và trả true khi hoàn tất.
    /// </summary>
    public async Task<bool> Handle(DeleteQuestDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _questRepo.DeleteQuestDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

// Command admin tạo/cập nhật định nghĩa achievement.
public record UpsertAchievementDefinitionCommand(AchievementDefinitionDto Achievement) : IRequest<bool>;

// Command admin xóa định nghĩa achievement theo code.
public record DeleteAchievementDefinitionCommand(string Code) : IRequest<bool>;

// Handler command admin cho định nghĩa achievement.
public class AdminAchievementCommandHandler :
    IRequestHandler<UpsertAchievementDefinitionCommand, bool>,
    IRequestHandler<DeleteAchievementDefinitionCommand, bool>
{
    private readonly IAchievementRepository _achRepo;

    /// <summary>
    /// Khởi tạo handler admin achievement.
    /// Luồng xử lý: nhận achievement repository để upsert/xóa định nghĩa achievement.
    /// </summary>
    public AdminAchievementCommandHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    /// <summary>
    /// Xử lý command upsert achievement definition.
    /// Luồng xử lý: gọi repository upsert và trả true khi hoàn tất.
    /// </summary>
    public async Task<bool> Handle(UpsertAchievementDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _achRepo.UpsertAchievementDefinitionAsync(request.Achievement, cancellationToken);
        return true;
    }

    /// <summary>
    /// Xử lý command delete achievement definition.
    /// Luồng xử lý: gọi repository xóa achievement theo code và trả true khi hoàn tất.
    /// </summary>
    public async Task<bool> Handle(DeleteAchievementDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _achRepo.DeleteAchievementDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

// Command admin tạo/cập nhật định nghĩa title.
public record UpsertTitleDefinitionCommand(TitleDefinitionDto Title) : IRequest<bool>;

// Command admin xóa định nghĩa title theo code.
public record DeleteTitleDefinitionCommand(string Code) : IRequest<bool>;

// Handler command admin cho định nghĩa title.
public class AdminTitleCommandHandler :
    IRequestHandler<UpsertTitleDefinitionCommand, bool>,
    IRequestHandler<DeleteTitleDefinitionCommand, bool>
{
    private readonly ITitleRepository _titleRepo;

    /// <summary>
    /// Khởi tạo handler admin title.
    /// Luồng xử lý: nhận title repository để upsert/xóa định nghĩa title.
    /// </summary>
    public AdminTitleCommandHandler(ITitleRepository titleRepo)
    {
        _titleRepo = titleRepo;
    }

    /// <summary>
    /// Xử lý command upsert title definition.
    /// Luồng xử lý: gọi repository upsert và trả true khi hoàn tất.
    /// </summary>
    public async Task<bool> Handle(UpsertTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _titleRepo.UpsertTitleDefinitionAsync(request.Title, cancellationToken);
        return true;
    }

    /// <summary>
    /// Xử lý command delete title definition.
    /// Luồng xử lý: gọi repository xóa title theo code và trả true khi hoàn tất.
    /// </summary>
    public async Task<bool> Handle(DeleteTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _titleRepo.DeleteTitleDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

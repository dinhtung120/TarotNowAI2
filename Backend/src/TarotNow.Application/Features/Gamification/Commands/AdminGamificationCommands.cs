using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

// === QUEST COMMANDS ===
public record UpsertQuestDefinitionCommand(QuestDefinitionDto Quest) : IRequest<bool>;
public record DeleteQuestDefinitionCommand(string Code) : IRequest<bool>;

public class AdminQuestCommandHandler : 
    IRequestHandler<UpsertQuestDefinitionCommand, bool>,
    IRequestHandler<DeleteQuestDefinitionCommand, bool>
{
    private readonly IQuestRepository _questRepo;

    public AdminQuestCommandHandler(IQuestRepository questRepo)
    {
        _questRepo = questRepo;
    }

    public async Task<bool> Handle(UpsertQuestDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _questRepo.UpsertQuestDefinitionAsync(request.Quest, cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteQuestDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _questRepo.DeleteQuestDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

// === ACHIEVEMENT COMMANDS ===
public record UpsertAchievementDefinitionCommand(AchievementDefinitionDto Achievement) : IRequest<bool>;
public record DeleteAchievementDefinitionCommand(string Code) : IRequest<bool>;

public class AdminAchievementCommandHandler :
    IRequestHandler<UpsertAchievementDefinitionCommand, bool>,
    IRequestHandler<DeleteAchievementDefinitionCommand, bool>
{
    private readonly IAchievementRepository _achRepo;

    public AdminAchievementCommandHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    public async Task<bool> Handle(UpsertAchievementDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _achRepo.UpsertAchievementDefinitionAsync(request.Achievement, cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteAchievementDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _achRepo.DeleteAchievementDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

// === TITLE COMMANDS ===
public record UpsertTitleDefinitionCommand(TitleDefinitionDto Title) : IRequest<bool>;
public record DeleteTitleDefinitionCommand(string Code) : IRequest<bool>;

public class AdminTitleCommandHandler :
    IRequestHandler<UpsertTitleDefinitionCommand, bool>,
    IRequestHandler<DeleteTitleDefinitionCommand, bool>
{
    private readonly ITitleRepository _titleRepo;

    public AdminTitleCommandHandler(ITitleRepository titleRepo)
    {
        _titleRepo = titleRepo;
    }

    public async Task<bool> Handle(UpsertTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _titleRepo.UpsertTitleDefinitionAsync(request.Title, cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _titleRepo.DeleteTitleDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

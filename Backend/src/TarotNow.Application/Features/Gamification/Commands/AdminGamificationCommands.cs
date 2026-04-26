using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Gamification.Commands;

// Command admin tạo/cập nhật định nghĩa quest.
public record UpsertQuestDefinitionCommand(QuestDefinitionDto Quest) : IRequest<bool>;

// Command admin xóa định nghĩa quest theo code.
public record DeleteQuestDefinitionCommand(string Code) : IRequest<bool>;

// Handler command admin cho định nghĩa quest.
public class AdminQuestCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AdminQuestCommandHandlerRequestedDomainEvent>
{
    private readonly IQuestRepository _questRepo;

    /// <summary>
    /// Khởi tạo handler admin quest.
    /// Luồng xử lý: nhận quest repository để upsert/xóa định nghĩa quest.
    /// </summary>
    public AdminQuestCommandHandlerRequestedDomainEventHandler(
        IQuestRepository questRepo,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
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

    protected override async Task HandleDomainEventAsync(
        AdminQuestCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

// Handler command admin xóa định nghĩa quest.
public class DeleteQuestDefinitionCommandHandler : IRequestHandler<DeleteQuestDefinitionCommand, bool>
{
    private readonly IQuestRepository _questRepo;

    public DeleteQuestDefinitionCommandHandler(IQuestRepository questRepo)
    {
        _questRepo = questRepo;
    }

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
public class AdminAchievementCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AdminAchievementCommandHandlerRequestedDomainEvent>
{
    private readonly IAchievementRepository _achRepo;

    /// <summary>
    /// Khởi tạo handler admin achievement.
    /// Luồng xử lý: nhận achievement repository để upsert/xóa định nghĩa achievement.
    /// </summary>
    public AdminAchievementCommandHandlerRequestedDomainEventHandler(
        IAchievementRepository achRepo,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
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

    protected override async Task HandleDomainEventAsync(
        AdminAchievementCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

// Handler command admin xóa định nghĩa achievement.
public class DeleteAchievementDefinitionCommandHandler : IRequestHandler<DeleteAchievementDefinitionCommand, bool>
{
    private readonly IAchievementRepository _achRepo;

    public DeleteAchievementDefinitionCommandHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

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
public class AdminTitleCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AdminTitleCommandHandlerRequestedDomainEvent>
{
    private readonly ITitleRepository _titleRepo;

    /// <summary>
    /// Khởi tạo handler admin title.
    /// Luồng xử lý: nhận title repository để upsert/xóa định nghĩa title.
    /// </summary>
    public AdminTitleCommandHandlerRequestedDomainEventHandler(
        ITitleRepository titleRepo,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
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

    protected override async Task HandleDomainEventAsync(
        AdminTitleCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

// Handler command admin xóa định nghĩa title.
public class DeleteTitleDefinitionCommandHandler : IRequestHandler<DeleteTitleDefinitionCommand, bool>
{
    private readonly ITitleRepository _titleRepo;

    public DeleteTitleDefinitionCommandHandler(ITitleRepository titleRepo)
    {
        _titleRepo = titleRepo;
    }

    public async Task<bool> Handle(DeleteTitleDefinitionCommand request, CancellationToken cancellationToken)
    {
        await _titleRepo.DeleteTitleDefinitionAsync(request.Code, cancellationToken);
        return true;
    }
}

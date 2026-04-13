using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler gửi email OTP bất đồng bộ từ domain event.
/// </summary>
public sealed class EmailOtpIssuedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<EmailOtpIssuedDomainEvent>
{
    private readonly IEmailSender _emailSender;

    /// <summary>
    /// Khởi tạo handler email OTP.
    /// </summary>
    public EmailOtpIssuedDomainEventHandler(
        IEmailSender emailSender,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _emailSender = emailSender;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        EmailOtpIssuedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _emailSender.SendEmailAsync(domainEvent.Email, domainEvent.Subject, domainEvent.Body, cancellationToken);
    }
}

/// <summary>
/// Handler hậu xử lý khi phiên đọc bài hoàn tất.
/// </summary>
public sealed class ReadingCompletedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingCompletedDomainEvent>
{
    private readonly IStreakService _streakService;
    private readonly IGamificationService _gamificationService;

    /// <summary>
    /// Khởi tạo handler reading completed.
    /// </summary>
    public ReadingCompletedDomainEventHandler(
        IStreakService streakService,
        IGamificationService gamificationService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _streakService = streakService;
        _gamificationService = gamificationService;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingCompletedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        await _streakService.IncrementStreakOnValidDrawAsync(domainEvent.UserId, cancellationToken);
        await _gamificationService.OnReadingCompletedAsync(domainEvent.UserId, cancellationToken);
    }
}

/// <summary>
/// Handler hậu xử lý gamification khi check-in thành công.
/// </summary>
public sealed class DailyCheckInCompletedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DailyCheckInCompletedDomainEvent>
{
    private readonly IGamificationService _gamificationService;

    /// <summary>
    /// Khởi tạo handler daily check-in completed.
    /// </summary>
    public DailyCheckInCompletedDomainEventHandler(
        IGamificationService gamificationService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _gamificationService = gamificationService;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        DailyCheckInCompletedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _gamificationService.OnCheckInAsync(domainEvent.UserId, domainEvent.CurrentStreak, cancellationToken);
    }
}

/// <summary>
/// Handler hậu xử lý gamification khi tạo bài viết community.
/// </summary>
public sealed class CommunityPostCreatedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CommunityPostCreatedDomainEvent>
{
    private readonly IGamificationService _gamificationService;

    /// <summary>
    /// Khởi tạo handler community post created.
    /// </summary>
    public CommunityPostCreatedDomainEventHandler(
        IGamificationService gamificationService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _gamificationService = gamificationService;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        CommunityPostCreatedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _gamificationService.OnPostCreatedAsync(domainEvent.AuthorId, cancellationToken);
    }
}

/// <summary>
/// Handler cấp title từ domain event.
/// </summary>
public sealed class TitleGrantedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<TitleGrantedDomainEvent>
{
    private readonly ITitleRepository _titleRepository;

    /// <summary>
    /// Khởi tạo handler title granted.
    /// </summary>
    public TitleGrantedDomainEventHandler(
        ITitleRepository titleRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _titleRepository = titleRepository;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        TitleGrantedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _titleRepository.GrantTitleAsync(domainEvent.UserId, domainEvent.TitleCode, cancellationToken);
    }
}

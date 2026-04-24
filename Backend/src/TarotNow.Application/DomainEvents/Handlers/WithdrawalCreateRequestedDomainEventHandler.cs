using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Xử lý domain event tạo yêu cầu rút tiền.
/// </summary>
public sealed partial class WithdrawalCreateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<WithdrawalCreateRequestedDomainEvent>
{
    private const string WithdrawalReferenceSource = "withdrawal_request";
    private const string FreezeDescriptionTemplate = "Freeze {0} diamond for withdrawal request {1}";

    private readonly IWithdrawalRepository _withdrawalRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler tạo withdrawal request.
    /// </summary>
    public WithdrawalCreateRequestedDomainEventHandler(
        IWithdrawalRepository withdrawalRepository,
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _withdrawalRepository = withdrawalRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        WithdrawalCreateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var normalizedRequestKey = NormalizeIdempotencyKey(domainEvent.IdempotencyKey);
        var requestId = BuildDeterministicRequestId(domainEvent.UserId, normalizedRequestKey);

        var existingRequest = await _withdrawalRepository.GetByIdAsync(requestId, cancellationToken);
        if (existingRequest != null)
        {
            HydrateCreateResult(domainEvent, existingRequest);
            return;
        }

        var user = await LoadAndValidateUserAsync(domainEvent, cancellationToken);
        var businessWeekStartUtc = ResolveBusinessWeekStartUtc(DateTime.UtcNow);
        await EnsureNoWeeklyRequestAsync(domainEvent.UserId, businessWeekStartUtc, cancellationToken);
        await FreezeDiamondAsync(domainEvent, requestId, cancellationToken);
        var pendingRequestContext = new PendingRequestContext(
            requestId,
            normalizedRequestKey,
            businessWeekStartUtc,
            BuildWithdrawalPlan(
                domainEvent.AmountDiamond,
                _systemConfigSettings.WithdrawalFeeRate,
                _systemConfigSettings.EconomyVndPerDiamond));

        var request = BuildPendingRequest(
            domainEvent,
            user,
            pendingRequestContext);

        await _withdrawalRepository.AddAsync(request, cancellationToken);
        await _withdrawalRepository.SaveChangesAsync(cancellationToken);
        await PublishSideEffectsAsync(domainEvent, request, cancellationToken);
        HydrateCreateResult(domainEvent, request);
    }

    private async Task<User> LoadAndValidateUserAsync(
        WithdrawalCreateRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(domainEvent.UserId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");
        ValidateUserCanWithdraw(user, domainEvent.AmountDiamond, _systemConfigSettings.WithdrawalMinDiamond);
        return user;
    }

    private async Task EnsureNoWeeklyRequestAsync(
        Guid userId,
        DateOnly businessWeekStartUtc,
        CancellationToken cancellationToken)
    {
        var hasAnyRequestInWeek = await _withdrawalRepository.HasAnyRequestInWeekAsync(
            userId,
            businessWeekStartUtc,
            cancellationToken);
        if (!hasAnyRequestInWeek)
        {
            return;
        }

        throw new BadRequestException("Bạn đã tạo yêu cầu rút tiền trong tuần này. Hệ thống reset vào thứ Hai 00:00 UTC.");
    }

    private async Task FreezeDiamondAsync(
        WithdrawalCreateRequestedDomainEvent domainEvent,
        Guid requestId,
        CancellationToken cancellationToken)
    {
        await _walletRepository.FreezeAsync(
            userId: domainEvent.UserId,
            amount: domainEvent.AmountDiamond,
            referenceSource: WithdrawalReferenceSource,
            referenceId: requestId.ToString(),
            description: string.Format(FreezeDescriptionTemplate, domainEvent.AmountDiamond, requestId),
            idempotencyKey: BuildFreezeIdempotencyKey(requestId),
            cancellationToken: cancellationToken);
    }

    private async Task PublishSideEffectsAsync(
        WithdrawalCreateRequestedDomainEvent domainEvent,
        WithdrawalRequest request,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.UserId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.EscrowFreeze,
                DeltaAmount = -domainEvent.AmountDiamond,
                ReferenceId = request.Id.ToString()
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new WithdrawalRequestedDomainEvent
            {
                RequestId = request.Id,
                UserId = domainEvent.UserId,
                AmountDiamond = domainEvent.AmountDiamond,
                NetAmountVnd = request.NetAmountVnd,
                BankName = request.BankName,
                BankAccountNumber = request.BankAccountNumber
            },
            cancellationToken);
    }

}

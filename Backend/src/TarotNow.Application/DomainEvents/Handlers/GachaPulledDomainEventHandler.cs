using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler chính xử lý write-side cho luồng pull gacha.
/// </summary>
public sealed partial class GachaPulledDomainEventHandler
    : IdempotentDomainEventNotificationHandler<GachaPulledDomainEvent>
{
    private const int MinimumProbabilityBasisPoints = 10000;

    private readonly IGachaPoolRepository _gachaPoolRepository;
    private readonly IItemDefinitionRepository _itemDefinitionRepository;
    private readonly IUserItemRepository _userItemRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IRngService _rngService;
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;
    private readonly ITransactionCoordinator _transactionCoordinator;

    /// <summary>
    /// Khởi tạo handler.
    /// </summary>
    public GachaPulledDomainEventHandler(
        IGachaPoolRepository gachaPoolRepository,
        IItemDefinitionRepository itemDefinitionRepository,
        IUserItemRepository userItemRepository,
        IWalletRepository walletRepository,
        IRngService rngService,
        IInlineDomainEventDispatcher inlineDomainEventDispatcher,
        ITransactionCoordinator transactionCoordinator,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _gachaPoolRepository = gachaPoolRepository;
        _itemDefinitionRepository = itemDefinitionRepository;
        _userItemRepository = userItemRepository;
        _walletRepository = walletRepository;
        _rngService = rngService;
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
        _transactionCoordinator = transactionCoordinator;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        GachaPulledDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var pool = await GetActivePoolAsync(domainEvent.PoolCode, cancellationToken);
        var rates = await GetValidatedRewardRatesAsync(pool.Id, cancellationToken);
        var itemDefinitionMap = await BuildItemDefinitionMapAsync(rates, cancellationToken);
        var operationResult = default(GachaPullOperationCreateResult);
        PullExecutionContext? pullContext = null;
        PullExecutionOutcome? pullOutcome = null;

        await _transactionCoordinator.ExecuteAsync(
            async innerCancellationToken =>
            {
                operationResult = await TryCreateOperationAsync(domainEvent, pool, innerCancellationToken);
                if (!operationResult.IsCreated)
                {
                    return;
                }

                var userPity = await _gachaPoolRepository.GetOrCreateUserPityAsync(
                    domainEvent.UserId,
                    pool.Id,
                    innerCancellationToken);
                pullContext = new PullExecutionContext(
                    domainEvent,
                    pool,
                    operationResult.Operation,
                    itemDefinitionMap,
                    userPity,
                    userPity.PullCount);

                await DebitPullCostAsync(pullContext, innerCancellationToken);
                pullOutcome = await ExecutePullRollsAsync(pullContext, rates, innerCancellationToken);
                await FinalizeOperationAsync(
                    pullContext,
                    pullOutcome.RewardLogs,
                    pullOutcome.WasPityReset,
                    innerCancellationToken);
            },
            cancellationToken);

        if (operationResult is null)
        {
            throw new InvalidOperationException("Gacha pull operation result was not initialized.");
        }

        domainEvent.OperationId = operationResult.Operation.Id;

        if (!operationResult.IsCreated)
        {
            await PopulateReplayResultAsync(domainEvent, operationResult.Operation, cancellationToken);
            return;
        }

        if (pullContext is null || pullOutcome is null)
        {
            throw new InvalidOperationException("Gacha pull execution context was not initialized.");
        }
    }

    private async Task<GachaPool> GetActivePoolAsync(string poolCode, CancellationToken cancellationToken)
    {
        var pool = await _gachaPoolRepository.GetActivePoolByCodeAsync(poolCode, cancellationToken);
        if (pool is null)
        {
            throw new BusinessRuleException(GachaErrorCodes.PoolNotFound, "Gacha pool was not found.");
        }

        return pool;
    }

    private async Task<IReadOnlyList<GachaPoolRewardRate>> GetValidatedRewardRatesAsync(Guid poolId, CancellationToken cancellationToken)
    {
        var rates = await _gachaPoolRepository.GetActiveRewardRatesAsync(poolId, cancellationToken);
        if (rates.Count == 0)
        {
            throw new BusinessRuleException(GachaErrorCodes.InvalidPoolConfiguration, "Gacha pool reward rates are empty.");
        }

        var totalProbability = rates.Sum(x => x.ProbabilityBasisPoints);
        if (totalProbability != MinimumProbabilityBasisPoints)
        {
            throw new BusinessRuleException(
                GachaErrorCodes.InvalidPoolConfiguration,
                "Gacha pool reward rate probabilities must sum to 10000 basis points.");
        }

        return rates;
    }

    private Task<GachaPullOperationCreateResult> TryCreateOperationAsync(
        GachaPulledDomainEvent domainEvent,
        GachaPool pool,
        CancellationToken cancellationToken)
    {
        var operation = new GachaPullOperation(
            domainEvent.UserId,
            pool.Id,
            domainEvent.IdempotencyKey,
            domainEvent.Count);

        return _gachaPoolRepository.TryCreatePullOperationAsync(operation, cancellationToken);
    }

    private async Task<Dictionary<Guid, ItemDefinition>> BuildItemDefinitionMapAsync(
        IReadOnlyList<GachaPoolRewardRate> rates,
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<Guid, ItemDefinition>();
        var itemDefinitionIds = rates
            .Where(x => x.ItemDefinitionId.HasValue)
            .Select(x => x.ItemDefinitionId!.Value)
            .Distinct()
            .ToList();

        foreach (var itemDefinitionId in itemDefinitionIds)
        {
            var definition = await _itemDefinitionRepository.GetByIdAsync(itemDefinitionId, cancellationToken);
            if (definition is null || !definition.IsActive)
            {
                throw new BusinessRuleException(
                    GachaErrorCodes.RewardResolutionFailed,
                    "Gacha reward item definition was not found.");
            }

            result[itemDefinitionId] = definition;
        }

        return result;
    }

    private sealed record PullExecutionContext(
        GachaPulledDomainEvent DomainEvent,
        GachaPool Pool,
        GachaPullOperation Operation,
        IReadOnlyDictionary<Guid, ItemDefinition> ItemDefinitionMap,
        UserGachaPity UserPity,
        int PityBefore);
}

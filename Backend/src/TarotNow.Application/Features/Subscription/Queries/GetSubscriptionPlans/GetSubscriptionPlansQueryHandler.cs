

using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Subscription.Commands.Subscribe;
using TarotNow.Application.Features.Subscription.Queries.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Queries.GetSubscriptionPlans;

public class GetSubscriptionPlansQueryHandler : IRequestHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>>
{
    private static readonly Dictionary<string, int> EmptyEntitlements = [];

    private readonly ISubscriptionRepository _subscriptionRepository;

    public GetSubscriptionPlansQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<List<SubscriptionPlanDto>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _subscriptionRepository.GetActivePlansAsync(cancellationToken);
        return plans.Select(MapPlanToDto).ToList();
    }

    private SubscriptionPlanDto MapPlanToDto(dynamic plan)
    {
        return new SubscriptionPlanDto(
            plan.Id,
            plan.Name,
            plan.Description,
            plan.PriceDiamond,
            plan.DurationDays,
            plan.IsActive,
            ParseEntitlements(plan.EntitlementsJson),
            plan.CreatedAt);
    }

    private static Dictionary<string, int> ParseEntitlements(string? entitlementsJson)
    {
        if (string.IsNullOrWhiteSpace(entitlementsJson))
        {
            return [];
        }

        return ParseListFormat(entitlementsJson) ?? ParseDictionaryFormat(entitlementsJson) ?? [];
    }

    private static Dictionary<string, int>? ParseListFormat(string entitlementsJson)
    {
        try
        {
            var configList = System.Text.Json.JsonSerializer.Deserialize<List<EntitlementConfigDto>>(entitlementsJson);
            if (configList == null || configList.Count == 0) return null;

            var entitlements = new Dictionary<string, int>();
            foreach (var config in configList)
            {
                if (string.IsNullOrEmpty(config.key)) continue;
                entitlements[config.key] = config.dailyQuota;
            }

            return entitlements;
        }
        catch
        {
            return null;
        }
    }

    private static Dictionary<string, int>? ParseDictionaryFormat(string entitlementsJson)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(entitlementsJson);
        }
        catch
        {
            return EmptyEntitlements;
        }
    }
}

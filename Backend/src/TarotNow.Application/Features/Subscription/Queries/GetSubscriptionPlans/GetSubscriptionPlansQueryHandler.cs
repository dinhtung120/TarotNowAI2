

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
    private readonly ISubscriptionRepository _subscriptionRepository;

    public GetSubscriptionPlansQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<List<SubscriptionPlanDto>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _subscriptionRepository.GetActivePlansAsync(cancellationToken);

        return plans.Select(p => {
            
            var entDict = new Dictionary<string, int>();
            try
            {
                if (!string.IsNullOrWhiteSpace(p.EntitlementsJson))
                {
                    
                    var configList = System.Text.Json.JsonSerializer.Deserialize<List<EntitlementConfigDto>>(p.EntitlementsJson);
                    if (configList != null && configList.Count > 0)
                    {
                        foreach (var cfg in configList)
                        {
                            if (!string.IsNullOrEmpty(cfg.key))
                                entDict[cfg.key] = cfg.dailyQuota;
                        }
                    }
                    else
                    {
                        
                        entDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(p.EntitlementsJson) 
                                  ?? new Dictionary<string, int>();
                    }
                }
            }
            catch
            {
                
                entDict = new Dictionary<string, int>();
            }

            return new SubscriptionPlanDto(
                p.Id,
                p.Name,
                p.Description,
                p.PriceDiamond,
                p.DurationDays,
                p.IsActive,
                entDict,
                p.CreatedAt
            );
        }).ToList();
    }
}

/*
 * ===================================================================
 * FILE: GetSubscriptionPlansQueryHandler.cs
 * ===================================================================
 * MỤC ĐÍCH:
 *   Truy vấn tất cả gói đăng ký (Plans) đang mở bán (IsActive == true).
 *   
 * THIẾT KẾ:
 *   EntitlementsJson trong DB lưu dưới dạng mảng đối tượng:
 *   [{"key":"free_spread_3_daily","dailyQuota":2}, ...]
 *   
 *   Handler phải parse đúng format này rồi chuyển đổi sang Dictionary<string,int>
 *   để trả về DTO thân thiện cho Frontend (flat key-value map).
 *   
 *   QUAN TRỌNG: Format JSON phải ĐỒNG NHẤT với SubscribeCommandHandler
 *   (cả hai đều dùng List<EntitlementConfigDto> = [{key, dailyQuota}]).
 * ===================================================================
 */

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
            /*
             * Parse EntitlementsJson → Dictionary<string, int> cho Frontend.
             * DB lưu dạng: [{"key":"free_spread_3_daily","dailyQuota":2}]
             * Frontend cần: {"free_spread_3_daily": 2}
             * 
             * Thử parse format mảng trước (chuẩn chính thức),
             * nếu thất bại thì fallback thử parse flat dictionary (tương thích ngược).
             */
            var entDict = new Dictionary<string, int>();
            try
            {
                if (!string.IsNullOrWhiteSpace(p.EntitlementsJson))
                {
                    // Ưu tiên parse format chính thức: [{key, dailyQuota}]
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
                        // Fallback: thử parse flat dictionary {"key": value}
                        entDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(p.EntitlementsJson) 
                                  ?? new Dictionary<string, int>();
                    }
                }
            }
            catch
            {
                // Nếu JSON bị hỏng, trả về rỗng an toàn
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

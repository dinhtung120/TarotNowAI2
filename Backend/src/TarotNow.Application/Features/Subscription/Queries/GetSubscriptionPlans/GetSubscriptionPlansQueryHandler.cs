using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Subscription.Commands.Subscribe;
using TarotNow.Application.Features.Subscription.Queries.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Queries.GetSubscriptionPlans;

// Handler lấy danh sách gói subscription active.
public class GetSubscriptionPlansQueryHandler : IRequestHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>>
{
    // Giá trị rỗng tái sử dụng khi parse entitlement thất bại.
    private static readonly Dictionary<string, int> EmptyEntitlements = [];

    private readonly ISubscriptionRepository _subscriptionRepository;

    /// <summary>
    /// Khởi tạo handler lấy subscription plans.
    /// Luồng xử lý: nhận subscription repository để đọc danh sách gói active.
    /// </summary>
    public GetSubscriptionPlansQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    /// <summary>
    /// Xử lý query lấy subscription plans.
    /// Luồng xử lý: tải plan active từ repository, map từng plan sang DTO và parse entitlement JSON theo định dạng hỗ trợ.
    /// </summary>
    public async Task<List<SubscriptionPlanDto>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _subscriptionRepository.GetActivePlansAsync(cancellationToken);
        return plans.Select(MapPlanToDto).ToList();
    }

    /// <summary>
    /// Map dữ liệu plan động từ persistence sang DTO chuẩn.
    /// Luồng xử lý: ánh xạ field cơ bản và parse entitlements JSON sang dictionary key/quota.
    /// </summary>
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

    /// <summary>
    /// Parse chuỗi entitlement JSON sang dictionary quota.
    /// Luồng xử lý: ưu tiên parse dạng list DTO, fallback dạng dictionary trực tiếp, cuối cùng trả rỗng khi không parse được.
    /// </summary>
    private static Dictionary<string, int> ParseEntitlements(string? entitlementsJson)
    {
        if (string.IsNullOrWhiteSpace(entitlementsJson))
        {
            // Edge case: không có entitlement data thì trả dictionary rỗng.
            return [];
        }

        return ParseListFormat(entitlementsJson) ??
               ParseDictionaryFormat(entitlementsJson) ??
               [];
    }

    /// <summary>
    /// Parse entitlement theo định dạng danh sách EntitlementConfigDto.
    /// Luồng xử lý: deserialize list, lọc key rỗng và dựng dictionary key -> dailyQuota.
    /// </summary>
    private static Dictionary<string, int>? ParseListFormat(string entitlementsJson)
    {
        try
        {
            var configList = JsonSerializer.Deserialize<List<EntitlementConfigDto>>(entitlementsJson);
            if (configList is null || configList.Count == 0)
            {
                // Dữ liệu list rỗng thì trả null để thử fallback format tiếp theo.
                return null;
            }

            var entitlements = new Dictionary<string, int>();
            foreach (var config in configList)
            {
                if (string.IsNullOrEmpty(config.key))
                {
                    // Bỏ qua entry thiếu key để tránh làm hỏng dictionary kết quả.
                    continue;
                }

                entitlements[config.key] = config.dailyQuota;
            }
            // Kết thúc vòng lặp, dictionary phản ánh đầy đủ entitlement hợp lệ từ list config.

            return entitlements;
        }
        catch
        {
            // Parse lỗi thì trả null để chuyển sang fallback format dictionary.
            return null;
        }
    }

    /// <summary>
    /// Parse entitlement theo định dạng dictionary JSON trực tiếp.
    /// Luồng xử lý: deserialize JSON thành Dictionary<string,int>; nếu lỗi trả EmptyEntitlements.
    /// </summary>
    private static Dictionary<string, int>? ParseDictionaryFormat(string entitlementsJson)
    {
        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, int>>(entitlementsJson);
        }
        catch
        {
            // Parse lỗi ở format dictionary thì trả giá trị rỗng để caller không crash.
            return EmptyEntitlements;
        }
    }
}

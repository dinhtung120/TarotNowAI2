using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Queries.GetMyEntitlements;

// Handler lấy danh sách entitlement balance của user.
public class GetMyEntitlementsQueryHandler : IRequestHandler<GetMyEntitlementsQuery, List<EntitlementBalanceDto>>
{
    private readonly IEntitlementService _entitlementService;

    /// <summary>
    /// Khởi tạo handler lấy entitlement balance.
    /// Luồng xử lý: nhận entitlement service để truy vấn số dư quota hiện tại của user.
    /// </summary>
    public GetMyEntitlementsQueryHandler(IEntitlementService entitlementService)
    {
        _entitlementService = entitlementService;
    }

    /// <summary>
    /// Xử lý query lấy entitlement.
    /// Luồng xử lý: gọi service lấy toàn bộ balance theo user và materialize về List để trả cho API.
    /// </summary>
    public async Task<List<EntitlementBalanceDto>> Handle(
        GetMyEntitlementsQuery request,
        CancellationToken cancellationToken)
    {
        var summaries = await _entitlementService.GetAllBalancesAsync(request.UserId, cancellationToken);
        return summaries.ToList();
    }
}

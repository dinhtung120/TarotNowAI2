using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Queries.GetMyEntitlements;

public class GetMyEntitlementsQueryHandler : IRequestHandler<GetMyEntitlementsQuery, List<EntitlementBalanceDto>>
{
    private readonly IEntitlementService _entitlementService;

    public GetMyEntitlementsQueryHandler(IEntitlementService entitlementService)
    {
        _entitlementService = entitlementService;
    }

    public async Task<List<EntitlementBalanceDto>> Handle(GetMyEntitlementsQuery request, CancellationToken cancellationToken)
    {
        var summaries = await _entitlementService.GetAllBalancesAsync(request.UserId, cancellationToken);
        return summaries.ToList();
    }
}

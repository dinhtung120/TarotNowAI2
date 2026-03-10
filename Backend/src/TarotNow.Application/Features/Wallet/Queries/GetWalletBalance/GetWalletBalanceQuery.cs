using MediatR;

namespace TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

public class GetWalletBalanceQuery : IRequest<WalletBalanceDto>
{
    public Guid UserId { get; set; }
    
    public GetWalletBalanceQuery(Guid userId)
    {
        UserId = userId;
    }
}

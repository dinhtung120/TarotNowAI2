

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, WalletBalanceDto>
{
    private readonly IUserRepository _userRepository;

    public GetWalletBalanceQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<WalletBalanceDto> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new BusinessRuleException("USER_NOT_FOUND", "User not found.");

        
        return new WalletBalanceDto
        {
            GoldBalance = user.GoldBalance, 
            DiamondBalance = user.DiamondBalance, 
            FrozenDiamondBalance = user.FrozenDiamondBalance 
        };
    }
}

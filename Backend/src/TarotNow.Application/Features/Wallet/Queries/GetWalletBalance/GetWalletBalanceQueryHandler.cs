using MediatR;
using TarotNow.Domain.Exceptions;
using TarotNow.Application.Interfaces;

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
        // Lấy thông tin user hiện tại
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new DomainException("USER_NOT_FOUND", "User not found.");

        return new WalletBalanceDto
        {
            GoldBalance = user.GoldBalance,
            DiamondBalance = user.DiamondBalance,
            FrozenDiamondBalance = user.FrozenDiamondBalance
        };
    }
}

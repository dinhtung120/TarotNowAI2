using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

// Handler truy vấn số dư ví người dùng.
public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, WalletBalanceDto>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler lấy số dư ví.
    /// Luồng xử lý: nhận user repository để đọc số dư ví từ hồ sơ người dùng.
    /// </summary>
    public GetWalletBalanceQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý query lấy wallet balance.
    /// Luồng xử lý: tải user theo id, ném lỗi khi không tồn tại và map các trường số dư sang DTO.
    /// </summary>
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

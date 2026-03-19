/*
 * ===================================================================
 * FILE: GetWalletBalanceQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Wallet.Queries.GetWalletBalance
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lấy Giá Trị Vàng/Kim Cương của người dùng ra Trưng Bày Lên Góc Phải Màn Hình.
 *   Tiền của User hiện tại đang cắm thẳng vào bảng PostgreSQL (User Tbl) luôn cho lẹ, 
 *   mặc dù Lịch Sử Nạp Rút (Ledger) lại cất ở MongoDb.
 * ===================================================================
 */

using MediatR;
using TarotNow.Domain.Exceptions;
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
        // Tra ID Này Coi Có Ở Đợ Nhà Ai Không. (User Tái Phạm hoặc Xoá Nick Sẽ K Có Số Dư Này).
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new DomainException("USER_NOT_FOUND", "User not found.");

        // Nhặt Số Dư ra Thẩy Về Thôi.
        return new WalletBalanceDto
        {
            GoldBalance = user.GoldBalance, // Coins Free Mảnh Vỡ (Mua bằng điểm danh)
            DiamondBalance = user.DiamondBalance, // Tiền Bỏ Túi Ra Mua (Giá Trị Cứng)
            FrozenDiamondBalance = user.FrozenDiamondBalance // Tiền Đang Nằm Vùng Chờ Kết Quả Bói Tarot.
        };
    }
}

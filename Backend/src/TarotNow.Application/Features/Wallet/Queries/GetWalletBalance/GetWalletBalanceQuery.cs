using MediatR;
using System;

namespace TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

// Query lấy số dư ví hiện tại của user.
public class GetWalletBalanceQuery : IRequest<WalletBalanceDto>
{
    // Định danh user cần lấy số dư ví.
    public Guid UserId { get; set; }

    /// <summary>
    /// Khởi tạo query số dư ví theo user id.
    /// Luồng xử lý: gán UserId để handler truy xuất đúng hồ sơ tài khoản.
    /// </summary>
    public GetWalletBalanceQuery(Guid userId)
    {
        UserId = userId;
    }
}

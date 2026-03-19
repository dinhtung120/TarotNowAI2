/*
 * ===================================================================
 * FILE: GetWalletBalanceQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Wallet.Queries.GetWalletBalance
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lệnh Xin Cập Nhật Số Dư Hiện Tại Của Khách (Chứa Tiền Gì, Bị Giam Giữ Nhiều Không).
 *   Sử dụng để Cập Nhập Con Số Gó Cùng Trên Nút Ví Tiền User Mọi Lúc Mọi Nơi.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

public class GetWalletBalanceQuery : IRequest<WalletBalanceDto>
{
    public Guid UserId { get; set; }
    
    public GetWalletBalanceQuery(Guid userId)
    {
        UserId = userId;
    }
}

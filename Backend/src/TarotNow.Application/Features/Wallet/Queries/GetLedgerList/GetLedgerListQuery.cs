/*
 * ===================================================================
 * FILE: GetLedgerListQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Wallet.Queries.GetLedgerList
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Hỏi Xin Cuốn Sổ Cái (Ledger). Dùng để vẽ trang Lịch Sử Biến Động Số Dư
 *   cho User xem họ đã nạp bao nhiêu, bị trừ tiền vô vụ gì, được hoàn tiền khi nào.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common.Models;
using System;

namespace TarotNow.Application.Features.Wallet.Queries.GetLedgerList;

public class GetLedgerListQuery : IRequest<PaginatedList<WalletTransactionDto>>
{
    public Guid UserId { get; set; }
    
    /// <summary>Số trang hiện tại (Trang 1, 2, 3...)</summary>
    public int Page { get; set; } = 1;
    
    /// <summary>Độ dài 1 trang. Mặc định 20 dòng lịch sử 1 mặt trang.</summary>
    public int Limit { get; set; } = 20;
    
    public GetLedgerListQuery(Guid userId, int page = 1, int limit = 20)
    {
        UserId = userId;
        // Bắt lỗi truyền tào lao từ Frontend (Truyền trang Âm 1 thì set về 1).
        Page = page < 1 ? 1 : page;
        // Chặn Load Max 1 Triệu Dòng Gây Đứng Máy Phía Server. Tối Đa 100 Dòng 1 Lần.
        Limit = limit < 1 ? 20 : limit > 100 ? 100 : limit;
    }
}

/*
 * ===================================================================
 * FILE: GetLedgerListQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Wallet.Queries.GetLedgerList
 * ===================================================================
 * MỤC ĐÍCH:
 *   Người Kế Toán Trưởng: Chạy vào kho (DB) Lôi Mớ Biên Lai (Lịch Sử Nạp Rút) Của Khách.
 *   Trả Về Gói Có Chỉ Số Total (Dùng để Frontend Hiển Thị Nút Trang 1,2,3).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common.Models;
using TarotNow.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.Wallet.Queries.GetLedgerList;

public class GetLedgerListQueryHandler : IRequestHandler<GetLedgerListQuery, PaginatedList<WalletTransactionDto>>
{
    private readonly ILedgerRepository _ledgerRepository;

    public GetLedgerListQueryHandler(ILedgerRepository ledgerRepository)
    {
        _ledgerRepository = ledgerRepository;
    }

    public async Task<PaginatedList<WalletTransactionDto>> Handle(GetLedgerListQuery request, CancellationToken cancellationToken)
    {
        // 1. Phép Đếm: Tổng số dòng biên lai chú này từng xài. (Để Frontend biết mà vẽ số Trang).
        var totalCount = await _ledgerRepository.GetTotalCountAsync(request.UserId, cancellationToken);

        // 2. Chặn truy vấn Đỡ tốn Dữ liệu (Nếu nghèo rớt mồng tơi Chưa Nạp Bao Giờ Thì Trả Về Mảng Rỗng Rẻ Tiền, Đỡ Mất Cấp Lôi CSDL Ra Quét).
        if (totalCount == 0)
        {
            return new PaginatedList<WalletTransactionDto>(new List<WalletTransactionDto>(), 0, request.Page, request.Limit);
        }

        // 3. Thực Sự Đào Database Dựa Vào Chỉ Số Vị Trí Hiện Tại (Page x Limit).
        var transactions = await _ledgerRepository.GetTransactionsAsync(request.UserId, request.Page, request.Limit, cancellationToken);

        // 4. Định Dạng Lại Áo Quần Gọn Gàng (Map Object Sang Dto) Bơm cho Frontend Xử.
        var dtos = transactions.Select(t => new WalletTransactionDto
        {
            Id = t.Id,
            Currency = t.Currency.ToString(),
            Type = t.Type.ToString(),
            Amount = t.Amount,
            BalanceBefore = t.BalanceBefore, // Tiền Trước Khi Sụp Hố.
            BalanceAfter = t.BalanceAfter,   // Cạn Lời Còn Lại X Đồng.
            Description = t.Description,
            CreatedAt = t.CreatedAt
        }).ToList();

        // 5. Build Khối Hàng Gói Ghém Đầy Đủ Số Liệu (Items + Metadata Trang).
        return new PaginatedList<WalletTransactionDto>(dtos, totalCount, request.Page, request.Limit);
    }
}

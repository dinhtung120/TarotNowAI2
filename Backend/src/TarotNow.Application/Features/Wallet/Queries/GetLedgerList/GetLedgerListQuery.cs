using MediatR;
using System;
using TarotNow.Application.Common.Models;

namespace TarotNow.Application.Features.Wallet.Queries.GetLedgerList;

// Query lấy danh sách giao dịch ví theo phân trang.
public class GetLedgerListQuery : IRequest<PaginatedList<WalletTransactionDto>>
{
    // Định danh user cần lấy ledger.
    public Guid UserId { get; set; }

    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Số bản ghi mỗi trang.
    public int Limit { get; set; } = 20;

    /// <summary>
    /// Khởi tạo query ledger với tham số phân trang đã chuẩn hóa.
    /// Luồng xử lý: ép page tối thiểu 1 và clamp limit trong khoảng [1,100] để bảo vệ hiệu năng truy vấn.
    /// </summary>
    public GetLedgerListQuery(Guid userId, int page = 1, int limit = 20)
    {
        UserId = userId;

        Page = page < 1 ? 1 : page;
        // Edge case: page âm/0 được chuẩn hóa về trang đầu tiên.

        Limit = limit < 1 ? 20 : limit > 100 ? 100 : limit;
        // Clamp limit để tránh request quá nhỏ/quá lớn gây ảnh hưởng hiệu năng.
    }
}

using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.Models;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Wallet.Queries.GetLedgerList;

// Handler truy vấn ledger giao dịch ví.
public class GetLedgerListQueryHandler : IRequestHandler<GetLedgerListQuery, PaginatedList<WalletTransactionDto>>
{
    private readonly ILedgerRepository _ledgerRepository;

    /// <summary>
    /// Khởi tạo handler lấy ledger list.
    /// Luồng xử lý: nhận ledger repository để truy vấn tổng bản ghi và dữ liệu giao dịch theo trang.
    /// </summary>
    public GetLedgerListQueryHandler(ILedgerRepository ledgerRepository)
    {
        _ledgerRepository = ledgerRepository;
    }

    /// <summary>
    /// Xử lý query lấy ledger.
    /// Luồng xử lý: lấy total count, trả trang rỗng nếu không có dữ liệu, ngược lại truy vấn giao dịch và map DTO phân trang.
    /// </summary>
    public async Task<PaginatedList<WalletTransactionDto>> Handle(
        GetLedgerListQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await _ledgerRepository.GetTotalCountAsync(request.UserId, cancellationToken);

        if (totalCount == 0)
        {
            // Edge case: user chưa có giao dịch nào thì trả cấu trúc phân trang rỗng ổn định cho client.
            return new PaginatedList<WalletTransactionDto>(
                new List<WalletTransactionDto>(),
                0,
                request.Page,
                request.Limit);
        }

        var transactions = await _ledgerRepository.GetTransactionsAsync(
            request.UserId,
            request.Page,
            request.Limit,
            cancellationToken);

        var dtos = transactions.Select(transaction => new WalletTransactionDto
        {
            Id = transaction.Id,
            Currency = transaction.Currency.ToString(),
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount,
            BalanceBefore = transaction.BalanceBefore,
            BalanceAfter = transaction.BalanceAfter,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        }).ToList();
        // Map entity sang DTO để API trả format ổn định, tách khỏi chi tiết persistence model.

        return new PaginatedList<WalletTransactionDto>(dtos, totalCount, request.Page, request.Limit);
    }
}

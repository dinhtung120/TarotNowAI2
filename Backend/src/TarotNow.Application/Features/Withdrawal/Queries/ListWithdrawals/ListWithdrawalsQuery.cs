using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals;

// Query lấy danh sách yêu cầu rút theo điều kiện lọc.
public class ListWithdrawalsQuery : IRequest<List<WithdrawalResult>>
{
    // Lọc theo user cụ thể (tùy chọn).
    public Guid? UserId { get; set; }

    // Cờ chỉ lấy request pending.
    public bool PendingOnly { get; set; }

    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;
}

// DTO kết quả một yêu cầu rút tiền.
public class WithdrawalResult
{
    // Định danh request.
    public Guid Id { get; set; }

    // Định danh user tạo request.
    public Guid UserId { get; set; }

    // Số diamond yêu cầu rút.
    public long AmountDiamond { get; set; }

    // Tổng tiền quy đổi VND.
    public long AmountVnd { get; set; }

    // Phí xử lý VND.
    public long FeeVnd { get; set; }

    // Số tiền thực nhận sau phí.
    public long NetAmountVnd { get; set; }

    // Tên ngân hàng nhận tiền.
    public string BankName { get; set; } = string.Empty;

    // Tên chủ tài khoản ngân hàng.
    public string BankAccountName { get; set; } = string.Empty;

    // Số tài khoản ngân hàng.
    public string BankAccountNumber { get; set; } = string.Empty;

    // Trạng thái request (pending/approved/rejected).
    public string Status { get; set; } = string.Empty;

    // Ghi chú user khi tạo yêu cầu.
    public string? UserNote { get; set; }

    // Ghi chú admin khi xử lý.
    public string? AdminNote { get; set; }

    // Thời điểm request được xử lý.
    public DateTime? ProcessedAt { get; set; }

    // Thời điểm tạo request.
    public DateTime CreatedAt { get; set; }
}

// Handler liệt kê yêu cầu rút tiền.
public class ListWithdrawalsQueryHandler : IRequestHandler<ListWithdrawalsQuery, List<WithdrawalResult>>
{
    private readonly IWithdrawalRepository _repo;

    /// <summary>
    /// Khởi tạo handler lấy danh sách withdrawal.
    /// Luồng xử lý: nhận withdrawal repository để truy vấn theo bộ lọc pending/user.
    /// </summary>
    public ListWithdrawalsQueryHandler(IWithdrawalRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Xử lý query liệt kê withdrawal requests.
    /// Luồng xử lý: chọn nguồn dữ liệu theo cờ PendingOnly/UserId, sau đó map entity sang DTO trả về.
    /// </summary>
    public async Task<List<WithdrawalResult>> Handle(ListWithdrawalsQuery request, CancellationToken cancellationToken)
    {
        List<WithdrawalRequest> items;

        if (request.PendingOnly)
        {
            items = await _repo.ListPendingAsync(request.Page, request.PageSize, cancellationToken);
            // Ưu tiên lọc pending cho dashboard xử lý yêu cầu chờ duyệt.
        }
        else if (request.UserId.HasValue)
        {
            items = await _repo.ListByUserAsync(request.UserId.Value, request.Page, request.PageSize, cancellationToken);
            // Khi có userId thì lấy lịch sử request theo user cụ thể.
        }
        else
        {
            items = await _repo.ListPendingAsync(request.Page, request.PageSize, cancellationToken);
            // Fallback mặc định: trả danh sách pending để tránh lộ toàn bộ lịch sử không có bộ lọc.
        }

        return items.Select(item => new WithdrawalResult
        {
            Id = item.Id,
            UserId = item.UserId,
            AmountDiamond = item.AmountDiamond,
            AmountVnd = item.AmountVnd,
            FeeVnd = item.FeeVnd,
            NetAmountVnd = item.NetAmountVnd,
            BankName = item.BankName,
            BankAccountName = item.BankAccountName,
            BankAccountNumber = item.BankAccountNumber,
            Status = item.Status,
            UserNote = item.UserNote,
            AdminNote = item.AdminNote,
            ProcessedAt = item.ProcessedAt,
            CreatedAt = item.CreatedAt
        }).ToList();
    }
}

using MediatR;

namespace TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

// DTO phản ánh chênh lệch số dư giữa bảng user và ledger.
public class MismatchRecordDto
{
    // Định danh người dùng bị lệch số dư.
    public Guid UserId { get; set; }

    // Số dư gold ghi trên hồ sơ user.
    public long UserGoldBalance { get; set; }

    // Số dư gold tính từ ledger giao dịch.
    public long LedgerGoldBalance { get; set; }

    // Số dư diamond ghi trên hồ sơ user.
    public long UserDiamondBalance { get; set; }

    // Số dư diamond tính từ ledger giao dịch.
    public long LedgerDiamondBalance { get; set; }
}

// Query lấy danh sách bản ghi lệch số dư để đối soát tài chính.
public class GetLedgerMismatchQuery : IRequest<List<MismatchRecordDto>>
{
}

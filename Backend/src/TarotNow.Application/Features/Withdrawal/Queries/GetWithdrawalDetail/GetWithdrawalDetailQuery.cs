using MediatR;
using TarotNow.Application.Common.Helpers;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Withdrawal.Queries.GetWithdrawalDetail;

// Query lấy chi tiết một withdrawal request phục vụ màn hình admin.
public class GetWithdrawalDetailQuery : IRequest<WithdrawalDetailResult>
{
    // Định danh yêu cầu rút tiền.
    public Guid WithdrawalId { get; set; }
}

// DTO chi tiết yêu cầu rút tiền kèm thông tin QR chuyển khoản.
public class WithdrawalDetailResult
{
    // Định danh yêu cầu rút tiền.
    public Guid Id { get; set; }

    // Định danh người dùng tạo yêu cầu.
    public Guid UserId { get; set; }

    // Số diamond yêu cầu rút.
    public long AmountDiamond { get; set; }

    // Tổng tiền quy đổi trước phí.
    public long AmountVnd { get; set; }

    // Phí xử lý theo VND.
    public long FeeVnd { get; set; }

    // Số tiền thực nhận theo VND.
    public long NetAmountVnd { get; set; }

    // Tên ngân hàng nhận tiền.
    public string BankName { get; set; } = string.Empty;

    // Mã BIN ngân hàng nhận tiền.
    public string BankBin { get; set; } = string.Empty;

    // Số tài khoản nhận tiền.
    public string BankAccountNumber { get; set; } = string.Empty;

    // Tên chủ tài khoản nhận tiền.
    public string BankAccountName { get; set; } = string.Empty;

    // Trạng thái yêu cầu.
    public string Status { get; set; } = string.Empty;

    // Ghi chú từ người dùng.
    public string? UserNote { get; set; }

    // Ghi chú từ admin.
    public string? AdminNote { get; set; }

    // Thời điểm tạo yêu cầu.
    public DateTime CreatedAt { get; set; }

    // Thời điểm xử lý yêu cầu.
    public DateTime? ProcessedAt { get; set; }

    // Nội dung chuyển khoản gợi ý cho admin.
    public string TransferContent { get; set; } = string.Empty;

    // URL QR chuyển khoản theo chuẩn VietQR.
    public string VietQrImageUrl { get; set; } = string.Empty;
}

// Handler truy vấn chi tiết withdrawal request.
public class GetWithdrawalDetailQueryHandler : IRequestHandler<GetWithdrawalDetailQuery, WithdrawalDetailResult>
{
    private const string TransferContentPrefix = "TAROTNOWWD";
    private const int TransferSuffixLength = 12;

    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Khởi tạo handler lấy chi tiết withdrawal.
    /// </summary>
    public GetWithdrawalDetailQueryHandler(IWithdrawalRepository withdrawalRepository)
    {
        _withdrawalRepository = withdrawalRepository;
    }

    /// <summary>
    /// Xử lý query chi tiết yêu cầu rút tiền.
    /// Luồng xử lý: tải request theo id, dựng transfer content và QR URL cho admin thao tác chuyển khoản.
    /// </summary>
    public async Task<WithdrawalDetailResult> Handle(GetWithdrawalDetailQuery request, CancellationToken cancellationToken)
    {
        var item = await _withdrawalRepository.GetByIdAsync(request.WithdrawalId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy yêu cầu rút tiền.");
        var transferContent = BuildTransferContent(item.Id);
        return new WithdrawalDetailResult
        {
            Id = item.Id,
            UserId = item.UserId,
            AmountDiamond = item.AmountDiamond,
            AmountVnd = item.AmountVnd,
            FeeVnd = item.FeeVnd,
            NetAmountVnd = item.NetAmountVnd,
            BankName = item.BankName,
            BankBin = item.BankBin,
            BankAccountNumber = item.BankAccountNumber,
            BankAccountName = item.BankAccountName,
            Status = item.Status,
            UserNote = item.UserNote,
            AdminNote = item.AdminNote,
            CreatedAt = item.CreatedAt,
            ProcessedAt = item.ProcessedAt,
            TransferContent = transferContent,
            VietQrImageUrl = VietQrHelper.BuildTransferQrImageUrl(
                item.BankBin,
                item.BankAccountNumber,
                item.NetAmountVnd,
                transferContent,
                item.BankAccountName)
        };
    }

    private static string BuildTransferContent(Guid withdrawalId)
    {
        var suffix = withdrawalId.ToString("N").ToUpperInvariant()[..TransferSuffixLength];
        return $"{TransferContentPrefix}{suffix}";
    }
}

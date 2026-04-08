

using TarotNow.Domain.Enums;
using TarotNow.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract nghiệp vụ ví để chuẩn hóa các thao tác ghi sổ credit/debit/freeze/release/refund.
public interface IWalletRepository
{
    /// <summary>
    /// Ghi tăng số dư và trả kết quả chi tiết để caller xử lý nhánh thất bại mềm.
    /// Luồng xử lý: tạo bút toán credit theo tham số đầu vào và trả WalletOperationResult sau khi thực thi.
    /// </summary>
    Task<WalletOperationResult> CreditWithResultAsync(Guid userId, string currency, string type, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi tăng số dư ví cho giao dịch không cần payload kết quả chi tiết.
    /// Luồng xử lý: thực thi bút toán credit theo user/currency/type và commit vào sổ cái.
    /// </summary>
    Task CreditAsync(Guid userId, string currency, string type, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi giảm số dư và trả kết quả chi tiết để kiểm soát trường hợp thiếu số dư.
    /// Luồng xử lý: thực hiện debit có kiểm tra điều kiện và trả WalletOperationResult tương ứng.
    /// </summary>
    Task<WalletOperationResult> DebitWithResultAsync(Guid userId, string currency, string type, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi giảm số dư ví cho giao dịch không yêu cầu đọc kết quả chi tiết.
    /// Luồng xử lý: thực thi debit theo tham số giao dịch và cập nhật sổ cái.
    /// </summary>
    Task DebitAsync(Guid userId, string currency, string type, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đóng băng một phần số dư để giữ tiền chờ kết quả xử lý nghiệp vụ.
    /// Luồng xử lý: chuyển amount từ khả dụng sang trạng thái hold theo reference tương ứng.
    /// </summary>
    Task FreezeAsync(Guid userId, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Giải ngân tiền hold từ payer sang receiver khi giao dịch được xác nhận hoàn tất.
    /// Luồng xử lý: giảm hold của payer, tăng số dư receiver và ghi bút toán liên kết.
    /// </summary>
    Task ReleaseAsync(Guid payerId, Guid receiverId, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Hoàn tiền về ví người dùng khi giao dịch bị hủy hoặc lỗi xử lý.
    /// Luồng xử lý: ghi bút toán refund, cộng lại số dư khả dụng theo reference giao dịch.
    /// </summary>
    Task RefundAsync(Guid userId, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tiêu thụ số dư ví cho các nghiệp vụ thanh toán trực tiếp trong hệ thống.
    /// Luồng xử lý: debit amount theo userId với metadata giao dịch để đối soát.
    /// </summary>
    Task ConsumeAsync(Guid userId, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);
}

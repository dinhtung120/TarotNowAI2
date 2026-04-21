using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract quản lý lệnh nạp tiền để đảm bảo vòng đời giao dịch được theo dõi đầy đủ.
public interface IDepositOrderRepository
{
    /// <summary>
    /// Khóa transaction theo client request key để serialize luồng tạo order idempotent.
    /// </summary>
    Task AcquireCreateOrderLockAsync(string clientRequestKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo id.
    /// </summary>
    Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo id với khóa hàng để cập nhật an toàn.
    /// </summary>
    Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo id và owner user.
    /// </summary>
    Task<DepositOrder?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo client request key.
    /// </summary>
    Task<DepositOrder?> GetByClientRequestKeyAsync(string clientRequestKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo PayOS order code.
    /// </summary>
    Task<DepositOrder?> GetByPayOsOrderCodeAsync(long payOsOrderCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo PayOS order code với khóa hàng.
    /// </summary>
    Task<DepositOrder?> GetByPayOsOrderCodeForUpdateAsync(long payOsOrderCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy các lệnh nạp pending quá ngưỡng thời gian để chạy job kiểm tra trạng thái.
    /// </summary>
    Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách lệnh nạp có phân trang cho màn hình quản trị.
    /// </summary>
    Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lịch sử nạp theo user có phân trang.
    /// </summary>
    Task<(IReadOnlyCollection<DepositOrder> Orders, int TotalCount)> GetPaginatedByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        string? status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo lệnh nạp mới.
    /// </summary>
    Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo lệnh nạp mới hoặc trả về lệnh đã tồn tại theo client request key khi có race idempotency.
    /// </summary>
    Task<DepositOrder> AddOrGetExistingByClientRequestKeyAsync(
        DepositOrder order,
        string clientRequestKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật lệnh nạp hiện có.
    /// </summary>
    Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default);
}

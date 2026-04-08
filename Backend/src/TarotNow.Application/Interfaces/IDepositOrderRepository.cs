
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract quản lý lệnh nạp tiền để đảm bảo vòng đời giao dịch được theo dõi đầy đủ.
public interface IDepositOrderRepository
{
    /// <summary>
    /// Lấy lệnh nạp theo id để xử lý các bước nghiệp vụ theo giao dịch cụ thể.
    /// Luồng xử lý: truy vấn chính xác theo Guid và trả null khi không tồn tại.
    /// </summary>
    Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo id với ngữ cảnh cập nhật để tránh xung đột ghi đồng thời.
    /// Luồng xử lý: truy vấn bản ghi cần sửa và chuẩn bị cho luồng update trong transaction.
    /// </summary>
    Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lệnh nạp theo mã transaction của cổng thanh toán để đối soát callback.
    /// Luồng xử lý: lọc theo transactionId và trả bản ghi khớp nếu có.
    /// </summary>
    Task<DepositOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy các lệnh nạp pending quá ngưỡng thời gian để chạy job kiểm tra trạng thái.
    /// Luồng xử lý: lọc theo trạng thái pending và tuổi lệnh lớn hơn olderThan.
    /// </summary>
    Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách lệnh nạp có phân trang để phục vụ màn hình quản trị giao dịch.
    /// Luồng xử lý: áp bộ lọc status tùy chọn, phân trang theo page/pageSize và trả tổng số.
    /// </summary>
    Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo lệnh nạp mới khi người dùng bắt đầu quy trình thanh toán.
    /// Luồng xử lý: persist entity order vào kho dữ liệu.
    /// </summary>
    Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật lệnh nạp hiện có để phản ánh trạng thái thanh toán mới nhất.
    /// Luồng xử lý: ghi đè các trường thay đổi của entity order tương ứng.
    /// </summary>
    Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default);
}

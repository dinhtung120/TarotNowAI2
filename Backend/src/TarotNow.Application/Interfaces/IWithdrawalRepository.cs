

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract yêu cầu rút tiền để quản lý luồng tạo đơn, xét duyệt và cập nhật trạng thái.
public interface IWithdrawalRepository
{
    /// <summary>
    /// Lấy yêu cầu rút tiền theo id để xử lý chi tiết hoặc duyệt thủ công.
    /// Luồng xử lý: truy vấn theo Guid và trả null nếu không tồn tại.
    /// </summary>
    Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Kiểm tra người dùng đã có đơn rút pending trong ngày hay chưa để áp dụng giới hạn tần suất.
    /// Luồng xử lý: lọc theo userId/businessDate và trả true khi đã tồn tại đơn chờ xử lý.
    /// </summary>
    Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default);

    /// <summary>
    /// Lấy lịch sử yêu cầu rút tiền của người dùng có phân trang.
    /// Luồng xử lý: lọc theo userId, áp page/pageSize và trả danh sách yêu cầu.
    /// </summary>
    Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Lấy danh sách yêu cầu rút tiền đang pending để vận hành xử lý hàng đợi.
    /// Luồng xử lý: lọc theo trạng thái pending và phân trang kết quả.
    /// </summary>
    Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Tạo yêu cầu rút tiền mới sau khi qua các bước validate nghiệp vụ.
    /// Luồng xử lý: persist request đầu vào vào kho dữ liệu rút tiền.
    /// </summary>
    Task AddAsync(WithdrawalRequest request, CancellationToken ct = default);

    /// <summary>
    /// Cập nhật yêu cầu rút tiền khi chuyển trạng thái xử lý hoặc ghi nhận kết quả.
    /// Luồng xử lý: ghi đè dữ liệu request theo id hiện có.
    /// </summary>
    Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default);

    /// <summary>
    /// Commit các thay đổi pending của ngữ cảnh rút tiền trong cùng đơn vị công việc.
    /// Luồng xử lý: lưu tất cả update/add đang theo dõi xuống dữ liệu bền vững.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}

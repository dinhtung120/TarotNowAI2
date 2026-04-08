
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract điều phối transaction để gom nhiều thao tác ghi dữ liệu vào cùng một đơn vị nhất quán.
public interface ITransactionCoordinator
{
    /// <summary>
    /// Thực thi một action trong transaction để đảm bảo tính toàn vẹn khi có nhiều bước cập nhật.
    /// Luồng xử lý: mở transaction, chạy action truyền vào, commit khi thành công hoặc rollback khi lỗi.
    /// </summary>
    Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default);
}

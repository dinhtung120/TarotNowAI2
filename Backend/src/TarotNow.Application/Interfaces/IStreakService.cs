using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract xử lý streak để cập nhật chuỗi hoạt động liên tục của người dùng.
public interface IStreakService
{
    /// <summary>
    /// Tăng streak khi người dùng thực hiện lượt rút bài hợp lệ trong ngày.
    /// Luồng xử lý: kiểm tra điều kiện hợp lệ theo userId rồi cập nhật chuỗi streak tương ứng.
    /// </summary>
    Task IncrementStreakOnValidDrawAsync(Guid userId, CancellationToken cancellationToken = default);
}

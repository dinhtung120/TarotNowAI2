

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract đẩy sự kiện ví để đồng bộ số dư realtime trên client.
public interface IWalletPushService
{
    /// <summary>
    /// Phát sự kiện thay đổi số dư ví để client cập nhật ngay mà không cần reload.
    /// Luồng xử lý: gửi thông báo balance changed theo userId đến kênh realtime tương ứng.
    /// </summary>
    Task PushBalanceChangedAsync(Guid userId, CancellationToken cancellationToken = default);
}

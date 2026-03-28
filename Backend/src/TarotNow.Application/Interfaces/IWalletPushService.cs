/*
 * ===================================================================
 * FILE: IWalletPushService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Interface quy định hợp đồng cho dịch vụ thông báo (Push) thay đổi 
 *   số dư ví tới Client theo một thời gian thực.
 *
 * LỢI ÍCH (Clean Architecture):
 *   - Các Handlers / Domain Events trong Application layer chỉ gọi Interface.
 *   - Không bị phụ thuộc vào SignalR hay các framework truyền tải bên dưới.
 * ===================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Service đẩy thông tin sự thay đổi Số dư Ví xuống Client.
/// </summary>
public interface IWalletPushService
{
    /// <summary>
    /// Bắn Signal "wallet.balance_changed" về Client (User).
    /// Client nhận được sẽ trigger refetch Ví mà không cần F5 trình duyệt.
    /// </summary>
    /// <param name="userId">ID của User có ví vừa thay đổi.</param>
    /// <param name="cancellationToken">Token nắn / hủy tiến trình.</param>
    Task PushBalanceChangedAsync(Guid userId, CancellationToken cancellationToken = default);
}

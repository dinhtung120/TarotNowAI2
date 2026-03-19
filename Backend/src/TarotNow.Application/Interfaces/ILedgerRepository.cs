/*
 * ===================================================================
 * FILE: ILedgerRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Sổ Cái Kế Toán (Ledger). Truy cập DB Lấy Biên Lai
 *   Thu / Chi để ráp nối ra trang Lịch Sử Biến Động Số Dư Của Khách.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Giao Tiếp Đọc Cuốn Sách Sổ Cái Chi Tiết Của Người Dùng.
/// (Chỉ Đọc Lịch Sử Lên chứ Không Có Hàm Sửa Lịch Sử Ở Đây Vì Sổ Cái Không Được Phép Sửa).
/// </summary>
public interface ILedgerRepository
{
    /// <summary>
    /// Bốc một Nắm Khai Báo Biến Lập Tài Khoản Ra Theo Khổ Giấy Vừa Đủ Cho Màn Hình App Tránh Nặng Máy (Phân Trang).
    /// </summary>
    Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Quẹt Hết Database Của Người Khách Này Coi Có Tới Hàng Nghìn Dòng Giao Dịch Không Để Frontend Còn Phân Dài Ra.
    /// </summary>
    Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default);
}

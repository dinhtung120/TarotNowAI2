/*
 * ===================================================================
 * FILE: IUserCollectionRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Sổ Sưu Tập Bài Tarot Cá Nhân Phím Chơi Của User (UserCollection - Giống Gacha Cày Cấp Nhân Vật Lục Card).
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Kho Gặp Tới Sổ Lưu Bộ Bài Của Người Khách. (Bốc Bao Nhiêu Lần Tích Được Exp Đủ Sắp Cấp Mới Nâng Số Cấp Tướng).
/// </summary>
public interface IUserCollectionRepository
{
    /// <summary>
    /// Tích Hợp Thông Minh 2 Cái Lệnh Insert (Sơ Khai Lần Đầu Mở) Hoặc Lệnh Update (Có Thẻ Này Rồi Thì Xin Phép Bồi Thêm Điểm Nhá/Upsert).
    /// </summary>
    /// <param name="userId">Bé Đi Xem Chơi Tên Gì (Mã ID).</param>
    /// <param name="cardId">ID Từ 0 Tới 77 Tương Ứng Với Vị Bài Trúng Đưa Ra Từ RNG Thuật Giải.</param>
    /// <param name="expToGain">Điểm Tích Lũy Ăn Đánh Ra Tặng Xịn Kịp Thăng Cấp (EXP).</param>
    Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default);

    /// <summary>Vét Lấy Bảng Đồ So Sò Đem Trưng Lên Kệ Giao Diện (User Xem Hộp Sưu Tặp Của Tui Có Ai Cấp 99 Đang Cháy Không).</summary>
    Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default);
}

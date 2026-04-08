

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract bộ sưu tập lá bài người dùng để quản lý tiến trình sở hữu và tăng cấp thẻ.
public interface IUserCollectionRepository
{
    /// <summary>
    /// Tạo mới hoặc cập nhật thẻ trong bộ sưu tập khi người dùng nhận thêm kinh nghiệm.
    /// Luồng xử lý: định vị thẻ theo userId/cardId, cộng expToGain và lưu trạng thái mới.
    /// </summary>
    Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ bộ sưu tập của người dùng để hiển thị kho thẻ hiện có.
    /// Luồng xử lý: lọc theo userId và trả danh sách UserCollection tương ứng.
    /// </summary>
    Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default);
}

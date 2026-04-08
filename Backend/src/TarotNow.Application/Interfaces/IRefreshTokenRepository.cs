

using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý refresh token để kiểm soát phiên đăng nhập dài hạn và thu hồi token.
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Lấy refresh token theo giá trị token để phục vụ xác thực gia hạn phiên.
    /// Luồng xử lý: truy vấn theo token và trả null nếu token không tồn tại hoặc đã bị xóa.
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu refresh token mới sau khi đăng nhập hoặc xoay vòng token thành công.
    /// Luồng xử lý: persist entity refresh token vào kho dữ liệu.
    /// </summary>
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật refresh token khi thay đổi trạng thái sử dụng hoặc thu hồi cục bộ.
    /// Luồng xử lý: ghi các trường thay đổi của entity refresh token tương ứng.
    /// </summary>
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi toàn bộ refresh token của người dùng khi logout all hoặc sự cố bảo mật.
    /// Luồng xử lý: tìm tất cả token thuộc userId và đánh dấu/loại bỏ để không thể dùng tiếp.
    /// </summary>
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

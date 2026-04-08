
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract quản lý chương trình khuyến mãi nạp tiền để đồng bộ ưu đãi theo từng giai đoạn.
public interface IDepositPromotionRepository
{
    /// <summary>
    /// Lấy khuyến mãi theo id để xử lý chỉnh sửa hoặc kiểm tra hiệu lực.
    /// Luồng xử lý: truy vấn theo Guid và trả null nếu bản ghi không tồn tại.
    /// </summary>
    Task<DepositPromotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ khuyến mãi để phục vụ quản trị cấu hình ưu đãi.
    /// Luồng xử lý: đọc tất cả bản ghi hiện có và trả danh sách kết quả.
    /// </summary>
    Task<IEnumerable<DepositPromotion>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy các khuyến mãi đang active để áp dụng cho giao dịch nạp tiền hiện tại.
    /// Luồng xử lý: lọc theo trạng thái hiệu lực và khoảng thời gian áp dụng.
    /// </summary>
    Task<IEnumerable<DepositPromotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo khuyến mãi mới khi có chương trình ưu đãi nạp tiền mới.
    /// Luồng xử lý: persist entity promotion vào kho dữ liệu.
    /// </summary>
    Task AddAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật khuyến mãi hiện có để điều chỉnh mức thưởng hoặc thời gian.
    /// Luồng xử lý: ghi các thay đổi của promotion theo id tương ứng.
    /// </summary>
    Task UpdateAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa khuyến mãi khi chương trình không còn áp dụng.
    /// Luồng xử lý: loại bỏ bản ghi promotion khỏi nguồn dữ liệu.
    /// </summary>
    Task DeleteAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);
}

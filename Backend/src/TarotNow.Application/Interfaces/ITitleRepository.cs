using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

// Contract quản lý danh hiệu để cấp và truy vấn title theo tiến độ người dùng.
public interface ITitleRepository
{
    /// <summary>
    /// Lấy toàn bộ định nghĩa danh hiệu để phục vụ cấu hình và hiển thị hệ thống.
    /// Luồng xử lý: đọc tất cả title definition hiện có và trả danh sách đầy đủ.
    /// </summary>
    Task<List<TitleDefinitionDto>> GetAllTitlesAsync(CancellationToken ct);

    /// <summary>
    /// Lấy định nghĩa danh hiệu theo mã để kiểm tra hợp lệ khi cấp.
    /// Luồng xử lý: truy vấn theo titleCode và trả null nếu không tồn tại.
    /// </summary>
    Task<TitleDefinitionDto?> GetByCodeAsync(string titleCode, CancellationToken ct);

    /// <summary>
    /// Lấy các danh hiệu người dùng đã sở hữu để hiển thị hồ sơ thành tựu.
    /// Luồng xử lý: lọc theo userId và trả danh sách user title.
    /// </summary>
    Task<List<UserTitleDto>> GetUserTitlesAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Kiểm tra người dùng đã sở hữu danh hiệu hay chưa để ngăn cấp trùng.
    /// Luồng xử lý: đối chiếu userId với titleCode và trả true nếu đã có.
    /// </summary>
    Task<bool> OwnsTitleAsync(Guid userId, string titleCode, CancellationToken ct);

    /// <summary>
    /// Cấp danh hiệu cho người dùng khi đạt điều kiện tương ứng.
    /// Luồng xử lý: tạo liên kết user-title mới và lưu vào kho dữ liệu.
    /// </summary>
    Task GrantTitleAsync(Guid userId, string titleCode, CancellationToken ct);

    /// <summary>
    /// Tạo mới hoặc cập nhật định nghĩa danh hiệu để đồng bộ metadata.
    /// Luồng xử lý: upsert theo title code và ghi dữ liệu definition mới nhất.
    /// </summary>
    Task UpsertTitleDefinitionAsync(TitleDefinitionDto title, CancellationToken ct);

    /// <summary>
    /// Xóa định nghĩa danh hiệu khi không còn được áp dụng trong hệ thống.
    /// Luồng xử lý: định vị theo titleCode và loại bỏ khỏi danh mục.
    /// </summary>
    Task DeleteTitleDefinitionAsync(string titleCode, CancellationToken ct);
}

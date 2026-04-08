

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract dữ liệu người dùng để phục vụ xác thực, hồ sơ và truy vấn quản trị.
public interface IUserRepository
{
    /// <summary>
    /// Lấy người dùng theo id để xử lý nghiệp vụ dựa trên định danh nội bộ.
    /// Luồng xử lý: truy vấn theo Guid và trả null nếu không có bản ghi tương ứng.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy người dùng theo email để hỗ trợ đăng nhập và kiểm tra trùng.
    /// Luồng xử lý: chuẩn hóa khóa email ở tầng lưu trữ rồi trả entity khớp.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy người dùng theo username để phục vụ đăng nhập hoặc tìm kiếm hồ sơ.
    /// Luồng xử lý: truy vấn theo username duy nhất và trả null nếu không tồn tại.
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra email đã tồn tại chưa để chặn đăng ký tài khoản trùng.
    /// Luồng xử lý: tìm theo email và trả true khi đã có user sở hữu.
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra username đã được sử dụng chưa trước khi tạo tài khoản mới.
    /// Luồng xử lý: truy vấn theo username và trả true nếu đã có bản ghi.
    /// </summary>
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm người dùng mới vào hệ thống sau bước validate đầu vào thành công.
    /// Luồng xử lý: persist entity user và khóa các trường định danh cần thiết.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật dữ liệu người dùng khi thay đổi trạng thái, hồ sơ hoặc quyền.
    /// Luồng xử lý: ghi các trường thay đổi của entity user theo id.
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách người dùng có phân trang để phục vụ màn hình quản trị.
    /// Luồng xử lý: áp tìm kiếm theo searchTerm, phân trang và trả users kèm total count.
    /// </summary>
    Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm người dùng theo một phần username để hỗ trợ autocomplete/chọn đối tượng.
    /// Luồng xử lý: truy vấn theo chuỗi usernamePart và trả danh sách khớp.
    /// </summary>
    Task<IEnumerable<User>> SearchUsersByUsernameAsync(string usernamePart, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy map userId -> username để giảm số lần truy vấn lặp cho cùng tập người dùng.
    /// Luồng xử lý: batch query theo userIds và trả Dictionary ánh xạ tên đăng nhập.
    /// </summary>
    Task<Dictionary<Guid, string>> GetUsernameMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin cơ bản người dùng theo batch để dựng UI danh sách hội thoại/bài viết.
    /// Luồng xử lý: truy vấn theo userIds và trả map gồm displayName, avatar, activeTitle.
    /// </summary>
    Task<Dictionary<Guid, (string DisplayName, string? AvatarUrl, string? ActiveTitle)>> GetUserBasicInfoMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
}

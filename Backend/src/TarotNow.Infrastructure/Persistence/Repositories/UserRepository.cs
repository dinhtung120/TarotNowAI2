

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository thao tác dữ liệu người dùng trên PostgreSQL.
public class UserRepository : IUserRepository
{
    // DbContext truy cập bảng users.
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository user.
    /// Luồng xử lý: nhận DbContext từ DI để dùng chung trong các luồng auth/admin/profile.
    /// </summary>
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Lấy user theo id.
    /// Luồng xử lý: query bản ghi đầu tiên khớp id.
    /// </summary>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <summary>
    /// Lấy user theo email.
    /// Luồng xử lý: query theo email chính xác.
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Lấy user theo username.
    /// Luồng xử lý: query theo username chính xác.
    /// </summary>
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra email đã tồn tại hay chưa.
    /// Luồng xử lý: dùng AnyAsync để trả kết quả boolean nhanh.
    /// </summary>
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra username đã tồn tại hay chưa.
    /// Luồng xử lý: dùng AnyAsync theo username.
    /// </summary>
    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>
    /// Thêm mới user.
    /// Luồng xử lý: add entity và save để phát sinh dữ liệu định danh hoàn chỉnh.
    /// </summary>
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cập nhật user hiện có.
    /// Luồng xử lý: mark modified và save vào DB.
    /// </summary>
    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách user phân trang có tìm kiếm.
    /// Luồng xử lý: chuẩn hóa page/pageSize, áp search theo email/displayName/username, đếm tổng rồi lấy trang theo created_at desc.
    /// </summary>
    public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Page size tối đa 200 để bảo vệ truy vấn admin list users.

        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u => u.Email.Contains(searchTerm) || u.DisplayName.Contains(searchTerm) || u.Username.Contains(searchTerm));
            // Tìm kiếm đa trường để tăng khả năng tra cứu user trong vận hành.
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    /// <summary>
    /// Tìm user theo một phần username.
    /// Luồng xử lý: chuẩn hóa chuỗi tìm kiếm về lower-case rồi lọc Contains trên username lower-case.
    /// </summary>
    public async Task<IEnumerable<User>> SearchUsersByUsernameAsync(string usernamePart, CancellationToken cancellationToken = default)
    {
        var part = usernamePart.Trim().ToLower();
        return await _dbContext.Users
            .Where(u => u.Username.ToLower().Contains(part))
            .ToListAsync(cancellationToken);
        // Dùng ToLower để thực hiện tìm kiếm gần đúng không phân biệt hoa thường.
    }

    /// <summary>
    /// Lấy map userId -> username.
    /// Luồng xử lý: lọc theo tập id, project trường tối thiểu rồi ToDictionaryAsync.
    /// </summary>
    public async Task<Dictionary<Guid, string>> GetUsernameMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var idList = userIds.ToList();
        return await _dbContext.Users
            .Where(u => idList.Contains(u.Id))
            .Select(u => new { u.Id, u.Username })
            .ToDictionaryAsync(x => x.Id, x => x.Username, cancellationToken);
    }

    /// <summary>
    /// Lấy map thông tin cơ bản user cho hiển thị.
    /// Luồng xử lý: project các trường cần thiết và fallback displayName về username khi displayName rỗng.
    /// </summary>
    public async Task<Dictionary<Guid, (string DisplayName, string? AvatarUrl, string? ActiveTitle)>> GetUserBasicInfoMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var idList = userIds.ToList();
        var users = await _dbContext.Users
            .Where(u => idList.Contains(u.Id))
            .Select(u => new { u.Id, u.DisplayName, xUsername = u.Username, u.AvatarUrl, u.ActiveTitleRef })
            .ToListAsync(cancellationToken);

        return users.ToDictionary(
            x => x.Id,
            x => (!string.IsNullOrWhiteSpace(x.DisplayName) ? x.DisplayName : x.xUsername, x.AvatarUrl, x.ActiveTitleRef)
        );
        // Edge case: user chưa đặt display name thì dùng username để tránh tên trống ở UI.
    }
}

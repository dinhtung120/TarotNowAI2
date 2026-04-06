/*
 * FILE: UserRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng users (PostgreSQL).
 *   Bảng users là bảng GỐC, quan trọng nhất — chứa thông tin tài khoản.
 *
 *   CÁC CHỨC NĂNG:
 *   → GetByIdAsync / GetByEmailAsync / GetByUsernameAsync: tìm User 
 *   → ExistsByEmailAsync / ExistsByUsernameAsync: kiểm tra trùng (đăng ký)
 *   → AddAsync / UpdateAsync: CRUD cơ bản
 *   → GetPaginatedUsersAsync: Admin dashboard (phân trang + tìm kiếm)
 *   → SearchUsersByUsernameAsync: tìm User theo username (chat/friend_chain)
 *   → GetUsernameMapAsync: bulk lookup — lấy bản đồ userId → username (hiệu quả)
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IUserRepository — truy cập bảng users (PostgreSQL).
/// Bảng trung tâm của hệ thống — mọi module đều tham chiếu tới.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>Tìm User theo Primary Key (UUID).</summary>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <summary>Tìm User theo email — dùng khi login hoặc đăng ký (kiểm tra trùng).</summary>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>Tìm User theo username — dùng khi login bằng username.</summary>
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>Kiểm tra email đã tồn tại chưa — dùng khi đăng ký (validation nhanh).</summary>
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>Kiểm tra username đã tồn tại chưa.</summary>
    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>Tạo User mới (đăng ký tài khoản).</summary>
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Cập nhật User (đổi thông tin, role, số dư, v.v.).</summary>
    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Admin dashboard: danh sách User có phân trang và tìm kiếm.
    /// Tìm kiếm theo: email, display_name, hoặc username (Contains = LIKE '%term%').
    /// Sắp xếp: User mới nhất trước (CreatedAt DESC).
    /// Giới hạn pageSize tối đa 200 để bảo vệ performance.
    /// </summary>
    public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var query = _dbContext.Users.AsQueryable();

        // Filter tìm kiếm (nếu có)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u => u.Email.Contains(searchTerm) || u.DisplayName.Contains(searchTerm) || u.Username.Contains(searchTerm));
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
    /// Tìm User theo username (partial match) — dùng cho autocomplete chat/friend.
    /// ToLower(): case-insensitive search (PostgreSQL ILIKE equivalent).
    /// </summary>
    public async Task<IEnumerable<User>> SearchUsersByUsernameAsync(string usernamePart, CancellationToken cancellationToken = default)
    {
        var part = usernamePart.Trim().ToLower();
        return await _dbContext.Users
            .Where(u => u.Username.ToLower().Contains(part))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Bulk lookup: lấy bản đồ userId → username cho nhiều User cùng lúc.
    /// Tại sao cần? → Khi hiển thị danh sách giao dịch/log, cần biết tên User.
    /// Thay vì N query (1 per user), 1 query duy nhất lấy tất cả → O(1) thay vì O(N).
    /// ToDictionaryAsync: kết quả = Dictionary&lt;Guid, string&gt; → lookup O(1).
    /// </summary>
    public async Task<Dictionary<Guid, string>> GetUsernameMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var idList = userIds.ToList();
        return await _dbContext.Users
            .Where(u => idList.Contains(u.Id))
            .Select(u => new { u.Id, u.Username }) // Chỉ lấy 2 trường cần thiết (không load toàn bộ User)
            .ToDictionaryAsync(x => x.Id, x => x.Username, cancellationToken);
    }

    /// <summary>
    /// Bản Đồ Kéo Gộp Cho Lệnh Tranh Trang Tin Nhắn Hộp Thư Tổng Hợp:
    /// Trả Name Và Hình Thay Vì Sầu Rụng UUID (Yêu cầu Mới Tính Năng Hỗn Hợp Vai Trò).
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
    }
}

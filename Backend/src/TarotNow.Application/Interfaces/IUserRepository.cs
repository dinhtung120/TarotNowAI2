/*
 * ===================================================================
 * FILE: IUserRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Giao Dịch Sổ Hộ Khẩu Dành Cho Bảng Cốt Lõi Hệ Thống: User.
 *   Hổ Lốn Tất Cả Dữ Liệu Tên Tuổi Pass Lưu Tại Data Cứng SQL Liên Quan Khách Đăng Nhập Đập Sóng Tới DB Thục Mạng.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Chòi Gác Lưu Thẻ Id Khách Vào Cõi. Ẩn che SQL Context Entity Framework Core Bên Trọng.
/// </summary>
public interface IUserRepository
{
    // Bốc Người Từ Lưới Bằng Ba Chân Kiềng (Guid ID/Mail Chuỗi/Tên Gọi Gọn):
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    // Check Tra Khởi Trị Không Bốc Khối User To Tốn Ram (Trả Có Hoặc Không Chặn Sớm Mấy Đoạn Sign Up Tạo Tài Khoản Vi Phạm List Đám Trùng):
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

    // Ký Báo Sanh Đi (Tạo Khách).
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    
    // Khách Trình Sự Thay Đổi Biến Động (Đổi Pass Đổi Thông Tin Vvv).
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    
    // Lệnh Dành Riêng Cho Quỷ Sếp Rảnh Rỗi Lướt Sổ Khách Thằng Admin Paginated Ngâm Cứu Tội Phạm, Phân Nhóm Thằng Thường, Thằng Quỷ.
    Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);

    // Search Kẹp @UserName Gợi Ý Gán User Tự Động Như Tự Tag Tên Kéo 1 Mớ Tên Thả Cửa Phím Chat Tìm Người Khác Nhắn (Tối Đa Số Gọi Trả Nhanh). Tồn Tại Do Auto-Complete Typeahead Chờ Bấm Ở Frontend Mượt Trả Chữ).
    Task<IEnumerable<User>> SearchUsersByUsernameAsync(string usernamePart, CancellationToken cancellationToken = default);
    
    // Thu Thập Tên Xóa ID Guids Gọi Về Dict Truy Ngược Mạng Không Để Mấy Ô UI Kẹp Chat Hiển Thị Giùm Guid Phê Mã (Convert Ráp Áo Bù UUID Thành Chữ Thường Trả UI).
    Task<Dictionary<Guid, string>> GetUsernameMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);

    // Kéo Đồng Thời Cả DisplayName, AvatarUrl Lẫn ActiveTitle Về Để Trải Ra Map Giao Diện Chat/Leaderboard Mới Gộp Chức Năng.
    Task<Dictionary<Guid, (string DisplayName, string? AvatarUrl, string? ActiveTitle)>> GetUserBasicInfoMapAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
}

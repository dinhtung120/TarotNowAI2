/*
 * ===================================================================
 * FILE: IUserConsentRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Chứa Thùng Sổ Văn Bản Cam Kết Của Khách.
 *   Nắm Ràng Buộc Hợp Pháp Sự Đồng Ý Ký Của Người Dùng Cho Các Tờ Điều Khoản Luật AI (ToS, Privacy) Khi Có Phốt Thì Lôi Ra Check.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Khúc Mộc Đóng Dấu Đỏ Của Quan Chiếu Ấn Tờ Chấp Thuận Của Người Chơi Phục Vụ Quy Tắc Giới Thiệu Chống Kiện Tụng Legal Đu Vấn Đề AI Đọc Loạn Trả Lời Xấu Tự Tử.
/// </summary>
public interface IUserConsentRepository
{
    /// <summary>Ra Lỗ Tra Lại Biên Bản Sự Đồng Ý Theo Phiên Bản Version Để Coi Lấy 1 Cái (Hôm Bữa Khách Kêu Tao Không Chịu Ký Bản V2). Đừng Mở Truy Lọc Lên Nữa Hả?</summary>
    Task<UserConsent?> GetConsentAsync(Guid userId, string documentType, string version, CancellationToken cancellationToken = default);
    
    /// <summary>Tóm Đầu Lôi Ra Cả Xấp Khế Ước Của Nó Đã Điểm Chỉ Ở DB (ToS v1, AI v2, Event v1...).</summary>
    Task<IEnumerable<UserConsent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>Tiến Xíu Thêm Cho Dây Bản Chữ Ký Xác Nhận Lưu Bảo Cất (Log Trạng Thái Khách Ấn I Agree).</summary>
    Task AddAsync(UserConsent consent, CancellationToken cancellationToken = default);
}

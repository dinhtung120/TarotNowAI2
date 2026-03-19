/*
 * ===================================================================
 * FILE: ITransactionCoordinator.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Đốc Công Quản Lý Đợi Việc (Transaction).
 *   Lớp Application Giao Cục Code Ở Trong Vòng Đóng Gói (Ví dụ Lệnh: Trừ Tiền Kèm Sinh Session)
 *   Bắt Buộc Ở Trọng Tài Rằng "Chạy Hết Không Lọt Đoạn Nào Hoặc Lỗi Thì Trả Liền Rollback".
 * ===================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Bao Công Phủ - Unit Of Work Đóng Gói Một Tổ Hợp Mã Thành Nguyên Tử (Atomicity).
/// Giới hạn Trách Nhiệm Application Chỉ Biết Gọi Hàm Này Và Truyền Hàm Ẩn Danh (Action) Ra, Không Biết Logic SqlTransaction Hay Chấm SaveChanges Gì Rườm Rà.
/// </summary>
public interface ITransactionCoordinator
{
    /// <summary>
    /// Chốt Phóng Bao Bọc. Thực Thi Bầy Code Khối Kèm Chặn Hủy Nếu Rối.
    /// Nếu Giữa Đường Đứt Bấm Tắt Máy Ngang Hoặc Mã Xảy Xung Đột Database Lỗi SQL Khảm Rách -> Trả Lại Sạch Bóng SQL Lúc Trước Khi Vô Khung Viết Này.
    /// </summary>
    Task ExecuteAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default);
}

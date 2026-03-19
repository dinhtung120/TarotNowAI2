/*
 * ===================================================================
 * FILE: ReaderApprovalStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Trang Thái Nộp Đơn Xin Làm Thầy Bói Cho Ban Admin Duyệt.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Status Duyệt Cho Bộ Hồ Sơ Xin Lên Chức Phán Thần Của Khách Bình Thường.
/// </summary>
public static class ReaderApprovalStatus
{
    // Cầm Giấy Ngồi Chờ Trong Lô Nộp Đơn Admin Đang Đi Uống Cafe Chưa Ngó.
    public const string Pending = "pending";
    
    // Con Dấu Đóng Mộc Đỏ Chấp Thuận Cấp Phép Lệnh Bài Đã Chuyển Role Lên Thành Thầy Reader.
    public const string Approved = "approved";
    
    // Gạch Hồ Sơ Vứt Thùng Rác Trả Tí Comment Admin Note Khinh Bỉ Vì Không Có Kinh Nghiệm Hay Thẻ Xấu.
    public const string Rejected = "rejected";
}

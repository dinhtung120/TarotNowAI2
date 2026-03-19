/*
 * ===================================================================
 * FILE: DomainException.cs
 * NAMESPACE: TarotNow.Domain.Exceptions
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gốc Gác Sâu Thẳm Của Mọi Kiểu Khóc Lỗi Khác. Khi Một Lớp DB Bị Lỗi Logic Nào Đó (VD: Trừ Tiền Âm), Nó Phải Sút Object Lỗi Này Lên Cho Application Chụp Để Tránh Lộ "SqlException" Trắng Chữ.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Exceptions;

/// <summary>
/// Thùng Chứa Báo Lỗi Ngạo Mạn Thuần Tuý Tầng Core Domain (Business Logic Rule).
/// Khối Code Này Đứng Đầu Kháng Gió Các Khối Ném Lỗi Khác (Khách Ko Đủ Tiền, Thẻ Bị Ẩn, Chát Khóa).
/// Thay Vì Gào Khóc Bằng `Exception` Thường Gây Bối Rối Web Tạch. Ta Tạo Object Này Ném Báo Dịu Dàng (Sạch Clean Architecture).
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// ID Mã Lỗi In Chữ Viết Hoa (Ví dụ: "INSUFFICIENT_FUNDS_BROKE").
    /// Giúp Thằng Frontend Dịch Bắt Key JSON Hiểu Sai Gì Không Cần Parse Chữ Tiếng Việt.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>Đúc Nặn Quả Bom Lỗi Để Ọt Quăng Xa.</summary>
    public DomainException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}

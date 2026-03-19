/*
 * ===================================================================
 * FILE: ValidationException.cs
 * NAMESPACE: TarotNow.Application.Exceptions
 * ===================================================================
 * MỤC ĐÍCH:
 *   Exception tùy chỉnh cho lỗi VALIDATION dữ liệu (HTTP 400).
 *   Được throw bởi ValidationBehavior khi dữ liệu đầu vào không hợp lệ.
 *
 * KHÁC GÌ VỚI BadRequestException?
 *   - BadRequestException: 1 thông báo lỗi chung
 *   - ValidationException: DANH SÁCH lỗi chi tiết theo từng field
 *
 * VÍ DỤ RESPONSE:
 *   {
 *     "status": 400,
 *     "title": "Validation Failed",
 *     "errors": {
 *       "Email": ["Email không đúng format", "Email đã tồn tại"],
 *       "Password": ["Password phải ít nhất 8 ký tự"],
 *       "DisplayName": ["Tên hiển thị không được để trống"]
 *     }
 *   }
 *
 * LUỒNG:
 *   Request vào → ValidationBehavior chạy FluentValidation
 *   → Nếu có lỗi → throw ValidationException(errorDictionary)
 *   → GlobalExceptionHandler bắt → trả ProblemDetails + errors
 * ===================================================================
 */

namespace TarotNow.Application.Exceptions;

/// <summary>
/// Exception tung ra khi dữ liệu đầu vào không vượt qua FluentValidation.
/// Kế thừa Exception để GlobalExceptionHandler dễ dàng bắt 
/// và chuyển đổi thành ProblemDetails HTTP 400 Bad Request.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Danh sách chi tiết các field bị lỗi và thông báo tương ứng.
    /// 
    /// Kiểu: IDictionary<string, string[]>
    ///   Key (string): tên field bị lỗi (ví dụ: "Email", "Password")
    ///   Value (string[]): mảng thông báo lỗi cho field đó
    ///
    /// "get;": read-only property (chỉ đọc, không set từ bên ngoài).
    ///   Giá trị chỉ được gán trong constructor → immutable.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Constructor nhận errors dictionary.
    ///
    /// ": base(...)": gọi constructor của class cha (Exception).
    ///   Message cố định "One or more validation failures..."
    ///   vì chi tiết lỗi nằm trong property Errors, không phải Message.
    /// </summary>
    public ValidationException(IDictionary<string, string[]> errors) 
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }
}

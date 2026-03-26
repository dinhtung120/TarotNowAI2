/*
 * ===================================================================
 * FILE: BadRequestException.cs
 * NAMESPACE: TarotNow.Application.Exceptions
 * ===================================================================
 * MỤC ĐÍCH:
 *   Exception tùy chỉnh cho lỗi "yêu cầu không hợp lệ" (HTTP 400).
 *
 * KHI NÀO DÙNG?
 *   Khi handler phát hiện yêu cầu sai logic (nhưng KHÔNG phải lỗi validation):
 *   - Ví dụ: "Tài khoản đã bị khóa, không thể đăng nhập"
 *   - Ví dụ: "Đã có đơn xin reader đang pending, không thể gửi thêm"
 *   - Ví dụ: "Số dư không đủ"
 *
 * KHÁC GÌ VỚI ValidationException?
 *   - ValidationException: lỗi FORMAT dữ liệu (email sai, password ngắn)
 *   - BadRequestException: lỗi LOGIC nghiệp vụ (tài khoản khóa, đã tồn tại)
 *   Cả hai đều trả HTTP 400, nhưng ValidationException kèm danh sách lỗi chi tiết.
 *
 * CÁCH DÙNG TRONG HANDLER:
 *   throw new BadRequestException("Tài khoản đã bị khóa.");
 *   → GlobalExceptionHandler bắt → trả ProblemDetails { status: 400, detail: "..." }
 * ===================================================================
 */

namespace TarotNow.Application.Exceptions;

/*
 * Kế thừa Exception (class gốc của mọi lỗi trong .NET).
 * ": base(message)": truyền message lên class cha Exception.
 *   Exception.Message sẽ chứa thông báo lỗi.
 *
 * Tại sao tạo class riêng thay vì dùng Exception trực tiếp?
 *   Để GlobalExceptionHandler phân biệt: 
 *   catch (BadRequestException) → 400
 *   catch (NotFoundException) → 404
 *   catch (Exception) → 500 (mặc định)
 */
public class BadRequestException : Exception
{
    public BadRequestException()
        : this("Bad request.")
    {
    }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

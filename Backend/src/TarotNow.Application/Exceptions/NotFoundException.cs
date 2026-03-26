/*
 * ===================================================================
 * FILE: NotFoundException.cs
 * NAMESPACE: TarotNow.Application.Exceptions
 * ===================================================================
 * MỤC ĐÍCH:
 *   Exception tùy chỉnh cho lỗi "không tìm thấy" (HTTP 404).
 *
 * KHI NÀO DÙNG?
 *   Khi handler tìm kiếm resource trong database nhưng KHÔNG có:
 *   - Ví dụ: "Không tìm thấy user với ID abc-123"
 *   - Ví dụ: "Conversation không tồn tại"
 *   - Ví dụ: "Phiên đọc bài không hợp lệ"
 *
 * CÁCH DÙNG TRONG HANDLER:
 *   var user = await _repo.GetById(userId);
 *   if (user == null)
 *       throw new NotFoundException($"User with Id {userId} was not found.");
 *   → GlobalExceptionHandler bắt → trả ProblemDetails { status: 404 }
 *
 * TẠI SAO KHÔNG DÙNG return NotFound()?
 *   Handler KHÔNG biết về HTTP (Clean Architecture).
 *   Handler chỉ throw exception → Controller/GlobalExceptionHandler xử lý HTTP.
 * ===================================================================
 */

namespace TarotNow.Application.Exceptions;

/*
 * Cấu trúc giống BadRequestException:
 *   - Kế thừa Exception
 *   - Constructor nhận message
 *   - GlobalExceptionHandler catch riêng → map sang HTTP 404
 */
public class NotFoundException : Exception
{
    public NotFoundException()
        : this("Resource was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

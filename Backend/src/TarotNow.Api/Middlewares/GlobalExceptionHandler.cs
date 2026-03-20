/*
 * ===================================================================
 * FILE: GlobalExceptionHandler.cs
 * NAMESPACE: TarotNow.Api.Middlewares
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   BỘ XỬ LÝ LỖI TOÀN CỤC - bắt TẤT CẢ exception (lỗi) chưa được xử lý
 *   trong toàn bộ ứng dụng và chuyển đổi thành response chuẩn hóa.
 *
 * TẠI SAO CẦN?
 *   Khi code throw exception mà không ai catch:
 *   - KHÔNG có handler: Server trả HTML lỗi 500 xấu xí, lộ stack trace (bảo mật kém)
 *   - CÓ handler (file này): Server trả JSON chuẩn ProblemDetails, ghi log, ẩn chi tiết
 *
 * PROBLEMDETAILS LÀ GÌ? (RFC 7807)
 *   Tiêu chuẩn quốc tế cho format lỗi API. Mọi lỗi đều có cấu trúc thống nhất:
 *   {
 *     "type": "https://...",       ← URL tài liệu mô tả loại lỗi
 *     "title": "Not Found",       ← Tóm tắt ngắn gọn
 *     "status": 404,              ← HTTP status code
 *     "detail": "User not found"  ← Chi tiết lỗi
 *   }
 *   Client chỉ cần xử lý 1 format duy nhất cho TẤT CẢ lỗi → đơn giản hóa code FE.
 *
 * BẢNG MAPPING EXCEPTION → HTTP STATUS:
 *   ValidationException       → 400 Bad Request (dữ liệu sai)
 *   BadRequestException       → 400 Bad Request (yêu cầu không hợp lệ)
 *   NotFoundException         → 404 Not Found (không tìm thấy)
 *   BusinessRuleException     → 422 Unprocessable (vi phạm quy tắc nghiệp vụ)
 *   ArgumentException         → 400 Bad Request (tham số sai)
 *   InvalidOperationException → 422 Unprocessable (thao tác không hợp lệ)
 *   UnauthorizedAccessException → 401 Unauthorized (chưa xác thực)
 *   DbUpdateException (withdrawal) → 400 Bad Request (giới hạn 1 lần/ngày)
 *   DbUpdateException (escrow)     → 409 Conflict (trùng lặp giao dịch)
 *   Exception (mặc định)     → 500 Internal Server Error
 * ===================================================================
 */

using Microsoft.AspNetCore.Diagnostics; // IExceptionHandler interface
using Microsoft.AspNetCore.Mvc;          // ProblemDetails class
using Microsoft.EntityFrameworkCore;     // DbUpdateException
using Npgsql;                            // PostgresException, PostgresErrorCodes

using TarotNow.Application.Exceptions;  // ValidationException, BadRequestException, NotFoundException, BusinessRuleException

namespace TarotNow.Api.Middlewares;

/// <summary>
/// Global exception handler sử dụng .NET 8/9 IExceptionHandler.
/// Bắt mọi exception không được xử lý và trả về ProblemDetails (RFC 7807) chuẩn hóa.
/// 
/// IExceptionHandler: Interface mới từ .NET 8, thay thế middleware cũ.
/// Được đăng ký trong Program.cs: builder.Services.AddExceptionHandler<GlobalExceptionHandler>()
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Method chính: được .NET gọi khi có exception chưa được xử lý.
    ///
    /// Tham số:
    ///   httpContext: thông tin request/response hiện tại
    ///   exception: lỗi xảy ra
    ///   cancellationToken: token hủy nếu client ngắt kết nối
    ///
    /// Trả về: true = đã xử lý xong, false = chuyển cho handler tiếp theo
    ///
    /// ValueTask<bool>: giống Task<bool> nhưng tối ưu hơn cho trường hợp
    ///   hàm hoàn thành nhanh (không cần allocate Task object trên heap).
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // GHI LOG LỖI — luôn log TRƯỚC khi xử lý (phòng trường hợp xử lý cũng lỗi)
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        /*
         * Khởi tạo ProblemDetails MẶC ĐỊNH = 500 Internal Server Error.
         * Nếu exception không khớp case nào bên dưới → dùng default 500.
         * Type: URL trỏ đến tài liệu RFC mô tả mã lỗi (quy ước quốc tế).
         * Detail: thông báo chung, KHÔNG lộ thông tin nhạy cảm.
         */
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error occurred while processing your request. Please try again later."
        };

        /*
         * SWITCH EXPRESSION: kiểm tra loại exception và tùy chỉnh ProblemDetails.
         * Mỗi case xử lý một loại lỗi khác nhau.
         * Thứ tự case QUAN TRỌNG vì C# kiểm tra từ trên xuống, case đầu tiên khớp → dừng.
         */
        switch (exception)
        {
            // ===== LỖI VALIDATION (400 Bad Request) =====
            // Do FluentValidation bắn khi dữ liệu đầu vào không hợp lệ
            // Ví dụ: email sai format, password quá ngắn
            case ValidationException validationException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Failed";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Detail = validationException.Message;
                // Thêm danh sách lỗi chi tiết: {"errors": [{"field": "Email", "message": "..."}]}
                problemDetails.Extensions["errors"] = validationException.Errors;
                break;

            // ===== LỖI YÊU CẦU KHÔNG HỢP LỆ (400 Bad Request) =====
            // Do handler bắn khi logic yêu cầu sai
            // Ví dụ: nạp số tiền âm, gửi đơn reader khi đã có đơn pending
            case BadRequestException badRequestException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Detail = badRequestException.Message;
                break;

            // ===== KHÔNG TÌM THẤY (404 Not Found) =====
            // Resource được yêu cầu không tồn tại trong database
            // Ví dụ: xem profile reader đã bị xóa, xem session không tồn tại
            case NotFoundException notFoundException:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
                problemDetails.Detail = notFoundException.Message;
                break;

            // ===== VI PHẠM QUY TẮC NGHIỆP VỤ (422 Unprocessable Entity) =====
            // Do Domain layer bắn khi business rule bị vi phạm
            // Ví dụ: số dư không đủ, email đã tồn tại, vượt quota
            // 422 thay vì 400: dữ liệu đúng format nhưng vi phạm logic nghiệp vụ
            case BusinessRuleException businessRuleException:
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "Domain Rule Violation";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2";
                problemDetails.Detail = businessRuleException.Message;
                // Thêm errorCode: để client xử lý theo từng loại lỗi cụ thể
                // Ví dụ: "INSUFFICIENT_BALANCE", "EMAIL_EXISTS"
                problemDetails.Extensions["errorCode"] = businessRuleException.ErrorCode;
                break;

            // ===== THAM SỐ SAI (400 Bad Request) =====
            // Do .NET bắn khi argument không hợp lệ
            case ArgumentException argumentException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Detail = argumentException.Message;
                break;

            // ===== THAO TÁC KHÔNG HỢP LỆ (422 Unprocessable Entity) =====
            // Ví dụ: gọi reveal khi session chưa init, confirm khi chưa reply
            case InvalidOperationException invalidOperationException:
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "Invalid Operation";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2";
                problemDetails.Detail = invalidOperationException.Message;
                break;

            // ===== CHƯA XÁC THỰC (401 Unauthorized) =====
            // Client chưa đăng nhập hoặc JWT hết hạn
            case UnauthorizedAccessException:
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = "You are not authorized to access this resource.";
                break;

            // ===== GIỚI HẠN RÚT TIỀN 1 LẦN/NGÀY (400 Bad Request) =====
            // PostgreSQL unique constraint "ix_withdrawal_one_per_day_active" bị vi phạm
            // Nghĩa là: user đã rút tiền hôm nay, không được rút thêm
            case DbUpdateException dbUpdateException when IsWithdrawalDailyLimitViolation(dbUpdateException):
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Detail = "Bạn đã có yêu cầu rút tiền hôm nay. Vui lòng thử lại ngày mai.";
                break;

            // ===== ESCROW TRÙNG LẶP (409 Conflict) =====
            // PostgreSQL unique constraint cho escrow bị vi phạm
            // Nghĩa là: giao dịch escrow này đã được tạo (idempotency key trùng)
            // 409 Conflict: request xung đột với trạng thái hiện tại của server
            case DbUpdateException dbUpdateException when IsEscrowUniquenessViolation(dbUpdateException):
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Conflict";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
                problemDetails.Detail = "Yêu cầu trùng lặp hoặc đã được xử lý trước đó.";
                break;
        }

        // Gán HTTP status code và content type cho response
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json"; // MIME type chuẩn RFC 7807

        // Serialize ProblemDetails thành JSON và ghi vào response body
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        // true = đã xử lý xong exception, không cần chuyển cho handler khác
        return true;
    }

    // ===================================================================
    // CÁC HÀM Helper KI ỂM TRA LỖI DATABASE CỤ THỂ
    // ===================================================================

    /// <summary>
    /// Kiểm tra xem lỗi database có phải do vi phạm giới hạn rút tiền 1/ngày không.
    /// 
    /// CÁCH HOẠT ĐỘNG:
    ///   Database PostgreSQL có unique index "ix_withdrawal_one_per_day_active":
    ///   chỉ cho phép 1 withdrawal request active (pending) mỗi ngày cho mỗi user.
    ///   Nếu user tạo request thứ 2 trong ngày → PostgreSQL throw UniqueViolation.
    ///   
    ///   "when" clause trong switch: chỉ khớp case nếu điều kiện kèm theo đúng.
    ///   Ví dụ: case DbUpdateException when IsWithdrawalDailyLimitViolation(...)
    ///   → chỉ khớp khi VỪA là DbUpdateException VỪA là lỗi daily limit.
    /// </summary>
    private static bool IsWithdrawalDailyLimitViolation(DbUpdateException exception)
    {
        // Kiểm tra inner exception có phải PostgresException không
        // "is not" pattern matching: nếu KHÔNG phải type PostgresException → return false
        if (exception.InnerException is not PostgresException postgresException)
        {
            return false;
        }

        // SqlState = "23505": mã lỗi PostgreSQL cho UNIQUE VIOLATION
        // ConstraintName: tên constraint bị vi phạm
        return postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(postgresException.ConstraintName, "ix_withdrawal_one_per_day_active", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kiểm tra xem lỗi database có phải do trùng lặp escrow không.
    /// 
    /// CÓ 2 CONSTRAINT ĐƯỢC KIỂM TRA:
    ///   1. "ix_chat_finance_sessions_conversation_ref": mỗi conversation chỉ có 1 finance session
    ///   2. "ix_chat_question_items_idempotency_key": mỗi idempotency key chỉ dùng 1 lần
    ///   
    ///   Cả 2 đều là cơ chế chống trùng lặp (idempotency) ở tầng database.
    /// </summary>
    private static bool IsEscrowUniquenessViolation(DbUpdateException exception)
    {
        if (exception.InnerException is not PostgresException postgresException)
        {
            return false;
        }

        // Phải là UNIQUE VIOLATION trước
        if (postgresException.SqlState != PostgresErrorCodes.UniqueViolation)
        {
            return false;
        }

        // Kiểm tra tên constraint khớp với escrow constraints
        return string.Equals(postgresException.ConstraintName, "ix_chat_finance_sessions_conversation_ref", StringComparison.OrdinalIgnoreCase)
               || string.Equals(postgresException.ConstraintName, "ix_chat_question_items_idempotency_key", StringComparison.OrdinalIgnoreCase);
    }

}

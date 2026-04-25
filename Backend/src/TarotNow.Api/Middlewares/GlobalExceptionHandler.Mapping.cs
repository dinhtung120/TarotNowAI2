using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;

namespace TarotNow.Api.Middlewares;

public partial class GlobalExceptionHandler
{
    /// <summary>
    /// Chuyển exception bất kỳ sang ProblemDetails tương ứng theo mức độ ưu tiên.
    /// Luồng xử lý: ưu tiên bắt lỗi database đặc thù, sau đó map lỗi nghiệp vụ đã biết, cuối cùng fallback 500.
    /// </summary>
    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        if (TryCreateDatabaseProblem(exception, out var databaseProblem))
        {
            // Nhánh ưu tiên cao: lỗi DB có constraint nghiệp vụ rõ ràng để trả thông điệp đúng ngữ cảnh.
            return databaseProblem;
        }

        var knownProblem = CreateKnownProblem(exception);
        // Fallback theo loại exception đã biết, nếu không khớp thì trả lỗi máy chủ chung.
        return knownProblem ?? CreateServerProblem();
    }

    /// <summary>
    /// Map nhóm exception ứng dụng thường gặp sang mã lỗi HTTP tương ứng.
    /// Luồng xử lý: lần lượt kiểm tra kiểu exception để đảm bảo rule domain trả đúng semantics cho client.
    /// </summary>
    private static ProblemDetails? CreateKnownProblem(Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            // Validation lỗi đầu vào: giữ chi tiết lỗi trường để client hiển thị đúng.
            return CreateValidationProblem(validationException);
        }

        if (exception is BadRequestException badRequestException)
        {
            // Nhánh bad request nghiệp vụ: trả 400 với thông điệp đã chuẩn hóa ở tầng application.
            _ = badRequestException;
            return CreateBadRequestProblem();
        }

        if (exception is NotFoundException notFoundException)
        {
            // Không tìm thấy tài nguyên: map sang 404 thay vì 500 để client xử lý retry hợp lý.
            _ = notFoundException;
            return CreateNotFoundProblem();
        }

        if (exception is BusinessRuleException businessRuleException)
        {
            // Vi phạm luật domain: dùng 422 để phân biệt với lỗi cú pháp request.
            return CreateBusinessRuleProblem(businessRuleException);
        }

        if (exception is ArgumentException argumentException)
        {
            // Guard clause ở tầng dưới ném ArgumentException thì map về 400 cho nhất quán.
            _ = argumentException;
            return CreateBadRequestProblem();
        }

        if (exception is InvalidOperationException invalidOperationException)
        {
            // Trạng thái không hợp lệ theo luồng xử lý nghiệp vụ: trả 422.
            _ = invalidOperationException;
            return CreateInvalidOperationProblem();
        }

        if (exception is UnauthorizedAccessException)
        {
            // Không đủ quyền truy cập tài nguyên: trả 401 để client kích hoạt luồng đăng nhập lại.
            return CreateUnauthorizedProblem();
        }

        // Edge case: exception chưa có mapping cụ thể, để caller quyết định fallback 500.
        return null;
    }

    /// <summary>
    /// Nhận diện lỗi constraint từ database và map sang thông điệp thân thiện với nghiệp vụ.
    /// Luồng xử lý: chỉ xử lý DbUpdateException, sau đó rẽ nhánh theo từng constraint đặc thù.
    /// </summary>
    private static bool TryCreateDatabaseProblem(Exception exception, out ProblemDetails problemDetails)
    {
        if (exception is not DbUpdateException dbUpdateException)
        {
            problemDetails = default!;
            // Không phải lỗi DB cập nhật: bỏ qua để mapping theo nhánh khác.
            return false;
        }

        if (IsWithdrawalWeeklyLimitViolation(dbUpdateException))
        {
            // Rule nghiệp vụ: mỗi tuần UTC chỉ cho một yêu cầu rút tiền.
            problemDetails = CreateBadRequestProblem();
            return true;
        }

        if (IsEscrowUniquenessViolation(dbUpdateException))
        {
            // Rule idempotency/uniqueness escrow: trả 409 để client tránh xử lý trùng.
            problemDetails = CreateConflictProblem("Yêu cầu trùng lặp hoặc đã được xử lý trước đó.");
            return true;
        }

        problemDetails = default!;
        // Lỗi DB khác chưa được nhận diện: cho phép fallback sang lỗi hệ thống.
        return false;
    }

    /// <summary>
    /// Tạo payload lỗi validation kèm danh sách lỗi chi tiết theo từng trường.
    /// Luồng xử lý: khởi tạo problem chuẩn 400 rồi gắn extension errors.
    /// </summary>
    private static ProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var problemDetails = CreateClientProblem(
            StatusCodes.Status400BadRequest,
            "Validation Failed",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            "One or more validation errors occurred.");

        // Gắn metadata lỗi chi tiết để frontend map trực tiếp vào form validation.
        problemDetails.Extensions["errors"] = exception.Errors;
        return problemDetails;
    }

    /// <summary>
    /// Tạo payload lỗi vi phạm quy tắc domain kèm mã lỗi nghiệp vụ.
    /// Luồng xử lý: khởi tạo problem 422 rồi thêm errorCode để client điều hướng UX theo rule cụ thể.
    /// </summary>
    private static ProblemDetails CreateBusinessRuleProblem(BusinessRuleException exception)
    {
        var status = ResolveBusinessRuleStatusCode(exception.ErrorCode);
        var title = status == StatusCodes.Status429TooManyRequests
            ? "Too Many Requests"
            : status == StatusCodes.Status401Unauthorized
                ? "Unauthorized"
                : "Domain Rule Violation";
        var problemDetails = CreateClientProblem(
            status,
            title,
            status == StatusCodes.Status429TooManyRequests
                ? "https://datatracker.ietf.org/doc/html/rfc6585#section-4"
                : status == StatusCodes.Status401Unauthorized
                    ? "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1"
                    : "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
            status == StatusCodes.Status429TooManyRequests
                ? "Rate limit exceeded. Please retry later."
                : status == StatusCodes.Status401Unauthorized
                    ? "Authentication is required or token is invalid."
                    : "A business rule was violated.");

        // Mã lỗi domain giúp client xử lý chính xác hơn thay vì chỉ dựa vào message tự do.
        problemDetails.Extensions["errorCode"] = exception.ErrorCode;
        return problemDetails;
    }

    private static int ResolveBusinessRuleStatusCode(string? errorCode)
    {
        return errorCode switch
        {
            AuthErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
            AuthErrorCodes.TokenExpired => StatusCodes.Status401Unauthorized,
            AuthErrorCodes.TokenReplay => StatusCodes.Status401Unauthorized,
            AuthErrorCodes.UserBlocked => StatusCodes.Status401Unauthorized,
            AuthErrorCodes.RateLimited => StatusCodes.Status429TooManyRequests,
            _ => StatusCodes.Status422UnprocessableEntity
        };
    }
}

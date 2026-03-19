/*
 * ===================================================================
 * FILE: ValidationBehavior.cs
 * NAMESPACE: TarotNow.Application.Behaviors
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   "Pipeline Behavior" (hành vi đường ống) của MediatR - TỰ ĐỘNG kiểm tra
 *   dữ liệu đầu vào TRƯỚC KHI handler xử lý command/query.
 *
 * GIẢI THÍCH ĐƠN GIẢN:
 *   Hãy tưởng tượng nhà máy sản xuất:
 *   - Nguyên liệu (request) đi qua KIỂM ĐỊNH CHẤT LƯỢNG (validation)
 *   - Nếu OK → chuyển vào XƯỞNG SẢN XUẤT (handler)
 *   - Nếu KHÔNG OK → trả về lỗi, KHÔNG cho vào xưởng
 *
 *   Pipeline behavior giống như trạm kiểm định đặt giữa đường ống:
 *   Request → [ValidationBehavior kiểm tra] → Handler xử lý → Response
 *
 * TẠI SAO DÙNG PATTERN NÀY?
 *   1. DRY (Don't Repeat Yourself): không cần viết validation ở MỖI handler
 *   2. Tách biệt concerns: handler chỉ lo business logic, không lo validation
 *   3. Tự động: mọi request đều được validate, không thể quên
 *
 * CÁCH HOẠT ĐỘNG:
 *   1. MediatR nhận request (command/query)
 *   2. TRƯỚC KHI gửi đến handler → chạy ValidationBehavior
 *   3. ValidationBehavior tìm tất cả Validator cho request type
 *   4. Chạy tất cả validator song song (Task.WhenAll)
 *   5. Nếu có lỗi → throw ValidationException → GlobalExceptionHandler → 400
 *   6. Nếu không lỗi → next() → cho request đi tiếp vào handler
 *
 * GENERIC TYPE PARAMETERS:
 *   TRequest: kiểu request (ví dụ: LoginCommand, GetProfileQuery)
 *   TResponse: kiểu response (ví dụ: AuthResponse, ProfileDto)
 * ===================================================================
 */

using FluentValidation; // Thư viện validation (kiểm tra dữ liệu)
using MediatR;          // Thư viện CQRS mediator pattern

namespace TarotNow.Application.Behaviors;

/// <summary>
/// Pipeline behavior tự động chạy FluentValidation trước handler.
/// Xử lý lỗi validation → throw ValidationException → HTTP 400/422.
/// </summary>
/*
 * IPipelineBehavior<TRequest, TResponse>: interface của MediatR.
 *   Khai báo class này là một "bước" trong pipeline xử lý request.
 *   MediatR sẽ tự động gọi Handle() TRƯỚC khi gọi handler.
 *
 * where TRequest : IRequest<TResponse>:
 *   Ràng buộc generic: TRequest phải implement IRequest<TResponse>.
 *   Đảm bảo chỉ áp dụng cho MediatR request, không phải object random.
 */
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /*
     * _validators: Danh sách TẤT CẢ validator cho request type TRequest.
     * 
     * IEnumerable<IValidator<TRequest>>: nhiều validator cho cùng 1 request type.
     *   Ví dụ: LoginCommand có thể có LoginCommandValidator VÀ LoginRateLimitValidator.
     *   DI container tự động inject tất cả validator đã đăng ký.
     *   Nếu không có validator nào → danh sách rỗng (không lỗi).
     */
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Method chính: được MediatR gọi TRƯỚC handler.
    ///
    /// Tham số:
    ///   request: command/query chứa dữ liệu cần validate
    ///   next: delegate gọi handler tiếp theo (hoặc behavior tiếp theo)
    ///   cancellationToken: token hủy khi client ngắt kết nối
    ///
    /// LUỒNG:
    ///   1. Nếu KHÔNG có validator → gọi next() ngay (bỏ qua validation)
    ///   2. Nếu CÓ validator → chạy tất cả → kiểm tra lỗi
    ///   3. Có lỗi → throw ValidationException
    ///   4. Không lỗi → gọi next() (cho request đi tiếp)
    /// </summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Kiểm tra có validator nào không (Any() = có ít nhất 1)
        if (_validators.Any())
        {
            /*
             * Tạo ValidationContext: bao bọc request để validator sử dụng.
             * Context chứa: instance request + metadata để validator đọc.
             */
            var context = new ValidationContext<TRequest>(request);

            /*
             * Chạy TẤT CẢ validator SONG SONG (parallel) để tối ưu performance.
             * Task.WhenAll: chờ tất cả task hoàn thành.
             * _validators.Select(v => v.ValidateAsync(...)): mỗi validator chạy async.
             * 
             * Kết quả: mảng ValidationResult[], mỗi phần tử chứa danh sách lỗi.
             */
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            /*
             * Gộp tất cả lỗi từ tất cả validator vào 1 danh sách:
             * SelectMany: "phẳng hóa" (flatten) danh sách lồng nhau.
             *   [[lỗi1, lỗi2], [lỗi3]] → [lỗi1, lỗi2, lỗi3]
             * Where(f => f != null): loại bỏ null (safety check).
             */
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // Nếu có lỗi → throw exception
            if (failures.Count != 0)
            {
                /*
                 * Nhóm lỗi theo tên property (field).
                 * Ví dụ kết quả:
                 * {
                 *   "Email": ["Email không hợp lệ", "Email đã tồn tại"],
                 *   "Password": ["Password phải ít nhất 8 ký tự"]
                 * }
                 *
                 * GroupBy(x => x.PropertyName): nhóm theo tên field
                 * ToDictionary: chuyển thành Dictionary<string, string[]>
                 *   Key = tên field, Value = mảng thông báo lỗi
                 */
                var errorDictionary = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
                
                /*
                 * Throw ValidationException tùy chỉnh (không phải FluentValidation.ValidationException).
                 * Exception này sẽ được GlobalExceptionHandler bắt → trả HTTP 400 + ProblemDetails.
                 */
                throw new TarotNow.Application.Exceptions.ValidationException(errorDictionary);
            }
        }

        /*
         * next(): Gọi handler tiếp theo trong pipeline.
         * Nếu không có behavior nào khác → gọi thẳng command/query handler.
         * Trả về TResponse (kết quả xử lý của handler).
         */
        return await next();
    }
}

/*
 * ===================================================================
 * FILE: SubscribeCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Subscription.Commands.Subscribe
 * ===================================================================
 * MỤC ĐÍCH:
 *   FluentValidation cho lệnh mua gói (SubscribeCommand).
 *   Kiểm tra các ràng buộc đầu vào trước khi handler thực thi logic nghiệp vụ.
 *
 * THIẾT KẾ:
 *   - UserId: không được rỗng (Guid.Empty) → ngăn lỗi "user not found" ở handler.
 *   - PlanId: không được rỗng → ngăn lỗi "plan not found" ở handler. 
 *   - IdempotencyKey: bắt buộc, phải có giá trị → ngăn trùng lặp giao dịch.
 *
 *   Lý do dùng FluentValidation thay vì check thủ công trong handler:
 *   MediatR pipeline behavior sẽ tự động chạy validator này TRƯỚC khi vào handler,
 *   trả về 400 Bad Request chuẩn ProblemDetails nếu lỗi — giữ handler sạch sẽ.
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

/// <summary>
/// Validator kiểm tra đầu vào của lệnh mua gói Subscription.
/// </summary>
public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
{
    public SubscribeCommandValidator()
    {
        // UserId không được rỗng — nếu rỗng nghĩa là token JWT parse sai hoặc request giả mạo
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        // PlanId không được rỗng — frontend phải gửi đúng ID gói từ danh sách
        RuleFor(x => x.PlanId)
            .NotEmpty()
            .WithMessage("PlanId is required.");

        // IdempotencyKey bắt buộc — đây là cơ chế chống double-click/retry trùng lặp
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithMessage("IdempotencyKey is required to prevent duplicate transactions.")
            .MaximumLength(200)
            .WithMessage("IdempotencyKey must not exceed 200 characters.");
    }
}

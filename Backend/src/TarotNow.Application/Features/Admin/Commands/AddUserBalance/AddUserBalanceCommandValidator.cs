/*
 * ===================================================================
 * FILE: AddUserBalanceCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.AddUserBalance
 * ===================================================================
 * MỤC ĐÍCH:
 *   Validator kiểm tra dữ liệu đầu vào cho AddUserBalanceCommand
 *   TRƯỚC KHI handler xử lý (qua ValidationBehavior pipeline).
 *
 * FLUENTVALIDATION:
 *   Thư viện cho phép viết validation rules dạng "fluent" (đọc như tiếng Anh).
 *   Ví dụ: RuleFor(x => x.Amount).GreaterThan(0)
 *   Đọc: "Quy tắc cho Amount: phải lớn hơn 0"
 *
 * TẠI SAO CÓ CẢ VALIDATOR VÀ KIỂM TRA TRONG HANDLER?
 *   - Validator: kiểm tra FORMAT (amount > 0, currency đúng, key không rỗng)
 *   - Handler: kiểm tra LOGIC (user tồn tại, idempotency key chưa dùng)
 *   Validator chạy TRƯỚC → nếu lỗi format → không cần gọi database.
 * ===================================================================
 */

using FluentValidation; // Thư viện validation
using TarotNow.Domain.Enums; // CurrencyType constants

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

/*
 * AbstractValidator<T>: class base của FluentValidation.
 *   T = kiểu command/query cần validate.
 *   Khai báo rules trong constructor.
 *   ValidationBehavior (pipeline) tự tìm validator cho command type → chạy.
 */
public class AddUserBalanceCommandValidator : AbstractValidator<AddUserBalanceCommand>
{
    public AddUserBalanceCommandValidator()
    {
        // UserId không được rỗng (Guid.Empty = 00000000-0000-0000-0000-000000000000)
        RuleFor(x => x.UserId).NotEmpty();

        /*
         * Amount phải > 0 (không cộng số âm hoặc 0).
         * .GreaterThan(0): rule tích hợp sẵn.
         * .WithMessage("..."): tùy chỉnh thông báo lỗi (thay vì mặc định tiếng Anh).
         */
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount phải lớn hơn 0.");

        /*
         * Currency phải là "gold" hoặc "diamond".
         * .Must(lambda): rule tùy chỉnh (trả true/false).
         *   - Normalize input (trim, lower) → so sánh
         *   - Trả true nếu hợp lệ, false nếu không
         */
        RuleFor(x => x.Currency)
            .Must(currency =>
            {
                var normalized = currency?.Trim().ToLowerInvariant();
                return normalized == CurrencyType.Gold || normalized == CurrencyType.Diamond;
            })
            .WithMessage("Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.");

        /*
         * IdempotencyKey: bắt buộc có, tối đa 128 ký tự.
         * NotEmpty(): không rỗng/null.
         * MaximumLength(128): giới hạn độ dài (tránh abuse gửi string quá dài).
         */
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}

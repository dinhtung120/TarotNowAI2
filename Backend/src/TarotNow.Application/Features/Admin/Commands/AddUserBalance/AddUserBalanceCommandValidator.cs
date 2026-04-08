using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

// Validator bảo vệ dữ liệu đầu vào cho command cộng số dư thủ công.
public class AddUserBalanceCommandValidator : AbstractValidator<AddUserBalanceCommand>
{
    /// <summary>
    /// Khởi tạo bộ rule validation cho AddUserBalanceCommand.
    /// Luồng xử lý: ràng buộc user id, amount dương, currency hợp lệ và idempotency key bắt buộc.
    /// </summary>
    public AddUserBalanceCommandValidator()
    {
        // UserId bắt buộc để xác định chính xác người nhận số dư.
        RuleFor(x => x.UserId).NotEmpty();

        // Rule nghiệp vụ: amount phải lớn hơn 0 để ngăn thao tác cộng vô nghĩa/âm.
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount phải lớn hơn 0.");

        RuleFor(x => x.Currency)
            .Must(currency =>
            {
                // Chuẩn hóa input trước khi so khớp để tránh sai khác do khoảng trắng/chữ hoa.
                var normalized = currency?.Trim().ToLowerInvariant();
                return normalized == CurrencyType.Gold || normalized == CurrencyType.Diamond;
            })
            .WithMessage("Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.");

        // IdempotencyKey bắt buộc và giới hạn chiều dài để phòng chống key bất thường.
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}

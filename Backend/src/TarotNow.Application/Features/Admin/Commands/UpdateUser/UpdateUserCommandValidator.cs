using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.UpdateUser;

// Validator đầu vào cho command cập nhật user từ admin.
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Khởi tạo bộ rule validation cho UpdateUserCommand.
    /// Luồng xử lý: kiểm tra user id, role/status, ràng buộc số dư không âm và idempotency key bắt buộc.
    /// </summary>
    public UpdateUserCommandValidator()
    {
        // UserId bắt buộc để định danh người dùng cần cập nhật.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // Role bắt buộc và giới hạn độ dài chuỗi.
        RuleFor(x => x.Role)
            .NotEmpty()
            .MaximumLength(50);

        // Status bắt buộc và giới hạn độ dài chuỗi.
        RuleFor(x => x.Status)
            .NotEmpty()
            .MaximumLength(50);

        // Số dư kim cương đích không được âm.
        RuleFor(x => x.DiamondBalance)
            .GreaterThanOrEqualTo(0);

        // Số dư vàng đích không được âm.
        RuleFor(x => x.GoldBalance)
            .GreaterThanOrEqualTo(0);

        // Idempotency key bắt buộc để chống cập nhật lặp.
        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

// Validator đầu vào cho command refresh token.
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho RefreshTokenCommand.
    /// Luồng xử lý: kiểm tra refresh token bắt buộc và client IP không rỗng/không vượt độ dài.
    /// </summary>
    public RefreshTokenCommandValidator()
    {
        // Token bắt buộc để thực hiện luồng refresh.
        RuleFor(x => x.Token)
            .NotEmpty();

        // Client IP bắt buộc để lưu audit thông tin phiên.
        RuleFor(x => x.ClientIpAddress)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.UserAgentHash)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);
    }
}

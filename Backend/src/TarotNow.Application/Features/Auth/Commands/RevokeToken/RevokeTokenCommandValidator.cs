using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

// Validator đầu vào cho command revoke token.
public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho RevokeTokenCommand.
    /// Luồng xử lý: nếu revoke all thì UserId bắt buộc; ngược lại token bắt buộc, đồng thời giới hạn độ dài token.
    /// </summary>
    public RevokeTokenCommandValidator()
    {
        // Validation điều kiện chéo giữa RevokeAll, UserId và Token.
        RuleFor(x => x)
            .Must(command =>
            {
                if (command.RevokeAll)
                {
                    return command.UserId.HasValue && command.UserId.Value != Guid.Empty;
                }

                return string.IsNullOrWhiteSpace(command.Token) == false;
            })
            .WithMessage("Khi RevokeAll=true cần UserId hợp lệ; ngược lại Token là bắt buộc.");

        // Token có thể dài, nhưng vẫn giới hạn để bảo vệ input bất thường.
        RuleFor(x => x.Token)
            .MaximumLength(1024)
            .When(x => string.IsNullOrWhiteSpace(x.Token) == false);
    }
}

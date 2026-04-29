using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(x => x)
            .Must(command =>
            {
                if (command.RevokeAll)
                {
                    return (command.UserId.HasValue && command.UserId.Value != Guid.Empty)
                           || string.IsNullOrWhiteSpace(command.Token) == false;
                }

                return string.IsNullOrWhiteSpace(command.Token) == false;
            })
            .WithMessage("Khi RevokeAll=true cần UserId hợp lệ hoặc refresh token; ngược lại Token là bắt buộc.");

        RuleFor(x => x.Token)
            .MaximumLength(1024)
            .When(x => string.IsNullOrWhiteSpace(x.Token) == false);
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenCommandValidator()
    {
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

        RuleFor(x => x.Token)
            .MaximumLength(1024)
            .When(x => string.IsNullOrWhiteSpace(x.Token) == false);
    }
}

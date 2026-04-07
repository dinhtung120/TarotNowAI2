using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.ClientIpAddress)
            .NotEmpty()
            .MaximumLength(64);
    }
}

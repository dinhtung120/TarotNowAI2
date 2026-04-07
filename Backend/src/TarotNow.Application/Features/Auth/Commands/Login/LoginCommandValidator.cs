

using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        
        
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email or Username is required.");

        
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        
        
        
    }
}



using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain alphanumeric characters and underscores.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(100).WithMessage("DisplayName cannot exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required.")
            .Must(BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old to register."); 

        RuleFor(x => x.HasConsented)
            .Equal(true).WithMessage("You must consent to the Terms of Service and Privacy Policy.");
    }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var minAge = 18;
        var today = DateTime.UtcNow.Date;
        var diff = today.Year - dateOfBirth.Year;
        
        
        if (dateOfBirth.Date > today.AddYears(-diff)) 
            diff--;

        return diff >= minAge;
    }
}

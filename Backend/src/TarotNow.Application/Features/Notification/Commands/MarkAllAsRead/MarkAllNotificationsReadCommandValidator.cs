using FluentValidation;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

public class MarkAllNotificationsReadCommandValidator : AbstractValidator<MarkAllNotificationsReadCommand>
{
    public MarkAllNotificationsReadCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

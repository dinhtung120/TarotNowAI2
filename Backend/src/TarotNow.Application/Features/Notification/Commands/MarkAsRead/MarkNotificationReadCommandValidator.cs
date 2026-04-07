using FluentValidation;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

public class MarkNotificationReadCommandValidator : AbstractValidator<MarkNotificationReadCommand>
{
    public MarkNotificationReadCommandValidator()
    {
        RuleFor(x => x.NotificationId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

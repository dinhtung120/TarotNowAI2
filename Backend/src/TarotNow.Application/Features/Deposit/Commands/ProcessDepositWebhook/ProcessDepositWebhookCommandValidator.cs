using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

// Validator đầu vào cho command xử lý webhook nạp tiền.
public class ProcessDepositWebhookCommandValidator : AbstractValidator<ProcessDepositWebhookCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho ProcessDepositWebhookCommand.
    /// </summary>
    public ProcessDepositWebhookCommandValidator()
    {
        RuleFor(x => x.RawPayload)
            .NotEmpty()
            .MaximumLength(64_000);
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

public class ProcessDepositWebhookCommandValidator : AbstractValidator<ProcessDepositWebhookCommand>
{
    public ProcessDepositWebhookCommandValidator()
    {
        RuleFor(x => x.RawPayload)
            .NotEmpty();

        RuleFor(x => x.Signature)
            .NotEmpty();

        RuleFor(x => x.PayloadData)
            .NotNull();

        RuleFor(x => x.PayloadData.OrderId)
            .NotEmpty();

        RuleFor(x => x.PayloadData.TransactionId)
            .NotEmpty();

        RuleFor(x => x.PayloadData.Amount)
            .GreaterThan(0);

        RuleFor(x => x.PayloadData.Status)
            .NotEmpty()
            .Must(status =>
            {
                var normalized = status?.Trim().ToUpperInvariant();
                return normalized is "SUCCESS" or "FAILED";
            })
            .WithMessage("Status webhook phải là SUCCESS hoặc FAILED.");
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

// Validator đầu vào cho command xử lý deposit webhook.
public class ProcessDepositWebhookCommandValidator : AbstractValidator<ProcessDepositWebhookCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho ProcessDepositWebhookCommand.
    /// Luồng xử lý: bắt buộc raw payload/signature/payload data và các trường con OrderId/TransactionId/Amount/Status hợp lệ.
    /// </summary>
    public ProcessDepositWebhookCommandValidator()
    {
        // RawPayload bắt buộc để verify chữ ký.
        RuleFor(x => x.RawPayload)
            .NotEmpty();

        // Signature bắt buộc để xác thực webhook.
        RuleFor(x => x.Signature)
            .NotEmpty();

        // PayloadData bắt buộc để xử lý nghiệp vụ.
        RuleFor(x => x.PayloadData)
            .NotNull();

        // OrderId bắt buộc để định vị order.
        RuleFor(x => x.PayloadData.OrderId)
            .NotEmpty();

        // TransactionId bắt buộc để đối soát giao dịch.
        RuleFor(x => x.PayloadData.TransactionId)
            .NotEmpty();

        // Amount phải dương.
        RuleFor(x => x.PayloadData.Amount)
            .GreaterThan(0);

        // Status chỉ chấp nhận SUCCESS hoặc FAILED.
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

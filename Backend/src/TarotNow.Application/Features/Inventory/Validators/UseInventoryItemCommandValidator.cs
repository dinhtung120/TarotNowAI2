using FluentValidation;
using TarotNow.Application.Features.Inventory.Commands;

namespace TarotNow.Application.Features.Inventory.Validators;

/// <summary>
/// Validator cho command sử dụng item trong kho đồ.
/// </summary>
public sealed class UseInventoryItemCommandValidator : AbstractValidator<UseInventoryItemCommand>
{
    /// <summary>
    /// Khởi tạo tập luật validate cho UseInventoryItemCommand.
    /// </summary>
    public UseInventoryItemCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ItemCode)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.TargetCardId)
            .GreaterThan(0)
            .When(x => x.TargetCardId.HasValue);
    }
}

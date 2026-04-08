using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

// Validator đầu vào cho command xử lý tranh chấp.
public class ResolveDisputeCommandValidator : AbstractValidator<ResolveDisputeCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho ResolveDisputeCommand.
    /// Luồng xử lý: kiểm tra item/admin id, action hợp lệ, split percent khi cần và độ dài admin note.
    /// </summary>
    public ResolveDisputeCommandValidator()
    {
        // ItemId bắt buộc để xác định đúng transaction item đang dispute.
        RuleFor(x => x.ItemId)
            .NotEmpty();

        // AdminId bắt buộc để truy vết người ra quyết định xử lý.
        RuleFor(x => x.AdminId)
            .NotEmpty();

        // Action chỉ cho phép release/refund/split.
        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action =>
            {
                // Chuẩn hóa action trước khi so khớp để tránh sai khác hoa-thường.
                var normalized = action?.Trim().ToLowerInvariant();
                return normalized is "release" or "refund" or "split";
            })
            .WithMessage("Action phải là 'release', 'refund' hoặc 'split'.");

        // Khi split thì tỷ lệ reader bắt buộc nằm trong [1,99].
        RuleFor(x => x.SplitPercentToReader)
            .NotNull()
            .InclusiveBetween(1, 99)
            .When(x => string.Equals(x.Action?.Trim(), "split", StringComparison.OrdinalIgnoreCase));

        // Admin note tùy chọn nhưng cần giới hạn độ dài để giữ metadata gọn.
        RuleFor(x => x.AdminNote)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.AdminNote) == false);
    }
}

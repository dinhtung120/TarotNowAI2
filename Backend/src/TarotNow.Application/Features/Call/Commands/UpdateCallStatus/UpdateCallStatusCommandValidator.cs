using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.UpdateCallStatus;

public sealed class UpdateCallStatusCommandValidator : AbstractValidator<UpdateCallStatusCommand>
{
    // Trạng thái call hợp lệ cho thao tác cập nhật.
    private static readonly string[] AllowedStatuses = ["requested", "accepted", "rejected", "ended"];

    /// <summary>
    /// Khởi tạo rule validation cho UpdateCallStatusCommand.
    /// Luồng xử lý: kiểm tra call session id, new status hợp lệ và expected previous status (nếu có) hợp lệ.
    /// </summary>
    public UpdateCallStatusCommandValidator()
    {
        // CallSessionId bắt buộc.
        RuleFor(x => x.CallSessionId)
            .NotEmpty();

        // NewStatus phải thuộc danh sách trạng thái được hỗ trợ.
        RuleFor(x => x.NewStatus)
            .NotEmpty()
            .Must(status => AllowedStatuses.Contains(status.Trim().ToLowerInvariant()))
            .WithMessage("Unsupported target call status.");

        // ExpectedPreviousStatus là tùy chọn nhưng nếu có thì phải hợp lệ.
        RuleFor(x => x.ExpectedPreviousStatus)
            .Must(status => string.IsNullOrWhiteSpace(status)
                || AllowedStatuses.Contains(status.Trim().ToLowerInvariant()))
            .WithMessage("Unsupported expected previous call status.");
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

public sealed class EndCallCommandValidator : AbstractValidator<EndCallCommand>
{
    // Danh sách lý do kết thúc cuộc gọi được hệ thống chấp nhận.
    private static readonly string[] AllowedReasons = ["normal", "timeout", "cancelled", "disconnected", "timeout_server"];

    /// <summary>
    /// Khởi tạo rule validation cho EndCallCommand.
    /// Luồng xử lý: kiểm tra call session id, user id và reason nằm trong danh sách cho phép.
    /// </summary>
    public EndCallCommandValidator()
    {
        // CallSessionId bắt buộc để xác định đúng cuộc gọi.
        RuleFor(x => x.CallSessionId)
            .NotEmpty();

        // UserId bắt buộc để kiểm tra quyền thao tác.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // Reason phải là một giá trị hợp lệ theo contract realtime/call flow.
        RuleFor(x => x.Reason)
            .NotEmpty()
            .Must(reason => AllowedReasons.Contains(reason))
            .WithMessage("Unsupported call end reason.");
    }
}

using FluentValidation;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Escrow.Commands.OpenDispute;

// Validator đầu vào cho command mở tranh chấp.
public class OpenDisputeCommandValidator : AbstractValidator<OpenDisputeCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cho OpenDisputeCommand.
    /// Luồng xử lý: bắt buộc ItemId/UserId/Reason, giới hạn Reason trong khoảng 10-1000 ký tự.
    /// </summary>
    public OpenDisputeCommandValidator(ISystemConfigSettings systemConfigSettings)
    {
        var minReasonLength = Math.Max(1, systemConfigSettings.EscrowDisputeMinReasonLength);

        // ItemId bắt buộc để định vị item tài chính.
        RuleFor(x => x.ItemId)
            .NotEmpty();

        // UserId bắt buộc để xác thực quyền participant.
        RuleFor(x => x.UserId)
            .NotEmpty();

        // Reason bắt buộc và phải đủ dài để moderation xử lý.
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(minReasonLength)
            .MaximumLength(1000);
    }
}

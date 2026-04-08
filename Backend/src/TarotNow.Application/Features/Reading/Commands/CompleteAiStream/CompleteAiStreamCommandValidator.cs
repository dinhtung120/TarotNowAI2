using FluentValidation;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

// Validator cho command hoàn tất AI stream.
public class CompleteAiStreamCommandValidator : AbstractValidator<CompleteAiStreamCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu complete stream.
    /// Luồng xử lý: kiểm tra định danh bắt buộc, ràng buộc final status hợp lệ và giới hạn các metric/lỗi đầu vào.
    /// </summary>
    public CompleteAiStreamCommandValidator()
    {
        RuleFor(x => x.AiRequestId)
            .NotEmpty();
        // AiRequestId bắt buộc để định vị bản ghi stream cần chốt.

        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để xử lý wallet settlement theo đúng chủ tài khoản.

        RuleFor(x => x.FinalStatus)
            .NotEmpty()
            .Must(AiStreamFinalStatuses.IsSupported)
            .WithMessage("FinalStatus không hợp lệ.");
        // Chỉ chấp nhận bộ trạng thái kết thúc chuẩn để tránh branch settlement không xác định.

        RuleFor(x => x.OutputTokens)
            .GreaterThanOrEqualTo(0);
        // Output token là metric nên không được âm.

        RuleFor(x => x.LatencyMs)
            .GreaterThanOrEqualTo(0);
        // Latency là metric thời gian nên không được âm.

        RuleFor(x => x.ErrorMessage)
            .MaximumLength(4000)
            .When(x => string.IsNullOrWhiteSpace(x.ErrorMessage) == false);
        // Giới hạn độ dài lỗi để tránh đẩy payload telemetry/bản ghi quá lớn.
    }
}

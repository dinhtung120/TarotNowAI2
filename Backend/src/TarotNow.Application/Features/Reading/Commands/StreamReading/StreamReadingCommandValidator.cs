using FluentValidation;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

// Validator cho command stream reading.
public class StreamReadingCommandValidator : AbstractValidator<StreamReadingCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu stream reading.
    /// Luồng xử lý: kiểm tra định danh bắt buộc, giới hạn follow-up question và ràng buộc language.
    /// </summary>
    public StreamReadingCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để kiểm tra quyền và quota theo tài khoản.

        RuleFor(x => x.ReadingSessionId)
            .NotEmpty();
        // SessionId bắt buộc để truy xuất đúng phiên cần stream.

        RuleFor(x => x.FollowupQuestion)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.FollowupQuestion) == false);
        // Follow-up là tùy chọn nhưng cần giới hạn độ dài để tránh payload quá lớn.

        RuleFor(x => x.Language)
            .NotEmpty()
            .MaximumLength(10);
        // Language bắt buộc và giới hạn ngắn để giữ format mã ngôn ngữ nhất quán.
    }
}

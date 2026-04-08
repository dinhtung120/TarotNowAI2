using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

// Validator cho command cập nhật trạng thái Reader.
public class UpdateReaderStatusCommandValidator : AbstractValidator<UpdateReaderStatusCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu đổi trạng thái reader.
    /// Luồng xử lý: kiểm tra UserId, đảm bảo status normalize được và chặn đặt thủ công trạng thái online.
    /// </summary>
    public UpdateReaderStatusCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để định vị chính xác reader cần thay đổi trạng thái.

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status => ReaderOnlineStatus.TryNormalize(status, out _))
            .WithMessage("Status không hợp lệ.");
        // Chỉ nhận status có thể normalize về tập giá trị hợp lệ của hệ thống.

        RuleFor(x => x.Status)
            .Must(status =>
            {
                if (!ReaderOnlineStatus.TryNormalize(status, out var normalized))
                {
                    // Không normalize được thì đã fail rule trước; trả false để giữ kết quả nhất quán.
                    return false;
                }

                return normalized != ReaderOnlineStatus.Online;
            })
            .WithMessage("Không thể đặt status thủ công là 'online'.");
        // Business rule: trạng thái online do hệ thống realtime quản lý, không cho client set tay.
    }
}

using FluentValidation;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

// Validator cho command cập nhật reader profile.
public class UpdateReaderProfileCommandValidator : AbstractValidator<UpdateReaderProfileCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu cập nhật reader profile.
    /// Luồng xử lý: kiểm tra user id, giới hạn độ dài bio đa ngôn ngữ, ràng buộc giá và từng specialty.
    /// </summary>
    public UpdateReaderProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để truy xuất đúng profile cần chỉnh sửa.

        RuleFor(x => x.BioVi)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.BioVi) == false);
        // Chỉ validate độ dài khi có dữ liệu để hỗ trợ patch field tùy chọn.

        RuleFor(x => x.BioEn)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.BioEn) == false);
        // Chỉ validate độ dài khi có dữ liệu để hỗ trợ patch field tùy chọn.

        RuleFor(x => x.BioZh)
            .MaximumLength(2000)
            .When(x => string.IsNullOrWhiteSpace(x.BioZh) == false);
        // Chỉ validate độ dài khi có dữ liệu để hỗ trợ patch field tùy chọn.

        RuleFor(x => x.DiamondPerQuestion)
            .GreaterThan(0)
            .When(x => x.DiamondPerQuestion.HasValue);
        // Giá dịch vụ phải dương khi user gửi yêu cầu thay đổi giá.

        RuleForEach(x => x.Specialties!)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Specialties is { Count: > 0 });
        // Mỗi specialty cần có nội dung hợp lệ để tránh lưu phần tử rỗng.
    }
}

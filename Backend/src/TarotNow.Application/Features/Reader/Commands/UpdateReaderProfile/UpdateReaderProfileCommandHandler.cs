using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

// Handler cập nhật thông tin hồ sơ Reader.
public class UpdateReaderProfileCommandHandler : IRequestHandler<UpdateReaderProfileCommand, bool>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật reader profile.
    /// Luồng xử lý: nhận repository để tải hồ sơ reader theo user và lưu thay đổi sau cập nhật.
    /// </summary>
    public UpdateReaderProfileCommandHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <summary>
    /// Xử lý command cập nhật reader profile.
    /// Luồng xử lý: tải profile hiện tại, cập nhật có chọn lọc theo trường được gửi lên, kiểm tra giá hợp lệ và persist.
    /// </summary>
    public async Task<bool> Handle(UpdateReaderProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(),
            cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader. Bạn cần được admin duyệt trước.");

        if (request.BioVi is not null)
        {
            profile.BioVi = request.BioVi;
        }

        if (request.BioEn is not null)
        {
            profile.BioEn = request.BioEn;
        }

        if (request.BioZh is not null)
        {
            profile.BioZh = request.BioZh;
        }
        // Cập nhật từng ngôn ngữ theo kiểu partial update để không ghi đè trường không được gửi.

        if (request.DiamondPerQuestion.HasValue)
        {
            if (request.DiamondPerQuestion.Value <= 0)
            {
                // Business rule: giá đọc bài phải dương để tránh cấu hình miễn phí ngoài chính sách.
                throw new BadRequestException("Giá mỗi câu hỏi phải lớn hơn 0 Diamond.");
            }

            profile.DiamondPerQuestion = request.DiamondPerQuestion.Value;
            // Đổi state giá dịch vụ sau khi vượt qua kiểm tra nghiệp vụ.
        }

        if (request.Specialties is not null)
        {
            profile.Specialties = request.Specialties;
            // Cho phép thay mới danh sách chuyên môn để đồng bộ đúng dữ liệu người dùng chỉnh sửa.
        }

        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        // Persist toàn bộ thay đổi profile để hiển thị ngay ở màn hình listing/detail reader.

        return true;
    }
}

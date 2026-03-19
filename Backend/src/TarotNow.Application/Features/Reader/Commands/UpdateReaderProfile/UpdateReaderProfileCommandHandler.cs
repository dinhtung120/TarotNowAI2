/*
 * ===================================================================
 * FILE: UpdateReaderProfileCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thực thi việc Cập nhật Profile xuống Database MongoDB (Partial Update).
 * ===================================================================
 */

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

public class UpdateReaderProfileCommandHandler : IRequestHandler<UpdateReaderProfileCommand, bool>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UpdateReaderProfileCommandHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<bool> Handle(UpdateReaderProfileCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra xem Người Mày có Dấu Tích Xanh (Profile) trong Database chưa?
        // Nếu chưa đậu duyệt Admin, MongoDB sẽ không có dòng nào cho ID này.
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader. Bạn cần được admin duyệt trước.");

        // 2. Chắp vá (Patching). Cập nhật cái gì thì Ghi Đè cái đó (Bỏ qua Null).
        if (request.BioVi != null) profile.BioVi = request.BioVi;
        if (request.BioEn != null) profile.BioEn = request.BioEn;
        if (request.BioZh != null) profile.BioZh = request.BioZh;

        // 3. Logic Kinh Tế: Cấm Set Giá Free Hoặc Lấy Tiền Âm Của Khách.
        if (request.DiamondPerQuestion.HasValue)
        {
            if (request.DiamondPerQuestion.Value <= 0)
                throw new BadRequestException("Giá mỗi câu hỏi phải lớn hơn 0 Diamond.");

            profile.DiamondPerQuestion = request.DiamondPerQuestion.Value;
        }

        // 4. Sửa danh sách Chuyên Nghành Gõ Bài.
        if (request.Specialties != null)
        {
            profile.Specialties = request.Specialties;
        }

        // 5. Thắp Hương Cố định Dữ liệu Xuống Tủ (Database).
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);

        return true;
    }
}

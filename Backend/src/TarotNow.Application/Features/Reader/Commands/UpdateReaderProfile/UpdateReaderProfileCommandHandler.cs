using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

/// <summary>
/// Handler cập nhật hồ sơ Reader — partial update pattern.
/// Chỉ cập nhật fields không null trong request.
/// </summary>
public class UpdateReaderProfileCommandHandler : IRequestHandler<UpdateReaderProfileCommand, bool>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UpdateReaderProfileCommandHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<bool> Handle(UpdateReaderProfileCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy profile hiện tại — chỉ reader đã approved mới có
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader. Bạn cần được admin duyệt trước.");

        // 2. Partial update — chỉ ghi đè fields được cung cấp (không null)
        // DTO dùng flat fields (BioVi, BioEn, BioZh) thay vì nested object
        if (request.BioVi != null) profile.BioVi = request.BioVi;
        if (request.BioEn != null) profile.BioEn = request.BioEn;
        if (request.BioZh != null) profile.BioZh = request.BioZh;

        // 3. Validate và cập nhật pricing — phải dương
        if (request.DiamondPerQuestion.HasValue)
        {
            if (request.DiamondPerQuestion.Value <= 0)
                throw new BadRequestException("Giá mỗi câu hỏi phải lớn hơn 0 Diamond.");

            profile.DiamondPerQuestion = request.DiamondPerQuestion.Value;
        }

        // 4. Cập nhật specialties nếu có
        if (request.Specialties != null)
        {
            profile.Specialties = request.Specialties;
        }

        // 5. Lưu — repository sẽ map DTO → Document nội bộ
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);

        return true;
    }
}

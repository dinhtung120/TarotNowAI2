

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
        
        
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader. Bạn cần được admin duyệt trước.");

        
        if (request.BioVi != null) profile.BioVi = request.BioVi;
        if (request.BioEn != null) profile.BioEn = request.BioEn;
        if (request.BioZh != null) profile.BioZh = request.BioZh;

        
        if (request.DiamondPerQuestion.HasValue)
        {
            if (request.DiamondPerQuestion.Value <= 0)
                throw new BadRequestException("Giá mỗi câu hỏi phải lớn hơn 0 Diamond.");

            profile.DiamondPerQuestion = request.DiamondPerQuestion.Value;
        }

        
        if (request.Specialties != null)
        {
            profile.Specialties = request.Specialties;
        }

        
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);

        return true;
    }
}

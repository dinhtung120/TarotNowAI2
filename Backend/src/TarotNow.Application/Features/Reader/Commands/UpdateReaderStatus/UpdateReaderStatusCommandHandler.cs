

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

public class UpdateReaderStatusCommandHandler : IRequestHandler<UpdateReaderStatusCommand, bool>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UpdateReaderStatusCommandHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<bool> Handle(UpdateReaderStatusCommand request, CancellationToken cancellationToken)
    {
        
        
        if (!ReaderOnlineStatus.TryNormalize(request.Status, out var normalizedStatus))
            throw new BadRequestException($"Trạng thái '{request.Status}' không hợp lệ. Chỉ chấp nhận: offline, busy.");
            
        
        if (normalizedStatus == ReaderOnlineStatus.Online)
            throw new BadRequestException("Trạng thái 'online' được cập nhật tự động khi kết nối. Truyền 'busy' hoặc 'offline' để đổi trạng thái thủ công.");

        
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        
        profile.Status = normalizedStatus;
        
        
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);

        return true;
    }
}

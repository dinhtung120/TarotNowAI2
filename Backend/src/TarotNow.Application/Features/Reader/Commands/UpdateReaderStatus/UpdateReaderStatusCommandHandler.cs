using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

/// <summary>
/// Handler chuyển đổi trạng thái online của Reader.
///
/// Validate status hợp lệ (online/offline/accepting_questions)
/// và cập nhật trong reader_profiles collection.
///
/// Tại sao validate status ở handler thay vì FluentValidation?
/// → Status là domain concept (ReaderOnlineStatus enum).
/// → Validate ở đây để có error message business-friendly.
/// </summary>
public class UpdateReaderStatusCommandHandler : IRequestHandler<UpdateReaderStatusCommand, bool>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public UpdateReaderStatusCommandHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<bool> Handle(UpdateReaderStatusCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate status hợp lệ — chỉ chấp nhận 3 giá trị
        var validStatuses = new[] {
            ReaderOnlineStatus.Online,
            ReaderOnlineStatus.Offline,
            ReaderOnlineStatus.AcceptingQuestions
        };

        if (!validStatuses.Contains(request.Status))
            throw new BadRequestException($"Trạng thái '{request.Status}' không hợp lệ. Chỉ chấp nhận: online, offline, accepting_questions.");

        // 2. Lấy profile Reader — chỉ reader đã approved mới có profile
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        // 3. Cập nhật status và lưu
        profile.Status = request.Status;
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);

        return true;
    }
}

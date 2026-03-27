/*
 * ===================================================================
 * FILE: UpdateReaderStatusCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus
 * ===================================================================
 * MỤC ĐÍCH:
 *   Áp Dụng lệnh gạt công tắc Online/Offline ở Database.
 * ===================================================================
 */

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
        // 1. Dựng Hàng Rào Phòng Thủ (Validation):
        // Nếu Frontend truyền bậy chữ "DangDiChoi" vào, ta Búng Tay văng Lỗi Error 400.
        if (!ReaderOnlineStatus.TryNormalize(request.Status, out var normalizedStatus))
            throw new BadRequestException($"Trạng thái '{request.Status}' không hợp lệ. Chỉ chấp nhận: online, offline, accepting_questions, away.");

        // 2. Tra Lý Lịch: Thầy Bói này có Hồ Sơ Môn Phái (Profile) không?
        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        // 3. Phép Dịch Chuyển Ký Tự.
        profile.Status = normalizedStatus;
        
        // 4. Lãnh Ấn Lưu.
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);

        return true;
    }
}

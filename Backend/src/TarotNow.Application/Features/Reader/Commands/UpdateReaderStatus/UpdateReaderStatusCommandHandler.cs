using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

// Handler cập nhật trạng thái Reader theo thao tác thủ công.
public class UpdateReaderStatusCommandHandler : IRequestHandler<UpdateReaderStatusCommand, bool>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật trạng thái reader.
    /// Luồng xử lý: nhận reader profile repository để tải profile hiện tại và lưu trạng thái sau khi chuẩn hóa.
    /// </summary>
    public UpdateReaderStatusCommandHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <summary>
    /// Xử lý command đổi trạng thái reader.
    /// Luồng xử lý: normalize status đầu vào, chặn status online thủ công, tải profile và persist trạng thái mới.
    /// </summary>
    public async Task<bool> Handle(UpdateReaderStatusCommand request, CancellationToken cancellationToken)
    {
        if (!ReaderOnlineStatus.TryNormalize(request.Status, out var normalizedStatus))
        {
            // Business rule: chỉ chấp nhận tập trạng thái reader được định nghĩa chuẩn hóa trong hệ thống.
            throw new BadRequestException(
                $"Trạng thái '{request.Status}' không hợp lệ. Chỉ chấp nhận: offline, busy.");
        }

        if (normalizedStatus == ReaderOnlineStatus.Online)
        {
            // Business rule: online được set tự động theo trạng thái kết nối realtime, không cho chỉnh tay.
            throw new BadRequestException(
                "Trạng thái 'online' được cập nhật tự động khi kết nối. Truyền 'busy' hoặc 'offline' để đổi trạng thái thủ công.");
        }

        var profile = await _readerProfileRepository.GetByUserIdAsync(
            request.UserId.ToString(),
            cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        profile.Status = normalizedStatus;
        // Đổi state hồ sơ reader sang trạng thái đã normalize để thống nhất giá trị lưu trữ.

        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        // Persist trạng thái mới để API danh sách reader và kênh realtime cùng thấy dữ liệu đồng nhất.

        return true;
    }
}

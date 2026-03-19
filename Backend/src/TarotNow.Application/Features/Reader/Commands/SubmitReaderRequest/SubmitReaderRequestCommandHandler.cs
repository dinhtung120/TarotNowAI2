/*
 * ===================================================================
 * FILE: SubmitReaderRequestCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest
 * ===================================================================
 * MỤC ĐÍCH:
 *   Ánh Xạ và Phê Duyệt Sơ Bộ Đơn Xin Trở Thành Reader.
 *   - Lỗi sẽ ném ra nếu rác rưởi (Ví dụ Banned ráng xin làm Reader).
 *   - Thành công sẽ thẩy Document mới vào MongoDB cho Admin duyệt sau.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

public class SubmitReaderRequestCommandHandler : IRequestHandler<SubmitReaderRequestCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderRequestRepository _readerRequestRepository;

    public SubmitReaderRequestCommandHandler(
        IUserRepository userRepository,
        IReaderRequestRepository readerRequestRepository)
    {
        _userRepository = userRepository;
        _readerRequestRepository = readerRequestRepository;
    }

    public async Task<bool> Handle(SubmitReaderRequestCommand request, CancellationToken cancellationToken)
    {
        // 1. Quét Căn Cước dưới PostgreSQL để xem khách này là ai.
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        // 2. Anti-Spam (Chặn Nick Khóa) — Tài khoản có tì vết thì miễn làm thầy tu tập.
        if (user.Status != UserStatus.Active)
            throw new BadRequestException("Tài khoản chưa được kích hoạt hoặc đã bị khóa.");

        // 3. Phân biệt Giai Cấp — Nếu đã lên chức Reader/Admin rồi thì còn đi Vác Đơn xin làm gì nữa.
        if (user.Role != UserRole.User)
            throw new BadRequestException("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader.");

        // 4. Anti-Spam (Chặn Multiple Request) — Đang có lá đơn chờ Duyệt thì cấm nộp thêm.
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(
            request.UserId.ToString(), cancellationToken);

        if (latestRequest != null && latestRequest.Status == ReaderApprovalStatus.Pending)
            throw new BadRequestException("Bạn đã có đơn đang chờ duyệt. Vui lòng chờ admin xử lý.");

        // 5. Mọi thứ trong sạch -> Tiến Hành đúc 1 Bản Ghi (Document) Nhét Sang Database MongoDB.
        var readerRequest = new ReaderRequestDto
        {
            UserId = request.UserId.ToString(),
            Status = ReaderApprovalStatus.Pending, // Mặc định phải ngâm ở trạng thái Chờ (Pending)
            IntroText = request.IntroText,
            ProofDocuments = request.ProofDocuments,
            CreatedAt = DateTime.UtcNow
        };

        // Lưu bản ghi vào bảng Chờ Trảm (ReaderRequests). Admin sẽ ngó vô bảng này.
        await _readerRequestRepository.AddAsync(readerRequest, cancellationToken);

        return true;
    }
}

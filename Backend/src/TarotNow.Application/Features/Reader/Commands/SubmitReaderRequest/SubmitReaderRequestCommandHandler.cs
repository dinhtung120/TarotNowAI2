using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

// Handler xử lý gửi đơn đăng ký Reader.
public class SubmitReaderRequestCommandHandler : IRequestHandler<SubmitReaderRequestCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderRequestRepository _readerRequestRepository;

    /// <summary>
    /// Khởi tạo handler gửi đơn reader.
    /// Luồng xử lý: nhận user repository để kiểm tra điều kiện tài khoản và reader request repository để kiểm tra/lưu đơn.
    /// </summary>
    public SubmitReaderRequestCommandHandler(
        IUserRepository userRepository,
        IReaderRequestRepository readerRequestRepository)
    {
        _userRepository = userRepository;
        _readerRequestRepository = readerRequestRepository;
    }

    /// <summary>
    /// Xử lý command gửi đơn reader.
    /// Luồng xử lý: xác thực điều kiện user hợp lệ, chặn đơn pending trùng, tạo đơn mới trạng thái pending và lưu persistence.
    /// </summary>
    public async Task<bool> Handle(SubmitReaderRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (user.Status != UserStatus.Active)
        {
            // Business rule: chỉ tài khoản active mới được gửi đơn để tránh luồng duyệt cho user bị khóa/chưa kích hoạt.
            throw new BadRequestException("Tài khoản chưa được kích hoạt hoặc đã bị khóa.");
        }

        if (user.Role != UserRole.User)
        {
            // Business rule: tài khoản đã là role đặc biệt thì không cần đăng ký Reader lần nữa.
            throw new BadRequestException("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader.");
        }

        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(
            request.UserId.ToString(),
            cancellationToken);

        if (latestRequest is not null && latestRequest.Status == ReaderApprovalStatus.Pending)
        {
            // Chặn gửi trùng khi vẫn còn đơn chờ duyệt để giữ luồng vận hành admin rõ ràng.
            throw new BadRequestException("Bạn đã có đơn đang chờ duyệt. Vui lòng chờ admin xử lý.");
        }

        var readerRequest = new ReaderRequestDto
        {
            UserId = request.UserId.ToString(),
            Status = ReaderApprovalStatus.Pending,
            IntroText = request.IntroText,
            ProofDocuments = request.ProofDocuments,
            CreatedAt = DateTime.UtcNow
        };
        // Tạo đơn mới luôn ở trạng thái Pending để admin review trước khi cấp quyền Reader.

        await _readerRequestRepository.AddAsync(readerRequest, cancellationToken);
        // Ghi nhận trạng thái đơn mới vào persistence để hiển thị trong màn hình theo dõi đơn.

        return true;
    }
}

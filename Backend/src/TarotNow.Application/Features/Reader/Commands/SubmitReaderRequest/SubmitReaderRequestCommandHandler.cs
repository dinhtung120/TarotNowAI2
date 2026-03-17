using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// Handler xử lý đơn xin trở thành Reader.
///
/// Business rules:
/// 1. User không được có đơn pending trước đó (tránh spam).
/// 2. User phải có role = "user" (admin/reader không cần apply).
/// 3. User phải active (không bị lock/banned).
/// 4. Tạo document trong reader_requests collection với status = "pending".
/// </summary>
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
        // 1. Lấy thông tin user từ PostgreSQL — kiểm tra tồn tại
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        // 2. Kiểm tra user phải active — không cho locked/banned user apply
        if (user.Status != UserStatus.Active)
            throw new BadRequestException("Tài khoản chưa được kích hoạt hoặc đã bị khóa.");

        // 3. Kiểm tra role — đã là reader/admin thì không cần apply
        if (user.Role != UserRole.User)
            throw new BadRequestException("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader.");

        // 4. Kiểm tra đơn pending — tránh gửi nhiều đơn cùng lúc
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(
            request.UserId.ToString(), cancellationToken);

        if (latestRequest != null && latestRequest.Status == ReaderApprovalStatus.Pending)
            throw new BadRequestException("Bạn đã có đơn đang chờ duyệt. Vui lòng chờ admin xử lý.");

        // 5. Tạo DTO mới — Repository sẽ map sang MongoDocument nội bộ
        var readerRequest = new ReaderRequestDto
        {
            UserId = request.UserId.ToString(),
            Status = ReaderApprovalStatus.Pending,
            IntroText = request.IntroText,
            ProofDocuments = request.ProofDocuments,
            CreatedAt = DateTime.UtcNow
        };

        await _readerRequestRepository.AddAsync(readerRequest, cancellationToken);

        return true;
    }
}

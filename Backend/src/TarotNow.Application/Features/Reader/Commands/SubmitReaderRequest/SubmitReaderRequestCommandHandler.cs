

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
        
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        
        if (user.Status != UserStatus.Active)
            throw new BadRequestException("Tài khoản chưa được kích hoạt hoặc đã bị khóa.");

        
        if (user.Role != UserRole.User)
            throw new BadRequestException("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader.");

        
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(
            request.UserId.ToString(), cancellationToken);

        if (latestRequest != null && latestRequest.Status == ReaderApprovalStatus.Pending)
            throw new BadRequestException("Bạn đã có đơn đang chờ duyệt. Vui lòng chờ admin xử lý.");

        
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

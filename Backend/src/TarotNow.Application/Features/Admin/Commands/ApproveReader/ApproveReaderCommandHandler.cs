

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public partial class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
{
    
    private readonly IReaderRequestRepository _readerRequestRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    public ApproveReaderCommandHandler(
        IReaderRequestRepository readerRequestRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerRequestRepository = readerRequestRepository;
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ApproveReaderCommand request, CancellationToken cancellationToken)
    {
        
        var action = request.Action.ToLowerInvariant();
        if (action != "approve" && action != "reject")
            throw new BadRequestException("Action không hợp lệ. Chỉ chấp nhận: approve, reject.");

        
        
        var readerRequest = await _readerRequestRepository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");

        
        
        if (readerRequest.Status != ReaderApprovalStatus.Pending)
            throw new BadRequestException($"Đơn này đã được xử lý ({readerRequest.Status}).");

        
        if (!Guid.TryParse(readerRequest.UserId, out var userId))
            throw new BadRequestException("Reader request chứa UserId không hợp lệ.");

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        
        if (action == "approve")
        {
            
            await HandleApproveFlowAsync(request, readerRequest, user, cancellationToken);
            return true;
        }

        
        
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        
        readerRequest.Status = ReaderApprovalStatus.Rejected;

        
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;

        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);

        return true;
    }
}

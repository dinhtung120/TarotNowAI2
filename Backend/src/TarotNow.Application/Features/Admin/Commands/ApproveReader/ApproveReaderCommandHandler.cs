

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public partial class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
{
    private const string ApproveAction = "approve";
    private const string RejectAction = "reject";
    private const string InvalidActionMessage = "Action không hợp lệ. Chỉ chấp nhận: approve, reject.";

    
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
        var action = ValidateAndNormalizeAction(request.Action);
        var readerRequest = await _readerRequestRepository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");
        EnsureRequestIsPending(readerRequest);

        var userId = ParseRequestUserId(readerRequest.UserId);

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");
        if (action == ApproveAction)
        {
            await HandleApproveFlowAsync(request, readerRequest, user, cancellationToken);
            return true;
        }

        await HandleRejectFlowAsync(request, readerRequest, user, cancellationToken);
        return true;
    }

    private static string ValidateAndNormalizeAction(string action)
    {
        var normalizedAction = action.Trim().ToLowerInvariant();
        if (normalizedAction == ApproveAction || normalizedAction == RejectAction)
        {
            return normalizedAction;
        }

        throw new BadRequestException(InvalidActionMessage);
    }

    private static void EnsureRequestIsPending(ReaderRequestDto readerRequest)
    {
        if (readerRequest.Status == ReaderApprovalStatus.Pending) return;
        throw new BadRequestException($"Đơn này đã được xử lý ({readerRequest.Status}).");
    }

    private static Guid ParseRequestUserId(string userId)
    {
        if (Guid.TryParse(userId, out var parsedUserId)) return parsedUserId;
        throw new BadRequestException("Reader request chứa UserId không hợp lệ.");
    }

    private async Task HandleRejectFlowAsync(
        ApproveReaderCommand request,
        ReaderRequestDto readerRequest,
        Domain.Entities.User user,
        CancellationToken cancellationToken)
    {
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        readerRequest.Status = ReaderApprovalStatus.Rejected;
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }
}

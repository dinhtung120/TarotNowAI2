using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

/// <summary>
/// Handler phê duyệt / từ chối đơn xin Reader.
///
/// Cross-system:
/// → MongoDB: cập nhật reader_requests status.
/// → PostgreSQL: cập nhật users.role + users.reader_status.
/// → MongoDB: tạo reader_profiles document (nếu approve).
/// </summary>
public class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
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
        // 1. Validate action
        var action = request.Action.ToLowerInvariant();
        if (action != "approve" && action != "reject")
            throw new BadRequestException("Action không hợp lệ. Chỉ chấp nhận: approve, reject.");

        // 2. Lấy reader request
        var readerRequest = await _readerRequestRepository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");

        // 3. Kiểm tra đơn phải đang pending
        if (readerRequest.Status != ReaderApprovalStatus.Pending)
            throw new BadRequestException($"Đơn này đã được xử lý ({readerRequest.Status}).");

        // 4. Lấy user từ PostgreSQL
        if (!Guid.TryParse(readerRequest.UserId, out var userId))
            throw new BadRequestException("Reader request chứa UserId không hợp lệ.");

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (action == "approve")
        {
            await HandleApproveFlowAsync(request, readerRequest, user, cancellationToken);
            return true;
        }

        // === REJECT FLOW ===
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);
        readerRequest.Status = ReaderApprovalStatus.Rejected;

        // 7. Cập nhật audit trail
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;

        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);

        return true;
    }

    private async Task HandleApproveFlowAsync(
        ApproveReaderCommand request,
        ReaderRequestDto readerRequest,
        Domain.Entities.User user,
        CancellationToken cancellationToken)
    {
        var originalRole = user.Role;
        var originalReaderStatus = user.ReaderStatus;
        var profileCreated = false;

        try
        {
            // Step 1: Promote user trong PostgreSQL
            user.ApproveAsReader();
            await _userRepository.UpdateAsync(user, cancellationToken);

            // Step 2: Tạo reader profile nếu chưa có
            var existingProfile = await _readerProfileRepository.GetByUserIdAsync(
                readerRequest.UserId, cancellationToken);

            if (existingProfile == null)
            {
                var profile = new ReaderProfileDto
                {
                    UserId = readerRequest.UserId,
                    Status = ReaderOnlineStatus.Offline,
                    DiamondPerQuestion = 5,
                    BioVi = readerRequest.IntroText,
                    BioEn = string.Empty,
                    BioZh = string.Empty,
                    DisplayName = user.DisplayName,
                    AvatarUrl = user.AvatarUrl,
                    CreatedAt = DateTime.UtcNow
                };

                await _readerProfileRepository.AddAsync(profile, cancellationToken);
                profileCreated = true;
            }

            // Step 3: Đóng đơn request
            readerRequest.Status = ReaderApprovalStatus.Approved;
            readerRequest.AdminNote = request.AdminNote;
            readerRequest.ReviewedBy = request.AdminId.ToString();
            readerRequest.ReviewedAt = DateTime.UtcNow;
            await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            await CompensateApproveFailureAsync(
                user,
                originalRole,
                originalReaderStatus,
                readerRequest.UserId,
                profileCreated,
                cancellationToken);

            throw new InvalidOperationException("Approve reader failed and was rolled back.", ex);
        }
    }

    private async Task CompensateApproveFailureAsync(
        Domain.Entities.User user,
        string originalRole,
        string originalReaderStatus,
        string userId,
        bool profileCreated,
        CancellationToken cancellationToken)
    {
        List<Exception>? compensationErrors = null;

        try
        {
            user.RestoreRoleAndReaderStatus(originalRole, originalReaderStatus);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            compensationErrors ??= new List<Exception>();
            compensationErrors.Add(ex);
        }

        if (profileCreated)
        {
            try
            {
                await _readerProfileRepository.DeleteByUserIdAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                compensationErrors ??= new List<Exception>();
                compensationErrors.Add(ex);
            }
        }

        if (compensationErrors is { Count: > 0 })
            throw new AggregateException("Compensation failed after approve reader error.", compensationErrors);
    }
}

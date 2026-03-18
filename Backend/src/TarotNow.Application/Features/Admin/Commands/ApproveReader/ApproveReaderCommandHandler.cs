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
            // === APPROVE FLOW ===
            // 5a. Cập nhật User entity
            user.ApproveAsReader();
            await _userRepository.UpdateAsync(user, cancellationToken);

            // 5b. Tạo reader_profiles (idempotent)
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
            }

            readerRequest.Status = ReaderApprovalStatus.Approved;
        }
        else
        {
            // === REJECT FLOW ===
            user.RejectReaderRequest();
            await _userRepository.UpdateAsync(user, cancellationToken);
            readerRequest.Status = ReaderApprovalStatus.Rejected;
        }

        // 7. Cập nhật audit trail
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;

        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);

        return true;
    }
}

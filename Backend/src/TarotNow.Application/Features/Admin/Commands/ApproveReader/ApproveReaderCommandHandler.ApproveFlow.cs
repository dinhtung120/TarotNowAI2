using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public partial class ApproveReaderCommandHandler
{
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
            user.ApproveAsReader();
            await _userRepository.UpdateAsync(user, cancellationToken);
            profileCreated = await EnsureReaderProfileAsync(readerRequest, user, cancellationToken);
            await UpdateReaderRequestAsync(request, readerRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            await CompensateApproveFailureAsync(new ApproveCompensationContext(
                user,
                originalRole,
                originalReaderStatus,
                readerRequest.UserId,
                profileCreated,
                cancellationToken));

            throw new InvalidOperationException("Approve reader failed and was rolled back.", ex);
        }
    }

    private async Task<bool> EnsureReaderProfileAsync(
        ReaderRequestDto readerRequest,
        Domain.Entities.User user,
        CancellationToken cancellationToken)
    {
        var existingProfile = await _readerProfileRepository.GetByUserIdAsync(
            readerRequest.UserId,
            cancellationToken);
        if (existingProfile != null)
        {
            return false;
        }

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
        return true;
    }

    private async Task UpdateReaderRequestAsync(
        ApproveReaderCommand request,
        ReaderRequestDto readerRequest,
        CancellationToken cancellationToken)
    {
        readerRequest.Status = ReaderApprovalStatus.Approved;
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }
}

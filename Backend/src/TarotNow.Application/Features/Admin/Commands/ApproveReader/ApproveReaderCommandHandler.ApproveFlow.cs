using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public partial class ApproveReaderCommandHandler
{
    /// <summary>
    /// Thực thi nhánh approve reader kèm cơ chế bù trừ khi xảy ra lỗi giữa chừng.
    /// Luồng xử lý: đổi trạng thái user, tạo profile nếu thiếu, cập nhật request; nếu lỗi thì rollback.
    /// </summary>
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
            // Bước 1: nâng quyền user thành reader trước khi tạo profile.
            user.ApproveAsReader();
            await _userRepository.UpdateAsync(user, cancellationToken);
            // Bước 2: đảm bảo reader có profile để các luồng directory/booking hoạt động.
            profileCreated = await EnsureReaderProfileAsync(readerRequest, user, cancellationToken);
            // Bước 3: chốt trạng thái request sau khi các bước trước thành công.
            await UpdateReaderRequestAsync(request, readerRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            // Nhánh lỗi: rollback các thay đổi đã thực hiện để tránh trạng thái nửa vời.
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

    /// <summary>
    /// Đảm bảo user có hồ sơ reader; chỉ tạo mới khi chưa tồn tại.
    /// Luồng xử lý: tra profile theo user id, nếu có thì bỏ qua; nếu chưa có thì tạo profile mặc định.
    /// </summary>
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
            // Edge case đã có profile từ trước: không tạo mới để tránh trùng dữ liệu.
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

        // Tạo profile mới cho reader vừa được duyệt.
        await _readerProfileRepository.AddAsync(profile, cancellationToken);
        return true;
    }

    /// <summary>
    /// Cập nhật reader request sang trạng thái approved cùng metadata reviewer.
    /// Luồng xử lý: gán trạng thái/note/reviewer/review time rồi persist vào repository.
    /// </summary>
    private async Task UpdateReaderRequestAsync(
        ApproveReaderCommand request,
        ReaderRequestDto readerRequest,
        CancellationToken cancellationToken)
    {
        // Đổi state request sau khi approve flow hoàn tất thành công.
        readerRequest.Status = ReaderApprovalStatus.Approved;
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }
}

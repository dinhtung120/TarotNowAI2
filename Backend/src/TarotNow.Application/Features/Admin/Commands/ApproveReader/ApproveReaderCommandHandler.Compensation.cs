namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public partial class ApproveReaderCommandHandler
{
    // Ngữ cảnh rollback cho approve flow khi xảy ra lỗi giữa các bước.
    private readonly record struct ApproveCompensationContext(
        Domain.Entities.User User,
        string OriginalRole,
        string OriginalReaderStatus,
        string UserId,
        bool ProfileCreated,
        CancellationToken CancellationToken);

    /// <summary>
    /// Thực hiện bù trừ khi approve flow thất bại để hệ thống quay về trạng thái nhất quán.
    /// Luồng xử lý: rollback user role/status, rollback profile nếu vừa tạo, gom lỗi rollback nếu có.
    /// </summary>
    private async Task CompensateApproveFailureAsync(
        ApproveCompensationContext context)
    {
        List<Exception>? compensationErrors = null;

        try
        {
            // Hoàn tác thay đổi role/status user về trạng thái ban đầu.
            context.User.RestoreRoleAndReaderStatus(context.OriginalRole, context.OriginalReaderStatus);
            await _userRepository.UpdateAsync(context.User, context.CancellationToken);
        }
        catch (Exception ex)
        {
            // Ghi nhận lỗi rollback user để tổng hợp và ném ra cuối cùng.
            compensationErrors ??= [];
            compensationErrors.Add(ex);
        }

        if (context.ProfileCreated)
        {
            try
            {
                // Chỉ xóa profile khi profile được tạo trong phiên approve hiện tại.
                await _readerProfileRepository.DeleteByUserIdAsync(context.UserId, context.CancellationToken);
            }
            catch (Exception ex)
            {
                // Gom lỗi rollback profile để không nuốt lỗi bù trừ.
                compensationErrors ??= [];
                compensationErrors.Add(ex);
            }
        }

        if (compensationErrors is { Count: > 0 })
        {
            // Ném aggregate để caller biết rollback chưa hoàn tất toàn phần.
            throw new AggregateException("Compensation failed after approve reader error.", compensationErrors);
        }
    }
}

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

public partial class ApproveReaderCommandHandler
{
    private readonly record struct ApproveCompensationContext(
        Domain.Entities.User User,
        string OriginalRole,
        string OriginalReaderStatus,
        string UserId,
        bool ProfileCreated,
        CancellationToken CancellationToken);

    private async Task CompensateApproveFailureAsync(
        ApproveCompensationContext context)
    {
        List<Exception>? compensationErrors = null;

        try
        {
            context.User.RestoreRoleAndReaderStatus(context.OriginalRole, context.OriginalReaderStatus);
            await _userRepository.UpdateAsync(context.User, context.CancellationToken);
        }
        catch (Exception ex)
        {
            compensationErrors ??= [];
            compensationErrors.Add(ex);
        }

        if (context.ProfileCreated)
        {
            try
            {
                await _readerProfileRepository.DeleteByUserIdAsync(context.UserId, context.CancellationToken);
            }
            catch (Exception ex)
            {
                compensationErrors ??= [];
                compensationErrors.Add(ex);
            }
        }

        if (compensationErrors is { Count: > 0 })
        {
            throw new AggregateException("Compensation failed after approve reader error.", compensationErrors);
        }
    }
}

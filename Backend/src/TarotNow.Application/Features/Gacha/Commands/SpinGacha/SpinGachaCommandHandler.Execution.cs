namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    private async Task<SpinBatchState> ExecuteSpinBatchAsync(
        SpinGachaCommand request,
        SpinExecutionContext context,
        CancellationToken cancellationToken)
    {
        var state = new SpinBatchState(context.CurrentPity);

        for (var spinIndex = 0; spinIndex < request.Count; spinIndex++)
        {
            await ExecuteSingleSpinAsync(request, context, state, spinIndex, cancellationToken);
        }

        return state;
    }

    private async Task ExecuteSingleSpinAsync(
        SpinGachaCommand request,
        SpinExecutionContext context,
        SpinBatchState state,
        int spinIndex,
        CancellationToken cancellationToken)
    {
        var selection = SelectSpinSelection(context, state.CurrentPity);

        state.CurrentPity = selection.NextPityCount;
        state.AnyPityTriggered = state.AnyPityTriggered || selection.WasHardPity;

        await ApplyRewardAsync(request.UserId, selection.Item, state, cancellationToken);

        var rewardLog = BuildRewardLog(request, context.Banner, selection, spinIndex);
        await _gachaRepository.LogRewardAsync(rewardLog, cancellationToken);

        await InsertAnalyticsLogSafeAsync(request, context.Banner, selection, rewardLog.CreatedAt, cancellationToken);
        state.Items.Add(ToSpinItemResult(selection.Item));
    }
}

namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public partial class SpinGachaCommandHandler
{
    /// <summary>
    /// Thực thi toàn bộ lượt quay trong một request.
    /// Luồng xử lý: khởi tạo batch state từ pity hiện tại, lặp theo Count và thực thi từng lượt quay.
    /// </summary>
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

    /// <summary>
    /// Thực thi một lượt quay đơn lẻ.
    /// Luồng xử lý: chọn item theo pity/weight, cập nhật state pity, áp phần thưởng, ghi reward log và analytics log, rồi thêm item vào kết quả.
    /// </summary>
    private async Task ExecuteSingleSpinAsync(
        SpinGachaCommand request,
        SpinExecutionContext context,
        SpinBatchState state,
        int spinIndex,
        CancellationToken cancellationToken)
    {
        var selection = SelectSpinSelection(context, state.CurrentPity);

        // Cập nhật trạng thái pity cho lượt kế tiếp trong cùng batch.
        state.CurrentPity = selection.NextPityCount;
        state.AnyPityTriggered = state.AnyPityTriggered || selection.WasHardPity;

        await ApplyRewardAsync(request.UserId, selection.Item, state, cancellationToken);

        var rewardLog = BuildRewardLog(request, context.Banner, selection, spinIndex);
        await _gachaRepository.LogRewardAsync(rewardLog, cancellationToken);

        // Analytics log chỉ phục vụ thống kê, lỗi ở đây không làm fail giao dịch quay.
        await InsertAnalyticsLogSafeAsync(request, context.Banner, selection, rewardLog.CreatedAt, cancellationToken);
        state.Items.Add(ToSpinItemResult(selection.Item));
    }
}

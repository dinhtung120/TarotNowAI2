using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

namespace TarotNow.Api.Controllers;

public partial class AiController
{
    // Ngữ cảnh tối thiểu để finalize AI stream mà không phụ thuộc trực tiếp vào request ban đầu.
    private readonly record struct StreamCompletionDispatch(
        Guid AiRequestId,
        Guid UserId,
        string? FollowUpQuestion,
        StreamExecutionState State,
        string FinalStatus,
        string? ErrorMessage,
        bool IsClientDisconnect,
        CancellationToken CancellationToken);

    /// <summary>
    /// Gửi command hoàn tất stream để ghi nhận trạng thái cuối và telemetry.
    /// Luồng xử lý: tính latency từ mốc token đầu tiên, gom dữ liệu state, dispatch command hoàn tất.
    /// </summary>
    /// <param name="completion">Ngữ cảnh finalize được gom từ runtime stream.</param>
    private async Task CompleteStreamAsync(StreamCompletionDispatch completion)
    {
        // Nếu chưa nhận token nào thì giữ latency = 0 để phản ánh đúng nhánh failed-before-first-token.
        var latencyMs = completion.State.FirstTokenAt.HasValue
            ? (int)(DateTimeOffset.UtcNow - completion.State.FirstTokenAt.Value).TotalMilliseconds
            : 0;

        // Đẩy đầy đủ snapshot runtime sang command để lưu nhất quán một lần ở tầng ứng dụng.
        await _mediator.Send(new CompleteAiStreamCommand
        {
            AiRequestId = completion.AiRequestId,
            UserId = completion.UserId,
            FinalStatus = completion.FinalStatus,
            ErrorMessage = completion.ErrorMessage,
            IsClientDisconnect = completion.IsClientDisconnect,
            FirstTokenAt = completion.State.FirstTokenAt,
            OutputTokens = completion.State.OutputTokens,
            LatencyMs = latencyMs,
            FullResponse = completion.State.FullResponseBuilder.ToString(),
            FollowupQuestion = completion.FollowUpQuestion
        }, completion.CancellationToken);
    }
}

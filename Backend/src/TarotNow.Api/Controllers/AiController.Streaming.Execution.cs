namespace TarotNow.Api.Controllers;

public partial class AiController
{
    /// <summary>
    /// Ghi tuần tự các chunk từ upstream stream ra SSE response.
    /// Luồng xử lý: cập nhật state runtime, sanitize dữ liệu, ghi event và flush theo từng chunk.
    /// </summary>
    /// <param name="stream">Luồng chunk từ nhà cung cấp AI.</param>
    /// <param name="state">State runtime dùng cho finalize và telemetry.</param>
    /// <param name="cancellationToken">Token hủy request stream.</param>
    private async Task WriteStreamAsync(
        IAsyncEnumerable<string> stream,
        StreamExecutionState state,
        CancellationToken cancellationToken)
    {
        await foreach (var chunk in stream.WithCancellation(cancellationToken))
        {
            if (state.FirstTokenAt == null)
            {
                // Chỉ ghi mốc token đầu tiên một lần để tính latency first-token chính xác.
                state.FirstTokenAt = DateTimeOffset.UtcNow;
            }

            // Cập nhật state tích lũy cho bước finalize trạng thái và lưu full response.
            state.FullResponseBuilder.Append(chunk);
            state.OutputTokens++;

            // Escape xuống dòng để SSE parser phía client không tách sai frame dữ liệu.
            var sanitizedChunk = chunk.Replace("\n", "\\n");
            await WriteServerEventAsync(sanitizedChunk, cancellationToken);
            // Flush từng chunk để giảm độ trễ hiển thị realtime ở phía client.
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gửi sự kiện kết thúc stream theo chuẩn giao tiếp với client.
    /// </summary>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Tác vụ ghi sự kiện DONE.</returns>
    private Task WriteDoneEventAsync(CancellationToken cancellationToken)
    {
        return WriteServerEventAsync("[DONE]", cancellationToken);
    }

    /// <summary>
    /// Ghi một frame SSE dạng <c>data:</c> vào response hiện tại.
    /// </summary>
    /// <param name="payload">Nội dung payload của SSE frame.</param>
    /// <param name="cancellationToken">Token hủy request.</param>
    /// <returns>Tác vụ ghi dữ liệu ra response.</returns>
    private Task WriteServerEventAsync(string payload, CancellationToken cancellationToken)
    {
        return Response.WriteAsync($"data: {payload}\n\n", cancellationToken);
    }

    // State runtime của một phiên stream để phục vụ finalize và ghi nhận telemetry.
    private sealed class StreamExecutionState
    {
        // Thời điểm nhận token đầu tiên để tính latency.
        public DateTimeOffset? FirstTokenAt { get; set; }

        // Tổng số token/chunk đầu ra đã gửi xuống client.
        public int OutputTokens { get; set; }

        // Bộ đệm lưu full response để persist vào bản ghi hoàn tất.
        public System.Text.StringBuilder FullResponseBuilder { get; } = new();
    }
}

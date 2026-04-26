using System;
using System.Collections.Generic;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

// DTO kết quả stream reading trả về cho caller.
public class StreamReadingResult
{
    // Luồng token nội dung AI trả dần theo thời gian thực.
    public required IAsyncEnumerable<AiStreamChunk> Stream { get; init; }

    // Định danh AI request dùng để chốt completion/billing ở bước sau.
    public required Guid AiRequestId { get; init; }

    // Token input ước lượng tại thời điểm gửi request lên provider.
    public int EstimatedInputTokens { get; init; }
}

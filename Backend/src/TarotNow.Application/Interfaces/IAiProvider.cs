/*
 * ===================================================================
 * FILE: IAiProvider.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Giao Diện (Interface) Định Nghĩa Luật Lệ Phải Tuân Theo Cho Bất Cứ AI Nào.
 *   (Dù xài ChatGPT, Gemini hay Claude thì đều phải đẻ ra hàm giống vầy).
 * ===================================================================
 */

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Hợp đồng trừu tượng (Contract) cho bất kỳ ông AI nào muốn đầu quân làm Tarot Reader.
/// Bắt buộc phải có Mánh Khóe Nhả Chữ Liên Tục (Streaming) để khách khỏi sốt ruột chờ đợi.
/// </summary>
public interface IAiProvider
{
    /// <summary>
    /// Gửi Lá Đơn Xin Bói Toán tới Vị Thần AI (LLM) và Bốc Hứng Text Chạy Rơi Lả Tả Về.
    /// Kèm Theo Nút Panic (CancellationToken) Để Huỷ Gọi Nếu Khách Bấm Xoay Màn Hình Văng Ra Ngoài.
    /// </summary>
    /// <param name="systemPrompt">Hướng dẫn Nhập Vai Thầy Cúng (Vd: You are a mystic Tarot reader).</param>
    /// <param name="userPrompt">Hình Ảnh Lá Bài Bốc Được Kèm Lời Than Vãn Của Khách Khứa.</param>
    /// <param name="cancellationToken">Lệnh Cắt Cầu Dao Đứt Dây Cuộc Gọi Nếu Cần (CancellationToken).</param>
    /// <returns>Một đường ống IAsyncEnumerable cứ lâu lâu lọt ra vài chữ (Chunk stream).</returns>
    IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken);
    
    /// <summary>
    /// Niêm Yết Rõ Tên Thầy (VD: OpenAI hay Google) để tính tiền Auditing sau này.
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Đang xài Model phiên bản mấy? (GPT-4o hay Gemini-1.5-Pro).
    /// </summary>
    string ModelName { get; }

    /// <summary>
    /// Ghi telemetry cho request AI; implementation có thể no-op nếu không hỗ trợ.
    /// </summary>
    Task LogRequestAsync(
        Guid userId,
        string? sessionId,
        string? requestId,
        int inputTokens,
        int outputTokens,
        int latencyMs,
        string status,
        string? errorCode = null);
}

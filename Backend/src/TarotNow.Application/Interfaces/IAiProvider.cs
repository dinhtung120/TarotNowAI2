namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface trừu tượng hóa các gọi hàm tới Server AI (OpenAI, Gemini,...).
/// Sử dụng IAsyncEnumerable để phục vụ chức năng Streaming (Server-Sent Events).
/// </summary>
public interface IAiProvider
{
    /// <summary>
    /// Gửi truy vấn tới AI Provider và nhận kết quả trả về liên tục (Streaming).
    /// </summary>
    /// <param name="systemPrompt">Hướng dẫn cho hệ thống (Vd: You are a mystic Tarot reader).</param>
    /// <param name="userPrompt">Nội dung câu hỏi và các lá bài của người dùng.</param>
    /// <param name="cancellationToken">Token để hủy luồng Stream nếu Request bị ngắt (Client tắt Browser).</param>
    /// <returns>Một luồng (Stream) chứa text sinh ra theo từng Chunk.</returns>
    IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken);
    
    /// <summary>
    /// Thuộc tính xác định Version Prompt / Policy đang sử dụng.
    /// </summary>
    string ProviderName { get; }
    string ModelName { get; }
}

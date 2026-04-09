using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Ai;

// Provider tích hợp OpenAI cho các tác vụ sinh nội dung AI.
public partial class OpenAiProvider : IAiProvider
{
    // HttpClient được cấu hình BaseAddress/headers để tái sử dụng kết nối ổn định.
    private readonly HttpClient _httpClient;
    // Repository lưu telemetry request để theo dõi chi phí và lỗi provider.
    private readonly IAiProviderLogRepository _logRepo;
    // Logger phục vụ quan sát retry/lỗi ở tầng tích hợp.
    private readonly ILogger<OpenAiProvider> _logger;
    // Model mặc định hoặc model cấu hình từ môi trường.
    private readonly string _modelName;
    // Số lần retry tối đa cho lỗi tạm thời từ mạng/provider.
    private readonly int _maxRetries;

    // Tên provider trả về cho tầng ứng dụng để định danh nguồn AI.
    public string ProviderName => "OpenAI";

    // Tên model đang được dùng để gọi API.
    public string ModelName => _modelName;

    /// <summary>
    /// Khởi tạo provider OpenAI với cấu hình runtime.
    /// Luồng này chuẩn hóa model, retry, timeout và header xác thực trước khi gửi request.
    /// </summary>
    public OpenAiProvider(
        HttpClient httpClient,
        IOptions<AiProviderOptions> options,
        IAiProviderLogRepository logRepo,
        ILogger<OpenAiProvider> logger)
    {
        _httpClient = httpClient;
        _logRepo = logRepo;
        _logger = logger;

        // Đọc cấu hình provider và fail-fast nếu thiếu API key bắt buộc.
        var providerOptions = options.Value;
        var apiKey = providerOptions.ApiKey
                     ?? throw new ArgumentNullException("AiProvider:ApiKey is missing.");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("AiProvider:ApiKey is missing.");
        }

        var modelName = providerOptions.Model?.Trim();
        if (string.IsNullOrWhiteSpace(modelName))
        {
            throw new InvalidOperationException("AiProvider:Model is missing.");
        }
        _modelName = modelName;

        // Giới hạn retry ở giá trị không âm để tránh cấu hình âm gây hành vi bất định.
        _maxRetries = providerOptions.MaxRetries >= 0 ? providerOptions.MaxRetries : 2;

        // Chuẩn hóa timeout/baseUrl giúp client luôn có thông số chạy tối thiểu hợp lệ.
        var timeoutSeconds = providerOptions.TimeoutSeconds > 0 ? providerOptions.TimeoutSeconds : 30;
        var baseUrl = providerOptions.BaseUrl?.Trim();
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("AiProvider:BaseUrl is missing.");
        }

        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var parsedBaseUrl))
        {
            throw new InvalidOperationException("AiProvider:BaseUrl must be a valid absolute URI.");
        }

        // Cấu hình HttpClient một lần ở constructor để các request sau tái sử dụng nhất quán.
        _httpClient.BaseAddress = parsedBaseUrl;
        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }
}

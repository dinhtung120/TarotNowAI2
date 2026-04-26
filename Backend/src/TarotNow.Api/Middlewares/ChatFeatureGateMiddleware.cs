using Microsoft.FeatureManagement;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Middlewares;

// Chặn truy cập luồng chat khi feature flag chưa bật để rollout an toàn theo môi trường.
public class ChatFeatureGateMiddleware
{
    // Prefix endpoint conversation cần đi qua cơ chế bật/tắt chat.
    private static readonly PathString ConversationsPrefix = "/" + ApiRoutes.Conversations;
    // Prefix endpoint hub realtime chat cũng phải chịu cùng feature gate.
    private static readonly PathString ChatHubPrefix = "/" + ApiRoutes.ChatHub;

    private readonly RequestDelegate _next;

    /// <summary>
    /// Khởi tạo middleware chặn chat theo feature flag.
    /// Luồng xử lý: nhận delegate kế tiếp để có thể chuyển tiếp request khi hợp lệ.
    /// </summary>
    public ChatFeatureGateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Chặn hoặc cho phép request chat dựa trên trạng thái tính năng ChatV2.
    /// Luồng xử lý: 1) bỏ qua endpoint không liên quan chat, 2) kiểm tra flag, 3) trả 404 khi tắt.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IFeatureManagerSnapshot featureManager)
    {
        if (IsChatPath(context.Request.Path) == false)
        {
            // Nhánh non-chat: không can thiệp để tránh ảnh hưởng endpoint khác.
            await _next(context);
            return;
        }

        var chatEnabled = await featureManager.IsEnabledAsync(FeatureFlags.ChatV2Enabled);
        if (chatEnabled)
        {
            // Nhánh chat đã bật: cho phép đi tiếp pipeline bình thường.
            await _next(context);
            return;
        }

        // Nhánh chat bị tắt: trả 404 để ẩn endpoint tạm thời khỏi client.
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = "Chat service is temporarily unavailable.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            Extensions =
            {
                ["errorCode"] = "CHAT_V2_DISABLED"
            }
        });
    }

    /// <summary>
    /// Xác định request hiện tại có thuộc nhóm endpoint chat cần áp dụng feature gate hay không.
    /// Luồng xử lý: so khớp theo prefix conversation hoặc hub chat.
    /// </summary>
    private static bool IsChatPath(PathString path)
    {
        return path.StartsWithSegments(ConversationsPrefix)
            || path.StartsWithSegments(ChatHubPrefix);
    }
}

using Microsoft.FeatureManagement;

namespace TarotNow.Api.Middlewares;

public class ChatFeatureGateMiddleware
{
    private static readonly PathString ConversationsPrefix = "/" + ApiRoutes.Conversations;
    private static readonly PathString ChatHubPrefix = "/" + ApiRoutes.ChatHub;

    private readonly RequestDelegate _next;

    public ChatFeatureGateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IFeatureManagerSnapshot featureManager)
    {
        if (IsChatPath(context.Request.Path) == false)
        {
            await _next(context);
            return;
        }

        var chatEnabled = await featureManager.IsEnabledAsync(FeatureFlags.ChatV2Enabled);
        if (chatEnabled)
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "CHAT_V2_DISABLED",
            message = "Chat service is temporarily unavailable."
        });
    }

    private static bool IsChatPath(PathString path)
    {
        return path.StartsWithSegments(ConversationsPrefix)
            || path.StartsWithSegments(ChatHubPrefix);
    }
}

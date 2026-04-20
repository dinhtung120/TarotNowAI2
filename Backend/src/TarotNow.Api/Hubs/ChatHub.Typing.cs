using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.PublishTypingState;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Broadcast trạng thái bắt đầu gõ trong conversation.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh typing event.</param>
    public async Task TypingStarted(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            // Chặn typing event từ kết nối không xác thực.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        await PublishTypingStateAsync(conversationId, userGuid, isTyping: true);
    }

    /// <summary>
    /// Broadcast trạng thái dừng gõ trong conversation.
    /// </summary>
    /// <param name="conversationId">Id conversation phát sinh typing event.</param>
    public async Task TypingStopped(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            // Chặn typing event từ kết nối không xác thực.
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        await PublishTypingStateAsync(conversationId, userGuid, isTyping: false);
    }

    private async Task PublishTypingStateAsync(string conversationId, Guid userGuid, bool isTyping)
    {
        try
        {
            await _mediator.Send(new PublishTypingStateCommand
            {
                ConversationId = conversationId,
                UserId = userGuid,
                IsTyping = isTyping
            });
        }
        catch (BadRequestException ex)
        {
            await SendClientErrorAsync(ex.Message);
        }
        catch (NotFoundException ex)
        {
            await SendClientErrorAsync(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "[ChatHub] Publish typing state failed. ConversationId={ConversationId}, UserId={UserId}, IsTyping={IsTyping}",
                conversationId,
                userGuid,
                isTyping);
            await SendClientErrorAsync("Unable to publish typing state right now.");
        }
    }
}

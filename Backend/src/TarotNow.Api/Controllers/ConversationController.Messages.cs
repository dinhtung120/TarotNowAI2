using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.QueueChatModeration;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Chat.Queries.ListMessages;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
        [HttpGet("{id}/messages")]
    public async Task<IActionResult> Messages(
        string id,
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 50)
    {
        if (TryGetUserId(out var userId) == false)
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new ListMessagesQuery
        {
            ConversationId = id,
            RequesterId = userId,
            Cursor = cursor,
            Limit = limit
        });

        return Ok(result);
    }

        [HttpPost("{id}/messages")]
    public async Task<IActionResult> SendMessage(string id, [FromBody] ConversationSendMessageBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new SendMessageCommand
        {
            ConversationId = id,
            SenderId = userId,
            Type = string.IsNullOrWhiteSpace(body.Type) ? "text" : body.Type.Trim(),
            Content = body.Content,
            MediaPayload = body.MediaPayload
        });

        await TryBroadcastMessageCreatedAsync(id, result);
        await TryBroadcastConversationUpdatedAsync(id, "message_created");
        await TryQueueModerationAsync(result);
        return Ok(result);
    }

    private async Task TryBroadcastMessageCreatedAsync(string conversationId, ChatMessageDto message)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        try
        {
            await ChatHubContext.Clients.Group(ConversationGroup(conversationId)).SendAsync("message.created", message);
        }
        catch
        {
            
        }
    }

    private async Task TryQueueModerationAsync(ChatMessageDto message)
    {
        try
        {
            await Mediator.Send(new QueueChatModerationCommand
            {
                Payload = new ChatModerationPayload
                {
                    MessageId = message.Id,
                    ConversationId = message.ConversationId,
                    SenderId = message.SenderId,
                    Type = message.Type,
                    Content = message.Content,
                    CreatedAt = message.CreatedAt
                }
            });
        }
        catch
        {
            
        }
    }
}

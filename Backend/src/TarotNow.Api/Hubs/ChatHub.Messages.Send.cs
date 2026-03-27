using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public async Task SendMessage(string conversationId, string content, string type = "text")
    {
        if (!TryGetUserGuid(out var userGuid))
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        await SendMessageCoreAsync(conversationId, content, type, userGuid);
    }

    private async Task SendMessageCoreAsync(
        string conversationId,
        string content,
        string type,
        Guid userGuid)
    {
        try
        {
            var command = BuildSendMessageCommand(conversationId, content, type, userGuid);
            TryAttachSpecialPayload(command, content);

            var message = await _mediator.Send(command);
            var groupKey = ConversationGroup(conversationId);

            await Clients.Group(groupKey).SendAsync("message.created", message);
            await BroadcastConversationUpdatedToParticipantsAsync(
                conversationId,
                "message_created",
                message.CreatedAt);

            await TryQueueModerationAsync(message);
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
                "[ChatHub] SendMessage failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await SendClientErrorAsync("Unable to send message. Please try again.");
        }
    }

    private static SendMessageCommand BuildSendMessageCommand(
        string conversationId,
        string content,
        string type,
        Guid userGuid)
    {
        return new SendMessageCommand
        {
            ConversationId = conversationId,
            SenderId = userGuid,
            Type = type,
            Content = content
        };
    }
}

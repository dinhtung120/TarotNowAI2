using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public async Task MarkRead(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        try
        {
            await MarkReadCoreAsync(conversationId, userGuid);
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
                "[ChatHub] MarkRead failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await SendClientErrorAsync("Unable to mark messages as read. Please try again.");
        }
    }

    private async Task MarkReadCoreAsync(string conversationId, Guid userGuid)
    {
        await _mediator.Send(new MarkMessagesReadCommand
        {
            ConversationId = conversationId,
            ReaderId = userGuid
        });

        var payload = new
        {
            userId = userGuid,
            conversationId,
            readAt = DateTime.UtcNow
        };

        var groupKey = ConversationGroup(conversationId);
        await Clients.Group(groupKey).SendAsync("message.read", payload);
        await BroadcastConversationUpdatedToParticipantsAsync(
            conversationId,
            "message_read",
            DateTime.UtcNow);
    }
}

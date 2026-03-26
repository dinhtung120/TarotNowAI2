using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public async Task MarkRead(string conversationId)
    {
        if (!TryGetUserGuid(out var userGuid))
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        try
        {
            await _mediator.Send(new MarkMessagesReadCommand
            {
                ConversationId = conversationId,
                ReaderId = userGuid
            });

            await Clients.Group(conversationId).SendAsync("MessagesRead", new
            {
                userId = userGuid,
                conversationId,
                readAt = DateTime.UtcNow
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
                "[ChatHub] MarkRead failed. ConversationId: {ConversationId}, UserId: {UserId}",
                conversationId,
                userGuid);
            await SendClientErrorAsync("Unable to mark messages as read. Please try again.");
        }
    }
}

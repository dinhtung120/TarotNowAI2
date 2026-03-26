using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common;
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
            if (string.Equals(type, "payment_offer", StringComparison.Ordinal))
            {
                TryAttachPaymentPayload(command, content);
            }

            var message = await _mediator.Send(command);
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", message);
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

    private void TryAttachPaymentPayload(SendMessageCommand command, string content)
    {
        try
        {
            command.PaymentPayload = System.Text.Json.JsonSerializer.Deserialize<PaymentPayloadDto>(
                content,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            command.Content = !string.IsNullOrWhiteSpace(command.PaymentPayload?.Description)
                ? command.PaymentPayload.Description
                : "Đề xuất Thanh toán Dịch vụ";
        }
        catch (System.Text.Json.JsonException ex)
        {
            _logger.LogWarning(ex, "[ChatHub] Lỗi giải mã Payment Payload.");
        }
    }
}

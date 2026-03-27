using System.Text.Json;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    private void TryAttachSpecialPayload(SendMessageCommand command, string content)
    {
        if (string.Equals(command.Type, "payment_offer", StringComparison.Ordinal))
        {
            TryAttachPaymentPayload(command, content);
            return;
        }

        if (string.Equals(command.Type, "image", StringComparison.Ordinal)
            || string.Equals(command.Type, "voice", StringComparison.Ordinal))
        {
            TryAttachMediaPayload(command, content);
        }
    }

    private void TryAttachPaymentPayload(SendMessageCommand command, string content)
    {
        try
        {
            command.PaymentPayload = JsonSerializer.Deserialize<PaymentPayloadDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            command.Content = !string.IsNullOrWhiteSpace(command.PaymentPayload?.Description)
                ? command.PaymentPayload.Description
                : "Đề xuất Thanh toán Dịch vụ";
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "[ChatHub] Lỗi giải mã Payment Payload.");
        }
    }

    private void TryAttachMediaPayload(SendMessageCommand command, string content)
    {
        try
        {
            command.MediaPayload = JsonSerializer.Deserialize<MediaPayloadDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            command.Content = !string.IsNullOrWhiteSpace(command.MediaPayload?.Description)
                ? command.MediaPayload.Description
                : string.Equals(command.Type, "voice", StringComparison.OrdinalIgnoreCase)
                    ? "[voice]"
                    : "[image]";
        }
        catch (JsonException)
        {
            command.MediaPayload = new MediaPayloadDto { Url = content, ProcessingStatus = "fallback_unvalidated" };
            command.Content = string.Equals(command.Type, "voice", StringComparison.OrdinalIgnoreCase)
                ? "[voice]"
                : "[image]";
        }
    }
}

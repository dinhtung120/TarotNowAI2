using System.Text.Json;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    /// <summary>
    /// Gắn payload đặc biệt theo loại message.
    /// Luồng xử lý: tách nhánh payment_offer và media type để parse payload phù hợp.
    /// </summary>
    /// <param name="command">Command gửi message cần bổ sung payload.</param>
    /// <param name="content">Nội dung thô từ client.</param>
    private void TryAttachSpecialPayload(SendMessageCommand command, string content)
    {
        if (string.Equals(command.Type, "payment_offer", StringComparison.Ordinal))
        {
            // Nhánh payment dùng payload riêng để phục vụ nghiệp vụ đề nghị thanh toán.
            TryAttachPaymentPayload(command, content);
            return;
        }

        if (string.Equals(command.Type, "image", StringComparison.Ordinal)
            || string.Equals(command.Type, "voice", StringComparison.Ordinal))
        {
            TryAttachMediaPayload(command, content);
        }
    }

    /// <summary>
    /// Thử parse payment payload từ content JSON.
    /// Luồng xử lý: parse JSON, gán payload + nội dung hiển thị fallback khi thiếu description.
    /// </summary>
    /// <param name="command">Command gửi message cần gắn payment payload.</param>
    /// <param name="content">JSON payment payload từ client.</param>
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
            // Parse lỗi chỉ ghi warning, giữ luồng gửi message tiếp tục theo xử lý mặc định.
            _logger.LogWarning(ex, "[ChatHub] Lỗi giải mã Payment Payload.");
        }
    }

    /// <summary>
    /// Thử parse media payload từ content JSON.
    /// Luồng xử lý: parse JSON thành media payload; nếu lỗi thì fallback gán URL thô và trạng thái unvalidated.
    /// </summary>
    /// <param name="command">Command gửi message cần gắn media payload.</param>
    /// <param name="content">JSON media payload hoặc URL fallback.</param>
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
            // Edge case payload không phải JSON hợp lệ: fallback lưu URL thô để không mất dữ liệu media.
            command.MediaPayload = new MediaPayloadDto { Url = content, ProcessingStatus = "fallback_unvalidated" };
            command.Content = string.Equals(command.Type, "voice", StringComparison.OrdinalIgnoreCase)
                ? "[voice]"
                : "[image]";
        }
    }
}

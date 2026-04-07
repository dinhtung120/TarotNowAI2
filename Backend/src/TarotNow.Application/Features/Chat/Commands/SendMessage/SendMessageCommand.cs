

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public class SendMessageCommand : IRequest<ChatMessageDto>
{
        public string ConversationId { get; set; } = string.Empty;

        public Guid SenderId { get; set; }

        public string Type { get; set; } = "text";

        public string Content { get; set; } = string.Empty;

        public PaymentPayloadDto? PaymentPayload { get; set; }

        public MediaPayloadDto? MediaPayload { get; set; }

        public CallSessionDto? CallPayload { get; set; }
}

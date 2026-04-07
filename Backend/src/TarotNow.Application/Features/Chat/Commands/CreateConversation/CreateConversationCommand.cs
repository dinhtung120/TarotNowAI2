

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

public class CreateConversationCommand : IRequest<ConversationDto>
{
        public Guid UserId { get; set; }

        public Guid ReaderId { get; set; }

        public int SlaHours { get; set; } = 12;
}

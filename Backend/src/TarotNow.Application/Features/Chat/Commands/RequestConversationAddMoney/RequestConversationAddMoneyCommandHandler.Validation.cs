using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public partial class RequestConversationAddMoneyCommandHandler
{
    private static void ValidateRequest(RequestConversationAddMoneyCommand request)
    {
        if (request.AmountDiamond <= 0)
        {
            throw new BadRequestException("Số tiền phải lớn hơn 0.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new BadRequestException("Lý do cộng tiền là bắt buộc.");
        }

        if (request.Description.Trim().Length > 100)
        {
            throw new BadRequestException("Lý do cộng tiền tối đa 100 ký tự.");
        }

        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            throw new BadRequestException("IdempotencyKey là bắt buộc.");
        }
    }
}

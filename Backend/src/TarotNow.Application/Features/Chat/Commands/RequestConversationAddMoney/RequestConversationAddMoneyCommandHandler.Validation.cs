using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationAddMoney;

public partial class RequestConversationAddMoneyCommandHandler
{
    /// <summary>
    /// Validate business rule đầu vào cho đề nghị cộng tiền.
    /// Luồng xử lý: kiểm tra amount dương, description bắt buộc/độ dài, idempotency key bắt buộc.
    /// </summary>
    private static void ValidateRequest(RequestConversationAddMoneyCommand request)
    {
        if (request.AmountDiamond <= 0)
        {
            // Amount phải dương để tránh đề nghị không hợp lệ.
            throw new BadRequestException("Số tiền phải lớn hơn 0.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            // Description bắt buộc để user biết lý do đề nghị cộng tiền.
            throw new BadRequestException("Lý do cộng tiền là bắt buộc.");
        }

        if (request.Description.Trim().Length > 100)
        {
            // Giới hạn mô tả theo UX hiển thị trong chat.
            throw new BadRequestException("Lý do cộng tiền tối đa 100 ký tự.");
        }

        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            // Idempotency key bắt buộc để chống tạo proposal trùng.
            throw new BadRequestException("IdempotencyKey là bắt buộc.");
        }
    }
}

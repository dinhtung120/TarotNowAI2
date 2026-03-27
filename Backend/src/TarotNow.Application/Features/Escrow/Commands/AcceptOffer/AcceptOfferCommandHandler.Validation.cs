using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AcceptOffer;

public partial class AcceptOfferCommandHandler
{
    private static string ValidateIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException("IdempotencyKey là bắt buộc.");
        }

        if (normalized.Length > 128)
        {
            throw new BadRequestException("IdempotencyKey quá dài (tối đa 128 ký tự).");
        }

        return normalized;
    }

    private async Task ValidateConversationAsync(AcceptOfferCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationRef, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.UserId != request.UserId.ToString() || conversation.ReaderId != request.ReaderId.ToString())
        {
            throw new BadRequestException("Thông tin phiên trò chuyện không hợp lệ cho giao dịch escrow.");
        }

        if (conversation.Status != ConversationStatus.Pending && conversation.Status != ConversationStatus.Active)
        {
            throw new BadRequestException($"Cuộc trò chuyện ở trạng thái '{conversation.Status}', không thể accept offer.");
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Tính chi phí cho lần stream hiện tại.
    /// Luồng xử lý: initial reading miễn phí, follow-up tính theo pricing service.
    /// </summary>
    private async Task<long> CalculateCostAsync(
        StreamReadingCommand request,
        ReadingSession session,
        Guid readingSessionRef,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FollowupQuestion))
        {
            // Luồng giải bài ban đầu không tính thêm phí follow-up.
            return 0;
        }

        var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(
            readingSessionRef,
            cancellationToken);

        long cost;
        try
        {
            cost = _pricingService.CalculateNextFollowupCost(session.CardsDrawn ?? "[]", followUpCount);
            // Tính phí dựa trên số follow-up hiện có và số lá đã rút trong session.
        }
        catch (InvalidOperationException ex)
        {
            // Quy đổi lỗi tính phí thành BadRequest để trả thông điệp nghiệp vụ nhất quán cho client.
            throw new BadRequestException(ex.Message);
        }

        return cost;
    }

    /// <summary>
    /// Đóng băng escrow trước khi bắt đầu stream.
    /// Luồng xử lý: bỏ qua khi cost bằng 0, freeze theo idempotency key, nếu lỗi thì cập nhật aiRequest trạng thái thất bại và ném lỗi nghiệp vụ tương ứng.
    /// </summary>
    private async Task FreezeEscrowAsync(
        StreamReadingCommand request,
        AiRequest aiRequest,
        long calculatedCost,
        CancellationToken cancellationToken)
    {
        if (calculatedCost <= 0)
        {
            // Không có chi phí cần thu thì bỏ qua bước freeze.
            return;
        }

        try
        {
            await _walletRepo.FreezeAsync(
                userId: request.UserId,
                amount: calculatedCost,
                referenceSource: "AiRequest",
                referenceId: aiRequest.Id.ToString(),
                description: ResolveEscrowDescription(request.FollowupQuestion),
                idempotencyKey: $"freeze_{aiRequest.Id}",
                cancellationToken: cancellationToken);
        }
        catch (InvalidOperationException)
        {
            // Nhóm lỗi nghiệp vụ số dư/điều kiện ví không đủ.
            throw new BadRequestException("Not enough balance to perform AI Reading.");
        }
    }

    /// <summary>
    /// Resolve mô tả giao dịch escrow theo loại yêu cầu.
    /// Luồng xử lý: follow-up và initial reading dùng mô tả khác nhau để dễ đối soát sổ cái.
    /// </summary>
    private static string ResolveEscrowDescription(string? followupQuestion)
    {
        return string.IsNullOrWhiteSpace(followupQuestion)
            ? "Escrow freeze for initial Tarot Reading"
            : "Escrow freeze for Follow-up Chat";
    }
}

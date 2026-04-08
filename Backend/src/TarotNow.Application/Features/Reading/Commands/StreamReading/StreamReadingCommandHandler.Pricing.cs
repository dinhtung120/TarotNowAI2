using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandler
{
    /// <summary>
    /// Tính chi phí cho lần stream hiện tại.
    /// Luồng xử lý: initial reading miễn phí, follow-up tính theo pricing service, sau đó thử consume entitlement miễn phí ngày.
    /// </summary>
    private async Task<long> CalculateCostAsync(
        StreamReadingCommand request,
        ReadingSession session,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FollowupQuestion))
        {
            // Luồng giải bài ban đầu không tính thêm phí follow-up.
            return 0;
        }

        var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(
            request.ReadingSessionId,
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

        if (cost > 0)
        {
            var consumeResult = await _entitlementService.ConsumeAsync(
                new EntitlementConsumeRequest(
                    request.UserId,
                    EntitlementKey.FreeAiStreamDaily,
                    "StreamReadingFollowup",
                    session.Id,
                    $"ai_stream_ent_{request.UserId:N}_{session.Id}_{followUpCount + 1}"),
                cancellationToken);
            // Thử dùng quyền miễn phí hằng ngày trước khi trừ ví để tối ưu trải nghiệm người dùng.

            if (consumeResult.Success)
            {
                // Consume entitlement thành công thì phí follow-up được giảm về 0.
                cost = 0;
            }
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
        catch (Exception ex)
        {
            aiRequest.Status = AiRequestStatus.FailedBeforeFirstToken;
            aiRequest.FinishReason = "insufficient_funds_or_error";
            await _aiRequestRepo.UpdateAsync(aiRequest, cancellationToken);
            // Đổi state request về failed sớm khi không freeze được để downstream không chờ completion vô nghĩa.

            if (ex is InvalidOperationException)
            {
                // Nhóm lỗi số dư không đủ được ánh xạ sang thông điệp người dùng dễ hiểu.
                throw new BadRequestException("Not enough balance to perform AI Reading.");
            }

            // Lỗi kỹ thuật khác khi freeze được quy về thông điệp chung để tránh lộ chi tiết hệ thống.
            throw new BadRequestException("Unable to reserve balance for AI Reading. Please try again later.");
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

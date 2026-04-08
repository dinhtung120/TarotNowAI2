using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public partial class InitReadingSessionCommandHandler
{
    /// <summary>
    /// Dựng entity ReadingSession ban đầu từ request.
    /// Luồng xử lý: map dữ liệu đầu vào và pricing đã resolve sang constructor session để chuyển cho orchestrator.
    /// </summary>
    private static ReadingSession BuildSession(
        InitReadingSessionCommand request,
        string currencyUsed,
        long amountCharged)
    {
        return new ReadingSession(
            request.UserId.ToString(),
            request.SpreadType,
            request.Question,
            currencyUsed,
            amountCharged);
    }

    /// <summary>
    /// Bắt đầu phiên reading thông qua orchestrator.
    /// Luồng xử lý: gọi StartPaidSession với session + chi phí, kiểm tra kết quả success và ném lỗi khi khởi tạo thất bại.
    /// </summary>
    private async Task StartSessionAsync(
        InitReadingSessionCommand request,
        ReadingSession session,
        SessionPricing pricing,
        CancellationToken cancellationToken)
    {
        var (success, _) = await _readingSessionOrchestrator.StartPaidSessionAsync(
            new StartPaidSessionRequest
            {
                UserId = request.UserId,
                SpreadType = request.SpreadType,
                Session = session,
                CostGold = pricing.CostGold,
                CostDiamond = pricing.CostDiamond
            },
            cancellationToken);

        if (!success)
        {
            // Edge case: orchestrator không mở được phiên (ví không đủ hoặc lỗi nghiệp vụ khác).
            throw new BadRequestException("Failed to start session. Please try again.");
        }
    }
}

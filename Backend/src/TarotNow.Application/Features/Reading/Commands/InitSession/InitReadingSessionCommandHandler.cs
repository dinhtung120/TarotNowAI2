/*
 * ===================================================================
 * FILE: InitReadingSessionCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Commands.InitSession
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thi Hành Xét Duyệt Yêu Cầu, Check Giá Tiền Xem Bảng Niêm Yết,
 *   Và Trừ Ngay Tiền Tại Lỗ nếu Thu Phí Trước Khi Mở Phòng Khấn.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public class InitReadingSessionCommandHandler : IRequestHandler<InitReadingSessionCommand, InitReadingSessionResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IUserRepository _userRepo;
    private readonly IRngService _rngService;
    private readonly ISystemConfigSettings _systemConfigSettings;

    public InitReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo, 
        IUserRepository userRepo,
        IRngService rngService,
        ISystemConfigSettings systemConfigSettings)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
        _rngService = rngService;
        _systemConfigSettings = systemConfigSettings;
    }

    public async Task<InitReadingSessionResult> Handle(InitReadingSessionCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra User tồn tại Lỡ Bị Xóa Nick Hay Hacker Lộng Hành.
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) throw new NotFoundException("User not found");

        long costGold = 0;
        long costDiamond = 0;

        // 2. Tòa Án Định Giá Quy Mô Bàn Gõ (Logic Pricing & Daily Limit)
        if (request.SpreadType == SpreadType.Daily1Card)
        {
            // Bài Theo Ngày (Bói 1 Lá May Mắn Đầu Giờ Sáng) -> Phải Check Kẻo Bốc 2 Lần.
            var alreadyDrawn = await _readingRepo.HasDrawnDailyCardAsync(request.UserId, DateTime.UtcNow, cancellationToken);
            if (alreadyDrawn)
            {
                throw new BadRequestException("You have already drawn your free daily card today. Please try other spreads.");
            }
            // Free cost = 0 (Từ Thủy Chí Chung)
        }
        else if (request.SpreadType == SpreadType.Spread3Cards)
        {
            // Trải 3 Lá Chết Tiền Vàng (50 Cắc)
            costGold = _systemConfigSettings.Spread3GoldCost;
        }
        else if (request.SpreadType == SpreadType.Spread5Cards)
        {
            costGold = _systemConfigSettings.Spread5GoldCost;
        }
        else if (request.SpreadType == SpreadType.Spread10Cards)
        {
            // Trải Đại Thập Tự Giá Celtic Cross (10 Lá) - Đòi Máu Kim Cương. (Hệ VIP Trả Tiền Tươi Thóc Thật Trả Nạp Card).
            costDiamond = _systemConfigSettings.Spread10DiamondCost;
        }

        // 3. Phân Mảnh Giới Thiệu Ngoại Tệ (Dùng cho Ghi Bill Auditing/Log Trạng Thái Xài Tiền).
        string? currencyUsed = null;
        long amountCharged = 0;
        
        if (costGold > 0)
        {
            currencyUsed = CurrencyType.Gold;
            amountCharged = costGold;
        }
        else if (costDiamond > 0)
        {
            currencyUsed = CurrencyType.Diamond;
            amountCharged = costDiamond;
        }

        // 4. Tạo Object Hồ Sơ Sếp Vào MongoDB (ReadingSession Document).
        var session = new ReadingSession(
            request.UserId.ToString(),
            request.SpreadType,
            request.Question,
            currencyUsed,
            amountCharged
        );

        // 5. Thẩm Định Giao Tiền Đóng Mộc (Transaction Giam Tiền Két Wallet + Chặn Thấy).
        var (success, _) = await _readingRepo.StartPaidSessionAtomicAsync(request.UserId, request.SpreadType, session, costGold, costDiamond, cancellationToken);
        
        // Không móc Hầu Bao Khách thì Fail. Văng Lỗi "Thiếu Tiền".
        if (!success)
        {
            throw new BadRequestException("Failed to start session. Please try again.");
        }

        // 6. Quán Lá Trà Cáo Chung (Mở Khoá). Bốc Session Cấp Về Frontend.
        // NHƯNG NHỚ: Lúc này Chưa Lật Bất Cứ Lá Bài Nào Hết Tới Lệnh "Reveal".
        return new InitReadingSessionResult
        {
            SessionId = session.Id,
            CostGold = costGold,
            CostDiamond = costDiamond
        };
    }
}

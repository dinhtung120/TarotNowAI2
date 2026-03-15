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

    // TODO: Nên move Pricing ra SystemConfig DB. Gắn cứng trong Phase này cho tiện.
    private const long SPREAD_3_COST = 50;  // 50 Gold
    private const long SPREAD_5_COST = 100; // 100 Gold
    private const long SPREAD_10_COST = 50; // 50 Diamond

    public InitReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo, 
        IUserRepository userRepo,
        IRngService rngService)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
        _rngService = rngService;
    }

    public async Task<InitReadingSessionResult> Handle(InitReadingSessionCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra User tồn tại
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) throw new NotFoundException("User not found");

        long costGold = 0;
        long costDiamond = 0;

        // 2. Logic Pricing & Daily Limit
        if (request.SpreadType == SpreadType.Daily1Card)
        {
            var alreadyDrawn = await _readingRepo.HasDrawnDailyCardAsync(request.UserId, DateTime.UtcNow, cancellationToken);
            if (alreadyDrawn)
            {
                throw new BadRequestException("You have already drawn your free daily card today. Please try other spreads.");
            }
            // Free cost
        }
        else if (request.SpreadType == SpreadType.Spread3Cards)
        {
            costGold = SPREAD_3_COST;
        }
        else if (request.SpreadType == SpreadType.Spread5Cards)
        {
            costGold = SPREAD_5_COST;
        }
        else if (request.SpreadType == SpreadType.Spread10Cards)
        {
            costDiamond = SPREAD_10_COST;
        }

        // 3. Xác định loại tiền và số tiền (cho auditing/stats)
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

        // 4. Tạo Object Session với thông đẩy đủ (Phase 1.3 spec)
        var session = new ReadingSession(
            request.UserId,
            request.SpreadType,
            request.Question,
            currencyUsed,
            amountCharged
        );

        // 5. Lưu Database Transaction (Trừ tiền + Lưu session đồng thời)
        var (success, error) = await _readingRepo.StartPaidSessionAtomicAsync(request.UserId, request.SpreadType, session, costGold, costDiamond, cancellationToken);
        
        if (!success)
        {
            throw new BadRequestException($"Failed to start session: {error}");
        }

        // 6. Trả kết quả (Lúc này CHƯA thực sự rút bài, chỉ là Cổng phòng chờ)
        return new InitReadingSessionResult
        {
            SessionId = session.Id,
            CostGold = costGold,
            CostDiamond = costDiamond
        };
    }
}

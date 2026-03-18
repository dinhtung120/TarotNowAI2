using MediatR;
using Microsoft.Extensions.Configuration;
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
    private readonly IConfiguration _configuration;

    public InitReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo, 
        IUserRepository userRepo,
        IRngService rngService,
        IConfiguration configuration)
    {
        _readingRepo = readingRepo;
        _userRepo = userRepo;
        _rngService = rngService;
        _configuration = configuration;
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
            costGold = ResolveCost("Spread3Gold", 50);
        }
        else if (request.SpreadType == SpreadType.Spread5Cards)
        {
            costGold = ResolveCost("Spread5Gold", 100);
        }
        else if (request.SpreadType == SpreadType.Spread10Cards)
        {
            costDiamond = ResolveCost("Spread10Diamond", 50);
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
            request.UserId.ToString(),
            request.SpreadType,
            request.Question,
            currencyUsed,
            amountCharged
        );

        // 5. Lưu Database Transaction (Trừ tiền + Lưu session đồng thời)
        var (success, _) = await _readingRepo.StartPaidSessionAtomicAsync(request.UserId, request.SpreadType, session, costGold, costDiamond, cancellationToken);
        
        if (!success)
        {
            throw new BadRequestException("Failed to start session. Please try again.");
        }

        // 6. Trả kết quả (Lúc này CHƯA thực sự rút bài, chỉ là Cổng phòng chờ)
        return new InitReadingSessionResult
        {
            SessionId = session.Id,
            CostGold = costGold,
            CostDiamond = costDiamond
        };
    }

    private long ResolveCost(string key, long defaultValue)
    {
        var configured = _configuration[$"SystemConfig:Pricing:{key}"];
        return long.TryParse(configured, out var value) && value >= 0 ? value : defaultValue;
    }
}

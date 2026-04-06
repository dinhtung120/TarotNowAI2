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

public partial class InitReadingSessionCommandHandler : IRequestHandler<InitReadingSessionCommand, InitReadingSessionResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IReadingSessionOrchestrator _readingSessionOrchestrator;
    private readonly IUserRepository _userRepo;
    private readonly IRngService _rngService;
    private readonly ISystemConfigSettings _systemConfigSettings;
    private readonly IEntitlementService _entitlementService;

    public InitReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo,
        IReadingSessionOrchestrator readingSessionOrchestrator,
        IUserRepository userRepo,
        IRngService rngService,
        ISystemConfigSettings systemConfigSettings,
        IEntitlementService entitlementService)
    {
        _readingRepo = readingRepo;
        _readingSessionOrchestrator = readingSessionOrchestrator;
        _userRepo = userRepo;
        _rngService = rngService;
        _systemConfigSettings = systemConfigSettings;
        _entitlementService = entitlementService;
    }

    public async Task<InitReadingSessionResult> Handle(InitReadingSessionCommand request, CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(request.UserId, cancellationToken);
        var pricing = await ResolvePricingAsync(request, cancellationToken);
        var session = BuildSession(request, pricing.CurrencyUsed, pricing.AmountCharged);
        await StartSessionAsync(request, session, pricing, cancellationToken);

        return new InitReadingSessionResult
        {
            SessionId = session.Id,
            CostGold = pricing.CostGold,
            CostDiamond = pricing.CostDiamond
        };
    }
}

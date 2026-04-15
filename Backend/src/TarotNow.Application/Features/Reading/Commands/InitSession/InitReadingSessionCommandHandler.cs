using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

// Handler điều phối khởi tạo phiên reading: kiểm tra user, tính pricing, dựng session và bắt đầu phiên trả phí.
public partial class InitReadingSessionCommandHandler : IRequestHandler<InitReadingSessionCommand, InitReadingSessionResult>
{
    private readonly IReadingSessionRepository _readingRepo;
    private readonly IReadingSessionOrchestrator _readingSessionOrchestrator;
    private readonly IUserRepository _userRepo;
    private readonly IRngService _rngService;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler init reading session.
    /// Luồng xử lý: nhận các repository/service cần thiết để validate user, resolve giá và mở phiên đọc.
    /// </summary>
    public InitReadingSessionCommandHandler(
        IReadingSessionRepository readingRepo,
        IReadingSessionOrchestrator readingSessionOrchestrator,
        IUserRepository userRepo,
        IRngService rngService,
        ISystemConfigSettings systemConfigSettings)
    {
        _readingRepo = readingRepo;
        _readingSessionOrchestrator = readingSessionOrchestrator;
        _userRepo = userRepo;
        _rngService = rngService;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Xử lý command khởi tạo reading session.
    /// Luồng xử lý: xác thực user tồn tại, resolve pricing theo spread/currency, khởi tạo session và gọi orchestrator mở phiên.
    /// </summary>
    public async Task<InitReadingSessionResult> Handle(
        InitReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(request.UserId, cancellationToken);
        // Chặn mở phiên cho user không hợp lệ trước khi phát sinh xử lý thanh toán.

        var pricing = await ResolvePricingAsync(request, cancellationToken);
        var session = BuildSession(request, pricing.CurrencyUsed, pricing.AmountCharged);
        await StartSessionAsync(request, session, pricing, cancellationToken);
        // Chỉ trả kết quả sau khi orchestrator xác nhận mở phiên thành công.

        return new InitReadingSessionResult
        {
            SessionId = session.Id,
            CostGold = pricing.CostGold,
            CostDiamond = pricing.CostDiamond
        };
    }
}

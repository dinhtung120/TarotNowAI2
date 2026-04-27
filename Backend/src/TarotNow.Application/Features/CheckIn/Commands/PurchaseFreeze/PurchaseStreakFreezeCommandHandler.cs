using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

// Handler xử lý nghiệp vụ mua phục hồi streak.
public class PurchaseStreakFreezeCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<PurchaseStreakFreezeCommandHandlerRequestedDomainEvent>
{
    // Prefix idempotency cho giao dịch trừ kim cương mua freeze.
    private const string FreezeIdempotencyPrefix = "freeze_";

    // Mốc đầu ngày UTC để tính cửa sổ thời gian mua freeze.
    private static readonly TimeOnly StartOfDayTime = new(0, 0);

    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ISystemConfigSettings _settings;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler purchase streak freeze.
    /// Luồng xử lý: nhận repository user/wallet và system settings để kiểm tra cửa sổ mua.
    /// </summary>
    public PurchaseStreakFreezeCommandHandlerRequestedDomainEventHandler(
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        ISystemConfigSettings settings,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _settings = settings;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command mua phục hồi streak.
    /// Luồng xử lý: validate idempotency, tải user, kiểm tra điều kiện phục hồi + cửa sổ mua, trừ kim cương, restore streak và lưu user.
    /// </summary>
    public async Task<PurchaseStreakFreezeResult> Handle(PurchaseStreakFreezeCommand request, CancellationToken cancellationToken)
    {
        var idempotencyKey = ValidateAndNormalizeIdempotencyKey(request.IdempotencyKey);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            // Edge case: user không tồn tại nên không thể thực hiện giao dịch.
            throw new NotFoundException($"User {request.UserId} not found");
        }

        // Business rule: chỉ cho phục hồi khi có streak bị gãy và còn trong cửa sổ mua.
        EnsureUserHasBrokenStreak(user.PreBreakStreak);
        EnsurePurchaseWindowIsValid(user.LastStreakDate, _settings.StreakFreezeWindowHours);

        var priceDiamond = user.CalculateFreezePrice();
        // Trừ kim cương trước khi cập nhật state user để đảm bảo tính nhất quán tài chính.
        await DebitDiamondAsync(user, priceDiamond, idempotencyKey, cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.MoneyChangedDomainEvent
            {
                UserId = user.Id,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.StreakFreezeCost,
                DeltaAmount = -priceDiamond,
                ReferenceId = idempotencyKey
            },
            cancellationToken);

        user.RestoreStreak();
        // Lưu thay đổi streak sau khi giao dịch ví thành công.
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new PurchaseStreakFreezeResult
        {
            Success = true,
            RestoredStreak = user.CurrentStreak,
            DiamondCost = priceDiamond
        };
    }

    /// <summary>
    /// Validate và chuẩn hóa idempotency key cho giao dịch mua freeze.
    /// Luồng xử lý: chặn giá trị rỗng và trim khoảng trắng để key ổn định.
    /// </summary>
    private static string ValidateAndNormalizeIdempotencyKey(string? idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new BadRequestException("Thiếu IdempotencyKey để giao dịch Kim Cương.");
        }

        return idempotencyKey.Trim();
    }

    /// <summary>
    /// Kiểm tra user có streak bị gãy để phục hồi hay không.
    /// Luồng xử lý: yêu cầu PreBreakStreak > 0; nếu không thì ném lỗi nghiệp vụ.
    /// </summary>
    private static void EnsureUserHasBrokenStreak(int preBreakStreak)
    {
        if (preBreakStreak > 0)
        {
            return;
        }

        throw new BadRequestException("Bạn không có chuỗi Streak bị gãy nào để phải tốn tiền phục hồi.");
    }

    /// <summary>
    /// Kiểm tra user còn trong cửa sổ thời gian được phép mua freeze hay không.
    /// Luồng xử lý: xác định ngày phát hiện vỡ chuỗi, đối chiếu với hôm nay và tính window end theo settings.
    /// </summary>
    private static void EnsurePurchaseWindowIsValid(DateOnly? lastStreakDate, int freezeWindowHours)
    {
        var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateBeforeBreak = lastStreakDate ?? todayDate.AddDays(-2);
        var breakDiscoveryDate = dateBeforeBreak.AddDays(2);
        if (breakDiscoveryDate != todayDate)
        {
            // Chỉ cho mua trong đúng ngày phát hiện vỡ chuỗi.
            throw new BadRequestException(
                "Đã quá hạn để đóng băng/phục hồi Streak (Chỉ có quyền mua trong ngày phát hiện vỡ chuỗi).");
        }

        var windowEnd = breakDiscoveryDate.ToDateTime(StartOfDayTime).AddHours(freezeWindowHours);
        if (DateTime.UtcNow <= windowEnd)
        {
            return;
        }

        // Quá hạn cửa sổ theo giờ cấu hình thì không cho phục hồi nữa.
        throw new BadRequestException(
            $"Đã quá cửa sổ {freezeWindowHours} tiếng kể từ lúc bị gãy. Không thể phục hồi nữa.");
    }

    /// <summary>
    /// Trừ kim cương từ ví user để thanh toán chi phí phục hồi streak.
    /// Luồng xử lý: gọi wallet debit với transaction type StreakFreezeCost và idempotency key đã chuẩn hóa.
    /// </summary>
    private async Task DebitDiamondAsync(
        Domain.Entities.User user,
        long priceDiamond,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _walletRepository.DebitAsync(
            userId: user.Id,
            currency: CurrencyType.Diamond,
            type: TransactionType.StreakFreezeCost,
            amount: priceDiamond,
            description: $"Mua Lệnh Hồi Sinh Chuỗi Streak {user.PreBreakStreak} Ngày.",
            idempotencyKey: FreezeIdempotencyPrefix + idempotencyKey,
            cancellationToken: cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        PurchaseStreakFreezeCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.AddUserBalance;

// Command cộng số dư thủ công cho người dùng từ màn quản trị.
public class AddUserBalanceCommand : IRequest<bool>
{
    // Định danh người dùng nhận số dư cộng thêm.
    public Guid UserId { get; set; }

    // Loại tiền tệ cần cộng (gold/diamond).
    public string Currency { get; set; } = CurrencyType.Gold;

    // Số lượng cần cộng.
    public long Amount { get; set; }

    // Lý do cộng tiền để lưu lịch sử đối soát.
    public string? Reason { get; set; }

    // Khóa idempotency bắt buộc để chống cộng trùng.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// Handler thực thi nghiệp vụ cộng số dư thủ công cho admin.
public class AddUserBalanceCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AddUserBalanceCommandHandlerRequestedDomainEvent>
{
    // Reference source cố định để phân loại giao dịch cộng tay từ admin.
    private const string AdminReferenceSource = "Admin_Manual";
    // Prefix idempotency nội bộ để tách namespace so với luồng cộng tiền khác.
    private const string IdempotencyPrefix = "admin_credit_";
    // Thông điệp lỗi khi currency ngoài danh sách cho phép.
    private const string InvalidCurrencyMessage = "Currency không hợp lệ. Chỉ chấp nhận 'gold' hoặc 'diamond'.";
    // Thông điệp lỗi khi thiếu khóa idempotency.
    private const string MissingIdempotencyKeyMessage = "IdempotencyKey là bắt buộc cho thao tác cộng tiền thủ công.";

    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler cộng số dư cho admin.
    /// Luồng xử lý: nhận repository user để kiểm tra tồn tại và wallet repository để thực thi credit.
    /// </summary>
    public AddUserBalanceCommandHandlerRequestedDomainEventHandler(
        IWalletRepository walletRepository,
        IUserRepository userRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command cộng số dư thủ công.
    /// Luồng xử lý: kiểm tra user tồn tại, chuẩn hóa currency/idempotency, sau đó gọi credit ví.
    /// </summary>
    public async Task<bool> Handle(AddUserBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            // Edge case user không tồn tại: trả false để lớp gọi quyết định thông điệp phù hợp.
            return false;
        }

        var normalizedCurrency = NormalizeAndValidateCurrency(request.Currency);
        var idempotencyKey = NormalizeAndValidateIdempotencyKey(request.IdempotencyKey);

        // Chỉ thực thi credit khi toàn bộ rule đầu vào hợp lệ để tránh side-effect không mong muốn.
        await CreditUserAsync(request, normalizedCurrency, idempotencyKey, cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.MoneyChangedDomainEvent
            {
                UserId = request.UserId,
                Currency = normalizedCurrency,
                ChangeType = TransactionType.AdminTopup,
                DeltaAmount = request.Amount,
                ReferenceId = idempotencyKey
            },
            cancellationToken);

        return true;
    }

    /// <summary>
    /// Chuẩn hóa và kiểm tra currency đầu vào.
    /// Luồng xử lý: trim+lowercase currency, chỉ chấp nhận gold/diamond, ném BadRequest nếu sai.
    /// </summary>
    private static string NormalizeAndValidateCurrency(string? currency)
    {
        var normalizedCurrency = currency?.Trim().ToLowerInvariant() ?? string.Empty;
        if (normalizedCurrency == CurrencyType.Gold || normalizedCurrency == CurrencyType.Diamond)
        {
            return normalizedCurrency;
        }

        // Rule nghiệp vụ: chỉ cho cộng hai loại currency hệ thống hỗ trợ.
        throw new BadRequestException(InvalidCurrencyMessage);
    }

    /// <summary>
    /// Chuẩn hóa và kiểm tra idempotency key đầu vào.
    /// Luồng xử lý: trim chuỗi, xác nhận không rỗng, ném BadRequest nếu thiếu.
    /// </summary>
    private static string NormalizeAndValidateIdempotencyKey(string? idempotencyKey)
    {
        var normalizedKey = idempotencyKey?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedKey))
        {
            return normalizedKey;
        }

        // Rule bắt buộc idempotency để tránh admin click lặp gây cộng tiền nhiều lần.
        throw new BadRequestException(MissingIdempotencyKeyMessage);
    }

    /// <summary>
    /// Gọi repository ví để ghi giao dịch credit với metadata admin topup.
    /// Luồng xử lý: truyền đầy đủ thông tin user/currency/amount/reference/idempotency cho transaction credit.
    /// </summary>
    private async Task CreditUserAsync(
        AddUserBalanceCommand request,
        string normalizedCurrency,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _walletRepository.CreditAsync(
            userId: request.UserId,
            currency: normalizedCurrency,
            type: TransactionType.AdminTopup,
            amount: request.Amount,
            referenceSource: AdminReferenceSource,
            referenceId: idempotencyKey,
            description: request.Reason ?? $"Admin credited {request.Amount} {request.Currency}",
            idempotencyKey: IdempotencyPrefix + idempotencyKey,
            cancellationToken: cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        AddUserBalanceCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}

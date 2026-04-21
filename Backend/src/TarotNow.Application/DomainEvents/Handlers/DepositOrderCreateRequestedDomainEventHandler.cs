using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler write-side cho event tạo đơn nạp tiền.
/// </summary>
public sealed class DepositOrderCreateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DepositOrderCreateRequestedDomainEvent>
{
    private const string DescriptionPrefix = "TOPUP";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IDepositPromotionRepository _depositPromotionRepository;
    private readonly IDepositPackageCatalog _depositPackageCatalog;
    private readonly IDepositPayOsSettings _depositPayOsSettings;
    private readonly IPayOsGateway _payOsGateway;

    /// <summary>
    /// Khởi tạo handler tạo đơn nạp.
    /// </summary>
    public DepositOrderCreateRequestedDomainEventHandler(
        IDepositOrderRepository depositOrderRepository,
        IDepositPromotionRepository depositPromotionRepository,
        IDepositPackageCatalog depositPackageCatalog,
        IDepositPayOsSettings depositPayOsSettings,
        IPayOsGateway payOsGateway,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _depositOrderRepository = depositOrderRepository;
        _depositPromotionRepository = depositPromotionRepository;
        _depositPackageCatalog = depositPackageCatalog;
        _depositPayOsSettings = depositPayOsSettings;
        _payOsGateway = payOsGateway;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        DepositOrderCreateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var clientRequestKey = BuildClientRequestKey(domainEvent.UserId, domainEvent.IdempotencyKey);
        await _depositOrderRepository.AcquireCreateOrderLockAsync(clientRequestKey, cancellationToken);
        var existingOrder = await _depositOrderRepository.GetByClientRequestKeyAsync(clientRequestKey, cancellationToken);
        if (existingOrder != null)
        {
            HydrateCreateResponse(domainEvent, existingOrder);
            return;
        }

        var selectedPackage = ResolvePackage(domainEvent.PackageCode);
        var bonusGoldAmount = await ResolveBonusGoldAsync(selectedPackage.AmountVnd, cancellationToken);
        var orderCode = GenerateOrderCode();
        var paymentLink = await CreatePaymentLinkAsync(selectedPackage, orderCode, cancellationToken);

        var order = new DepositOrder(
            domainEvent.UserId,
            selectedPackage.Code,
            selectedPackage.AmountVnd,
            selectedPackage.BaseDiamond,
            bonusGoldAmount,
            clientRequestKey,
            orderCode,
            paymentLink.PaymentLinkId,
            paymentLink.CheckoutUrl,
            paymentLink.QrCode,
            paymentLink.ExpiresAtUtc);

        var persistedOrder = await _depositOrderRepository.AddOrGetExistingByClientRequestKeyAsync(
            order,
            clientRequestKey,
            cancellationToken);
        HydrateCreateResponse(domainEvent, persistedOrder);
    }

    private Task<PayOsCreatePaymentLinkResult> CreatePaymentLinkAsync(
        DepositPackageDefinition selectedPackage,
        long orderCode,
        CancellationToken cancellationToken)
    {
        var expiredAtUnix = DateTimeOffset.UtcNow
            .AddMinutes(_depositPayOsSettings.LinkExpiryMinutes)
            .ToUnixTimeSeconds();

        return _payOsGateway.CreatePaymentLinkAsync(
            new PayOsCreatePaymentLinkRequest
            {
                OrderCode = orderCode,
                Amount = selectedPackage.AmountVnd,
                Description = BuildPaymentDescription(orderCode),
                CancelUrl = _depositPayOsSettings.CancelUrl,
                ReturnUrl = _depositPayOsSettings.ReturnUrl,
                ExpiredAtUnix = expiredAtUnix,
                Items =
                [
                    new PayOsPaymentItem
                    {
                        Name = $"TarotNow {selectedPackage.Code}",
                        Quantity = 1,
                        Price = selectedPackage.AmountVnd
                    }
                ]
            },
            cancellationToken);
    }

    private DepositPackageDefinition ResolvePackage(string packageCode)
    {
        var package = _depositPackageCatalog.FindByCode(packageCode);
        if (package == null || package.IsActive == false)
        {
            throw new BadRequestException("Invalid or inactive deposit package.");
        }

        return package;
    }

    private async Task<long> ResolveBonusGoldAsync(long amountVnd, CancellationToken cancellationToken)
    {
        var activePromotions = await _depositPromotionRepository.GetActivePromotionsAsync(cancellationToken);
        foreach (var promotion in activePromotions)
        {
            if (amountVnd >= promotion.MinAmountVnd)
            {
                return promotion.BonusGold;
            }
        }

        return 0;
    }

    private static void HydrateCreateResponse(DepositOrderCreateRequestedDomainEvent domainEvent, DepositOrder order)
    {
        domainEvent.OrderId = order.Id;
        domainEvent.Status = order.Status;
        domainEvent.AmountVnd = order.AmountVnd;
        domainEvent.BaseDiamondAmount = order.BaseDiamondAmount;
        domainEvent.BonusGoldAmount = order.BonusGoldAmount;
        domainEvent.TotalDiamondAmount = order.DiamondAmount;
        domainEvent.PayOsOrderCode = order.PayOsOrderCode;
        domainEvent.CheckoutUrl = order.CheckoutUrl;
        domainEvent.QrCode = order.QrCode;
        domainEvent.PaymentLinkId = order.PayOsPaymentLinkId;
        domainEvent.ExpiresAtUtc = order.ExpiresAtUtc;
    }

    private static string BuildClientRequestKey(Guid userId, string idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException("Idempotency key is required.");
        }

        var source = $"{userId:N}:{normalized}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        var hashPrefix = Convert.ToHexString(hash.AsSpan(0, 16)).ToLowerInvariant();
        return $"deposit_{userId:N}_{hashPrefix}";
    }

    private static long GenerateOrderCode()
    {
        var unixMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var suffix = RandomNumberGenerator.GetInt32(0, 1000);
        return checked(unixMilliseconds * 1000 + suffix);
    }

    private static string BuildPaymentDescription(long orderCode)
    {
        return $"{DescriptionPrefix} {orderCode}";
    }
}

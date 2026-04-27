using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Helpers;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Xử lý domain event cập nhật profile người dùng.
/// </summary>
public sealed class UserProfileUpdateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UserProfileUpdateRequestedDomainEvent>
{
    private const int MinAccountNumberLength = 6;
    private const int MaxAccountNumberLength = 32;

    private const string ErrorUserNotFoundTemplate = "User with Id {0} not found.";
    private const string ErrorReaderOnly = "Chỉ tài khoản Reader mới có thể cập nhật thông tin rút tiền.";
    private const string ErrorRequiredBankName = "Tên ngân hàng là bắt buộc.";
    private const string ErrorRequiredBankBin = "Mã BIN ngân hàng là bắt buộc.";
    private const string ErrorRequiredAccountNumber = "Số tài khoản là bắt buộc.";
    private const string ErrorInvalidAccountNumber = "Số tài khoản không hợp lệ.";
    private const string ErrorAccountNumberDigitsOnly = "Số tài khoản chỉ được chứa chữ số.";
    private const string ErrorRequiredAccountHolder = "Tên chủ tài khoản là bắt buộc.";
    private const string ErrorInvalidAccountHolder = "Tên chủ tài khoản phải là chữ hoa không dấu.";
    private const string ErrorUnsupportedBank = "Ngân hàng không nằm trong danh mục hỗ trợ payout.";

    private readonly IUserRepository _userRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler cập nhật profile.
    /// </summary>
    public UserProfileUpdateRequestedDomainEventHandler(
        IUserRepository userRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        UserProfileUpdateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var user = await LoadUserAsync(domainEvent.UserId, cancellationToken);
        user.UpdateProfile(domainEvent.DisplayName, user.AvatarUrl, domainEvent.DateOfBirth);
        ApplyPayoutBankInfoIfProvided(user, domainEvent);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new UserProfileProjectionSyncRequestedDomainEvent
            {
                UserId = user.Id,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                SourceUpdatedAtUtc = user.UpdatedAt ?? DateTime.UtcNow,
                OccurredAtUtc = DateTime.UtcNow
            },
            cancellationToken);
        domainEvent.Updated = true;
    }

    private async Task<User> LoadUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException(string.Format(ErrorUserNotFoundTemplate, userId));
    }

    private void ApplyPayoutBankInfoIfProvided(User user, UserProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (!HasAnyPayoutInput(domainEvent))
        {
            return;
        }

        EnsureReaderRole(user);
        var bankName = RequireText(domainEvent.PayoutBankName, ErrorRequiredBankName);
        var bankBin = RequireText(domainEvent.PayoutBankBin, ErrorRequiredBankBin);
        var accountNumber = ValidateAccountNumber(domainEvent.PayoutBankAccountNumber);
        var accountHolder = ValidateAccountHolder(domainEvent.PayoutBankAccountHolder);
        ValidateBankPair(bankBin, bankName);
        user.UpdatePayoutBankInfo(bankName, bankBin, accountNumber, accountHolder);
    }

    private static bool HasAnyPayoutInput(UserProfileUpdateRequestedDomainEvent domainEvent)
    {
        return string.IsNullOrWhiteSpace(domainEvent.PayoutBankName) == false
               || string.IsNullOrWhiteSpace(domainEvent.PayoutBankBin) == false
               || string.IsNullOrWhiteSpace(domainEvent.PayoutBankAccountNumber) == false
               || string.IsNullOrWhiteSpace(domainEvent.PayoutBankAccountHolder) == false;
    }

    private static void EnsureReaderRole(User user)
    {
        if (string.Equals(user.Role, UserRole.TarotReader, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        throw new BadRequestException(ErrorReaderOnly);
    }

    private static string RequireText(string? value, string message)
    {
        var normalized = value?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException(message);
        }

        return normalized;
    }

    private static string ValidateAccountNumber(string? accountNumber)
    {
        var normalized = RequireText(accountNumber, ErrorRequiredAccountNumber);
        if (normalized.Length < MinAccountNumberLength || normalized.Length > MaxAccountNumberLength)
        {
            throw new BadRequestException(ErrorInvalidAccountNumber);
        }

        if (normalized.All(char.IsDigit) == false)
        {
            throw new BadRequestException(ErrorAccountNumberDigitsOnly);
        }

        return normalized;
    }

    private static string ValidateAccountHolder(string? accountHolder)
    {
        var normalized = RequireText(accountHolder, ErrorRequiredAccountHolder);
        if (AccountHolderNameValidator.IsValidUppercaseNoAccent(normalized))
        {
            return normalized;
        }

        throw new BadRequestException(ErrorInvalidAccountHolder);
    }

    private static void ValidateBankPair(string bankBin, string bankName)
    {
        if (VietnamBankCatalog.IsValidPair(bankBin, bankName))
        {
            return;
        }

        throw new BadRequestException(ErrorUnsupportedBank);
    }

}

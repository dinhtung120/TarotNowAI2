using TarotNow.Application.Common;
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
/// Handler xử lý luồng submit đơn Reader.
/// </summary>
public sealed class ReaderRequestSubmitRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderRequestSubmitRequestedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IReaderRequestRepository _readerRequestRepository;

    /// <summary>
    /// Khởi tạo handler submit đơn Reader.
    /// </summary>
    public ReaderRequestSubmitRequestedDomainEventHandler(
        IUserRepository userRepository,
        IReaderRequestRepository readerRequestRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _userRepository = userRepository;
        _readerRequestRepository = readerRequestRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderRequestSubmitRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var user = await LoadUserAsync(domainEvent.UserId, cancellationToken);
        EnsureUserCanSubmitRequest(user);
        await EnsureNoPendingRequestAsync(user.Id, cancellationToken);

        var specialties = NormalizeAndEnsureSpecialties(domainEvent.Specialties);
        var socialLinks = NormalizeAndEnsureSocialLinks(
            domainEvent.FacebookUrl,
            domainEvent.InstagramUrl,
            domainEvent.TikTokUrl);

        var request = new ReaderRequestDto
        {
            UserId = user.Id.ToString(),
            Status = ReaderApprovalStatus.Pending,
            Bio = domainEvent.Bio.Trim(),
            Specialties = specialties,
            YearsOfExperience = domainEvent.YearsOfExperience,
            FacebookUrl = socialLinks.FacebookUrl,
            InstagramUrl = socialLinks.InstagramUrl,
            TikTokUrl = socialLinks.TikTokUrl,
            DiamondPerQuestion = domainEvent.DiamondPerQuestion,
            ProofDocuments = domainEvent.ProofDocuments.ToList(),
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _readerRequestRepository.AddAsync(request, cancellationToken);
            domainEvent.Submitted = true;
            domainEvent.RequestId = request.Id;
        }
        catch (BadRequestException)
        {
            var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(user.Id.ToString(), cancellationToken);
            if (latestRequest is not null && latestRequest.Status == ReaderApprovalStatus.Pending)
            {
                domainEvent.Submitted = true;
                domainEvent.RequestId = latestRequest.Id;
                return;
            }

            throw;
        }
    }

    private async Task<User> LoadUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");
    }

    private static void EnsureUserCanSubmitRequest(User user)
    {
        if (user.Status != UserStatus.Active)
        {
            throw new BadRequestException("Tài khoản chưa được kích hoạt hoặc đã bị khóa.");
        }

        if (user.Role != UserRole.User)
        {
            throw new BadRequestException("Bạn đã có vai trò đặc biệt, không cần đăng ký Reader.");
        }
    }

    private async Task EnsureNoPendingRequestAsync(Guid userId, CancellationToken cancellationToken)
    {
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(userId.ToString(), cancellationToken);
        if (latestRequest is not null && latestRequest.Status == ReaderApprovalStatus.Pending)
        {
            throw new BadRequestException("Bạn đã có đơn đang chờ duyệt. Vui lòng chờ admin xử lý.");
        }
    }

    private static List<string> NormalizeAndEnsureSpecialties(IEnumerable<string> specialties)
    {
        var normalized = ReaderSpecialties.NormalizeDistinct(specialties).ToList();
        if (normalized.Count == 0)
        {
            throw new BadRequestException("Reader phải chọn ít nhất 1 chuyên môn.");
        }

        if (normalized.Any(ReaderSpecialties.IsSupported) == false || normalized.Any(x => ReaderSpecialties.IsSupported(x) == false))
        {
            throw new BadRequestException("Danh sách chuyên môn không hợp lệ.");
        }

        return normalized;
    }

    private static ReaderSocialLinksState NormalizeAndEnsureSocialLinks(
        string? facebookUrl,
        string? instagramUrl,
        string? tikTokUrl)
    {
        var normalized = new ReaderSocialLinksState(
            ReaderSocialUrlValidator.NormalizeOptionalUrl(facebookUrl),
            ReaderSocialUrlValidator.NormalizeOptionalUrl(instagramUrl),
            ReaderSocialUrlValidator.NormalizeOptionalUrl(tikTokUrl));

        if (ReaderSocialUrlValidator.HasAtLeastOneSocialLink(normalized.FacebookUrl, normalized.InstagramUrl, normalized.TikTokUrl) == false)
        {
            throw new BadRequestException("Phải cung cấp ít nhất 1 link Facebook, Instagram hoặc TikTok.");
        }

        if (!ReaderSocialUrlValidator.IsValidFacebookUrl(normalized.FacebookUrl))
        {
            throw new BadRequestException("FacebookUrl không hợp lệ hoặc không đúng domain Facebook.");
        }

        if (!ReaderSocialUrlValidator.IsValidInstagramUrl(normalized.InstagramUrl))
        {
            throw new BadRequestException("InstagramUrl không hợp lệ hoặc không đúng domain Instagram.");
        }

        if (!ReaderSocialUrlValidator.IsValidTikTokUrl(normalized.TikTokUrl))
        {
            throw new BadRequestException("TikTokUrl không hợp lệ hoặc không đúng domain TikTok.");
        }

        return normalized;
    }

    private sealed record ReaderSocialLinksState(string? FacebookUrl, string? InstagramUrl, string? TikTokUrl);
}

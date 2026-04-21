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

        await _readerRequestRepository.AddAsync(request, cancellationToken);
        domainEvent.Submitted = true;
        domainEvent.RequestId = request.Id;
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

/// <summary>
/// Handler xử lý cập nhật hồ sơ Reader.
/// </summary>
public sealed class ReaderProfileUpdateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderProfileUpdateRequestedDomainEvent>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật hồ sơ Reader.
    /// </summary>
    public ReaderProfileUpdateRequestedDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderProfileUpdateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(domainEvent.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader. Bạn cần được admin duyệt trước.");

        ApplyBioPatch(profile, domainEvent);
        ApplyPricePatch(profile, domainEvent);
        ApplySpecialtiesPatch(profile, domainEvent);
        ApplyYearsOfExperiencePatch(profile, domainEvent);
        ApplySocialLinksPatch(profile, domainEvent);

        EnsureProfileInvariants(profile);

        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        domainEvent.Updated = true;
    }

    private static void ApplyBioPatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (domainEvent.BioVi is not null)
        {
            profile.BioVi = domainEvent.BioVi;
        }

        if (domainEvent.BioEn is not null)
        {
            profile.BioEn = domainEvent.BioEn;
        }

        if (domainEvent.BioZh is not null)
        {
            profile.BioZh = domainEvent.BioZh;
        }
    }

    private static void ApplyPricePatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (!domainEvent.DiamondPerQuestion.HasValue)
        {
            return;
        }

        if (domainEvent.DiamondPerQuestion.Value < 50)
        {
            throw new BadRequestException("Giá mỗi câu hỏi phải từ 50 Diamond.");
        }

        profile.DiamondPerQuestion = domainEvent.DiamondPerQuestion.Value;
    }

    private static void ApplySpecialtiesPatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (domainEvent.Specialties is null)
        {
            return;
        }

        var specialties = ReaderSpecialties.NormalizeDistinct(domainEvent.Specialties).ToList();
        if (specialties.Count == 0)
        {
            throw new BadRequestException("Reader phải chọn ít nhất 1 chuyên môn.");
        }

        if (specialties.Any(ReaderSpecialties.IsSupported) == false || specialties.Any(x => ReaderSpecialties.IsSupported(x) == false))
        {
            throw new BadRequestException("Danh sách chuyên môn không hợp lệ.");
        }

        profile.Specialties = specialties;
    }

    private static void ApplyYearsOfExperiencePatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        if (!domainEvent.YearsOfExperience.HasValue)
        {
            return;
        }

        if (domainEvent.YearsOfExperience.Value < 1)
        {
            throw new BadRequestException("Số năm kinh nghiệm tối thiểu là 1.");
        }

        profile.YearsOfExperience = domainEvent.YearsOfExperience.Value;
    }

    private static void ApplySocialLinksPatch(ReaderProfileDto profile, ReaderProfileUpdateRequestedDomainEvent domainEvent)
    {
        profile.FacebookUrl = ResolvePatchedSocialLink(
            domainEvent.FacebookUrl,
            profile.FacebookUrl,
            ReaderSocialUrlValidator.IsValidFacebookUrl,
            "FacebookUrl không hợp lệ hoặc không đúng domain Facebook.");

        profile.InstagramUrl = ResolvePatchedSocialLink(
            domainEvent.InstagramUrl,
            profile.InstagramUrl,
            ReaderSocialUrlValidator.IsValidInstagramUrl,
            "InstagramUrl không hợp lệ hoặc không đúng domain Instagram.");

        profile.TikTokUrl = ResolvePatchedSocialLink(
            domainEvent.TikTokUrl,
            profile.TikTokUrl,
            ReaderSocialUrlValidator.IsValidTikTokUrl,
            "TikTokUrl không hợp lệ hoặc không đúng domain TikTok.");
    }

    private static string? ResolvePatchedSocialLink(
        string? incoming,
        string? current,
        Func<string?, bool> validator,
        string invalidMessage)
    {
        if (incoming is null)
        {
            return current;
        }

        var normalized = ReaderSocialUrlValidator.NormalizeOptionalUrl(incoming);
        if (normalized is null)
        {
            return null;
        }

        if (!validator(normalized))
        {
            throw new BadRequestException(invalidMessage);
        }

        return normalized;
    }

    private static void EnsureProfileInvariants(ReaderProfileDto profile)
    {
        if (profile.Specialties.Count == 0)
        {
            throw new BadRequestException("Reader phải chọn ít nhất 1 chuyên môn.");
        }

        if (profile.YearsOfExperience < 1)
        {
            throw new BadRequestException("Số năm kinh nghiệm tối thiểu là 1.");
        }

        if (profile.DiamondPerQuestion < 50)
        {
            throw new BadRequestException("Giá mỗi câu hỏi phải từ 50 Diamond.");
        }

        if (!ReaderSocialUrlValidator.HasAtLeastOneSocialLink(profile.FacebookUrl, profile.InstagramUrl, profile.TikTokUrl))
        {
            throw new BadRequestException("Phải cung cấp ít nhất 1 link Facebook, Instagram hoặc TikTok.");
        }
    }
}

/// <summary>
/// Handler xử lý cập nhật trạng thái Reader.
/// </summary>
public sealed class ReaderStatusUpdateRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderStatusUpdateRequestedDomainEvent>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    /// <summary>
    /// Khởi tạo handler cập nhật trạng thái Reader.
    /// </summary>
    public ReaderStatusUpdateRequestedDomainEventHandler(
        IReaderProfileRepository readerProfileRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderStatusUpdateRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        if (!ReaderOnlineStatus.TryNormalize(domainEvent.Status, out var normalizedStatus))
        {
            throw new BadRequestException($"Trạng thái '{domainEvent.Status}' không hợp lệ. Chỉ chấp nhận: offline, busy.");
        }

        if (normalizedStatus == ReaderOnlineStatus.Online)
        {
            throw new BadRequestException("Trạng thái 'online' được cập nhật tự động khi kết nối. Truyền 'busy' hoặc 'offline' để đổi trạng thái thủ công.");
        }

        var profile = await _readerProfileRepository.GetByUserIdAsync(domainEvent.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        profile.Status = normalizedStatus;
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
        domainEvent.Updated = true;
    }
}

/// <summary>
/// Handler xử lý duyệt/từ chối đơn Reader.
/// </summary>
public sealed class ReaderRequestReviewRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReaderRequestReviewRequestedDomainEvent>
{
    private const string ApproveAction = "approve";
    private const string RejectAction = "reject";

    private readonly IReaderRequestRepository _readerRequestRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler xử lý duyệt đơn Reader.
    /// </summary>
    public ReaderRequestReviewRequestedDomainEventHandler(
        IReaderRequestRepository readerRequestRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _readerRequestRepository = readerRequestRepository;
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var readerRequest = await _readerRequestRepository.GetByIdAsync(domainEvent.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");

        EnsurePendingRequest(readerRequest);

        var userId = ParseUserId(readerRequest.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        if (string.Equals(domainEvent.Action, ApproveAction, StringComparison.Ordinal))
        {
            await ApproveAsync(readerRequest, user, domainEvent, cancellationToken);
            domainEvent.Processed = true;
            return;
        }

        if (string.Equals(domainEvent.Action, RejectAction, StringComparison.Ordinal))
        {
            await RejectAsync(readerRequest, user, domainEvent, cancellationToken);
            domainEvent.Processed = true;
            return;
        }

        throw new BadRequestException("Action không hợp lệ. Chỉ chấp nhận: approve, reject.");
    }

    private async Task ApproveAsync(
        ReaderRequestDto readerRequest,
        User user,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        EnsureRequestHasMandatoryFieldsForApproval(readerRequest);

        user.ApproveAsReader();
        await _userRepository.UpdateAsync(user, cancellationToken);

        await UpsertReaderProfileFromRequestAsync(readerRequest, user, cancellationToken);

        readerRequest.Status = ReaderApprovalStatus.Approved;
        readerRequest.AdminNote = domainEvent.AdminNote;
        readerRequest.ReviewedBy = domainEvent.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }

    private async Task RejectAsync(
        ReaderRequestDto readerRequest,
        User user,
        ReaderRequestReviewRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        readerRequest.Status = ReaderApprovalStatus.Rejected;
        readerRequest.AdminNote = domainEvent.AdminNote;
        readerRequest.ReviewedBy = domainEvent.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }

    private async Task UpsertReaderProfileFromRequestAsync(
        ReaderRequestDto readerRequest,
        User user,
        CancellationToken cancellationToken)
    {
        var existing = await _readerProfileRepository.GetByUserIdAsync(readerRequest.UserId, cancellationToken);
        if (existing is null)
        {
            var profile = new ReaderProfileDto
            {
                UserId = readerRequest.UserId,
                Status = ReaderOnlineStatus.Offline,
                DiamondPerQuestion = readerRequest.DiamondPerQuestion,
                BioVi = readerRequest.Bio,
                BioEn = string.Empty,
                BioZh = string.Empty,
                Specialties = readerRequest.Specialties,
                YearsOfExperience = readerRequest.YearsOfExperience,
                FacebookUrl = readerRequest.FacebookUrl,
                InstagramUrl = readerRequest.InstagramUrl,
                TikTokUrl = readerRequest.TikTokUrl,
                DisplayName = user.DisplayName,
                AvatarUrl = user.AvatarUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _readerProfileRepository.AddAsync(profile, cancellationToken);
            return;
        }

        existing.DiamondPerQuestion = readerRequest.DiamondPerQuestion;
        existing.BioVi = readerRequest.Bio;
        existing.Specialties = readerRequest.Specialties;
        existing.YearsOfExperience = readerRequest.YearsOfExperience;
        existing.FacebookUrl = readerRequest.FacebookUrl;
        existing.InstagramUrl = readerRequest.InstagramUrl;
        existing.TikTokUrl = readerRequest.TikTokUrl;
        existing.DisplayName = user.DisplayName;
        existing.AvatarUrl = user.AvatarUrl;

        await _readerProfileRepository.UpdateAsync(existing, cancellationToken);
    }

    private static void EnsurePendingRequest(ReaderRequestDto request)
    {
        if (request.Status != ReaderApprovalStatus.Pending)
        {
            throw new BadRequestException($"Đơn này đã được xử lý ({request.Status}).");
        }
    }

    private static Guid ParseUserId(string userId)
    {
        if (!Guid.TryParse(userId, out var parsed))
        {
            throw new BadRequestException("Reader request chứa UserId không hợp lệ.");
        }

        return parsed;
    }

    private static void EnsureRequestHasMandatoryFieldsForApproval(ReaderRequestDto request)
    {
        if (ReaderSpecialties.NormalizeDistinct(request.Specialties).Count == 0)
        {
            throw new BadRequestException("Đơn đăng ký thiếu chuyên môn hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (request.YearsOfExperience < 1)
        {
            throw new BadRequestException("Đơn đăng ký thiếu số năm kinh nghiệm hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (request.DiamondPerQuestion < 50)
        {
            throw new BadRequestException("Đơn đăng ký thiếu giá dịch vụ hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (!ReaderSocialUrlValidator.HasAtLeastOneSocialLink(request.FacebookUrl, request.InstagramUrl, request.TikTokUrl))
        {
            throw new BadRequestException("Đơn đăng ký thiếu link mạng xã hội. Vui lòng từ chối và yêu cầu user nộp lại.");
        }

        if (!ReaderSocialUrlValidator.IsValidFacebookUrl(request.FacebookUrl)
            || !ReaderSocialUrlValidator.IsValidInstagramUrl(request.InstagramUrl)
            || !ReaderSocialUrlValidator.IsValidTikTokUrl(request.TikTokUrl))
        {
            throw new BadRequestException("Đơn đăng ký chứa link mạng xã hội không hợp lệ. Vui lòng từ chối và yêu cầu user nộp lại.");
        }
    }
}

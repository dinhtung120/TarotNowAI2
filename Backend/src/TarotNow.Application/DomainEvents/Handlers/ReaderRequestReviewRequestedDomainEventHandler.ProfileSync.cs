using TarotNow.Application.Common;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ReaderRequestReviewRequestedDomainEventHandler
{
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
}

using MediatR;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

// Query lấy danh sách achievement và trạng thái unlock của user.
public record GetUserAchievementsQuery(Guid UserId) : IRequest<UserAchievementsDto>;

// Handler truy vấn user achievements.
public class GetUserAchievementsQueryHandler : IRequestHandler<GetUserAchievementsQuery, UserAchievementsDto>
{
    private readonly IAchievementRepository _achRepo;

    /// <summary>
    /// Khởi tạo handler get user achievements.
    /// Luồng xử lý: nhận achievement repository để lấy definitions và unlocked list.
    /// </summary>
    public GetUserAchievementsQueryHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    /// <summary>
    /// Xử lý query lấy achievements của user.
    /// Luồng xử lý: tải toàn bộ định nghĩa achievement, tải danh sách achievement user đã unlock, rồi trả DTO tổng hợp.
    /// </summary>
    public async Task<UserAchievementsDto> Handle(GetUserAchievementsQuery request, CancellationToken cancellationToken)
    {
        var definitions = await _achRepo.GetAllAchievementsAsync(cancellationToken);
        var unlocked = await _achRepo.GetUserAchievementsAsync(request.UserId, cancellationToken);
        return new UserAchievementsDto { Definitions = definitions, UnlockedList = unlocked };
    }
}

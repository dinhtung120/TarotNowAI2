/*
 * ===================================================================
 * FILE: GetReaderProfileQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Queries.GetReaderProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lệnh Thực thi: Kéo Dữ Liệu Hồ Sơ Thầy Bói Từ MongoDB.
 *   Nếu gọi trúng ID tào lao (Không phải Thầy Bói) -> Hàm sẽ văng Null 
 *   Và Frontend sẽ hiển thị Lỗi 404 Cảnh Cáo Khách nạp nhầm người.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

public class GetReaderProfileQueryHandler : IRequestHandler<GetReaderProfileQuery, ReaderProfileDto?>
{
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    public GetReaderProfileQueryHandler(
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    public async Task<ReaderProfileDto?> Handle(GetReaderProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        // Enrichment: Bổ sung AvatarUrl từ PostgreSQL khi MongoDB chưa được đồng bộ
        if (profile != null && string.IsNullOrEmpty(profile.AvatarUrl) && Guid.TryParse(profile.UserId, out var userGuid))
        {
            var userInfoMap = await _userRepository.GetUserBasicInfoMapAsync(new[] { userGuid }, cancellationToken);
            if (userInfoMap.TryGetValue(userGuid, out var info) && !string.IsNullOrEmpty(info.AvatarUrl))
            {
                profile.AvatarUrl = info.AvatarUrl;
            }
        }

        return profile;
    }
}

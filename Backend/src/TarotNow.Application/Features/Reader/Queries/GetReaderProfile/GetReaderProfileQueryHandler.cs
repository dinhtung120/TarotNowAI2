using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

// Handler lấy hồ sơ Reader và bổ sung avatar khi thiếu.
public class GetReaderProfileQueryHandler : IRequestHandler<GetReaderProfileQuery, ReaderProfileDto?>
{
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler lấy reader profile.
    /// Luồng xử lý: nhận reader profile repository để tải hồ sơ chính và user repository để enrich avatar fallback khi cần.
    /// </summary>
    public GetReaderProfileQueryHandler(
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý query lấy hồ sơ reader.
    /// Luồng xử lý: đọc profile theo user id, nếu thiếu avatar thì lấy thêm từ user basic info map để hoàn thiện dữ liệu hiển thị.
    /// </summary>
    public async Task<ReaderProfileDto?> Handle(GetReaderProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (profile is not null &&
            string.IsNullOrEmpty(profile.AvatarUrl) &&
            Guid.TryParse(profile.UserId, out var userGuid))
        {
            // Edge case: profile tồn tại nhưng chưa có avatar; fallback sang nguồn user profile để cải thiện UI.
            var userInfoMap = await _userRepository.GetUserBasicInfoMapAsync(new[] { userGuid }, cancellationToken);

            if (userInfoMap.TryGetValue(userGuid, out var info) && !string.IsNullOrEmpty(info.AvatarUrl))
            {
                profile.AvatarUrl = info.AvatarUrl;
                // Cập nhật dữ liệu trả về tại runtime để tránh ô avatar trống ở trang chi tiết reader.
            }
        }

        return profile;
    }
}

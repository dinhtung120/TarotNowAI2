using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.ListReaders;

// Handler truy vấn danh sách Reader theo bộ lọc.
public class ListReadersQueryHandler : IRequestHandler<ListReadersQuery, ListReadersResult>
{
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler liệt kê reader.
    /// Luồng xử lý: nhận reader profile repository để lấy dữ liệu chính và user repository để enrich avatar khi thiếu.
    /// </summary>
    public ListReadersQueryHandler(
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý query liệt kê reader.
    /// Luồng xử lý: tải danh sách theo phân trang/bộ lọc, bổ sung avatar thiếu từ user info map, rồi trả response tổng hợp.
    /// </summary>
    public async Task<ListReadersResult> Handle(ListReadersQuery request, CancellationToken cancellationToken)
    {
        var (profiles, totalCount) = await _readerProfileRepository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            request.Specialty,
            request.Status,
            request.SearchTerm,
            cancellationToken);

        var profileList = profiles.ToList();
        var needsEnrichment = profileList
            .Where(profile => string.IsNullOrEmpty(profile.AvatarUrl) && !string.IsNullOrEmpty(profile.UserId))
            .ToList();
        // Chỉ enrich các profile thiếu avatar để tránh gọi user repository không cần thiết.

        if (needsEnrichment.Count > 0)
        {
            var userIds = needsEnrichment
                .Select(profile => Guid.TryParse(profile.UserId, out var guid) ? guid : (Guid?)null)
                .Where(guid => guid.HasValue)
                .Select(guid => guid!.Value);

            var userInfoMap = await _userRepository.GetUserBasicInfoMapAsync(userIds, cancellationToken);
            // Tải batch user info để tối ưu hiệu năng thay vì truy vấn từng reader.

            foreach (var profile in needsEnrichment)
            {
                if (Guid.TryParse(profile.UserId, out var guid) &&
                    userInfoMap.TryGetValue(guid, out var info) &&
                    !string.IsNullOrEmpty(info.AvatarUrl))
                {
                    profile.AvatarUrl = info.AvatarUrl;
                    // Gán avatar fallback vào profile kết quả để UI danh sách không bị thiếu ảnh đại diện.
                }
            }
            // Kết thúc vòng lặp enrich, danh sách reader đã có dữ liệu avatar đầy đủ hơn cho hiển thị.
        }

        return new ListReadersResult
        {
            Readers = profileList,
            TotalCount = totalCount
        };
    }
}

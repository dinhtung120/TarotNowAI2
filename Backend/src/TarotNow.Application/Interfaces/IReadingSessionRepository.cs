

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract dữ liệu phiên đọc bài để theo dõi lịch sử và trạng thái xử lý AI theo phiên.
public interface IReadingSessionRepository
{
    /// <summary>
    /// Tạo phiên đọc bài mới khi người dùng bắt đầu một lượt trải bài.
    /// Luồng xử lý: persist entity session và trả bản ghi đã tạo.
    /// </summary>
    Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy phiên đọc bài theo id để xử lý chi tiết hoặc tiếp tục luồng đang dở.
    /// Luồng xử lý: truy vấn theo session id và trả null nếu không tồn tại.
    /// </summary>
    Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật phiên đọc bài khi trạng thái, kết quả hoặc metadata thay đổi.
    /// Luồng xử lý: ghi đè các trường đã cập nhật của entity session.
    /// </summary>
    Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra người dùng đã rút lá daily card trong ngày hay chưa để chặn lặp.
    /// Luồng xử lý: đối chiếu userId với mốc utcNow theo quy tắc ngày nghiệp vụ.
    /// </summary>
    Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lịch sử phiên đọc bài của người dùng có phân trang và bộ lọc.
    /// Luồng xử lý: lọc theo userId, spreadType, date, áp page/pageSize và trả items cùng tổng số.
    /// </summary>
    Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId, 
        int page, 
        int pageSize, 
        string? spreadType = null,
        DateTime? date = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy phiên đọc bài kèm các AI request liên quan để tra cứu luồng sinh nội dung.
    /// Luồng xử lý: truy vấn sessionId và join dữ liệu request; trả null nếu session không tồn tại.
    /// </summary>
    Task<(ReadingSession ReadingSession, IEnumerable<TarotNow.Domain.Entities.AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ phiên đọc bài có phân trang và bộ lọc quản trị.
    /// Luồng xử lý: áp filter userIds/spreadType/date-range, phân trang và trả items + total count.
    /// </summary>
    Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page,
        int pageSize,
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}

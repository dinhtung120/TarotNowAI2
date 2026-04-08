using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

// Contract bảng xếp hạng gamification để lưu snapshot và điểm theo kỳ.
public interface ILeaderboardRepository
{
    /// <summary>
    /// Lấy snapshot leaderboard theo track và period để hiển thị trạng thái đã chốt.
    /// Luồng xử lý: truy vấn đúng scoreTrack/periodKey và trả null nếu chưa có snapshot.
    /// </summary>
    Task<LeaderboardSnapshotDto?> GetSnapshotAsync(string scoreTrack, string periodKey, CancellationToken ct);

    /// <summary>
    /// Tạo hoặc cập nhật snapshot leaderboard nhằm lưu trạng thái điểm tại thời điểm tổng hợp.
    /// Luồng xử lý: ghi đè snapshot theo khóa track/kỳ và lưu dữ liệu mới nhất.
    /// </summary>
    Task UpsertSnapshotAsync(LeaderboardSnapshotDto snapshot, CancellationToken ct);

    /// <summary>
    /// Cộng điểm cho người dùng theo track/kỳ để cập nhật thứ hạng động.
    /// Luồng xử lý: xác định bucket điểm theo scoreTrack/periodKey rồi tăng points tương ứng.
    /// </summary>
    Task IncrementScoreAsync(Guid userId, string scoreTrack, string periodKey, long points, CancellationToken ct);

    /// <summary>
    /// Lấy top leaderboard theo giới hạn để phục vụ màn hình bảng xếp hạng.
    /// Luồng xử lý: truy vấn theo track/kỳ, sắp xếp giảm dần điểm và lấy tối đa limit bản ghi.
    /// </summary>
    Task<List<LeaderboardEntryDto>> GetTopEntriesAsync(string scoreTrack, string periodKey, int limit, CancellationToken ct);

    /// <summary>
    /// Lấy thứ hạng của một người dùng để hiển thị vị trí cá nhân trong leaderboard.
    /// Luồng xử lý: tính rank theo userId trong scoreTrack/periodKey và trả null nếu chưa có điểm.
    /// </summary>
    Task<LeaderboardEntryDto?> GetUserRankAsync(Guid userId, string scoreTrack, string periodKey, CancellationToken ct);

    /// <summary>
    /// Reset toàn bộ điểm của một track/kỳ để bắt đầu chu kỳ mới.
    /// Luồng xử lý: xóa hoặc đưa về 0 dữ liệu điểm thuộc scoreTrack/periodKey mục tiêu.
    /// </summary>
    Task ResetScoreTrackAsync(string scoreTrack, string periodKey, CancellationToken ct);
}

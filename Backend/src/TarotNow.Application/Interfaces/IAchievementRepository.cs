using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

// Contract truy cập dữ liệu thành tựu để tách business logic khỏi tầng lưu trữ.
public interface IAchievementRepository
{
    /// <summary>
    /// Lấy toàn bộ định nghĩa thành tựu để dùng cho cấu hình hiển thị và đối soát điều kiện mở khóa.
    /// Luồng xử lý: đọc danh mục chuẩn từ kho dữ liệu và trả về danh sách đầy đủ cho tầng nghiệp vụ.
    /// </summary>
    Task<List<AchievementDefinitionDto>> GetAllAchievementsAsync(CancellationToken ct);

    /// <summary>
    /// Tìm định nghĩa thành tựu theo mã để kiểm tra tính hợp lệ trước khi thao tác.
    /// Luồng xử lý: truy vấn theo achievementCode và trả về null khi không tồn tại.
    /// </summary>
    Task<AchievementDefinitionDto?> GetByCodeAsync(string achievementCode, CancellationToken ct);

    /// <summary>
    /// Tạo mới hoặc cập nhật định nghĩa thành tựu nhằm đồng bộ metadata trong hệ thống.
    /// Luồng xử lý: nhận DTO định nghĩa, ghi đè theo mã và lưu lại bản mới nhất.
    /// </summary>
    Task UpsertAchievementDefinitionAsync(AchievementDefinitionDto achievement, CancellationToken ct);

    /// <summary>
    /// Xóa định nghĩa thành tựu theo mã khi không còn áp dụng trong nghiệp vụ.
    /// Luồng xử lý: định vị record theo achievementCode và loại bỏ khỏi kho dữ liệu.
    /// </summary>
    Task DeleteAchievementDefinitionAsync(string achievementCode, CancellationToken ct);

    /// <summary>
    /// Lấy danh sách thành tựu của người dùng để dựng hồ sơ tiến độ cá nhân.
    /// Luồng xử lý: lọc theo userId và trả về các thành tựu đã ghi nhận.
    /// </summary>
    Task<List<UserAchievementDto>> GetUserAchievementsAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Kiểm tra người dùng đã mở khóa thành tựu hay chưa để tránh cấp trùng.
    /// Luồng xử lý: đối chiếu theo cặp userId và achievementCode, trả về true nếu đã tồn tại.
    /// </summary>
    Task<bool> HasUnlockedAsync(Guid userId, string achievementCode, CancellationToken ct);

    /// <summary>
    /// Ghi nhận mở khóa thành tựu cho người dùng khi đủ điều kiện nghiệp vụ.
    /// Luồng xử lý: tạo liên kết user-achievement mới và lưu trạng thái đã mở khóa.
    /// </summary>
    Task UnlockAsync(Guid userId, string achievementCode, CancellationToken ct);
}

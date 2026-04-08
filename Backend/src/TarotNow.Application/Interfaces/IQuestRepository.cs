using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;

namespace TarotNow.Application.Interfaces;

// Contract dữ liệu nhiệm vụ để quản lý định nghĩa quest và tiến độ theo kỳ.
public interface IQuestRepository
{
    /// <summary>
    /// Lấy các quest đang active theo loại để hiển thị danh sách nhiệm vụ khả dụng.
    /// Luồng xử lý: lọc theo questType và trạng thái kích hoạt, trả danh sách định nghĩa quest.
    /// </summary>
    Task<List<QuestDefinitionDto>> GetActiveQuestsAsync(string questType, CancellationToken ct);

    /// <summary>
    /// Lấy định nghĩa quest theo mã để kiểm tra điều kiện và phần thưởng.
    /// Luồng xử lý: truy vấn theo questCode và trả null nếu quest không tồn tại.
    /// </summary>
    Task<QuestDefinitionDto?> GetQuestByCodeAsync(string questCode, CancellationToken ct);

    /// <summary>
    /// Lấy toàn bộ quest để phục vụ quản trị cấu hình nhiệm vụ.
    /// Luồng xử lý: đọc tất cả bản ghi quest trong hệ thống và trả danh sách kết quả.
    /// </summary>
    Task<List<QuestDefinitionDto>> GetAllQuestsAsync(CancellationToken ct);

    /// <summary>
    /// Tạo mới hoặc cập nhật định nghĩa quest để đồng bộ rule nhiệm vụ.
    /// Luồng xử lý: upsert theo quest code và lưu metadata mới nhất.
    /// </summary>
    Task UpsertQuestDefinitionAsync(QuestDefinitionDto quest, CancellationToken ct);

    /// <summary>
    /// Xóa định nghĩa quest khi nhiệm vụ không còn được áp dụng.
    /// Luồng xử lý: định vị quest theo questCode và loại bỏ khỏi kho dữ liệu.
    /// </summary>
    Task DeleteQuestDefinitionAsync(string questCode, CancellationToken ct);

    /// <summary>
    /// Lấy tiến độ quest cụ thể của người dùng theo kỳ để quyết định mở khóa thưởng.
    /// Luồng xử lý: truy vấn theo userId/questCode/periodKey và trả null nếu chưa có tiến độ.
    /// </summary>
    Task<QuestProgressDto?> GetProgressAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);

    /// <summary>
    /// Lấy toàn bộ tiến độ quest của người dùng trong một loại và một kỳ.
    /// Luồng xử lý: lọc theo userId/questType/periodKey và trả danh sách progress.
    /// </summary>
    Task<List<QuestProgressDto>> GetAllProgressAsync(Guid userId, string questType, string periodKey, CancellationToken ct);

    /// <summary>
    /// Tạo mới hoặc cập nhật tiến độ quest để phản ánh hành vi người dùng vừa thực hiện.
    /// Luồng xử lý: upsert progress theo khóa người dùng + quest + kỳ với số tăng tương ứng.
    /// </summary>
    Task UpsertProgressAsync(QuestProgressUpsertRequest request, CancellationToken ct);

    /// <summary>
    /// Đánh dấu quest đã claim thưởng để ngăn nhận thưởng lặp.
    /// Luồng xử lý: cập nhật trạng thái claimed theo userId/questCode/periodKey.
    /// </summary>
    Task MarkClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);

    /// <summary>
    /// Thử đánh dấu claimed theo cơ chế an toàn cạnh tranh để tránh double-claim.
    /// Luồng xử lý: cập nhật có điều kiện, trả true nếu thao tác ghi trạng thái thành công.
    /// </summary>
    Task<bool> TryMarkClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);

    /// <summary>
    /// Hoàn tác trạng thái claimed khi cần rollback do lỗi nghiệp vụ downstream.
    /// Luồng xử lý: cập nhật lại cờ claimed theo userId/questCode/periodKey về trạng thái chưa nhận.
    /// </summary>
    Task RevertClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct);
}

// Request upsert tiến độ quest để chuẩn hóa đầu vào thao tác cộng tiến độ.
public sealed record QuestProgressUpsertRequest(
    Guid UserId,
    string QuestCode,
    string PeriodKey,
    int Target,
    int IncrementBy);

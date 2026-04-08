

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract dữ liệu gacha để xử lý banner, pity và log phần thưởng một cách nhất quán.
public interface IGachaRepository
{
    /// <summary>
    /// Lấy banner active theo mã để xác định cấu hình quay hiện hành.
    /// Luồng xử lý: tra cứu bannerCode trong nhóm banner khả dụng và trả null nếu không active.
    /// </summary>
    Task<GachaBanner?> GetActiveBannerAsync(string bannerCode, CancellationToken ct);

    /// <summary>
    /// Lấy toàn bộ banner đang hoạt động để hiển thị danh sách quay cho người dùng.
    /// Luồng xử lý: lọc các banner active hiện tại và trả về danh sách kết quả.
    /// </summary>
    Task<List<GachaBanner>> GetAllActiveBannersAsync(CancellationToken ct);

    /// <summary>
    /// Lấy danh sách item trong banner để phục vụ thuật toán chọn thưởng.
    /// Luồng xử lý: truy vấn toàn bộ item theo bannerId và trả tập phần thưởng cấu hình.
    /// </summary>
    Task<List<GachaBannerItem>> GetBannerItemsAsync(Guid bannerId, CancellationToken ct);

    /// <summary>
    /// Lấy pity count của người dùng theo banner để áp dụng rule bảo hiểm phần thưởng.
    /// Luồng xử lý: đếm số lượt chưa trúng mốc pity theo userId/bannerId.
    /// </summary>
    Task<int> GetUserPityCountAsync(Guid userId, Guid bannerId, CancellationToken ct);

    /// <summary>
    /// Ghi log phần thưởng sau khi quay để cố định kết quả và phục vụ truy vết.
    /// Luồng xử lý: persist entity reward log và trả bản ghi đã lưu.
    /// </summary>
    Task<GachaRewardLog> LogRewardAsync(GachaRewardLog log, CancellationToken ct);

    /// <summary>
    /// Kiểm tra idempotency key đã tồn tại hay chưa để chặn ghi trùng lần quay.
    /// Luồng xử lý: truy vấn theo key duy nhất và trả true khi đã có bản ghi.
    /// </summary>
    Task<bool> IdempotencyKeyExistsAsync(string key, CancellationToken ct);

    /// <summary>
    /// Lấy các log phần thưởng theo idempotency key để hoàn nguyên kết quả khi retry request.
    /// Luồng xử lý: lọc log theo key và trả toàn bộ bản ghi đã ghi trước đó.
    /// </summary>
    Task<List<GachaRewardLog>> GetRewardLogsByIdempotencyKeyAsync(string key, CancellationToken ct);
}



using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract lưu trữ consent pháp lý của người dùng để truy vết chấp thuận theo phiên bản tài liệu.
public interface IUserConsentRepository
{
    /// <summary>
    /// Lấy consent cụ thể theo loại tài liệu và phiên bản để kiểm tra đã chấp thuận hay chưa.
    /// Luồng xử lý: truy vấn theo userId/documentType/version và trả null nếu chưa có bản ghi.
    /// </summary>
    Task<UserConsent?> GetConsentAsync(Guid userId, string documentType, string version, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ consent của người dùng để hiển thị lịch sử chấp thuận.
    /// Luồng xử lý: lọc theo userId và trả danh sách record consent liên quan.
    /// </summary>
    Task<IEnumerable<UserConsent>> GetUserConsentsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm bản ghi consent mới khi người dùng đồng ý một tài liệu pháp lý.
    /// Luồng xử lý: persist entity consent vào nguồn dữ liệu phục vụ audit.
    /// </summary>
    Task AddAsync(UserConsent consent, CancellationToken cancellationToken = default);
}

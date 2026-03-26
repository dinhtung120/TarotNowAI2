/*
 * ===================================================================
 * FILE: ApproveReaderCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.ApproveReader
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler XỬ LÝ LOGIC phê duyệt/từ chối đơn xin làm Reader.
 *   Đây là file PHỨC TẠP NHẤT trong Admin features vì:
 *   1. Thao tác CROSS-SYSTEM: MongoDB + PostgreSQL cùng lúc
 *   2. Có COMPENSATION PATTERN (hoàn tác) khi lỗi
 *
 * COMPENSATION PATTERN:
 *   Vì MongoDB và PostgreSQL là 2 database KHÁC NHAU,
 *   không thể dùng 1 ACID transaction bao trùm cả 2.
 *   Giải pháp: nếu bước sau lỗi → thủ công hoàn tác bước trước.
 *   Đây là pattern "Saga" đơn giản hóa.
 *
 *   Ví dụ:
 *   ✅ Step 1: Update user role (PostgreSQL)
 *   ✅ Step 2: Create reader profile (MongoDB)
 *   ❌ Step 3: Update request status (MongoDB) → LỖI!
 *   → Compensation: undo Step 1 (restore role) + undo Step 2 (delete profile)
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

/// <summary>
/// Handler phê duyệt / từ chối đơn xin Reader.
/// Cross-system: thao tác trên CẢ MongoDB VÀ PostgreSQL.
/// </summary>
public partial class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
{
    /*
     * 3 Repositories cho 3 nguồn dữ liệu:
     * _readerRequestRepository: MongoDB collection "reader_requests"
     * _readerProfileRepository: MongoDB collection "reader_profiles"
     * _userRepository: PostgreSQL table "users"
     */
    private readonly IReaderRequestRepository _readerRequestRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    public ApproveReaderCommandHandler(
        IReaderRequestRepository readerRequestRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerRequestRepository = readerRequestRepository;
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ApproveReaderCommand request, CancellationToken cancellationToken)
    {
        // Bước 1: Validate action (chỉ chấp nhận "approve" hoặc "reject")
        var action = request.Action.ToLowerInvariant();
        if (action != "approve" && action != "reject")
            throw new BadRequestException("Action không hợp lệ. Chỉ chấp nhận: approve, reject.");

        // Bước 2: Lấy reader request từ MongoDB
        /*
         * "?? throw new NotFoundException(...)": null-coalescing throw.
         * Nếu GetByIdAsync trả null → throw exception ngay.
         * Cú pháp ngắn gọn thay vì:
         *   var rr = await _repo.GetByIdAsync(...);
         *   if (rr == null) throw new NotFoundException(...);
         */
        var readerRequest = await _readerRequestRepository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");

        // Bước 3: Kiểm tra đơn phải đang "pending" (chưa xử lý)
        // Nếu đã approved/rejected → không được xử lý lại
        if (readerRequest.Status != ReaderApprovalStatus.Pending)
            throw new BadRequestException($"Đơn này đã được xử lý ({readerRequest.Status}).");

        // Bước 4: Lấy user từ PostgreSQL
        if (!Guid.TryParse(readerRequest.UserId, out var userId))
            throw new BadRequestException("Reader request chứa UserId không hợp lệ.");

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        // Bước 5: Thực hiện hành động
        if (action == "approve")
        {
            // Approve flow phức tạp → tách riêng method (có compensation)
            await HandleApproveFlowAsync(request, readerRequest, user, cancellationToken);
            return true;
        }

        // === REJECT FLOW (đơn giản hơn) ===
        /*
         * user.RejectReaderRequest(): Domain method cập nhật user entity.
         * Giữ nguyên role = "user", đánh dấu đã bị reject.
         * Domain layer quản lý business logic → handler chỉ gọi method.
         */
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Cập nhật MongoDB: đánh dấu đơn bị reject
        readerRequest.Status = ReaderApprovalStatus.Rejected;

        // Ghi audit trail (ai xử lý, lúc nào, ghi chú gì)
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;

        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);

        return true;
    }
}

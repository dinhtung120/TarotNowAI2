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
public class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
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

    /// <summary>
    /// APPROVE FLOW: Phức tạp vì thao tác trên nhiều hệ thống.
    /// Có TRY-CATCH với COMPENSATION (hoàn tác) nếu lỗi.
    ///
    /// Các bước:
    ///   Step 1: Promote user trong PostgreSQL (role → tarot_reader)
    ///   Step 2: Tạo reader profile trong MongoDB (nếu chưa có)
    ///   Step 3: Đóng đơn request trong MongoDB (status → approved)
    ///
    /// Nếu bất kỳ bước nào lỗi → compensation hoàn tác tất cả bước trước.
    /// </summary>
    private async Task HandleApproveFlowAsync(
        ApproveReaderCommand request,
        ReaderRequestDto readerRequest,
        Domain.Entities.User user,
        CancellationToken cancellationToken)
    {
        /*
         * Lưu trạng thái GỐC trước khi thay đổi.
         * Nếu cần hoàn tác → restore về trạng thái này.
         */
        var originalRole = user.Role;
        var originalReaderStatus = user.ReaderStatus;
        var profileCreated = false; // Flag theo dõi: đã tạo profile chưa?

        try
        {
            // Step 1: Promote user → thay đổi role + reader status trong PostgreSQL
            user.ApproveAsReader(); // Domain method: set Role = "tarot_reader"
            await _userRepository.UpdateAsync(user, cancellationToken);

            // Step 2: Tạo reader profile trong MongoDB (nếu chưa có)
            var existingProfile = await _readerProfileRepository.GetByUserIdAsync(
                readerRequest.UserId, cancellationToken);

            if (existingProfile == null)
            {
                /*
                 * Tạo profile mới với giá trị mặc định:
                 *   Status = Offline (reader mới chưa online)
                 *   DiamondPerQuestion = 5 (giá mặc định)
                 *   BioVi = lời giới thiệu từ đơn xin (introText)
                 *   DisplayName/AvatarUrl = copy từ user (denormalized)
                 */
                var profile = new ReaderProfileDto
                {
                    UserId = readerRequest.UserId,
                    Status = ReaderOnlineStatus.Offline,
                    DiamondPerQuestion = 5, // Giá mặc định 5 diamond/câu hỏi
                    BioVi = readerRequest.IntroText,
                    BioEn = string.Empty,
                    BioZh = string.Empty,
                    DisplayName = user.DisplayName,
                    AvatarUrl = user.AvatarUrl,
                    CreatedAt = DateTime.UtcNow
                };

                await _readerProfileRepository.AddAsync(profile, cancellationToken);
                profileCreated = true; // Đánh dấu đã tạo (để biết cần xóa nếu lỗi)
            }

            // Step 3: Đóng đơn request → approved + audit trail
            readerRequest.Status = ReaderApprovalStatus.Approved;
            readerRequest.AdminNote = request.AdminNote;
            readerRequest.ReviewedBy = request.AdminId.ToString();
            readerRequest.ReviewedAt = DateTime.UtcNow;
            await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            /*
             * COMPENSATION (HOÀN TÁC):
             * Nếu bất kỳ step nào lỗi → hoàn tác tất cả bước đã hoàn thành.
             * Đây là pattern "Saga" đơn giản cho distributed transaction.
             * 
             * Truyền vào:
             *   - user: entity cần restore role
             *   - originalRole/Status: giá trị gốc
             *   - profileCreated: có cần xóa profile không
             */
            await CompensateApproveFailureAsync(
                user,
                originalRole,
                originalReaderStatus,
                readerRequest.UserId,
                profileCreated,
                cancellationToken);

            // Throw lại exception (bọc trong InvalidOperationException)
            // để caller biết approve thất bại
            throw new InvalidOperationException("Approve reader failed and was rolled back.", ex);
        }
    }

    /// <summary>
    /// COMPENSATION METHOD: Hoàn tác khi approve flow bị lỗi.
    ///
    /// CÁCH TIẾP CẬN "BEST EFFORT":
    ///   Cố gắng hoàn tác từng bước. Nếu hoàn tác cũng lỗi:
    ///   - Ghi nhận lỗi vào danh sách
    ///   - Tiếp tục hoàn tác các bước khác
    ///   - Cuối cùng throw AggregateException chứa TẤT CẢ lỗi
    ///   → Admin phải xử lý thủ công nếu compensation cũng thất bại.
    /// </summary>
    private async Task CompensateApproveFailureAsync(
        Domain.Entities.User user,
        string originalRole,
        string originalReaderStatus,
        string userId,
        bool profileCreated,
        CancellationToken cancellationToken)
    {
        List<Exception>? compensationErrors = null;

        // Hoàn tác Step 1: Restore role và reader status về giá trị gốc
        try
        {
            user.RestoreRoleAndReaderStatus(originalRole, originalReaderStatus);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            /*
             * "??=" (null-coalescing assignment):
             * Nếu compensationErrors == null → tạo mới List.
             * Nếu đã có → giữ nguyên.
             * Cú pháp ngắn gọn thay vì: if (list == null) list = new List();
             */
            compensationErrors ??= new List<Exception>();
            compensationErrors.Add(ex);
        }

        // Hoàn tác Step 2: Xóa reader profile (nếu đã tạo)
        if (profileCreated)
        {
            try
            {
                await _readerProfileRepository.DeleteByUserIdAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                compensationErrors ??= new List<Exception>();
                compensationErrors.Add(ex);
            }
        }

        /*
         * Nếu có lỗi compensation → throw AggregateException.
         * AggregateException: chứa NHIỀU exception bên trong.
         * "is { Count: > 0 }": pattern matching kiểm tra list không null VÀ có phần tử.
         */
        if (compensationErrors is { Count: > 0 })
            throw new AggregateException("Compensation failed after approve reader error.", compensationErrors);
    }
}

/*
 * ===================================================================
 * FILE: WithdrawalController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý RÚT TIỀN (Withdrawal) cho Reader.
 *   Reader kiếm diamond từ việc trả lời câu hỏi → rút về VND qua ngân hàng.
 *
 * LUỒNG RÚT TIỀN:
 *   1. Reader tạo yêu cầu rút (POST /create) → kiểm tra đủ điều kiện → trừ tiền
 *   2. Admin xem hàng đợi rút tiền (GET /admin/withdrawals/queue)
 *   3. Admin duyệt/từ chối (POST /admin/withdrawals/process)
 *   4. Nếu duyệt → chuyển khoản thật cho reader
 *   5. Nếu từ chối → hoàn lại diamond
 *
 * CÁC RÀNG BUỘC:
 *   - Tối thiểu 50 diamond mỗi lần rút
 *   - Tối đa 1 yêu cầu rút/ngày (chống spam)
 *   - Phải có KYC approved (xác minh danh tính)
 *   - Đủ số dư (trừ phần đóng băng escrow)
 *   - Phí rút: 10% (trừ trước khi tính số tiền nhận)
 *   - Yêu cầu MFA code (xác thực 2 lớp)
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý rút tiền (Withdrawal).
/// Reader tạo yêu cầu rút → Admin duyệt → Chuyển khoản thật.
/// 
/// PHÂN QUYỀN:
///   - POST /create: reader (user đã được duyệt)
///   - GET /my: reader xem lịch sử rút của mình
///   - Admin endpoints: ở AdminController
/// </summary>
[Route(ApiRoutes.Withdrawal)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] // Tất cả endpoint đều cần đăng nhập
public class WithdrawalController : ControllerBase
{
    private readonly IMediator _mediator;

    // Constructor expression body
    public WithdrawalController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Helper: lấy UserId từ JWT token.
    /// </summary>
    private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/withdrawal/create
    /// MỤC ĐÍCH: Reader tạo yêu cầu rút tiền.
    ///
    /// CÁC GUARD (KIỂM TRA) TRONG HANDLER:
    ///   1. Số tiền ≥ 50 diamond
    ///   2. Chưa có yêu cầu rút nào đang pending trong ngày (1/ngày)
    ///   3. User đã KYC (xác minh danh tính)
    ///   4. Số dư khả dụng ≥ số tiền rút + phí
    ///   5. MFA code hợp lệ (xác thực 2 lớp)
    ///
    /// KHI TẠO THÀNH CÔNG:
    ///   - TRỪ TIỀN NGAY (debit) → tránh rút trùng
    ///   - Tạo bản ghi WithdrawalRequest (trạng thái: pending)
    ///   - Phí 10% tính trước: rút 100 diamond → nhận 90, phí 10
    ///
    /// TẠI SAO TRỪ TIỀN NGAY?
    ///   Nếu đợi admin duyệt mới trừ → reader có thể tạo nhiều yêu cầu
    ///   cùng lúc, tổng vượt quá số dư → double-spend attack.
    ///   Trừ ngay = "đặt cọc" → nếu bị từ chối sẽ hoàn lại.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateWithdrawalBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new CreateWithdrawalCommand
        {
            UserId = userId.Value,                       // Reader rút tiền (từ JWT)
            AmountDiamond = body.AmountDiamond,           // Số diamond muốn rút
            BankName = body.BankName,                     // Tên ngân hàng (VD: Vietcombank)
            BankAccountName = body.BankAccountName,       // Tên chủ tài khoản
            BankAccountNumber = body.BankAccountNumber,   // Số tài khoản ngân hàng
            MfaCode = body.MfaCode                        // Mã xác thực 2 lớp
        };

        // Handler trả về requestId (UUID) của yêu cầu rút tiền
        var requestId = await _mediator.Send(command);
        return Ok(new { success = true, requestId });
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/withdrawal/my?page=1&amp;pageSize=20
    /// MỤC ĐÍCH: Reader xem lịch sử rút tiền của mình.
    ///
    /// PendingOnly = false nghĩa là lấy TẤT CẢ (pending + approved + rejected).
    /// Reader cần xem cả lịch sử để biết đơn nào đã xử lý, đơn nào đang chờ.
    ///
    /// BẢO MẬT: UserId từ JWT → reader chỉ xem được đơn CỦA MÌNH.
    /// </summary>
    [HttpGet("my")]
    public async Task<IActionResult> MyWithdrawals([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var query = new ListWithdrawalsQuery
        {
            UserId = userId.Value,   // Chỉ lấy đơn của user này
            PendingOnly = false,     // Lấy tất cả trạng thái (không chỉ pending)
            Page = page,
            PageSize = pageSize,
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

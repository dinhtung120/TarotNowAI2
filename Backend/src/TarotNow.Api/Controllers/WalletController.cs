/*
 * ===================================================================
 * FILE: WalletController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý VÍ ĐIỆN TỬ (Wallet) của người dùng.
 *   Cho phép xem SỐ DƯ và LỊCH SỬ GIAO DỊCH.
 *
 * HỆ THỐNG TIỀN TỆ TRONG APP:
 *   - Diamond (💎): Đơn vị tiền ảo chính, dùng để:
 *     + Trả phí đọc bài AI
 *     + Trả cho reader khi đặt câu hỏi
 *     + Reader rút tiền (quy đổi diamond → VND)
 *   - Gold (🥇): Tiền thưởng/ảo, dùng cho tính năng gamification
 *     (ví dụ: điểm danh hàng ngày, hoàn thành nhiệm vụ)
 *
 * LEDGER (SỔ CÁI) LÀ GÌ?
 *   Mỗi giao dịch (nạp tiền, trừ tiền, nhận thưởng) đều được ghi vào sổ cái.
 *   Giống như sổ tay ngân hàng: ghi lại mọi biến động số dư.
 *   Dùng để: user theo dõi chi tiêu, admin đối soát, và audit kiểm toán.
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Import Query cho wallet
using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller xử lý các tác vụ liên quan đến Ví người dùng.
/// Bắt buộc đăng nhập — thông tin ví là dữ liệu nhạy cảm.
/// </summary>
[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] // Bảo mật: chỉ user đã xác thực mới truy cập
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Wallet/balance
    /// MỤC ĐÍCH: Lấy số dư hiện tại của ví người dùng.
    ///
    /// TRẢ VỀ: Đối tượng chứa:
    ///   - Diamond: số diamond khả dụng (đã trừ phần bị escrow đóng băng)
    ///   - Gold: số gold 
    ///   - FrozenDiamond: số diamond đang bị đóng băng trong escrow
    ///
    /// CÁCH GỌI: GET /api/v1/Wallet/balance
    ///   Header: Authorization: Bearer {access_token}
    ///   Không cần tham số — server tự biết user nào nhờ JWT.
    /// </summary>
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        /*
         * new GetWalletBalanceQuery(userId):
         * Constructor nhận userId trực tiếp (thay vì gán property).
         * Đây là cú pháp "primary constructor" hoặc "positional record" trong C#.
         */
        var query = new GetWalletBalanceQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Wallet/ledger?page=1&amp;limit=20
    /// MỤC ĐÍCH: Lấy lịch sử giao dịch ví (Ledger) có phân trang.
    ///
    /// MỖI GIAO DỊCH BAO GỒM:
    ///   - Type: loại giao dịch (deposit, debit, reward, escrow_freeze, escrow_release...)
    ///   - Amount: số tiền (+/-)
    ///   - Currency: loại tiền (diamond/gold)
    ///   - Description: mô tả (ví dụ: "Nạp 100.000 VND", "Phí đọc bài AI")
    ///   - CreatedAt: thời điểm giao dịch
    ///   - Balance: số dư SAU giao dịch
    ///
    /// PHÂN TRANG:
    ///   - page: trang (mặc định 1)
    ///   - limit: số giao dịch mỗi trang (mặc định 20)
    /// </summary>
    [HttpGet("ledger")]
    public async Task<IActionResult> GetLedger([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        // Constructor với 3 tham số: userId, page, limit
        var query = new GetLedgerListQuery(userId, page, limit);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}

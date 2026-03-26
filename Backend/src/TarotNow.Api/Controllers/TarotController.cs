/*
 * ===================================================================
 * FILE: TarotController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller xử lý TÍNH NĂNG CỐT LÕI của ứng dụng: 
 *   RÚT BÀI TAROT VÀ XEM KẾT QUẢ.
 *
 * LUỒNG ĐỌC BÀI TAROT (3 BƯỚC):
 *   ┌────────────┐    ┌────────────┐    ┌────────────┐
 *   │ BƯỚC 1:    │    │ BƯỚC 2:    │    │ BƯỚC 3:    │
 *   │ Init       │───→│ Reveal     │───→│ AI Stream  │
 *   │ (Tạo phiên)│    │ (Lật bài)  │    │ (Giải nghĩa)
 *   └────────────┘    └────────────┘    └────────────┘
 *   
 *   BƯỚC 1 - Init: Tạo phiên đọc bài, trừ tiền, tạo seed ngẫu nhiên
 *   BƯỚC 2 - Reveal: Lật bài từ phiên, xác định bài nào được rút, cộng EXP
 *   BƯỚC 3 - AI Stream: Gọi AI phân tích (ở AiController.cs)
 *   
 *   Ngoài ra có tính năng Collection: xem bộ sưu tập lá bài đã từng rút.
 *
 * RNG LÀ GÌ?
 *   RNG = Random Number Generator (bộ sinh số ngẫu nhiên).
 *   Khi rút bài tarot, server tạo "seed" (hạt giống) ngẫu nhiên.
 *   Seed này quyết định bài nào sẽ được rút.
 *   Seed được tạo ở Bước 1 nhưng chỉ "chốt" ở Bước 2 → đảm bảo công bằng.
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reading.Commands.InitSession;

namespace TarotNow.Api.Controllers;

/*
 * [Route(ApiRoutes.Reading)]: URL gốc = /api/v1/reading
 * [Authorize]: Tất cả endpoint yêu cầu đăng nhập.
 *   Rút bài cần trừ tiền → phải biết ai đang rút.
 */
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Reading)]
[Authorize] // Bắt buộc đăng nhập
public class TarotController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarotController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/reading/init
    /// MỤC ĐÍCH: Bước 1 - Khởi tạo phiên rút bài Tarot.
    ///
    /// CÁCH HOẠT ĐỘNG:
    ///   1. Client gửi: kiểu trải bài (spread type), câu hỏi (question)
    ///   2. Handler kiểm tra user có đủ tiền không
    ///   3. TRỪ TIỀN (diamond) ngay lập tức
    ///   4. Tạo session trong database (trạng thái: initialized)
    ///   5. Tạo Client Seed + Server Seed (cho RNG)
    ///   6. Trả về SessionId + thông tin animation
    ///
    /// CLIENT NHẬN SessionId → chạy animation LẬT BÀI (bước 2 là Reveal)
    ///
    /// TẠI SAO TRỪ TIỀN Ở BƯỚC 1 MÀ KHÔNG PHẢI BƯỚC 2?
    ///   Để ngăn user rút bài → xem bài → nếu không thích → không trả tiền.
    ///   Trừ trước đảm bảo "pay-to-play" (trả rồi mới chơi).
    /// </summary>
    [HttpPost("init")]
    public async Task<IActionResult> InitSession([FromBody] InitReadingSessionCommand command)
    {
        // Gắn UserId từ JWT Token vào command
        // TẠI SAO GÁN TRỰC TIẾP? Vì ở đây command được dũng trực tiếp từ body,
        // chỉ override trường UserId từ JWT (các trường khác client gửi).
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        command.UserId = userId; // Override userId bằng JWT (bảo mật)

        var result = await _mediator.Send(command);
        return Ok(result); // Trả về SessionId, thông tin phiên
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/reading/reveal
    /// MỤC ĐÍCH: Bước 2 - Lật bài từ phiên đã khởi tạo.
    ///
    /// CÁCH HOẠT ĐỘNG:
    ///   1. Client gửi: SessionId (từ Bước 1)
    ///   2. Handler kiểm tra session hợp lệ và thuộc user
    ///   3. CHỐT RNG: dùng Server Seed + Client Seed → tạo danh sách lá bài
    ///   4. Mỗi lá bài có: tên, vị trí, hướng (xuôi/ngược)
    ///   5. Cộng EXP cho UserCollection (bộ sưu tập lá bài)
    ///   6. Trả về danh sách lá bài cho client hiển thị
    ///
    /// SAU REVEAL: client có thể gọi AiController để xin AI giải nghĩa.
    ///
    /// RNG (Random Number Generator):
    ///   Server Seed: seed bí mật do server tạo ở Bước 1
    ///   Client Seed: seed do client gửi (tùy chọn, tăng tính minh bạch)
    ///   Kết hợp 2 seed → tạo chuỗi số ngẫu nhiên → xác định bài nào được rút
    ///   Cơ chế này đảm bảo: server không gian lận, client không dự đoán được.
    /// </summary>
    [HttpPost("reveal")]
    public async Task<IActionResult> RevealCards([FromBody] TarotNow.Application.Features.Reading.Commands.RevealSession.RevealReadingSessionCommand command)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();
        command.UserId = userId; // Override userId từ JWT

        var result = await _mediator.Send(command);
        return Ok(result); // Trả về danh sách lá bài đã rút
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/reading/cards-catalog
    /// MỤC ĐÍCH: Lấy toàn bộ catalog 78 lá bài (tên đa ngôn ngữ, ý nghĩa, ảnh, suit...).
    /// Dùng cho frontend hiển thị tên/meaning/ảnh đồng nhất từ DB.
    /// </summary>
    [HttpGet("cards-catalog")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalog(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new TarotNow.Application.Features.Reading.Queries.GetCardsCatalog.GetCardsCatalogQuery(),
            cancellationToken
        );
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/reading/collection
    /// MỤC ĐÍCH: Xem bộ sưu tập (collection) lá bài tarot của user.
    ///
    /// BỘ SƯU TẬP LÀ GÌ?
    ///   Mỗi khi user rút bài, các lá bài được "thu thập" vào bộ sưu tập.
    ///   Giống như Pokédex trong Pokémon: ghi nhận mỗi lá bài đã từng rút,
    ///   số lần rút, EXP (kinh nghiệm) tích lũy.
    ///   Đây là tính năng gamification (trò chơi hóa) để tăng engagement.
    ///
    /// TRẢ VỀ: Danh sách 78 lá bài tarot với trạng thái:
    ///   - Đã thu thập: số lần rút, EXP, lần rút gần nhất
    ///   - Chưa thu thập: đánh dấu "locked" (ẩn ảnh)
    /// </summary>
    [HttpGet("collection")]
    public async Task<IActionResult> GetCollection()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var result = await _mediator.Send(
            new TarotNow.Application.Features.Reading.Queries.GetCollection.GetUserCollectionQuery { UserId = userId }
        );
        return Ok(result);
    }
}

/*
 * ===================================================================
 * FILE: InitReadingSessionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Commands.InitSession
 * ===================================================================
 * MỤC ĐÍCH:
 *   Phát Thẻ Cào Tạo Ngôi Nhà Lồng Kính (Reading Session). 
 *   Khách hàng vừa bấm "Bắt Đầu" trên UI là API này tạo phòng.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public class InitReadingSessionCommand : IRequest<InitReadingSessionResult>
{
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Các Loại Bàn Cờ Sinh Ra: 
    /// Thẻ Ngày (Daily 1 Lá), 3 Lá, 5 Lá Hoặc Cao Cấp 10 Lá Thập Tự Giá.
    /// </summary>
    public string SpreadType { get; set; } = string.Empty;
    
    /// <summary>Câu Liền Đề Khách Ghi Trong Ô Textbox: "Bao Giờ Lấy Được Vợ Đi Xế Hộp?"</summary>
    public string? Question { get; set; }

    /// <summary>Người dùng chọn thanh toán bằng gì (Gold hoặc Diamond)</summary>
    public string Currency { get; set; } = string.Empty;
}

public class InitReadingSessionResult
{
    /// <summary>Trả Về Chìa Khóa Phòng UUID để FrontEnd Bứng Vào Chế Độ Trải Bài 3D Giao Diện Đồ Hoạ.</summary>
    public string SessionId { get; set; } = string.Empty;
    public long CostGold { get; set; }
    public long CostDiamond { get; set; }
}

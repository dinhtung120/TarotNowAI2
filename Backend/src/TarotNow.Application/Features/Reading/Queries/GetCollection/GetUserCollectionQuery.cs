/*
 * ===================================================================
 * FILE: GetUserCollectionQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Queries.GetCollection
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Hỏi Xin Xem Bộ Sưu Tập Bài Tarot (Kho Đồ).
 *   Trả Về Mức Độ Thông Thạo (Level/Exp) Của Từng Lá Bài Tương Ứng Với Các Lần Rút.
 * ===================================================================
 */

using MediatR;
using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

public class GetUserCollectionQuery : IRequest<List<UserCollectionDto>>
{
    public Guid UserId { get; set; }
}

public class UserCollectionDto
{
    /// <summary>Số Thứ Tự Của Lá Bài (0-77 trong bộ chuẩn Rider Waite).</summary>
    public int CardId { get; set; }
    
    /// <summary>Thẻ Thăng Cấp Bao Nhiêu (Càng bốc Trúng Nhiều Thì Level Thẻ Càng Cao - Gacha style).</summary>
    public int Level { get; set; }
    
    /// <summary>Số Điểm Thông Thạo Của Thẻ Này Mày Tích Được.</summary>
    public long ExpGained { get; set; }
    
    /// <summary>Bốc ra cái lá bài này vô lúc mấy giờ (Hôm qua hay năm ngoái?)</summary>
    public DateTime LastDrawnAt { get; set; }
    
    /// <summary>Có Bao Nhiêu Bản Sao Khác Nhau Nếu Trò Chơi Thích Áp Dụng Chế Độ Trộn Thẻ Gacha.</summary>
    public int Copies { get; set; }

    /// <summary>Chỉ số Tấn công của lá bài (Dùng cho Gacha/Battle Mode).</summary>
    public int Atk { get; set; }

    /// <summary>Chỉ số Phòng thủ của lá bài (Dùng cho Gacha/Battle Mode).</summary>
    public int Def { get; set; }
}

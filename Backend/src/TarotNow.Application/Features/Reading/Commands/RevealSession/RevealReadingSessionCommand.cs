/*
 * ===================================================================
 * FILE: RevealReadingSessionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reading.Commands.RevealSession
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh chốt đơn Lật Bài (Reveal).
 *   Được kích hoạt khi khách bấm "Lật Bài Mở Quẻ" ở Giao Diện 3D.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public class RevealReadingSessionCommand : IRequest<RevealReadingSessionResult>
{
    /// <summary>Căn Cước Khách Bấm Nut Lật (Trích Từ JWT Token để tránh Khách bấm hộ bài Của Nhau).</summary>
    public Guid UserId { get; set; }

    /// <summary>ID Bàn Đang Trải được Trả Về Từ API Trước Đó (InitSession).</summary>
    public string SessionId { get; set; } = string.Empty;
}

public class RevealReadingSessionResult
{
    /// <summary>
    /// Mảng chứa ID các lá bài được bốc ngẫu nhiên (vd: [0, 4, 12]).
    /// Frontend sẽ Map số index 0-77 này thành Tên Hình Đồ Họa Của Bộ Tarot Rider-Waite.
    /// </summary>
    public int[] Cards { get; set; } = Array.Empty<int>();
}

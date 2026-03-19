/*
 * ===================================================================
 * FILE: UpdateReaderStatusCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh thay đổi Trạng Thái Đón Khách của 1 Thầy Bói (Reader).
 *   Ví dụ: Đang bận đi ăn cơm -> Gạt Nút Sang Offline.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

public class UpdateReaderStatusCommand : IRequest<bool>
{
    /// <summary>Căn Cước của Thầy Bói. Dùng JWT nhét vào tự động chứ không cho FrontEnd Fake.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Các Mức Cảnh Giới:
    /// - "Online": Đang treo máy chờ khách (Chat rảnh rỗi).
    /// - "Offline": Đang ngủ/Đi ăn cơm (Khóa mõm các nút Đặt Lịch).
    /// - "AcceptingQuestions": Trạng thái đặc thù (Sẵn sàng hốt Khách lạ vào xem Bói bằng tính năng Stream Mới).
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

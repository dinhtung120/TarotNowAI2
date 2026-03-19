/*
 * ===================================================================
 * FILE: ToggleUserLockCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.ToggleUserLock
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command cho Admin KHÓA hoặc MỞ KHÓA tài khoản user.
 *
 * KHI NÀO KHÓA TÀI KHOẢN?
 *   - User vi phạm quy tắc cộng đồng
 *   - Account bị nghi ngờ hack
 *   - Gian lận tài chính (double-spend attempt)
 *   - Nhiều report vi phạm nghiêm trọng
 *
 * KHI BỊ KHÓA:
 *   - Không thể đăng nhập (JWT bị reject)
 *   - Tiền trong ví vẫn giữ nguyên (không bị mất)
 *   - Admin có thể mở khóa bất cứ lúc nào
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

/// <summary>
/// Command chứa dữ liệu cho thao tác khóa/mở khóa user.
/// IRequest<bool>: trả về true nếu thành công.
/// </summary>
public class ToggleUserLockCommand : IRequest<bool>
{
    /// <summary>UUID user cần khóa/mở khóa.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// true = KHÓA tài khoản, false = MỞ KHÓA.
    /// Admin chọn từ toggle switch trên UI.
    /// </summary>
    public bool Lock { get; set; }
}

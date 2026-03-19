/*
 * ===================================================================
 * FILE: UpdateProfileCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Profile.Commands.UpdateProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh cho phép User thay đổi thông tin cá nhân (Hồ Sơ).
 *
 * SCOPE BẢO MẬT:
 *   Chú ý, API này CHỈ cho phép đổi Tên Hiển Thị (DisplayName), Ảnh Đại Diện (Avatar), Ngày Sinh.
 *   Nó TUYỆT ĐỐI KHÔNG mở cửa cho đổi Mật Khẩu, Đổi Email hay Đổi Vai Trò (Reader/Admin) ở đây. 
 *   Phân Tách Trách Nhiệm (Separation of Concerns).
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    
    /// <summary>Tên cúng cơm hiển thị trong Box Chat Tarot.</summary>
    public string DisplayName { get; set; } = string.Empty;
    
    /// <summary>Đường link ảnh đã được đẩy lên Cloud (S3/Cloudinary) trước đó rồi lấy URL nhét vào đây.</summary>
    public string? AvatarUrl { get; set; }
    
    /// <summary>Sinh thần bát tự (Rất Quan Trọng đối với bộ môn bói Tarot / Chiêm tinh học).</summary>
    public DateTime DateOfBirth { get; set; }
}

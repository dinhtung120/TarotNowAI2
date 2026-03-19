/*
 * ===================================================================
 * FILE: GetProfileQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Profile.Queries.GetProfile
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh Trích Xuất Toàn Bộ Sinh Mệnh (Hồ Sơ) của Khách Hàng.
 *   
 * ỨNG DỤNG UI:
 *   Thường gọi sau khi Login Thành Công (Để vẽ Avatar lên góc phải màn hình).
 *   Hoặc gọi khi User bấm vào trang Cá Nhân của họ.
 * ===================================================================
 */

using MediatR;
using System;

namespace TarotNow.Application.Features.Profile.Queries.GetProfile;

public class GetProfileQuery : IRequest<ProfileResponse>
{
    public Guid UserId { get; set; }
}

public class ProfileResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    // ==========================================
    // CÁC THÔNG SỐ ĐƯỢC TÍNH TOÁN (COMPUTED CHỈ ĐỌC)
    // ==========================================
    
    /// <summary>Cung Hoàng Đạo (Tự suy ra từ Ngày Sinh chứ không lưu Database cứng ngắc).</summary>
    public string Zodiac { get; set; } = string.Empty;
    
    /// <summary>Số Chủ Đạo Thần Số Học Numerology (Tính nhẩm từ Ngày Tháng Năm Sinh).</summary>
    public int Numerology { get; set; }
    
    // ==========================================
    // GAMIFICATION (TRÒ CHƠI HOÁ SẢN PHẨM)
    // ==========================================
    
    /// <summary>Cấp độ hiện tại của khách (Tích cực chat điểm vút lên Level 99 rớt đồ Vip).</summary>
    public int Level { get; set; }
    public long Exp { get; set; }
    
    // ==========================================
    // CỜ BÁO HIỆU PHÁP LÝ (LEGAL POPUP FLAG)
    // ==========================================
    
    /// <summary>True = Ký Đủ. False = Nợ Hồ Sơ (Frontend dùng biến này văng cái Bảng bắt Đồng Ý ngay).</summary>
    public bool HasConsented { get; set; }
}

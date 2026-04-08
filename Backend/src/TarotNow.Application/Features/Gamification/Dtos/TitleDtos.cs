using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

// DTO định nghĩa title.
public class TitleDefinitionDto
{
    // Mã title.
    public string Code { get; set; } = string.Empty;

    // Tên title tiếng Việt.
    public string NameVi { get; set; } = string.Empty;

    // Tên title tiếng Anh.
    public string NameEn { get; set; } = string.Empty;

    // Tên title tiếng Trung.
    public string NameZh { get; set; } = string.Empty;

    // Mô tả tiếng Việt.
    public string DescriptionVi { get; set; } = string.Empty;

    // Mô tả tiếng Anh.
    public string DescriptionEn { get; set; } = string.Empty;

    // Mô tả tiếng Trung.
    public string DescriptionZh { get; set; } = string.Empty;

    // Độ hiếm title.
    public string Rarity { get; set; } = string.Empty;

    // Cờ active/inactive của title.
    public bool IsActive { get; set; }
}

// DTO title user đã mở khóa.
public class UserTitleDto
{
    // Mã title đã mở khóa.
    public string TitleCode { get; set; } = string.Empty;

    // Thời điểm được cấp title.
    public DateTime GrantedAt { get; set; }
}

// DTO tổng hợp title definitions + danh sách title user sở hữu.
public class UserTitlesDto
{
    // Danh sách định nghĩa title.
    public List<TitleDefinitionDto> Definitions { get; set; } = new();

    // Danh sách title user đã mở khóa.
    public List<UserTitleDto> UnlockedList { get; set; } = new();

    // Title đang active của user (nếu có).
    public string? ActiveTitleCode { get; set; }
}

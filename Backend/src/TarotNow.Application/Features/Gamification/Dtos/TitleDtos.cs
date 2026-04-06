using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Gamification.Dtos;

public class TitleDefinitionDto
{
    public string Code { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameZh { get; set; } = string.Empty;
    public string DescriptionVi { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionZh { get; set; } = string.Empty;
    public string Rarity { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class UserTitleDto
{
    public string TitleCode { get; set; } = string.Empty;
    public DateTime GrantedAt { get; set; }
}

public class UserTitlesDto
{
    public List<TitleDefinitionDto> Definitions { get; set; } = new();
    public List<UserTitleDto> UnlockedList { get; set; } = new();
    public string? ActiveTitleCode { get; set; }
}

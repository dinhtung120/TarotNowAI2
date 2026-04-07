

using System.ComponentModel.DataAnnotations;

namespace TarotNow.Api.Contracts.Requests;

public class CreatePostBody
{
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public string Visibility { get; set; } = string.Empty;
}

public class UpdatePostBody
{
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;
}

public class ToggleReactionBody
{
    [Required]
    public string Type { get; set; } = string.Empty;
}

public class ReportPostBody
{
    [Required]
    public string ReasonCode { get; set; } = string.Empty;

    [Required]
    [MinLength(10)]
    public string Description { get; set; } = string.Empty;
}

public class ResolveReportBody
{
    [Required]
    public string Result { get; set; } = string.Empty;

    public string? AdminNote { get; set; }
}

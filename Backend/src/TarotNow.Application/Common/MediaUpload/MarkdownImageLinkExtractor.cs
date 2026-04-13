using System.Text.RegularExpressions;

namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Trích xuất URL ảnh markdown từ nội dung post/comment.
/// </summary>
public static partial class MarkdownImageLinkExtractor
{
    [GeneratedRegex("!\\[[^\\]]*\\]\\((?<url>[^)]+)\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex ImageRegex();

    /// <summary>
    /// Trích xuất tất cả URL ảnh markdown theo thứ tự xuất hiện.
    /// </summary>
    public static IReadOnlyList<string> ExtractUrls(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return Array.Empty<string>();
        }

        var matches = ImageRegex().Matches(markdown);
        if (matches.Count == 0)
        {
            return Array.Empty<string>();
        }

        var urls = new List<string>(matches.Count);
        foreach (Match match in matches)
        {
            var url = match.Groups["url"].Value.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                continue;
            }

            urls.Add(url);
        }

        return urls;
    }
}

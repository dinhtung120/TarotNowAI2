namespace TarotNow.Api.Constants;

/// <summary>
/// Định nghĩa version API để route và cấu hình versioning dùng chung một nguồn.
/// </summary>
public static class ApiVersions
{
    // Version logic hiện tại của API công khai.
    public const string V1 = "1.0";

    // Segment route dùng cho Asp.Versioning trong attribute route.
    public const string Segment = "v{version:apiVersion}";
}

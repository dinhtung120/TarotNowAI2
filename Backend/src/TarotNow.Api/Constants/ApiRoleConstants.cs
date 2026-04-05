namespace TarotNow.Api.Constants;

/// <summary>
/// API-level role constants.
/// Keep role literals in presentation layer to avoid direct Domain dependency.
/// </summary>
public static class ApiRoleConstants
{
    public const string User = "user";
    public const string TarotReader = "tarot_reader";
    public const string Admin = "admin";
    public const string System = "system";
}

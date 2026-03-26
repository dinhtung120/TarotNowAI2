namespace TarotNow.Api.Constants;

public static class ApiRoutes
{
    private const string Prefix = "api/" + ApiVersions.Segment;

    public const string Controller = Prefix + "/[controller]";
    public const string Admin = Prefix + "/admin";
    public const string Auth = Prefix + "/auth";
    public const string Reading = Prefix + "/reading";
    public const string Sessions = Prefix + "/sessions";
    public const string Escrow = Prefix + "/escrow";
    public const string Deposits = Prefix + "/deposits";
    public const string Conversations = Prefix + "/conversations";
    public const string Legal = Prefix + "/legal";
    public const string Reader = Prefix + "/reader";
    public const string Reports = Prefix + "/reports";
    public const string Profile = Prefix + "/profile";
    public const string Withdrawal = Prefix + "/withdrawal";

    public const string ReadersAbsolute = "/" + Prefix + "/readers";
    public const string ChatHub = "/" + Prefix + "/chat";
}

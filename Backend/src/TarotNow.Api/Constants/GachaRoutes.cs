namespace TarotNow.Api.Constants;

/// <summary>
/// Route con cho module gacha theo API mới pool/pull.
/// </summary>
public static class GachaRoutes
{
    /// <summary>
    /// Route lấy danh sách pool active.
    /// </summary>
    public const string Pools = "pools";

    /// <summary>
    /// Route lấy odds của pool.
    /// </summary>
    public const string PoolOdds = "pools/{poolCode}/odds";

    /// <summary>
    /// Route lấy lịch sử pull.
    /// </summary>
    public const string History = "history";

    /// <summary>
    /// Route thực thi pull gacha.
    /// </summary>
    public const string Pull = "pull";
}

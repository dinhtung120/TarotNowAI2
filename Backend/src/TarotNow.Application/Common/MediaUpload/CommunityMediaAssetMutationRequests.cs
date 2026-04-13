namespace TarotNow.Application.Common.MediaUpload;

/// <summary>
/// Request gắn asset theo draft context sang entity chính thức.
/// </summary>
public sealed record AttachDraftCommunityAssetsRequest(
    Guid OwnerUserId,
    string ContextType,
    string ContextDraftId,
    string ContextEntityId,
    IReadOnlyCollection<string> ObjectKeys,
    DateTime AttachedAtUtc);

/// <summary>
/// Request gắn asset theo object keys vào entity hiện tại.
/// </summary>
public sealed record AttachCommunityAssetsByObjectKeysRequest(
    Guid OwnerUserId,
    string ContextType,
    string ContextEntityId,
    IReadOnlyCollection<string> ObjectKeys,
    DateTime AttachedAtUtc);

/// <summary>
/// Request reconcile asset attached để chuyển object key không còn dùng sang orphaned.
/// </summary>
public sealed record ReconcileCommunityAssetsRequest(
    Guid OwnerUserId,
    string ContextType,
    string ContextEntityId,
    IReadOnlyCollection<string> ActiveObjectKeys,
    DateTime ReconciledAtUtc,
    DateTime OrphanExpiresAtUtc);

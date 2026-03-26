namespace TarotNow.Application.Common;

public enum WalletOperationStatus
{
    Executed = 0,
    AlreadyHandled = 1
}

public sealed class WalletOperationResult
{
    private WalletOperationResult(WalletOperationStatus status)
    {
        Status = status;
    }

    public WalletOperationStatus Status { get; }

    public bool Executed => Status == WalletOperationStatus.Executed;

    public static WalletOperationResult ExecutedResult { get; } =
        new(WalletOperationStatus.Executed);

    public static WalletOperationResult AlreadyHandledResult { get; } =
        new(WalletOperationStatus.AlreadyHandled);
}


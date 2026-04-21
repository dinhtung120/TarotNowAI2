namespace TarotNow.Domain.Enums;

// Tập hằng action xử lý yêu cầu rút tiền.
public static class WithdrawalProcessAction
{
    // Action phê duyệt yêu cầu.
    public const string Approve = "approve";

    // Action từ chối yêu cầu.
    public const string Reject = "reject";
}

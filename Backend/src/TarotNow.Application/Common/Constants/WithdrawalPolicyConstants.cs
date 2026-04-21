namespace TarotNow.Application.Common.Constants;

// Tập trung hằng số nghiệp vụ cho luồng rút tiền.
public static class WithdrawalPolicyConstants
{
    // Số kim cương tối thiểu mỗi lần rút.
    public const long MinimumWithdrawDiamond = 500;

    // Tỷ lệ phí rút (10%).
    public const decimal FeeRate = 0.10m;

    // Độ dài tối đa của idempotency key.
    public const int IdempotencyKeyMaxLength = 128;

    // Độ dài tối đa của ghi chú user/admin.
    public const int NoteMaxLength = 1000;
}

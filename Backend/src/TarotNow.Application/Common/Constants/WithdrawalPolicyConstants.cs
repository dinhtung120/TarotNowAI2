namespace TarotNow.Application.Common.Constants;

// Tập trung hằng số nghiệp vụ cho luồng rút tiền.
public static class WithdrawalPolicyConstants
{
    // Độ dài tối đa của idempotency key.
    public const int IdempotencyKeyMaxLength = 128;

    // Độ dài tối đa của ghi chú user/admin.
    public const int NoteMaxLength = 1000;
}

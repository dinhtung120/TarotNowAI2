using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TarotNow.Api.Middlewares;

public partial class GlobalExceptionHandler
{
    /// <summary>
    /// Kiểm tra lỗi unique constraint cho rule mỗi tuần UTC chỉ có một yêu cầu rút tiền.
    /// Luồng xử lý: xác nhận inner exception là PostgresException rồi so khớp SqlState và tên constraint.
    /// </summary>
    private static bool IsWithdrawalWeeklyLimitViolation(DbUpdateException exception)
    {
        if (exception.InnerException is not PostgresException postgresException)
        {
            // Không phải lỗi từ PostgreSQL thì không thể kết luận theo constraint name.
            return false;
        }

        // Điều kiện đầy đủ để nhận diện đúng business rule "một yêu cầu rút tiền mỗi tuần UTC".
        return postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(
                   postgresException.ConstraintName,
                   "ix_withdrawal_one_per_week",
                   StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Kiểm tra lỗi uniqueness/idempotency của các bảng escrow liên quan hội thoại.
    /// Luồng xử lý: xác nhận loại lỗi unique violation rồi so khớp các constraint hỗ trợ chống xử lý trùng.
    /// </summary>
    private static bool IsEscrowUniquenessViolation(DbUpdateException exception)
    {
        if (exception.InnerException is not PostgresException postgresException)
        {
            // Không có PostgresException thì bỏ qua nhánh nhận diện constraint chi tiết.
            return false;
        }

        if (postgresException.SqlState != PostgresErrorCodes.UniqueViolation)
        {
            // Chỉ xử lý duy nhất nhánh vi phạm unique để tránh map nhầm loại lỗi DB khác.
            return false;
        }

        // Chấp nhận cả hai constraint vì đều biểu thị yêu cầu giao dịch/phiên bị xử lý trùng.
        return string.Equals(
                   postgresException.ConstraintName,
                   "ix_chat_finance_sessions_conversation_ref",
                   StringComparison.OrdinalIgnoreCase)
               || string.Equals(
                   postgresException.ConstraintName,
                   "ix_chat_question_items_idempotency_key",
                   StringComparison.OrdinalIgnoreCase);
    }
}

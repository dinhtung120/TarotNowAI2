using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TarotNow.Api.Middlewares;

public partial class GlobalExceptionHandler
{
    private static bool IsWithdrawalDailyLimitViolation(DbUpdateException exception)
    {
        if (exception.InnerException is not PostgresException postgresException)
        {
            return false;
        }

        return postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(
                   postgresException.ConstraintName,
                   "ix_withdrawal_one_per_day_active",
                   StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsEscrowUniquenessViolation(DbUpdateException exception)
    {
        if (exception.InnerException is not PostgresException postgresException)
        {
            return false;
        }

        if (postgresException.SqlState != PostgresErrorCodes.UniqueViolation)
        {
            return false;
        }

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

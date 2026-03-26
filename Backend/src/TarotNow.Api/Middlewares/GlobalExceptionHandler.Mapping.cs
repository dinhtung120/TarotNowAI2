using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Exceptions;

namespace TarotNow.Api.Middlewares;

public partial class GlobalExceptionHandler
{
    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        if (TryCreateDatabaseProblem(exception, out var databaseProblem))
        {
            return databaseProblem;
        }

        return exception switch
        {
            ValidationException validationException => CreateValidationProblem(validationException),
            BadRequestException badRequestException => CreateBadRequestProblem(badRequestException.Message),
            NotFoundException notFoundException => CreateNotFoundProblem(notFoundException.Message),
            BusinessRuleException businessRuleException => CreateBusinessRuleProblem(businessRuleException),
            ArgumentException argumentException => CreateBadRequestProblem(argumentException.Message),
            InvalidOperationException invalidOperationException => CreateInvalidOperationProblem(invalidOperationException.Message),
            UnauthorizedAccessException => CreateUnauthorizedProblem(),
            _ => CreateServerProblem()
        };
    }

    private static bool TryCreateDatabaseProblem(Exception exception, out ProblemDetails problemDetails)
    {
        if (exception is not DbUpdateException dbUpdateException)
        {
            problemDetails = default!;
            return false;
        }

        if (IsWithdrawalDailyLimitViolation(dbUpdateException))
        {
            problemDetails = CreateBadRequestProblem("Bạn đã có yêu cầu rút tiền hôm nay. Vui lòng thử lại ngày mai.");
            return true;
        }

        if (IsEscrowUniquenessViolation(dbUpdateException))
        {
            problemDetails = CreateConflictProblem("Yêu cầu trùng lặp hoặc đã được xử lý trước đó.");
            return true;
        }

        problemDetails = default!;
        return false;
    }

    private static ProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var problemDetails = CreateClientProblem(
            StatusCodes.Status400BadRequest,
            "Validation Failed",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            exception.Message);

        problemDetails.Extensions["errors"] = exception.Errors;
        return problemDetails;
    }

    private static ProblemDetails CreateBusinessRuleProblem(BusinessRuleException exception)
    {
        var problemDetails = CreateClientProblem(
            StatusCodes.Status422UnprocessableEntity,
            "Domain Rule Violation",
            "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
            exception.Message);

        problemDetails.Extensions["errorCode"] = exception.ErrorCode;
        return problemDetails;
    }
}

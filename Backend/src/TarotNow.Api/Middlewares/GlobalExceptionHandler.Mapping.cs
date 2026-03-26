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

        var knownProblem = CreateKnownProblem(exception);
        return knownProblem ?? CreateServerProblem();
    }

    private static ProblemDetails? CreateKnownProblem(Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            return CreateValidationProblem(validationException);
        }

        if (exception is BadRequestException badRequestException)
        {
            return CreateBadRequestProblem(badRequestException.Message);
        }

        if (exception is NotFoundException notFoundException)
        {
            return CreateNotFoundProblem(notFoundException.Message);
        }

        if (exception is BusinessRuleException businessRuleException)
        {
            return CreateBusinessRuleProblem(businessRuleException);
        }

        if (exception is ArgumentException argumentException)
        {
            return CreateBadRequestProblem(argumentException.Message);
        }

        if (exception is InvalidOperationException invalidOperationException)
        {
            return CreateInvalidOperationProblem(invalidOperationException.Message);
        }

        if (exception is UnauthorizedAccessException)
        {
            return CreateUnauthorizedProblem();
        }

        return null;
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

using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Extensions;

public static class ControllerProblemDetailsExtensions
{
    public static IActionResult UnauthorizedProblem(this ControllerBase controller, string? detail = null)
    {
        return controller.Problem(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "Unauthorized",
            detail: detail ?? "Yêu cầu xác thực hợp lệ.");
    }

    public static IActionResult NotFoundProblem(this ControllerBase controller, string detail)
    {
        return controller.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: detail);
    }
}

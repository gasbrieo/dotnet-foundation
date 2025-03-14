using Microsoft.AspNetCore.Mvc;

namespace Modello.Foundation.AspNetCore;

public static partial class ResultExtensions
{
    public static IActionResult ToActionResult(this ControllerBase controller, IResult result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => controller.Ok(result.GetValue()),
            ResultStatus.Error => BuildErrorResponse(controller, result),
            ResultStatus.NotFound => BuildNotFoundResponse(controller),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    private static BadRequestObjectResult BuildErrorResponse(ControllerBase controller, IResult result)
    {
        var errorResponse = new ErrorListResponse(
            controller.HttpContext.Request.Path,
            controller.HttpContext.TraceIdentifier,
            result.Errors.Select(e => new ErrorDetail(e.Type, e.Error, e.Detail)).ToList()
        );

        return controller.BadRequest(errorResponse);
    }

    private static NotFoundObjectResult BuildNotFoundResponse(ControllerBase controller)
    {
        var notFoundResponse = new ErrorResponse(
            controller.HttpContext.Request.Path,
            controller.HttpContext.TraceIdentifier,
            "NotFound",
            "Not Found",
            "The requested resource was not found."
        );

        return controller.NotFound(notFoundResponse);
    }
}

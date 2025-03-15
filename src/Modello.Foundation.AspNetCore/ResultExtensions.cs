namespace Modello.Foundation.AspNetCore;

public static partial class ResultExtensions
{
    public static IActionResult ToActionResult(this IResult result, ControllerBase controller)
    {
        return result.Status switch
        {
            ResultStatus.Ok => controller.Ok(result.GetValue()),
            ResultStatus.Created => controller.Created(result.Location, result.GetValue()),
            ResultStatus.NoContent => controller.NoContent(),
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
            result.Errors.Select(e => new ErrorDetail(e.Type, e.Error, e.Detail))
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

using IFoundationResult = Modello.Foundation.IResult;
using IHttpResult = Microsoft.AspNetCore.Http.IResult;

namespace Modello.Foundation.AspNetCore;

public static class MinimalApiResultExtensions
{
    public static IHttpResult ToMinimalApiResult(this IFoundationResult result, HttpContext context) =>
        result.Status switch
        {
            ResultStatus.Ok => TypedResults.Ok(result.GetValue()),
            ResultStatus.Created => TypedResults.Created(result.Location, result.GetValue()),
            ResultStatus.NoContent => TypedResults.NoContent(),
            ResultStatus.Error => TypedResults.BadRequest(BuildBadRequestResponse(result, context)),
            ResultStatus.NotFound => TypedResults.NotFound(BuildNotFoundResponse(context)),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };

    private static ErrorListResponse BuildBadRequestResponse(IFoundationResult result, HttpContext context)
    {
        return new ErrorListResponse(
            context.Request.Path,
            context.TraceIdentifier,
            result.Errors.Select(e => new ErrorDetail(e.Type, e.Error, e.Detail))
        );
    }

    private static ErrorResponse BuildNotFoundResponse(HttpContext context)
    {
        return new ErrorResponse(
            context.Request.Path,
            context.TraceIdentifier,
            "NotFound",
            "Not Found",
            "The requested resource was not found."
        );
    }
}

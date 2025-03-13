namespace Modello.Foundation;

public static class IResultExtensions
{
    public static bool IsOk(this IResult result) => result.Status == ResultStatus.Ok;

    public static bool IsError(this IResult result) => result.Status == ResultStatus.Error;

    public static bool IsNotFound(this IResult result) => result.Status == ResultStatus.NotFound;
}

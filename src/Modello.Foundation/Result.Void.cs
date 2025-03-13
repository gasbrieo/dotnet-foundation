namespace Modello.Foundation;

public class Result : Result<Result>
{
    public Result() : base() { }

    protected internal Result(ResultStatus status) : base(status) { }

    public static Result Success() => new();

    public static Result<T> Success<T>(T value) => new(value);

    public new static Result Error(params ValidationError[] errors) => new(ResultStatus.Error)
    {
        Errors = errors
    };

    public new static Result NotFound() => new(ResultStatus.NotFound);
}

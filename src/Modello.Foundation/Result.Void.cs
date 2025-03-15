namespace Modello.Foundation;

public class Result : Result<Result>
{
    public Result() : base() { }

    protected internal Result(ResultStatus status) : base(status) { }

    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    public static Result<TValue> Created<TValue>(TValue value, string location) => Result<TValue>.Created(value, location);

    public new static Result NoContent() => new(ResultStatus.NoContent);

    public new static Result Error(params ValidationError[] errors) => new(ResultStatus.Error)
    {
        Errors = errors
    };

    public new static Result NotFound() => new(ResultStatus.NotFound);
}

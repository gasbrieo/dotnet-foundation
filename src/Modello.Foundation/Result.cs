namespace Modello.Foundation;

public class Result<TValue> : IResult
{
    public TValue? Value { get; }

    public ResultStatus Status { get; protected set; } = ResultStatus.Ok;

    public bool IsSuccess => Status is ResultStatus.Ok;

    public IEnumerable<ValidationError> Errors { get; protected set; } = [];

    protected Result() { }

    public Result(TValue? value) => Value = value;

    protected Result(ResultStatus status) => Status = status;

    public object? GetValue() => Value;

    public PagedResult<TValue> ToPagedResult(PagedInfo pagedInfo)
    {
        var pagedResult = new PagedResult<TValue>(pagedInfo, Value)
        {
            Status = Status,
            Errors = Errors
        };

        return pagedResult;
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Error(params ValidationError[] errors) => new(ResultStatus.Error)
    {
        Errors = errors
    };

    public static Result<TValue> NotFound() => new(ResultStatus.NotFound);

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Result result) => new(default(TValue))
    {
        Status = result.Status,
        Errors = result.Errors,
    };
}

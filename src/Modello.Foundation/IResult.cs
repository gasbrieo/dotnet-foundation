namespace Modello.Foundation;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<ValidationError> Errors { get; }
    object? GetValue();
}

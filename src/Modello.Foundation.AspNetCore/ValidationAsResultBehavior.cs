namespace Modello.Foundation.AspNetCore;

public class ValidationAsResultBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var responseType = typeof(TResponse);
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count > 0)
            {
                var errors = failures
                    .Select(f => new ValidationError("ValidationError", f.ErrorCode, f.ErrorMessage))
                    .ToArray();

                var errorResult = responseType
                    .GetMethod("Error", [typeof(ValidationError[])])?
                    .Invoke(null, [errors]);

                return (TResponse)errorResult!;
            }
        }

        return await next();
    }
}

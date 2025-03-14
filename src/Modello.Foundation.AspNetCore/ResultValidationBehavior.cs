namespace Modello.Foundation.AspNetCore;

public sealed class ResultValidationBehavior<TRequest>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, IResult>
    where TRequest : class
{
    public async Task<IResult> Handle(TRequest request, RequestHandlerDelegate<IResult> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

            var failures = validationResults
                .Where(r => r.Errors.Count > 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                var errors = failures
                    .Select(f => new ValidationError("ValidationError", f.ErrorCode, f.ErrorMessage))
                    .ToArray();

                return Result.Error(errors);
            }
        }

        return await next();
    }
}

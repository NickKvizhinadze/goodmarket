using FluentValidation;
using GoodMarket.Shared.Result;
using GoodMarket.Shared.Mediator.Pipeline;

namespace GoodMarket.Identity.Api.Pipelines;

public class ValidationBehavior<TInput, TOutput> : IPipelineBehavior<TInput, TOutput>
{
    private readonly IEnumerable<IValidator<TInput>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TInput>> validators)
    {
        _validators = validators;
    }

    public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TInput>(input);

            var validatorResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));
            

            var failures = validatorResults.SelectMany(v => v.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var validationErrors = failures
                    .Select(e => new Error(e.ErrorCode, e.ErrorMessage))
                    .ToList();

                if (typeof(TOutput).IsGenericType && typeof(TOutput).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(TOutput).GetGenericArguments()[0];

                    var resultClassType = typeof(Result<>)
                        .MakeGenericType(resultType);

                    var errorResult = Result.BadRequestResult().WithErrors(validationErrors);

                    var resultInstance = Activator.CreateInstance(resultClassType, errorResult);

                    return (TOutput)resultInstance!;
                }

                if (typeof(TOutput) == typeof(Result))
                {
                    return (TOutput)(object)Result.BadRequestResult().WithErrors(validationErrors);
                }

                throw new ValidationException(failures);
            }
        }

        var result = await next();
        return result;
    }
}
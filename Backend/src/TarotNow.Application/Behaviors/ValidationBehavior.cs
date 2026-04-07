

using FluentValidation; 
using MediatR;          

namespace TarotNow.Application.Behaviors;


public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = await ValidateRequestAsync(request, cancellationToken);
        if (failures.Count > 0)
        {
            throw new TarotNow.Application.Exceptions.ValidationException(
                failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(x => x.ErrorMessage).ToArray()));
        }

        return await next();
    }

    private async Task<List<FluentValidation.Results.ValidationFailure>> ValidateRequestAsync(
        TRequest request,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return [];

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        return validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();
    }
}

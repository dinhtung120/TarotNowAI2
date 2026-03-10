using FluentValidation;
using MediatR;

namespace TarotNow.Application.Behaviors;

/// <summary>
/// Pipeline behavior của MediatR dùng để tự động chạy các Validator (FluentValidation)
/// trước khi request đi vào CommandHandler. Xử lý các lỗi HTTP 422 - ValidationException.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                // Sử dụng Exception Validation cụ thể của app, lát nữa sẽ map qua RFC ProblemDetails
                var errorDictionary = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
                
                throw new TarotNow.Application.Exceptions.ValidationException(errorDictionary);
            }
        }

        return await next();
    }
}

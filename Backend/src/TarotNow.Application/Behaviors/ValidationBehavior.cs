using FluentValidation;
using MediatR;

namespace TarotNow.Application.Behaviors;

// Pipeline behavior chạy toàn bộ validator trước khi request vào handler nghiệp vụ.
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // Danh sách validator được DI theo kiểu request hiện tại.
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Khởi tạo behavior xác thực request.
    /// Luồng xử lý: nhận tập validator để hợp nhất lỗi trước khi ném exception chuẩn hóa.
    /// </summary>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của request trước khi cho phép thực thi handler.
    /// Luồng xử lý: chạy tất cả validator, gom lỗi theo property, ném ValidationException nếu có lỗi.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = await ValidateRequestAsync(request, cancellationToken);
        if (failures.Count > 0)
        {
            // Rule nghiệp vụ: trả lỗi theo từng trường để frontend hiển thị chính xác thông điệp validation.
            throw new TarotNow.Application.Exceptions.ValidationException(
                failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(x => x.ErrorMessage).ToArray()));
        }

        return await next();
    }

    /// <summary>
    /// Chạy song song các validator của request và thu thập danh sách lỗi.
    /// Luồng xử lý: trả rỗng nếu không có validator, ngược lại chạy ValidateAsync và flatten errors.
    /// </summary>
    private async Task<List<FluentValidation.Results.ValidationFailure>> ValidateRequestAsync(
        TRequest request,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            // Edge case request không có validator: coi như hợp lệ để không chặn pipeline.
            return [];
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        // Lọc null defensive và gom toàn bộ lỗi về một danh sách duy nhất.
        return validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();
    }
}

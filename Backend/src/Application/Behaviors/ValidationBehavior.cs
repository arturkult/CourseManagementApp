using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationsResults = _validators.Select(validator => validator.Validate(request)).ToList();
        if (validationsResults.All(result=> result.IsValid))
        {
            return next();
        }

        throw new ValidationException(validationsResults.SelectMany(result => result.Errors));
    }
}
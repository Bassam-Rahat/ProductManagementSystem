using FluentValidation;
using MediatR;

namespace PMS.API.Application.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationtoNen, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any()) return await next();
            var context = new ValidationContext<TRequest>(request);
            var result = await Task.WhenAll(_validators.Select(e => e.ValidateAsync(context, cancellationtoNen)));
            var errors = result.Where(e => e.Errors.Count > 0).SelectMany(e => e.Errors).ToArray();
            if (errors.Length > 0)
                throw new ValidationException(errors);
            return await next();
        }
    }
}
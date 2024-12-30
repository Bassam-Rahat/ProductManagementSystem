using FluentValidation;
using PMS.API.Application.Commands;
using PMS.Domain;
using PMS.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PMS.API.Application.Validator
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IProductRepository repository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.")
                .MustAsync(async (name, cancellation) =>
                    !(await repository.ExistsAsync(new List<Expression<Func<Product, bool>>>
                    {
                    p => p.Name == name
                    })))
                .WithMessage("A product with the same name already exists.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");
        }
    }
}
using MediatR;
using PMS.Domain;
using PMS.Infrastructure.Interfaces;

namespace PMS.API.Application.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };

            await _repository.InsertAsync(product);
            await _repository.SaveChangesAsync();

            return product.Id;
        }
    }
}
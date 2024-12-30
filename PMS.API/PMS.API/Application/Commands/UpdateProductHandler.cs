using MediatR;
using PMS.Infrastructure.Interfaces;

namespace PMS.API.Application.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null) throw new KeyNotFoundException();

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            _repository.Update(product);
            _repository.SaveChanges();

            return product.Id;
        }
    }
}

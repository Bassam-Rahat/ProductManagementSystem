using MediatR;
using PMS.Infrastructure.Interfaces;

namespace PMS.API.Application.Commands
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, string>
    {
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return $"No product found against the id: {request.Id}";

            _repository.Delete(product);
            _repository.SaveChanges();

            return $"{product.Name} deleted successfully.";
        }
    }
}
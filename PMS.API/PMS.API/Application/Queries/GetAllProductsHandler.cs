using MediatR;
using PMS.Domain;
using PMS.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PMS.API.Application.Queries
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>>? filter = null;
            string orderBy = nameof(Product.Name);

            return await _repository.GetAsync(filter, orderBy, isAscending: true);
        }
    }
}
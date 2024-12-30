using MediatR;
using PMS.Domain;

namespace PMS.API.Application.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
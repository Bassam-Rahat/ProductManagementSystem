using MediatR;
using PMS.Domain;

namespace PMS.API.Application.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public Guid Id { get; set; }
    }
}
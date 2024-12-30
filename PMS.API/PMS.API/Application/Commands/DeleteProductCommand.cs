using MediatR;

namespace PMS.API.Application.Commands
{
    public class DeleteProductCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
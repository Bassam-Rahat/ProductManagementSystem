using PMS.Domain;
using PMS.Shared.BaseRepository;

namespace PMS.Infrastructure.Interfaces
{
    public interface IProductRepository : ITriggersBaseRepository<Product>
    {
       
    }
}
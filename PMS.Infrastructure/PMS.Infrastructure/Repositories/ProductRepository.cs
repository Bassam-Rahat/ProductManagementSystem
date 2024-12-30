using PMS.Domain;
using PMS.Infrastructure.Interfaces;
using PMS.Shared.BaseRepository;

namespace PMS.Infrastructure.Repositories
{
    public class ProductRepository : TriggersBaseRepository<Product, ProductDbContext>, IProductRepository
    {
        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
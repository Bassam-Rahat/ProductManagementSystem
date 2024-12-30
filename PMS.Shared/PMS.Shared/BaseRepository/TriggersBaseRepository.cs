using EntityFrameworkCore.Triggers;
using PMS.Shared.Triggers;

namespace PMS.Shared.BaseRepository
{
    public class TriggersBaseRepository<TEntity, TDbContext> :
        BaseRepository<TEntity, TDbContext>,
        ITriggersBaseRepository<TEntity>
        where TEntity : BaseEntity
        where TDbContext : DbContextWithTriggers
    {
        public TriggersBaseRepository(TDbContext dbContext) : base(dbContext)
        {
        }

        public async Task SaveChangesAsync(bool isTrigger = true)
        {
            if (!isTrigger)
            {
                _dbContext.TriggersEnabled = isTrigger;
            }

            await _dbContext.SaveChangesAsync();

            _dbContext.TriggersEnabled = true;
        }

        public void SaveChanges(bool isTrigger = true)
        {
            if (!isTrigger)
            {
                _dbContext.TriggersEnabled = isTrigger;
            }

            _dbContext.SaveChanges();

            _dbContext.TriggersEnabled = true;
        }
    }
}
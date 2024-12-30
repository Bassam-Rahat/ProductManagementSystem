namespace PMS.Shared.BaseRepository
{
    public interface ITriggersBaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        Task SaveChangesAsync(bool isTrigger = true);
        void SaveChanges(bool isTrigger = true);
    }
}
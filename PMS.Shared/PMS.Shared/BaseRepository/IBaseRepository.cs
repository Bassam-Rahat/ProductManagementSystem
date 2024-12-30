using Microsoft.EntityFrameworkCore.Storage;
using PMS.Shared.BaseRepository.Helper;
using PMS.Shared.BaseRepository.Pagging;
using System.Linq.Expressions;

namespace PMS.Shared.BaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            string orderBy = "",
            bool isAscending = false
        );
        Task<IEnumerable<TEntity>> GetAsync(
           List<Expression<Func<TEntity, bool>>>? filter = null,
           string orderBy = "",
           bool isAscending = false
       );
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            string orderBy = "",
            bool isAscending = false
        );
        PaginationResult<TEntity> GetPaginatedList(
            PaggingParmeter parameter,
            Expression<Func<TEntity, bool>>? filter = null
        );

        PaginationResult<TEntity> GetPaginatedList(
        PaggingParmeter parameter,
        List<Expression<Func<TEntity, bool>>>? filters = null
        );
        TEntity? GetById(object id);
        Task<TEntity?> GetByIdAsync(object id);
        void Update(TEntity entityToUpdate);
        void Delete(TEntity entityToDelete);
        Task SaveChangesAsync();
        void SaveChanges();
        IDbContextTransaction GetTransaction();
        Task<IDbContextTransaction> GetTransactionAsync();
        void BulkInsert(List<TEntity> entities);
        Task<IEnumerable<TEntity>> GetLatestAsync(int count = 1, List<Expression<Func<TEntity, bool>>>? filters = null, string orderBy = "CreatedAt");

        Task<IEnumerable<TEntity>> GetOldestAsync(int count = 1, List<Expression<Func<TEntity, bool>>>? filters = null, string orderBy = "CreatedAt");

        IEnumerable<TEntity> GetLatest(int count = 1, List<Expression<Func<TEntity, bool>>>? filters = null, string orderBy = "CreatedAt");

        IEnumerable<TEntity> GetOldest(int count = 1, List<Expression<Func<TEntity, bool>>>? filters = null, string orderBy = "CreatedAt");

        Task<bool> ExistsAsync(List<Expression<Func<TEntity, bool>>>? filters = null);

        bool Exists(List<Expression<Func<TEntity, bool>>>? filters = null);
        string ConvertFiltersToSql(List<Expression<Func<TEntity, bool>>> filters);
        string ConvertExpressionToSql(Expression<Func<TEntity, bool>> expression);
        Task<int> ExecuteCountQueryAsync(string countQuery);
    }
}
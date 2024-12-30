using Ardalis.GuardClauses;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PMS.Shared.BaseRepository.Extensions;
using PMS.Shared.BaseRepository.Helper;
using PMS.Shared.BaseRepository.Pagging;
using System.Linq.Expressions;

namespace PMS.Shared.BaseRepository
{
    public class BaseRepository<TEntity, TDbContext> : IBaseRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        public TDbContext _dbContext { get; private set; }
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TDbContext dbContext)
        {
            _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
            _dbSet = dbContext.Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> GetEntityQueryable(
            List<Expression<Func<TEntity, bool>>>? filters = null,
            string orderBy = "",
            bool isAscending = false
        )
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderByPropertyOrField(orderBy, isAscending);
            }

            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            string orderBy = "",
            bool isAscending = false
        )
        {
            var filters = new List<Expression<Func<TEntity, bool>>>();
            if (filter is not null)
            {
                filters.Add(filter);
            }

            return await GetEntityQueryable(filters, orderBy, isAscending).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            List<Expression<Func<TEntity, bool>>>? filters = null,
            string orderBy = "",
            bool isAscending = false
        )
        {
            return await GetEntityQueryable(filters, orderBy, isAscending).ToListAsync();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            string orderBy = "",
            bool isAscending = false
        )
        {
            var filters = new List<Expression<Func<TEntity, bool>>>();
            if (filter is not null)
            {
                filters.Add(filter);
            }

            return GetEntityQueryable(filters, orderBy, isAscending).ToList();
        }

        public virtual PaginationResult<TEntity> GetPaginatedList(
            PaggingParmeter parameter,
            Expression<Func<TEntity, bool>>? filter = null
        )
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            var paginatedData = RepositoryUtility<TEntity>.GetPaginationAsync(query, parameter, filter);
            return paginatedData;
        }

        public virtual PaginationResult<TEntity> GetPaginatedList(
            PaggingParmeter parameter,
            List<Expression<Func<TEntity, bool>>>? filters = null
        )
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            var paginatedData = RepositoryUtility<TEntity>.GetPaginationAsync(query, parameter);
            return paginatedData;
        }


        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual TEntity? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public virtual IDbContextTransaction GetTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public virtual async Task<IDbContextTransaction> GetTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public void BulkInsert(List<TEntity> entities)
        {
            _dbContext.BulkInsert(entities);
        }

        public virtual async Task<IEnumerable<TEntity>> GetLatestAsync(
            int count = 1,
            List<Expression<Func<TEntity, bool>>>? filters = null,
            string orderBy = "CreatedAt"
        )
        {
            var query = GetEntityQueryable(filters, orderBy, false);
            return await query.Take(count).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetOldestAsync(
            int count = 1,
            List<Expression<Func<TEntity, bool>>>? filters = null,
            string orderBy = "CreatedAt"
        )
        {
            var query = GetEntityQueryable(filters, orderBy, true);
            return await query.Take(count).ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetLatest(
            int count = 1,
            List<Expression<Func<TEntity, bool>>>? filters = null,
            string orderBy = "CreatedAt"
        )
        {
            var query = GetEntityQueryable(filters, orderBy, false);
            return query.Take(count).ToList();
        }

        public virtual IEnumerable<TEntity> GetOldest(
            int count = 1,
            List<Expression<Func<TEntity, bool>>>? filters = null,
            string orderBy = "CreatedAt"
        )
        {
            var query = GetEntityQueryable(filters, orderBy, true);
            return query.Take(count).ToList();
        }

        public virtual async Task<bool> ExistsAsync(List<Expression<Func<TEntity, bool>>>? filters = null)
        {
            return await GetEntityQueryable(filters).AnyAsync();
        }

        public virtual bool Exists(List<Expression<Func<TEntity, bool>>>? filters = null)
        {
            return GetEntityQueryable(filters).Any();
        }

        public string ConvertFiltersToSql(List<Expression<Func<TEntity, bool>>> filters)
        {
            var conditions = new List<string>();

            foreach (var filter in filters)
            {
                var condition = ConvertExpressionToSql(filter);
                conditions.Add(condition);
            }

            return string.Join(" AND ", conditions);
        }

        public string ConvertExpressionToSql(Expression<Func<TEntity, bool>> expression)
        {
            if (expression.Body is BinaryExpression binaryExpression)
            {
                var left = GetMemberName(binaryExpression.Left);
                var value = GetExpressionValue(binaryExpression.Right);

                return $"{left} = '{value}'";
            }
            else if (expression.Body is MethodCallExpression methodCall && methodCall.Method.Name == "Contains")
            {
                var memberName = GetMemberName(methodCall.Object);
                var value = GetExpressionValue(methodCall.Arguments[0]);

                return $"{memberName} LIKE '%{value}%'";
            }
            else
            {
                throw new NotSupportedException("Only simple equality and Contains expressions are supported.");
            }
        }

        private string GetMemberName(Expression expression)
        {
            if (expression is MemberExpression memberExpr)
                return memberExpr.Member.Name;

            throw new NotSupportedException("Unsupported expression type for member name extraction.");
        }

        private object GetExpressionValue(Expression expression)
        {
            if (expression is ConstantExpression constantExpr)
                return constantExpr.Value;

            if (expression is MemberExpression memberExpr)
            {
                var objectMember = Expression.Convert(memberExpr, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                return getter();
            }

            throw new NotSupportedException("Unsupported expression type for value extraction.");
        }

        public async Task<int> ExecuteCountQueryAsync(string countQuery)
        {
            using (var connection = _dbContext.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = countQuery;
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PMS.Shared.BaseRepository.Helper;
using PMS.Shared.BaseRepository.Queries;
using System.Linq.Expressions;

namespace PMS.Shared.BaseRepository.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginationResult<T> GetPage<T>(
            this IQueryable<T> queryable,
            PageQuery pageQuery,
            Expression<Func<T, bool>>? filterBy = null,
            List<(Expression<Func<T, object>> OrderBy, bool IsAscending)>? orderBy = null
        )
        {
            queryable = ApplySortingAndFiltering(queryable, orderBy, filterBy);

            var paginatedResult = PagedModel<T>.ToPagedList(queryable, pageQuery.PageNumber, pageQuery.PageSize);
            return paginatedResult;
        }

        public static async Task<List<T>> GetAsync<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>>? filterBy = null,
            List<(Expression<Func<T, object>> OrderBy, bool IsAscending)>? orderBy = null
        )
        {
            queryable = ApplySortingAndFiltering(queryable, orderBy, filterBy);

            return await queryable.ToListAsync();
        }

        private static IQueryable<T> ApplySortingAndFiltering<T>(
            IQueryable<T> queryable,
            List<(Expression<Func<T, object>> OrderBy, bool IsAscending)>? orderBy,
            Expression<Func<T, bool>>? filterBy = null
        )
        {
            if (filterBy != null)
            {
                queryable = queryable.Where(filterBy);
            }

            if (orderBy != null && orderBy.Any())
            {
                foreach (var (OrderBy, IsAscending) in orderBy)
                {
                    if (IsAscending)
                    {
                        queryable = queryable.OrderBy(OrderBy);
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(OrderBy);
                    }
                }
            }

            return queryable;
        }
    }
}
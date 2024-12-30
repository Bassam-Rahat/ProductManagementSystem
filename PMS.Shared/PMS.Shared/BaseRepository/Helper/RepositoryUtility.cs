using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PMS.Shared.BaseRepository.Pagging;
using PMS.Shared.BaseRepository.Extensions;

namespace PMS.Shared.BaseRepository.Helper
{
    public static class RepositoryUtility<T> where T : class
    {
        public static PaginationResult<T> GetPaginationAsync(IQueryable<T> query, PaggingParmeter param, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            if (filter != null)
                query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            }
            if (!string.IsNullOrEmpty(param.OrderBy))
                query = query.OrderByPropertyOrField(param.OrderBy, param.IsAscending);
            var clients = PagedModel<T>.ToPagedList(query, param.PageNumber, param.PageSize);
            return clients;
        }
    }
    public static class IdentityRepositoryUtility<T> where T : class
    {
        public static PaginationResult<T> GetPaginationAsync(IQueryable<T> query, PaggingParmeter param, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            if (filter != null)
                query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            }
            if (!string.IsNullOrEmpty(param.OrderBy))
                query = query.OrderByPropertyOrField(param.OrderBy, param.IsAscending);
            query = query.IsDeletedPropertyOrField();
            var users = PagedModel<T>.ToPagedList(query, param.PageNumber, param.PageSize);
            return users;
        }
    }
}

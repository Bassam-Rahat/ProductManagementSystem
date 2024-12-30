namespace PMS.Shared.BaseRepository.Helper
{
    public class PagedModel<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public static PaginationResult<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginationResult<T>(items, pageNumber, pageSize, count);
        }
    }

    public class PaginationResult<T>
    {
        public PaginationResult(List<T> data, int currentpage, int pagesize, int totalcount)
        {
            CurrentPage = currentpage;
            TotalPages = (int)Math.Ceiling(totalcount / (double)pagesize);
            PageSize = pagesize;
            Results = data;
            TotalCount = totalcount;

        }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Results { get; set; }
    }
}

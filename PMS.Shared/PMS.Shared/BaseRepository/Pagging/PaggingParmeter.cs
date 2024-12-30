namespace PMS.Shared.BaseRepository.Pagging
{
    public class PaggingParmeter
    {
        const int maxPageSize = 500;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public bool IsAscending { get; set; }
        public string? OrderBy { get; set; }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
    }
}

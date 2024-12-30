namespace PMS.Shared.BaseRepository.Queries
{
    public class OrderByQuery
    {
        public bool? IsAscending { get; set; }
        public string? OrderBy { get; set; }
    }
}
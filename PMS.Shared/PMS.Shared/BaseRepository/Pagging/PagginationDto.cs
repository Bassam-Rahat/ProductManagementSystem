using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Shared.BaseRepository.Pagging
{
    public class PagginationDto<T> where T : class
    {
        public PagginationDto()
        {


        }
        public PagginationDto(IEnumerable<T> doctypes, int totalnumberofpage, int pagenumber, int pagesize, int totalNumberOfRecords)
        {
            Results = doctypes;
            TotalNumberOfPages = totalnumberofpage;
            PageNumber = pagenumber;
            PageSize = pagesize;
            TotalNumberOfRecords = totalNumberOfRecords;

        }
        public int TotalNumberOfPages { get; set; } = 0;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}

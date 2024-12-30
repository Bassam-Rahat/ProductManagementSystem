using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Shared.BaseRepository.Gets
{
    public class GetRequest
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public bool? UsePaging { get; set; }
        public string? OrderBy { get; set; }
        public string? FilterBy { get; set; }
        public bool? IsAscending { get; set; }
    }
}

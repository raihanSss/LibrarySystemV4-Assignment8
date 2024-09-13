using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class SearchCriteria
    {
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
    }
}

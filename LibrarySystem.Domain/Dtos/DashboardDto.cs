using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Dtos
{
    public class DashboardDto
    {
        public int TotalBooks { get; set; }
        public IEnumerable<User> MostActiveMember {  get; set; }
        public IEnumerable<OverdueBookDto> OverdueBooks { get; set; }
        public Dictionary<string, int> BooksCountPerCategory { get; set; }

        public IEnumerable<Process> Processes { get; set; }
    }
}

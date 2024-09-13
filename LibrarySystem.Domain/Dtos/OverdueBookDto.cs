using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Dtos
{
    public class OverdueBookDto
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public int OverdueDays { get; set; }
        public DateTime DateReturn { get; set; }
    }
}

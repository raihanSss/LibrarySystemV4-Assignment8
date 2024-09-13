using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class Process
    {
        public int Id_process { get; set; }
        public string processname { get; set; }
        public string description { get; set; }
        public DateTime startdate { get; set; }
        public DateTime? enddate { get; set; }

        // Relasi ke Request
        public ICollection<Request> Requests { get; set; }
    }
}

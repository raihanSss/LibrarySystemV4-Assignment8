using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class NewBookRequest
    {
        public string Title { get; set; } 
        public string Isbn { get; set; } 
        public string Author { get; set; }
        public string Publisher { get; set; } 
        public string RequesterId { get; set; } 
    }
}

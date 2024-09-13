using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class WorkflowAction
    {
        public int Id_action { get; set; }
        public int Id_request { get; set; }
        public int Id_step { get; set; }
        public string action { get; set; }
        public DateTime actiondate { get; set; }
        public string comments { get; set; }

       
        public Request Request { get; set; }
        public WorkflowSequence WorkflowSequence { get; set; }
    }
}

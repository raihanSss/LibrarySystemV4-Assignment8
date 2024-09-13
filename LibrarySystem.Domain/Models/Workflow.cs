using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class Workflow
    {
        public int Id_workflow { get; set; }
        public string workflowname { get; set; }
        public string description { get; set; }

     
        public ICollection<Request> Requests { get; set; }  
        public ICollection<WorkflowSequence> WorkflowSequences { get; set; }
    }
}

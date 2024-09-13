using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class Request
    {
        public int Id_request { get; set; }
        public int Id_workflow { get; set; }
        public string Id_requester { get; set; }
        public string requesttype { get; set; }
        public string status { get; set; }
        public int currentstepId { get; set; }
        public DateTime requestdate { get; set; }
        public int Id_process { get; set; }

        // Relasi ke Process
        public Process Process { get; set; }
        // Relasi ke Workflow
        public Workflow Workflow { get; set; }
        // Relasi ke WorkflowActions
        public ICollection<WorkflowAction> WorkflowActions { get; set; }
    }
}

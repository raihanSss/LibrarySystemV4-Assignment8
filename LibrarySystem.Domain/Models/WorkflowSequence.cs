using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Domain.Models
{
    public class WorkflowSequence
    {
        public int Id { get; set; }
        public int Id_workflow { get; set; }
        public int steporder { get; set; }
        public string stepname { get; set; }
        public string requiredrole { get; set; }
        

        public Workflow Workflow { get; set; }
        public IdentityRole Role { get; set; } 
        public ICollection<WorkflowAction> WorkflowActions { get; set; }
    }
}

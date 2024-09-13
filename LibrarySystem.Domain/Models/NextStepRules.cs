using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Domain.Models
{
    public class NextStepRules
    {
        public int Id_rule { get; set; }
        public int Id_currentstep { get; set; }
        public int Id_nextstep { get; set; }
        public string conditiontype { get; set; }
        public string conditionvalue { get; set; }

        
        public WorkflowSequence CurrentStep { get; set; }
        public WorkflowSequence NextStep { get; set; }
    }
}

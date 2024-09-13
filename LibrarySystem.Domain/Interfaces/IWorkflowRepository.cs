using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IWorkflowRepository
    {
        Task<WorkflowSequence> GetInitialStepAsync(int workflowId);
        Task AddWorkflowActionAsync(WorkflowAction action);
        Task SaveChangesAsync();
        Task<IEnumerable<Process>> GetAllProcessesAsync();
    }
}

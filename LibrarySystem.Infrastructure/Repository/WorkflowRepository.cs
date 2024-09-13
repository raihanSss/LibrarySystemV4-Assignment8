using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repository
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly LibrarysystemDbContext _context;

        public WorkflowRepository(LibrarysystemDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowSequence> GetInitialStepAsync(int workflowId)
        {
            
            return await _context.WorkflowSequences
                .Where(ws => ws.Id_workflow == workflowId && ws.steporder == 1)
                .FirstOrDefaultAsync();
        }

        public async Task AddWorkflowActionAsync(WorkflowAction action)
        {
            _context.WorkflowActions.Add(action);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Process>> GetAllProcessesAsync()
        {
          return await _context.Processes.ToListAsync();
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IDashboardService
    {
        //Task<int> GetTotalBookAsync();
        //Task<IEnumerable<User>> GetMostActiveMemberAsync(int count);
        //Task<IEnumerable<Borrow>> GetOverdueBooksAsync();

        Task<DashboardDto> GetDashboardAsync();
    }
}

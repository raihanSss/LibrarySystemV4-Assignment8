using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IBorrowRepository
    {
        Task AddBorrowBookAsync(Borrow borrow);
        Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync();
        Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId);
        Task<IEnumerable<Borrow>> GetBorrowsByCriteriaAsync(SearchCriteria criteria);
        Task<IEnumerable<OverdueBookDto>> GetOverdueBooksAsync();

    }
}

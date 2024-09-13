using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IBorrowService
    {

        Task<string> AddBorrowBookAsync(Borrow borrow);
        Task<byte[]> GenerateOverdueReportPdfAsync();
        Task<byte[]> GenerateUserReportPdfAsync(int userId);
        Task<byte[]> GenerateSignOutReportAsync(SearchCriteria criteria);
    }
}

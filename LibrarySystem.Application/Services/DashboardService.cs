using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.VisualBasic;

namespace LibrarySystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IWorkflowRepository _workflowRepository;

        public DashboardService(IBookRepository bookRepository, IUserRepository userRepository, IBorrowRepository borrowRepository, IWorkflowRepository workflowRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _borrowRepository = borrowRepository;
            _workflowRepository = workflowRepository;
        }

        //public async Task<int> GetTotalBookAsync()
        //{
        //    return await _bookRepository.GetTotalBooksAsync();
        //}

        //public async Task<IEnumerable<User>> GetMostActiveMemberAsync(int count)
        //{
        //    return await _userRepository.GetMostActiveMemberAsync(count);
        //}

        //public async Task<IEnumerable<Borrow>> GetOverdueBooksAsync()
        //{
        //    return await _borrowRepository.GetOverdueBooksAsync();
        //}

        //public async Task<Dictionary<string, int>> GetBooksCountPerCategoryAsync()
        //{
        //    return await _bookRepository.GetBooksCountPerCategoryAsync();
        //}


        public async Task<DashboardDto> GetDashboardAsync()
        {
            var totalBooks = await _bookRepository.GetTotalBooksAsync();
            var activeMember = await _userRepository.GetMostActiveMemberAsync(10);
            var overdueBooks = await _borrowRepository.GetOverdueBooksAsync();
            var booksCountCat = await _bookRepository.GetBooksCountPerCategoryAsync();
            var process = await _workflowRepository.GetAllProcessesAsync();

            return new DashboardDto
            {
                TotalBooks = totalBooks,
                MostActiveMember = activeMember,
                OverdueBooks = overdueBooks,
                BooksCountPerCategory = booksCountCat,
                Processes = process
            };
        }

        
    }
}

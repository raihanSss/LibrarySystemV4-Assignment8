using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repository
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibrarysystemDbContext _context;

        public BorrowRepository(LibrarysystemDbContext context)
        {
            _context = context;
        }

        public async Task AddBorrowBookAsync(Borrow borrow)
        {
            var bookExists = await _context.Books.AnyAsync(b => b.IdBook == borrow.IdBook);
            if (!bookExists)
            {
                throw new Exception("Buku tidak ditemukan.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.IdUser == borrow.IdUser);
            if (!userExists)
            {
                throw new Exception("user tidak ada");
            }

            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Borrows
                             .Where(b => b.DateReturn > b.DateBorrow.AddMonths(1) && b.Penalty > 0)
                             .Include(b => b.IdUserNavigation)
                             .Include(b => b.IdBookNavigation)
                             .OrderBy(b => b.IdUserNavigation)

        .ToListAsync();
        }

        public async Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId)
        {
            return await _context.Borrows
                             .Where(b => b.IdUser == userId)
                             .Include(b => b.IdUserNavigation)
                             .Include(b => b.IdBookNavigation)
                             .OrderBy(b => b.IdUserNavigation)
                             .ToListAsync();
        }


        public async Task<IEnumerable<Borrow>> GetBorrowsByCriteriaAsync(SearchCriteria criteria)
        {
            var borrowsQuery = _context.Borrows
                .Include(b => b.IdBookNavigation)
                .Where(b => b.DateBorrow >= criteria.startdate && b.DateBorrow <= criteria.enddate);

            return await borrowsQuery.ToListAsync();
        }

        public async Task<IEnumerable<OverdueBookDto>> GetOverdueBooksAsync()
        {
            var currentDate = DateTime.Now; 
            return await _context.Borrows
                .Where(b => b.DateReturn > b.DateBorrow.AddMonths(1) && b.Penalty > 0)
                .Include(b => b.IdUserNavigation)
                .Include(b => b.IdBookNavigation)
                .OrderBy(b => b.IdUserNavigation.Fname) 
                .Select(b => new OverdueBookDto
                {
                    FullName = $"{b.IdUserNavigation.Fname} {b.IdUserNavigation.Lname}", 
                    Title = b.IdBookNavigation.Title, 
                    OverdueDays = (b.DateReturn - b.DateBorrow.AddMonths(1)).Days, 
                    DateReturn = b.DateReturn 
                })
                .ToListAsync();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Interfaces;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using LibrarySystem.Domain.Models;
using Microsoft.Extensions.Options;

namespace LibrarySystem.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly PenaltySettings _penaltySettings;

        public BorrowService(IBorrowRepository borrowRepository, IOptions<PenaltySettings> penaltySettings)
        {
            _borrowRepository = borrowRepository;
            _penaltySettings = penaltySettings.Value;
        }


        public async Task<string> AddBorrowBookAsync(Borrow borrow)
        {
            var newBorrow = new Borrow
            {
                IdUser = borrow.IdUser,
                IdBook = borrow.IdBook,
                DateBorrow = DateTime.Now,
                DateReturn = DateTime.Now.AddMonths(1), 
                Penalty = 0 
            };

            await _borrowRepository.AddBorrowBookAsync(newBorrow);

            return "Data peminjaman buku berhasil ditambah";
        }

        public async Task<byte[]> GenerateOverdueReportPdfAsync()
        {
            var overdueBorrows = await _borrowRepository.GetOverdueBorrowsAsync();

            string htmlContent = "<h1>Overdue Books Report</h1>";
            htmlContent += "<table>";
            htmlContent += "<tr>" +
                "<th>User Id</th>" +
                "<th>First Name</th>" +
                "<th>Last Name</th>" +
                "<th>Library Card Number</th>" +
                "<th>Book Title</th>" +
                "<th>Date Borrowed</th>" +
                "<th>Date Returned</th>" +
                "<th>Days Overdue</th>" +
                "<th>Penalty</th>" +
                "</tr>";

            foreach (var borrow in overdueBorrows)
            {
                var overdueDays = (borrow.DateReturn - borrow.DateBorrow.AddMonths(1)).Days;
                var penalty = overdueDays * _penaltySettings.PenaltyPerDay;

                htmlContent += $"<tr>" +
                               $"<td>{borrow.IdUserNavigation.IdUser}</td>" +
                               $"<td>{borrow.IdUserNavigation.Fname}</td>" +
                               $"<td>{borrow.IdUserNavigation.Lname}</td>" +
                               $"<td>{borrow.IdUserNavigation.Librarycard}</td>" +
                               $"<td>{borrow.IdBookNavigation.Title}</td>" +
                               $"<td>{borrow.DateBorrow:yyyy-MM-dd}</td>" +
                               $"<td>{borrow.DateReturn:yyyy-MM-dd}</td>" +
                               $"<td>{overdueDays} days</td>" +
                               $"<td>{penalty:C}</td>" +
                               $"</tr>";
            }

            htmlContent += "</table>";

            var document = new PdfDocument();
            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Landscape,
                PageSize = PageSize.A4
            };

            string cssStr = File.ReadAllText(@"./Template/report/style1.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);
            PdfGenerator.AddPdfPages(document, htmlContent, config, css);

            MemoryStream stream = new MemoryStream();

            document.Save(stream, false);

            byte[] bytes = stream.ToArray();

            return bytes;
        }

        public async Task<byte[]> GenerateUserReportPdfAsync(int userId)
        {
            var userBorrows = await _borrowRepository.GetBorrowsByUserIdAsync(userId);

            var user = userBorrows.FirstOrDefault()?.IdUserNavigation;

            if (user == null)
            {
                throw new Exception("User not found");
            }

            string Name = $"{user.Fname} {user.Lname}";

            string htmlContent = $"<h1>Report for {Name}</h1>";
            htmlContent += "<table>";
            htmlContent += "<tr>" +
                "<th>Full Name</th>" +
                "<th>Book Title</th>" +
                "<th>Date Borrowed</th>" +
                "<th>Date Returned</th>" +
                "<th>Days Overdue</th>" +
                "<th>Penalty</th>" +
                "</tr>";

            foreach (var borrow in userBorrows)
            {

                string fullName = $"{borrow.IdUserNavigation.Fname} {borrow.IdUserNavigation.Lname}";
                int overdueDays = (borrow.DateReturn - borrow.DateBorrow.AddMonths(1)).Days;
                int penalty = overdueDays > 0 ? overdueDays * _penaltySettings.PenaltyPerDay : 0;

                htmlContent += $"<tr>" +
                               $"<td>{fullName}</td>" +
                               $"<td>{borrow.IdBookNavigation.Title}</td>" +
                               $"<td>{borrow.DateBorrow:yyyy-MM-dd}</td>" +
                               $"<td>{borrow.DateReturn:yyyy-MM-dd}</td>" +
                               $"<td>{(overdueDays > 0 ? overdueDays : 0)} days</td>" + 
                               $"<td>{penalty:C}</td>" +
                               $"</tr>";
            }

            htmlContent += "</table>";

            var document = new PdfDocument();
            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Landscape,
                PageSize = PageSize.A4
            };

            string cssStr = File.ReadAllText(@"./Template/report/style1.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);
            PdfGenerator.AddPdfPages(document, htmlContent, config, css);

            MemoryStream stream = new MemoryStream();

            document.Save(stream, false);

            byte[] bytes = stream.ToArray();

            return bytes;
        }

        public async Task<byte[]> GenerateSignOutReportAsync(SearchCriteria criteria)
        {
            var borrows = await _borrowRepository.GetBorrowsByCriteriaAsync(criteria);

            var groupedBorrows = borrows.GroupBy(b => b.IdBookNavigation.Category)
                                    .Select(group => new
                                    {
                                        Category = group.Key,
                                        Books = group.ToList(),
                                        Count = group.Count()
                                    });


            string htmlContent = $"<h1>Sign Out Books Report</h1>";
            htmlContent += $"<p>Time Period: {criteria.startdate:yyyy-MM-dd} to {criteria.enddate:yyyy-MM-dd}</p>";
            htmlContent += "<table>";
            htmlContent += "<tr><th>Category</th><th>Number of Books</th></tr>";

            foreach (var group in groupedBorrows)
            {
                htmlContent += $"<tr>" +
                    $"<td>{group.Category}</td>" +
                    $"<td>{group.Count}</td>" +
                    $"</tr>";
            }


            var document = new PdfDocument();
            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Portrait,
                PageSize = PageSize.A4
            };

            string cssStr = File.ReadAllText(@"./Template/report/style1.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);
            PdfGenerator.AddPdfPages(document, htmlContent, config, css);

            MemoryStream stream = new MemoryStream();

            document.Save(stream, false);

            byte[] bytes = stream.ToArray();

            return bytes;
        }
    }
}

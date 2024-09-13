using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace LibrarySystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWorkflowRepository _workflowRepository;

        public BookService(IBookRepository bookRepository, IWorkflowRepository workflowRepository)
        {
            _bookRepository = bookRepository;
            _workflowRepository = workflowRepository;
        }

        public async Task<string> AddBookAsync(Book book)
        {
            await _bookRepository.AddBookAsync(book);
            return "Data buku berhasil ditambah";
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task<string> UpdateBookAsync(Book book, int id)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Isbn = book.Isbn;
                existingBook.Category = book.Category;
                existingBook.Description = book.Description;
                existingBook.Location = book.Location;
                existingBook.Publisher = book.Publisher;
                existingBook.Status = book.Status;
                existingBook.Availablebook = book.Availablebook;
                existingBook.Language = book.Language;

                await _bookRepository.UpdateBookAsync(existingBook);
                return "Data buku berhasil diubah";
            }
            return "Buku tidak ditemukan";
        }

        public async Task<string> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book != null)
            {
                await _bookRepository.DeleteBookAsync(id);
                return "Data buku berhasil dihapus";
            }
            return "Buku tidak ditemukan";
        }


        public async Task<string> RequestNewBookAsync(NewBookRequest newBookRequest)
        {
            
            var initialStep = await _workflowRepository.GetInitialStepAsync(1); 

            if (initialStep == null)
            {
                return "Initial workflow step not found.";
            }

            var request = new Request
            {
                Id_workflow = initialStep.Id_workflow, 
                Id_requester = newBookRequest.RequesterId,
                requesttype = "NewBook",
                status = "Pending",
                currentstepId = initialStep.Id, 
                requestdate = DateTime.UtcNow,
                Id_process = 1 
            };

            await _bookRepository.RequestNewBookAsync(request);


            var action = new WorkflowAction
            {
                Id_request = request.Id_request,
                Id_step = initialStep.Id, 
                action = "Submit",
                actiondate = DateTime.UtcNow,
                comments = $"Requested new book: {newBookRequest.Title}, ISBN: {newBookRequest.Isbn}, Author: {newBookRequest.Author}, Publisher: {newBookRequest.Publisher}"
            };

            await _workflowRepository.AddWorkflowActionAsync(action);

            return "New book request submitted successfully.";
        }


        public async Task<byte[]> generatereportpdf()
        {

            var bookList = await _bookRepository.GetAllBooksAsync();

            string htmlcontent = String.Empty;

            htmlcontent += "<table>";

            htmlcontent += "<tr>" +
                "<td>Id</td>" +
                "<td>Title</td>" +
                "<td>Author</td>" +
                "<td>Category</td>" +
                "<td>Description</td>" +
                "</tr>";

            bookList.ToList().ForEach(item => {

                htmlcontent += "<tr style='border:1px solid #ccc;>";

                htmlcontent += "<td>" + item.IdBook + "</td>";

                htmlcontent += "<td>" + item.Title + "</td>";

                htmlcontent += "<td>" + item.Author + "</td>";

                htmlcontent += "<td>" + item.Category + "</td>";

                htmlcontent += "<td>" + item.Description + "</td>";

                htmlcontent += "</tr>";

            });

            htmlcontent += "</table>";

            var document = new PdfDocument();

            var config = new PdfGenerateConfig();

            config.PageOrientation = PageOrientation.Landscape;

            config.PageSize = PageSize.A4;

            string cssStr = File.ReadAllText(@"./Template/report/style.css");

            CssData css = PdfGenerator.ParseStyleSheet(cssStr);

            PdfGenerator.AddPdfPages(document, htmlcontent, config, css);

            MemoryStream stream = new MemoryStream();

            document.Save(stream, false);

            byte[] bytes = stream.ToArray();

            return bytes;

        }
    }
}

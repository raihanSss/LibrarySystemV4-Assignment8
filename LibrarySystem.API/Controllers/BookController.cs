using LibrarySystem.Domain;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    //[Authorize]
    [Route("api/buku")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookRepository _bookRepository;
        private readonly IEmailService _emailService;
        

        public BookController(IBookService bookService, IBookRepository bookRepository, IEmailService emailService)
        {
            _bookService = bookService;
            _bookRepository = bookRepository;
            _emailService = emailService;
        }


        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {

            var books = await _bookService.GetAllBooksAsync();

            var mailData = new MailData
            {
                EmailToName = "Bellerick",
                EmailSubject = "List Buku",
                EmailBody = $"Found {books.Count()} books based on your query."
            };

            mailData.EmailToIds.Add("vanquish00vip@gmail.com");
            mailData.EmailToIds.Add("raihansss34@gmail.com");

            var registerModel = new RegisterModel();
            var emailResult = await _emailService.SendMail(mailData,registerModel);
            if (!emailResult)
            {
                return StatusCode(500, "Error sending email.");
            }

            return Ok(books);
        }

        [Authorize(Roles = "Librarian")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound("Buku tidak ditemukan");
            }
            return Ok(book);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Data buku tidak valid");
            }

            var result = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.IdBook }, result);
        
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || book.IdBook != id)
            {
                return BadRequest("Data buku tidak valid");
            }

            var result = await _bookService.UpdateBookAsync(book, id);
            if (result == "Buku tidak ditemukan")
            {
                return NotFound(result);
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (result == "Buku tidak ditemukan")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooksAsync([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Masukan judul buku");
                
            }

            var books = await _bookRepository.SearchBooksAsync(title);

            if (books == null || !books.Any())
            {
                return NotFound("Buku tidak ditemukan");
            }

            return Ok(books);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> FilterBooksAsync([FromQuery] BookSearchCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest("masukan filternya");
            }

            var books = await _bookRepository.FilterBooksAsync(criteria);

            if (books == null || !books.Any())
            {
                return NotFound("No books found");
            }

            return Ok(books);
        }

        [HttpPost("request-new-book")]
        //[Authorize(Roles = "Library User")]
        public async Task<IActionResult> RequestNewBook([FromBody] NewBookRequest newBookRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookService.RequestNewBookAsync(newBookRequest);

            if (result.Contains("error", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("report")]
        public async Task<IActionResult> Report()
        {
            var Filename = "BookReport.pdf";
            var file = await _bookService.generatereportpdf();
            return File(file, "application/pdf", Filename);
        }

    }
}

using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Route("api/borrow")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddBorrow([FromBody] Borrow borrow)
        {
            
            var result = await _borrowService.AddBorrowBookAsync(borrow);
            if (result.Contains("berhasil"))
            {
                return Ok(new { message = result });
            }

           
            return BadRequest(new { error = result });
        }

        [HttpGet("overdue-report")]
        public async Task<IActionResult> GetOverdueReport()
        {
            var Filename = "OverdueReport.pdf";
            var file = await _borrowService.GenerateOverdueReportPdfAsync();
            return File(file, "application/pdf", Filename);
        }


        [HttpGet("user-report/{userId}")]
        public async Task<IActionResult> GetUserReport(int userId)
        {
            var nameFile = "UserReport.pdf";
            var file = await _borrowService.GenerateUserReportPdfAsync(userId);
            return File(file, "application/pdf", nameFile);
        }

        [HttpPost("signout")]
        public async Task<IActionResult> GenerateSignOutReport([FromBody] SearchCriteria criteria)
        {
            if (criteria.startdate == null || criteria.enddate == null)
            {
                return BadRequest(new { error = "Start date and end date are required." });
            }

            
            if (criteria.startdate > criteria.enddate)
            {
                return BadRequest(new { error = "Start date cannot be after end date." });
            }

            try
            {
               
                var pdfBytes = await _borrowService.GenerateSignOutReportAsync(criteria);

                return File(pdfBytes, "application/pdf", "SignOutBooksReport.pdf");
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}

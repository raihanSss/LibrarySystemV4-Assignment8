using LibrarySystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Route("api/dash")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("kpi")]
        public async Task<IActionResult> GetDashboardKpi()
        {
            var dashboardKpis = await _dashboardService.GetDashboardAsync();
            return Ok(dashboardKpis);

        }
    }
}

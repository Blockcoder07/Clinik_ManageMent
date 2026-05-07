using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clini_Management_System.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("revenue-summary")]
    public async Task<IActionResult> RevenueSummary([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
    {
        var result = await _dashboardService.GetRevenueSummaryAsync(from, to, ct);
        return Ok(ApiResponse<RevenueSummaryDto>.Ok(result, "Revenue summary fetched."));
    }
}

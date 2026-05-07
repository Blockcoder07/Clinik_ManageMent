using System.Net.Mime;
using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clini_Management_System.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/dashboard")]
[Produces(MediaTypeNames.Application.Json)]
public class DashboardController : ControllerBase
{
    #region Fields

    private readonly IDashboardService _dashboardService;

    #endregion

    #region Constructor

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    #endregion

    #region Endpoints

    [HttpGet("revenue-summary")]
    [ProducesResponseType(typeof(ApiResponse<RevenueSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevenueSummary(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
    {
        var result = await _dashboardService.GetRevenueSummaryAsync(from, to, ct);
        return Ok(ApiResponse<RevenueSummaryDto>.Ok(result, "Revenue summary fetched."));
    }

    #endregion
}

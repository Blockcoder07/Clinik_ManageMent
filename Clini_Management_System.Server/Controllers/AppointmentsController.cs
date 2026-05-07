using System.Net.Mime;
using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clini_Management_System.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/appointments")]
[Produces(MediaTypeNames.Application.Json)]
public class AppointmentsController : ControllerBase
{
    #region Fields

    private readonly IAppointmentService _appointmentService;

    #endregion

    #region Constructor

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    #endregion

    #region Endpoints

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _appointmentService.GetPagedAsync(from, to, pageNumber, pageSize, ct);
        return Ok(ApiResponse<PagedResult<AppointmentDto>>.Ok(result, "Appointments fetched."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateDto request, CancellationToken ct)
    {
        var result = await _appointmentService.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<AppointmentDto>.Ok(result, "Appointment created."));
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] AppointmentStatusUpdateDto request, CancellationToken ct)
    {
        var result = await _appointmentService.UpdateStatusAsync(id, request, ct);
        return Ok(ApiResponse<AppointmentDto>.Ok(result, "Appointment updated."));
    }

    #endregion
}

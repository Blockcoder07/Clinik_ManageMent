using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clini_Management_System.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateDto request, CancellationToken ct)
    {
        var result = await _appointmentService.CreateAsync(request, ct);
        return Ok(ApiResponse<AppointmentDto>.Ok(result, "Appointment created."));
    }

    [HttpGet]
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

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] AppointmentStatusUpdateDto request, CancellationToken ct)
    {
        var result = await _appointmentService.UpdateStatusAsync(id, request, ct);
        return Ok(ApiResponse<AppointmentDto>.Ok(result, "Appointment updated."));
    }
}

using System.Net.Mime;
using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clini_Management_System.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/patients")]
[Produces(MediaTypeNames.Application.Json)]
public class PatientsController : ControllerBase
{
    #region Fields

    private readonly IPatientService _patientService;

    #endregion

    #region Constructor

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    #endregion

    #region Endpoints

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PatientDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = false,
        CancellationToken ct = default)
    {
        var result = await _patientService.GetPagedAsync(pageNumber, pageSize, search, sortBy, sortDesc, ct);
        return Ok(ApiResponse<PagedResult<PatientDto>>.Ok(result, "Patients fetched."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PatientDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] PatientCreateDto request, CancellationToken ct)
    {
        var result = await _patientService.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<PatientDto>.Ok(result, "Patient created."));
    }

    #endregion
}

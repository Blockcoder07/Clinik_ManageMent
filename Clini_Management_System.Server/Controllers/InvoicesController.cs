using System.Net.Mime;
using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clini_Management_System.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/invoices")]
[Produces(MediaTypeNames.Application.Json)]
public class InvoicesController : ControllerBase
{
    #region Fields

    private readonly IInvoiceService _invoiceService;

    #endregion

    #region Constructor

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    #endregion

    #region Endpoints

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InvoiceDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] InvoiceCreateDto request, CancellationToken ct)
    {
        var result = await _invoiceService.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<InvoiceDto>.Ok(result, "Invoice created."));
    }

    #endregion
}

using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Models.Enums;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Services.Implementations;

public sealed class InvoiceService : IInvoiceService
{
    #region Fields

    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<InvoiceService> _logger;

    #endregion

    #region Constructor

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        IAppointmentRepository appointmentRepository,
        ILogger<InvoiceService> logger)
    {
        _invoiceRepository = invoiceRepository;
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    #endregion

    #region Public Methods

    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto request, CancellationToken ct = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, ct)
            ?? throw new NotFoundException("Appointment not found.");

        if (appointment.Status != AppointmentStatus.Completed)
            throw new BadRequestException("Invoice can only be created for completed appointments.");

        var invoice = new Invoice
        {
            AppointmentId = request.AppointmentId,
            Amount = request.Amount
        };

        try
        {
            await _invoiceRepository.AddAsync(invoice, ct);
            await _invoiceRepository.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Duplicate invoice creation prevented for appointment {AppointmentId}", request.AppointmentId);
            throw new ConflictException("Invoice already exists for this appointment.");
        }

        return Map(invoice);
    }

    #endregion

    #region Private Methods

    private static InvoiceDto Map(Invoice invoice) => new()
    {
        Id            = invoice.Id,
        AppointmentId = invoice.AppointmentId,
        Amount        = invoice.Amount,
        CreatedAt     = invoice.CreatedAt
    };

    #endregion
}

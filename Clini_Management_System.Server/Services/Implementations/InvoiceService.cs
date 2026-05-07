using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Models.Enums;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Services.Implementations;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository, IAppointmentRepository appointmentRepository)
    {
        _invoiceRepository = invoiceRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto request, CancellationToken ct = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, ct)
                          ?? throw new NotFoundException("Appointment not found.");

        if (appointment.Status != AppointmentStatus.Completed)
            throw new BadRequestException("Invoice can only be created for completed appointments.");

        if (request.Amount <= 0)
            throw new BadRequestException("Amount must be greater than zero.");

        var invoice = new Invoice
        {
            AppointmentId = request.AppointmentId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _invoiceRepository.AddAsync(invoice, ct);
            await _invoiceRepository.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            throw new ConflictException("Invoice already exists for this appointment.");
        }

        return new InvoiceDto
        {
            Id = invoice.Id,
            AppointmentId = invoice.AppointmentId,
            Amount = invoice.Amount,
            CreatedAt = invoice.CreatedAt
        };
    }
}

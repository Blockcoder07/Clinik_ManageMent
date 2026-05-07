using Clini_Management_System.Server.Models.DTOs;

namespace Clini_Management_System.Server.Services.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceDto> CreateAsync(InvoiceCreateDto request, CancellationToken ct = default);
}

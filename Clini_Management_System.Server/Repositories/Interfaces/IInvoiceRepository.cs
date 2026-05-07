using Clini_Management_System.Server.Models.Entities;

namespace Clini_Management_System.Server.Repositories.Interfaces;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

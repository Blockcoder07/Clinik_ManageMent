using Clini_Management_System.Server.Data;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;

namespace Clini_Management_System.Server.Repositories.Implementations;

public sealed class InvoiceRepository : IInvoiceRepository
{
    #region Fields

    private readonly AppDbContext _db;

    #endregion

    #region Constructor

    public InvoiceRepository(AppDbContext db)
    {
        _db = db;
    }

    #endregion

    #region Public Methods

    public Task AddAsync(Invoice invoice, CancellationToken ct = default) =>
        _db.Invoices.AddAsync(invoice, ct).AsTask();

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);

    #endregion
}

using Clini_Management_System.Server.Data;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Repositories.Implementations;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Appointment?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Appointments.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Appointment?> GetByIdWithPatientAsync(int id, CancellationToken ct = default) =>
        _db.Appointments.Include(x => x.Patient).FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<(IReadOnlyList<Appointment> Items, int TotalCount)> GetPagedAsync(
        DateTime? from, DateTime? to, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var query = _db.Appointments.Include(x => x.Patient).AsQueryable();

        if (from.HasValue) query = query.Where(x => x.AppointmentDate >= from.Value);
        if (to.HasValue) query = query.Where(x => x.AppointmentDate <= to.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(x => x.AppointmentDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task AddAsync(Appointment appointment, CancellationToken ct = default) =>
        _db.Appointments.AddAsync(appointment, ct).AsTask();

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);

    public Task<int> SaveStatusWithConcurrencyAsync(Appointment appointment, byte[] originalRowVersion, CancellationToken ct = default)
    {
        _db.Entry(appointment).Property(x => x.RowVersion).OriginalValue = originalRowVersion;
        return _db.SaveChangesAsync(ct);
    }
}

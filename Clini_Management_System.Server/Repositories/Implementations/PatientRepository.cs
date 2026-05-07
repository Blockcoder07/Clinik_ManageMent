using Clini_Management_System.Server.Data;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Repositories.Implementations;

public sealed class PatientRepository : IPatientRepository
{
    #region Fields

    private readonly AppDbContext _db;

    #endregion

    #region Constructor

    public PatientRepository(AppDbContext db)
    {
        _db = db;
    }

    #endregion

    #region Public Methods

    public Task<Patient?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Patients.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        _db.Patients.AnyAsync(x => x.Id == id, ct);

    public async Task<(IReadOnlyList<Patient> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? sortBy,
        bool sortDesc,
        CancellationToken ct = default)
    {
        var query = _db.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.MobileNumber.Contains(search));
        }

        query = ApplySorting(query, sortBy, sortDesc);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task AddAsync(Patient patient, CancellationToken ct = default) =>
        _db.Patients.AddAsync(patient, ct).AsTask();

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);

    #endregion

    #region Private Methods

    private static IQueryable<Patient> ApplySorting(IQueryable<Patient> query, string? sortBy, bool sortDesc) =>
        sortBy?.ToLowerInvariant() switch
        {
            "mobilenumber" => sortDesc
                ? query.OrderByDescending(x => x.MobileNumber)
                : query.OrderBy(x => x.MobileNumber),
            _ => sortDesc
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name)
        };

    #endregion
}

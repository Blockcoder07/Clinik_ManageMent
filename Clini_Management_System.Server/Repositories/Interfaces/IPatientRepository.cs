using Clini_Management_System.Server.Models.Entities;

namespace Clini_Management_System.Server.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<(IReadOnlyList<Patient> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, int pageSize, string? search, string? sortBy, bool sortDesc, CancellationToken ct = default);
    Task AddAsync(Patient patient, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

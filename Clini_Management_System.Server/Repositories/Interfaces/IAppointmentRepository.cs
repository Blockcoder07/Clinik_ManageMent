using Clini_Management_System.Server.Models.Entities;

namespace Clini_Management_System.Server.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Appointment?> GetByIdWithPatientAsync(int id, CancellationToken ct = default);
    Task<(IReadOnlyList<Appointment> Items, int TotalCount)> GetPagedAsync(
        DateTime? from, DateTime? to, int pageNumber, int pageSize, CancellationToken ct = default);
    Task AddAsync(Appointment appointment, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<int> SaveStatusWithConcurrencyAsync(Appointment appointment, byte[] originalRowVersion, CancellationToken ct = default);
}

using Clini_Management_System.Server.Data;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Enums;
using Clini_Management_System.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Repositories.Implementations;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _db;

    public DashboardRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RevenueSummaryDto> GetRevenueSummaryAsync(int clinicId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var appointments = _db.Appointments
            .IgnoreQueryFilters()
            .Where(x => x.ClinicId == clinicId
                     && x.AppointmentDate >= from
                     && x.AppointmentDate <= to);

        var totalAppointments = await appointments.CountAsync(ct);
        var completed = await appointments.CountAsync(x => x.Status == AppointmentStatus.Completed, ct);
        var cancelled = await appointments.CountAsync(x => x.Status == AppointmentStatus.Cancelled, ct);

        var totalRevenue = await _db.Invoices
            .IgnoreQueryFilters()
            .Where(x => x.ClinicId == clinicId && x.CreatedAt >= from && x.CreatedAt <= to)
            .SumAsync(x => (decimal?)x.Amount, ct) ?? 0m;

        return new RevenueSummaryDto
        {
            TotalRevenue = totalRevenue,
            TotalAppointments = totalAppointments,
            CompletedAppointments = completed,
            CancelledAppointments = cancelled
        };
    }
}

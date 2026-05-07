using Clini_Management_System.Server.Data;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Enums;
using Clini_Management_System.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Repositories.Implementations;

public sealed class DashboardRepository : IDashboardRepository
{
    #region Fields

    private readonly AppDbContext _db;

    #endregion

    #region Constructor

    public DashboardRepository(AppDbContext db)
    {
        _db = db;
    }

    #endregion

    #region Public Methods

    public async Task<RevenueSummaryDto> GetRevenueSummaryAsync(int clinicId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var appointmentStats = await _db.Appointments
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.ClinicId == clinicId
                     && x.AppointmentDate >= from
                     && x.AppointmentDate <= to)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Completed = g.Count(x => x.Status == AppointmentStatus.Completed),
                Cancelled = g.Count(x => x.Status == AppointmentStatus.Cancelled)
            })
            .FirstOrDefaultAsync(ct);

        var totalRevenue = await _db.Invoices
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.ClinicId == clinicId
                     && x.CreatedAt >= from
                     && x.CreatedAt <= to)
            .SumAsync(x => (decimal?)x.Amount, ct) ?? 0m;

        return new RevenueSummaryDto
        {
            TotalRevenue          = totalRevenue,
            TotalAppointments     = appointmentStats?.Total ?? 0,
            CompletedAppointments = appointmentStats?.Completed ?? 0,
            CancelledAppointments = appointmentStats?.Cancelled ?? 0
        };
    }

    #endregion
}

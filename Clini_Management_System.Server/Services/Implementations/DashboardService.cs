using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;

namespace Clini_Management_System.Server.Services.Implementations;

public sealed class DashboardService : IDashboardService
{
    #region Fields

    private readonly IDashboardRepository _dashboardRepository;
    private readonly ITenantContext _tenant;

    #endregion

    #region Constructor

    public DashboardService(IDashboardRepository dashboardRepository, ITenantContext tenant)
    {
        _dashboardRepository = dashboardRepository;
        _tenant = tenant;
    }

    #endregion

    #region Public Methods

    public Task<RevenueSummaryDto> GetRevenueSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from > to)
            throw new BadRequestException("'from' date cannot be greater than 'to' date.");

        return _dashboardRepository.GetRevenueSummaryAsync(_tenant.ClinicId, from, to, ct);
    }

    #endregion
}

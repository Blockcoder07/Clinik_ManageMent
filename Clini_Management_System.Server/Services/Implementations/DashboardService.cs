using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;

namespace Clini_Management_System.Server.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;
    private readonly ITenantContext _tenant;

    public DashboardService(IDashboardRepository repository, ITenantContext tenant)
    {
        _repository = repository;
        _tenant = tenant;
    }

    public Task<RevenueSummaryDto> GetRevenueSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from > to) throw new BadRequestException("'from' date cannot be greater than 'to' date.");
        return _repository.GetRevenueSummaryAsync(_tenant.ClinicId, from, to, ct);
    }
}

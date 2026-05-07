using Clini_Management_System.Server.Models.DTOs;

namespace Clini_Management_System.Server.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<RevenueSummaryDto> GetRevenueSummaryAsync(int clinicId, DateTime from, DateTime to, CancellationToken ct = default);
}

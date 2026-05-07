using Clini_Management_System.Server.Models.DTOs;

namespace Clini_Management_System.Server.Services.Interfaces;

public interface IDashboardService
{
    Task<RevenueSummaryDto> GetRevenueSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default);
}

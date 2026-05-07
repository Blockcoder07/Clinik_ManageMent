using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;

namespace Clini_Management_System.Server.Services.Interfaces;

public interface IPatientService
{
    Task<PatientDto> CreateAsync(PatientCreateDto request, CancellationToken ct = default);
    Task<PagedResult<PatientDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, bool sortDesc, CancellationToken ct = default);
}

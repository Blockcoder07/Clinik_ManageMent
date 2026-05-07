using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;

namespace Clini_Management_System.Server.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentDto> CreateAsync(AppointmentCreateDto request, CancellationToken ct = default);
    Task<PagedResult<AppointmentDto>> GetPagedAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<AppointmentDto> UpdateStatusAsync(int id, AppointmentStatusUpdateDto request, CancellationToken ct = default);
}

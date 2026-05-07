using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;

namespace Clini_Management_System.Server.Services.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<PatientDto> CreateAsync(PatientCreateDto request, CancellationToken ct = default)
    {
        var entity = new Patient
        {
            Name = request.Name.Trim(),
            MobileNumber = request.MobileNumber.Trim()
        };

        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        return Map(entity);
    }

    public async Task<PagedResult<PatientDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, bool sortDesc, CancellationToken ct = default)
    {
        var (items, total) = await _repository.GetPagedAsync(pageNumber, pageSize, search, sortBy, sortDesc, ct);
        return new PagedResult<PatientDto>(items.Select(Map).ToList(), total, pageNumber, pageSize);
    }

    private static PatientDto Map(Patient p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        MobileNumber = p.MobileNumber
    };
}

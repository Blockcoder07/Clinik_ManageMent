using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;

namespace Clini_Management_System.Server.Services.Implementations;

public sealed class PatientService : IPatientService
{
    #region Fields

    private readonly IPatientRepository _patientRepository;

    #endregion

    #region Constructor

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    #endregion

    #region Public Methods

    public async Task<PatientDto> CreateAsync(PatientCreateDto request, CancellationToken ct = default)
    {
        var patient = new Patient
        {
            Name = request.Name.Trim(),
            MobileNumber = request.MobileNumber.Trim()
        };

        await _patientRepository.AddAsync(patient, ct);
        await _patientRepository.SaveChangesAsync(ct);

        return Map(patient);
    }

    public async Task<PagedResult<PatientDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? sortBy,
        bool sortDesc,
        CancellationToken ct = default)
    {
        var (items, total) = await _patientRepository.GetPagedAsync(pageNumber, pageSize, search, sortBy, sortDesc, ct);
        return new PagedResult<PatientDto>(items.Select(Map).ToList(), total, pageNumber, pageSize);
    }

    #endregion

    #region Private Methods

    private static PatientDto Map(Patient patient) => new()
    {
        Id = patient.Id,
        Name = patient.Name,
        MobileNumber = patient.MobileNumber
    };

    #endregion
}

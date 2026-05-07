using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.DTOs;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Models.Enums;
using Clini_Management_System.Server.Repositories.Interfaces;
using Clini_Management_System.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Services.Implementations;

public sealed class AppointmentService : IAppointmentService
{
    #region Fields

    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<AppointmentService> _logger;

    #endregion

    #region Constructor

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        ILogger<AppointmentService> logger)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _logger = logger;
    }

    #endregion

    #region Public Methods

    public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto request, CancellationToken ct = default)
    {
        if (request.AppointmentDate < DateTime.UtcNow)
            throw new BadRequestException("Appointment cannot be created in the past.");

        if (!await _patientRepository.ExistsAsync(request.PatientId, ct))
            throw new NotFoundException("Patient not found.");

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorName = request.DoctorName.Trim(),
            AppointmentDate = request.AppointmentDate,
            Status = AppointmentStatus.Scheduled
        };

        await _appointmentRepository.AddAsync(appointment, ct);
        await _appointmentRepository.SaveChangesAsync(ct);

        var saved = await _appointmentRepository.GetByIdWithPatientAsync(appointment.Id, ct);
        return Map(saved!);
    }

    public async Task<PagedResult<AppointmentDto>> GetPagedAsync(
        DateTime? from,
        DateTime? to,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var (items, total) = await _appointmentRepository.GetPagedAsync(from, to, pageNumber, pageSize, ct);
        return new PagedResult<AppointmentDto>(items.Select(Map).ToList(), total, pageNumber, pageSize);
    }

    public async Task<AppointmentDto> UpdateStatusAsync(int id, AppointmentStatusUpdateDto request, CancellationToken ct = default)
    {
        var appointment = await _appointmentRepository.GetByIdWithPatientAsync(id, ct)
            ?? throw new NotFoundException("Appointment not found.");

        if (appointment.Status == AppointmentStatus.Completed && request.Status == AppointmentStatus.Cancelled)
            throw new BadRequestException("Completed appointment cannot be cancelled.");

        appointment.Status = request.Status;

        try
        {
            var rowVersion = Convert.FromBase64String(request.RowVersion);
            await _appointmentRepository.SaveStatusWithConcurrencyAsync(appointment, rowVersion, ct);
        }
        catch (FormatException)
        {
            throw new BadRequestException("Invalid row version format.");
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogWarning("Concurrency conflict on appointment {AppointmentId}", id);
            throw new ConflictException("Appointment has been modified by another user. Please reload and try again.");
        }

        return Map(appointment);
    }

    #endregion

    #region Private Methods

    private static AppointmentDto Map(Appointment appointment) => new()
    {
        Id              = appointment.Id,
        PatientId       = appointment.PatientId,
        PatientName     = appointment.Patient?.Name ?? string.Empty,
        DoctorName      = appointment.DoctorName,
        AppointmentDate = appointment.AppointmentDate,
        Status          = appointment.Status,
        RowVersion      = Convert.ToBase64String(appointment.RowVersion)
    };

    #endregion
}

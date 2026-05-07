using Clini_Management_System.Server.Models.DTOs;
using FluentValidation;

namespace Clini_Management_System.Server.Validators;

public sealed class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
{
    public AppointmentCreateValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("Patient is required.");

        RuleFor(x => x.DoctorName)
            .NotEmpty().WithMessage("Doctor name is required.")
            .MaximumLength(150);

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required.");
    }
}

public sealed class AppointmentStatusUpdateValidator : AbstractValidator<AppointmentStatusUpdateDto>
{
    public AppointmentStatusUpdateValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid appointment status.");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("Row version is required for concurrency check.");
    }
}

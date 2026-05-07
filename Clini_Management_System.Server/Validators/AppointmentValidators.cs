using Clini_Management_System.Server.Models.DTOs;
using FluentValidation;

namespace Clini_Management_System.Server.Validators;

public class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
{
    public AppointmentCreateValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.AppointmentDate).NotEmpty();
    }
}

public class AppointmentStatusUpdateValidator : AbstractValidator<AppointmentStatusUpdateDto>
{
    public AppointmentStatusUpdateValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.RowVersion).NotEmpty();
    }
}

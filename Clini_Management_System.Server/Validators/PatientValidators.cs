using Clini_Management_System.Server.Models.DTOs;
using FluentValidation;

namespace Clini_Management_System.Server.Validators;

public sealed class PatientCreateValidator : AbstractValidator<PatientCreateDto>
{
    private const string MobilePattern = @"^\+?[0-9]{8,15}$";

    public PatientCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Patient name is required.")
            .MaximumLength(150);

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Matches(MobilePattern).WithMessage("Mobile number must contain 8-15 digits.");
    }
}

using Clini_Management_System.Server.Models.DTOs;
using FluentValidation;

namespace Clini_Management_System.Server.Validators;

public class PatientCreateValidator : AbstractValidator<PatientCreateDto>
{
    public PatientCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\+?[0-9]{8,15}$")
            .WithMessage("Mobile number must contain 8-15 digits.");
    }
}
